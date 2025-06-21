namespace EmojiMemory.UI.Domain.Entities;

public class HighscoreEntry
{
    public int Score { get; set; } = 0;
    public TimeSpan Time { get; set; } = TimeSpan.Zero;
}
