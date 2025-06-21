namespace EmojiMemory.UI.Application.Contracts
{
    public interface IHighscore
    {
        ValueTask SaveScoreAsync(int score);
        ValueTask<int?> GetScoreAsync();
    }
}
