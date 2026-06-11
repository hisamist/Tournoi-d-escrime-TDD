using FluentAssertions;
using TournoiEscrime.Core;

public class TestCaseParameterized
{
    [Theory]
    [Trait("Requirement", "REQ-Z-001")]
    [Trait("Requirement", "REQ-Z-002")]
    [Trait("Requirement", "REQ-Z-003")]
    [Trait("Requirement", "REQ-Z-004")]
    [Trait("TestCase", "TC21")]
    [InlineData(new int[] { 3, 0, 0 }, 14)]
    [InlineData(new int[] { 2, 1, 0 }, 7)]
    [InlineData(new int[] { 0, 0, 3 }, 0)]
    public void CalculateScore_VariousMatchResults_ReturnsExpectedResult(int[] matchResults, int expectedScore)
    {
        // Arrange
        var scoreCalculator = new ScoreCalculator();
        var matches = new List<MatchResult>();
        foreach (var matchResult in matchResults)
        {
            matches.Add(new MatchResult((MatchResult.Result)matchResult));
        }

        // Act
        int result = scoreCalculator.CalculateScore(matches);

        // Assert
        result.Should().Be(expectedScore);
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
        }, 28 },
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
        }, 23 },
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
