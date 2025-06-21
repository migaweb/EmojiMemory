using Microsoft.JSInterop;
using EmojiMemory.UI.Application.Contracts;

namespace EmojiMemory.UI.Infrastructure.Storage;

public class LocalStorageHighscore : IHighscore
{
    private readonly IJSRuntime _jsRuntime;
    private const string Key = "highscore";

    public LocalStorageHighscore(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async ValueTask SaveScoreAsync(int score)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", Key, score.ToString());
    }

    public async ValueTask<int?> GetScoreAsync()
    {
        var result = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", Key);
        if (int.TryParse(result, out var value))
        {
            return value;
        }
        return null;
    }
}
