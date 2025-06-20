namespace EmojiMemory.UI.Domain.Entities
{
    using EmojiMemory.UI.Domain.Enums;

    public class GameSession
    {
        public GameBoard Board { get; init; } = new GameBoard();
        public ScoreBoard ScoreBoard { get; init; } = new ScoreBoard();
        public Player Player { get; init; } = new Player();
        public GameState State { get; set; } = GameState.NotStarted;
    }
}
