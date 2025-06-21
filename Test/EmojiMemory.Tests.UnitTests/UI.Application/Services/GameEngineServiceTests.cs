using EmojiMemory.UI.Application.Services;
using EmojiMemory.UI.Application.Contracts;
using EmojiMemory.UI.Domain.Entities;
using EmojiMemory.UI.Domain.Enums;
using EmojiMemory.UI.Domain.ValueObjects;
using Xunit;
using System.Linq;
using System;
using System.Collections.Generic;

namespace EmojiMemory.Tests.UnitTests.UI.Application.Services;

public class GameEngineServiceTests
{
    private class StubHighscore : IHighscore
    {
        public Dictionary<string, HighscoreEntry?> Stored { get; } = new();

        public ValueTask SaveScoreAsync(GridSize size, HighscoreEntry score)
        {
            Stored[$"{size.Rows}x{size.Columns}"] = score;
            return ValueTask.CompletedTask;
        }

        public ValueTask<HighscoreEntry?> GetScoreAsync(GridSize size)
        {
            Stored.TryGetValue($"{size.Rows}x{size.Columns}", out var entry);
            return ValueTask.FromResult(entry);
        }
    }

    private static readonly EmojiId[] _emojis = new[]
    {
        new EmojiId("😀"),
        new EmojiId("🎉"),
        new EmojiId("🚀"),
        new EmojiId("🍕")
    };

    [Fact]
    public void StartGame_InitializesSessionWithCards()
    {
        var engine = new GameEngineService(new StubHighscore());
        engine.StartGame(new GridSize(2, 4), _emojis.Take(4));

        Assert.Equal(GameState.InProgress, engine.Session.State);
        Assert.Equal(8, engine.Session.Board.Cards.Count);
        Assert.All(engine.Session.Board.Cards, c => Assert.Equal(CardState.FaceDown, c.State));
        Assert.Equal(0, engine.Session.ScoreBoard.Moves);
    }

    [Fact]
    public void FlipCard_FlipsFaceDownCard()
    {
        var engine = new GameEngineService(new StubHighscore());
        engine.StartGame(new GridSize(2, 4), _emojis.Take(4));
        var pos = engine.Session.Board.Cards.First().Position;

        engine.FlipCard(pos);

        var card = engine.Session.Board.Cards.First(c => c.Position.Equals(pos));
        Assert.Equal(CardState.FaceUp, card.State);
    }

    [Fact]
    public void EvaluateFlip_MatchingCards_SetAsMatched()
    {
        var engine = new GameEngineService(new StubHighscore());
        engine.StartGame(new GridSize(2, 4), _emojis.Take(4));
        var pair = engine.Session.Board.Cards
            .GroupBy(c => c.Emoji)
            .First(g => g.Count() == 2)
            .Select(c => c.Position)
            .ToArray();

        engine.FlipCard(pair[0]);
        engine.FlipCard(pair[1]);
        engine.EvaluateFlip();

        var cards = engine.Session.Board.Cards.Where(c => pair.Contains(c.Position)).ToList();
        Assert.All(cards, c => Assert.Equal(CardState.Matched, c.State));
        Assert.Equal(1, engine.Session.ScoreBoard.Moves);
    }

    [Fact]
    public void EvaluateFlip_AllCardsMatched_EndsGame()
    {
        var engine = new GameEngineService(new StubHighscore());
        engine.StartGame(new GridSize(2, 2), _emojis.Take(2));
        var positions = engine.Session.Board.Cards.Select(c => c.Position).ToArray();

        foreach (var pos in positions)
            engine.FlipCard(pos);

        engine.EvaluateFlip();
        engine.EvaluateFlip();

        Assert.Equal(GameState.Completed, engine.Session.State);
        Assert.Equal(GameState.Completed, engine.Session.Board.State);
    }

    [Fact]
    public void PauseGame_SetsStateToPaused()
    {
        var engine = new GameEngineService(new StubHighscore());
        engine.StartGame(new GridSize(2, 2), _emojis.Take(2));

        engine.PauseGame();

        Assert.Equal(GameState.Paused, engine.Session.State);
        Assert.Equal(GameState.Paused, engine.Session.Board.State);
    }

    [Fact]
    public void ResumeGame_FromPaused_SetsStateToInProgress()
    {
        var engine = new GameEngineService(new StubHighscore());
        engine.StartGame(new GridSize(2, 2), _emojis.Take(2));
        engine.PauseGame();

        engine.ResumeGame();

        Assert.Equal(GameState.InProgress, engine.Session.State);
        Assert.Equal(GameState.InProgress, engine.Session.Board.State);
    }
}
