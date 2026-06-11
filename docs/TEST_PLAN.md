# Plan de test — ScoreCalculator

## 1. Identification

| Champ | Valeur |
|---|---|
| Nom | Plan de test - ScoreCalculator |
| Version | 1.1 |
| Projet | Tournoi d'Escrime Fantastique |
| Équipe | Backend |
| Date | 2026-06-11 |
| Statut | Validé |

---

## 2. Périmètre

**IN SCOPE :**
- `ScoreCalculator.CalculateScore(matches, isDisqualified, penaltyPoints)`
- Règles de points de base (victoire/nul/défaite)
- Bonus de série de victoires consécutives
- Disqualification
- Pénalités
- Gestion des cas limites et exceptions

**OUT OF SCOPE :**
- `TournamentRanking.GetRanking()` / `GetChampion()` — couvert par un plan de test séparé
- API REST, persistance en base de données, application web — phases ultérieures

---

## 3. Stratégie

- **Framework** : xUnit (`[Fact]` / `[Theory]`)
- **Assertions** : FluentAssertions, avec message explicite (`because ...`)
- **Structure** : AAA (Arrange / Act / Assert), un seul concept par test
- **Nommage** : `Should_<Résultat>_When_<Condition>()`
- **Isolation** : chaque test crée sa propre instance de `ScoreCalculator` (constructeur de la classe de test), aucune dépendance partagée
- **Traçabilité** : chaque test est annoté avec `[Trait("Requirement", "REQ-XXX")]`
- **Tests d'intégration** : non requis (méthode pure, sans I/O)
- **Tests de performance** : 10 000 matchs traités en moins de 100 ms (indicatif)

Couverture de code cible : **≥ 95 %** sur `ScoreCalculator`.

---

## 4. Critères d'entrée

- Spécifications métier validées (voir §5)
- `MatchResult.cs` implémenté
- Squelette `ScoreCalculator.cs` en place dans `TournoiEscrime.Core`
- Projet de tests configuré (xUnit + FluentAssertions)

---

## 5. Règles métier à valider

| # | Règle | Détail |
|---|-------|--------|
| R1 | Points de base | Win = +3, Draw = +1, Loss = +0 |
| R2 | Bonus de série | +5 points dès qu'une série de **3 victoires consécutives** est atteinte |
| R3 | Bonus unique par série | Une série de 4+ victoires consécutives ne donne le bonus qu'**une seule fois** |
| R4 | Séries multiples | Deux séries de ≥3 victoires séparées par un Draw/Loss donnent **deux bonus distincts** |
| R5 | Disqualification | Le score final est **0**, peu importe les combats et les pénalités |
| R6 | Pénalités | Soustraites du score, mais le résultat final ne peut jamais être **négatif** (min = 0) |
| R7 | Liste null | Lève une `ArgumentNullException` (paramètre `matches`) |
| R8 | Pénalités négatives | Lève une `ArgumentException` (paramètre `penaltyPoints`) |

---

## 6. Note sur les divergences du sujet original

Le sujet PDF original contient deux incohérences, corrigées dans ce plan en s'appuyant sur le document de référence pédagogique (Code&Passion) :

1. **Pattern `Win, Win, Loss, Win`** : le sujet annonce 6 points (item 7), ce qui est mathématiquement faux (3+3+0+3 = **9**). Le document officiel (TC-SCORE-003) confirme **9**. → **Valeur retenue : 9**.

2. **Pattern `Win×3, Loss, Win×4`** : le sujet annonce 26 points (item 8 — un seul bonus), tandis que l'Exemple 3 et le document officiel (TC-SCORE-004) donnent **31** (deux bonus, un par série de ≥3 victoires, conformément à R4). → **Valeur retenue : 31**.

Ces divergences sont également documentées en section Risques (§9).

---

## 7. Cas de test

### 7.1 Tests de base (fonctionnement normal)

| ID | Titre | Entrée (`matches`) | Disqualifié | Pénalités | Résultat attendu | Exigence(s) |
|----|-------|---------------------|:---:|:---:|:---:|---|
| TC01 | Calcul simple sans bonus | Win, Draw, Loss | non | 0 | 4 | REQ-001, REQ-002, REQ-003 |
| TC02 | Que des nuls | Draw, Draw, Draw | non | 0 | 3 | REQ-002 |
| TC03 | Que des défaites | Loss, Loss | non | 0 | 0 | REQ-003 |
| TC04 | Victoires sans atteindre la série | Win, Win | non | 0 | 6 | REQ-001 |

### 7.2 Tests du bonus de série

| ID | Titre | Entrée | Résultat attendu | Exigence(s) |
|----|-------|--------|:---:|---|
| TC05 | Bonus exact pour 3 victoires consécutives | Win, Win, Win, Draw | 15 (9+1+5) | REQ-001, REQ-002, REQ-004 |
| TC06 | Bonus accordé une seule fois pour 4 victoires consécutives | Win, Win, Win, Win | 17 (12+5) | REQ-004, REQ-005 |
| TC07 | Pas de bonus si la série est interrompue | Win, Win, Loss, Win | 9 (3+3+0+3) | REQ-004 *(corrige le sujet original : 9, pas 6)* |
| TC08 | Bonus pour chaque série distincte de 3+ | Win×3, Loss, Win×4 | 31 (21+5+5) | REQ-005 *(confirme Exemple 3 / TC-SCORE-004 : 31, pas 26)* |
| TC09 | Pas de bonus si aucune série de 3 | Win, Draw, Win, Win | 10 | REQ-004 |
| TC10 | Bonus uniquement sur la série finale après une coupure | Win, Loss, Win, Win, Win | 17 (12+5) | REQ-004, REQ-005 |

### 7.3 Tests de disqualification

| ID | Titre | Entrée | Disqualifié | Résultat attendu | Exigence(s) |
|----|-------|--------|:---:|:---:|---|
| TC11 | Score remis à zéro malgré un score positif | Win, Win, Win | oui | 0 | REQ-006 |
| TC12 | Disqualifié sans aucun combat | (vide) | oui | 0 | REQ-006 |
| TC13 | Disqualification prioritaire même avec pénalités | Win, Win, Win | oui (pénalités=5) | 0 | REQ-006, REQ-007 |

### 7.4 Tests des pénalités

| ID | Titre | Entrée | Pénalités | Résultat attendu | Exigence(s) |
|----|-------|--------|:---:|:---:|---|
| TC14 | Pénalité normale soustraite du score | Win, Win, Draw, Win (10 pts) | 3 | 7 | REQ-007 |
| TC15 | Pénalité supérieure au score → 0 | Win, Draw, Draw (5 pts) | 8 | 0 | REQ-007 |
| TC16 | Pénalité égale au score → 0 | Win, Win, Draw (7 pts) | 7 | 0 | REQ-007 |

### 7.5 Tests des cas limites et exceptions

| ID | Titre | Entrée | Résultat attendu | Exigence(s) |
|----|-------|--------|:---:|---|
| TC17 | Liste vide → score nul | (liste vide) | 0 | REQ-001 à REQ-003 |
| TC18 | Liste null lève une exception | null | `ArgumentNullException` (paramètre `matches`, message contenant "cannot be null") | REQ-008 |
| TC19 | Pénalité négative refusée | Win, pénalités=-5 | `ArgumentException` (paramètre `penaltyPoints`, message indiquant une valeur négative) | REQ-008 |
| TC20 | Tournoi long avec motif répétitif | 25× (Win,Win,Win,Loss) = 100 combats | 350 (25 × (9+5)) | REQ-001 à REQ-005 |

### 7.6 Tests paramétrés

| ID | Titre | Type | Détail | Exigence(s) |
|----|-------|------|--------|---|
| TC21 | Calcul de score selon différents résultats | `[Theory]` + `InlineData` | (3,0,0)→14, (2,1,0)→7, (0,0,3)→0 | REQ-001 à REQ-004 |
| TC22 | Scénarios complexes (séries multiples / interrompues) | `[Theory]` + `MemberData` | Voir §8 — clarification requise avant écriture | REQ-004, REQ-005 |

---

## 8. Cas TC22 — clarification requise avant implémentation

Le cas de **6 victoires consécutives** est ambigu dans le sujet original :

- **Interprétation A (1 seul bonus)** : `6 + 5 = 23` → la série entière de 6 ne donne qu'un bonus, comme une série de 4 (R3 généralisée à toute longueur ≥3)
- **Interprétation B (2 bonus)** : `6 + 5 + 5 = 28` → la série de 6 est découpée en groupes de 3 consécutifs

**→ Action requise** : organiser un atelier avec le PO pour trancher (voir Risque 1, §9). Les exemples détaillés (Exemple 3, R4, TC-SCORE-004) suggèrent l'**interprétation B** (séries *séparées* par un Draw/Loss = bonus distincts), mais ne couvrent pas le cas d'une **série continue** de 6+.

Cas concrets suggérés pour TC22 (une fois la règle clarifiée) :
```
Draw, Win, Win, Win, Draw, Win, Win, Win → 25 (1+9+1+9+5+5)
Win, Win, Loss, Loss, Win, Win, Win      → 20 (6+0+0+9+5)
Win×6                                     → 23 ou 28 selon interprétation retenue
```

---

## 9. Risques

| ID | Risque | Stratégie de mitigation |
|---|---|---|
| Risque 1 | Règle ambiguë sur les séries continues de 6+ victoires (TC22) | Atelier avec le PO avant d'écrire TC22 |
| Risque 2 | Divergence entre le sujet original et les valeurs attendues (TC07 : 6 vs 9 ; TC08 : 26 vs 31) | Retenir les valeurs du document de référence officiel (Code&Passion), documentées en §6 ; signaler au correcteur si besoin |
| Risque 3 | Confusion entre "score brut" et "score après pénalités/disqualification" dans le code | Implémenter dans un ordre fixe : (1) somme + bonus, (2) disqualification, (3) pénalités, (4) clamp à 0 |

---

## 10. Critères de sortie

- 100 % des cas de test (TC01–TC22) passent
- Couverture de code ≥ 95 % sur `ScoreCalculator`
- Zéro bug bloquant ou critique
- Matrice de traçabilité (§11) entièrement couverte

---

## 11. Matrice de traçabilité

| Exigence | Description | Cas de test associés | Statut |
|---|---|---|:---:|
| REQ-001 | Une victoire vaut +3 points | TC01, TC04, TC05, TC21 | ✓ Couverte |
| REQ-002 | Un match nul vaut +1 point | TC01, TC02, TC05 | ✓ Couverte |
| REQ-003 | Une défaite vaut 0 point | TC01, TC03 | ✓ Couverte |
| REQ-004 | +5 points de bonus pour 3 victoires consécutives | TC05, TC06, TC07, TC09, TC10, TC21, TC22 | ✓ Couverte |
| REQ-005 | Le bonus est attribué pour chaque série distincte de 3+ | TC06, TC08, TC10, TC22 | ✓ Couverte |
| REQ-006 | Disqualification met le score à 0 | TC11, TC12, TC13 | ✓ Couverte |
| REQ-007 | Pénalités soustraites, jamais de score négatif | TC13, TC14, TC15, TC16 | ✓ Couverte |
| REQ-008 | Entrées invalides lèvent une exception | TC18, TC19 | ✓ Couverte |

---

## 12. Environnements

- .NET 8 SDK
- xUnit 2.6.1, xunit.runner.visualstudio 2.5.3
- FluentAssertions 6.12.0
- Microsoft.NET.Test.Sdk 17.8.0
- coverlet.collector (mesure de couverture)

---

## 13. Pattern de traçabilité dans le code

Chaque test doit être annoté avec les exigences couvertes :

```csharp
[Fact]
[Trait("Requirement", "REQ-001")]
[Trait("Requirement", "REQ-002")]
[Trait("Requirement", "REQ-003")]
public void Should_Calculate_Score_Without_Bonus_When_No_Streak()
{
    // Arrange
    var calculator = new ScoreCalculator();
    var matches = new List<MatchResult>
    {
        new(MatchResult.Result.Win),
        new(MatchResult.Result.Draw),
        new(MatchResult.Result.Loss)
    };

    // Act
    var score = calculator.CalculateScore(matches);

    // Assert
    score.Should().Be(4, "REQ-001 (Win=3), REQ-002 (Draw=1) et REQ-003 (Loss=0) appliqués");
}
```

Filtrage par exigence :
```bash
dotnet test --filter "Requirement=REQ-001"
```

---

## 14. Responsabilités

| Rôle | Responsabilité |
|---|---|
| Dev | Implémentation de `ScoreCalculator.cs` + écriture des tests unitaires |
| QA | Revue de la matrice de traçabilité, validation de la couverture |
| PO | Validation des cas métier, arbitrage du Risque 1 (§9) |

---

## 15. Hors-périmètre (bonus, si temps disponible)

- `TournamentRanking.GetRanking()` / `GetChampion()` : classement par score décroissant, gestion des égalités, champion, joueurs disqualifiés — à traiter dans un plan de test séparé.
