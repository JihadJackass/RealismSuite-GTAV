# Arena War Business (HKH191) – Decompiled Source Catalogue

Source: `dec/ArenaWarBusiness/` (mostly `Class1.cs`, 22,944 lines). Data: `x/ArenaWar Business/`.
All line numbers refer to `Class1.cs` unless another file is named. Coordinates are copied exactly as `Vector3(x, y, z)`; a heading is given where the code passes one.

This is a description of what the code does, not what the menu text or the in-game "Alan" messages claim. Where the two differ, the report says so.

---

## 0. Global architecture (read first)

- One `Script` (`Class1`) runs everything. Its constructor (1132) reads `scripts//DisableBusinesses.ini` → `[ArenaWarBusiness] MazeBankArena`. If true, it creates `Enableinteriors`, `AllStocks` and `SaveCar` with `new`, calls `Setup()`, hooks `OnTick`, then calls `LoadIniFile("scripts//ArenaWarBusiness//Main.ini")`. Because `Setup()` runs before `LoadIniFile`, all menus are built before config values are known.
- A mission is a set of shared fields, with no mission objects:
  - `MissionSetup` (bool): the gate for all mission ticks
  - `MissionId` (int): the mode
  - `Race` (int): the variant
  - `StartedRace`, `inRace`, `FinishedRace`
  - `TimeLeft`, `timer`, `missiletimer`, `Timer2` (frame counters)
  - `moneytobeawarded` (float), `checkpointReaced` (wave, loot or checkpoint counter)
  - `lap`, `checkpoint`
  - opponents: `VtoGet`, `VtoGet1`…`VtoGet7`, with blips `VtoGetBlip`…`VtoGetBlip7`
  - player vehicles: `PLayerVehicle`, `PLayerVehicle2`
  - lists: `Peds`, `Props`, `ExtraV`, `Trap_*`
- Every timer counts frames, not real time. For example, `TimeLeft--` runs every 25 ticks (`timer` 0..24), so all durations depend on FPS.
- Many blocking `Script.Wait()` calls run inside the tick: the 3 s "Ready/Set/Go" countdowns, the 3000 ms waits between wave spawns, and `MissionScreen` (100 frames).
- Randomness uses `new Random()` on every call, seeded by tick count. Calls made in quick succession often give the same value; the code adds `Script.Wait(100)` between spawns to get around this.
- Player vehicle choice is almost always `getVehicle()` (random, 10003-10116). The menu offers no choice of vehicle.

### 0.1 Opponent and vehicle helpers

**`getVehicle()`** (9991): rolls `random.Next(1, 310)`. The ranges are:

| Roll | Vehicle |
|---|---|
| <10 | Dominator4 |
| 10–19 | Dominator5 |
| 20–29 | ZR380 |
| 30–39 | ZR3802 |
| 40–49 | ZR3803 |
| 50–59 | ZR3802 |
| 60–79 | Imperator |
| 80–89 | Imperator3 |
| 90–99 | Imperator2 |
| 100–109 | Impaler2 |
| 110–119 | Impaler3 |
| 120–129 | Impaler4 |
| 130–139 | Monster3 |
| **140 (unassigned, gap)** | keeps the previous value |
| 141–149 | Monster4 |
| 150–159 | Monster5 |
| 160–169 | Brutus |
| 170–179 | Issi4 |
| 180–189 | Deathbike |
| 190–199 | Scarab |
| 200–209 | RCBandito |
| 210–219 | Cerberus2 |
| 220–229 | Cerberus |
| 230–239 | Cerberus3 |
| 240–249 | Bruiser |
| 250–259 | Bruiser2 |
| 260–269 | Bruiser3 |
| 270–279 | Brutus2 |
| 280–289 | Brutus3 |
| 290–299 | Deathbike2 |
| 300–309 | Deathbike3 |

The result goes through `RequestModel` (a spin-wait until loaded; the model is never released).

**`getVehicle2()`** (10118): rolls `Next(1,150)`. The ranges are:

| Roll | Vehicle |
|---|---|
| <10 | Dominator4 |
| 10–19 | Dominator5 |
| 20–29 | ZR380 |
| 30–39 | ZR3802 |
| 40–49 | ZR3803 |
| 50–59 | ZR380 |
| 60–69 | Imperator |
| 70–79 | Imperator2 |
| 80–89 | Imperator3 |
| 90–99 | Impaler |
| **100–109 (dead branch `num >= 110 && num < 110`)** | keeps the previous value |
| 110–119 | Impaler3 |
| 120–129 | Imperator3 |
| 130–139 | Dominator4 |
| 140–149 | Dominator6 |

**`SetupRivalCar(int i, Vehicle V)`** (10266). It is called on almost every spawned vehicle, including the player's. Only `i == 0` is ever used:
- `MaxSpeed = 400`.
- `EngineTorqueMultiplier = random.Next(15,100)/10` and `EnginePowerMultiplier = random2.Next(15,100)/10`. This is integer division, so the values are whole numbers from 1 to 9. The two `Random`s are created back to back and usually give the same value.
- It then calls `SetupModifications(V)`.
- Presets `i = 1..6` (fixed colours, liveries, multipliers) exist but are never used.

**`SetupModifications(V)`** (10185):
- `SET_VEHICLE_MOD_KIT(0)`.
- Random mods: Exhaust 0–3, Grille 0–2, Frame 0–2, EngineBlock 0–3, ArchCover 0–2, Tank 0–1, Struts 0–2, AirFilter 0–2, Livery 0–16.
- `SET_VEHICLE_MOD(44, Next(0,1) = always 0)`.
- Random primary and secondary colour (`GetColor`, which never picks index 0).
- Xenon on, lights on, high beams on.
- `SET_VEHICLE_XENON_LIGHT_COLOR_INDEX` (hash 0xE41033B25D003A07, called as decimal `16433691881432431111`) with a random 1–12. The branch for 255 is unreachable, because `Next(1,13)` never returns 13.

**`UpgradeVehicle2`** (9879) sets Brakes, Suspension, Transmission and Engine to 3. **`UpgradeVehicle3`** (9887) does the same, plus mod kit 0 and random colours.

**AI driver setup**, copied into dozens of places:
```
TASK_SET_BLOCKING_OF_NON_TEMPORARY_EVENTS(ped, true)  // 0x90D2156198831D69
SET_PED_FLEE_ATTRIBUTES(ped, 0, false)                 // 0x70A2D1137C8ED7C9
SET_PED_COMBAT_ATTRIBUTES(ped, 17, true)               // 0x9F7794730795E019 – 17 = BF_AlwaysFlee (!)
SET_BLOCKING_OF_NON_TEMPORARY_EVENTS(ped, true)        // 0x9F8AA94D6D97DBF4
SET_PED_FLEE_ATTRIBUTES(ped, 0, false); SET_PED_COMBAT_ATTRIBUTES(ped, 17, true)  // repeated
```
After this the ped gets `Task.VehicleChase(player)`, `Task.FightAgainst(player)`, or a per-tick `TASK_VEHICLE_MISSION` (0x659427E0EF36BCDE) with missionType 7. In the nativedb VehMissionType enum, 7 is MISSION_FOLLOW, not attack. That call uses cruise speed `playerVeh.Speed*4+30`, drivingStyle 2883621 and targetReached 200. It passes only 7 of the native's 9 arguments.

**Spawn corners used by most arena deathmatch-style modes** (`SetupDeathMatchCars`, 10368, and `SetupDeathMatchCars2`, `3`, `4`):

| Slot | Position | Rotation Z |
|---|---|---|
| A | `Vector3(2736f, -3892f, 140f)` | 0 |
| B | `Vector3(2866f, -3892f, 140f)` | 0 |
| C | `Vector3(2865f, -3707f, 140f)` | 180 |
| D | `Vector3(2732f, -3707f, 140f)` | 180 |

Each is created with heading 10, then has its rotation set.
- `SetupDeathMatchCars`: `VtoGet..VtoGet3`, `getVehicle()`, `SetupRivalCar(0)`, random ped driver, `VehicleChase(player)`, GunCar blip with colour 8.
- `SetupDeathMatchCars2` (11198): the same at the **same four positions**, into `VtoGet4..VtoGet7`.
- `SetupDeathMatchCars3` (11514): **Panto** x4 into `VtoGet4..7`.
- `SetupDeathMatchCars4` (11626): **"RCBandito"** x4 into `VtoGet..VtoGet3`, with drivers on `Task.FightAgainst(player)`.

On-foot peds in `Peds` get weapon hash `1198256469` (WEAPON_RAYCARBINE, "Unholy Hellbringer", via GIVE_WEAPON_TO_PED with 999999 ammo) and `FightAgainst(player)` every tick. This happens in MissionIds 2, 5, 15, 16, 26 and 27.

### 0.2 Common mission start (arena modes)
Almost every arena menu item does the following:
1. Deletes `VtoGet` (only that one).
2. Teleports the player to `Vector3(2782.99f, -3898.15f, 140.001f)`.
3. Calls a `Load<Theme>_<N>()`.
4. Sets `RaceMarker = Vector3(2640f, -3801f, 140f)` for most modes.
5. `Script.Wait(200)`.
6. Creates `PLayerVehicle = getVehicle()` at `Vector3(2782.99f, -3898.15f, 140.001f)` with heading 148, warps the player in, and calls `SetupRivalCar(0, PLayerVehicle)`.
7. Creates a white RaceFinish blip "Start Marker" at `RaceMarker`.
8. Sets `MissionSetup = true`, `MissionId`, `Race`, `StartedRace = false`, `TimeLeft = 40`, `checkpointReaced = 0`, `moneytobeawarded = 0`, `vehicleHealth = 600`, and `Game.Player.Character.IsInvincible = true`.
9. Clears `IsinInterior`, `IsInInterior`, `isInGarage` and `isinsecondVehicleBay`.

The mode starts when the player drives within 7 m of `RaceMarker`. A red `VerticalCylinder` is drawn there, scale (7,7,1).

### 0.3 Common end condition ("death or leave" payout)
In almost every arena mode the run ends only in one of two ways:
- **The player's vehicle is dead** (`!CurrentVehicle.IsAlive`). The mod pays `Game.Player.Money += (int)moneytobeawarded`, removes blips, deletes opponents, warps the player out, explodes and deletes the vehicle, teleports the player to `Vector3(2782.99f, -3898.15f, 140.001f)` (Sky Wars uses `MenuMarker`), sets `StartedRace = false`, `MissionSetup = false`, `Race = 0`, waits 400 ms, sets `IsInvincible = false`, and deletes the on-foot peds in `Peds`.
- **The player is out of a vehicle.** The payout is identical, without the explode or teleport, and the mod shows "Mission Complete".

There is **no losing**: dying pays out whatever has been earned. `MissionId` is usually **not** reset, only `MissionSetup`. Because the player is invincible, the run normally ends through the vehicle.

---

## 1. Game modes – full catalogue

### Menu tree
The main menu (`mainMenu`, "Arena War ") opens at `MenuMarker = Vector3(205f, 5163f, -87f)` in the workshop (22601). Its submenus are created at 1378-1390:

| Top-level submenu | Builder (line) | MissionId(s) |
|---|---|---|
| Purchase Options / Product Options | business (9133, 9441) | – |
| "Arena War Races (Missions)" → "Races" | `SetupRaces` 2628 | 3 |
| "Special Missions (Missions)" → "Special Missions" | `SetupSpecialMissions` 5152 | 8, 6, 7, 11, 18, 25 |
| "Point to Point (Missions)" | `SetupSpecialMissions2` 4856 | 10 |
| "Vehicle Death Matches (Missions)" | `SetupSpecialMissions3` 4167 | 2 (4x), 5 (8x) |
| "Championchip Matches (Missions)" | `ChampionChipMatch` 1963 | 26 |
| "Sky Wars Death Matches (Missions)" | `SetupSkyWars` 1579 | 27 |
| "Turret Mayhem Matches (Missions)" | `SetupTurretMayhem` 1445 | 28 |
| "RC King (Missions)" | `SetupSpecialMissions4` 3850 | 4 |
| "Hot Ring (Missions)" | `SetupSpecialMissions5` 3778 | 13 |
| "Smash And Grab (Missions)" | `SetupSpecialMissions6` 3678 | 15 |
| "Demoltion Men (Missions)" | `SetupSpecialMissions8` 3212 | 20 |

Code that exists but cannot be reached:
- **Beast Wars** (`SetupSpecialMissions7`, 3586, MissionId 16): never called from `Setup()`. Its parent `BeastWars` UIMenu is never created.
- **MissionId 14** ("Panto" vehicle soccer, 15660): no menu sets it.
- **MissionId 3 Race 1** (arena circuit race, 12571): no menu sets `Race = 1` for MissionId 3.

### 1.1 Arena War Race – MissionId 3, Race 5 ("Arena War Race (3 Lap, RaceTrack)")
- **Menu** (2635-2658):
  - `RaceMarker = Vector3(1099f, 163f, 81f)` (Vinewood Racetrack) and blip "Arena War Race".
  - The player is teleported to the marker, and `PLayerVehicle = getVehicle()` is spawned at `Vector3(1108f, 188f, 81f)` with heading 148.
  - It does not set invincibility, does not close menus, and does not reset `lap` or `checkpoint`.
- **Start** (12808-12915), within 7 m of `RaceMarker`:
  - Player car → `Vector3(1108f,188f,81f)`, rotation 148, frozen.
  - Three AI cars, each `getVehicle()` with heading 148:
    - `VehicleA` at `Vector3(1117f,184f,81f)`
    - `VehicleB` at `Vector3(1114f,198f,81f)`
    - `VehicleC` at `Vector3(1123f,194f,81f)`
  - Each AI car gets a random ped, `UpgradeVehicle2`, mod kit, `SetupRivalCar(0)`, rotation 143 and the AI flags.
  - A blocking countdown ("Ready!" / "Set! " / "Go!", 1 s each) runs, then `CircuitRace = true`.
- **AI route** (12439-12503, `ChecktoSeeifinthelead` 9898): `DriveTo` with radius 20 and speed 420 through `(1005,-72,81)` → `(1165,24,81)` → `(1253,267,82)` → `(1101,166,81)` → back. It advances within 60 m of each point and increments `Alap`, `Blap` or `Clap`. An AI within 20 m of the player switches to `FightAgainst(player)`; otherwise its `DriveTo` is re-issued **every tick**.
- **Player checkpoints** (12926-12958): `(1005,-72,80)` → `(1165,24,80)` → `(1253,267,80)` → `(1138,236,81)` (lap++), each with a 20 m trigger. The finish is within 10 m of `(1138,236,81)` with `lap == 3`.
- **Payout** (12959-13024):
  - If "leading", +$450,000.
  - Else, if any AI lap counter is 3, $0.
  - **Else, +$4,500,000.**
  - "Leading" is a distance comparison to `FinishLine`. **`FinishLine` is never assigned**, so it is `(0,0,0)`, and the comparison is meaningless.
- **Fail or leave:** not handled. Leaving the car does nothing, and the race continues.

### 1.2 Arena circuit race – MissionId 3, Race 1 (unreachable)
- Start marker `Racemarker2 = Vector3(2800f, -3800f, 140f)`.
- AI at `(2795,-3886,140)`, then moved to `(2780,-3886,140)`, `(2796,-3876,140)` and `(2780,-3876,140)`. They are forced into the arena room with `FORCE_ROOM_FOR_ENTITY` (0x52923C4710DD9907).
- Checkpoints: `(2882,-3877,140)` → `(2883,-3723,140)` → `(2721,-3722,140)` → `(2800,-3880,140)`. The AI route (`CircuitRace2`) follows the same points.
- Payout is the same as 1.1. The B/C AI logic uses `ACheck` instead of `BCheck`/`CCheck` (copy-paste error, 12528-12568).

### 1.3 Point to Point – MissionId 10 (menu 4873-5111, tick 13030-14006)
"Drive through the checkpoints to earn cash, while a timer counts to 0 … avoid the mines".

| Menu text | Theme loaded | Race | Start TimeLeft | Checkpoint pool | $ per checkpoint | +time per checkpoint |
|---|---|---|---|---|---|---|
| 40 Second Start (Dystopian 2) | LoadDystopian_2 | 4 | 40 | RandomLocationFun2 | 12,500 | Next(3,12) → 3–11 |
| 50 Second Start (Dystopian 3) | LoadDystopian_3 | 8 | 50 | RandomLocationFun5 | 16,500 | Next(5,14) → 5–13 |
| 40 Second Start (Scifi 2) | LoadScifi_2 | 6 | 40 | RandomLocationFun3 | 15,000 | Next(3,8) → 3–7 |
| 50 Second Start (Scifi 9) | LoadScifi_9 | 9 | 50 | RandomLocationFun6 | 18,500 | Next(4,10) → 4–9 |
| 40 Second Start (Wasteland 2) | LoadWasteland_2 | 7 | 40 | RandomLocationFun4 | 18,000 | Next(3,12) → 3–11 |
| 50 Second Start (Wasteland 9) | LoadWasteland_9 | 10 | 50 | RandomLocationFun7 | 20,500 | Next(4,8) → 4–7 |

- **Start:** within 7 m of `Vector3(2731f, -3882f, 140f)`. The first `CheckPointPosition` is picked from the pool and the "Checkpoint" blip moves there.
- **Loop:**
  - A red cylinder (7,7,7) is drawn at the checkpoint, with help text "Time Left / Money Earned / Checkpoints reached".
  - Reaching the checkpoint within 10 m picks a new one, adds the money and time, and calls `ChangeMIneLoc`.
  - `TimeLeft--` every 25 frames.
  - `Firemissile()` every 46 frames. This is not gated by `UseMIssilesInVDM`. It fires 3× `WEAPON_AIRSTRIKE_ROCKET` at the player from one of the 6 roof points (section 3.3).
- **Mines:** the six invisible `MineLoc1..6` (within 12 m) call `FireMine`. See section 3.4: in practice the mines are never placed.
- **End:**
  - When `TimeLeft` reaches 0, pay `moneytobeawarded`, explode and delete the vehicle, and teleport to the arena start.
  - If the vehicle dies, the same.
  - If the player leaves the vehicle, they are paid ("you abandoned your vehicle").
  - There is no fail state. `MissionId` is not reset.
- **Checkpoint pools** (8375-8775). Rolls that are exact multiples of 10 fall into gaps and leave the previous value unchanged.

| Pool | Points |
|---|---|
| Fun2 | `(2774,-3779,140)`, `(2899,-3749,140)`, `(2962,-3790,140)`, `(2874,-3866,138)`, `(2850,-3852,139)`, `(2676,-3795,143)`, `(2670,-3745,140)`, `(2738,-3784,140)`, `(2820,-3727,140)`, `(2887,-3811,140)`, `(2684,-3871,140)` |
| Fun3 | `(2684,-3853,140)`, `(2682,-3796,140)`, `(2728,-3758,140)`, `(2799,-3799,140)`, `(2870,-3841,140)`, `(2895,-3825,140)`, `(2918,-3803,140)`, `(2917,-3749,140)`, `(2800,-3758,127)`, `(2757,-3800,127)`, `(2801,-3836,127)` |
| Fun4 | `(2733,-3803,127)`, `(2790,-3754,127)`, `(2855,-3790,127)`, `(2886,-3878,140)`, `(2886,-3826,140)`, `(2909,-3755,140)`, `(2762,-3779,141)`, `(2643,-3821,140)`, `(2784,-3728,140)`, `(2906,-3752,140)`, `(2941,-3786,140)` |
| Fun5 | `(2738,-3853,140)`, `(2745,-3808,140)`, `(2752,-3742,137)`, `(2799,-3825,147)`, `(2799,-3749,147)`, `(2875,-3761,140)`, `(2892,-3821,140)`, `(2837,-3795,157)`, `(2652,-3763,142)`, `(2772,-3797,157)`, `(2923,-3727,139)`, `(2937,-3837,141)`, `(2675,-3796,144)`, `(2926,-3796,144)` |
| Fun6 | `(2931,-3828,139)`, `(2932,-3771,139)`, `(2901,-3799,139)`, `(2880,-3800,139)`, `(2842,-3798,139)`, `(2800,-3800,139)`, `(2761,-3800,139)`, `(2720,-3799,139)`, `(2668,-3772,139)`, `(2667,-3828,139)`, `(2822,-3854,147)`, `(2776,-3854,147)`, `(2823,-3745,147)`, `(2777,-3745,147)`, `(2757,-3800,126)` |
| Fun7 | `(2714,-3871,139)`, `(2705,-3803,137)`, `(2759,-3745,139)`, `(2834,-3782,146)`, `(2879,-3796,136)`, `(2803,-3800,150)`, `(2841,-3739,148)`, `(2801,-3847,149)`, `(2747,-3753,139)`, `(2855,-3744,139)`, `(2942,-3835,139)`, `(2887,-3849,139)`, `(2748,-3862,139)`, `(2695,-3744,147)`, `(2757,-3800,126)` |
| Fun8 (Smash & Grab) | `(2748,-3754,134)`, `(2797,-3751,134)`, `(2845,-3773,134)`, `(2840,-3845,134)`, `(2850,-3852,134)`, `(2779,-3830,133)`, `(2722,-3807,133)`, `(2759,-3790,132)`, `(2777,-3810,132)`, `(2816,-3812,132)`, `(2817,-3785,132)` |

### 1.4 Vehicle Death Match – MissionId 2 (4x) / MissionId 5 (8x), Race 1 (menu 4203-4853, tick 14007-14795)
"Destroy each vehicle to progress to the next wave … each wave vehicles are harder to destroy". The second claim is false; see the bugs section.

| Menu text | Theme loaded | MissionId |
|---|---|---|
| Death Match (Scifi 2) 4x Opp | LoadScifi_2 | 2 |
| Death Match (Scifi 5) 4x Opp | LoadScifi_5 | 2 |
| Death Match (Scifi 10) 4x Opp | LoadScifi_10 | 2 |
| Death Match (Dystopian 5) 4x Opp | LoadDystopian_5 | 2 |
| Death Match (Dystopian 10) 4x Opp | LoadDystopian_10 | 2 |
| Death Match (Wasteland 5) 4x Opp | LoadWasteland_5 | 2 |
| Death Match (Dystopian 7) 4x Opp | **LoadDystopian_8** (4362) | 2 |
| Death Match (Dystopian 12) 4x Opp | LoadDystopian_12 | 2 |
| Death Match (Scifi 2) 8x Opp | LoadScifi_2 | 5 |
| Death Match (Scifi 5) 8x Opp | LoadScifi_5 | 5 |
| Death Match (Scifi 10) 8x Opp | LoadScifi_10 | 5 |
| Death Match (Dystopian 5) 8x Opp | LoadDystopian_5 | 5 |
| Death Match (Dystopian 10) 8x Opp | LoadDystopian_10 | 5 |
| Death Match (Wasteland 5) 8x Opp | LoadWasteland_5 | 5 |
| Death Match (Dystopian 7) 8x Opp | **LoadDystopian_8** (4284) | 5 |
| Death Match (Dystopian 12) 8x Opp | LoadDystopian_12 | 5 |

- **Start:** within 7 m of `RaceMarker (2640,-3801,140)`.
  - 4x: `SetupDeathMatchCars()`, then `checkpointReaced = 1`.
  - 8x: `SetupDeathMatchCars()`, `Script.Wait(3000)`, then `SetupDeathMatchCars2()`, which spawns at the same four corners.
- **Waves:** when all 4 (or all 8) opponents are dead, everything is deleted and respawned, `checkpointReaced++`, and the mod adds **$100,000** (4x) or **$200,000** (8x). `vehicleHealth += 100` is never applied to anything.
- **Missiles** (only if `UseMIssilesInVDM`): `Firemissile4_1()` every 151 frames (4x) or 191 frames (8x). It fires one airstrike rocket at a random target among the player, `VtoGet`, `VtoGet1` and `VtoGet2`; `VtoGet3` can never be picked (bug 6.4).
- **MissionId 5 extra** (14415-14434): every tick, any `VtoGetN` whose driver is `null` is exploded and notified ("Vehilce B has been Eliminated…", mislabelled for C and D). It is unclear whether this ever fires: SHVDN2 `GetPedOnSeat` returns a wrapper, not `null`.
- **Mines:** MissionId 2 is the only mode where `ChangeMIneLoc` can relocate mines (pool Fun3), but they only relocate after one has already been hit at `(0,0,0)`. See section 3.4.
- **End:** see section 0.3. Help text: "Current Wave :N Cash Earned : $X".

### 1.5 Championship Matches – MissionId 26, Race 1 (menu 1983-2365, tick 18017-18297)

| Menu text | Theme loaded |
|---|---|
| Death Match (Scifi 2) 4x Opp | LoadScifi_2 |
| Death Match (Scifi 5) 4x Opp | LoadScifi_5 |
| Death Match (Scifi 10) 4x Opp | LoadScifi_10 |
| Death Match (Dystopian 5) 4x Opp | LoadDystopian_5 |
| Death Match (Dystopian 10) 4x Opp | LoadDystopian_10 |
| Death Match (Wasteland 5) 4x Opp | LoadWasteland_5 |
| Death Match (Dystopian 7) 4x Opp | **LoadDystopian_8** (2062) |
| Death Match (Dystopian 12) 4x Opp | LoadDystopian_12 |

- The menu deletes the existing `Props` and teleports the player before loading the theme. Some items show a debug "test1".
- **Start:** within 7 m of `RaceMarker`, `SetupDeathMatchCars()` (4 opponents).
- **Loop:**
  - Every tick, each opponent is re-tasked with `TASK_VEHICLE_MISSION(…, 7 /*FOLLOW*/, player speed*4+30, 2883621, 200)`.
  - A dead opponent is **respawned individually** by `SetupChampionchipCars(n)` (10706) at its corner. Each respawn adds **$75,000** and `checkpointReaced++`.
  - `SetupChampionchipCars` has branches only for 1, 2 and 3; **there is no `I == 4` branch** (see bugs).
  - Deathbike riders get `CanBeKnockedOffBike = false`.
  - Missiles: `Firemissile4_4()` every 151 frames if `UseMIssilesInVDM`. It fires 4 rockets scattered 5–20 m around a random target.
- **End:** see section 0.3; all `Props` are also deleted. There is **no** championship progression: no rounds, scores, standings or persistence. The run is an endless wave match.

### 1.6 Sky Wars – MissionId 27, Race 1 (4x) / Race 2 (8x) (menu 1587-1960, tick 18298-19130)
- **Menu:** "Death Match (Dystopian A) 4x Opp" (Race 1) and "… 8x Opp" (Race 2). The arena IPL is **not** used. The arena is built in the sky above downtown:
  - Floor prop `xs_terrain_set_dystopian_10` at `Vector3(-58.9f, -861f, 1000f)` and turret prop `xs_prop_arena_turret_01a` at `Vector3(-42f, -871f, 1009f)`.
  - 27 trap slots at z 1001.5 are filled randomly (section 3.2).
  - The player car `getVehicle()` is placed at `Vector3(113f, -873f, 1005f)` with heading 85.
  - `TimeLeft = 40`, `vehicleHealth = 600`, invincible. The menu closes all menu pools.
- **Start:** immediate. There is no marker check: on the first tick, if the player is in a vehicle, `SetupSkyWarsCars()` runs (18674-18679).
- **Opponents:**
  - `SetupSkyWarsCars` (10480): `(-132,-774,1005)`, `(-148,-945,1005)`, `(14,-944,1005)`, `(21,-775,1005)`, all rotation −180.
  - 8x also calls `SetupSkyWarsCars2` (10593): `(-122,-774,1005)`, `(-138,-945,1005)`, `(24,-944,1005)`, `(31,-775,1005)`. These are 10 m from the first set. Race 2 calls Cars2 then Cars at 19124-19125.
  - All use `getVehicle()`, `SetupRivalCar(0)`, a random driver and `VehicleChase`, plus a per-tick `TASK_VEHICLE_MISSION` FOLLOW in the loop.
- **Respawn:** an opponent that is dead or below z 990 (off the platform) is exploded and respawned at its slot by `RespawnSkywarsCar(n)` (10833), worth **+$25,000** and `checkpointReaced++`. Slot 4 has two identical `if (I == 4)` blocks, so it spawns two vehicles and leaks one (10959/10998).
- **Traps:** active every tick (section 3.2).
- **Missiles:** `Firemissile4_8()` every 151 frames if `UseMIssilesInVDM`. It fires from the sky turret `Vector3(-42f,-871f,1014f)`.
- **End:** the player's vehicle is dead or below z 990, or the player is out of the vehicle. Pay out, delete all `Props`, and teleport to `MenuMarker (205,5163,-87)` (workshop).

### 1.7 Turret Mayhem – MissionId 28 (menu 1457-1576, tick 19131-19396)

| Menu text | Theme loaded | Race | Start marker |
|---|---|---|---|
| Turret Mayhem 1 (Wasteland 4) | LoadWasteland_4 | 1 | `(2773,-3853.73,130)` |
| Turret Mayhem 2 (Scifi 4) | LoadScifi_4 | 1 | `(2773,-3853.73,130)` |
| Turret Mayhem 3 (Dystopian 4) | LoadDystopian_4 | 2 | `(2790,-3879.2,140)` |
| Turret Mayhem 4 (Dystopian 8) | LoadDystopian_8 | 2 | `(2790,-3879.2,140)` |

- The player is teleported to the marker, but the car is spawned at `(2782.99,-3898.15,140)` with heading 148 and the player warped in, so the player must drive to the marker. The menu does not close and nothing is reset (`lap`, `checkpoint`).
- **Start:** a blocking 3 s countdown, then invincible. There are **no AI opponents**.
- **Turrets:** `Firemissile4_16()` runs **every frame** (section 3.3).
- **Laps:** each lap is four checkpoints with a 20 m trigger, drawn as a cylinder (8,8,3):
  - Race 1 (lower ring, z 130): `(2927,-3840,130)` → `(2869,-3747,130)` → `(2684,-3754,130)` → `(2773,-3853.73,130)`, then lap++.
  - Race 2 (z 140): `(2950.9,-3842.17,140)` → `(2884,-3724,140)` → `(2634,-3795,140)` → `(2790,-3879.2,140)`, then lap++.
- **End:** the vehicle dies or the player leaves it. Pay **$100,000 × lap**, set `MissionId = 0`, and teleport to the arena start. There is no time limit.

### 1.8 RC King – MissionId 4 (menu 3868-4164, tick 14257-14412)

| Menu text | Theme | Race | Start |
|---|---|---|---|
| RC King (Scifi 2) | LoadScifi_2 | 1 | `(2731,-3882,140)` |
| RC King (Dystopian 3) | LoadDystopian_3 | 1 | same |
| RC King (Wasteland 9) | LoadWasteland_9 | 1 | same |
| RC King (Dystopian 7) | LoadDystopian_7 | 1 | same |
| RC King (Scifi 9) | LoadScifi_9 | 1 | same |
| RC King (Wasteland 7) | LoadWasteland_7 | 1 | same |
| RC King (BMX Park) | none | 2 | `(-888,-802,14)` (outside the arena; the player and vehicle spawn there) |

- The player vehicle is always `"rcbandito"` with `SetupRivalCar(0)`, so it has random 1–9× engine multipliers. `TimeLeft = 80`.
- **Scoring:** every frame the RC is in the air, +$1,000. Every frame it is upside down, +$1,000. This depends on FPS and makes the mode an unbounded money source.
- **End:** `TimeLeft` 0 pays out, explodes the RC and teleports the player to the arena start (Race 1) or `(-888,-802,14)` (Race 2). Leaving the vehicle pays out (see bug 6.3, a crash). `MissionId = 0` is reset here.

### 1.9 Hot Ring – MissionId 13, Race 1 (menu 3786-3847, tick 15345-15659)

| Menu text | Theme |
|---|---|
| Hot Ring Circuit 1 (Wasteland 4) | LoadWasteland_4 |
| Hot Ring Circuit 2 (Scifi 4) | LoadScifi_4 |

- The start marker is `(2773,-3853.73,130)`; the car spawns at the arena start. The notification text is truncated: "…all the while avoiding the ".
- **Start:**
  - A 3 s countdown.
  - `SetupDeathMatchCars()` spawns 4 chasers with collision turned off for 0.5 s.
  - Invincible.
- **Loop:**
  - `Firemissile()` every 36 frames.
  - On a 1 % per-frame roll (`Next(1,1000) <= 10`), an "EMP mine" check runs against `TrapsPos` (within 10 m, section 3.1): it sets the vehicle invincible and undriveable, sets `Health = 10`, waits 100 ms, then sets `Health = 100` and makes it driveable again.
  - Wave respawn when all 4 are dead, with no reward.
- **Laps:** the same four lower-ring checkpoints as Turret Mayhem Race 1.
- **End:** the vehicle dies or the player leaves it; pay $100,000 × lap and set `MissionId = 0`.

### 1.10 Smash and Grab – MissionId 15 (menu 3686-3775, tick 15944-16654)

| Menu text | Theme | Race | Opponents |
|---|---|---|---|
| Smash and Grab 1, 4x Opp (Wasteland 5) | LoadWasteland_5 | 1 | 4 (`SetupDeathMatchCars`) |
| Smash and Grab 1, 8x Opp (Wasteland 5) | LoadWasteland_5 | 2 | 8 (+ `Wait(3000)` + `SetupDeathMatchCars2`) |

- The player is teleported to `(2773,-3853.73,130)` and the car is created at `(2782.99,-3898.15,140)`. The 8x item creates **two** player cars and leaks the first (3714-3715).
- The `FinishLineBLip` "Start Marker" is at `RaceMarker (2640,-3801,140)`.
- **Loop:**
  1. Drive to `Goal2 = Vector3(2926f, -3800f, 140f)` (within 12 m) to set `GotCargo`.
  2. Pass 6 checkpoints from `RandomLocationFun8` (within 12 m, the lower level at z 132–134).
  3. When `CheckPointstoProceed == 6`, deliver to `Goal1 = Vector3(2665f, -3800f, 140f)` for **+$250,000** and `checkpointReaced++`.
- Opponent waves respawn when all are dead, with no reward. Missiles: `Firemissile4_1()` every 61 frames if `UseMIssilesInVDM`.
- **End:** see section 0.3 (help text "Amount of Loot Retreived").

### 1.11 Demolition Men – MissionId 20 (menu 3230-3583, tick 17255-17771)

| Menu text | Theme | Race | Opponents |
|---|---|---|---|
| Demolition Men, 4x Opp (Dystopian 5) | LoadDystopian_5 | 1 | 4 RC Bandito (`SetupDeathMatchCars4`) |
| … (Dystopian 10) | LoadDystopian_10 | 1 | same |
| … (Scifi 5) | LoadScifi_5 | 1 | same |
| … (Scifi 10) | LoadScifi_10 | 1 | same |
| … (Wasteland 2) | LoadWasteland_2 | 1 | same |
| … (Wasteland 5) | LoadWasteland_5 | 1 | same |
| … (Random Track & Car) | random (3258-3290) | 2 | 4 random arena cars (`SetupDeathMatchCars`) |

- The intent is to "avoid the RC banditos … survive as long as possible".
- **Start:** within 7 m of `RaceMarker`. The opponents spawn and the player's `BodyHealth` is set to **100**.
- **Scoring:**
  - +$5,000 and `checkpointReaced++` every 37 frames survived.
  - **Any opponent within 5 m of the player explodes and gives +$75,000.** Ramming pays, which contradicts the brief.
  - All 4 gone: +$100,000 and a respawn.
- **Debug:** "Health : <BodyHealth>" is notified every frame (17468, 17725).
- **End:** see section 0.3.
- **Random-track roll** (`Next(1,80)`):

| Roll | Theme |
|---|---|
| <10 | Dystopian_10 |
| 11–19 | Dystopian_5 |
| 21–29 | Scifi_2 |
| 31–39 | Scifi_5 |
| 41–49 | Scifi_10 |
| 51–59 | Wasteland_5 |
| 61–69 | Wasteland_5 |
| 71–79 | Dystopian_12 |

  Rolls of exactly 10, 20, … 70 load nothing.

### 1.12 Special Missions (free-roam, MissionIds 8 / 6 / 7 / 11 / 18 / 25)
These are not arena modes. They mostly pay into `stocksvalue` (business stock), not cash. `MissionScreen.SetPass/SetFail` only *displays* a payout; it never pays.

**Rogue Contestant – MissionId 8** (5623-5654, tick 19579-19614)
- `VtoGet = getVehicle()` at `Randomlocation()`, heading 45. `RandomVehicleFun()` is called and its result ignored.
- A random driver and passenger; the driver `FleeFrom(player)`. GunCar blip "Find Vehicle". `SetupRivalCar(0)`. `Health = BodyHealth = 12000`.
- **Win:** `Health <= 1500` (help text "Vehicle Health N /1500") gives +$250,000 cash and the pass screen.
- **Fail:** the vehicle is destroyed (fail screen).
- `Randomlocation` points (8838):

  | Randomlocation points |
  |---|
  | `(-1069.32,-72.28,19)` |
  | `(-1579.93,-155.28,55)` |
  | `(-711.74,-28.28,37)` |
  | `(6,253.58,109)` |
  | `(703,32,84)` |
  | `(1197,-421.5,68)` |
  | `(1257,-1428,35)` |
  | `(1264,-2039,45)` |
  | `(527,-2052,28)` |
  | `(-161,-2087.8,26)` |
  | `(756,-1723,30)` (listed twice) |
  | `(-816,-2300,11)` |
  | `(-1839,136,78)` |

**The Rookie – MissionId 6** (5565-5622, tick 14796-14941)
- Glendale at `(-2037,-466,11)` with heading 11, Dominator at `(-2033,-457,10)` with heading 93, SlamVan at `(-2044,-459,11)` with heading 58. Only `VtoGet` gets `SetupRivalCar`.
- 11 random peds:

  | Ped spawns |
  |---|
  | `(-2042,-464,11)` |
  | `(-2033,-454,11)` |
  | `(-2034,-462,11)` |
  | `(-2024,-463,11)` |
  | `(-2034,-447,11)` |
  | `(-2044,-445,11)` |
  | `(-2053,-453,11)` |
  | `(-2035,-460,11)` |
  | `(-2021,-455,11)` |
  | `(-2027,-448,11)` |
  | `(-2013,-460,11)` |

- Bomb blip at `(-2040,-456,11)`. Within 40 m, every ped gets the AI flags, an SMG and `FightAgainst(player)`.
- **Win:** all 3 vehicles destroyed. `stocksvalue += 150000 + 100000*profitMargin/100`; the screen shows "$540,000".
- **Fail:** the player dies (the screen shows $1,400,000).

**The Oil Fued – MissionId 7** (5475-5564, tick 14942-15061)
- 3× "Cerberus" at `(-2173,-346,12)`, `(-217,260,91)` and `(1201,-379,58)`, each with an ArmGoon02GMY driver and the AI flags.
- `DriveTo(EndPointPos = (-382,-1835,20), radius 20, speed 120, style 1)`. Castle blip "Delivery Point".
- **Win:** all 3 dead. `stocksvalue += 1,500,000 + 1,000,000*profitMargin/100`, saved; the screen shows $1,450,000.
- **Fail:** any within 20 m of the end point. There is no player-death handling.

**The Air Tanker – MissionId 11** (5353-5474, tick 15062-15344)
- 3 invincible Skylifts:
  - `(-2499,3155,33)`, rotation −169
  - `(2026,4748,41)`, rotation 172
  - `(1094,3044,40)`, rotation 172
- Each carries an invincible "Scarab" (`UpgradeVehicle3`, `SetupRivalCar`) attached with `ATTACH_ENTITY_TO_ENTITY` (0x6B9BBD38AB0796DF) at bone 0, offset (0,−3,−1).
- The Skylift drivers (ArmGoon02GMY) are tasked to `FightAgainst` an invisible, frozen, invincible random ped at `EndPointPos`. This is a hack to make the helicopters fly there.
- Within 1300 m the vehicles become vulnerable and visible. Within 300 m the Scarab is detached, gets a random driver and `DriveTo(EndPointPos, 20, 120, 1)`.
- **Win:** all 6 vehicles dead (stocks +1.5M + …).
- **Fail:** any of the 6 within 20 m of the end point.

**The Beast – MissionId 18** (5294-5352, tick 17224-17254)
- `VtoGet = getVehicle2()` at `Randomlocation()`, towing a `TrailerSmall2` (the AA trailer) spawned at a *different* `Randomlocation()` and then `AttachTo`.
- The car driver gets the AI flags; both drivers get `FightAgainst(player)`. Roof mod 0. Health and BodyHealth 1300 on both.
- **Win:** the car is dead; the trailer is exploded. +$450,000 cash and the pass screen. There is no fail. The text "with the cops" is not implemented: no wanted level is set.

**Traning Day – MissionId 25** (5183-5293, tick 17772-18016)
- Two racers `getVehicle2()` at `(1891,2786,44)` and `(1895,2789,44)`, rotation 33, each with a `CreatePed` FreemodeMale01 in a fixed outfit (5115-5150), `UpgradeVehicle3` and `SetupModifications`. The fallback is ZR3802 or Imperator2.
- Gantry `stt_prop_race_gantry_01` at `(1886,2804,43)`, rotation −66. Pink RaceArenaWar blip at `(1882,2801,43)` with a route.
- 12 parked random arena cars (`getVehicle`, `SetupRivalCar`, random heading) into `ExtraV`:

  | Parked-car spawns |
  |---|
  | `(1841,2769.2,46)` |
  | `(1844,2751,46)` |
  | `(1830,2767,46)` |
  | `(1879,2831,46)` |
  | `(1897,2830,46)` |
  | `(1907,2812,46)` |
  | `(1959,2725,46)` |
  | `(1944,2739,46)` |
  | `(1727,2800,46)` |
  | `(1715,2793,46)` |
  | `(1692,2809,45)` |
  | `(1689,2822,45)` |

- **Racer loop:** `(1882,2801,43)` → `(1802,2873,45)` → `(1719,2821,45)` → `(1825,2823,45)` → `(1951,2674,45)` → back, `DriveTo(radius 30, speed 100, style 2883621)`. The waypoint state is stored in **vehicle Alpha** (255 → 251).
- **Escape:** the player shooting within 500 m sets `TriggerEscape`, and the racers flee (re-tasked every tick within 750 m).
- **Fail:** either racer alive and more than 750 m away after escape triggers.
- **Win:** both racers dead. `stocksvalue += 100000 × (destroyed ExtraV) + 2,200,000 + 1,000,000*profitMargin/100`.

### 1.13 Unreachable modes (documented for completeness)

**Beast Wars – MissionId 16** (3586-3676, tick 16655-17223)
- The player gets `getVehicle2()` plus an attached `TrailerSmall2` with an invincible, untargetable "RESPECT_PLAYER" gunner (`Friendlyped`, `GET_HASH_KEY`/`SET_PED_RELATIONSHIP_GROUP_HASH`). It re-targets `ChooseTarget()` every 121 frames.
- Both player vehicles get BodyHealth 2000.
- Opponents: `SetupBeastWarsCars` (11329), 4 cars plus AA trailers with invincible gunners at the 4 corners.
- If a car dies, its trailer is exploded.
- The tick contains two `if (Race == 1)` blocks.

**Vehicle Soccer – MissionId 14** (15660-15943)
- Pantos (`SetupDeathMatchCars3`, stored in `VtoGet4..7`). The tick, however, checks `VtoGet..VtoGet3` against `Goal1` and `Goal2` (within 40 m): explode and +$100,000.

---

## 2. The arena interior

### 2.1 Natives seen (resolved against alloc8or nativedb)

| Hash | Name |
|---|---|
| 0x41B4893843BBDB74 | REQUEST_IPL |
| 0xEE6C5AD3ECE0A82D | REMOVE_IPL |
| 0xB0F7F8663821D9C3 | GET_INTERIOR_AT_COORDS |
| 0x55E86AF2712B36A1 | ACTIVATE_INTERIOR_ENTITY_SET |
| 0x420BD37289EEE162 | DEACTIVATE_INTERIOR_ENTITY_SET |
| 0x41F37C3427C75AE0 | REFRESH_INTERIOR |
| 0x52923C4710DD9907 | FORCE_ROOM_FOR_ENTITY |
| 0x659427E0EF36BCDE | TASK_VEHICLE_MISSION |
| 0x6B9BBD38AB0796DF | ATTACH_ENTITY_TO_ENTITY |
| 0x90D2156198831D69 | TASK_SET_BLOCKING_OF_NON_TEMPORARY_EVENTS |
| 0x9F8AA94D6D97DBF4 | SET_BLOCKING_OF_NON_TEMPORARY_EVENTS |
| 0x70A2D1137C8ED7C9 | SET_PED_FLEE_ATTRIBUTES |
| 0x9F7794730795E019 | SET_PED_COMBAT_ATTRIBUTES (attr 17 = BF_AlwaysFlee) |
| 0xC80A74AC829DDD92 | SET_PED_RELATIONSHIP_GROUP_HASH |
| 0xD24D37CC275948CC | GET_HASH_KEY |
| 0xBF0FD6E56C964FCB | GIVE_WEAPON_TO_PED |
| 0x36E353271F0E90EE / 0x5443438F033E29C3 | HAS_WEAPON_ASSET_LOADED / REQUEST_WEAPON_ASSET |
| 0x262B14F48D29DE80 | SET_PED_COMPONENT_VARIATION |
| 0x1F2AA07F00B3217A | SET_VEHICLE_MOD_KIT |
| 0x6AF0636DDEDCB6DD | SET_VEHICLE_MOD |
| 0xE41033B25D003A07 (decimal 16433691881432431111) | SET_VEHICLE_XENON_LIGHT_COLOR_INDEX |
| 0x3DFF319A831E0CDB | GET_VEHICLE_XENON_LIGHT_COLOR_INDEX |
| 0xDF735600A4696DAF / 0x03D7FB09E75D6B7E | SET_BLIP_SPRITE / SET_BLIP_COLOUR |
| 0x8509B634FBE7DA11 / 0x6C188BE134E074AA / 0x238FFE5C7B0498A6 | BEGIN_TEXT_COMMAND_DISPLAY_HELP / ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME / END_TEXT_COMMAND_DISPLAY_HELP |
| 0x142A02425FF02BD9 | TASK_START_SCENARIO_IN_PLACE |
| 0xC8CA9670B9D83B3B | ADVANCE_CLOCK_TIME_TO |
| 0x8FE22675A5A45817 / 0x9C720776DAA43E7E | CLEAR_PED_BLOOD_DAMAGE / CLEAR_PED_WETNESS |
| 0xB4EC2312F4E5B1F1 / 0x6D0858B8EDFD2B7D | SET_GAMEPLAY_CAM_RELATIVE_HEADING / _PITCH |
| 0x837765A25378F0BB | GET_GAMEPLAY_CAM_ROT (called with 0 args) |
| 0x060D935D3981A275, 0x428BDCB9DA58DA53, 0xFB71170B7E76ACBA, 0x9F47B058362C84B5 | DOES_VEHICLE_HAVE_CREW_EMBLEM, ADD_VEHICLE_CREW_EMBLEM, GET_ENTITY_BONE_INDEX_BY_NAME, GET_ENTITY_MODEL (VehiclesEmblem.cs) |
| 0xD966D51AA5B28BB9, 0x78F0424C34306220, 0x50969B9B89ED5738, 0x5CEE3DF569CECAB0 | GIVE_WEAPON_COMPONENT_TO_PED, ADD_AMMO_TO_PED, SET_PED_WEAPON_TINT_INDEX, DOES_WEAPON_TAKE_WEAPON_COMPONENT (GunLocker.cs) |
| 0x25223CA6B4D20B7F, 0x13D2B8ADD79640F2, 0x3D10BC92A4DB1D35, 0xBBC72712E80257A1, 0x961777E64BDAF717 | GET_CLOCK_HOURS/MINUTES/DAY_OF_MONTH/MONTH/YEAR (AllStocks.cs) |
| **0x31D23FC8CCD18DC3** (decimal 3590001984630001091) | **Not in nativedb.** It is used in Enableinteriors.cs 425-445 as `(interior, entitySetName, int colour)`, a signature that matches SET_INTERIOR_ENTITY_SET_TINT_INDEX (real hash 0xC1F1920BAF281317). I could not confirm what this hash is; it may be a wrong or legacy hash. |

### 2.2 IPLs

| Where | Calls |
|---|---|
| `Class1.Setup()` 1413-1420 | REMOVE then REQUEST: `xs_arena_interior`, `xs_arena_interior_mod`, `xs_Arena_Interior_Mod_2`, `xs_arena_interior_vip` |
| `Enableinteriors()` ctor 61-68, `ReloadInterior()` 448-459 | REMOVE then REQUEST: `xs_arena_interior`, `xs_arena_interior_mod`, `xs_arena_interior_vip`, `xs_arena_interior_Mod_2`; then `Enableinterior()` |
| `Enableinterior()` 433 | REQUEST `xs_arena_interior_mod_2` |
| Every `Load*` | REQUEST `xs_arena_interior`; **REMOVE `xs_arena_interior_vip`** |
| VIP entry (22258, 22266) | REQUEST `xs_arena_interior_vip`. VIP exit (22279) removes it. |
| Business entry and exit (22569, 22592, 22621) | REQUEST `Design` (from `Main.ini [Design] InteriorDesign`, **absent from the shipped Main.ini**, so null), then GET_INTERIOR_AT_COORDS(-75.8466, -826.9893, 243.3859) (Maze Bank office) and REFRESH. This is leftover code from the Executive office mod. |
| Garage A (21467-21468, dead code; `EnterMarkerA` is never set) | REMOVE `imp_dt1_11_cargarage_b`, `imp_dt1_11_cargarage_c` |

`EI.ReloadInterior()` (which removes and re-requests all four arena IPLs) is called on **every teleport**: arena, VIP, garages and stairs (22114, 22124, 22186, …).

### 2.3 Interior handles / coords

| Coordinate | Used for |
|---|---|
| `GET_INTERIOR_AT_COORDS(2782.99f, -3898.15f, 140f)` | Arena bowl; all `Load*` calls, FORCE_ROOM (12635) and the spectator branch (22262) |
| `GET_INTERIOR_AT_COORDS(2799.529, -3930.539, 182.235)` | Setup() 1421: VIP area (the jackpot and VIP exit are here) |
| `GET_INTERIOR_AT_COORDS(205f, 5180f, -90f)` | Workshop (`xs_arena_interior_mod`), Enableinteriors 389 |
| `GET_INTERIOR_AT_COORDS(170f, 5190f, 10f)` | Workshop B1 / vehicle garage (`xs_arena_interior_mod_2`), Enableinteriors 434 |

### 2.4 `NStadium` (SetupStadium, 987-1062)
This is the list of every arena-bowl entity set the script knows about. Its only use is to **deactivate them all** at the start of every `Load*`, so that one theme can then be turned on. Contents, in order:
- **Pit fences:** `Set_Pit_Fence_Closed`, `Set_Pit_Fence_Demolition`, `Set_Pit_Fence_Oval`, `set_pit_fence_ovala`, `set_pit_fence_ovalb`, `Set_Pit_Fence_Wall`, `set_wall_no_pit`
- **Centrelines:** `set_centreline_dystopian_05`, `set_centreline_scifi_05`, `Set_CentreLine_Wasteland_05`
- **Turrets:** `Set_Turrets`, `set_turrets_scifi`, `set_turrets_wasteland`
- **Team bands:** `Set_Team_Band_A`, `Set_Team_Band_B`, `Set_Team_Band_C`, `Set_Team_Band_D`
- **Lighting:** `Set_Lights_atlantis`, `Set_Lights_evening`, `Set_Lights_hell`, `Set_Lights_midday`, `Set_Lights_morning`, `Set_Lights_night`, `set_lights_sfnight`, `Set_Lights_saccharine`, `Set_Lights_sandstorm`, `Set_Lights_storm`, `Set_Lights_toxic`
- **Maps:** `Set_Dystopian_01` … `Set_Dystopian_17`, `Set_Scifi_01` … `Set_Scifi_10`, `Set_Wasteland_01` … `Set_Wasteland_10`
- **Scenes:** `Set_Dystopian_Scene`, `Set_Scifi_Scene`, `Set_Wasteland_Scene`
- **Crowds:** `Set_Crowd_A`, `Set_Crowd_B`, `Set_Crowd_C`, `Set_Crowd_D`

The `Load*` functions activate names in different letter case (for example `Set_Crowd_a`, `Set_Team_band_a`, `Set_Turrets_scifi`). Entity-set names are hashed case-insensitively by the game as far as I know, so this is probably harmless. **No `Set_Lights_*` set is ever re-activated after the deactivation loop.**

### 2.5 `Load*` functions (19815-20415)

**Template shared by all 18 functions:**
```
UI.Notify("test"); REQUEST_IPL("xs_arena_interior"); REMOVE_IPL("xs_arena_interior_vip"); UI.Notify("test1");
int = GET_INTERIOR_AT_COORDS(2782.99, -3898.15, 140)
[Dystopian only: DEACTIVATE one specific set, see below]
REFRESH_INTERIOR(int)                       // <- before any of the changes below
for each NStadium name: DEACTIVATE
UI.Notify("test2")
ACTIVATE: Set_Crowd_a, Set_Crowd_b, Set_Crowd_c, Set_Crowd_d,
          Set_Team_band_a, Set_Team_band_b, Set_Team_band_c, Set_Team_band_d,
          <map set>, Set_Pit_Fence_Oval, <scene>, <centreline>, <turrets>
UI.Notify("Loaded Arena ")   // D4, D5;  others "Loaded Arena 2"
```
Every function activates the shared sets (all four crowds, all four team bands, `Set_Pit_Fence_Oval`), plus the per-theme triple.

| Theme | Scene | Centreline | Turrets |
|---|---|---|---|
| Dystopian | `Set_Dystopian_Scene` | `set_centreline_dystopian_05` | `Set_Turrets` |
| Scifi | `Set_Scifi_Scene` | `set_centreline_scifi_05` | `Set_Turrets_scifi` |
| Wasteland | `Set_Wasteland_Scene` | `Set_CentreLine_Wasteland_05` | `Set_Turrets_wasteland` |

| Function (line) | Pre-refresh DEACTIVATE | Map set ACTIVATED | Used by |
|---|---|---|---|
| LoadDystopian_4 (19815) | Set_Dystopian_02 | Set_Dystopian_04 | Turret Mayhem 3; random arena entry |
| LoadDystopian_5 (19849) | Set_Dystopian_02 | Set_Dystopian_05 | VDM, Championship, Demolition Men, random entry, DM-random |
| LoadDystopian_3 (19883) | Set_Dystopian_04 | Set_Dystopian_03 | P2P D3, RC King D3, random entry |
| LoadDystopian_2 (19917) | Set_Dystopian_04 | Set_Dystopian_02 | P2P D2, random entry |
| LoadDystopian_10 (19951) | Set_Dystopian_04 | Set_Dystopian_10 | VDM, Championship, Demolition Men, random entry, DM-random |
| LoadDystopian_7 (19985) | Set_Dystopian_04 | Set_Dystopian_07 | RC King D7, random entry |
| LoadDystopian_8 (20019) | Set_Dystopian_04 | Set_Dystopian_08 | Turret Mayhem 4, **and every "Dystopian 7" VDM/Championship item** |
| LoadDystopian_12 (20053) | Set_Dystopian_04 | Set_Dystopian_12 | VDM, Championship, DM-random |
| LoadScifi_2 (20087) | – | Set_Scifi_02 | VDM, Championship, P2P, RC King, random entry, DM-random |
| LoadScifi_4 (20120) | – | Set_Scifi_04 | Turret Mayhem 2, Hot Ring 2 |
| LoadScifi_5 (20153) | – | Set_Scifi_05 | VDM, Championship, Demolition Men, Beast Wars, random entry, DM-random |
| LoadScifi_10 (20186) | – | Set_Scifi_10 | VDM, Championship, Demolition Men, random entry, DM-random |
| LoadScifi_9 (20252) | – | Set_Scifi_09 | P2P S9, RC King S9, random entry |
| LoadWasteland_2 (20219) | – | Set_Wasteland_02 | P2P W2, Demolition Men W2, random entry |
| LoadWasteland_9 (20285) | – | Set_Wasteland_09 | P2P W9, RC King W9, random entry |
| LoadWasteland_4 (20318) | – | Set_Wasteland_04 | Turret Mayhem 1, Hot Ring 1 |
| LoadWasteland_5 (20351) | – | Set_Wasteland_05 | VDM, Championship, Smash & Grab, Demolition Men, DM-random |
| LoadWasteland_7 (20384) | – | Set_Wasteland_07 | RC King W7 (its random-entry slot can never roll) |

- There are no loaders for Dystopian 1/6/9/11/13-17, Scifi 1/3/6/7/8 or Wasteland 1/3/6/8/10, although `NStadium` lists them.
- The pre-refresh deactivation in the Dystopian functions is redundant, because NStadium deactivates the same set afterwards.

**Setup() bowl/VIP block** (1421-1427), on the VIP interior handle:
1. DEACTIVATE `xs_x18int01_pit_fenceoval_col`, `xs_dystopian_17`
2. REFRESH
3. ACTIVATE `xs_dystopian_17`, `xs_x18int01_pit_fenceoval_col`
4. REFRESH

These names look like model or collision names, not entity-set names. They use the VIP interior handle, not the bowl. I believe this is a no-op, with low-to-medium confidence.

**Spectator branch** (22260-22267): if `IsSpectatingMatch`, which is never set to true, DEACTIVATE `Set_Pit_Fence_Oval`, ACTIVATE `set_wall_no_pit`, with no refresh.

**Random theme on entering the arena** (ArenaEntry 22129-22182, VipEntry 22203-22256, outside vehicle teleport 22287-22399): `Next(1,13)` maps 1..12 to D2, S2, W2, W9, S9, D3, D4, D10, D5, S5, S10, D7. The case 13 (W7) is unreachable.

### 2.6 Workshop interior (Enableinteriors.cs)
`Enableinterior()` runs on the interior at `(205,5180,-90)`:
1. DEACTIVATE everything in `Workshop`:
   - booths: `Set_Int_MOD_BOOTH_DEF`, `_BEN`, `_WP`, `_COMBO`
   - `Set_Int_MOD_BEDROOM_BLOCKER`
   - `Set_Int_MOD_CONSTRUCTION_01..03`
   - offices: `SET_OFFICE_STANDARD`, `_INDUSTRIAL`, `_HITECH`
   - styles: `Set_Mod1_Style_01..09`
   - `set_arena_peds`, `set_arena_no peds`
2. ACTIVATE or DEACTIVATE all trinkets according to `TrinketsAndTrophiesEnabled`:
   - `SET_XMAS_DECORATIONS`
   - trophies: `Set_Int_MOD_TROPHY_CAREER`, `_SCORE`, `_WAGEWORKER`, `_TIME_SERVED`, `_GOT_ONE`, `_OUTTA_HERE`, `_SHUNT`, `_BOBBY`, `_KILLED`, `_CROWD`, `_DUCK`, `_BANDITO`, `_SPINNER`, `_LENS`, `_WAR`, `_UNSTOPPABLE`, `_CONTACT`, `_TOWER`, `_STEP`, `_PEGASUS`
   - `SET_BANDITO_RC`
   - `SET_OFFICE_TRINKET_07/06/03/04/02`
3. ACTIVATE `Set_Int_MOD_SHELL_DEF`, `WorkGraphic`, `WorkOff` and `WorkMechanic`. If `!WLiving`, also `Set_Int_MOD_BEDROOM_BLOCKER`.
4. Apply the tint (unknown hash) with `WColor` to the shell, office, mechanic and graphic.
5. If `!nopeds`, ACTIVATE `set_arena_no peds` (inverted, see bugs).
6. Garage B1 at `(170,5190,10)`: DEACTIVATE `WorkGraphic` (×9, in a loop that never changes the name), DEACTIVATE `Set_Int_MOD2_B2`, ACTIVATE `Set_Int_MOD2_B_TINT`, `Set_Int_MOD2_B1` and `WorkGraphic`, and tint with `WColor1`.
7. No REFRESH_INTERIOR is called inside `Enableinterior`.

The design menu opens at `Marker = Vector3(221.088f, 5168.41f, -90f)` (Context key). Its options (Design, Office, Mechanic, Main Colour, B1 Colour, Trinkets) are saved to `Interior/Main.ini`.

---

## 3. Traps, turrets and hazards

### 3.1 `SetupTrapsPositions` / `TrapsPos` (1064-1092)
The list is used **only** by the Hot Ring "EMP mine" (1 % per frame, within 10 m; see section 1.9). No props are spawned there; the positions are invisible.

| Lower ring (z 130) | Upper ring (z 136) |
|---|---|
| `(2759.76,-3849,130)` | `(2740.01,-3873,136)` |
| `(2814.76,-3853,130)` | `(2772.01,-3874,136)` |
| `(2870.76,-3852,130)` | `(2822.01,-3876,136)` |
| `(2925.76,-3838,130)` | `(2885.01,-3875,136)` |
| `(2949,-3799,130)` | `(2934.01,-3858,136)` |
| `(2925,-3754,130)` | `(2962.01,-3806,136)` |
| `(2867,-3744,130)` | `(2950.01,-3754,136)` |
| `(2800,-3745,130)` | `(2903.01,-3726,136)` |
| `(2742,-3746,130)` | `(2776.01,-3722,136)` |
| `(2688,-3752,130)` | `(2649,-3760,136)` |
| `(2653,-3782,130)` | `(2638,-3827,136)` |
| `(2657,-3727,130)` | `(2704,-3876,136)` |
| `(2710.01,-3856,130)` | |

### 3.2 Sky Wars prop traps (spawned in the menu, 1650-1746; run in the tick, 18330-18440)
Slot positions (z 1001.5). The last entry has **+871.2**, almost certainly a typo for −871.2.

| Slot positions | Slot positions | Slot positions |
|---|---|---|
| `(23,-915)` | `(-128.021,-898.6)` | `(-196.814,-913.4)` |
| `(45,-863)` | `(-124.2,-856.3)` | `(-206.149,-823.8)` |
| `(31,-810)` | `(108.6,-830)` | `(-74.46,-794.8)` |
| `(-24,-811)` | `(-79.7,-794)` | `(-217.8,-866.8)` |
| `(63,-826)` | `(-123.3,-791.4)` | `(-151.7,-919.1)` |
| `(86,-837.7)` | `(-145.8,-808.2)` | `(-48.01,-924.8)` |
| `(83.4,-895.5)` | `(-155.6,-836.8)` | `(8.4,-792.5)` |
| `(32.1,-931.4)` | `(-166.6,-872.9)` | `(-122.2,-833.44)` |
| `(6,-919.6)` | | `(-66.15, 871.2)` |
| `(-32.1,-942.1)` | | |

Per slot: a random rotation 0–359 and a roll `n = Next(1,150)`:

| Roll | Prop | Z offset | List |
|---|---|---|---|
| <25 | `xs_prop_arena_spikes_01a` | – | Trap_Spike |
| 26–49 | `xs_prop_arena_pit_fire_01a` | −0.6 | Trap_Flame |
| 51–59 | `xs_prop_arena_landmine_01a` | – | Trap_Mines |
| 61–79 | `xs_prop_arena_landmine_03a_wl` | −0.4 | Trap_Mines |
| 81–99, 101–119 | `xs_prop_arena_flipper_small_01a` | −3 | Trap_Flipper |
| 121–144 | `xs_prop_arena_turntable_03a` | −0.4 | Trap_Turning |
| >145 | `xs_prop_arena_turret_post_01a` | – | Trap_GunTower |
| 25, 50, 60, 80, 100, 120, 145 | nothing | | |

All props are frozen with collision on. Trap effects are applied every frame to **any** vehicle (player or AI) while MissionId 27 is started:

| Trap | Trigger radius | Effect |
|---|---|---|
| Gun tower | 30 m | Picks a random nearby vehicle (`Next(0,len-1)`, never the last one) and calls `Firemissile4_12(target, towerPos+5z)`: 4 × `FireGun`, each 3 `VEHICLE_WEAPON_TURRET_TECHNICAL` bullets with damage 3, aimed within 2–6 m. Fires every frame. |
| Flame | 3 m | 5 × `World.AddExplosion(Molotov1, radius 10, shake 1)` within 3 m; Engine/BodyHealth −0.5. |
| Flipper | 3 m | The prop yaws +2°/frame (twice when occupied); the vehicle rotation is set to the prop's; `Speed += 75`. |
| Mine | 4 m | `vehicle.Explode()`, `ApplyForce(WorldUp, 0)`, `Speed += 20`. The mine is never consumed. |
| Spike | 5 m | `BurstTire(Next(0,5))`. |
| Turntable | – | Yaw +2°/frame only. |

The bowl entity sets `Set_Turrets*` are static scenery. **No bowl entity-set trap is ever scripted.**

### 3.3 Missiles and turrets (20417-21070)

**Fire primitives:**

| Function | Weapon | Shots | Damage | Owner |
|---|---|---|---|---|
| `FireGun` | VEHICLE_WEAPON_TURRET_TECHNICAL | 3 | 3 | passed |
| `FireMissile` | WEAPON_AIRSTRIKE_ROCKET | 3 | 15000 | always the **player** (the owner argument is ignored); notifies "Firing missile" |
| `FireMine` | VEHICLE_WEAPON_NOSE_TURRET_VALKYRIE | 1, straight down 5 m | 25000 | passed; notifies "player hit mine" |
| `FireTrap` | "DIR_FLAME_EXPLODE" | 6 | 15000 / 25000 | passed; unused |

**Roof launch points** used by Firemissile, 4_1, 4_4 and 8_1, all at z 154: `(2718,-3870)`, `(2629,-3805)`, `(2712,-3717)`, `(2884,-3722)`, `(2970,-3799)`, `(2886,-3883)`. The roll is `Next(1,60)` in bands of 10; exact multiples of 10 fire nothing.

**Callers:**

| Function | Behaviour | Used by |
|---|---|---|
| `Firemissile()` | Target: the player | P2P (46 frames), Hot Ring (36 frames) |
| `Firemissile4_1()` | Target: player, VtoGet, VtoGet1 or VtoGet2 (`Next(1,5)` never returns 5) | VDM, Smash & Grab |
| `Firemissile4_4()` | 4 rockets scattered 5–20 m | Championship |
| `Firemissile4_8()` | Fires from `(-42,-871,1014)` (the sky turret) | Sky Wars |
| `Firemissile4_16()` | Every frame in Turret Mayhem: for each of 6 points [`(2718,-3870,154)` twice, `(2629,-3805,154)`, `(2884,-3722,154)`, `(2970,-3799,154)`, `(2886,-3883,154)`], for every vehicle within 100 m of that point, `FireGun` from the point at the **player's** position ±1–4 m | Turret Mayhem |
| `Firemissile8_1()` | – | Unused |

Every call creates a `PedHash.Fabien` at `(0,0,0)` and deletes it.

### 3.4 Invisible "mines" (MineLoc1..6)
- Every timed or wave mode checks the six mine positions (within 12 m) and calls `FireMine` plus `ChangeMIneLoc`.
- `ChangeMIneLoc` (11746) only assigns positions when **MissionId == 1** (no such mode exists; the per-Race pools are Fun2–Fun7) or **MissionId == 2** (Race 2 → Fun2, Race 1 → Fun3).
- Nothing initialises the mines, so they stay at `(0,0,0)`.
- Net effect: **mines never exist in any reachable mode.** In MissionId 2 they could only appear after one is triggered at the world origin.

---

## 4. Entry points, teleports and spawn points

### 4.1 Map blips (SetupMarker 2616)

| Blip | Position | Sprite / colour | Name |
|---|---|---|---|
| `TowerBlip` | `(-371,-1850,20)` | 458, colour 8 | "Arena War Business " |
| `TowerBlip2` | `(-393,-1870,20)` | 269, colour 8 | "Arena War Business (Arena Teleport)" |
| `TowerBlip3` | at `ArenaExit` | 269, colour 8 | "Arena War Business (Workshop Teleport)" |

`TowerBlip3` is shown only while the player is within 200 m of `ArenaExit` and between z 120 and 175 (19785-19802).

### 4.2 Markers and teleports
All are Context (E) unless noted. "Cover" means the Q key.

| From | To | Notes |
|---|---|---|
| `MarkerEnter (-377,-1877,19)` (Maze Bank Arena exterior) | `MarkerExit (216,5205,-90)` (workshop) | Buys the business for $1,000,000 if `BuisnessLevel == 0`. Entry requires wanted level 0. Calls LoadVehicles. |
| `MarkerExit (216,5205,-90)` | `MarkerEnter` | |
| `StadiumUpperEntry (-242.472,-1995.96,24)` | `StadiumUpperExit (211.903,5199.94,-90)` | And back. Help text "Enter ARB" / "Exit ARB". |
| `ArenaEntry (221,5180,-89)` (workshop) | `ArenaExit (2810,-3897,139)` (bowl) | Applies a random theme. ArenaExit → ArenaEntry returns. |
| `ArenaVehicleMenuMarker (-394,-1871,19)` (outside) | `ArenaExit` | Takes the current vehicle along. Random theme. |
| `VipEntry (217.034,5159.5,-90)` (workshop) | `VipExit (2797.71,-3942.01,184)` | Random theme, plus REQUEST vip IPL. The reverse REMOVEs the vip IPL. |
| `GarageEntry (-364.8876,-1871,19)` | `GarageAMarkerInside (172,5163,10)` | Garage A, `CreateCars("GarageA")` |
| `GarageAMarkerInside` (Cover) | `GarageBMarkerOuside (-388,-1880,19)` or `GarageAMarkerOuside (-365,-1871,19)` | |
| `GarageBMarkerOuside (-388,-1880,19)` | `ExitMarker3 (172,5216,10)` | Second Vehicle Bay, `LoadVehicles2` |
| `VehicleBayMarker (169,5157,-90)` (workshop) | `ExitMarker3` | Second bay |
| `SecondEntry (168,5210,-90)` (workshop) | `ExitMarker3` | As Garage A |
| `ExitMarker3` (Cover) | `VehicleBayMarker` (from bay) or `SecondEntry` (from garage) | |
| `(-365,-1871,19)` (Cover) | `EntryMarker (-365,-1871,20)` | |
| `GarageMarker (-81,-813.158,35)` | `RoofEnterMarker (-74,-809,242.5)` | Maze Bank leftover; two handlers |

**Menus and interactions inside the interiors:**

| Position | Function |
|---|---|
| `MenuMarker (205,5163,-87)` | Main mission menu |
| `ArenaMenuMarker (2789.97,-3903.44,139)` | Arena vehicle request menu; the vehicle spawns at `(2785,-3909.6,140)`, heading 0 |
| `GarageMenuSpawn (169,5180,-90)` | Workshop vehicle menu |
| `GarageMenuMarker2 (171,5186,10)` | Second-bay menu |
| `EnterGarageMarker (-371.8876,-1850,21)` | Save vehicle to Garage A |
| `RemoveMarker (188,5186,10)` | Remove a vehicle |
| `EI.Marker (221.088,5168.41,-90)` | Interior design menu |
| `SleepPos (209,5164,-90)` | Sleep: `ADVANCE_CLOCK_TIME_TO(6,0,0)`, clears wanted, blood and wetness |
| `Sitpos (210,5166.3,-87)` | Sit: anim `anim@amb@office@seating@male@var_e@base@` / `base`; stand up at `(209,5165,-85)` |
| `CharacterSwitchPos (211.37,5161.95,-90)` | Cycles Franklin → Michael → Trevor |
| `DrinkPos (2810,-3929,183)` (VIP) | `WORLD_HUMAN_DRINKING` for 9 s |
| `JackpotTable (2796.17,-3933.43,182)` (VIP) | $250,000 "prize" roll (`GetAJackPot` 21071) |

**Prizes from `GetAJackPot`:**
- $1M
- unlock flags for Clique, Itali GTO, Tulip, Vamos, Schlagen, Deviant, Toros, Deveste and several arena variants
- 2× / 3× / 10× stocks
- weapons: Up-n-Atomizer, Hellbringer, Widowmaker, Mk II rifles, minigun, grenade launcher
- health or armour, or clearing the wanted level
- "nothing" for rolls above 1000

Defined but never used: `ArenaVehicleMarker (2815,-3909,140)`, `GarageAEntryOutside (-365,-1817,19)`, `ExitMarker2 (-71.949,-831.736,222.001)`, `OutsideMenuMarker`, and `EnterMarkerA` (never assigned).

### 4.3 How the player gets into the arena
There are three routes:
- On foot from the workshop at `ArenaEntry`.
- From the VIP marker.
- From outside at `ArenaVehicleMenuMarker`, which carries the current vehicle.

All three land at `ArenaExit (2810,-3897,139)` and load a random theme. Mission menu items instead teleport the player straight to `(2782.99,-3898.15,140.001)` and load the theme named in the item.

### 4.4 Vehicle display spawns
Workshop, z −90, heading −90 in `LoadVehicles`:

| Vehicle | Position |
|---|---|
| Cerberus | `(159,5180)` |
| Scarab | `(178,5189.4)` |
| Dominator | `(161,5171)` |
| Imperator | `(177,5171)` |
| Deathbike | `(159,5158)` |
| Issi | `(160,5201)` |
| Monster | `(160,5189)` |
| Impaler | `(178,5201)` |
| Slamvan | `(160,5207)` |
| Brutus | `(178,5158)` |

Second bay (`LoadVehicles2`), z 10:

| Vehicle | Position |
|---|---|
| Bruiser | `(159,5187)` |
| ZR380 | `(182,5187)` |
| Clique | `(180,5196)` |
| Italigto | `(163,5196)` |
| Tulip | `(163.48,5208.43)` |
| Vamos | `(164.1225,5214.5)` |
| Schlagen | `(180,5208)` |
| Deviant | `(163,5177)` |
| Toros | `(181.2755,5177.701)` |
| Deveste | `(163,5165)` |
| RcBandito | `(182,5165,11)` |

Garage A save slots, z 10:

| Slot | Position |
|---|---|
| 1 | `(159,5187)` |
| 2 | `(162,5196)` |
| 3 | `(163,5208)` |
| 4 | `(163,5214)` |
| 5 | `(180,5208)` |
| 6 | `(181,5196)` |
| 7 | `(183,5187)` |
| 8 | `(181,5177)` |
| 9 | `(181,5165)` |
| 10 | `(163,5165)` |

A vehicle taken out of a garage is placed at `(-371.8876,-1850,21)`, rotation (6,−1,10).

`LoadVehicles` first creates 10 Adders at `(0,0,0)` and deletes them again (2812ff), apparently as a hack.

---

## 5. Arena vehicle data (`ArenaWarBusiness/Vehicles/*`)

### Format
Standard SHVDN `ScriptSettings` INI, written in upper case. `GetValue` is case-insensitive.
- `[CONFIGURATIONS]` keys:
  - `VEHICLENAME`, `VEHICLEHASH`
  - one `…ID` per mod slot: ROOFID, AERIALSID, AIRFILTERID, ARCHCOVERID, ARMORID, BACKWHEELSID, BRAKESID, COLUMNSHIFTERLEVERSID, DASHBOARDID, DIALDESIGNID, DOORSPEEKERSID, ENGINEID, ENGINEBLOCKID, EXAUSTID, FENDERID, FRAMEID, FRONTBUMPERID, FRONTWHEELSID, GRILLEID, HOODID, HORNSID, HYDRALICSID, LIVERYID, ORNAMENTSID, PLAQUESID, PLATEHOLDER, REARBUMPERID, RIGHTFENDERID, SEATSID, SPEAKERS, SPOILERSID, STEERINGWHEELID, STRUTID, SUSPENSIONID, TRANSMISSIONID, TRIMDESIGNID, TRUNKID, VANITYPLATESID, WINDOWID
  - colours: PRIMARYCOLOR, SECONDARYCOLOR, PEARLCENT, WHEELCOLOUR, DASHBOARDCOLOUR, TRIMCOLOR (`VehicleColor` enum names)
  - `TINTID` (`VehicleWindowTint` name or −1)
  - `NEONCOLOUR` / `NEONCOLOR` ("Color [A=…, R=…, G=…, B=…]") and the LEFT/RIGHT/FRONT/BACK NEONON flags
  - `HASXENONLIGHTS`, `LIGHTCOLOR` (xenon index: 1 Blue … 12 Blacklight, 255 Stock, per the shipped README)
  - `TURBOACTIVE`, `TIRESMOKE`, `TIRESMOKECOLOR`, `BULLETPROOFTIRES`, `BLADES`, `PLANTENO`
- `[TOGGLEABLEEXTRAS] EXTRAS0..20` appears only in the Dominator and Imperator FutureShock files.

`SaveCar.LoadIniFile` / `Load(Vehicle)` read these keys (SaveCar.cs 194-260, 684-1112) and apply:
- the mod kit and every `SetMod`
- livery
- colours
- xenon on/off plus `SET_VEHICLE_XENON_LIGHT_COLOR_INDEX`
- neons
- window tint
- `Torque` / `Power` multipliers
- a crew emblem if `CLanTagOn`

### Keys present but ignored
- `ROOF`
- `ARCHCOVERID//1..13`, `HOODID//1..13`, `STRUTID//1..13`: junk from some other saver, never read.
- `VEHICLEHASH`: `Load(Vehicle)` applies to whichever model the code spawned.

Because each `GetValue` passes the previous value as its default, any key missing from a file inherits the value from the previously loaded file. `Torque` and `Power` are in no shipped file, so they keep whatever was loaded last (initially 0).

### Variant mapping in code
The menu at 2423-2578 and `LoadVehicles` map file names to models:
- `1Apocalypse.ini` → base models: Bruiser, Brutus, Cerberus, Deathbike, Dominator4, Impaler2, Imperator, Issi4, Monster3, Scarab, SlamVan4, ZR380.
- `3FutureShock.ini` → "2" models: Bruiser2, Brutus2, Cerberus2, Deathbike2, Dominator5, Impaler3, Imperator2, Issi5, Monster4, Scarab2, SlamVan5, ZR3802.
- `2NIghtmare.ini` (sic) → "3" models: Bruiser3, Brutus3, Cerberus3, Deathbike3, Dominator6, Impaler4, Imperator3, Issi6, Monster5, Scarab3, SlamVan6, ZR3803.
- Non-arena cars use `Car.ini` in their own folders: Clique, Italigto, Tulip, Vamos, Schlagen, Deviant, Toros, Deveste, RC_Bandito.

### What the files set (summary)
- **Performance:** nearly every arena file sets armour 4, brakes 2, engine 3 and transmission 2–3. Most also set Frame 2 and RightFender 0 or 4 (RightFender is the arena weapon/armour slot on these models), plus suspension 0–4.
- **Apocalypse:**
  - Cerberus, Brutus, Deathbike, Impaler and Monster are **byte-identical templates**: Beechwood/Beechwood/DarkSilver, livery 1, roof 0, frame 1, RightFender 0, suspension 0.
  - Scarab and Imperator Apocalypse use the olive "Nightmare" template.
  - Bruiser, Dominator, Issi and Slamvan have their own setups:
    - Bruiser: livery 3, DesertRed/GunMetal
    - Dominator: livery 17, GarnetRed/SteelGray, pearl HotPink
    - Issi: livery 3, many mods
    - Slamvan: livery 0
  - ZR380 is stock black with almost no mods.
- **Nightmare:** the olive template (OliveGreen/OliveGreen, pearl MidnightSilver, livery 39, FrontWheels 37, Horns 51, tint PureBlack) for Cerberus, Brutus, Deathbike, Dominator, Monster, Scarab and Bruiser (Beechwood variant). Impaler, Imperator, Issi, Slamvan and ZR380 have custom liveries (3) and colours.
- **Future Shock:** mostly livery 17, tint PureBlack, xenon on with colours 1/2/8/11/6, bright colour pairs (Black/FormulaRed, BrightBlue/White, and so on).
- **Errors in the data:**
  - `Bruitus/2NIghtmare.ini`: `VEHICLENAME = INSURGENT3`, no VEHICLEHASH.
  - `RC_Bandito/Car.ini`: `VEHICLENAME = SLAMVAN4` (a copied Slamvan file).
  - The identical Apocalypse templates described above.

`Main.ini`:
- `[SETUP]`: BUISNESSLEVEL, STOCKSLEVEL, STOCKSVALUE, MARKERCOLOR = Magenta, USEMISSILESINVDM = true, and others.
- `[VEHICLES]`: `CURRENT*` and the `*BOUGHT` flags.
- `[CONFIGURATIONS] KEY1 = E`.
- `[ADDON2]`: money vault data.
- There is **no `[Design] InteriorDesign`**.

`Interior/Main.ini` holds the workshop style, office, mechanic, colours, living area, peds and trinket flags (documented in `Interior/To Load Interior.txt`).

---

## 6. Bugs (with line numbers and my confidence)

Confidence scale: **H** = certain from the code, **M** = very likely, **L** = plausible or dependent on engine behaviour.

### 6.1 Theme/menu mapping
1. **H.** "Dystopian 7" calls `LoadDystopian_8` in Championship (2062), VDM 4x (4362) and VDM 8x (4284). `LoadDystopian_7` exists (19985). RC King "Dystopian 7" is correct (4011).
2. **H.** The random arena-entry theme uses `Next(1,13)`, so case 13 (`LoadWasteland_7`) is unreachable. The same pattern appears in 4 copies: 22130, 22204, 22288, 22347.
3. **H.** Demolition Men "Random Track & Car" (3258-3290): rolls of exactly 10, 20 … 70 load no theme. Both 51–59 and 61–69 load `Wasteland_5`. `Wasteland_2` and `Scifi_9` are never picked.
4. **H.** Smash & Grab: both items read "Smash and Grab 1"; only the Opp count differs (3687/3689).
5. **H.** Sky Wars "Dystopian A" does not load an arena theme; it is a sky platform.

### 6.2 Interior / IPL
1. **H.** `REFRESH_INTERIOR` is called *before* the deactivation loop and the activations in every `Load*` (for example 19823, 19857, 20094, 20127 …), and never after. **M**: the changes may not show until the interior is next reloaded.
2. **H.** Every `Set_Lights_*` is deactivated and none is activated again (19824-19831 and the same block in each loader). **L/M**: the bowl ends up with no lighting preset.
3. **H.** Every loader activates all four crowd sets and all four team bands at once (for example 19833-19840).
4. **H.** Every `Load*` REMOVEs `xs_arena_interior_vip` (19819 …), even when entered from the VIP route, which REQUESTs it again right after (22258).
5. **M.** Setup() 1421-1427 uses the VIP interior handle with names (`xs_dystopian_17`, `xs_x18int01_pit_fenceoval_col`) that look like models, not entity sets. It also refreshes twice.
6. **H.** `Design` is not in the shipped Main.ini, so `REQUEST_IPL(null)` runs at 22569, 22592 and 22621. The Maze Bank office coordinates are leftover code.
7. **H.** `EI.ReloadInterior()` (which removes and re-requests 4 IPLs) runs on every teleport (22114, 22124, 22186, 22197, 22271, 22405, 22415, 22437, 22448, 22462, 22470, 22482, 22494, 22505). This is expensive and resets entity-set state.
8. **H.** Enableinteriors 202: the "Workshop B1 / Main Color" menu adds `list4` instead of `list5`. `list5.get_Index()` (305) is therefore always 0, and the B1 colour is always "0".
9. **H.** Enableinteriors 167-176: the colour list has "1" twice and lacks "5".
10. **H.** Enableinteriors 429-432: `if (!nopeds)` ACTIVATEs `"set_arena_no peds"`, which is the inverted logic.
11. **H.** Enableinteriors 435-438: the loop deactivates `WorkGraphic` 9 times instead of `Set_Mod1_Style_01..09`.
12. **H.** Enableinteriors never REFRESHes the interior, so its notifications tell the user to reload the mod.
13. **L.** Enableinteriors 425-445 uses an unidentified native hash (0x31D23FC8CCD18DC3) for tints.
14. **L.** `Enableinteriors`, `AllStocks`, `SaveCar`, `MissionScreen`, `VehiclesEmblem`, `GunLocker` and `MoneyVault` all derive from `Script` and are also created with `new`. SHVDN will also auto-instantiate them, so there can be duplicate instances with separate state.

### 6.3 Crashes and null dereferences
1. **H.** P2P Race 4 (Dystopian 2), 13040-13053: `if (CurrentVehicle == null) { if (!CurrentVehicle.IsAlive) …` dereferences null, which throws an NRE when the player leaves the vehicle. The same block then calls `CurrentVehicle.Explode()`.
2. **H.** RC King 14267-14286 and 14344-14362: after the "abandoned vehicle" block, execution continues to `CurrentVehicle.IsInAir`, which throws an NRE on foot.
3. **H.** Championship (18054, 18068, 18082, 18096) and Sky Wars (18455, 18469, 18483, 18497) read `Game.Player.Character.CurrentVehicle.Speed` before any null check, which throws an NRE when the player is on foot.
4. **H.** The time-out branches call `Game.Player.Character.CurrentVehicle.Explode()` *after* `Task.WarpOutOfVehicle` (13053, 13126, 13200, 14319, 14395 …). If the warp has already happened, this is a null dereference. **M**: warping may be deferred, in which case it works by accident.
5. **M.** Training Day 17858-17868: after the escape triggers, `VtoGet.GetPedOnSeat(Driver).Task.FleeFrom` runs even when `VtoGet` is a wreck.
6. **M.** `VtoGet != null && VtoGet != null && VtoGet2 …` (14044, 15369, 15668, 16032, 17301): `VtoGet1` is never null-checked.
7. **H.** OnTick 12274-12361 calls `X.GetPedOnSeat(Driver).IsPlayer` for every garage vehicle every tick. **L**: this throws if SHVDN returns null.
8. **H.** `SpawnProp` failure (1094) followed by `Props[Props.Count-1]` (1696 …) attaches the wrong (previous or deleted) prop to the trap list, or throws when `Props` is empty.

### 6.4 Game logic
1. **H.** Mines never exist. `ChangeMIneLoc` (11749) checks `MissionId == 1` while Point to Point is MissionId 10, and `MineLoc1..6` start at `(0,0,0)`. The "avoid the mines" brief is non-functional everywhere except, theoretically, MissionId 2.
2. **H.** `FinishLine` is never assigned. `ChecktoSeeifinthelead` (9898) compares distances to `(0,0,0)`, so race placing is meaningless. A finish marker is drawn at the origin (12700, 12921, 15506, 19175).
3. **H.** `lap` is never reset anywhere, and `checkpoint` is never reset at mission start. The 3-lap race requires `lap == 3` exactly (12738, 12959): a second race never finishes, or a third finishes instantly. Hot Ring and Turret Mayhem pay `100000*lap` with the carried-over count.
4. **H.** `vehicleHealth` is incremented per wave (14078, 14549, 15403, 16066, 17335) and initialised to 600, but it is never applied. The "each wave harder" claim is false.
5. **H.** `SetupChampionchipCars` has no `I == 4` branch (10706-10831), while `18113-18116` calls it for a dead `VtoGet3`. Championship therefore has only 3 respawning opponents, and the 4th corner never comes back.
6. **H.** `SetupDeathMatchCars` (10423-10450), `SetupSkyWarsCars` (10537) and `SetupDeathMatchCars4` (11682) apply the AI attribute natives to `GetPedOnSeat` *before* `CreateRandomPedOnSeat` runs, so the drivers get no attributes.
7. **M.** The AI attribute block sets combat attribute 17 (BF_AlwaysFlee) to true on every opponent. This was probably meant to be 46 (CanFightArmedPedsWhenNotArmed) or 5 (AlwaysFight).
8. **H.** In Championship and Sky Wars, `TASK_VEHICLE_MISSION` with type 7 (MISSION_FOLLOW, not ATTACK) is re-issued every tick. `DriveTo`, `FleeFrom` and `FightAgainst` are also re-issued every tick (9912-9980, 17858ff, 16719ff), which resets tasks constantly.
9. **H.** `getVehicle()` roll 140 (10047) and `getVehicle2()` rolls 100–109 (10162) assign nothing. The previous `VehicleChoosen` is reused, or `0` (an invalid model) on the first call.
10. **H.** `SetupModifications`: the xenon 255 branch is unreachable (10260), and mod 44 is always 0 (10202). `SetupRivalCar`: integer division gives 1–9× torque and power, and the two `Random` instances share a seed (10271-10275).
11. **H.** `Firemissile4_1`: `Next(1,5)` makes `num == 5` (`VtoGet3`) unreachable (20541). The `Firemissile*` rolls that are multiples of 10 fire nothing. `FireMissile` ignores the owner and credits the player (20444).
12. **M.** `Firemissile4_16` lists `(2718,-3870,154)` twice and omits `(2712,-3717,154)` (20879-20884). It also targets the player's position for vehicles near *any* turret.
13. **H.** Turret Mayhem calls `Firemissile4_16` every frame (19141, 19273): 6 GetNearbyVehicles calls, a CreatePed/Delete and up to dozens of bullets per frame. Sky Wars gun towers do the same (`Firemissile4_12` per frame).
14. **H.** `RespawnSkywarsCar` has a duplicated `if (I == 4)` (10959, 10998), which spawns two vehicles and leaks one.
15. **H.** `SetupSkyWarsCars2` positions are 10 m from `SetupSkyWarsCars` positions, and `SetupDeathMatchCars2` spawns exactly on the `SetupDeathMatchCars` corners (11203-11209). The 8x modes rely on a 3 s wait, and vehicles still overlap if the first wave has not moved.
16. **H.** Smash & Grab, 15967-15989 (and 16262ff): once `CheckPointstoProceed == 6`, "Got Loot, head to goal" is notified every frame, and the blip is immediately overwritten with `CheckPointPosition`, so it never points to `Goal1`. `CheckPointstoProceed` is never reset at mission start.
17. **H.** Demolition Men (17265-17284): touching an RC Bandito blows it up and *pays* $75,000, which contradicts "avoid". The player's `BodyHealth` is set to 100 at start (17510).
18. **H.** Hot Ring EMP (15490-15500) sets vehicle `Health = 100` after the "EMP", leaving the car at 10 % health, not restored. It also shows a debug `UI.Notify("C"+num8)` (15486).
19. **H.** MissionId 11: the "Scarab A" drop blip is created at `VtoGet5.Position` instead of `VtoGet4` (15119). Blips 4–6 are not removed on pass or fail (15251-15262, 15315-15326).
20. **H.** MissionId 7: blip C is named "Vehicle A" (5549). There is no player-death fail.
21. **H.** MissionId 14 (soccer) checks `VtoGet..3`, but `SetupDeathMatchCars3` fills `VtoGet4..7` (11544ff vs 15668ff). It is dead code anyway.
22. **H.** Beast Wars: `SetupSpecialMissions7` is never called (Setup 1436-1442), and the `BeastWars` menu field is never created, so it would throw if called. `ChooseTarget` (12254) can return the player.
23. **H.** MissionId 5 "no driver" check (14415-14434) runs every tick, even before the wave starts, and spams notifications. It mislabels C and D as "B". **L**: it may never fire, because SHVDN2 `GetPedOnSeat` is not null.
24. **H.** `SetupRaces` (the racetrack race) never sets invincibility, never closes menus, and never ends if the player leaves.
25. **H.** `MissionScreen.SetPass/SetFail` never pays. The amounts it shows (Rookie $540,000, Oil $1,450,000 …) differ from the stock increment actually applied. Several fail screens show a "payout".
26. **H.** Smash & Grab 8x creates `PLayerVehicle` twice (3714-3715) and leaks one.
27. **H.** The Beast spawns its trailer at a different random location from the car before attaching it (5316-5320). `RandomVehicleFun()` in Rogue Contestant is unused (5634).
28. **H.** `RandomVehicleFun` (8776) re-rolls on every `if`, and branches 13 and above are unreachable.
29. **H.** `EnterMarkerA` is never assigned (733), so the Garage A code at 21461 triggers near world origin.
30. **H.** `MoneyVaultBought` and `AATrailerCBought` are read with a leading space in the key (`" MoneyVaultBought"`, 1170; `" AATrailerCBought"`, 1173), so the value is never loaded.

### 6.5 Money exploits
1. **H.** **Championship infinite money.** When `VtoGet3` dies, `SetupChampionchipCars(4)` runs every frame. It spawns nothing but adds $75,000 and `checkpointReaced++` per frame (18113-18116 → 10829-10830).
2. **H.** **Racetrack race:** finishing while not "leading", with no AI at 3 laps, pays **$4,500,000**, ten times the win payout (13021, 12800).
3. **H.** **RC King:** +$1,000 per *frame* while upside down or airborne (14286-14293, 14362-14369). Parking upside down prints money.
4. **H.** **Point to Point "Dystopian 3"** (Race 8, 5025-5034) does not reset `checkpointReaced` or `moneytobeawarded`, so the earnings of the previous mission carry over and are paid again.
5. **H.** **Hot Ring and Turret Mayhem:** `lap` is never reset, so the payout uses the cumulative lap count.
6. **H.** Every wave or timer mode pays the full amount when the player simply leaves the car. There is no penalty, and death also pays.
7. **M.** **P2P checkpoint re-trigger:** when the random pool hits a gap value, `CheckPointPosition` is unchanged and still within 10 m of the player, so the checkpoint is collected again on the next frame (+cash and +time).
8. **H.** **Sky Wars:** opponents that fall off by themselves respawn and pay $25,000 each (`RespawnSkywarsCar` 11194-11195).
9. **H.** **Jackpot** (21071): the ranges overlap and award two prizes: 391–400 gives Widow Maker *and* FS Dominator, 481–500 gives FS Monster *and* Nightmare Bruiser, and 631–660 gives the Heavy Sniper Mk II *and* the Marksman Mk II. The "Nightmare Impaler" prize is announced as "Apocalypse ZR380" (21195). A debug "Prize : N" is shown.

### 6.6 Cleanup and state
1. **H.** The player is made invincible at mission start (`Game.Player.Character.IsInvincible = true` at 1488, 1515, 1542, 1569, 1614, 1797, 2039 … 5104, 15468, 19167, 19298: 58 sites). It is reset only on normal end paths. It is never reset when:
   - a new mission is chosen mid-run
   - the player teleports out through `ArenaExit` or VIP
   - the script is disposed (`Dispose`, 22631, does not touch it)
2. **H.** Menu items delete only `VtoGet` (sometimes plus `FinishLineBLip` or `Props`) before starting. `VtoGet1..7`, `VtoGetBlip*`, `RaceBlip`, `PLayerVehicle2`, `Peds` and `ExtraV` from a previous run survive. A new `RaceBlip` or `VtoGetBlip` is created on top of the old one (1475, 2029, 3305 …), leaking blips.
3. **H.** `Peds` and `Props` are never `.Clear()`ed. They grow forever with deleted handles. On-foot ped cleanup only deletes peds whose `CurrentVehicle == null`; drivers are left to vehicle deletion. The Sky Wars menu deletes `Props` but keeps the list and then iterates it (1600-1606, 1680-1686).
4. **H.** `MissionId` is not reset on most end paths (only `MissionSetup`). `StartedRace`, `inRace`, `CircuitRace` and `CircuitRace2` are not reset in all paths: `CircuitRace2` is never reset, and `CircuitRace` is reset only on lose or "$4.5M" paths.
5. **H.** When Sky Wars ends, the player is teleported to `MenuMarker` but the interior flags are not set (18596, 18656, 19011, 19103).
6. **H.** `MissionScreen` is a `Script` created with `new` on every pass or fail. It blocks the caller for 100 frames.
7. **H.** `LoadVehicles` spawns 10 throwaway Adders at `(0,0,0)` each time (2816-2826).
8. **H.** `RequestModel` never calls `MarkAsNoLongerNeeded` on success (1299-1329).
9. **H.** The Sky Wars trap list has a typo at its last slot, `Vector3(-66.15f, 871.2f, 1001.5f)` (1678, 1861): a trap spawns about 1.7 km away in the air.
10. **M.** `SaveCar` keeps the previous file's values for keys missing in the next file, including `Torque` and `Power`, which no file has, so they start at 0.

### 6.7 Leftover debug UI
1. **H.** There are 153 `UI.Notify("test…")` / `"Test…"` calls. They appear in:
   - every `Load*` ("test", "test1", "test2", "Loaded Arena")
   - menu handlers (2019, 2065, 2246, 3294, 3304, 3310, …, 5464-5647)
   - race starts (12609-12692, 12846-12913, 15439-15457, 19154-19164, 19286-19289)
   - teleports (22117, 22127, 22200, 22408, 22416, 22439, 22450, 22465, 22478, 22485, 22497, 22508)
   - P2P (13222, "testaa2")
2. **H.** Other debug notifications:
   - "Health : <BodyHealth>" every frame (17468, 17725)
   - "C"+n (15486)
   - "Firing missile" (20447), "player hit mine" (20452), "player hit Trap" (20469)
   - "Prize : N" (21311)
   - "DN <displayname>" (21893, 21920)
   - "Ped :…", "Ped is on Seat", "AI changed Target", "Ped is Dead" (16738-16770)
   - "fire" ×2 whenever the player fires a Hellbringer or Widowmaker anywhere (19398-19404); the related "teleport into puff of smoke" feature is gated by `PedsDieinPufofSmoke`, which is never set
3. **H.** Pressing LookBehind (C) in any vehicle shows its Rotation and Position (22102-22106).
4. **H.** Several fields are declared but unused: `VehicleABet…VehicleDBet`, `VehicleATarget…`, `IsSpectatingMatch` (never true), `PedBlips`, `Trap_Flipper2` (never filled), `TimeTrial`, `longhaul`, `Rivals`, `SpeedCam`, `PointA`, `Chaser2`, `lockz`, `UseCustomWaitTime`. These look like the remains of a betting or spectating feature.

---

## 7. Teams, bands, crowd, scoreboard, announcer, championship

- **Team bands:**
  - `Set_Team_Band_A`, `_B`, `_C` and `_D` are in `NStadium` (1003-1006). Every `Load*` deactivates them all and then **activates all four** (lowercase `Set_Team_band_a..d`).
  - No code picks a band per team or per player. Nothing ties band colour to vehicles or blips.
  - For a teams feature: these are four separate bowl entity sets that can be toggled individually on the bowl interior at `(2782.99,-3898.15,140)`. Call REFRESH after the change, not before as this code does.
- **Crowds:** `Set_Crowd_A..D` are handled the same way: deactivated with the rest of `NStadium`, then all four re-activated (`Set_Crowd_a..d`). No crowd reaction or intensity logic exists.
- **Lighting:** 11 `Set_Lights_*` sets are known (section 2.4). They are deactivated and none is chosen. A rebuild could choose one per match.
- **Teams:**
  - There is no concept of teams. Every opponent is individually hostile.
  - The only allegiance code is the unused Beast Wars gunner: `GET_HASH_KEY("RESPECT_PLAYER")` + `SET_PED_RELATIONSHIP_GROUP_HASH`, invincible, `CanBeTargetted = false`, targeting `ChooseTarget()` (3640-3644, 16719ff). A teams rebuild would need real relationship groups.
  - Opponents use the default relationship groups, so they may also fight each other: they are chased or tasked only toward the player.
- **Scoreboard / HUD:**
  - Only `DisplayHelpTextThisFrame` strings ("Current Wave", "Cash Earned", "Time Left", "Lap N", "Amount of Loot Retreived", "Survived for").
  - Blips: GunCar sprite, colour 8 for opponents (10466-10477 and others). White RaceFinish for start and checkpoints.
  - `MissionScreen` shows a "mission passed/failed" banner (sprite `mpentry`/`mp_modenotselected_gradient`, `UIResText` in Pricedown).
  - There is no scaleform scoreboard (an `instructional_buttons` scaleform is loaded and rendered empty).
- **Announcer and sounds:** there are no arena announcer lines or crowd audio. The only audio is `MissionScreen`: `Audio.PlaySoundFrontend("Mission_Pass_Notify", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS")` and `("Bed", "WastedSounds")`, with `SetAudioFlag("LoadMPData", true/false)`. All "voice" is `UI.Notify("Alan : …")` text.
- **Championship progression:**
  - None. "Championchip Matches" is an endless respawn deathmatch: +$75,000 per kill, 3 of the 4 advertised opponents (bug 6.4 #5), and an exploit (6.5 #1).
  - There are no rounds, points table, persistence or unlocks tied to it.
  - The only persistent "progression" is in Main.ini: the `*Bought` vehicle flags, with `CURRENT*` set to 1, 2 or 3 per vehicle for Apocalypse, Nightmare or Future Shock, plus business `StocksValue`.
  - The Jackpot table can unlock vehicles. Nothing in the arena modes awards vehicles or trophies; the trophy entity sets in the workshop are all toggled by a single ini flag.
- **Useful raw data for a teams/arena rebuild:**
  - The four spawn corners: `(2736,-3892,140)` and `(2866,-3892,140)` facing 0; `(2865,-3707,140)` and `(2732,-3707,140)` facing 180.
  - Goals: `Goal1 (2665,-3800,140)` and `Goal2 (2926,-3800,140)` lie on the long axis and suit two-team modes.
  - The lower-ring and upper-ring lap checkpoints (sections 1.7 and 1.9).
  - The 25 `TrapsPos` points (section 3.1).
  - The six roof turret points at z 154 (section 3.3).
  - The player start `(2782.99,-3898.15,140.001)`, heading 148.
  - The arena menu marker `(2789.97,-3903.44,139)` and vehicle request spawn `(2785,-3909.6,140)`.
