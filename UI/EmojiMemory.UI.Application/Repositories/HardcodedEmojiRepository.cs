using EmojiMemory.UI.Application.Contracts;
using EmojiMemory.UI.Domain.ValueObjects;

namespace EmojiMemory.UI.Application.Repositories;

public class HardcodedEmojiRepository : IEmojiRepository
{
    private static readonly EmojiId[] _emojis =
    [
        new("🐶"),
        new("🍕"),
        new("🚀"),
        new("🐸"),
        new("🎈"),
        new("🎮"),
        new("🎁"),
        new("🧠"),
        new("🐱"),
        new("🍩")
    ];

    public IEnumerable<EmojiId> GetEmojis() => _emojis;
}
