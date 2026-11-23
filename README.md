# RealismSuite-GTAV

![GTA V Enhanced](https://img.shields.io/badge/GTA%20V-Enhanced-1f6feb)
![Game build](https://img.shields.io/badge/game%20build-2.0.1158.16-555)
![ScriptHookVDotNet](https://img.shields.io/badge/ScriptHookVDotNet-3.9.0-6f42c1)
![LemonUI](https://img.shields.io/badge/LemonUI-2.2-f0c419)
![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8-512bd4)
![Language](https://img.shields.io/badge/language-C%23-239120)
![Mode](https://img.shields.io/badge/mode-story%20mode%20only-2ea043)
![Status](https://img.shields.io/badge/status-in%20development-orange)

A single player mod pack for GTA V Enhanced that pulls Online content into story mode, adds a realistic economy, and replaces the wanted system with something closer to GTA IV and GTA 6: cops have to actually see you, witnesses call it in, and you get a description and a warrant instead of a magic radar.

Everything ships as one script, `RealismSuite.dll`. Several older mods were merged into it so they share one codebase, stop fighting each other, and can be fixed in one place.

> Story mode only. Do not use this anywhere near GTA Online.

---

## Contents

- [Features](#features)
  - [Police](#police)
  - [Heists](#heists)
  - [Economy](#economy)
  - [Vehicles and fuel](#vehicles-and-fuel)
  - [Quality of life and fixes](#quality-of-life-and-fixes)
  - [Merged mods](#merged-mods)
- [Requirements](#requirements)
- [Installation](#installation)
- [Controls](#controls)
- [Configuration](#configuration)
- [Building from source](#building-from-source)
- [Project layout](#project-layout)
- [Known issues](#known-issues)
- [Credits](#credits)

---

## Features

### Police

A full replacement for the vanilla wanted system. No on-screen instructions; you find out you're in trouble the way you would in real life.

- **Witnesses matter.** Cops only react to crimes they see. Civilians who see something phone it in after a short delay, and scaring them off can stop a minor report.
- **Bodies get found.** Kill someone out of sight and the body may be discovered later, with the response scaled to how many there are. Attacking an officer or a police vehicle escalates straight away.
- **Search areas, not radar.** Police search where you were last seen. The area grows over time and moves when there's a possible sighting. It shows on the minimap.
- **Descriptions and warrants.** Get away and you may leave with a warrant that describes your outfit, car and weapon. Change clothes or cars and it takes them longer to recognise you; a mask makes you unrecognisable.
- **Escalating force.** Tasers and batons at low levels, a warning before lethal force, lethal response at higher levels.
- **Surrender and booking.** Hands up to be cuffed and booked, then choose a lawyer, bail or time. Bribes are possible if you're lucky.
- **Consequences.** Dying while wanted costs money, scaled by the wanted level.
- **HUD.** A star row with knowledge icons (outfit, vehicle, weapon) so you know what the police know about you.
- **Police computer.** Take vigilante jobs from a police car's computer.

### Heists

Lester runs side jobs you can do outside the story, planned and crewed like the story heists.

- **Planning board** at Lester's garment factory. The factory interior opens while you're there and is put back exactly as the story had it when you leave.
- **Crew.** Pick gunmen and a driver (or drive yourself). Each has a skill level and a cut. Crew can die for good, and their spot on the board is gone. Lester finds new contacts over time so a role is never empty. The story crew become available after The Big Score.
- **The Fleeca Job** (two setups and a finale):
  - Scope out the branch, then steal an armored Kuruma from a guarded spot.
  - Hold up the bank. Drill the vault lock with the drilling minigame, or intimidate the manager into opening it.
  - Your gunmen hold the lobby and the door. Without them, hostages make a run for it and the cops arrive sooner.
  - Grab cash (or gold) a handful at a time, with a take counter, and stop whenever you decide it's time to go.
  - Police response time depends on the distance to the nearest station.
  - Get shot while carrying the bag and you drop money.
- **Results screen** in the style of the game's own heist summary: take, money dropped, fence fees, Lester's cut, crew cuts, Lester's expenses and your cut. Gold is fenced by Lester and paid out later.

### Economy

- **Stock market.** Live BAWSAQ and LCN prices that drift back toward a fair value over time. Story missions move the market through an editable events file, including the Lester assassination missions, with crash-and-rebound moves where it makes sense.
- **Dynasty 8.** Properties and businesses listed together on the Dynasty 8 site.
- **Businesses.** Buy and run businesses for daily income (BusinessV).
- **Illegal gambling dens** and **Online vehicle shops** in story mode.

### Vehicles and fuel

- **Fuel.** Cars use fuel and need to be refuelled at gas stations. Traffic refuels too, one car at a time, and avoids stations where something has gone wrong (fires, shootouts).
- **Trunk storage.** Store weapons in your personal vehicle's trunk and take them back out later, including during missions. Works with a mouse.
- **Vehicle mechanics.** Wear and breakdowns, paused during missions so they can't ruin one.

### Quality of life and fixes

- Minimap and HUD safety net: if a script leaves the minimap or HUD hidden in free roam, it comes back.
- Phone fixes: no more blank contact rows, and the phone's internet no longer gets stuck on an error page.
- Stuck-menu release: hold Backspace to close a menu that has locked your controls.
- Smaller fixes for pickups, plates, blips and personal vehicles.

### Merged mods

These were separate mods and now live inside RealismSuite. Each keeps its own source folder so it can be maintained separately.

| Mod | What it adds |
| --- | --- |
| SPA II | Apartments, garages and properties in story mode |
| iFruitAddon2 | Custom phone contacts (blank-row bug fixed) |
| INMNativeUI | Menu library used by SPA II |
| BusinessV | Businesses with daily income |
| Illegal Gambling Dens | Gambling dens (ported to the current ScriptHookVDotNet API) |
| Online Vehicle Shops | Online vehicle websites |
| Vehicle Mechanics | Vehicle wear and breakdowns |
| AddonWeapons2 | Add-on weapon support |

---

## Requirements

- GTA V **Enhanced** (tested on build 2.0.1158.16)
- [Script Hook V](http://www.dev-c.com/gtav/scripthookv/)
- [ScriptHookVDotNet 3](https://github.com/scripthookvdotnet/scripthookvdotnet) (3.9.0 or newer, Enhanced-compatible build)
- [LemonUI](https://github.com/LemonUIbyLemon/LemonUI) for ScriptHookVDotNet 3 (`LemonUI.SHVDN3.dll`, 2.2)

Optional, but some features expect them:

- GameplayFixesV with `SilentWanted = true` (hides the vanilla wanted HUD and music so the suite's own star row is the only one on screen)

---

## Installation

1. Install Script Hook V, ScriptHookVDotNet 3 and LemonUI as their own instructions describe.
2. Copy into your game's `scripts` folder:
   - `RealismSuite.dll`
   - `LemonUI.SHVDN3.dll`
   - the data folders: `RealismSuite`, `SPA II`, `BusinessV`, `iFruitAddon2`, `AddonWeapons`
3. **Remove the standalone versions** of any mod listed under [Merged mods](#merged-mods). Running both will cause conflicts.
4. Start the game in story mode.

Settings and logs are written to `scripts\RealismSuite\`.

---

## Controls

| Action | Default |
| --- | --- |
| Hands up / surrender | `J` |
| Offer a bribe | `B` |
| Call a lawyer | `G` |
| Open trunk storage | Hold `E` at the back of your car |
| Plan a heist | `E` at the marker at Lester's factory |
| Lester's jobs on the phone | Call **Lester (Jobs)** |
| Drill: spin / push / stop | Left mouse / `W` / `Backspace` |
| Cash grab: stop and go | `E` |
| Release a stuck menu | Hold `Backspace` |
| Reload scripts | ScriptHookVDotNet's reload key (`\` by default in some setups) |

Keys can be changed in the `.ini` files listed below.

---

## Configuration

All files are in `scripts\RealismSuite\`.

| File | What it controls |
| --- | --- |
| `Police.ini` | Keys, HUD, civilian reports, warrant length, death costs, lawyer/bail/bribe prices |
| `PoliceComputer.ini` | Vigilante jobs |
| `RealFuel.ini` | Fuel use, traffic refuelling |
| `StockMarket.ini` | Market behaviour |
| `StockEvents.txt` | Story events that move stocks (reloads by itself when saved) |
| `Heists.ini` | Lester's factory interior |
| `Heists.dat`, `Crew.dat` | Heist progress and your crew (delete to reset) |

---

## Building from source

The project builds with Mono's `mcs` on Linux or with any C# compiler that targets .NET Framework 4.8.

1. Put these reference assemblies in a `ref` folder:
   - `ScriptHookVDotNet3.dll`
   - `LemonUI.SHVDN3.dll`
   - `PresentationCore.dll` (a reference stub is enough)
2. Run:

```sh
./build.sh
```

The output is `build/RealismSuite.dll`.

The compiler flags that matter: `-target:library -unsafe -langversion:7 -sdk:4.7.2-api`. Two embedded resources are needed (`BusinessV.Defaults.businesses.json` and `iFruitAddon2.Properties.Resources.resources`); `build.sh` passes both.

---

## Project layout

```
src/            RealismSuite's own code
  Heists/       heist system: jobs, crew, minigames, results screen
merged/         source of each merged mod, kept separate
heists-src/     reference code used while building the heist system
build.sh        build script
fix_spa.py      regenerates the SPA II source
spa_patches.py  RealismSuite's changes to SPA II
```

---

## Known issues

- The heist system is new. Expect rough edges in crew AI under heavy fire.
- The bank manager doesn't have a proper keycard animation yet.
- On gold runs the bars may not visibly disappear from the trolley while you grab.
- BAWSAQ can sit on a loading page after a trade; going back to the site's home page clears it.

Bug reports are welcome. Please include `scripts\RealismSuite\*.log` and `ScriptHookVDotNet.log` from your game folder.

---

## Credits

- **Script Hook V** by Alexander Blade
- **ScriptHookVDotNet** by crosire and contributors
- **LemonUI** by justalemon
- **iFruitAddon2** by Bob74
- **Single Player Heists** by HKH191 (positions and animation references for the heist system)
- The original authors of **SPA II**, **INMNativeUI**, **BusinessV**, **Illegal Gambling Dens**, **Online Vehicle Shops**, **Vehicle Mechanics** and **AddonWeapons2**, whose work is merged into this pack

Rockstar Games owns Grand Theft Auto V. This project is not affiliated with or endorsed by Rockstar Games or Take-Two Interactive.

## License

No license has been chosen yet. Until one is added, all rights are reserved by the author, except for the merged mods, which remain under their original authors' terms.
