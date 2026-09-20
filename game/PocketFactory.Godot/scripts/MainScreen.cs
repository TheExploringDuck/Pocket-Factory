using System;
using Godot;
using PocketFactory.Core.Simulation;

namespace PocketFactory.Godot;

public partial class MainScreen : Control
{
    private static readonly TimeSpan MaximumOfflineProduction = TimeSpan.FromHours(8);

    private GameState state = null!;
    private readonly MiningSimulator miningSimulator = new(new FactoryChanceSource());

    private Label metalLabel = null!;
    private Label currencyLabel = null!;
    private Label rateLabel = null!;
    private Label siteLabel = null!;
    private Label pickaxeDetail = null!;
    private Label drillDetail = null!;
    private Label lineDetail = null!;
    private Label activityLabel = null!;
    private Button overclockButton = null!;
    private Button industrialViewButton = null!;
    private Button advancedViewButton = null!;
    private FactoryFloor factoryFloor = null!;
    private double drillAccumulator;
    private double overclockRemaining;
    private double saveAccumulator;
    private bool isAdvancedVisualTheme;
    private string? startupActivity;

    public override void _Ready()
    {
        LoadOrStartRun();
        BuildInterface();
        SetVisualTheme(isAdvancedVisualTheme ? FactoryVisualTheme.Advanced : FactoryVisualTheme.Industrial, false);
        if (!string.IsNullOrEmpty(startupActivity))
        {
            activityLabel.Text = startupActivity;
        }

        Refresh();
        CaptureRequestedFrame();
    }

    public override void _Process(double delta)
    {
        UpdateAutomaticMining(delta);
        overclockRemaining = Math.Max(0d, overclockRemaining - delta);
        factoryFloor.SetAnimationState(overclockRemaining > 0d, state.DrillLevel, Line1Level);
        saveAccumulator += delta;
        if (saveAccumulator >= 20d)
        {
            SaveGame();
        }

        RefreshHudOnly();
    }

    public override void _ExitTree() => SaveGame();

    private void LoadOrStartRun()
    {
        var save = PocketFactorySaveStore.TryLoad();
        if (save is null)
        {
            state = GameState.NewRun();
            InitializeVerticalSlice();
            return;
        }

        try
        {
            state = GameState.FromSnapshot(save.GameState);
            isAdvancedVisualTheme = save.IsAdvancedVisualTheme;

            var savedAt = DateTimeOffset.FromUnixTimeMilliseconds(save.SavedAtUnixMilliseconds);
            var elapsed = DateTimeOffset.UtcNow - savedAt;
            if (elapsed < TimeSpan.Zero)
            {
                startupActivity = "CLOCK CHECK  //  OFFLINE PRODUCTION PAUSED";
                return;
            }

            var offlineResult = OfflineProgression.ApplyDrillProduction(state, elapsed, MaximumOfflineProduction);
            if (offlineResult.CompletedCycles > 0)
            {
                var capNote = offlineResult.WasCapped ? "  //  SHIFT CAP REACHED" : string.Empty;
                startupActivity = $"OFFLINE SHIFT +{offlineResult.MetalGained:0} METAL{capNote}";
            }
        }
        catch (ArgumentException exception)
        {
            GD.PushWarning($"Pocket Factory save was ignored because it is invalid: {exception.Message}");
            state = GameState.NewRun();
            InitializeVerticalSlice();
            startupActivity = "SAVE CHECK  //  STARTED A FRESH TEST RUN";
        }
    }

    private void InitializeVerticalSlice()
    {
        // Upgrade prices are intentionally TBD. The screen starts at an approved active-factory state.
        state.SetPickaxeLevel(EconomyRules.MaxPickaxeLevel);
        state.SetDrillLevel(8);
        state.SetProductionLineLevel(1, 1);
        state.AddMetal(480m);
        state.AddCurrency(18m);
    }

    private void CaptureRequestedFrame()
    {
        var arguments = OS.GetCmdlineUserArgs();
        if (Array.IndexOf(arguments, "--capture-advanced") >= 0)
        {
            SetVisualTheme(FactoryVisualTheme.Advanced);
        }

        if (Array.IndexOf(arguments, "--capture-overclock") >= 0)
        {
            overclockRemaining = EconomyRules.RewardedAdProductionMultiplierDuration.TotalSeconds;
            activityLabel.Text = "TEST OVERCLOCK ACTIVE  //  2X PRODUCTION FOR 05:00";
        }

        foreach (var argument in arguments)
        {
            if (!argument.StartsWith("--capture-screen=", StringComparison.Ordinal))
            {
                continue;
            }

            var destination = argument["--capture-screen=".Length..];
            if (DisplayServer.GetName().ToString() == "headless")
            {
                GD.PushWarning("Screen capture requires a rendering display server.");
                GetTree().Quit();
                return;
            }

            GetTree().CreateTimer(0.8d).Timeout += () =>
            {
                GetViewport().GetTexture().GetImage().SavePng(destination);
                GetTree().Quit();
            };
            return;
        }
    }

    private void BuildInterface()
    {
        AddChild(new ColorRect { Color = Palette.Backdrop, MouseFilter = MouseFilterEnum.Ignore, AnchorRight = 1f, AnchorBottom = 1f });

        var page = new MarginContainer { AnchorRight = 1f, AnchorBottom = 1f };
        page.AddThemeConstantOverride("margin_left", 12);
        page.AddThemeConstantOverride("margin_top", 12);
        page.AddThemeConstantOverride("margin_right", 12);
        page.AddThemeConstantOverride("margin_bottom", 10);
        AddChild(page);

        var root = new VBoxContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
        root.AddThemeConstantOverride("separation", 8);
        page.AddChild(root);
        root.AddChild(BuildHeader());
        root.AddChild(BuildResourceBar());
        root.AddChild(BuildViewSelector());

        factoryFloor = new FactoryFloor { CustomMinimumSize = new Vector2(0f, 300f), SizeFlagsVertical = SizeFlags.ExpandFill };
        root.AddChild(factoryFloor);

        activityLabel = new Label { HorizontalAlignment = HorizontalAlignment.Center, Text = "INDUSTRIAL SHIFT  //  PRODUCTION FLOW STABLE" };
        activityLabel.AddThemeColorOverride("font_color", Palette.Muted);
        activityLabel.AddThemeFontSizeOverride("font_size", 12);
        root.AddChild(Wrap(activityLabel, Palette.PanelDeep, 6));

        var machines = new HBoxContainer();
        machines.AddThemeConstantOverride("separation", 6);
        machines.AddChild(BuildMachineCard("PICKAXE", "MANUAL", out pickaxeDetail, TunePickaxe));
        machines.AddChild(BuildMachineCard("REFINERY", "ACTIVE", out drillDetail, TuneDrill));
        machines.AddChild(BuildMachineCard("LINE 01", "ONLINE", out lineDetail, TuneProductionLine));
        root.AddChild(machines);

        var actions = new HBoxContainer();
        actions.AddThemeConstantOverride("separation", 8);
        actions.AddChild(BuildActionButton("PICKAXE STRIKE", MineOre, Palette.Amber, Palette.AmberDark));
        overclockButton = BuildActionButton("BOOST OUTPUT", ActivateOverclock, Palette.Teal, Palette.TealDark);
        actions.AddChild(overclockButton);
        root.AddChild(actions);

        var footer = new Label { Text = "THEEXPLORINGDUCK  //  LATE-NES FACTORY PROTOTYPE", HorizontalAlignment = HorizontalAlignment.Center };
        footer.AddThemeColorOverride("font_color", Palette.Subtle);
        footer.AddThemeFontSizeOverride("font_size", 10);
        root.AddChild(footer);
    }

    private Control BuildHeader()
    {
        var row = new HBoxContainer();
        row.AddThemeConstantOverride("separation", 8);
        var titleBlock = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
        var title = new Label { Text = "POCKET FACTORY" };
        title.AddThemeColorOverride("font_color", Palette.Cream);
        title.AddThemeFontSizeOverride("font_size", 23);
        titleBlock.AddChild(title);
        siteLabel = new Label { Text = "INDUSTRIAL  /  IRON WORKS" };
        siteLabel.AddThemeColorOverride("font_color", Palette.Muted);
        siteLabel.AddThemeFontSizeOverride("font_size", 10);
        titleBlock.AddChild(siteLabel);
        row.AddChild(titleBlock);

        rateLabel = new Label { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Center };
        rateLabel.AddThemeColorOverride("font_color", Palette.TealLight);
        rateLabel.AddThemeFontSizeOverride("font_size", 13);
        row.AddChild(rateLabel);
        return Wrap(row, Palette.Panel, 8);
    }

    private Control BuildResourceBar()
    {
        var row = new HBoxContainer();
        row.AddThemeConstantOverride("separation", 7);
        metalLabel = MakeResourceLabel();
        currencyLabel = MakeResourceLabel();
        row.AddChild(Wrap(metalLabel, Palette.Panel, 6, true));
        row.AddChild(Wrap(currencyLabel, Palette.Panel, 6, true));
        return row;
    }

    private Control BuildViewSelector()
    {
        var row = new HBoxContainer();
        row.AddThemeConstantOverride("separation", 5);
        industrialViewButton = BuildViewButton("INDUSTRIAL", FactoryVisualTheme.Industrial);
        advancedViewButton = BuildViewButton("ADVANCED", FactoryVisualTheme.Advanced);
        row.AddChild(industrialViewButton);
        row.AddChild(advancedViewButton);
        return Wrap(row, Palette.PanelDeep, 4);
    }

    private Button BuildViewButton(string text, FactoryVisualTheme theme)
    {
        var button = new Button { Text = text, SizeFlagsHorizontal = SizeFlags.ExpandFill, CustomMinimumSize = new Vector2(0f, 28f) };
        button.AddThemeFontSizeOverride("font_size", 10);
        button.Pressed += () => SetVisualTheme(theme);
        return button;
    }

    private static Label MakeResourceLabel()
    {
        var label = new Label { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center };
        label.AddThemeColorOverride("font_color", Palette.Cream);
        label.AddThemeFontSizeOverride("font_size", 15);
        return label;
    }

    private Control BuildMachineCard(string title, string status, out Label detail, Action action)
    {
        var box = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
        box.AddThemeConstantOverride("separation", 2);
        box.AddChild(CenteredLabel(title, Palette.Cream, 12));
        box.AddChild(CenteredLabel(status, Palette.TealLight, 9));
        detail = CenteredLabel(string.Empty, Palette.Muted, 10);
        box.AddChild(detail);
        var button = new Button { Text = "TUNE", CustomMinimumSize = new Vector2(0f, 29f) };
        button.Pressed += action;
        StyleButton(button, Palette.PanelLight, Palette.PanelDeep, Palette.Cream);
        box.AddChild(button);
        return Wrap(box, Palette.Panel, 6, true);
    }

    private static Label CenteredLabel(string text, Color color, int size)
    {
        var label = new Label { Text = text, HorizontalAlignment = HorizontalAlignment.Center };
        label.AddThemeColorOverride("font_color", color);
        label.AddThemeFontSizeOverride("font_size", size);
        return label;
    }

    private Button BuildActionButton(string text, Action action, Color normal, Color pressed)
    {
        var button = new Button { Text = text, SizeFlagsHorizontal = SizeFlags.ExpandFill, CustomMinimumSize = new Vector2(0f, 42f) };
        button.AddThemeFontSizeOverride("font_size", 13);
        button.Pressed += action;
        StyleButton(button, normal, pressed, Palette.Ink);
        return button;
    }

    private void MineOre()
    {
        var result = miningSimulator.MineWithPickaxe(state);
        activityLabel.Text = $"PICKAXE STRIKE +{result.MetalGained:0.0} METAL  //  INTAKE BAY ONLINE";
        factoryFloor.TriggerManualPulse();
        SaveGame();
        Refresh();
    }

    private void ActivateOverclock()
    {
        overclockRemaining = EconomyRules.RewardedAdProductionMultiplierDuration.TotalSeconds;
        activityLabel.Text = "OUTPUT BOOST ACTIVE  //  TEMPORARY PRODUCTION LIFT";
        SaveGame();
        Refresh();
    }

    private void SetVisualTheme(FactoryVisualTheme theme, bool announce = true)
    {
        factoryFloor.SetVisualTheme(theme);
        var isIndustrial = theme == FactoryVisualTheme.Industrial;
        isAdvancedVisualTheme = !isIndustrial;
        siteLabel.Text = isIndustrial ? "INDUSTRIAL  /  IRON WORKS" : "ADVANCED  /  SIGNAL FOUNDRY";
        if (announce)
        {
            activityLabel.Text = isIndustrial
                ? "INDUSTRIAL SHIFT  //  PRODUCTION FLOW STABLE"
                : "ADVANCED SHIFT  //  SYNTHESIS FLOW STABLE";
        }

        StyleButton(industrialViewButton, isIndustrial ? Palette.Amber : Palette.PanelLight, Palette.AmberDark, isIndustrial ? Palette.Ink : Palette.Cream);
        StyleButton(advancedViewButton, isIndustrial ? Palette.PanelLight : Palette.Teal, Palette.TealDark, isIndustrial ? Palette.Cream : Palette.Ink);
        SaveGame();
    }

    private void TunePickaxe() => activityLabel.Text = "EXTRACTION BAY IS ALREADY DIALED IN AT LEVEL 10.";

    private void TuneDrill()
    {
        if (state.DrillLevel >= EconomyRules.MaxDrillLevel)
        {
            activityLabel.Text = "REFINERY IS AT ITS CURRENT PROTOTYPE CAP.";
            return;
        }

        state.SetDrillLevel(state.DrillLevel + 1);
        activityLabel.Text = $"REFINERY TUNED TO LEVEL {state.DrillLevel}. COSTS ARE PENDING BALANCE.";
        SaveGame();
        Refresh();
    }

    private void TuneProductionLine()
    {
        var currentLevel = Line1Level;
        if (currentLevel >= 5)
        {
            activityLabel.Text = "LINE 01 IS AT ITS CURRENT PROTOTYPE CAP.";
            return;
        }

        var nextRule = EconomyRules.GetProductionLineLevelRule(currentLevel + 1);
        if (state.DrillLevel < nextRule.SupportingDrillLevel)
        {
            activityLabel.Text = $"LINE {currentLevel + 1} NEEDS DRILL LEVEL {nextRule.SupportingDrillLevel}.";
            return;
        }

        state.SetProductionLineLevel(1, currentLevel + 1);
        activityLabel.Text = $"LINE 01 TUNED TO LEVEL {currentLevel + 1}. COSTS ARE PENDING BALANCE.";
        SaveGame();
        Refresh();
    }

    private void UpdateAutomaticMining(double delta)
    {
        var rule = EconomyRules.GetDrillRule(state.DrillLevel);
        var multiplier = overclockRemaining > 0d ? EconomyRules.RewardedAdProductionMultiplier : 1d;
        drillAccumulator += delta * multiplier;

        while (drillAccumulator >= rule.CycleTimeSeconds)
        {
            drillAccumulator -= rule.CycleTimeSeconds;
            miningSimulator.MineWithDrill(state);
        }
    }

    private void Refresh()
    {
        RefreshHudOnly();
        pickaxeDetail.Text = "LV. 10 / 10";
        drillDetail.Text = $"LV. {state.DrillLevel} / 10";
        lineDetail.Text = $"LV. {Line1Level} / 5";
    }

    private void RefreshHudOnly()
    {
        if (metalLabel is null)
        {
            return;
        }

        var rule = EconomyRules.GetDrillRule(state.DrillLevel);
        var multiplier = overclockRemaining > 0d ? EconomyRules.RewardedAdProductionMultiplier : 1d;
        metalLabel.Text = $"METAL  {state.Metal:0}";
        currencyLabel.Text = $"CURRENCY  {state.Currency:0}";
        rateLabel.Text = $"+{rule.MetalPerSecond * multiplier:0.0}/S";
        if (overclockButton is not null)
        {
            overclockButton.Text = overclockRemaining > 0d ? $"BOOST {TimeSpan.FromSeconds(overclockRemaining):mm\\:ss}" : "BOOST OUTPUT";
        }
    }

    private int Line1Level => state.ProductionLineLevels.TryGetValue(1, out var level) ? level : 0;

    private void SaveGame()
    {
        if (state is null)
        {
            return;
        }

        PocketFactorySaveStore.Save(state, isAdvancedVisualTheme);
        saveAccumulator = 0d;
    }

    private static Control Wrap(Control child, Color background, int padding, bool expand = false)
    {
        var panel = new PanelContainer { SizeFlagsHorizontal = expand ? SizeFlags.ExpandFill : SizeFlags.ShrinkBegin };
        panel.AddThemeStyleboxOverride("panel", CreatePanelStyle(background, Palette.Border, 6));
        panel.AddThemeConstantOverride("margin_left", padding);
        panel.AddThemeConstantOverride("margin_top", padding);
        panel.AddThemeConstantOverride("margin_right", padding);
        panel.AddThemeConstantOverride("margin_bottom", padding);
        panel.AddChild(child);
        return panel;
    }

    private static void StyleButton(Button button, Color normal, Color pressed, Color text)
    {
        button.AddThemeStyleboxOverride("normal", CreatePanelStyle(normal, Palette.Border, 5));
        button.AddThemeStyleboxOverride("hover", CreatePanelStyle(normal.Lightened(0.08f), Palette.Cream, 5));
        button.AddThemeStyleboxOverride("pressed", CreatePanelStyle(pressed, Palette.Ink, 5));
        button.AddThemeStyleboxOverride("disabled", CreatePanelStyle(Palette.PanelDeep, Palette.Border, 5));
        button.AddThemeColorOverride("font_color", text);
        button.AddThemeColorOverride("font_hover_color", text);
    }

    private static StyleBoxFlat CreatePanelStyle(Color background, Color border, int radius) => new()
    {
        BgColor = background,
        BorderColor = border,
        BorderWidthLeft = 1,
        BorderWidthTop = 1,
        BorderWidthRight = 1,
        BorderWidthBottom = 1,
        CornerRadiusTopLeft = radius,
        CornerRadiusTopRight = radius,
        CornerRadiusBottomRight = radius,
        CornerRadiusBottomLeft = radius,
    };

    private sealed class FactoryChanceSource : IChanceSource
    {
        private readonly Random random = new(1888);

        public bool Roll(double probability) => random.NextDouble() < probability;
    }
}

internal static class Palette
{
    public static readonly Color Backdrop = Color.FromHtml("071325");
    public static readonly Color PanelDeep = Color.FromHtml("0c1a2e");
    public static readonly Color Panel = Color.FromHtml("122844");
    public static readonly Color PanelLight = Color.FromHtml("1f3f5d");
    public static readonly Color Border = Color.FromHtml("557594");
    public static readonly Color Cream = Color.FromHtml("fff1c9");
    public static readonly Color Muted = Color.FromHtml("afc2d0");
    public static readonly Color Subtle = Color.FromHtml("6f8499");
    public static readonly Color Ink = Color.FromHtml("07101e");
    public static readonly Color Amber = Color.FromHtml("f3a13d");
    public static readonly Color AmberDark = Color.FromHtml("ba592d");
    public static readonly Color Teal = Color.FromHtml("4fd4cf");
    public static readonly Color TealDark = Color.FromHtml("267e96");
    public static readonly Color TealLight = Color.FromHtml("9beee1");
}
