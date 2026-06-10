namespace TournoiEscrime.Core;

public class ScoreCalculator
{
    /// <summary>
    /// Calcule le score final d'un joueur selon les règles du tournoi
    /// </summary>
    /// <param name="matches">Liste des résultats de combat dans l'ordrechronologique</param>
    /// <param name="isDisqualified">True si le joueur estdisqualifié</param>
    /// <param name="penaltyPoints">Points de pénalité (nombrepositif)</param>
    /// <returns>Score final (jamais négatif)</returns>
    public int CalculateScore(List<MatchResult> matches, bool
        isDisqualified = false, int penaltyPoints = 0)
    {
        // TODO: À implémenter selon les règles
        throw new NotImplementedException();
    }
}

