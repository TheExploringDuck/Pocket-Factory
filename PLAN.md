# Pocket Factory — Development Plan

This plan is the working roadmap for Pocket Factory. It should be updated as design and implementation decisions become concrete.

## Product direction

Pocket Factory is a mobile-first idle automation game built around the loop:

**Collect → Build → Automate → Optimize → Expand → Prestige → Repeat**

The game should feel satisfying without requiring purchases. Progression, factory design, optimization, offline production, and prestige are gameplay systems first; monetization must not make the unpaid game intentionally frustrating.

## Vertical slice

Build and validate a small complete loop before expanding content:

- 1 raw resource
- 3 machines
- 1 finished product
- 1 primary currency
- 1 production chain
- 5 meaningful upgrades
- Offline progress
- Save/load
- Basic mobile factory screen
- Simulation separated from presentation so economy tests can run headlessly

## Technical direction

Pocket Factory is a **Godot / C#** project. Keep gameplay economy and progression logic in a headless C# core so balancing tests can run without launching the Godot editor or rendering the game.

## Presentation direction

Pocket Factory should use a **late-NES-inspired pixel-art factory style built on a traditional idle-miner cutaway**. The target feel is a readable, capable company site: underground work levels, elevator logistics, practical early facilities, carts, conveyors, furnaces, shipping bays, simple framed HUD panels, and animation built from clear chunky sprites.

Industrial is the primary playable presentation. An Advanced sci-fi skin may reinterpret the same factory layout with signal infrastructure, synthesis equipment, cyan/violet light, and automation-forward visual language; it shares the economy and must not become a second game. Preserve the intuitive traditional idle-miner structure while giving the factory, logistics, and later sites their own identity. Do not assume every screen needs a bottom progression-flow strip; include progression flow only where it improves navigation, onboarding, or player understanding.

Use **TheExploringDuck** as the visible developer/publisher brand in store-facing text, splash/credit surfaces, and mockups.

## Advertising / monetization

Advertising is a planned part of the game architecture rather than a late-stage addition.

### Rewarded ads — primary format

Rewarded ads should be explicitly optional and provide clear, finite benefits such as:

- Temporary 2× production
- Double offline earnings when returning to the game
- Bonus shipment or delivery
- Temporary machine overclock
- Bonus resource delivery
- Extra research/progression opportunity

The player must know the reward before choosing to watch. Declining an ad must leave the normal gameplay loop functional and enjoyable.

### No forced ads — hard rule

Pocket Factory must not use forced advertising.

Do not implement:

- automatic interstitial ads;
- startup/pre-roll ads;
- level-transition or factory-transition ads;
- exit ads;
- banner ads occupying gameplay space;
- energy systems or artificial blockers designed to force an ad view.

Advertising is limited to **player-initiated rewarded ads**. The reward must be stated before the player opts in, and declining an ad must never harm or block normal progression.

Because there are no forced ads, an ad-free purchase is not part of the planned product model unless this direction is explicitly changed later.

### Avoid

- Paid/rewarded retries for ordinary progression
- Random paid rewards or gambling-like monetization
- Deliberately slowed progression intended to make ads feel mandatory
- Ads obscuring factory controls or important information
- Reward logic that can grant rewards twice or lose a legitimate reward because of lifecycle/network issues

## Ad architecture

Keep the game economy independent from the advertising SDK. Game systems should request an abstract ad/reward service rather than calling a provider directly. This allows provider changes, editor/test implementations, and deterministic tests without rewriting the economy.

Reward fulfillment must be idempotent and validated. Handle unavailable ads, failed loads, cancellation, application backgrounding, network loss, and provider callbacks without corrupting saves or granting duplicate rewards.

Ad availability must never be required for base progression.

## Economy testing

Economy changes should be tested through simulation before release. Track production rates, upgrade costs, time-to-upgrade, offline earnings, prestige timing, and the value of ad rewards.

Ad rewards should accelerate an already-functional economy rather than compensate for an intentionally weak baseline economy.

## First major progression chapter

The launch-era long-form progression target is now:

**Material Era 1 → Giga #1 → Material Era 2 → Giga #2 → Material Era 3 → Giga #3 → Material Era 4 → Giga #4 → Material Era 5 → Giga #5**

- Giga #1 targets roughly **35 hours** of baseline free progression.
- Each later Material Era targets roughly **38–40 additional hours**.
- Giga #5 therefore lands near **190–195 hours** of baseline free progression.
- After each Giga #1–#4 completion, use a short surprise-discovery message/event to introduce the next material.
- Each later era uses a **3:2 previous-resource:new-resource** requirement rather than stacking every historical resource.
- The newly discovered resource begins near **35% of the prior equivalent production rate** so the repeated era remains near the 38–40 hour target.
- Giga #5 ends this repeated Material Era chapter. The system after Giga #5 should be meaningfully different rather than simply adding Resource #6.

## Competition unlock

The recurring competition system becomes available once the player constructs **Production Line 1** in the main game.

Competition remains economically isolated from permanent account progression as defined in `COMPETITION.md`. Main-game spending must not directly buy leaderboard advantage.

## Development phases

1. Establish deterministic simulation and data model.
2. Complete the vertical-slice production chain.
3. Implement save/load and offline progress.
4. Build mobile-facing presentation and controls.
5. Add an abstract advertising service and test implementation.
6. Integrate a production ad provider after the gameplay loop is stable.
7. Add rewarded-ad placements one at a time and measure their effect on pacing.
8. Add additional content, machines, resources, research, and prestige systems.
9. Polish, device-test, balance, and prepare store release.

## Near-term milestone

Get the original Pocket Factory vertical slice **up and testing by the end of the week**. "Up and testing" means the first factory screen launches reliably, its core manual-to-automation loop is playable, and the headless economy checks cover the implemented rules.

## Closed-testing content target

Before beginning the planned Google Play closed test, target a build that is playable through **Giga-Factory #2**.

That means the closed-test candidate should include, at minimum:

- the opening/manual-mining sequence and Level-4 early automation;
- Drill progression;
- Production Lines 1–10 and Supervisors 1–5;
- all five standard factories of Material Era 1;
- Giga-Factory #1;
- the surprise discovery transition into Industrial Alloy / Material Era 2;
- the 3:2 previous:new resource economy for Material Era 2;
- all five standard factories of Material Era 2;
- Giga-Factory #2;
- competition unlocking at Production Line 1;
- save/load and offline progression across both eras;
- reviewer access for any paid/restricted features;
- optional rewarded-ad architecture, with no forced ads.

Giga-Factories #3–#5 remain part of the first major progression chapter but do **not** need to be complete for the first closed test.

## Deferred concept — Convergence

Keep this as a later expansion direction, not a current feature:

- Industrial sites and advanced sites may eventually become two distinct production approaches with different strengths and visual identities.
- A later **Convergence** event can join their supply chains, introduce cooperative work, and make combined production significantly more efficient than either approach alone.
- Do not implement or balance the split paths, the event, or its rewards until the original single-factory loop is playable and tested.

## Completed work

### 2026-09-16 — Initial Godot/C# foundation

- Switched the standing technical direction from Unity/C# to Godot/C#.
- Added a headless C# economy core for approved pickaxe, drill, production-line, supervisor, reset-rate, and rewarded-ad duration constants from `GAME_RULES.md`.
- Added a lightweight C# test runner that validates the approved economy tables and prevents TBD Drill reset rates from being silently invented.
- Added a Godot C# project shell with a placeholder mobile-sized main scene wired to the economy core.

Checks run:

- `dotnet run --project tests\PocketFactory.Core.Tests\PocketFactory.Core.Tests.csproj`
- `dotnet build game\PocketFactory.Godot\PocketFactory.Godot.csproj`

Still required:

- Open and validate the project in the Godot editor.
- Replace placeholder debug UI with the actual mobile factory screen.
- Add save/load and bounded offline progression tests before treating the first vertical slice as complete.

### 2026-09-16 — Factory Floor presentation pass

- Replaced the temporary debug-control screen with a playable portrait factory floor: live Drill production, manual mining, machine tuning, and a visible temporary 2x overclock state.
- Added a custom Godot C# pixel-inspired factory renderer with an animated mine cart, drill, furnace, conveyors, workers, lights, smoke, control room, and shipping floor.
- Kept unapproved upgrade prices out of the permanent economy. The current tuning controls clearly identify that their costs remain pending balance approval.
- Added the required Godot C# assembly-name setting to `project.godot` and verified the running scene can render through the Godot .NET runtime.

Checks run:

- `dotnet build game\PocketFactory.Godot\PocketFactory.Godot.csproj`
- `dotnet run --project tests\PocketFactory.Core.Tests\PocketFactory.Core.Tests.csproj`
- Godot .NET runtime captures of the normal and 2x-overclock factory states.

### 2026-09-17 — Factory floor polish pass

- Smoothed the mine cart and drill animation curves while preserving their chunky pixel-art movement.
- Added restrained factory depth cues: structural beams, controlled lamp and furnace glow, furnace sparks, moving ore, and stronger conveyor detailing.
- Added a lit shipping door to make the rightward production flow and next-factory handoff more readable.
- Added a headless-safe guard to the screenshot helper; visual captures now use Godot's normal renderer.

Checks run:

- `dotnet run --project tests\PocketFactory.Core.Tests\PocketFactory.Core.Tests.csproj --no-restore`
- `dotnet build game\PocketFactory.Godot\PocketFactory.Godot.csproj`
- Godot .NET renderer capture of the polished factory floor.

### 2026-09-17 — Industrial sci-fi tycoon direction

- Replaced the retro/pixel-art visual direction with a clean industrial sci-fi company-site direction.
- Reframed the first factory as three operational decks: acquisition, refinement, and assembly-to-shipping.
- Established the shipping door as a visible factory handoff so future sites can become distinct company branches rather than repeated mining rooms.
- Updated the prototype UI wording toward site operations while preserving the existing simulation and all economy behavior.

### 2026-09-17 — 3D command-view prototype

- Replaced the active factory-floor renderer with a procedural 3D clean-industrial site built in Godot C#.
- Established a high-angle operator camera that shows the full site as an overseen production system: extraction, refinery, conveyor line, robotic work cell, and shipping gate.
- Added real-time material flow, moving machine parts, responsive reactor and beacon pulses, purposeful lighting, and the first rounded refinery and signal forms.
- Kept the implementation asset-light and procedural for this prototype; future art passes can replace individual modules with authored 3D models without changing the gameplay screen.

Checks run:

- `dotnet build game\PocketFactory.Godot\PocketFactory.Godot.csproj`
- `dotnet run --project tests\PocketFactory.Core.Tests\PocketFactory.Core.Tests.csproj --no-restore`
- Godot .NET renderer capture of the high-angle command view.

### 2026-09-17 — Pickaxe retained as player agency

- Made the pickaxe an explicit manual-intervention pillar of the industrial sci-fi direction rather than retiring it with the pixel-art presentation.
- Reframed the active first-site interaction as a `PICKAXE STRIKE`, which feeds the intake bay while the rest of the facility automates its output.
- Added a visible pickaxe tool to the 3D acquisition bay and tied its strike motion to the existing manual action pulse.

### 2026-09-17 — 2.5D idle-miner cutaway direction

- Replaced the active full-3D command view with a clean 2.5D cutaway designed for the immediate readability of a traditional idle miner.
- Built a vertical mine shaft with three working levels, animated elevator logistics, drill and refinery equipment, a visible pickaxe strike at the ore face, carts, and moving material.
- Placed a compact factory wing beside the shaft so refined material visibly travels through automation and shipping without losing the mining identity.
- Removed the unused full-3D renderer. Future presentation work should extend the cutaway, its levels, factory wings, and later-site visual variety rather than return to a full-3D gameplay camera.

Checks run:

- `dotnet build game\PocketFactory.Godot\PocketFactory.Godot.csproj`
- `dotnet run --project tests\PocketFactory.Core.Tests\PocketFactory.Core.Tests.csproj --no-restore`
- Godot .NET renderer capture of the 2.5D cutaway.

### 2026-09-17 — Earth-led industrial palette

- Recolored the mine cutaway and mobile operations UI around coal-charcoal, stone, weathered steel, muted safety green, and rust/copper.
- Replaced the prior cool neon-cyan treatment with restrained production-status light, keeping the pickaxe strike and active heat/hazard moments warm and readable.
- Documented the early-site palette as a presentation rule; later sites can introduce more advanced materials while preserving strong material and status contrast.

Checks run:

- `dotnet build game\PocketFactory.Godot\PocketFactory.Godot.csproj`
- Godot .NET renderer capture of the earth-led industrial cutaway.

### 2026-09-17 — Mining safety-color pass

- Shifted early-site presentation to a recognizably mining-industrial color scheme: coal-black bays, worn rock and steel, safety yellow, and rust/copper accents.
- Added purposeful yellow-and-black hazard markings to the surface boundary, mine-level platforms, and shipping door.
- Used safety yellow as the active machine and primary action color, making the production path more legible at mobile scale.

Checks run:

- `dotnet build game\PocketFactory.Godot\PocketFactory.Godot.csproj`
- Godot .NET renderer capture of the mining safety-color cutaway.

### 2026-09-20 — Late-NES dual-site presentation

- Re-established late-NES-inspired pixel art as the active presentation direction: deliberate palettes, bold cutaway silhouettes, framed HUD panels, and crisp machine animation.
- Added a working in-game Industrial/Advanced presentation switch. Industrial is the default and primary playable route; Advanced is a sci-fi reinterpretation of the same mine, machines, and economy rather than a second game.
- Reworked the primary Industrial palette around deep navy rock, weathered steel, safety amber, rust-orange heat, off-white highlights, and restrained teal status light.
- Added Advanced signal beacons, synthesis arcs, and cyan/violet accents while preserving the same factory layout and interactions.

Checks run:

- `dotnet build game\PocketFactory.Godot\PocketFactory.Godot.csproj`
- `dotnet run --project tests\PocketFactory.Core.Tests\PocketFactory.Core.Tests.csproj --no-restore`
- Godot .NET renderer captures of Industrial and Advanced late-NES factory views.

### 2026-09-20 — Closed-test persistence and Android scaffold

- Added versioned local save data with validated restore behavior for the first factory state and the selected Industrial/Advanced presentation.
- Added deterministic, bounded offline Drill production that rejects backwards device-clock movement and keeps its duration cap centralized for later balance review.
- Corrected the manual Pickaxe Strike to use the approved Pickaxe rule rather than awarding Drill output.
- Added an Android App Bundle export preset using the proposed `com.theexploringduck.pocketfactory` identifier without committing any signing credential.
- Added a closed-test setup guide covering Android SDK, Godot export templates, external signing, and Play upload.

Checks run:

- `dotnet run --project tests\PocketFactory.Core.Tests\PocketFactory.Core.Tests.csproj --no-restore`
- `dotnet build game\PocketFactory.Godot\PocketFactory.Godot.csproj`

Still required:

- Upgrade the Godot .NET Android export environment to a template tested with Android API 36 before creating the first Google Play AAB. The currently installed Godot 4.5.1 template targets API 35, which is suitable only for local debug-device testing.
- Generate and securely store a release upload key outside the repository.
- Export and install the generated AAB through a Google Play closed-testing track.

### 2026-09-21 — Android debug-device build foundation

- Installed the Android SDK command-line tools, Platform-Tools/ADB, Android platforms 35 and 36, Build-Tools 35 and 36, CMake, and the Godot-required NDK locally; none are committed to the repository.
- Installed the matching Godot 4.5.1 .NET export templates and Android Gradle template locally.
- Added the Godot-project-local solution required to publish the C# assemblies for Android packing.
- Produced a signed arm64 debug APK with the proposed package id `com.theexploringduck.pocketfactory`, Android min SDK 24, and the currently supported API 35 target. The package is for local device smoke testing only, not Google Play.

Checks run:

- `dotnet run --project tests\PocketFactory.Core.Tests\PocketFactory.Core.Tests.csproj --no-restore`
- `dotnet build game\PocketFactory.Godot\PocketFactory.Godot.csproj --no-restore`
- Godot C# arm64 PCK export
- Android Gradle Mono debug APK build and `apksigner` verification
- Android manifest/package inspection with `aapt`

## Current principle

**Make the factory worth playing first. Ads may accelerate or enhance play; they should not be the reason the economy works.**
