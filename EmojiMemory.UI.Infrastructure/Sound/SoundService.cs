using EmojiMemory.UI.Application.Contracts;
using EmojiMemory.UI.Domain.Enums;
using Microsoft.JSInterop;

namespace EmojiMemory.UI.Infrastructure.Sound;

public class SoundService : ISoundService
{
  private readonly IJSRuntime js;

  public SoundService(IJSRuntime js) => this.js = js;

  public async Task PlayAsync(SoundEffect soundEffect)
  {
    await js.InvokeVoidAsync("soundPlayer.play", soundEffect.ToString());
  }
}

