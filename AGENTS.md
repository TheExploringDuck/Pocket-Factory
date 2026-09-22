# AGENTS.md — Pocket Factory Development Rules

These instructions apply to Codex and other coding agents working in this repository.

## Read Before Changing Code

Before modifying the project:

1. Read `README.md`.
2. Read `GAME_RULES.md` for the current economy, progression, reset, factory, and monetization design.
3. Read `PLAN.md`.
4. Read the GitHub issue being worked on.
5. Inspect the current implementation before assuming how a system works.

`GAME_RULES.md` is the numerical/design source of truth for currently approved gameplay parameters. Values marked **TBD** or **provisional** must remain configurable and must not be silently invented or treated as final.

Do not redesign unrelated systems as part of a targeted task.

## Codex / Arden Execution Rules

These are standing execution rules for Codex when working in this repository.

1. **Pull latest `main` before starting work.** If local work conflicts with newer design documents, stop and reconcile instead of overwriting the newer rules.
2. **Read the source-of-truth files first:** `AGENTS.md`, `GAME_RULES.md`, `PLAN.md`, `COMPETITION.md` when relevant, and the current implementation.
3. **Use `GAME_RULES.md` as the numerical authority.** Approved/finalized values are not suggestions. Do not rebalance, round, replace, or invent constants unless the user explicitly requests a balance revision.
4. **Use `PLAN.md` as the scope/roadmap authority.** The first closed-test target is playable progression through **Giga-Factory #2**.
5. **Keep Godot/C# as the runtime stack.** Do not migrate the game runtime to Python, Unity, another engine, or another language unless explicitly instructed. Python may be used only for offline balance/simulation tooling when useful.
6. **Preserve working systems.** Prefer the smallest change that satisfies the task. Do not refactor unrelated code, rename broad APIs, or rewrite functioning systems for style.
7. **Do not silently redesign presentation.** The current direction is a clean animated **2D/2.5D industrial cutaway** with moving characters, machinery, conveyors, resources, and environmental motion. Do not return to late-NES/pixel-art as the required final style unless explicitly instructed.
8. **Preserve manual player agency.** The pickaxe remains a visible early interaction even after automation begins.
9. **Respect monetization rules.** No forced ads. Advertising is player-initiated rewarded advertising only. Do not create pay-to-win competition mechanics.
10. **Respect competition isolation.** Competition unlocks at Production Line 1 and its economy must remain separate from permanent main-game spending/progression.
11. **Save compatibility matters.** Extend/migrate versioned saves safely; never delete or invalidate player progress without an explicit migration plan.
12. **Test incrementally.** Run the smallest relevant test/build after each meaningful change instead of waiting until the end of a large batch.
13. **Stop on ambiguity that changes game design.** If implementation requires a new economy value, progression rule, monetization rule, material ratio, or post-Giga-5 design that is not already approved, leave it configurable/TBD and report it instead of choosing a value.
14. **Conserve agent usage.** Complete the requested slice before optional polish. Do not spend substantial time generating speculative assets, broad refactors, or future systems outside the requested milestone.
15. **Do not overwrite newer repository decisions.** Before replacing a design/document file, compare it with current `main`. Merge intentional changes rather than restoring an older local copy.
16. **Finish with evidence.** Report files changed, behavior implemented, tests/builds actually run, failures/limitations, and deliberately deferred work. Never claim validation that was not executed.

When a request is too large for one pass, prioritize in this order:

**correct core simulation → save compatibility → tests → playable integration → presentation polish → optional tooling/documentation.**

## Product Direction

Pocket Factory is a **mobile-first industrial factory game** about building a capable production company from the mine floor outward. The industrial route is the primary game and current implementation focus; an advanced sci-fi counterpart is a supported presentation and future-content direction.

The pickaxe remains the player's signature hands-on tool: a direct, satisfying way to strike ore, start a new operation, and help a constrained intake bay before the wider automation network takes over. Preserve that manual agency as the company grows; do not reduce it to generic mining-game decoration.

The core loop is:

**Collect → Build → Automate → Optimize → Expand → Prestige → Repeat**

The factory itself should be fun to watch. Avoid reducing the game to menus and rapidly increasing numbers without visible production behavior.

## Technical Direction

- Target engine: **Godot / C#** unless a task explicitly changes this decision.
- First release target: **Android**.
- Keep economy/simulation logic separate from Godot node presentation code whenever practical.
- Prefer deterministic or controllable simulation logic so balancing tests can run without rendering.
- Abstract time and random behavior where useful for testing.
- Keep economy parameters data-driven rather than scattering tuning constants through gameplay code.
- Avoid unnecessary dependencies and large frameworks for small features.
- Do not commit secrets, signing credentials, API keys, keystores, service-account files, or private configuration.

## Economy Rules

- Follow the approved numerical and progression rules in `GAME_RULES.md`.
- Do not invent values for parameters marked TBD/provisional.
- Production must function without ads.
- Normal progression must remain possible without watching ads.
- Offline progression must be bounded and validated against clock errors and extreme elapsed-time values.
- No negative resources, NaN, infinity, duplicate reward grants, or impossible machine states.
- Economy changes should be validated through simulation before release.
- Major tuning changes must report before/after values or model outputs rather than relying only on feel.

## Advertising Rules

Pocket Factory is intended to monetize with advertising, but monetization must not become the gameplay loop.

Preferred model: **optional rewarded ads**.

Reward magnitudes, durations, prices, and bundle quantities are intentionally not fixed during early development. Keep them configurable and derive final values from economy simulations and player testing rather than inventing constants.

Allowed examples include:

- temporary production multiplier
- doubled offline earnings
- instant shipment completion
- temporary overclock
- bonus material delivery
- bonus research/prestige reward

Standing constraints:

- Never require an ad for ordinary progression.
- Never add energy systems that prevent the player from continuing to play.
- Never add paid/ad-gated retries.
- Never add loot boxes or randomized paid rewards.
- Do not intentionally create frustrating waits solely to pressure ad viewing.
- Do not place persistent banner ads over the active factory play area.
- **Never add forced ads of any kind.** This is a hard product rule.
- Do not add forced interstitials, startup/pre-roll ads, level-transition ads, exit ads, or forced banner placements.
- Advertising may only be player-initiated rewarded advertising unless the product direction is explicitly revised in `GAME_RULES.md`.
- Because forced ads are not part of the product, do not add an ad-free purchase solely to remove advertising.
- Rewarded-ad benefits must only be granted after a confirmed successful completion callback.
- Reward granting must be idempotent so callbacks cannot duplicate rewards.
- Core simulation code must not directly depend on a specific advertising SDK. Use an interface/adapter layer.
- Ad failure, no-fill, offline mode, or user refusal must never corrupt game state or block play.

## Save and Offline Progression

- Preserve save compatibility after the first public release whenever practical.
- Version save data from the beginning.
- Add explicit migration paths when save structure changes.
- Never delete player progress silently.
- Offline calculation should use a configurable maximum duration rather than unbounded elapsed time.
- Detect obviously invalid device-clock changes and fail safely.
- Add automated tests for save/load, migrations, and offline progression.

## Art and Presentation

The active visual direction is a **clean stylized 2D/2.5D industrial cutaway with animated figures and moving machinery**.

Pixel art / late-NES presentation is historical prototype work, **not the required final direction**.

Preserve the readable idle-miner-style cutaway and visible material flow, but present it with smoother authored animation and more expressive motion:

- animated workers/characters with clear states such as idle, walk, pick-up, swing, and work;
- visibly moving drills, conveyors, carts, elevators, machines, and processed materials;
- readable industrial structures and factory layers;
- environmental motion such as lights, smoke/steam, sparks, doors, and status indicators where useful;
- clear visual indication of production flow, bottlenecks, machine state, and shipping output;
- practical early industrial materials, with later eras allowed to introduce more advanced materials and visual language.

The short opening sequence should establish the character finding/using the pickaxe and transition quickly into live gameplay. Keep it skippable and do not build an elaborate cinematic/dialogue framework unless specifically requested.

Do not switch back to a full-3D command-view camera unless explicitly instructed.

Keep the pickaxe visually meaningful at early acquisition sites and maintain a clear relationship between manual input and the automation chain it starts.

Use **TheExploringDuck** for visible developer/publisher branding. Do not use **CasualGameStudios** in game branding, store-facing text, splash screens, menus, credits, or mockups unless a later explicit decision changes the brand.

Do not use visual complexity to hide unclear mechanics.

## Progression / release targets

- The first major progression chapter contains **five Giga-Factories**.
- Giga #1 closes Material Era 1 at roughly a 35-hour baseline target.
- Later Material Eras reuse the same relative five-factory structure with a **3:2 previous-resource:new-resource** requirement and target roughly **38–40 additional hours each**.
- Competition unlocks when the player constructs **Production Line 1**.
- The first Google Play closed-test candidate should be playable through **Giga-Factory #2**.
- Giga #3–#5 are later content within the same first major chapter and are not required for the first closed test.
- Do not invent the post-Giga-5 system; that second-act design is intentionally open.

## Scope Discipline

For every task:

- Work only on the requested issue unless a blocking dependency is discovered.
- Report blocking architecture problems before large refactors.
- Do not rewrite working code purely for style.
- Do not change monetization rules, save format, package identity, build settings, or unrelated balance unless the task requires it.
- Keep commits focused and explain meaningful design choices.

## Testing Expectations

Where applicable, add tests before declaring a task complete.

Important coverage includes:

- production rates and resource flow
- machine upgrades
- bottleneck behavior
- save/load and migrations
- offline progression
- prestige/reset behavior
- ad reward idempotency
- no-ad/offline behavior
- long-running economy simulation
- mobile performance-sensitive loops

For economy changes, simulate representative durations and progression states rather than testing only single functions.

If the current environment cannot launch Godot, distinguish clearly between:

- code/tests that were actually executed
- checks that were only reasoned about
- Godot Editor/mobile validation still required

Never claim a Godot build, Play Mode test, APK/AAB validation, or visual check occurred unless it actually did.

## Definition of Done

A task is complete only when:

- requested behavior is implemented
- relevant automated tests pass or remaining test limitations are documented
- no unrelated systems were changed without explanation
- save/economy implications are considered
- manual Unity/mobile checks still required are listed
- the GitHub issue acceptance criteria are addressed
