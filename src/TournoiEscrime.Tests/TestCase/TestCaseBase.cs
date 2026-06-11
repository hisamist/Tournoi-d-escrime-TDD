using FluentAssertions;
using TournoiEscrime.Core;

namespace TournoiEscrime.Tests.TestCase;

public class TestCaseBase
{
    [Fact]
    [Trait("Requirement", "REQ-Z-001")]
    [Trait("Requirement", "REQ-Z-002")]
    [Trait("Requirement", "REQ-Z-003")]
    [Trait("TestCase", "TC01")]
    public void CalculateScore_SimpleCalculateWithoutBonus_ReturnsExpectedResult()
    {
        // Arrange
        var scoreCalculator = new ScoreCalculator();
        var matches = new List<MatchResult>
        {
            new MatchResult(MatchResult.Result.Win),
            new MatchResult(MatchResult.Result.Draw),
            new MatchResult(MatchResult.Result.Loss)
        };

        // Act
        int result = scoreCalculator.CalculateScore(matches);

        // Assert
        result.Should().Be(4);
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-002")]
    [Trait("TestCase", "TC02")]
    public void CalculateScore_AllDraws_ReturnsExpectedResult()
    {
        // Arrange
        var scoreCalculator = new ScoreCalculator();
        var matches = new List<MatchResult>
        {
            new MatchResult(MatchResult.Result.Draw),
            new MatchResult(MatchResult.Result.Draw),
            new MatchResult(MatchResult.Result.Draw)
        };

        // Act
        int result = scoreCalculator.CalculateScore(matches);

        // Assert
        result.Should().Be(3);
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-002")]
    [Trait("TestCase", "TC03")]
    public void CalculateScore_AllLosses_ReturnsExpectedResult()
    {
        // Arrange
        var scoreCalculator = new ScoreCalculator();
        var matches = new List<MatchResult>
        {
            new MatchResult(MatchResult.Result.Loss),
            new MatchResult(MatchResult.Result.Loss)
        };

        // Act
        int result = scoreCalculator.CalculateScore(matches);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-001")]
    [Trait("TestCase", "TC04")]
    public void CalculateScore_MultipleWinsWithoutBonus_ReturnsExpectedResult()
    {
        // Arrange
        var scoreCalculator = new ScoreCalculator();
        var matches = new List<MatchResult>
        {
            new MatchResult(MatchResult.Result.Win),
            new MatchResult(MatchResult.Result.Win)
        };

        // Act
        int result = scoreCalculator.CalculateScore(matches);

        // Assert
        result.Should().Be(6);
    }
}