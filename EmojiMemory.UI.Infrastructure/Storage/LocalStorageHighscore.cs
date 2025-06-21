using Microsoft.JSInterop;
using System.Text.Json;
using EmojiMemory.UI.Application.Contracts;
using EmojiMemory.UI.Domain.Entities;
using EmojiMemory.UI.Domain.ValueObjects;

namespace EmojiMemory.UI.Infrastructure.Storage;

public class LocalStorageHighscore : IHighscore
{
    private readonly IJSRuntime _jsRuntime;
    private const string Key = "highscore";

    public LocalStorageHighscore(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async ValueTask SaveScoreAsync(GridSize size, HighscoreEntry score)
    {
        var scores = await GetDictionaryAsync();
        scores[$"{size.Rows}x{size.Columns}"] = score;
        var json = JsonSerializer.Serialize(scores);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", Key, json);
    }

    public async ValueTask<HighscoreEntry?> GetScoreAsync(GridSize size)
    {
        var scores = await GetDictionaryAsync();
        return scores.TryGetValue($"{size.Rows}x{size.Columns}", out var entry) ? entry : null;
    }

    private async ValueTask<Dictionary<string, HighscoreEntry>> GetDictionaryAsync()
    {
        var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", Key);
        if (string.IsNullOrEmpty(json))
        {
            return new Dictionary<string, HighscoreEntry>();
        }

        try
        {
            var result = JsonSerializer.Deserialize<Dictionary<string, HighscoreEntry>>(json);
            return result ?? new Dictionary<string, HighscoreEntry>();
        }
        catch
        {
            return new Dictionary<string, HighscoreEntry>();
        }
    }
}
