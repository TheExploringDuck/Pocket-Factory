# Pocket Factory

> **Working title:** Pocket Factory

**Pocket Factory** is a mobile-first idle automation game about turning a tiny workshop into an increasingly ridiculous, efficient, and satisfying production machine.

The visual direction is **retro-inspired pixel art with modern presentation**: crisp chunky machines, readable silhouettes, expressive animation, sparks, smoke, particles, clean UI, and smooth feedback rather than strict hardware-era 8-bit limitations.

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
- placeholder pixel-inspired art and effects

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

The current implementation target is **Unity / C#**, mobile-first, with Android as the first release platform.

Architecture should keep the production/economy simulation separate from presentation so the economy can be simulated, stress-tested, and balanced without rendering the full game.

Advertising should be isolated behind a provider interface so the core economy never depends directly on a specific ad SDK.

## Status

**Concept and planning phase.**

See:

- [`AGENTS.md`](AGENTS.md) for standing development rules
- [`PLAN.md`](PLAN.md) for the development roadmap and task order

## Development

Pocket Factory is an independently directed project developed with AI-assisted programming tools as part of the design, implementation, testing, and iteration workflow.
