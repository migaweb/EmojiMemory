using EmojiMemory.UI.Domain.Entities;

namespace EmojiMemory.UI.Application.Contracts
{
    public interface IHighscore
    {
        ValueTask SaveScoreAsync(HighscoreEntry score);
        ValueTask<HighscoreEntry?> GetScoreAsync();
    }
}
