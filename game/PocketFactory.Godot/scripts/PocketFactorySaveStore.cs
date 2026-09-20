using System;
using System.Text.Json;
using Godot;
using PocketFactory.Core.Simulation;

namespace PocketFactory.Godot;

public static class PocketFactorySaveStore
{
    private const string SavePath = "user://pocket_factory.save.json";
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public static PocketFactorySaveFile? TryLoad()
    {
        if (!global::Godot.FileAccess.FileExists(SavePath))
        {
            return null;
        }

        using var file = global::Godot.FileAccess.Open(SavePath, global::Godot.FileAccess.ModeFlags.Read);
        if (file is null)
        {
            GD.PushWarning("Pocket Factory save could not be opened.");
            return null;
        }

        try
        {
            var save = JsonSerializer.Deserialize<PocketFactorySaveFile>(file.GetAsText(), SerializerOptions);
            return save is { Version: PocketFactorySaveFile.CurrentVersion } ? save : null;
        }
        catch (JsonException exception)
        {
            GD.PushWarning($"Pocket Factory save was ignored because it is invalid: {exception.Message}");
            return null;
        }
    }

    public static void Save(GameState state, bool isAdvancedVisualTheme)
    {
        var save = new PocketFactorySaveFile(
            PocketFactorySaveFile.CurrentVersion,
            DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            state.ToSnapshot(),
            isAdvancedVisualTheme);

        using var file = global::Godot.FileAccess.Open(SavePath, global::Godot.FileAccess.ModeFlags.Write);
        if (file is null)
        {
            GD.PushWarning("Pocket Factory save could not be written.");
            return;
        }

        file.StoreString(JsonSerializer.Serialize(save, SerializerOptions));
    }
}

public sealed record PocketFactorySaveFile(
    int Version,
    long SavedAtUnixMilliseconds,
    GameStateSnapshot GameState,
    bool IsAdvancedVisualTheme)
{
    public const int CurrentVersion = 1;
}
