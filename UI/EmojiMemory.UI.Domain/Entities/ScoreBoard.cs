namespace EmojiMemory.UI.Domain.Entities
{
    public class ScoreBoard
    {
        public int Moves { get; set; } = 0;
        public TimeSpan TimeElapsed { get; set; } = TimeSpan.Zero;
    }
}
