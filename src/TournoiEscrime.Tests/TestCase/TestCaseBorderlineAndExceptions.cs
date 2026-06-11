using FluentAssertions;
using TournoiEscrime.Core;

namespace TournoiEscrime.Tests.TestCase;

public class TestCaseBorderlineAndExceptions
{
    private readonly ScoreCalculator _calculator;

    public TestCaseBorderlineAndExceptions()
    {
        _calculator = new ScoreCalculator();
    }

    [Fact]
    [Trait("TestCase", "TC17")]
    [Trait("Requirement", "REQ-001")]
    [Trait("Requirement", "REQ-002")]
    [Trait("Requirement", "REQ-003")]
    public void Should_Return_Zero_When_Match_List_Is_Empty()
    {
        // Arrange
        var matches = new List<MatchResult>();

        // Act
        var score = _calculator.CalculateScore(matches);

        // Assert
        score.Should().Be(0, "an empty match list scores zero points");
    }

    [Fact]
    [Trait("TestCase", "TC18")]
    [Trait("Requirement", "REQ-008")]
    public void Should_Throw_ArgumentNullException_When_Matches_Is_Null()
    {
        // Act
        Action act = () => _calculator.CalculateScore(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("matches")
            .WithMessage("*cannot be null*");
    }

    [Fact]
    [Trait("TestCase", "TC19")]
    [Trait("Requirement", "REQ-008")]
    public void Should_Throw_ArgumentException_When_Penalty_Is_Negative()
    {
        // Arrange
        var matches = new List<MatchResult> { new(MatchResult.Result.Win) };

        // Act
        Action act = () =>
        _calculator.CalculateScore(matches, penaltyPoints: -5);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("penaltyPoints")
            .WithMessage("*negative*");
    }

    [Fact]
    [Trait("TestCase", "TC20")]
    [Trait("Requirement", "REQ-001")]
    [Trait("Requirement", "REQ-002")]
    [Trait("Requirement", "REQ-003")]
    [Trait("Requirement", "REQ-004")]
    [Trait("Requirement", "REQ-005")]
    public void Should_Calculate_Score_For_Long_Tournament_With_Repeating_Pattern()
    {
        // Arrange
        var matches = new List<MatchResult>();
        for (var i = 0; i < 25; i++)
        {
            matches.Add(new MatchResult(MatchResult.Result.Win));
            matches.Add(new MatchResult(MatchResult.Result.Win));
            matches.Add(new MatchResult(MatchResult.Result.Win));
            matches.Add(new MatchResult(MatchResult.Result.Loss));
        }

        // Act
        var score = _calculator.CalculateScore(matches);

        // Assert
        score.Should().Be(350, "25 x (3+3+3+0 + 5 bonus) = 25 x 14 = 350");
    }
}
