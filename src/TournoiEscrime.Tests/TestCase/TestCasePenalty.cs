using FluentAssertions;
using TournoiEscrime.Core;

namespace TournoiEscrime.Tests.TestCase;

public class TestCasePenalty
{
    private readonly ScoreCalculator _calculator;

    public TestCasePenalty()
    {
        _calculator = new ScoreCalculator();
    }

    [Fact]
    [Trait("TestCase", "TC14")]
    [Trait("Requirement", "REQ-007")]
    public void Should_Subtract_Penalty_From_Score()
    {
        // Arrange
        var matches = new List<MatchResult>
        {
            new(MatchResult.Result.Win),
            new(MatchResult.Result.Win),
            new(MatchResult.Result.Draw),
            new(MatchResult.Result.Win)
        };

        // Act
        var score = _calculator.CalculateScore(matches, penaltyPoints: 3);

        // Assert
        score.Should().Be(7, "10 base points - 3 penalty points = 7 (REQ-007)");
    }

    [Fact]
    [Trait("TestCase", "TC15")]
    [Trait("Requirement", "REQ-007")]
    public void Should_Return_Zero_When_Penalty_Exceeds_Score()
    {
        // Arrange
        var matches = new List<MatchResult>
        {
            new(MatchResult.Result.Win),
            new(MatchResult.Result.Draw),
            new(MatchResult.Result.Draw)
        };

        // Act
        var score = _calculator.CalculateScore(matches, penaltyPoints: 8);

        // Assert
        score.Should().Be(0, "5 points - 8 penalty points cannot result in a negative score (REQ-007)");
    }

    [Fact]
    [Trait("TestCase", "TC16")]
    [Trait("Requirement", "REQ-007")]
    public void Should_Return_Zero_When_Penalty_Equals_Score()
    {
        // Arrange
        var matches = new List<MatchResult>
        {
            new(MatchResult.Result.Win),
            new(MatchResult.Result.Win),
            new(MatchResult.Result.Draw)
        };

        // Act
        var score = _calculator.CalculateScore(matches, penaltyPoints: 7);

        // Assert
        score.Should().Be(0, "7 points - 7 penalty points = 0 (REQ-007)");
    }

    [Fact]
    [Trait("TestCase", "TC27")]
    [Trait("Requirement", "REQ-007")]
    public void CalculateScore_Player_Should_Subtract_PenaltyPoints_From_Score()
    {
        // Arrange
        IScoreCalculator calculator = _calculator;
        var player = new Player
        {
            Name = "Zoe",
            Matches = new List<MatchResult>
            {
                new(MatchResult.Result.Win),
                new(MatchResult.Result.Win),
                new(MatchResult.Result.Draw),
                new(MatchResult.Result.Win)
            },
            PenaltyPoints = 3
        };

        // Act
        var score = calculator.CalculateScore(player);

        // Assert
        score.Should().Be(7, "10 base points - 3 penalty points = 7 (REQ-007)");
    }
}
