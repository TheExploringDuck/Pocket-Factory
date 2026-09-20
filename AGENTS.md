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
- Do not add forced interstitial ads unless a GitHub issue explicitly authorizes them.
- If interstitials are authorized later, use natural breaks and strict frequency caps.
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

The visual direction is **late-NES-inspired pixel art with modern polish**, not a literal hardware restriction or a full-3D management view. Use deliberate, limited palettes, bold tilework, chunky machine silhouettes, clean sprite animation, and sharp pixel edges.

Use a clear site-operation language within a traditional, layered idle-miner cutaway: practical early industrial factories with wood-and-steel supports, carts, conveyors, furnaces, shipping bays, elevator logistics, and active machinery. The same production layout may also be shown through an advanced sci-fi skin using signal infrastructure, synthesis machinery, robotics, and unfamiliar materials. Treat reference images as style guidance, not exact layout requirements.

Use the late-NES industrial palette for the primary site: deep navy rock, weathered steel, warm safety amber, rust-orange heat, off-white highlights, and restrained teal status lights. The advanced skin may add cyan and violet signal light while preserving the same crisp pixel-art language.

Show the pickaxe as a purposeful piece of field equipment at early acquisition sites, with a clear visual relationship to the material intake and the automated line it feeds.

Aim for:

- readable factory layers and an obvious left-to-right material handoff
- confident late-NES machine silhouettes, practical industrial materials, and deliberate visual hierarchy
- simple framed HUD panels that support production, contracts, logistics, and expansion
- smooth animation where it improves readability
- restrained particles, glow, robotic motion, and production feedback
- clear visual indication of bottlenecks, machine state, and shipping output

Do not require the bottom progression-flow strip on every screen. Use it only where it helps explain progression or navigation.

Embrace the immediate readability of a traditional idle-miner cutaway without becoming a clone. Extraction is an early capability, not the whole fantasy. Each factory should feel like a distinct company site that solves a new production problem. Keep Industrial as the default playable path while making Advanced an explicit alternate visual interpretation, not a second economy.

Use **TheExploringDuck** for visible developer/publisher branding. Do not use **CasualGameStudios** in game branding, store-facing text, splash screens, menus, credits, or mockups unless a later issue explicitly changes the brand.

Do not use visual complexity to hide unclear mechanics.

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
