using EmojiMemory.UI.Application.Contracts;
using EmojiMemory.UI.Domain.Enums;
using Microsoft.JSInterop;

namespace EmojiMemory.UI.Infrastructure.Sound;

public class SoundService(IJSRuntime js) : ISoundService
{
  private readonly IJSRuntime _js = js;

  public async Task PlayAsync(SoundEffect soundEffect)
  {
    await _js.InvokeVoidAsync("soundPlayer.play", soundEffect.ToString());
  }
}

