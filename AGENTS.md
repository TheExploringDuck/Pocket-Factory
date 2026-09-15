# AGENTS.md — Pocket Factory Development Rules

These instructions apply to Codex and other coding agents working in this repository.

## Read Before Changing Code

Before modifying the project:

1. Read `README.md`.
2. Read `PLAN.md`.
3. Read the GitHub issue being worked on.
4. Inspect the current implementation before assuming how a system works.

Do not redesign unrelated systems as part of a targeted task.

## Product Direction

Pocket Factory is a **mobile-first idle automation game** with a retro-inspired pixel look and modern presentation.

The core loop is:

**Collect → Build → Automate → Optimize → Expand → Prestige → Repeat**

The factory itself should be fun to watch. Avoid reducing the game to menus and rapidly increasing numbers without visible production behavior.

## Technical Direction

- Target engine: **Unity / C#** unless a task explicitly changes this decision.
- First release target: **Android**.
- Keep economy/simulation logic separate from MonoBehaviour presentation code whenever practical.
- Prefer deterministic or controllable simulation logic so balancing tests can run without rendering.
- Abstract time and random behavior where useful for testing.
- Keep economy parameters data-driven rather than scattering tuning constants through gameplay code.
- Avoid unnecessary dependencies and large frameworks for small features.
- Do not commit secrets, signing credentials, API keys, keystores, service-account files, or private configuration.

## Economy Rules

- Production must function without ads.
- Normal progression must remain possible without watching ads.
- Offline progression must be bounded and validated against clock errors and extreme elapsed-time values.
- No negative resources, NaN, infinity, duplicate reward grants, or impossible machine states.
- Economy changes should be validated through simulation before release.
- Major tuning changes must report before/after values or model outputs rather than relying only on feel.

## Advertising Rules

Pocket Factory is intended to monetize with advertising, but monetization must not become the gameplay loop.

Preferred model: **rewarded ads**.

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

The visual direction is **pixel-inspired, not hardware-limited 8-bit**.

Aim for:

- crisp pixel-style sprites and readable silhouettes
- chunky, expressive machine motion
- modern UI spacing and typography
- smooth animation where it improves readability
- restrained particles, sparks, smoke, glow, and production feedback
- clear visual indication of bottlenecks and machine state

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

If the current environment cannot launch Unity, distinguish clearly between:

- code/tests that were actually executed
- checks that were only reasoned about
- Unity Editor/mobile validation still required

Never claim a Unity build, Play Mode test, APK/AAB validation, or visual check occurred unless it actually did.

## Definition of Done

A task is complete only when:

- requested behavior is implemented
- relevant automated tests pass or remaining test limitations are documented
- no unrelated systems were changed without explanation
- save/economy implications are considered
- manual Unity/mobile checks still required are listed
- the GitHub issue acceptance criteria are addressed
