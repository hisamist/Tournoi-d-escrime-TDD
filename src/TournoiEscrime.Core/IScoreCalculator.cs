namespace TournoiEscrime.Core;

public interface IScoreCalculator
{
    /// <summary>Calcule le score final d'un joueur</summary>
    int CalculateScore(Player player);
}
