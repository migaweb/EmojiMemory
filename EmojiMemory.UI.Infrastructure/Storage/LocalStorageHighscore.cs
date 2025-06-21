using Microsoft.JSInterop;
using System.Text.Json;
using EmojiMemory.UI.Application.Contracts;
using EmojiMemory.UI.Domain.Entities;

namespace EmojiMemory.UI.Infrastructure.Storage;

public class LocalStorageHighscore : IHighscore
{
    private readonly IJSRuntime _jsRuntime;
    private const string Key = "highscore";

    public LocalStorageHighscore(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async ValueTask SaveScoreAsync(HighscoreEntry score)
    {
        var json = JsonSerializer.Serialize(score);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", Key, json);
    }

    public async ValueTask<HighscoreEntry?> GetScoreAsync()
    {
        var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", Key);
        if (string.IsNullOrEmpty(json))
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<HighscoreEntry>(json);
        }
        catch
        {
            return null;
        }
    }
}
