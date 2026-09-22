# Pocket Factory — Monthly Competition System

> Working competition specification. Competition balance values should remain configurable until real-player testing validates retention, fairness, and scoring behavior.

## Competition unlock

Competition becomes available when the player constructs **Production Line 1** in the main game.

At that milestone, show a one-time unlock message and expose the competition entry point. Do not surface the competitive system earlier; the player should first understand manual mining, automation, Drill progression, and the first production line.

## Competition structure

Pocket Factory uses a recurring **monthly ladder** built from **four weekly competition rounds**.

- Each tier contains **40 players**.
- There are **5 tiers**, supporting **200 players per complete ladder instance**.
- Players compete in a separate competition mine/factory.
- Main-game Metal, Currency, factory upgrades, and purchased progression do not transfer into competition.
- Competition progression resets for each weekly round.
- Weekly performance awards weekly rewards plus Monthly Ladder Points.
- Promotion, retention, and relegation occur after the four-week monthly cycle.
- Future seasonal competitions may sit above this system without replacing the monthly ladder.

If concurrency exceeds 200 players, create additional independent 200-player ladder instances rather than increasing individual leaderboard size.

## Five tiers and monthly movement

| Tier | Players | Promote | Stay | Relegate |
|---|---:|---:|---:|---:|
| 1 — Prospector | 40 | Top 10 | Remaining 30* | — |
| 2 — Excavator | 40 | Top 10 | Middle 20 | Bottom 10 |
| 3 — Industrial | 40 | Top 10 | Middle 20 | Bottom 10 |
| 4 — Magnate | 40 | Top 10 | Middle 20 | Bottom 10 |
| 5 — Titan | 40 | — | Top 30 | Bottom 10 |

\*Tier 1 receives the 10 relegated players from Tier 2, replacing its 10 promoted players. New-player placement must use a separate intake/overflow rule so it does not break the 40-player tier cap.

### Population conservation validation

For a full 200-player ladder:

- Tier 1: 40 - 10 promoted + 10 relegated from Tier 2 = **40**.
- Tier 2: 40 - 10 promoted - 10 relegated + 10 from Tier 1 + 10 from Tier 3 = **40**.
- Tier 3: 40 - 10 promoted - 10 relegated + 10 from Tier 2 + 10 from Tier 4 = **40**.
- Tier 4: 40 - 10 promoted - 10 relegated + 10 from Tier 3 + 10 from Tier 5 = **40**.
- Tier 5: 40 - 10 relegated + 10 promoted from Tier 4 = **40**.

Total after movement remains **200 players**. Movement is therefore population-neutral for a closed, fully populated ladder.

## Weekly competition resources and difficulty

Each weekly round uses a competition-exclusive resource. The resource resets when that week's competition closes and cannot be transferred into the main economy.

Difficulty should rise modestly each week through several small changes rather than one punitive modifier.

| Week | Resource | Base drop chance | Extraction difficulty | Processing cost | Approx. relative difficulty | Reward-value target |
|---|---|---:|---:|---:|---:|---:|
| 1 | Copper Crystal | 10.0% | 1.00x | 1.00x | 1.00x | 1.00x |
| 2 | Cobalt Crystal | 9.5% | 1.08x | 1.05x | ~1.13x | 1.20x |
| 3 | Obsidian | 9.0% | 1.15x | 1.10x | ~1.28x | 1.50x |
| 4 | Star Ore | 8.5% | 1.22x | 1.15x | ~1.44x | 2.00x |

These resource names are working names and may change with final art direction.

The intended curve makes Week 4 roughly 44% harder than Week 1 while targeting roughly double the reward value. Difficulty values are provisional and must be validated against actual completion/progression data.

## Weekly scoring

Competition is based on both **special-resource collection** and **production efficiency**.

Recommended starting weighting:

`weekly_score = (normalized_resource_score * 0.60) + (normalized_efficiency_score * 0.40)`

Where both normalized components are scaled consistently within the player's competition group before weighting.

### Resource score

Measures total weekly competition-exclusive resource successfully collected.

Example normalization:

`normalized_resource_score = player_resource / highest_group_resource`

### Efficiency score

Measures how effectively the player converts competition inputs and upgrades into useful production. The exact production-efficiency formula is **TBD** and must be resistant to trivial exploits such as deliberately avoiding upgrades merely to preserve a ratio.

Do not implement a final efficiency formula until upgrade costs and competition production curves are available for simulation.

## Monthly Ladder Points

Later weeks are harder and therefore award moderately more ladder points. This also allows a player who performs poorly early in the month to recover without making Week 1 meaningless.

| Finish | Week 1 | Week 2 | Week 3 | Week 4 |
|---|---:|---:|---:|---:|
| 1st | 100 | 110 | 125 | 150 |
| 2nd | 85 | 94 | 106 | 128 |
| 3rd | 75 | 83 | 94 | 113 |
| Top 10 (4th–10th) | 50 | 55 | 63 | 75 |
| Participation | 10 | 11 | 13 | 15 |

Implementation must define deterministic tie-breaking before production release. Suggested inputs for later validation include total special resource, efficiency score, and earliest time reaching the final score; no final tie-break order is approved yet.

## Weekly and monthly rewards

- Every weekly round awards a reward based on weekly placement.
- Reward value scales upward with weekly difficulty.
- Monthly placement determines tier promotion/relegation and a larger monthly reward.
- Higher tiers should provide meaningfully better rewards without making competition mandatory for ordinary main-game progression.
- Competition rewards may accelerate the main game but must not gate normal progression.
- Cosmetic badges, trophies, titles, and highest-tier history can persist permanently.

Exact resource quantities and monetary values remain **TBD** until the main economy is balanced.

## Fairness and monetization rules

- Players in the same competition tier begin each weekly competition from an equivalent competition state.
- Main-game account age must not determine competition output.
- Main-game purchased resources do not transfer into competition.
- Competition must not become direct pay-to-win.
- There are **no forced ads** in competition.
- Any rewarded-ad competition benefit must be voluntary, clearly disclosed, limited/configurable, and included in balance testing.
- Rewarded ads must not create an uncapped leaderboard advantage; equivalent ordinary-play access or a strict shared cap is required.
- Competition-specific purchases, if ever considered, require separate fairness review before implementation and must not create direct pay-to-win progression.
- Server-authoritative or otherwise tamper-resistant leaderboard validation will be required before public competitive rewards have meaningful value.

## Monthly lifecycle

1. Month begins and players are assigned to a 40-player tier group.
2. Week 1 competition runs with a fresh competition state and Week 1 resource.
3. Week 1 closes; score locks; weekly rewards and Ladder Points are recorded.
4. Weeks 2–4 repeat with fresh competition states and progressively harder resources.
5. Week 4 closes and monthly Ladder Points are totaled.
6. Ties are resolved using the approved deterministic tie-break rules.
7. Monthly rewards are distributed.
8. Top/bottom movement is applied according to the five-tier table.
9. The next monthly ladder begins.

## Validation checklist

Before production implementation, verify:

- [x] Five tiers x 40 players = **200 players per full ladder instance**.
- [x] Promotion/relegation math preserves 40 players in every tier for a closed full ladder.
- [x] Interior tiers exchange equal numbers upward and downward.
- [x] Tier 1 and Tier 5 boundary movement is balanced by the adjacent tier.
- [x] Weekly competition state is isolated from permanent main-game progression.
- [x] Difficulty increases progressively rather than spiking in one week.
- [x] Reward targets increase faster than difficulty targets.
- [x] Weekly scoring includes both collection and efficiency.
- [ ] Define exact efficiency formula and simulate exploit cases.
- [ ] Define deterministic leaderboard tie-breaking.
- [ ] Define new-player intake and partially filled ladder behavior.
- [ ] Define disconnect/offline submission and anti-cheat behavior.
- [ ] Validate weekly difficulty against real progression-time simulations.
- [ ] Validate rewarded-ad limits, if competition ads are enabled.
- [ ] Load-test leaderboard grouping beyond one 200-player ladder instance.

## Design intent

The competition system exists primarily to improve long-term retention by giving players recurring goals, social comparison, optimization challenges, and meaningful reasons to return. It should complement the core idle-factory experience rather than replace it.
