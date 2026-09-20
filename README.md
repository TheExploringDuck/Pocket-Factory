# Pocket Factory

> **Working title:** Pocket Factory

**Pocket Factory** is a mobile-first industrial factory game about growing a capable production company from its first mine into an ambitious inter-factory operation.

The visual direction is **late-NES-inspired pixel art with modern polish through a traditional idle-miner cutaway**: readable underground work levels, elevator logistics, practical factories, active conveyors, bold machine silhouettes, framed HUD panels, and crisp sprite animation. It is intentionally limited and graphic, not a literal low-fidelity restriction.

Industrial is the primary playable route, with deep navy rock, weathered steel, safety amber, rust-orange heat, off-white highlights, and restrained teal status lights. The same factory layout also supports an Advanced sci-fi visual mode with cyan/violet signal equipment and synthesis machinery. The two views share the same first-factory economy; they are not separate games.

The player should feel like they are running an in-world corporation without turning the game into a suit-and-tie office simulation. Extraction is an early capability, not the game's permanent identity. Each factory should solve a distinct production problem and visibly hand output to the next stage or site through logistics and shipping.

The pickaxe remains a signature manual tool: a player-driven ore strike that gets an intake bay moving or helps clear an early bottleneck, while the wider factory turns that direct action into automated output.

## Core Idea

The player starts with a small production setup and gradually builds a larger automated factory.

**Collect → Build → Automate → Optimize → Expand → Prestige → Repeat**

The game should be entertaining to *watch*, not just a spreadsheet with bigger numbers. Machines should visibly process materials, production chains should become busier as the factory grows, and bottlenecks should be understandable from the screen.

## Early Game Loop

A first playable vertical slice should stay intentionally small:

- 1 raw resource
- 3 machines
- 1 finished product
- 1 primary currency
- 1 visible production chain
- 5 meaningful upgrades
- save/load
- bounded offline progress
- one factory screen
- placeholder late-NES industrial and advanced presentation variants

If those three machines are satisfying to watch and optimize, the project has a strong foundation. More content comes later.

## Progression Direction

The longer-term progression is expected to move through increasingly capable production eras, for example:

**Workshop → Factory → Industrial Complex → Automated Plant → Experimental Facility**

Prestige should represent a meaningful technological leap rather than a reset for its own sake. A reset should unlock permanent capabilities, new automation possibilities, or new production systems.

## Monetization Philosophy

Pocket Factory is designed to use ads, with **rewarded ads as the preferred format**.

The base game must remain playable without watching ads. Ads may accelerate progress or provide optional bonuses, but they should not be required for ordinary progression.

Potential rewarded-ad uses include:

- temporary production multiplier
- doubled offline earnings
- instant shipment completion
- temporary machine overclock
- bonus material delivery
- extra research or prestige reward

The project should avoid:

- energy systems that prevent play
- paid or ad-gated retries
- loot boxes or randomized paid rewards
- intentionally frustrating waits designed only to force an ad
- constant banner ads covering the factory
- frequent interstitial interruptions during active play

Interstitial ads, if ever added, should only appear at natural breaks and with strict frequency limits.

## Technical Direction

The current implementation target is **Godot / C#**, mobile-first, with Android as the first release platform.

Architecture should keep the production/economy simulation separate from presentation so the economy can be simulated, stress-tested, and balanced without rendering the full game.

Advertising should be isolated behind a provider interface so the core economy never depends directly on a specific ad SDK.

## Status

**Initial Godot/C# foundation.**

The repository now contains a headless C# economy core, a lightweight test runner for approved `GAME_RULES.md` values, and a Godot C# project shell for the mobile-facing game.

See:

- [`AGENTS.md`](AGENTS.md) for standing development rules
- [`PLAN.md`](PLAN.md) for the development roadmap and task order
- [`docs/ANDROID_CLOSED_TESTING.md`](docs/ANDROID_CLOSED_TESTING.md) for Android closed-test export setup

## Development

Pocket Factory is an independently directed project by **TheExploringDuck**, developed with AI-assisted programming tools as part of the design, implementation, testing, and iteration workflow.
