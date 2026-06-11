using FluentAssertions;
using Moq;
using TournoiEscrime.Core;
using Xunit;

namespace TournoiEscrime.Tests.TestCase;

public class TestCaseTournamentRanking
{
    private readonly TournamentRanking _ranking;
    private readonly Mock<IScoreCalculator> _mockCalculator;

    public TestCaseTournamentRanking()
    {
        // 1. On crée le Mock de la dépendance
        _mockCalculator = new Mock<IScoreCalculator>();

        // 2. On l'injecte dans la classe à tester
        _ranking = new TournamentRanking(_mockCalculator.Object);
    }

    [Fact]
    [Trait("TestCase", "TC23")]
    [Trait("Requirement", "REQ-009")]
    public void GetRanking_Should_Return_Players_Sorted_By_Score_Desc()
    {
        // Arrange
        var alice = new Player { Name = "Alice" };
        var bob = new Player { Name = "Bob" };
        var charlie = new Player { Name = "Charlie" };

        // 🎯 On simule (Setup) les scores directement, plus besoin de remplir la liste Matches !
        _mockCalculator.Setup(c => c.CalculateScore(alice)).Returns(9);
        _mockCalculator.Setup(c => c.CalculateScore(bob)).Returns(4);
        _mockCalculator.Setup(c => c.CalculateScore(charlie)).Returns(1);

        var players = new List<Player> { bob, charlie, alice }; // Mélangés au départ

        // Act
        var rankedPlayers = _ranking.GetRanking(players);

        // Assert
        rankedPlayers.Should().HaveCount(3);
        rankedPlayers[0].Name.Should().Be("Alice");   // 9 pts
        rankedPlayers[1].Name.Should().Be("Bob");     // 4 pts
        rankedPlayers[2].Name.Should().Be("Charlie"); // 1 pt
    }

    [Fact]
    [Trait("TestCase", "TC24")]
    [Trait("Requirement", "REQ-010")]
    public void GetRanking_Should_Preserve_Input_Order_For_Tied_Scores()
    {
        // Arrange
        var bob = new Player { Name = "Bob" };
        var dave = new Player { Name = "Dave" };

        _mockCalculator.Setup(c => c.CalculateScore(bob)).Returns(4);
        _mockCalculator.Setup(c => c.CalculateScore(dave)).Returns(4);

        var players = new List<Player> { bob, dave };

        // Act
        var rankedPlayers = _ranking.GetRanking(players);

        // Assert
        rankedPlayers.Should().HaveCount(2);
        rankedPlayers[0].Name.Should().Be("Bob");
        rankedPlayers[1].Name.Should().Be("Dave");
    }

    [Fact]
    [Trait("TestCase", "TC25")]
    [Trait("Requirement", "REQ-011")]
    public void GetRanking_Should_Return_Player_With_Highest_Score_As_Champion()
    {
        // Arrange
        var eve = new Player { Name = "Eve" };
        var frank = new Player { Name = "Frank" };
        var grace = new Player { Name = "Grace" };

        _mockCalculator.Setup(c => c.CalculateScore(eve)).Returns(6);
        _mockCalculator.Setup(c => c.CalculateScore(frank)).Returns(14);
        _mockCalculator.Setup(c => c.CalculateScore(grace)).Returns(3);

        var players = new List<Player> { eve, frank, grace };

        // Act
        var champion = _ranking.GetChampion(players);

        // Assert
        champion.Should().NotBeNull();
        champion!.Name.Should().Be("Frank");
    }

    [Fact]
    [Trait("TestCase", "TC26")]
    [Trait("Requirement", "REQ-012")]
    public void GetChampion_Should_Return_Null_If_All_Players_Disqualified()
    {
        // Arrange
        var heidi = new Player { Name = "Heidi", IsDisqualified = true };
        var ivan = new Player { Name = "Ivan", IsDisqualified = true };

        // Même s'ils ont des scores, ils sont disqualifiés
        _mockCalculator.Setup(c => c.CalculateScore(heidi)).Returns(20);
        _mockCalculator.Setup(c => c.CalculateScore(ivan)).Returns(15);

        var players = new List<Player> { heidi, ivan };

        // Act
        var champion = _ranking.GetChampion(players);

        // Assert
        champion.Should().BeNull();
    }
}