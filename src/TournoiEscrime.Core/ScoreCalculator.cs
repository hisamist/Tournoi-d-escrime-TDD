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
        if (matches == null)
        {
            throw new ArgumentNullException(nameof(matches), "Matches result cannot be null");
        }

        if (isDisqualified)
        {
            return 0;
        }

        if (penaltyPoints < 0)
        {
            throw new ArgumentException("Penalty points cannot be negative", nameof(penaltyPoints));
        }

        var points 
        = 0;
        var consecutiveWins = 0;

        foreach (var match in matches)
        {
            if (match.Outcome == MatchResult.Result.Win)
            {
                points += 3;
                consecutiveWins++;
                if (consecutiveWins == 3)
                {
                    points += 5;
                    consecutiveWins = 0;
                }
            }
            else if (match.Outcome == MatchResult.Result.Draw)
            {
                points += 1;
                consecutiveWins = 0;
            }
            else if (match.Outcome == MatchResult.Result.Loss)
            {
                consecutiveWins = 0;
            }
        }



        return points - penaltyPoints < 0 ? 0 : points - penaltyPoints;
    }
}

