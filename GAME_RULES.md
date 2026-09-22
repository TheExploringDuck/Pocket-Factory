# Pocket Factory — Economy & Progression Rules

> **Core economy balance v1 — finalized 2026-09-22.**
>
> The values in the core progression sections below are the approved implementation targets. Coding agents should implement these values as written and must not silently rebalance them. Future changes should be explicit balance revisions based on play-test or telemetry evidence.
>
> Real-money pricing, paid bundle contents, and any future Gem spending catalog are intentionally outside this core-economy freeze.

## Core progression

The first Material Era follows:

**Pickaxe → early automation → Drill → Production Lines → five standard factories → Giga-Factory → Industrial Alloy**

The economy must remain playable without advertisements or purchases.

## Balance goals and final pacing check

The finalized v1 values were checked against an expected-value progression model with continuous production, no paid boosts, no rewarded ads, and no prestige/reset bonus. The purpose is not to guarantee exact player times; it is to keep the opening fast, lengthen each factory meaningfully, and keep the Giga-Factory visibly reachable.

Approximate modeled milestones:

| Milestone | Approximate cumulative time |
|---|---:|
| Pickaxe Level 4 / Auto Miner | 1.25 min |
| Pickaxe Level 10 | 12.7 min |
| Drill Level 8 / Production Line 1 available | 32 min |
| Factory 1 complete | ~1.8 hr from new save |
| Factory 2 complete | ~4.6 hr |
| Factory 3 complete | ~9.3 hr |
| Factory 4 complete | ~16.1 hr |
| Factory 5 complete | ~26 hr |
| Giga-Factory reachable | ~35 hr |

These are idealized expected-value targets, not promises. Currency RNG, offline play, manual interaction, reset timing, and future balance telemetry will move actual player times. The no-reset model is deliberately conservative: the permanent reset-rate tables can shorten later progression.

The separate Metal unlock gates were also checked against completed-factory throughput. At full prior-factory output, they take approximately 9 minutes, 19 minutes, 58 minutes, 2.9 hours, and 5.3 hours respectively; Currency remains the primary late-game pacing gate rather than Metal becoming an unreachable wall.

## Timing terminology

All mining and production cycle times are **seconds per action/cycle**.

- actions/sec = `1 / cycle_time`
- metal/sec = `metal_per_action / cycle_time`
- standard Production Line cycle time = **1.00 second**

## Resource roles

- **Metal** pays for Pickaxe and Drill progression and participates in later factory/Giga-Factory unlocks.
- **Currency** pays for Supervisors, Production Lines, and factory expansion.
- **Gems** are permanent milestone currency and are not required for ordinary progression.
- **Industrial Alloy** begins at the Giga-Factory and is the first Material Era 2 resource.

## Pickaxe — Levels 1–10

| Level | Cycle time (sec) | Metal/action | Upgrade cost (Metal) | Normal Currency drop | Post-reset Currency rate |
|---:|---:|---:|---:|---:|---:|
| 1 | 0.70 | 1.00 | — | 0% | 2% |
| 2 | 0.65 | 1.90 | 25 | 0% | 4% |
| 3 | 0.60 | 2.70 | 70 | 0% | 6% |
| 4 | 0.55 | 3.40 | 150 | 0% | 8% |
| 5 | 0.50 | 4.00 | 300 | 0% | 10% |
| 6 | 0.45 | 4.50 | 600 | 0% | 12% |
| 7 | 0.40 | 4.90 | 1,100 | 0% | 14% |
| 8 | 0.35 | 5.20 | 1,800 | 0% | 16% |
| 9 | 0.35 | 5.85 | 3,000 | 0% | 18% |
| 10 | 0.35 | 6.50 | 4,800 | 0% | 20% |

Pickaxe metal formula remains:

`metal_per_action = level * cycle_time + (0.3 * level)`

Speed reaches its floor at Level 8; Levels 8–10 increase output rather than speed.

### Early automation — Auto Miner

Auto Miner is the first automation lesson and is distinct from the Drill.

- Unlocks automatically at **Pickaxe Level 4**.
- Performs one automated Pickaxe mining action every **1.50 seconds**.
- Uses the current Pickaxe `metal_per_action`.
- Generates **Metal only**; it does not independently roll Currency.
- Manual Pickaxe mining remains available.
- Pickaxe upgrades automatically improve Auto Miner output.
- Unlock state persists through ordinary saves. A prestige/reset follows the reset rules below.

This gives the player a visible automation payoff at roughly the first 1–2 minutes while preserving manual interaction.

## Drill — Levels 1–10

The Drill becomes purchasable after Pickaxe Level 10 and is the first major mechanized mining tier.

| Level | Cycle time (sec) | Metal/action | Purchase/upgrade cost (Metal) | Currency/drop | Normal Currency rate | Post-reset rate |
|---:|---:|---:|---:|---:|---:|---:|
| 1 | 0.53 | 10.275 | 2,500 | 3 | 2% | 20% |
| 2 | 0.50 | 20.50 | 3,500 | 6 | 4% | 22% |
| 3 | 0.45 | 30.60 | 6,000 | 9 | 6% | 25% |
| 4 | 0.43 | 40.72 | 10,000 | 12 | 8% | 29% |
| 5 | 0.40 | 50.75 | 16,000 | 15 | 10% | 34% |
| 6 | 0.35 | 60.60 | 25,000 | 18 | 12% | 40% |
| 7 | 0.30 | 70.35 | 40,000 | 21 | 14% | 47% |
| 8 | 0.25 | 80.00 | 65,000 | 24 | 16% | 55% |
| 9 | 0.25 | 90.00 | 110,000 | 27 | 18% | 64% |
| 10 | 0.25 | 100.00 | 180,000 | 30 | 20% | 74% |

- Currency/drop = `3 * Drill Level`.
- Normal Drill Currency chance = `2% * Drill Level`.
- Post-reset Levels 8–10 continue the increasing-delta curve at **55% / 64% / 74%**.
- Drill production and manual Pickaxe/Auto Miner activity may coexist.

## Production Line base levels

Production Line 1 becomes available at **Drill Level 8**.

The five production levels use the following base values:

| PL level | Supporting Drill | Base metal | Increase | Base output/cycle | Line-level cost factor | Post-reset Currency rate |
|---:|---:|---:|---:|---:|---:|---:|
| 1 | 8 | 80 | 0.5% | 112 | 1.0× | 50% |
| 2 | 8 | 80 | 1.0% | 144 | 0.8× | 52% |
| 3 | 9 | 90 | 3.0% | 333 | 1.2× | 54% |
| 4 | 9 | 90 | 4.0% | 414 | 1.7× | 56% |
| 5 | 10 | 100 | 5.0% | 600 | 2.4× | 58% |

The existing base output formula remains the source of the five base outputs:

`output = base + (base * increase * base)`

The values above are now frozen for v1. Scaling to later lines/factories happens through the explicit multipliers below rather than extrapolating this formula indefinitely.

## Production Line 1–10 scaling

### Line output multipliers

Apply the line multiplier to the base output for the current Production Line level:

| Line | Output multiplier |
|---:|---:|
| 1 | 1.00× |
| 2 | 1.20× |
| 3 | 1.50× |
| 4 | 1.90× |
| 5 | 2.40× |
| 6 | 3.00× |
| 7 | 3.70× |
| 8 | 4.50× |
| 9 | 5.40× |
| 10 | 6.40× |

Per-cycle Metal output is:

`base_level_output * line_output_multiplier * factory_metal_multiplier`

Each Production Line completes one cycle per **1.00 second**.

### First-run Currency chances

Line 1 remains:

| Level | Currency chance |
|---:|---:|
| 1 | 16.5% |
| 2 | 17% |
| 3 | 19% |
| 4 | 22% |
| 5 | 25% |

Line 2 remains:

| Level | Currency chance |
|---:|---:|
| 1 | 22% |
| 2 | 24% |
| 3 | 26% |
| 4 | 28% |
| 5 | 30% |

For Lines 3–10, first-run chance is finalized as:

`22% + (2% * (line_number - 1)) + (2% * (line_level - 1))`

This yields:

| Line | L1 | L2 | L3 | L4 | L5 |
|---:|---:|---:|---:|---:|---:|
| 3 | 26% | 28% | 30% | 32% | 34% |
| 4 | 28% | 30% | 32% | 34% | 36% |
| 5 | 30% | 32% | 34% | 36% | 38% |
| 6 | 32% | 34% | 36% | 38% | 40% |
| 7 | 34% | 36% | 38% | 40% | 42% |
| 8 | 36% | 38% | 40% | 42% | 44% |
| 9 | 38% | 40% | 42% | 44% | 46% |
| 10 | 40% | 42% | 44% | 46% | 48% |

After the permanent reset-rate unlock, all lines use the level-based reset table **50% / 52% / 54% / 56% / 58%**. First-run and reset chances never stack.

### Currency amount per successful Production Line roll

A successful Production Line Currency roll awards:

`line_number * line_level * factory_currency_amount_multiplier`

Currency chance is never multiplied by factory tier. Later factories increase the **amount** earned on a successful drop, preventing probability from racing toward 100%.

## Production Line costs

All Production Line costs use Currency.

Base Level-1 line costs in Factory 1:

| Line | Base cost |
|---:|---:|
| 1 | 250 |
| 2 | 450 |
| 3 | 800 |
| 4 | 1,250 |
| 5 | 1,900 |
| 6 | 2,800 |
| 7 | 4,000 |
| 8 | 5,600 |
| 9 | 7,600 |
| 10 | 10,000 |

For each line:

- Level 1 / construction cost = `base_cost * 1.0`
- Level 2 upgrade = `base_cost * 0.8`
- Level 3 upgrade = `base_cost * 1.2`
- Level 4 upgrade = `base_cost * 1.7`
- Level 5 upgrade = `base_cost * 2.4`

Then multiply the result by the active Factory cost multiplier.

Factory 1 total Production Line spend to take all ten lines from unbuilt through Level 5 is **246,015 Currency** before Supervisors.

## Supervisors and production-line capacity

Line 1 is the exception and requires no supervisor.

| Lines available | Supervisors required |
|---:|---:|
| 1 | 0 |
| 2 | 1 |
| 3 | 1 |
| 4 | 2 |
| 5 | 2 |
| 6 | 3 |
| 7 | 3 |
| 8 | 4 |
| 9 | 4 |
| 10 | 5 |

Therefore:

- Supervisor #1 gates Line 2.
- Line 3 requires Lines 1–2 and Supervisor #1.
- Supervisor #2 gates Line 4.
- Supervisor #3 gates Line 6.
- Supervisor #4 gates Line 8.
- Supervisor #5 gates Line 10.

A completed standard factory contains **10 Level-5 Production Lines and 5 Supervisors**.

Base Supervisor costs in Factory 1:

| Supervisor | Base Currency cost |
|---:|---:|
| 1 | 2,000 |
| 2 | 5,000 |
| 3 | 12,000 |
| 4 | 25,000 |
| 5 | 50,000 |

Supervisor costs are multiplied by the active Factory cost multiplier.

Factory 1 total Supervisor spend is **94,000 Currency**. Combined with the ten fully upgraded Production Lines, the base full-factory build spend is **340,015 Currency**.

## Reset / prestige behavior

The first reset becomes available when **Production Line 3 reaches Level 5** in the current run.

On reset, the active unfinished run:

- returns Pickaxe to Level 1;
- removes the Drill;
- resets Metal to 0;
- removes Supervisors and Production Lines in the active unfinished factory.

The player keeps:

- all Currency;
- all Gems;
- completed factories and their ongoing production;
- already unlocked factory/material-era access;
- story/tutorial completion;
- the permanent reset-rate flag.

The first reset permanently activates the reset-rate tables. Additional resets do **not** increase the rates again; they are optional tactical reruns using the already-unlocked reset rates.

Normal and reset Currency rates never stack.

## Five-factory structure — finalized Material Era 1

Each standard factory has 10 Level-5 Production Lines and 5 Supervisors. Completed factories remain online when the player advances.

| Factory | Build-cost multiplier | Metal-output multiplier | Currency-amount multiplier | Unlock Metal | Unlock Currency | Completion Gems |
|---:|---:|---:|---:|---:|---:|---:|
| 1 | 1× | 1× | 1× | — | — | 5 |
| 2 | 6× | 6× | 3× | 10,000,000 | 500,000 | 10 |
| 3 | 30× | 24× | 8× | 150,000,000 | 3,000,000 | 20 |
| 4 | 100× | 72× | 18× | 2,000,000,000 | 15,000,000 | 35 |
| 5 | 300× | 180× | 36× | 20,000,000,000 | 60,000,000 | 50 |

Rules:

- Build-cost multiplier applies to Production Line and Supervisor costs.
- Metal-output multiplier applies to Production Line Metal throughput.
- Currency-amount multiplier applies only to the amount awarded on a successful Production Line Currency roll.
- Currency probability is determined by the line/level table and never multiplied by Factory tier.
- Unlock resources are spent when opening the next Factory.
- A Factory is complete only when all ten Production Lines are Level 5 and all five Supervisors are owned.

The multipliers intentionally grow more slowly than a flat 10× exponential chain. Costs outpace reward multipliers enough to lengthen each Factory, while completed factories continue producing so later goals remain visible and reachable.

## Giga-Factory — finalized Material Era 1 transition

The Giga-Factory becomes constructible after Factory 5 is complete.

Construction requires:

- **100,000,000,000 Metal**
- **250,000,000 Currency**

Building it:

- does not destroy or disable the five completed factories;
- keeps Currency and Gems;
- keeps the earlier factories economically useful;
- unlocks **Industrial Alloy** as the first Material Era 2 resource.

Base Giga-Factory conversion:

**10,000 Metal → 1 Industrial Alloy**

Base conversion cycle: **1.00 second**.

This makes Metal a continuing supply-chain input instead of obsolete currency. Material Era 2 costs and Giga-Factory upgrades will be balanced as a separate economy layer and must not silently alter the finalized Material Era 1 constants above.

## Gems — finalized earning rules for Material Era 1

Gems are permanent and are not required for core progression.

For Material Era 1:

- ordinary Pickaxe, Drill, and Production Line actions have **no random Gem drop**;
- Factory completion awards the fixed Gem amounts in the five-factory table;
- activating the Giga-Factory awards **100 Gems**;
- Gems survive resets and factory transitions.

The Gem spending catalog is a future quality-of-life/cosmetic/permanent-upgrade system and is intentionally not part of the Material Era 1 core-economy freeze.

## Advertising / monetization boundary

Pocket Factory may use optional monetization, but monetization must complement the finalized baseline economy rather than become the reason it works.

Current implementation may continue to use the existing **2× production for 5 minutes** rewarded-ad test value. Any additional rewarded-ad magnitudes, IAP quantities, real-money prices, ad-free pricing, or paid bundles must remain configurable until real progression testing exists.

Rules:

- The full game must remain playable without purchases or ads.
- Ads remain optional.
- No randomized paid rewards or loot boxes.
- Do not create artificial waits solely to pressure a purchase or ad view.
- Purchase/ad rewards must be idempotent and safe against duplicate callbacks.
- Monetization code stays abstracted from the core simulation.

## Balance freeze

The following Material Era 1 core values are now **approved/finalized for implementation**:

1. Pickaxe output, speed, and upgrade costs.
2. Auto Miner Level-4 unlock and 1.50-second cycle.
3. Drill output, upgrade costs, Currency amounts/chances, and all reset rates.
4. Production Line output multipliers, cycle time, Currency curves, and costs.
5. Supervisor gates and costs.
6. Reset trigger and wipe/retain behavior.
7. Factory 1–5 cost/output/Currency multipliers and unlock costs.
8. Factory completion Gem rewards.
9. Giga-Factory construction costs and Industrial Alloy conversion.
10. Material Era 1 Gem earning rules.

Coding agents should treat these as constants/data, add tests around them, and not substitute new values without an explicit balance revision.

Still intentionally open because they are outside the Material Era 1 core-economy freeze:

- Material Era 2 progression after the initial Industrial Alloy conversion.
- Gem spending catalog.
- real-money pricing and bundle contents.
- additional ad reward types/durations beyond the current 2×/5-minute test.
- live-service event/competition rewards.

**Balance principle:** fast early feedback, progressively longer factory goals, and a Giga-Factory target that requires effort without depending on ads or purchases.
