using System.Collections.Generic;
using EmojiMemory.UI.Domain.ValueObjects;

namespace EmojiMemory.UI.Application.Contracts;

public interface IEmojiRepository
{
    IEnumerable<EmojiId> GetEmojis();
}
