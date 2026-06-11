using TournoiEscrime.Core;

public class Player
{
    public required string Name { get; set; }
    public List<MatchResult> Matches { get; set; } = new();
    public bool IsDisqualified { get; set; }
    public int PenaltyPoints { get; set; }
}

public class TournamentRanking
{
    private readonly ScoreCalculator _scoreCalculator;

    public TournamentRanking(ScoreCalculator scoreCalculator)
    {
        _scoreCalculator = scoreCalculator;
    }

    /// <summary>Classe les joueurs par score décroissant</summary>
    public List<Player> GetRanking(List<Player> players){
        var players_rankeds = new List<(Player player, int score)>();

        foreach (var player in players)        {
            if (player.IsDisqualified)
            {
                continue;
            }
            var score = _scoreCalculator.CalculateScore(player.Matches, player.IsDisqualified, player.PenaltyPoints);
            players_rankeds.Add((player, score));
        }

        return players_rankeds.OrderByDescending(x => x.score).Select(x => x.player).ToList();
    }

    /// <summary>Trouve le champion (joueur avec le meilleur score)</summary>
    public Player? GetChampion(List<Player> players)
    {
        var rankedPlayers = GetRanking(players);
        if (rankedPlayers.Count == 0)
        {
            return null;
        }
        return rankedPlayers.First();
    }
}