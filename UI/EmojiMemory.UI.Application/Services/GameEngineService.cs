using System.Diagnostics;
using EmojiMemory.UI.Application.Contracts;
using EmojiMemory.UI.Domain.Entities;
using EmojiMemory.UI.Domain.Enums;
using EmojiMemory.UI.Domain.ValueObjects;

namespace EmojiMemory.UI.Application.Services;

public class GameEngineService(IHighscore highscore, ISoundService soundService)
{
    private readonly Stopwatch _stopwatch = new();
    private readonly IHighscore _highscore = highscore;
    private readonly ISoundService _soundService = soundService;
    private Card? _firstCard;
    private Card? _secondCard;
    private List<EmojiId> _emojiPool = [];

    public event Action? StateChanged;

    private void NotifyStateChanged() => StateChanged?.Invoke();

  public GameSession Session { get; private set; } = new();

    public TimeSpan Elapsed => _stopwatch.Elapsed;

    public void StartGame(GridSize size, IEnumerable<EmojiId> emojis)
    {
        _emojiPool = [.. emojis];
        var cards = BuildCards(size, _emojiPool);
        Session = new GameSession
        {
            Player = Session.Player,
            ScoreBoard = new ScoreBoard(),
            Board = new GameBoard
            {
                GridSize = size,
                Cards = cards,
                State = GameState.InProgress
            },
            State = GameState.InProgress
        };
        _firstCard = null;
        _secondCard = null;
        BeginTimer();
        NotifyStateChanged();
    }

    public void FlipCard(Position position)
    {
        if (_firstCard != null && _secondCard != null)
        {
            return;
        }

        var card = Session.Board.Cards.FirstOrDefault(c => c.Position.Equals(position));
        if (card == null || card.State != CardState.FaceDown)
        {
            return;
        }

        card.State = CardState.FaceUp;
        _ = _soundService.PlayAsync(SoundEffect.Flip);
        if (_firstCard == null)
        {
            _firstCard = card;
        }
        else _secondCard ??= card;
    }

    public async Task EvaluateFlip()
    {
        if (_firstCard == null || _secondCard == null)
        {
            return;
        }

        IncrementMoves();
        if (_firstCard.Emoji.Equals(_secondCard.Emoji))
        {
            _firstCard.State = CardState.Matched;
            _secondCard.State = CardState.Matched;
            _ = _soundService.PlayAsync(SoundEffect.Match);
        }
        else
        {
            _firstCard.State = CardState.FaceDown;
            _secondCard.State = CardState.FaceDown;
            _ = _soundService.PlayAsync(SoundEffect.Mismatch);
        }

        _firstCard = null;
        _secondCard = null;

        if (Session.Board.Cards.All(c => c.State == CardState.Matched))
        {
            Session.State = GameState.Completed;
            Session.Board.State = GameState.Completed;
            StopTimer();
            await CheckHighscoreAsync();
            _ = _soundService.PlayAsync(SoundEffect.Win);
            NotifyStateChanged();
        }
    }

    public void BeginTimer() => _stopwatch.Restart();

    public void StopTimer()
    {
        _stopwatch.Stop();
        Session.ScoreBoard.TimeElapsed = _stopwatch.Elapsed;
    }

    public void IncrementMoves() => Session.ScoreBoard.Moves++;

    public void PauseGame()
    {
        Session.State = GameState.Paused;
        Session.Board.State = GameState.Paused;
        StopTimer();
        NotifyStateChanged();
    }

    public void ResumeGame()
    {
        Session.State = GameState.InProgress;
        Session.Board.State = GameState.InProgress;
        _stopwatch.Start();
        NotifyStateChanged();
    }

    private async ValueTask CheckHighscoreAsync()
    {
        var current = new HighscoreEntry
        {
            Score = Session.ScoreBoard.Moves,
            Time = Session.ScoreBoard.TimeElapsed
        };

        var size = Session.Board.GridSize;
        var existing = await _highscore.GetScoreAsync(size);
        if (existing == null ||
            current.Time < existing.Time ||
            (current.Time == existing.Time && current.Score < existing.Score))
        {
            await _highscore.SaveScoreAsync(size, current);
        }
    }

    public void ResetGame()
    {
        StartGame(Session.Board.GridSize, _emojiPool);
    }

    private static List<Card> BuildCards(GridSize size, IEnumerable<EmojiId> emojis)
    {
        var emojiList = emojis.ToList();
        if (emojiList.Count * 2 != size.Rows * size.Columns)
        {
            throw new ArgumentException("Emoji count must be half the number of board cells.", nameof(emojis));
        }

        var deck = emojiList.Concat(emojiList).ToList();
        var rng = new Random();
        deck = [.. deck.OrderBy(_ => rng.Next())];

        var cards = new List<Card>();
        int index = 0;
        for (var r = 0; r < size.Rows; r++)
        {
            for (var c = 0; c < size.Columns; c++)
            {
                cards.Add(new Card
                {
                    Emoji = deck[index++],
                    Position = new Position(r, c),
                    State = CardState.FaceDown
                });
            }
        }

        return cards;
    }
}
