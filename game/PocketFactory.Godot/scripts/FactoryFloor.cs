using Godot;

namespace PocketFactory.Godot;

/// <summary>
/// A late-NES-inspired mine cutaway with two visual interpretations of the same production site.
/// </summary>
public partial class FactoryFloor : Control
{
    private FactoryVisualTheme visualTheme = FactoryVisualTheme.Industrial;

    private bool IsAdvanced => visualTheme == FactoryVisualTheme.Advanced;
    private Color Night => IsAdvanced ? Color.FromHtml("080a22") : Color.FromHtml("071426");
    private Color RockDeep => IsAdvanced ? Color.FromHtml("11183a") : Color.FromHtml("14243b");
    private Color Rock => IsAdvanced ? Color.FromHtml("26315d") : Color.FromHtml("2b425e");
    private Color RockLight => IsAdvanced ? Color.FromHtml("6375b6") : Color.FromHtml("6683a0");
    private Color Steel => IsAdvanced ? Color.FromHtml("bbcdf2") : Color.FromHtml("d5d9c5");
    private Color SteelDark => IsAdvanced ? Color.FromHtml("263560") : Color.FromHtml("284055");
    private Color Glass => IsAdvanced ? Color.FromHtml("3a79ac") : Color.FromHtml("3c6a7f");
    private Color Cyan => IsAdvanced ? Color.FromHtml("55e6e0") : Color.FromHtml("f2af3e");
    private Color Amber => IsAdvanced ? Color.FromHtml("bc72ff") : Color.FromHtml("e66d32");
    private Color White => IsAdvanced ? Color.FromHtml("f0f5ff") : Color.FromHtml("fff1c8");

    private double elapsed;
    private float manualPulse;
    private bool overclock;
    private int drillLevel;
    private int lineLevel;

    public override void _Ready()
    {
        MouseFilter = MouseFilterEnum.Ignore;
        QueueRedraw();
    }

    public override void _Process(double delta)
    {
        elapsed += delta;
        manualPulse = Mathf.Max(0f, manualPulse - (float)delta * 1.85f);
        QueueRedraw();
    }

    public void SetAnimationState(bool isOverclocked, int currentDrillLevel, int currentLineLevel)
    {
        overclock = isOverclocked;
        drillLevel = currentDrillLevel;
        lineLevel = currentLineLevel;
    }

    public void SetVisualTheme(FactoryVisualTheme theme)
    {
        visualTheme = theme;
        QueueRedraw();
    }

    public void TriggerManualPulse() => manualPulse = 1f;

    public override void _Draw()
    {
        var scale = Mathf.Min(Size.X / 360f, Size.Y / 540f);
        var origin = new Vector2((Size.X - 360f * scale) * 0.5f, (Size.Y - 540f * scale) * 0.5f);
        DrawRect(new Rect2(Vector2.Zero, Size), Night, true);
        DrawMineCutaway(origin, scale);
    }

    private void DrawMineCutaway(Vector2 origin, float scale)
    {
        Vector2 P(float x, float y) => origin + new Vector2(x * scale, y * scale);
        Rect2 R(float x, float y, float width, float height) => new(P(x, y), new Vector2(width * scale, height * scale));
        void Box(float x, float y, float width, float height, Color color, Color? border = null, float borderWidth = 1f)
        {
            DrawRect(R(x, y, width, height), color, true);
            if (border is Color outline)
            {
                DrawRect(R(x, y, width, height), outline, false, borderWidth * scale);
            }
        }

        var accent = overclock ? Amber : Cyan;
        var motion = overclock ? 1.85f : 1f;
        Box(0f, 0f, 360f, 540f, Night);
        DrawSurface(P, Box, scale, accent);
        DrawRockCutaway(P, Box, scale);
        DrawShaft(P, Box, scale, accent, motion);
        DrawProductionWing(P, Box, scale, accent, motion);
        DrawThemeAccents(P, Box, scale, accent);

        if (manualPulse > 0f)
        {
            var radius = (16f + (1f - manualPulse) * 38f) * scale;
            DrawArc(P(82f, 442f), radius, 0f, Mathf.Tau, 32, new Color(Amber, manualPulse * 0.72f), 1.8f * scale);
        }
    }

    private void DrawSurface(
        System.Func<float, float, Vector2> point,
        System.Action<float, float, float, float, Color, Color?, float> box,
        float scale,
        Color accent)
    {
        box(0f, 0f, 360f, 64f, Color.FromHtml("2b3028"), null, 1f);
        box(0f, 55f, 360f, 9f, Night, null, 1f);
        DrawHazardStripeBand(point, 0f, 55f, 360f, 9f, scale);
        box(15f, 16f, 82f, 31f, Color.FromHtml("393529"), SteelDark, 1f);
        box(24f, 23f, 44f, 7f, new Color(accent, 0.75f), null, 1f);
        box(24f, 35f, 60f, 4f, Steel, null, 1f);

        // Shipping platform: the site's visible connection to the next factory.
        box(252f, 18f, 77f, 31f, Color.FromHtml("3d392b"), SteelDark, 1f);
        box(263f, 25f, 36f, 16f, Color.FromHtml("655c45"), Glass, 1f);
        box(305f, 20f, 7f, 25f, Steel, null, 1f);
        DrawCircle(point(308f, 18f), 3f * scale, accent);
        DrawLine(point(8f, 55f), point(352f, 55f), new Color(accent, 0.75f), 1.4f * scale);
    }

    private void DrawRockCutaway(
        System.Func<float, float, Vector2> point,
        System.Action<float, float, float, float, Color, Color?, float> box,
        float scale)
    {
        box(0f, 64f, 202f, 476f, RockDeep, null, 1f);
        box(10f, 72f, 180f, 460f, Rock, RockLight, 1f);

        for (var y = 88; y < 520; y += 38)
        {
            for (var x = 18 + ((y / 38) % 2) * 11; x < 188; x += 36)
            {
                DrawCircle(point(x, y), 7f * scale, new Color(RockLight, 0.22f));
                DrawLine(point(x - 6f, y + 4f), point(x + 5f, y - 3f), new Color(RockDeep, 0.75f), scale);
            }
        }

        // Cutaway cavities: familiar mine levels that stay clean at phone size.
        box(29f, 126f, 139f, 72f, Night, SteelDark, 1f);
        box(29f, 247f, 139f, 72f, Night, SteelDark, 1f);
        box(29f, 368f, 139f, 104f, Night, SteelDark, 1f);
        foreach (var floorY in new[] { 194f, 315f, 468f })
        {
            box(25f, floorY, 148f, 8f, Night, null, 1f);
            DrawHazardStripeBand(point, 31f, floorY + 2f, 76f, 4f, scale);
            box(29f, floorY, 140f, 1f, Steel, null, 1f);
        }
    }

    private void DrawShaft(
        System.Func<float, float, Vector2> point,
        System.Action<float, float, float, float, Color, Color?, float> box,
        float scale,
        Color accent,
        float motion)
    {
        // Vertical logistics make the site read as an idle miner before the factory details resolve.
        box(178f, 78f, 20f, 434f, Color.FromHtml("211f17"), SteelDark, 1f);
        DrawLine(point(183f, 82f), point(183f, 505f), Steel, 1.25f * scale);
        DrawLine(point(193f, 82f), point(193f, 505f), Steel, 1.25f * scale);
        for (var y = 94; y < 500; y += 27)
        {
            DrawLine(point(182f, y), point(194f, y), SteelDark, 1.25f * scale);
        }

        var elevatorY = 105f + Mathf.PosMod((float)elapsed * 29f * motion, 340f);
        box(177f, elevatorY, 23f, 18f, Color.FromHtml("625944"), Steel, 1f);
        box(181f, elevatorY + 4f, 15f, 6f, Glass, null, 1f);
        DrawCircle(point(190f, elevatorY + 13f), 2.5f * scale, accent);

        DrawDrillLevel(point, box, scale, accent, motion);
        DrawRefineryLevel(point, box, scale, accent, motion);
        DrawPickaxeLevel(point, box, scale, accent, motion);
    }

    private void DrawDrillLevel(
        System.Func<float, float, Vector2> point,
        System.Action<float, float, float, float, Color, Color?, float> box,
        float scale,
        Color accent,
        float motion)
    {
        box(45f, 145f, 43f, 39f, Color.FromHtml("625640"), SteelDark, 1f);
        DrawCircle(point(68f, 165f), 27f * scale, new Color(accent, 0.07f));
        DrawCircle(point(68f, 165f), 11f * scale, Steel);
        DrawCircle(point(68f, 165f), 4f * scale, accent);
        var bore = 91f + Mathf.Sin((float)elapsed * 2f * motion) * 5f;
        DrawLine(point(78f, 165f), point(bore, 165f), Steel, 4f * scale);
        DrawLine(point(bore, 159f), point(bore + 10f, 165f), Amber, 2f * scale);
        DrawLine(point(bore, 171f), point(bore + 10f, 165f), Amber, 2f * scale);
        DrawCart(point, box, 112f, 178f, scale, accent, (float)elapsed * motion);
    }

    private void DrawRefineryLevel(
        System.Func<float, float, Vector2> point,
        System.Action<float, float, float, float, Color, Color?, float> box,
        float scale,
        Color accent,
        float motion)
    {
        box(44f, 263f, 49f, 38f, Color.FromHtml("655943"), SteelDark, 1f);
        box(55f, 271f, 27f, 22f, Color.FromHtml("867755"), Glass, 1f);
        var corePulse = 0.55f + Mathf.Sin((float)elapsed * 2.7f * motion) * 0.2f;
        DrawCircle(point(68f, 282f), 20f * scale, new Color(accent, 0.1f));
        DrawCircle(point(68f, 282f), 7f * scale, new Color(accent, corePulse));
        DrawCircle(point(68f, 282f), 2.5f * scale, White);

        box(105f, 268f, 47f, 30f, Color.FromHtml("4b4433"), SteelDark, 1f);
        DrawLine(point(101f, 287f), point(153f, 287f), Steel, 3f * scale);
        DrawFlowDot(point, 104f, 287f, 48f, scale, accent, (float)elapsed * 0.65f * motion);
    }

    private void DrawPickaxeLevel(
        System.Func<float, float, Vector2> point,
        System.Action<float, float, float, float, Color, Color?, float> box,
        float scale,
        Color accent,
        float motion)
    {
        box(40f, 386f, 48f, 57f, Color.FromHtml("4f4633"), SteelDark, 1f);
        DrawCircle(point(93f, 429f), 29f * scale, new Color(Amber, 0.08f + manualPulse * 0.09f));
        for (var rock = 0; rock < 4; rock++)
        {
            DrawCircle(point(105f + rock * 11f, 431f - (rock % 2) * 8f), 7f * scale, RockLight);
        }

        var strike = -0.78f + manualPulse * 0.8f + Mathf.Sin((float)elapsed * 1.1f * motion) * 0.04f;
        var hand = point(68f, 415f);
        var tip = hand + new Vector2(Mathf.Cos(strike) * 35f, Mathf.Sin(strike) * 35f) * scale;
        DrawCircle(hand, 6f * scale, Steel);
        DrawLine(hand, tip, Amber, 4f * scale);
        var head = tip + new Vector2(-Mathf.Sin(strike), Mathf.Cos(strike)) * 11f * scale;
        DrawLine(tip - (head - tip), head, Steel, 5f * scale);
        DrawCircle(tip, 2.5f * scale, White);
        DrawMiner(point, 58f, 424f, scale, accent);

        DrawCart(point, box, 130f, 451f, scale, accent, (float)elapsed * motion + 0.4f);
    }

    private void DrawProductionWing(
        System.Func<float, float, Vector2> point,
        System.Action<float, float, float, float, Color, Color?, float> box,
        float scale,
        Color accent,
        float motion)
    {
        box(207f, 78f, 137f, 434f, Color.FromHtml("211f17"), SteelDark, 1f);
        box(213f, 92f, 124f, 88f, Color.FromHtml("373224"), SteelDark, 1f);
        box(213f, 200f, 124f, 104f, Color.FromHtml("332f22"), SteelDark, 1f);
        box(213f, 325f, 124f, 150f, Color.FromHtml("2c2920"), SteelDark, 1f);

        // Intake conveyor from the shaft across a compact, layered factory wing.
        DrawConveyor(point, box, 201f, 184f, 129f, scale, accent, motion);
        DrawMovingOre(point, 214f, 178f, 104f, scale, accent, motion, 4);

        DrawModule(point, box, 232f, 108f, 36f, 52f, accent, 0.2f);
        DrawModule(point, box, 281f, 108f, 36f, 52f, Amber, 0.8f);
        DrawReactor(point, box, 234f, 218f, scale, accent, motion);
        DrawRobotArm(point, 294f, 273f, scale, accent, motion);

        DrawConveyor(point, box, 221f, 429f, 101f, scale, accent, motion);
        DrawMovingOre(point, 230f, 423f, 79f, scale, accent, motion, 3);
        DrawShippingDoor(point, box, 283f, 350f, scale, accent);

        for (var index = 0; index < Mathf.Clamp(lineLevel + 1, 1, 5); index++)
        {
            DrawCircle(point(226f + index * 13f, 316f), 3f * scale, index < drillLevel - 4 ? accent : SteelDark);
        }
    }

    private void DrawThemeAccents(
        System.Func<float, float, Vector2> point,
        System.Action<float, float, float, float, Color, Color?, float> box,
        float scale,
        Color accent)
    {
        if (IsAdvanced)
        {
            box(217f, 86f, 7f, 80f, Color.FromHtml("18244a"), SteelDark, 1f);
            box(326f, 86f, 7f, 80f, Color.FromHtml("18244a"), SteelDark, 1f);
            for (var y = 97f; y < 160f; y += 16f)
            {
                DrawCircle(point(220f, y), 2.5f * scale, Cyan);
                DrawCircle(point(329f, y), 2.5f * scale, Amber);
            }

            DrawArc(point(276f, 274f), 37f * scale, 3.5f, 5.95f, 20, new Color(Cyan, 0.65f), 1.4f * scale);
            DrawArc(point(276f, 274f), 45f * scale, 3.5f, 5.95f, 20, new Color(Amber, 0.45f), 1.1f * scale);
            DrawCircle(point(315f, 235f), 5f * scale, Cyan);
            DrawLine(point(315f, 235f), point(323f, 227f), Steel, 2f * scale);
            return;
        }

        for (var x = 217f; x < 339f; x += 29f)
        {
            box(x, 84f, 5f, 93f, Color.FromHtml("74482c"), SteelDark, 1f);
            DrawCircle(point(x + 2.5f, 98f), 2.2f * scale, Amber);
        }

        box(222f, 338f, 42f, 33f, Color.FromHtml("8b552d"), Color.FromHtml("d19a48"), 1f);
        DrawLine(point(225f, 341f), point(261f, 367f), SteelDark, 1.25f * scale);
        DrawLine(point(261f, 341f), point(225f, 367f), SteelDark, 1.25f * scale);
    }

    private void DrawModule(
        System.Func<float, float, Vector2> point,
        System.Action<float, float, float, float, Color, Color?, float> box,
        float x,
        float y,
        float width,
        float height,
        Color accent,
        float phase)
    {
        box(x, y, width, height, Color.FromHtml("69604b"), SteelDark, 1f);
        box(x + 6f, y + 8f, width - 12f, 8f, new Color(accent, 0.72f), null, 1f);
        box(x + 8f, y + 25f, width - 16f, height - 33f, Color.FromHtml("3e392a"), Glass, 1f);
        DrawCircle(point(x + width - 8f, y + 8f), 2.5f, new Color(White, 0.55f + Mathf.Sin((float)elapsed * 2.4f + phase) * 0.3f));
    }

    private void DrawReactor(
        System.Func<float, float, Vector2> point,
        System.Action<float, float, float, float, Color, Color?, float> box,
        float x,
        float y,
        float scale,
        Color accent,
        float motion)
    {
        box(x, y, 49f, 68f, Color.FromHtml("6a6048"), SteelDark, 1f);
        DrawCircle(point(x + 24f, y + 35f), 33f * scale, new Color(accent, 0.08f));
        DrawCircle(point(x + 24f, y + 35f), 15f * scale, Color.FromHtml("8a7d5c"));
        DrawCircle(point(x + 24f, y + 35f), (6f + Mathf.Sin((float)elapsed * 2.5f * motion) * 1.4f) * scale, accent);
        DrawCircle(point(x + 24f, y + 35f), 2.5f * scale, White);
    }

    private void DrawRobotArm(System.Func<float, float, Vector2> point, float x, float y, float scale, Color accent, float motion)
    {
        var basePoint = point(x, y);
        var joint = basePoint + new Vector2(10f * scale, -19f * scale);
        var angle = -0.95f + Mathf.Sin((float)elapsed * 1.6f * motion) * 0.33f;
        var tip = joint + new Vector2(Mathf.Cos(angle) * 25f, Mathf.Sin(angle) * 25f) * scale;
        DrawCircle(basePoint, 8f * scale, SteelDark);
        DrawLine(basePoint, joint, Steel, 5f * scale);
        DrawCircle(joint, 4f * scale, Steel);
        DrawLine(joint, tip, Steel, 3.5f * scale);
        DrawCircle(tip, 3f * scale, accent);
    }

    private void DrawShippingDoor(
        System.Func<float, float, Vector2> point,
        System.Action<float, float, float, float, Color, Color?, float> box,
        float x,
        float y,
        float scale,
        Color accent)
    {
        box(x, y, 38f, 69f, Color.FromHtml("252318"), Steel, 1f);
        box(x + 7f, y + 11f, 24f, 46f, Color.FromHtml("4b4632"), Glass, 1f);
        DrawHazardStripeBand(point, x + 7f, y + 49f, 24f, 7f, scale);
        box(x + 10f, y + 15f, 18f, 4f, accent, null, 1f);
        DrawCircle(point(x + 31f, y + 7f), 3f * scale, Amber);
    }

    private void DrawConveyor(
        System.Func<float, float, Vector2> point,
        System.Action<float, float, float, float, Color, Color?, float> box,
        float x,
        float y,
        float width,
        float scale,
        Color accent,
        float motion)
    {
        box(x, y, width, 15f, Night, SteelDark, 1f);
        for (var segment = 0; segment < width / 16f; segment++)
        {
            var offset = Mathf.PosMod(segment * 17f + (float)elapsed * 25f * motion, width);
            DrawLine(point(x + offset, y + 4f), point(x + offset - 8f, y + 11f), Steel, 1.35f * scale);
        }
        DrawLine(point(x, y + 13f), point(x + width, y + 13f), accent, 1f * scale);
    }

    private void DrawMovingOre(System.Func<float, float, Vector2> point, float x, float y, float length, float scale, Color accent, float motion, int count)
    {
        for (var index = 0; index < count; index++)
        {
            var oreX = x + Mathf.PosMod(index * (length / count) + (float)elapsed * 34f * motion, length);
            DrawCircle(point(oreX, y), 4.2f * scale, Steel);
            DrawCircle(point(oreX - 1f, y - 1f), 1.4f * scale, accent);
        }
    }

    private void DrawCart(
        System.Func<float, float, Vector2> point,
        System.Action<float, float, float, float, Color, Color?, float> box,
        float x,
        float y,
        float scale,
        Color accent,
        float phase)
    {
        var nudge = Mathf.Sin(phase * 1.9f) * 4f;
        box(x + nudge, y - 10f, 26f, 10f, Color.FromHtml("6d634c"), Steel, 1f);
        DrawCircle(point(x + 6f + nudge, y + 2f), 3f * scale, SteelDark);
        DrawCircle(point(x + 20f + nudge, y + 2f), 3f * scale, SteelDark);
        DrawCircle(point(x + 13f + nudge, y - 12f), 3f * scale, accent);
    }

    private void DrawFlowDot(System.Func<float, float, Vector2> point, float x, float y, float length, float scale, Color accent, float phase)
    {
        var position = x + Mathf.PosMod(phase * 42f, length);
        DrawCircle(point(position, y), 3f * scale, accent);
    }

    private void DrawHazardStripeBand(System.Func<float, float, Vector2> point, float x, float y, float width, float height, float scale)
    {
        DrawRect(new Rect2(point(x, y), new Vector2(width * scale, height * scale)), Night, true);
        for (var offset = -height; offset < width + height; offset += height * 2f)
        {
            DrawLine(point(x + offset, y + height), point(x + offset + height, y), Cyan, Mathf.Max(1f, height * 0.65f * scale));
        }
    }

    private void DrawMiner(System.Func<float, float, Vector2> point, float x, float y, float scale, Color accent)
    {
        DrawCircle(point(x, y - 11f), 5.5f * scale, Amber);
        DrawRect(new Rect2(point(x - 6f, y - 6f), new Vector2(12f * scale, 14f * scale)), SteelDark, true);
        DrawRect(new Rect2(point(x - 8f, y + 8f), new Vector2(6f * scale, 7f * scale)), RockDeep, true);
        DrawRect(new Rect2(point(x + 2f, y + 8f), new Vector2(6f * scale, 7f * scale)), RockDeep, true);
        DrawCircle(point(x + 2f, y - 11f), 1.3f * scale, White);
        DrawLine(point(x + 6f, y - 1f), point(x + 12f, y + 3f), accent, 2f * scale);
    }
}

public enum FactoryVisualTheme
{
    Industrial,
    Advanced,
}
