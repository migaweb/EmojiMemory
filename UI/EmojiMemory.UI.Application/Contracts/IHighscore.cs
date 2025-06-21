using EmojiMemory.UI.Domain.Entities;

namespace EmojiMemory.UI.Application.Contracts
{
    using EmojiMemory.UI.Domain.ValueObjects;

    public interface IHighscore
    {
        ValueTask SaveScoreAsync(GridSize size, HighscoreEntry score);
        ValueTask<HighscoreEntry?> GetScoreAsync(GridSize size);
    }
}
