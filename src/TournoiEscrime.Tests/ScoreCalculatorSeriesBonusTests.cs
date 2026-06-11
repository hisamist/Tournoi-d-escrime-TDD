using FluentAssertions;
using TournoiEscrime.Core;

namespace TournoiEscrime.Tests;

/// <summary>
/// TC05-TC10 du plan de test : règles du bonus de série de victoires consécutives
/// (REQ-004, REQ-005).
/// </summary>
public class ScoreCalculatorSeriesBonusTests
{
    private readonly ScoreCalculator _calculator;

    public ScoreCalculatorSeriesBonusTests()
    {
        _calculator = new ScoreCalculator();
    }

    [Fact]
    [Trait("Requirement", "REQ-001")]
    [Trait("Requirement", "REQ-002")]
    [Trait("Requirement", "REQ-004")]
    public void Should_Add_Bonus_When_Exactly_Three_Consecutive_Wins()
    {
        // Arrange
        var matches = new List<MatchResult>
        {
            new(MatchResult.Result.Win),
            new(MatchResult.Result.Win),
            new(MatchResult.Result.Win),
            new(MatchResult.Result.Draw)
        };

        // Act
        var score = _calculator.CalculateScore(matches);

        // Assert
        score.Should().Be(15, "3*3 + 1 + 5 (bonus for the 3-win streak) = 15 (REQ-004)");
    }

    [Fact]
    [Trait("Requirement", "REQ-004")]
    [Trait("Requirement", "REQ-005")]
    public void Should_Add_Bonus_Only_Once_For_Four_Consecutive_Wins()
    {
        // Arrange
        var matches = new List<MatchResult>
        {
            new(MatchResult.Result.Win),
            new(MatchResult.Result.Win),
            new(MatchResult.Result.Win),
            new(MatchResult.Result.Win)
        };

        // Act
        var score = _calculator.CalculateScore(matches);

        // Assert
        score.Should().Be(17, "4*3 + 5 (bonus awarded only once) = 17 (REQ-004, REQ-005)");
    }

    [Fact]
    [Trait("Requirement", "REQ-004")]
    public void Should_Not_Add_Bonus_When_Streak_Is_Interrupted()
    {
        // Arrange
        var matches = new List<MatchResult>
        {
            new(MatchResult.Result.Win),
            new(MatchResult.Result.Win),
            new(MatchResult.Result.Loss),
            new(MatchResult.Result.Win)
        };

        // Act
        var score = _calculator.CalculateScore(matches);

        // Assert
        score.Should().Be(9, "3+3+0+3 = 9, no streak of 3 consecutive wins (REQ-004)");
    }

    [Fact]
    [Trait("Requirement", "REQ-005")]
    public void Should_Add_Bonus_For_Each_Separate_Streak_Of_Three_Or_More()
    {
        // Arrange
        var matches = new List<MatchResult>
        {
            new(MatchResult.Result.Win),
            new(MatchResult.Result.Win),
            new(MatchResult.Result.Win),
            new(MatchResult.Result.Loss),
            new(MatchResult.Result.Win),
            new(MatchResult.Result.Win),
            new(MatchResult.Result.Win),
            new(MatchResult.Result.Win)
        };

        // Act
        var score = _calculator.CalculateScore(matches);

        // Assert
        score.Should().Be(31, "21 base points + 5 (1st streak of 3) + 5 (2nd streak of 4) = 31 (REQ-005)");
    }

    [Fact]
    [Trait("Requirement", "REQ-004")]
    public void Should_Not_Add_Bonus_When_No_Streak_Of_Three()
    {
        // Arrange
        var matches = new List<MatchResult>
        {
            new(MatchResult.Result.Win),
            new(MatchResult.Result.Draw),
            new(MatchResult.Result.Win),
            new(MatchResult.Result.Win)
        };

        // Act
        var score = _calculator.CalculateScore(matches);

        // Assert
        score.Should().Be(10, "3+1+3+3 = 10, maximum streak of 2 consecutive wins (REQ-004)");
    }

    [Fact]
    [Trait("Requirement", "REQ-004")]
    [Trait("Requirement", "REQ-005")]
    public void Should_Add_Bonus_Only_For_Trailing_Streak_After_Interruption()
    {
        // Arrange
        var matches = new List<MatchResult>
        {
            new(MatchResult.Result.Win),
            new(MatchResult.Result.Loss),
            new(MatchResult.Result.Win),
            new(MatchResult.Result.Win),
            new(MatchResult.Result.Win)
        };

        // Act
        var score = _calculator.CalculateScore(matches);

        // Assert
        score.Should().Be(17, "3+0+3+3+3 + 5 (bonus for the last 3 wins) = 17 (REQ-004, REQ-005)");
    }
}
