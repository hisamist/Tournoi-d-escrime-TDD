using FluentAssertions;
using TournoiEscrime.Core;

namespace TournoiEscrime.Tests.TestCase;

public class TestCaseTournamentRanking
{
    private readonly TournamentRanking _ranking;

    public TestCaseTournamentRanking()
    {
        _ranking = new TournamentRanking(new ScoreCalculator());
    }

    [Fact]
    [Trait("TestCase", "TC23")]
    [Trait("Requirement", "REQ-009")]
    public void GetRanking_Should_Return_Players_Sorted_By_Score_Desc()
    {
        // Arrange
        var players = new List<Player>
        {
            new Player { Name = "Alice", Matches = new List<MatchResult> {
                new(MatchResult.Result.Win),
                new(MatchResult.Result.Loss),
                new(MatchResult.Result.Win),
                new(MatchResult.Result.Win),
                } }, // 9 points
            new Player { Name = "Bob", Matches = new List<MatchResult> { new(MatchResult.Result.Win), new(MatchResult.Result.Draw) } }, // 4 points
            new Player { Name = "Charlie", Matches = new List<MatchResult> { new(MatchResult.Result.Draw) } }, // 1 point
        };

        // Act
        var rankedPlayers = _ranking.GetRanking(players);

        // Assert
        rankedPlayers.Should().HaveCount(3);
        rankedPlayers[0].Name.Should().Be("Alice"); // 9 points
        rankedPlayers[1].Name.Should().Be("Bob"); // 4 points
        rankedPlayers[2].Name.Should().Be("Charlie"); // 1 point
    }

    [Fact]
    [Trait("TestCase", "TC24")]
    [Trait("Requirement", "REQ-010")]
    public void GetRanking_Should_Preserve_Input_Order_For_Tied_Scores()
    {
        // Arrange
        var players = new List<Player>
        {
            new Player { Name = "Bob", Matches = new List<MatchResult> { new(MatchResult.Result.Win), new(MatchResult.Result.Draw) } }, // 4 points
            new Player { Name = "Dave", Matches = new List<MatchResult> { new(MatchResult.Result.Win), new(MatchResult.Result.Draw) } } // 4 points
        };

        // Act
        var rankedPlayers = _ranking.GetRanking(players);

        // Assert
        rankedPlayers.Should().HaveCount(2);
        rankedPlayers[0].Name.Should().Be("Bob"); // 4 points
        rankedPlayers[1].Name.Should().Be("Dave"); // 4 points, but after Bob in input list
    }

    [Fact]
    [Trait("TestCase", "TC25")]
    [Trait("Requirement", "REQ-011")]
    public void GetRanking_Should_Return_Player_With_Highest_Score_As_Champion()
    {
        // Arrange
        var players = new List<Player>
        {
            new Player { Name = "Eve", Matches = new List<MatchResult> { new(MatchResult.Result.Win), new(MatchResult.Result.Loss) } }, // 6 points
            new Player { Name = "Frank", Matches = new List<MatchResult> {
                new(MatchResult.Result.Win),
                new(MatchResult.Result.Win),
                new(MatchResult.Result.Win),
                new(MatchResult.Result.Draw)
                } }, // 14 points
            new Player { Name = "Grace", Matches = new List<MatchResult> { new(MatchResult.Result.Draw) } } // 3 points
        };

        // Act
        var champion = _ranking.GetChampion(players);

        // Assert
        champion.Should().NotBeNull();
        champion!.Name.Should().Be("Frank"); // 14 points, champion
    }

    [Fact]
    [Trait("TestCase", "TC26")]
    [Trait("Requirement", "REQ-012")]
    public void GetChampion_Should_Return_Null_If_All_Players_Disqualified()
    {
        // Arrange
        var players = new List<Player>
        {
            new Player { Name = "Heidi", IsDisqualified = true },
            new Player { Name = "Ivan", IsDisqualified = true },
            new Player { Name = "Judy", IsDisqualified = true }
        };

        // Act
        var champion = _ranking.GetChampion(players);

        // Assert
        champion.Should().BeNull(); // All players disqualified
    }
}