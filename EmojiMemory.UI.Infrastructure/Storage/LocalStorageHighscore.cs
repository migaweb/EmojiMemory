using Microsoft.JSInterop;
using System.Text.Json;
using EmojiMemory.UI.Application.Contracts;
using EmojiMemory.UI.Domain.Entities;
using EmojiMemory.UI.Domain.ValueObjects;

namespace EmojiMemory.UI.Infrastructure.Storage;

public class LocalStorageHighscore(IJSRuntime jsRuntime) : IHighscore
{
    private readonly IJSRuntime _jsRuntime = jsRuntime;
    private const string _key = "highscore";

  public async ValueTask SaveScoreAsync(GridSize size, HighscoreEntry score)
    {
        var scores = await GetDictionaryAsync();
        scores[$"{size.Rows}x{size.Columns}"] = score;
        var json = JsonSerializer.Serialize(scores);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", _key, json);
    }

    public async ValueTask<HighscoreEntry?> GetScoreAsync(GridSize size)
    {
        var scores = await GetDictionaryAsync();
        return scores.TryGetValue($"{size.Rows}x{size.Columns}", out var entry) ? entry : null;
    }

    private async ValueTask<Dictionary<string, HighscoreEntry>> GetDictionaryAsync()
    {
        var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", _key);
        if (string.IsNullOrEmpty(json))
        {
            return [];
        }

        try
        {
            var result = JsonSerializer.Deserialize<Dictionary<string, HighscoreEntry>>(json);
            return result ?? [];
        }
        catch
        {
            return [];
        }
    }
}
