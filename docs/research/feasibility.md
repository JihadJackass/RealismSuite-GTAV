# RealismSuite: feasibility of the gang, body and clothing features

Scope: the nine features on the roadmap (`suite/docs/ROADMAP.md`), assessed for GTA V Enhanced 2.0.1158.16 with ScriptHookV and SHVDN Enhanced 3.9.0.6, inside the single RealismSuite DLL. No mod code was written.

How sure each claim is:
- **Verified (script)** means it was found in Rockstar's decompiled scripts. The file and line are given. The scripts come from github.com/Sainan/GTA-V-Decompiled-Scripts (master, fetched 2026-11-25), plus the copies already in `/tmp/rs`. Downloaded copies are in `/tmp/dl`.
- **Verified (suite)** means it was read in the suite's own source.
- **Community** means an open-source mod uses it and the source was read. It was not seen in Rockstar's scripts.
- **UNVERIFIED** means it was not confirmed. It has to be tested in game before anything is built on it.

The decompiled scripts don't say which game build they came from. Script global indices (for example `Global_114963`) change between builds, so any global quoted here is UNVERIFIED for Enhanced 1158.16 until it is checked at runtime. Native names, animation dictionaries and clip names don't change between builds.

---

## 1. Summary

| # | Feature | Verdict | Effort | Depends on |
|---|---|---|---|---|
| 1 | Gang wars, SA style (join a gang, turf, waves) | Works with limits | XL | 3 (map), join-a-gang, Police |
| 2 | Drug dealing tied to turf | Works with limits | L | 1, 3 |
| 3 | Turf colours on the map | Works with limits (blip budget is the constraint) | M | BlipBudget |
| 4 | Kidnapping (force into car, hold, ransom) | Works with limits | L | 5, 6, Police, phone |
| 5 | Grabbing and dragging bodies | Works with limits (live peds: good; corpses: needs a stand-in ped) | M | none |
| 6 | Bodies in trunks | Works with limits (per-class offsets; hide ped when lid shut) | M | 5, Trunk.cs |
| 7 | Jerry cans in trunks | Works fully (already implemented in the current source) | S (polish only) | RealFuel, Trunk |
| 8 | Changing clothes at your main car's trunk | Works fully for "saved outfits"; reading owned clothes works with limits | M | Trunk.cs, SPA II wardrobe data |
| 9 | Interaction-menu accessories (hat, glasses, mask) | Works fully | S-M | Police (masks/hats already affect recognition) |

Effort scale: S = a day or two, M = about a week, L = two to three weeks, XL = a month or more, including in-game tuning.

Suggested build order (reasons in section 12):
1. Feature 9 (accessories)
2. Feature 8 (clothes at the trunk)
3. Feature 5 (carry/drag)
4. Feature 6 (body in trunk)
5. Feature 4 (kidnapping)
6. Feature 3 (turf map layer) plus joining a gang
7. Feature 1 (wars)
8. Feature 2 (dealers)

Feature 7 only needs a test pass.

---

## 2. Integration points found in the suite

- **Host.** `src/Suite.cs` registers modules with `Add("Name", () => new ...)`. Each module gets a staggered timer and error isolation. New features become new modules, for example `Gangs`, `Bodies` and `Outfits`.
- **Your own car ("main ride").** `src/Trunk.cs` decides it through `LoadMainRides`, `IsMainModel`, `IsPersonalCar` and `KeyOf`:
  - The story defaults are Michael's tailgater, Franklin's buffalo2 and bagger, and Trevor's bodhi2.
  - Entries from `SwapMainRide.ini` (an external mod's file next to the game folder) are added on top: model, colours and a fixed plate.
  - A car counts as the actual personal car if it has the fixed plate, or carries a personal-vehicle blip (`GET_BLIP_FROM_ENTITY`). Once seen, it's remembered by handle for missions that hide the blip.
  - Stashes are keyed `MAIN|player|model` for main rides and `model|PLATE` for other cars you've driven.
  - `src/RideGuard.cs` reads the same ini.
  - SPA II (`merged/SPAII`) has garages, but the suite treats Swap Main Ride plus the personal-vehicle blip as "your car". Feature 8 should reuse `KeyOf(v).StartsWith("MAIN|")` and not invent a new rule.
- **Trunk detection.** `Trunk.cs` `BootPos` uses bone `"boot"` (with `GET_ENTITY_BONE_INDEX_BY_NAME`) and works at either end of the car. That covers front-trunk cars. The trunk is opened with `SET_VEHICLE_DOOR_OPEN(v, 5, ...)`. Rockstar uses the same test: door 5 exists if bone `"boot"` exists, door 4 if `"bonnet"` exists (fmmc.c:96358-96370, Verified (script)).
- **Jerry can.** `RealFuel.cs` has the `[JerryCan]` ini section, buys with `WEAPON_PETROLCAN`, and fills with `timetable@gardener@filling_can` / `gar_ig_5_filling_can`. `RealFuelScript.CanLitres` (internal static, RealFuel.cs:254) exposes the carried can's petrol. See feature 7.
- **Police.**
  - Map colours are red radius blips for the hot area (`Police.cs` `UpdateHotBlip`: `World.CreateBlip(c, r)`, `BlipColor.Red`, alpha 85/110, `SET_BLIP_DISPLAY 8`, flashing). PoliceComputer uses red and yellow area blips (PoliceComputer.cs:352, 363).
  - The police keep a description of you: 12 drawables, hat prop 0, glasses prop 1 (Police.cs:391-396). A mask (component 1) and a changed shirt or pants (components 11 and 4) defeat recognition (Police.cs:440-450).
  - This makes features 8 and 9 gameplay features, not only cosmetics.
- **Heists.**
  - `src/Heists/Squad.cs` spawns crew, keeps them in the player's group, seats them, and handles cover and fighting.
  - `HeistKit.cs` sets hostile relationship groups (HeistKit.cs:64-65).
  - The phone contact comes from SPA II's iFruit list (`HeistsModule.cs:346-367`).
  - Crew and car blips are Blue (3), loot is Green (2), objectives are Yellow. These are point blips, so they don't clash with translucent turf areas.
- **NPC crowds.** `src/Gambling/Den.cs` spawns peds with `World.CreatePed` and parks them in scenarios with `TASK_START_SCENARIO_IN_PLACE` (Den.cs:236, 714-737). This is the pattern for dealers and turf loiterers.
- **Clothes data already in the DLL.** SPA II is compiled into the same assembly (`build.sh` uses `-recurse:'merged/*.cs'`), so its internal types are directly callable.
  - `merged/SPAII/SPAII/WardrobeList1.cs` and `WardrobeList2.cs` hold 73 lists with about 5,500 `Clothing` entries for Michael, Franklin, Trevor and freemode.
  - Each entry has the game's text label (for example `"TORSO_P0_8_0"`), component, drawable and texture, and for multi-part items up to three components.
  - `Wardrobe.cs` has the menus (`CreateMichaelWardrobeMenu` etc.) and a preview camera. `WardrobeMemory.cs` snapshots and restores a ped's look.
  - Nothing in SPA II checks ownership. Every item is offered.
- **Blip budget.** `src/BlipBudget.cs` documents that `blip_controller` waits while `GET_NUMBER_OF_ACTIVE_BLIPS() >= 150` (blip_controller.c:319, Verified (script)). Its in-memory patch found no match. See section 5 for an open-source mod that solved this on Enhanced 1158.16.

---

## 3. Feature 1: Gang wars (San Andreas style)

**Verdict: works with limits.** Every building block exists in natives. The limits are the ped pool and AI quality during big fights, and the absence of any game-side turf system (it's all ours to simulate).

### Approach
1. **Gangs and their relationship groups.** The game's ambient gangs use these relationship groups. The hashes were computed and found in Rockstar scripts:

   | Group | Hash | Seen in |
   |---|---|---|
   | AMBIENT_GANG_FAMILY | 1166638144 | franklin0.c:52348, fm_hideout_controler.c:12190 |
   | AMBIENT_GANG_BALLAS | -1033021910 | franklin0.c:3125, fm_hideout_controler.c:12211 |
   | AMBIENT_GANG_MEXICAN (Vagos) | 296331235 | bailbond1.c and others; fm_hideout_controler.c table |
   | AMBIENT_GANG_LOST | -1865950624 | fm_hideout_controler.c table |
   | AMBIENT_GANG_MARABUNTE | 2037579709 | same table |
   | AMBIENT_GANG_SALVA | -1821475077 | same table |
   | AMBIENT_GANG_WEICHENG | 1782292358 | same table |
   | AMBIENT_GANG_CULT, AMBIENT_GANG_HILLBILLY | 2017343592, -1285976420 | same table |

   The table is the GTA Online gang-attack script's list of groups (fm_hideout_controler.c:12170-12230).
   - Joining a gang means `SET_RELATIONSHIP_BETWEEN_GROUPS(1 /*respect*/ or 0 /*companion*/, gang, PLAYER)` both ways, as Rockstar does at franklin0.c:52348-52372.
   - Rivals get 5 (hate).
   - This immediately changes how ambient gang peds treat you all over the map. Reset it on leaving the gang, on character switch, and on load.
2. **Gang ped models (Verified (script), joaat strings found):**

   | Gang | Models |
   |---|---|
   | Families | g_m_y_famca_01, g_m_y_famdnf_01, g_m_y_famfor_01 |
   | Ballas | g_m_y_ballaorig_01, g_m_y_ballaeast_01, g_m_y_ballasout_01 |
   | Vagos | g_m_y_mexgoon_01/02/03, g_f_y_vagos_01, g_m_m_mexboss_01/02 |
   | Aztecas | g_m_y_azteca_01 |
   | Lost MC | g_m_y_lost_01/02/03, g_f_y_lost_01 |
   | Salvadorans | g_m_y_salvagoon_01, g_m_y_salvaboss_01 |
   | Korean | g_m_y_korean_01/02, g_m_y_korlieut_01 |
   | Marabunta | g_m_m_maragrande_01 (others UNVERIFIED) |
   | Chinese | g_m_m_chigoon_01/02 |
   | Armenian | g_m_m_armgoon_01 |

3. **Turf.**
   - Use the game's named zones (`GET_NAME_OF_ZONE` returns codes such as DAVIS, STRAW, CHAMH, RANCHO) as turf units, optionally split into sub-blocks. This is exactly what the open-source Gang and Turf Mod does (its TurfZone matches the game zone name).
   - Store owner, strength and dealers per zone in `scripts\RealismSuite\Gangs.dat`.
4. **Starting a war.** Trigger it by killing rival members inside their zone, or by a deliberate "attack" action. The war then runs in waves:
   - Spawn 4-8 rivals per wave off-screen at road or pavement nodes (`GET_NTH_CLOSEST_VEHICLE_NODE`, `GET_SAFE_COORD_FOR_PED`), some arriving by car (gang vehicles, see feature 3).
   - Give them weapons, `SET_PED_RELATIONSHIP_GROUP_HASH`, `TASK_COMBAT_HATED_TARGETS_AROUND_PED` and combat attributes.
   - Cap live hostiles at about 15-20.
   - Win after N waves, or when a ticket pool runs out (Gang and Turf Mod's "tickets" model). Then flip ownership and repaint the blip.
5. **Your backup.** Reuse `Heists/Squad.cs` patterns (player group, seating, follow). Lamar's Online gang call (`am_gang_call.c`, relationship set-up at 18646-19099) is a reference for "call in your homies".
6. **Counter-attacks.** Rivals attack your turf on a timer. Undefended zones flip after a grace period. Simulate this abstractly when you're far away, and only spawn peds when you're near.
7. **Police.** A war is a big shootout. Let the Police module see it through the normal wanted logic. Optionally add `SET_DISPATCH_COPS_FOR_PLAYER` style tuning (UNVERIFIED which knobs Police.cs already uses).

### Risks and limits
- **Ped pool and AI.** Large fights hit the ped pool. They can also hit the melee combat group pool. The LS Gangsta mod tells users to raise `CombatMeleeManager_Groups` PoolSize to 30 in gameconfig.xml ([page](https://www.gta5-mods.com/ms/scripts/ls-gangsta-mod)). Whether an Enhanced gameconfig edit is needed or possible is UNVERIFIED.
- **Ambient spawns.** Ambient gang peds keep spawning by popzone schedule and don't know about our turf. Keep ambient behaviour as is, and add our own loiterers only in zones we own.
- **Story missions.** Story missions set their own relationships (franklin0, the Lamar missions). Pause the module while `Missions.Active` is true and restore our relationships afterwards.
- **Character switches.** Joining is per character. Store it per player model, the same way `Trunk.cs` keys by player model.

### Effort: XL
Turf data, war state machine, spawner, save, AI tuning and balancing. Joining a gang itself is S-M: a contact, a mission or a payment, then the relationship set-up.

---

## 4. Feature 2: Drug dealing tied to turf

**Verdict: works with limits.** Dealers are ordinary persistent peds in a scenario. Income and control are our own simulation. The game has no drug economy in story mode to hook into.

### Approach
- **Hiring.** Hire dealers from your gang contact. Only spawn a dealer ped when you're within about 150 m of his corner. Otherwise he exists as data: zone, corner position, stock, earnings, alive or dead.
- **Ped.** Use a gang model of your gang, `SET_ENTITY_AS_MISSION_ENTITY`, and `TASK_START_SCENARIO_AT_POSITION` or `TASK_START_SCENARIO_IN_PLACE` with `WORLD_HUMAN_DRUG_DEALER`. That scenario name is Verified (script) at gb_contraband_buy.c:441971. Spawning and scenarios follow the existing `Den.cs` crowd pattern.
- **Customers.** Pick nearby ambient peds now and then and walk them up with `TASK_GO_TO_ENTITY`. Play a hand-off: `mp_common` / `givetake1_a` / `givetake1_b` is the usual community pair (UNVERIFIED; not found in the scripts checked).
- **Control.** A zone's strength comes from its dealers and their uptime. Rivals send small hits at corners (a mini-war), and police raids fit the Police module.
- **Money.** Collect money by visiting, or have it paid to the player's money, which is the same wallet the rest of the suite uses.
- **Map.** Show a dealer as a small short-range point blip (`SET_BLIP_AS_SHORT_RANGE`) only near you, to keep within the blip budget.

### Risks
- Persistent peds left everywhere would exhaust pools, so it has to be data-first.
- Ambient peds sometimes refuse tasks or wander off. Use `SET_BLOCKING_OF_NON_TEMPORARY_EVENTS` on the customer during the deal.

### Effort: L
Depends on feature 1's turf model and feature 3's map layer.

---

## 5. Feature 3: Turf colours on the map

**Verdict: works with limits.** Area blips do exactly what's needed. The only real constraint is the blip count and `blip_controller`'s threshold.

### What Rockstar does (Verified (script))
- **Rotated rectangles.** fmmc.c:762285-762297 (`func_13118`) makes a rectangle between two points:

  ```
  iVar5 = HUD::ADD_BLIP_FOR_AREA(centre, width, length);
  HUD::SET_BLIP_ROTATION_WITH_FLOAT(iVar5, heading);
  ```

- **Shaded areas.** fmmc.c:853596-853600 uses `ADD_BLIP_FOR_AREA`, `SET_BLIP_AS_SHORT_RANGE`, `SET_BLIP_ROTATION_WITH_FLOAT`, `SET_BLIP_COLOUR(team colour)` and `SET_BLIP_ALPHA(160)`.
- **Circles.** fmmc.c:762310-762316 uses `ADD_BLIP_FOR_RADIUS`, `SET_BLIP_COLOUR 5`, `SET_BLIP_ALPHA 120`.
- **Integer rotation.** fm_content_stash_house.c:396044 uses `SET_BLIP_ROTATION` with a whole number.
- **Other helpers that exist.**
  - `SET_RADIUS_BLIP_EDGE` (docks_heista.c) draws an outline-only radius.
  - `SET_BLIP_HIDDEN_ON_LEGEND` (blip_controller.c, am_mp_casino.c) keeps 40 turf blips out of the legend.
  - `SET_BLIP_DISPLAY` (Police.cs uses 8 = minimap and pause map).
  - `SET_BLIP_FLASHES` flashes the zone under attack, as Police already does for searches.
- **GTA Online gang attacks.** fm_hideout_controler.c marks gang attacks with point and ped blips, not areas. There is no Online "gang turf area" system to copy.

### Colours
Blip colour IDs are from the [FiveM blip reference](https://docs.fivem.net/docs/game-references/blips/). The names are approximate and several IDs repeat a name.
- **Avoid (Police / PoliceComputer / objectives):**
  - reds: 1, 6, 35, 49, 59, 75, 76, 79
  - oranges: 9, 17, 21, 44, 47, 64, 81
  - yellows: 5, 16, 20, 28, 33, 46, 60, 66, 70, 71, 73
- **Proposed palette:**

| Gang | Map colour (ID) | Reason |
|---|---|---|
| Families | Green (2) or Forest Green (25) | Families wear green |
| Ballas | Bright Purple (27) or Purple (83) | Ballas wear purple |
| Vagos | **Design decision.** Their colour is yellow, which is reserved. Suggest Pink (8) or Light Brown (10) and accept that it doesn't match | Conflict |
| Aztecas | Cyan (15) or Light Blue (18) | Aztecas wear light blue |
| Marabunta Grande | Blue (38) or Dark Blue (29) | wear blue |
| Lost MC | Dark Gray (40) or Silver (55) | black/white biker look |
| Salvadorans | Electric Blue (26) (UNVERIFIED match) | |
| Triads/Wei Cheng | Light Pink (23) or Mulberry Pink (61) | none canonical |

- **Matching the cars.** Make gang cars match the map colour, not the other way round.
  - Set `SET_VEHICLE_COLOURS` on cars we spawn.
  - Optionally recolour ambient gang cars near you. Which cars the game gives each gang is UNVERIFIED (not checked in vehicles.meta), so put model lists in an ini.
  - The open-source Gang and Turf Mod has exactly this per gang: blip colour plus car colours, `GangBlipColorSubMenu.cs` and `GangCarColorsSubMenu.cs`.

### The blip budget
- **The 150 threshold.** `blip_controller` will not add more shop, activity or mission icons while 150 or more blips exist (blip_controller.c:319). Whether area and radius blips count in `GET_NUMBER_OF_ACTIVE_BLIPS` is UNVERIFIED. Assume yes, and measure with BlipBudget's existing log.
- **The engine pool.** The engine pool on Enhanced 1158.16 was measured at 2,000 slots by the BlipLimitFix author (claim from their README, not re-measured).
- **Recommended design: coarse turf.**
  - Use about 25-40 turf zones, mainly south and east Los Santos plus a few Blaine County ones for the Lost.
  - Draw each zone as 1-3 rotated rectangles. That's about 40-80 blips.
  - Turn on `SET_BLIP_HIDDEN_ON_LEGEND` and short-range for distant ones.
  - That fits under 150 only if the rest of the map is light. So also:
- **Option B: show turf only on the pause map.** Create the area blips when `IS_PAUSE_MENU_ACTIVE` turns true and delete them when it closes. Show only your current zone's and nearby zones' blips on the minimap. Two things are UNVERIFIED: whether scripts run while the pause map is open (SHVDN ticks probably continue), and whether `blip_controller` happens to check during that window.
- **Option C: lift the threshold.**
  - **BlipLimitFix [Enhanced]** by rootBrz is open source under GPL-3.0 ([gta5-mods](https://www.gta5-mods.com/scripts/blip-limit-fix), github.com/rootBrz/gtav-blip-limit-fix). It is game-tested on Enhanced 1.0.1158.16 and raises the `blip_controller` threshold to a configurable value (default 1900).
  - Its README explains why BlipBudget's byte pattern found nothing. The threshold is not always 150 (some setups show 240), and it decodes the VM loop structurally instead of matching bytes. It finds the script program registry with the locators `44 88 05 ? ? ? ? 48 89 F9` (GEN9) and `48 8D 0D ? ? ? ? 41 8B D6 E8 ? ? ? ? FF 05` (GEN8), registry offset 0xD8.
  - Either recommend users install it, or re-implement the idea in BlipBudget. Copying its code would bring GPL-3.0 obligations.
- **Not practical: drawing our own polygons on the pause map.** No SP-accessible native draws custom shapes on the frontend map. FiveM's `ADD_MINIMAP_OVERLAY` is a CitizenFX-only extension, not a game native. Drawing sprites over the pause map with `DRAW_*` would need the map's pan and zoom, which aren't exposed. This conclusion is UNVERIFIED that nothing else exists, but nothing was found.

### Effort: M
Zone authoring is most of it: rectangles per zone, a small authoring tool or a text file.

---

## 6. Feature 4: Kidnapping

**Verdict: works with limits.** Getting a live ped to come with you and sit in your car is well supported: Rockstar's own bail-bond missions do exactly this. A scripted "grab and throw into a vehicle" only exists as an authored scene for one van model. Holding and ransom are our own logic.

### Approach (with Rockstar references)
1. **Make them give up.** Aim at them, then call `TASK_HANDS_UP(ped, -1, PLAYER_PED_ID(), -1, 0)` (bailbond1.c:38160) and `UPDATE_TASK_HANDS_UP_DURATION` (38003, 38156).
2. **Make them come with you.** This is Rockstar's surrendered-bounty set-up, bailbond1.c:38012-38037:
   - `SET_PED_AS_GROUP_MEMBER(ped, playerGroup)`, then `SET_GROUP_FORMATION(group, 0)`, `SET_GROUP_FORMATION_SPACING(1.5, 3.5, -1)`, `SET_PED_GROUP_MEMBER_PASSENGER_INDEX(ped, 0)`.
   - Path flags.
   - `SET_PED_CONFIG_FLAG` 167, 118 and 42 = true.
   - `SET_ENTITY_ONLY_DAMAGED_BY_PLAYER`.
   - `SET_PED_FLEE_ATTRIBUTES(ped, 1, false)`, `SET_PED_COMBAT_ATTRIBUTES` 17 = true and 0 = false.
   - `SET_PED_RESET_FLAG 237`.
   - `SET_BLOCKING_OF_NON_TEMPORARY_EVENTS(ped, true)`.

   The meaning of each config flag number is UNVERIFIED. Copy them as Rockstar uses them.
3. **Into the car.** When you're in a car below 1.75 m/s and close, they run `TASK_ENTER_VEHICLE(ped, veh, 25000, seat, 2.0, 1048577, 0)` after checking `IS_VEHICLE_SEAT_FREE` and `IS_ENTRY_POINT_FOR_SEAT_CLEAR` (bailbond1.c:38053-38076).
4. **Snatch scene (vans only).** re_snatched.c (the Lost MC kidnapping random event) uses dictionary `random@kidnap_girl`:
   - `CREATE_SYNCHRONIZED_SCENE` at the van, attached to bone `"chassis"` with `ATTACH_SYNCHRONIZED_SCENE_TO_ENTITY` (re_snatched.c:407, 727-735, 8821-8827).
   - `TASK_SYNCHRONIZED_SCENE` with clips `IG_1_GIRL_DRAG_INTO_VAN`, `IG_1_GUY1_DRAG_INTO_VAN`, `IG_1_GUY2_DRAG_INTO_VAN`.
   - `PLAY_SYNCHRONIZED_ENTITY_ANIM(van, scene, "DRAG_INTO_VAN_BURR", ...)` (8857-8867).
   - The van model is `gburrito` (re_snatched.c:410).
   - It's authored for the Burrito body only, so it's a nice extra for a "kidnap van", not a general solution.
5. **Restraint and human shield.**
   - GTA Online's Doomsday hostage uses `anim@GangOps@Hostage@` with clips `perp_idle`, `victim_idle`, `victim_success`, `perp_fail` (fmmc.c:730911-730950, 731077). It runs in a networked synced scene. In SP, use `CREATE_SYNCHRONIZED_SCENE` + `TASK_SYNCHRONIZED_SCENE`, or attach + `TASK_PLAY_ANIM`. That this MP dictionary loads in SP on Enhanced is UNVERIFIED but likely.
   - Bank-heist hostage cowering: `anim@heists@ornate_bank@hostages@*` and `anim@heists@fleeca_bank@hostages@*` (fmmc.c).
   - Cuffed or tied idle: `mp_arresting` / `idle` and `mp_arrest_paired` (Community, qb-policejob; UNVERIFIED in Rockstar scripts).
6. **Holding and ransom.**
   - Take them to an owned place: an SPA II property, the gang hideout, or the Den.
   - Keep them seated or kneeling with a scenario or anim.
   - Phone the family through an iFruit contact: reuse the Heists module's contact code, which shares SPA II's phone list.
   - Run a ransom timer and a drop-off.
   - Police: witnesses within line of sight report it. This hooks into Police's existing reporting.
7. **Trunk.** See feature 6.

### Risks
- **Persistence.** A captive is a live ped. The game doesn't save it, so on save or load or a character switch either release them or turn them into data (as in feature 6).
- **Ambient peds.** Ambient peds are cleaned up when far away unless they are mission entities.
- **Group cap.** The player's group holds a limited number of members (7 followers plus the leader is the usual figure, UNVERIFIED). Hostages share it with heist crew.

### Existing mods
- **Kidnap** by jedijosh920 (2016): passenger kidnapping and sacrifice at the Altruists. No ransom. Source not published ([page](https://www.gta5-mods.com/scripts/kidnap)).
- **Kidnap V** by avivr (2024): van, iFruitAddon2 contact, torture scenes. The author says it's buggy and abandoned. Source not stated ([page](https://www.gta5-mods.com/scripts/kidnap-v)).

### Effort: L

---

## 7. Feature 5: Grabbing and dragging bodies

**Verdict: works with limits.** Carrying a **live** ped over the shoulder is a solved problem with game animations. A **dead** ped ignores tasks and animations, so for corpses either use a stand-in ped posed as dead (Rockstar's own trick) or drag the real ragdoll with a physical attachment.

### Animations

| Use | Dictionary / clip | Flags | Evidence |
|---|---|---|---|
| Carrier, fireman's carry | `missfinale_c2mcs_1` / `fin_c2_mcs_1_camman` | 49 (upper body + loop + keep moving) | Rockstar: finalec2.c:6033 (dict), 123521-123529 (clips `FIN_C2_MCS_1_CAMMAN` and `FIN_C2_MCS_1_STEVE` in a synced scene). Community use with flag 49: rubbertoe98 CarryPeople, qb-policejob:342 |
| Carried ped | `nm` / `firemans_carry` | 33 | Community: rubbertoe98 `cl_carry.lua`, qb-policejob:316-322, RageMP-Player-carry. Not seen in Rockstar scripts (it's a NaturalMotion dictionary) |
| Attach for the carry | `ATTACH_ENTITY_TO_ENTITY(carried, carrier, 0, 0.27, 0.15, 0.63, 0.5, 0.5, 180, false, false, false, false, 2, false)` | | Community (rubbertoe98; qb-policejob:321) |
| Corpse pose | `dead` / `dead_a` ... `dead_h` | `TASK_PLAY_ANIM ... flag 1`, or synced scene flags 145 | Rockstar: armenian2.c:67636 (`dead_a`), carsteal3.c:109646 (`dead_d`, on a **live** ped with `SET_ENTITY_INVINCIBLE`, `STOP_PED_SPEAKING`, `SET_BLOCKING_OF_NON_TEMPORARY_EVENTS`), fmmc.c:64607 (`TASK_PLAY_ANIM_ADVANCED dead/dead_a`) |
| Dragging by the feet (physics) | `ATTACH_ENTITY_TO_ENTITY_PHYSICALLY(ped, other, GET_PED_BONE_INDEX(ped, 52301 /*R foot*/), -1, offsets..., breakForce -1, ...)`, a second call for bone 14201 (L foot) | | Rockstar: michael2.c:3169-3170, 49107 ("Fresh Meat", Michael hung by the ankles) |
| Michael being dragged (cutscene-like) | `missfam5mcs_6drag` / `fam_5_mcs_6_drag_michael` + cam `fam_5_mcs_6_drag_cam` | synced scene | family5.c:98199-98374 (authored for one location; not reusable as a loop) |
| Player dragging a ped | `combat@drag_ped@` / `injured_drag_plyr` and `injured_drag_ped` | | **UNVERIFIED.** Not found in any of the 100+ downloaded scripts. Known only from community use. Test that it loads on Enhanced |
| Carrying loot to the boot | `carry_to_boot_l` / `carry_to_boot_r` (gold) | | finale_heist2a.c (gold bars, not bodies) |

The user's guess `missfbi3_party` is **not** a trunk animation. It is the coke-snorting party in "By the Book"/fbi3 (fbi3.c:2784, clips `Snort_Coke_A_*`).

### Recommended method
- **Live ped (hostage, knocked out).**
  - Freeze AI with `SET_BLOCKING_OF_NON_TEMPORARY_EVENTS` and `SET_PED_CAN_RAGDOLL(false)` while carried.
  - Attach to the player, then `TASK_PLAY_ANIM nm/firemans_carry` with flag 33, and play the carrier clip on the player with flag 49.
  - `SET_ENTITY_NO_COLLISION_ENTITY` between the two.
  - Each tick, re-apply the animation if `IS_ENTITY_PLAYING_ANIM` is false (as the community scripts do).
  - Block sprint, jump, cover, entering vehicles and weapons.
  - Drop on damage, ragdoll, water or ladders.
- **Corpse.** Two options:
  - (a) **Stand-in ped.** Delete the corpse and spawn `CLONE_PED` of it alive, invincible and blocking events, playing `dead/dead_x` or the carried clip. Kill it again or re-pose it when dropped. Blood and damage decals probably don't copy (UNVERIFIED).
  - (b) **Real ragdoll dragged.** Keep the real corpse, attach its foot or hand bones physically to the player's hand. The player plays a dragging walk (the dictionary is UNVERIFIED, see above). The body slides as a real ragdoll. Risk: physics jitter or "exploding" ragdolls on stairs and kerbs; needs tuning of break force and offsets.
  - Rockstar itself uses live "posed dead" peds for bodies that must stay put (carsteal3).
- **Making the corpse stay.** Make it a mission entity so the game doesn't clean it up (`SET_ENTITY_AS_MISSION_ENTITY`).

### Risks on Enhanced/SHVDN3
- Dead peds can't be tasked.
- First-person camera clips into the carried body.
- Heavy or odd skeletons (animals, some scenario peds) fail. The Carry Ped mod reports the carried animation failing on scenario peds.
- Using `missfinale_c2mcs_1` outside its cutscene works in the community mods. Enhanced is assumed but UNVERIFIED.

### Existing mods

| Mod | Notes | Source |
|---|---|---|
| Carry Ped – Carry NPC's (ediotTV, 2025) | Custom carry animation in dlc.rpf; earlier versions used heist box-carry anims | Not open; "modify with credit" ([page](https://www.gta5-mods.com/ko/scripts/carry-ped-carry-npc-s)) |
| Pick Up Bodies And Put Them In A Trunk (w3irdo, 2024) | Tie up, carry, trunk; users report crashes; can't pick up corpses | Not open ([page](https://www.gta5-mods.com/vi/scripts/pick-up-bodies-and-put-them-in-a-trunk)) |
| Carry Bodies + Put Bodies In Vehicles | Not inspected (redirect) | UNVERIFIED ([page](https://www.gta5-mods.com/el/scripts/carry-bodies-put-bodies-in-vehicles)) |
| FiveM CarryPeople (rubbertoe98) | Fireman's carry, values above | Open (GitHub `rubbertoe98/FiveM-Scripts/CarryPeople`) |
| qb-policejob (QBCore) | Escort (attach bone 11816 at 0.45, 0.45, 0) and carry | Open source; GPL-3.0 assumed for QBCore resources (only qb-radialmenu's licence was checked) |

### Effort: M
Live carry: S. Corpse handling and tuning: M.

---

## 8. Feature 6: Bodies in trunks

**Verdict: works with limits.** There's no trunk-occupant system in the game. Rockstar's only "person in a trunk" (Devin in the finale) is a cutscene. The community approach is attach + a lying animation + per-class offsets, and it works, with clipping on small cars.

### Evidence
- **Devin's trunk is a cutscene.** finalec2.c:4453 and 4830 request cutscene `fin_ext_p1` (`func_285` is the cutscene loader). It plays sound `FINALE_C2_DEVIN_INSIDE_TRUNK_MASTER` from the car (4500, 5210). The script never attaches a Devin ped into the boot.
- **The cutscene's dictionary works as an animation.** Its sections exist as dictionaries named `<cutscene>-<section>`. The community uses `fin_ext_p1-7` / `cs_devin_dual-7` as a "lying in the trunk" pose. qb-radialmenu `client/trunk.lua:112-117` (GPL-3.0):

  ```
  TaskPlayAnim(ped, 'fin_ext_p1-7', 'cs_devin_dual-7', 8.0, 8.0, -1, 1, 999.0, 0, 0, 0)
  AttachEntityToEntity(ped, veh, 0, x, y, z, 0, 0, 40.0, 1, 1, 1, 1, 1, 1)
  ```

  Offsets per vehicle class come from qb-radialmenu `config.lua:710-733`:
  - Compacts and Coupes: y -1.5
  - Sedans, Muscle, Sports Classics, Sports, Super: y -2.0
  - SUVs, Off-road, Vans, Utility and the rest: y -1.0, z 0.25
  - Motorcycles: not allowed

  qb also keeps a disabled list of mid-engine cars (penetrator, vacca, monroe, turismor, osiris, comet*, zentorno, infernus, bullet and others), whose "boot" is a small front trunk.
- **Other lying poses.** `timetable@floyd@cryingonbed@base` exists (main.c:154258, Floyd's ambient on the bed) and is used by some community trunk scripts as a lying pose (UNVERIFIED for trunk use). `dead/dead_*` (above) works for corpses.

### Approach
1. Only cars with a `"boot"` bone (the same test as `Trunk.cs` and fmmc.c:96367) whose boot is at the rear and whose class is a car. Refuse bikes, vans without a boot door (the Burrito's rear doors are doors 2/3, so use seats or the re_snatched scene), and mid-engine cars (ini list).
2. Open door 5, play a short "put in" animation, then attach the ped to the vehicle:
   - Use a root offset derived from the `"boot"` bone (`GET_OFFSET_FROM_ENTITY_GIVEN_WORLD_COORDS(veh, BootPos(veh))` plus class tweaks) instead of fixed class numbers. This adapts per model.
   - Turn collision off on the attach.
   - Play the trunk pose (alive) or `dead_*` (stand-in corpse).
   - Close the lid.
3. **Hide the occupant while the lid is shut.** `SET_ENTITY_VISIBLE(false)` while `GET_VEHICLE_DOOR_ANGLE_RATIO(veh, 5) < 0.1`. That removes nearly all clipping problems on coupes and sports cars. Show them when the lid opens.
4. **Integration with the trunk stash.** Treat a body as a stash item under the same `KeyOf(vehicle)` key ("Body: model + variation"). While you're near, it's a real attached ped. When the car despawns or the game saves, it becomes data and is re-spawned on approach. This also survives the "main ride respawned with a new plate" case that `Trunk.cs` already handles with `MAIN|` keys. One body per trunk; two in large SUVs (ini).
5. Kidnap victims in the trunk try to escape on a timer. They can be heard (Police witness hook). Taking them out reverses the steps.

### Risks
- Crashes reported in other trunk mods are likely from deleting or attaching entities mid-task. Detach before deleting, and never re-attach while a ped is ragdolling.
- `fin_ext_p1-7` streaming on Enhanced is UNVERIFIED (works on FiveM's Legacy-based client).
- Destroying the car should kill or delete the occupant, the same as stashes are lost (`Trunk.cs` already detects destroyed cars).

### Effort: M
Depends on feature 5's stand-in corpse logic and on `Trunk.cs`.

---

## 9. Feature 7: Jerry cans in trunks

**Verdict: works fully. This is already implemented in the current source** (`Trunk.cs` and `RealFuel.cs` were updated during this research; ROADMAP marks it done).

- `Stored.Litres` was added (Trunk.cs:30) and is saved as the 7th field (Trunk.cs:261, 279).
- Storing a `WEAPON_PETROLCAN` moves `RealFuelScript.CanLitres` into the stored can and zeroes the carried value (Trunk.cs:495).
- Taking one back refuses if you already carry a can, and otherwise restores its litres (Trunk.cs:508-512). The log line shows litres.
- `RealFuelScript.CanLitres` (RealFuel.cs:254) clamps to `JerryCanLitres` and marks the save dirty.

Remaining polish (S):
- If the RealFuel module failed to start, `CanLitres` reads 0, so a stored can is saved empty. Consider refusing to store cans when `Instance == null`.
- `WEAPON_PETROLCAN` falls in "Other", not a named weapon group, in the trunk list.
- RealFuel leaves the can alone during story missions (RealFuel.cs:567-568). Storing or taking a can mid-mission should probably also be blocked, or a mission-given can would be lost.
- Loose ammo boxes (ammo by type with `GET_PED_AMMO_BY_TYPE` / `SET_PED_AMMO_BY_TYPE`, which `Trunk.cs` already uses) would be a small extension if "storing ammo" is wanted separately from guns.

---

## 10. Feature 8: Changing clothes at your main car's trunk

**Verdict: works fully as "outfits you've saved". Reading which clothes the character owns works with limits** (build-specific script globals).

### How Rockstar stores and sets story clothes (Verified (script))
- **Natives.**
  - `SET_PED_COMPONENT_VARIATION(ped, component, drawable, texture, palette)` and `SET_PED_PROP_INDEX(ped, anchor, drawable, texture, attach, ...)` / `CLEAR_PED_PROP` (clothes_shop_sp.c:53370, 54002-54006, 134625-134628).
  - Components: 0 head, 1 berd/mask, 2 hair, 3 torso, 4 legs, 5 hand, 6 feet, 7 teeth/accessory, 8 special, 9 special2, 10 decal, 11 jbib.
  - Prop anchors: 0 head (hats), 1 eyes (glasses), 2 ears.
- **Item catalogue.** Every item has a text label of the form `<COMP>_P<char>_<drawable>_<texture>`, for example `TORSO_P0_8_0`, `BERD_P0_1_0`, `PROPS_P0_E...`. Outfits are `OUTFIT_P0_n`. The table is compiled into each shop and mission script (clothes_shop_sp.c:354340 `OUTFIT_P0_0`, 355081 `BERD_P0_1_0`, 358624 `TORSO_P0_8_0`; the same tables in player_controller.c:22308, 23049, 26592).
- **SPA II already has this catalogue** (WardrobeList1/2.cs, about 5,500 entries, same labels). There's no need to extract it again.
- **Ownership.** clothes_shop_sp.c:12525-13040 (`func_96(compType, character 0-2, wordIndex, set, bit, doSet, doClear)`) reads and writes bitsets at `Global_114963.f_2370[character /*164 ints*/].f_(4*compType)[...]`.
  - Callers at 11621-11629 treat set 0 → item flag bit 1, set 1 → flag bit 2, and "not set 2" → flag bit 4.
  - Matching Rockstar's usual item flags, that's probably set 0 = available, set 1 = acquired (owned), set 2 = viewed or not new. This mapping is inferred, UNVERIFIED.
  - `Global_114963` is the story-mode saved-globals block.
- **Reading it from SHVDN3.** SHVDN3 can read script globals (`GTA.Native.GlobalVariable.Get(index)`, API name UNVERIFIED for the Enhanced 3.9.0.6 build). The index 114963 and the offsets are tied to whichever build the decompile came from. They must be found and checked on 2.0.1158.16, for example by confirming that bits for the default outfit (`OUTFIT_P0_0`) read as owned. A wrong index reads garbage or crashes, so it needs a sanity check and an off switch.

### Approach
- **Where.** In `Trunk.cs`'s panel, add a "Clothes" tab shown only when `KeyOf(open).StartsWith("MAIN|")`, so only your actual main ride, never a random car.
- **What's in the trunk.** Outfits you put there:
  - "Pack current outfit" snapshots the 12 drawables, textures and palettes plus 3 props (like `WardrobeMemory`), stored per character in the stash file under the MAIN key.
  - "Wear" applies it.
  - This sidesteps ownership entirely and is the most realistic option: you can only change into what you packed. Optionally let SPA II wardrobes add "pack to car" for any wardrobe item.
- **Owned items (optional).** If the global read is verified, filter SPA II's catalogue by the acquired bit. Otherwise offer only packed outfits.
- **Animation.** Short changing animations exist: `clothingtie` (`try_tie_*`, `check_out_a`), `clothingspecs`, `mp_clothing@female@*`, and the clothes shop's own (clothes_shop_sp.c:218885, 235510 `clothingspecs`). Fade the screen for full changes. Rockstar does a fade in the shop.
- **Police tie-in.** Changing components 11 and 4 out of sight clears "outfit known" (Police.cs:440-444). Changing at a car parked out of view becomes the realistic way to shake a description. Check that no cop has line of sight, which Police.cs already does somewhere ("change it out of their sight").

### Risks
- **Character switches.** Story character switches restore clothes from the game's own state. That outfits set by natives persist through switches and saves is expected (SPA II's wardrobe works that way) but UNVERIFIED.
- **Missions.** Some missions force outfits. Block the tab while `Missions.Active`.

### Effort: M
Plus S if the ownership read is pursued and verified.

---

## 11. Feature 9: Interaction-menu accessories

**Verdict: works fully.** Toggling or swapping hat, glasses and mask is plain natives plus short animations. GTA Online's own menu (`am_pi_menu.c`) is MP-specific, but its animations are usable.

### Slots
- **Hat:** prop anchor 0. Remember `GET_PED_PROP_INDEX` and `GET_PED_PROP_TEXTURE_INDEX` before `CLEAR_PED_PROP(ped, 0)`, and restore with `SET_PED_PROP_INDEX`.
- **Glasses:** prop 1. **Earrings and earpieces:** prop 2.
- **Mask:** component 1 (story masks carry `BERD_P0/1/2_*` labels, which confirms component 1 for story characters). Off means drawable 0.
- **Optional extras:** gloves are part of torso component 3. Bags and parachute are component 5 on story characters (the shop clears component 5 at clothes_shop_sp.c:54002).
- **Choosing which hat.** "Swap to another hat" needs a list. Use SPA II's catalogue for that character (hats, caps, glasses lists), or only items you've worn (remembered when seen on the ped). The owned-items question is the same as feature 8.

### Animations
- **Masks, Verified (script).** Clip `put_on_mask` (fmmc.c:426470), with the dictionary chosen by situation (fmmc.c:426483-426563):
  - `mp_masks@on_foot`
  - `mp_masks@standard_car@ds@`, `@ps@`, `@rds@`, `@rps@`
  - `mp_masks@low_car@*`, `mp_masks@van@*`, `mp_masks@truck@*`, `mp_masks@jeep@*`
- **Glasses.** `clothingspecs` (used by clothes_shop_sp.c).
- **Hat off.** `missheist_agency2ahelmet` / `take_off_helmet_stand`, and `mp_masks@standard_car@ds@` / `put_on_mask` for putting a hat on (Community, qb-radialmenu `client/clothing.lua:724-743`). Not checked in Rockstar scripts.
- Play with upper-body flags (48/49) so you can keep walking.

### Menu
LemonUI is already referenced by the build. A small radial or list menu on a key, disabled in cutscenes, Busy states and the trunk panel.

### Police tie-in
- Mask on: `Masked()` (Police.cs:447-450).
- Hat or glasses change: compared against `d.Hat` and `d.Glasses`.
- This is the main gameplay value of the feature. Also consider whether a mask on in public should attract police attention.

### Existing mods
- **MaskEquipper** (danistheman262, 2022): mask toggle with animation for story characters. Source not stated ([page](https://www.gta5-mods.com/hu/scripts/maskequipper)).
- **MP Interaction Menu For SP** (CruelMasterMC, 2024): outfits, inventory, LemonUI. Source not stated ([page](https://www.gta5-mods.com/ca/scripts/mp-interaction-menu-for-sp)).
- **qb-radialmenu**: clothing toggles with animations. GPL-3.0.

### Effort: S-M

---

## 12. Dependencies and build order

```
join-a-gang ──► (3) turf map ──► (1) wars ──► (2) dealers
(5) carry/drag ──► (6) body in trunk ──► (4) kidnapping ◄── Police witnesses, phone (Heists/SPA II iFruit)
Trunk.cs ──► (6), (7 done), (8)
SPA II wardrobe data ──► (8), (9)
Police description ──► (8), (9) gameplay value
BlipBudget / BlipLimitFix ──► (3)
```

Order and reasons:
1. **9, accessories (S-M).** Self-contained, immediate gameplay through Police, and it builds the outfit and prop snapshot code that 8 needs.
2. **8, clothes at the main-ride trunk (M).** Reuses the Trunk panel and the MAIN-key logic, and finishes the "shake your description" loop. Do the ownership-global investigation in parallel, as optional.
3. **5, carry/drag (M).** The foundation for 6 and 4. Start with live peds, then add the corpse stand-in.
4. **6, body in trunk (M).** The stash-as-data pattern carries over from `Trunk.cs`.
5. **4, kidnapping (L).** Combines 5 and 6 with Rockstar's bail-bond follow logic and the phone.
6. **3, turf map layer plus joining a gang (M + S-M).** Settle the blip strategy first: measure whether area blips count, and decide on BlipLimitFix or our own patch.
7. **1, wars (XL).**
8. **2, dealers (L).**
- **7** is already done; give it a short test pass.

---

## 13. Mods worth studying

| Mod | Relevance | Open source | Link |
|---|---|---|---|
| Gang and Turf Mod (lucasvinbr), v2.1.0, Jan 2025, SHVDN3 + LemonUI | Closest to features 1-3: zones keyed by game zone name, per-gang blip and car colours, wars with tickets and spawn points, shows area blips only for the 5 nearest zones to save blips (`ZoneManager.RefreshZoneBlips`) | Yes, MIT (github.com/lucasvinbr/GTA5GangMod, cloned to `/tmp/ext/GTA5GangMod`) | https://www.gta5-mods.com/de/scripts/gang-and-turf-mod |
| LS Gangsta Mod (RUSMarkB), v1.8, Feb 2024 | Join a gang at its blip, 22 capturable territories, CombatMeleeManager pool note; reported broken on current hooks | Not stated | https://www.gta5-mods.com/ms/scripts/ls-gangsta-mod |
| BlipLimitFix [Enhanced] (rootBrz), Oct 2026 | Raises blip_controller's threshold on Enhanced 1158.16; explains BlipBudget's failed patch | Yes, GPL-3.0 (cloned to `/tmp/ext/gtav-blip-limit-fix`) | https://www.gta5-mods.com/scripts/blip-limit-fix |
| FiveM CarryPeople (rubbertoe98) | Fireman's carry values | Yes (GitHub) | https://forum.cfx.re/t/release-carry-people-over-your-shoulder/788958 |
| qb-radialmenu (QBCore) | Ped-in-trunk anim, per-class offsets, mid-engine exclusions, clothing toggles with animations | Yes, GPL-3.0 | github.com/qbcore-framework/qb-radialmenu |
| qb-policejob (QBCore) | Escort and carry attach values, cuff animations | Yes (licence not checked) | github.com/qbcore-framework/qb-policejob |
| Carry Ped – Carry NPC's (ediotTV) | SP carry with a custom animation | No | https://www.gta5-mods.com/ko/scripts/carry-ped-carry-npc-s |
| Pick Up Bodies And Put Them In A Trunk (w3irdo) | SP hostage, carry and trunk; crash reports | No | https://www.gta5-mods.com/vi/scripts/pick-up-bodies-and-put-them-in-a-trunk |
| Kidnap (jedijosh920), Kidnap V (avivr) | SP kidnapping designs | No / not stated | https://www.gta5-mods.com/scripts/kidnap, https://www.gta5-mods.com/scripts/kidnap-v |
| MaskEquipper, MP Interaction Menu For SP | Accessory and interaction menus in SP | Not stated | see feature 9 |
| SPA II (already merged) | Wardrobe catalogue, preview camera, look snapshot | Decompiled into the suite | `merged/SPAII` |

---

## 14. Open items to verify in game first

1. Whether area and radius blips count in `GET_NUMBER_OF_ACTIVE_BLIPS`, and whether `blip_controller` keeps running while the pause map is open. Measure with BlipBudget's log.
2. That these dictionaries stream on Enhanced 1158.16:
   - `missfinale_c2mcs_1`, `nm` (`firemans_carry`)
   - `fin_ext_p1-7` (`cs_devin_dual-7`)
   - `combat@drag_ped@`
   - `anim@GangOps@Hostage@`
   - `mp_masks@on_foot`
   - `mp_common` (`givetake1_a`)
3. Whether `CLONE_PED` of a corpse keeps blood and damage, and how an attached dead ped looks.
4. The SP clothing ownership global on 2.0.1158.16 (index and layout), and the SHVDN3 Enhanced API for reading globals.
5. Whether outfits set by natives survive a character switch and a save/load.
6. The meanings of bail-bond config flags 167, 118, 42 and reset flag 237.
7. Gang vehicle models and colours per gang (vehicles.meta / popgroups), for matching car and map colours.
