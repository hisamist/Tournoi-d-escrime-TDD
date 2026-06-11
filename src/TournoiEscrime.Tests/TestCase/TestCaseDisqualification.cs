using FluentAssertions;
using TournoiEscrime.Core;

namespace TournoiEscrime.Tests.TestCase;

public class TestCaseDisqualification
{
    [Fact]
    [Trait("Requirement", "REQ-Z-006")]
    [Trait("TestCase", "TC11")]
    public void CalculateScore_Disqualification_ReturnsZero()
    {
        // Arrange
        var scoreCalculator = new ScoreCalculator();
        var matches = new List<MatchResult>
        {
            new MatchResult(MatchResult.Result.Win),
            new MatchResult(MatchResult.Result.Win),
            new MatchResult(MatchResult.Result.Win)
        };

        // Act
        int result = scoreCalculator.CalculateScore(matches, isDisqualified: true);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-006")]
    [Trait("TestCase", "TC12")]
    public void CalculateScore_DisqualificationWithoutAnyMatches_ReturnsZero()
    {
        // Arrange
        var scoreCalculator = new ScoreCalculator();
        var matches = new List<MatchResult>();

        // Act
        int result = scoreCalculator.CalculateScore(matches, isDisqualified: true);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-006")]
    [Trait("Requirement", "REQ-Z-007")]
    [Trait("TestCase", "TC13")]
    public void CalculateScore_DisqualificationWithPenalty_ReturnsZero()
    {
        // Arrange
        var scoreCalculator = new ScoreCalculator();
        var matches = new List<MatchResult>
        {
            new MatchResult(MatchResult.Result.Win),
            new MatchResult(MatchResult.Result.Win),
            new MatchResult(MatchResult.Result.Win),
        };

        // Act
        int result = scoreCalculator.CalculateScore(matches, isDisqualified: true, penaltyPoints: 5);

        // Assert
        result.Should().Be(0);
    }
}