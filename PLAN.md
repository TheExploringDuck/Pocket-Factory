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

### Interstitial ads — limited use

If interstitial ads are implemented, show them only at natural breaks in play and apply sensible frequency limits. Do not interrupt active factory management or repeatedly block the player from normal actions.

### Avoid

- Energy systems designed to force ad views
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

## Current principle

**Make the factory worth playing first. Ads may accelerate or enhance play; they should not be the reason the economy works.**
