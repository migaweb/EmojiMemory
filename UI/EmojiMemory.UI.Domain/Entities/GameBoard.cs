namespace EmojiMemory.UI.Domain.Entities
{
    using EmojiMemory.UI.Domain.Enums;
    using EmojiMemory.UI.Domain.ValueObjects;

    public class GameBoard
    {
        public GridSize GridSize { get; init; } = new GridSize(4, 4);
        public List<Card> Cards { get; init; } = new();
        public GameState State { get; set; } = GameState.NotStarted;
    }
}
