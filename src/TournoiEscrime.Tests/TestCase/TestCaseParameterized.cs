using FluentAssertions;
using TournoiEscrime.Core;

namespace TournoiEscrime.Tests.TestCase;

public class TestCaseParameterized
{
    [Theory]
    [Trait("Requirement", "REQ-Z-001")]
    [Trait("Requirement", "REQ-Z-002")]
    [Trait("Requirement", "REQ-Z-003")]
    [Trait("Requirement", "REQ-Z-004")]
    [Trait("TestCase", "TC21")]
    [InlineData(3, 0, 0, 14)]
    [InlineData(2, 1, 0, 7)]
    [InlineData(0, 0, 3, 0)]
    public void CalculateScore_VariousMatchResults_ReturnsExpectedResult(int wins, int draws, int losses, int expected)
    {
        // Arrange
        var scoreCalculator = new ScoreCalculator();

        var matches = new List<MatchResult>();
        for (int i = 0; i < wins; i++)
            matches.Add(new(MatchResult.Result.Win));
        for (int i = 0; i < draws; i++)
            matches.Add(new(MatchResult.Result.Draw));
        for (int i = 0; i < losses; i++)
            matches.Add(new(MatchResult.Result.Loss));

        // Act
        int result = scoreCalculator.CalculateScore(matches);

        // Assert
        result.Should().Be(expected);
    }

    public static IEnumerable<object[]> ComplexMatchScenarios => new List<object[]>
    {
        new object[] { new List<MatchResult> {
            new MatchResult(MatchResult.Result.Draw),
            new MatchResult(MatchResult.Result.Win),
            new MatchResult(MatchResult.Result.Win),
            new MatchResult(MatchResult.Result.Win),
            new MatchResult(MatchResult.Result.Draw),
            new MatchResult(MatchResult.Result.Win),
            new MatchResult(MatchResult.Result.Win),
            new MatchResult(MatchResult.Result.Win)
            }, 30},
        new object[] { new List<MatchResult> {
            new MatchResult(MatchResult.Result.Win),
            new MatchResult(MatchResult.Result.Win),
            new MatchResult(MatchResult.Result.Loss),
            new MatchResult(MatchResult.Result.Loss),
            new MatchResult(MatchResult.Result.Win),
            new MatchResult(MatchResult.Result.Win),
            new MatchResult(MatchResult.Result.Win),
        }, 20 },
        new object[] { new List<MatchResult>
        {
            new MatchResult(MatchResult.Result.Win),
            new MatchResult(MatchResult.Result.Win),
            new MatchResult(MatchResult.Result.Win),
            new MatchResult(MatchResult.Result.Win),
            new MatchResult(MatchResult.Result.Win),
            new MatchResult(MatchResult.Result.Win),
        }, 28 },
    };

    [Theory]
    [Trait("Requirement", "REQ-Z-004")]
    [Trait("Requirement", "REQ-Z-005")]
    [Trait("TestCase", "TC22")]
    [MemberData(nameof(ComplexMatchScenarios))]
    public void CalculateScore_ComplexMatchScenarios_ReturnsExpectedResult(List<MatchResult> matches, int expectedScore)
    {
        // Arrange
        var scoreCalculator = new ScoreCalculator();

        // Act
        int result = scoreCalculator.CalculateScore(matches);

        // Assert
        result.Should().Be(expectedScore);
    }
}
