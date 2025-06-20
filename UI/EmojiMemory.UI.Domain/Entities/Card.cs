namespace EmojiMemory.UI.Domain.Entities
{
    using EmojiMemory.UI.Domain.Enums;
    using EmojiMemory.UI.Domain.ValueObjects;

    public class Card
    {
        public EmojiId Emoji { get; init; } = new EmojiId(string.Empty);
        public Position Position { get; init; } = new Position(0, 0);
        public CardState State { get; set; } = CardState.FaceDown;
    }
}
