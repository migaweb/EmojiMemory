
using EmojiMemory.UI.Domain.Enums;

namespace EmojiMemory.UI.Application.Contracts;

public interface ISoundService
{
  Task PlayAsync(SoundEffect soundEffect);
}