# Pocket Factory — Economy & Progression Rules

> Working design specification for implementation and balancing. Values marked **TBD** or **provisional** must not be invented by coding agents; keep them data-driven until explicitly approved.

## Core progression

The player progresses from manual mining into mechanized mining and then factory production:

**Pickaxe → Drill → Production Lines → Factory completion → later Giga-Factory / material eras**

The economy should remain playable without advertisements or purchases.

## Timing terminology

All mining `cycle_time` values below are **seconds per action**, not actions per second.

- actions/sec = `1 / cycle_time`
- metal/sec = `metal_per_action / cycle_time`

## Pickaxe — Levels 1–10

| Level | Cycle time (sec) | Metal/action | Normal Currency drop | Post-reset Currency rate |
|---:|---:|---:|---:|---:|
| 1 | 0.70 | 1.00 | 0% | 2% |
| 2 | 0.65 | 1.90 | 0% | 4% |
| 3 | 0.60 | 2.70 | 0% | 6% |
| 4 | 0.55 | 3.40 | 0% | 8% |
| 5 | 0.50 | 4.00 | 0% | 10% |
| 6 | 0.45 | 4.50 | 0% | 12% |
| 7 | 0.40 | 4.90 | 0% | 14% |
| 8 | 0.35 | 5.20 | 0% | 16% |
| 9 | 0.35 | 5.85 | 0% | 18% |
| 10 | 0.35 | 6.50 | 0% | 20% |

Current Pickaxe metal formula:

`metal_per_action = level * cycle_time + (0.3 * level)`

Speed reaches its current floor at Level 8; Levels 8–10 increase output rather than speed.

## Drill — Levels 1–10

The Drill unlocks after Pickaxe Level 10 and is the first mechanized mining tier.

| Level | Cycle time (sec) | Metal/action | Currency/drop | Normal Currency rate | Post-reset rate |
|---:|---:|---:|---:|---:|---:|
| 1 | 0.53 | 10.275 | 3 | 2% | 20% |
| 2 | 0.50 | 20.50 | 6 | 4% | 22% |
| 3 | 0.45 | 30.60 | 9 | 6% | 25% |
| 4 | 0.43 | 40.72 | 12 | 8% | 29% |
| 5 | 0.40 | 50.75 | 15 | 10% | 34% |
| 6 | 0.35 | 60.60 | 18 | 12% | 40% |
| 7 | 0.30 | 70.35 | 21 | 14% | 47% |
| 8 | 0.25 | 80.00 | 24 | 16% | TBD |
| 9 | 0.25 | 90.00 | 27 | 18% | TBD |
| 10 | 0.25 | 100.00 | 30 | 20% | TBD |

Currency/drop = `3 * Drill Level`.

Normal Drill Currency chance = `2% * Drill Level`.

Do not invent Drill Levels 8–10 post-reset rates until they are approved.

## Production Line 1

Production Line 1 unlocks at **Drill Level 8**.

It can exist without a supervisor.

| PL level | Supporting Drill | Base metal | Increase | Output | First-run Currency rate | Post-reset rate |
|---:|---:|---:|---:|---:|---:|---:|
| 1 | 8 | 80 | 0.5% | 112 | 16.5% | 50% |
| 2 | 8 | 80 | 1.0% | 144 | 17% | 52% |
| 3 | 9 | 90 | 3.0% | 333 | 19% | 54% |
| 4 | 9 | 90 | 4.0% | 414 | 22% | 56% |
| 5 | 10 | 100 | 5.0% | 600 | 25% | 58% |

Current output formula:

`output = base + (base * increase * base)`

This formula is intentionally aggressive for the current factory. Do not extrapolate it indefinitely without balance simulation.

## Production Line 2

Production Line 2 uses the same basic five-level production structure as Line 1.

A player **must own Supervisor #1 before Production Line 2 can be built**.

First-run Currency rates by Line 2 level:

| Level | Currency rate |
|---:|---:|
| 1 | 22% |
| 2 | 24% |
| 3 | 26% |
| 4 | 28% |
| 5 | 30% |

After the first reset, use the Production Line reset-rate table (currently 50%, 52%, 54%, 56%, 58%) instead of stacking the first-run Line 2 rates onto reset rates.

## Supervisors and production-line capacity

Line 1 is the exception: it requires no supervisor.

After that, each supervisor supports a two-line capacity step.

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
- Line 3 requires the existing first two lines and Supervisor #1; it does not require Supervisor #2.
- Supervisor #2 gates Line 4.
- Supervisor #3 gates Line 6.
- Supervisor #4 gates Line 8.
- Supervisor #5 gates Line 10.

A completed standard factory contains **10 production lines and 5 supervisors**.

## Reset / prestige behavior

Lines 3–4 are intended to create the first meaningful slowdown and make resetting strategically attractive.

The **first reset**:

1. Restarts run progression.
2. Keeps accumulated Currency.
3. Permanently activates the Reset Rate Currency tables.

The reset-rate upgrade happens **once only**. Second and later resets do not increase Currency probabilities further.

Every later reset still retains Currency.

Normal and reset Currency rates do not stack. Use the active table appropriate to the player's permanent reset state.

The exact reset unlock trigger and complete wipe/retain list remain **TBD**.

## Five-factory structure

The current target is **five standard factories** in the first material era.

Each standard factory follows the 10-production-line / 5-supervisor organizational structure.

Factory 2–5 economy scaling is not yet final. Current balance-model placeholders are:

- Metal throughput: approximately **x10 per factory tier** — provisional.
- Physical Currency amount: approximately **x5 per factory tier** — provisional.
- Do **not** multiply Currency probability by the factory tier; probability should not simply race toward 100%.

These values must be validated through time-to-upgrade and reset simulations before becoming final constants.

## Beyond Factory 5 — Giga-Factory and materials

Five factories constitute the planned first **Material Era**.

Completing Factory 5 should eventually unlock a **Giga-Factory** transition rather than merely continuing to Factory 6 with larger Metal numbers.

The Giga-Factory is intended to consolidate/advance the existing production system and unlock progression into a new material tier. Future material eras can repeat a five-factory macro-cycle and culminate in another major industrial transition.

Potential future material progression includes conventional metal processing, alloys/steel, rare materials, advanced materials, and later exotic materials. Exact materials and conversion ratios are not yet locked.

Earlier resources should ideally retain utility through conversion or supply-chain relationships rather than becoming immediately worthless.

## Gems — planned permanent currency

A separate permanent currency tentatively called **Gems** is planned.

Current concept:

- survives resets and factory transitions;
- approximately **1–2% global drop chance**;
- rare enough to remain valuable;
- not required for ordinary progression;
- may support permanent quality-of-life upgrades, permanent efficiency improvements, cosmetics, or optional boosts.

Exact Gem drop probability, eligible roll sources, amount per drop, and spending catalog are **TBD**.

## Monetization concepts — planned, not final economy constants

### Rewarded advertising

Primary proposed reward:

- completed rewarded ad → **2x production rate for 5 minutes**.

Ads remain optional. Reward only after confirmed successful completion. Ad failure or refusal must never block normal progression.

### Resource bundles

Planned Metal Ore and Currency purchase bundles across approximately **$4.99 to $44.99**.

Exact quantities must be set after the base economy is balanced and should remain relevant across factory tiers without making normal progression meaningless.

### Supervisor bundle

Working concept: approximately **$29.99** for:

- 1 Supervisor;
- 2 Production Lines;
- included Production Lines already leveled to a useful state.

Exact included levels and value are TBD.

### Ad-free

Working price: **$7.99 or $9.99** — final price TBD.

If rewarded ads remain available to ad-free owners, they must remain explicitly voluntary.

## Balance parameters still requiring approval

Before treating the complete economy as production-ready, determine:

1. Pickaxe and Drill upgrade costs.
2. Supervisor purchase costs.
3. Production Line purchase and upgrade costs.
4. Production Lines 3–10 output and Currency curves.
5. Drill Levels 8–10 post-reset Currency rates.
6. Exact reset unlock trigger and reset wipe/retain list.
7. Factory 2–5 multipliers and unlock costs.
8. Factory completion rewards.
9. Giga-Factory transition rules and first new material.
10. Gem drop rules and Gem spending.
11. Paid resource-bundle quantities.
12. Final ad-free price and Supervisor-bundle contents.

Coding agents should keep these parameters configurable and must not silently choose final values for TBD items.
