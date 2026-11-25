# Verification of FEASIBILITY.md

I checked every concrete claim in `/home/claude/plans/FEASIBILITY.md` against its primary source myself. That meant the decompiled scripts in `/tmp/dl` and `/tmp/rs` (Sainan's master), the suite source in `/home/claude/suite`, the cloned repos in `/tmp/ext`, the live GitHub raw files (qb-radialmenu, qb-policejob, rubbertoe98 CarryPeople) and the gta5-mods pages. Line numbers below are the ones I saw. "Report" means FEASIBILITY.md.

Verdicts: CONFIRMED, WRONG, PARTLY (true but incomplete or imprecise in a way that matters), UNVERIFIABLE (no primary source available here; needs an in-game test).

Note: while I was checking, two new files appeared in the suite (`src/Accessories.cs`, `src/TrunkWardrobe.cs`, registered at `Suite.cs:93`). That means features 9 and 8 are already being built. Where a finding affects that code, I say so in section 2.

---

## 1. Claims checked

### 1a. The suite's own code

| # | Claim | Verdict | Evidence | Correction / note |
|---|---|---|---|---|
| S1 | Modules are registered with `Add("Name", () => new ...)` | CONFIRMED | `src/Suite.cs:81-106` | |
| S2 | The story main rides are tailgater (Michael), buffalo2 + bagger (Franklin), bodhi2 (Trevor) | CONFIRMED | `Trunk.cs:134-136` | bagger is a motorcycle and bodhi2 is a pickup. See missed issue M9. |
| S3 | `SwapMainRide.ini` sits next to the game folder and adds model, colours and plate | CONFIRMED | `Trunk.cs:139-170` (keys ReplaceModel, PrimaryColor, SecondaryColor, PlateText) | |
| S4 | A car is the personal car if it has the fixed plate **or** a personal-vehicle blip, and is remembered by handle | PARTLY | `Trunk.cs:181-194` | If the ini gives a plate, **only** the plate counts; the blip test is skipped. The blip test is `GET_BLIP_FROM_ENTITY != 0`, which matches *any* blip on the car, not specifically the personal-vehicle sprite. It is limited to main-ride models, so the risk is low. |
| S5 | Stash keys are `MAIN|player|model` and `model|PLATE` | CONFIRMED | `Trunk.cs:174, 210-215` | |
| S6 | RideGuard reads the same ini | CONFIRMED | `RideGuard.cs:2, 35` | |
| S7 | `BootPos` uses bone "boot" and works at either end of the car; the trunk opens door 5 | CONFIRMED | `Trunk.cs:288-293, 305-309, 414` | Door 5 is always opened, even when "boot" is at the front. |
| S8 | Jerry cans: `[JerryCan]` ini section, `timetable@gardener@filling_can` / `gar_ig_5_filling_can`, `CanLitres` at RealFuel.cs:254 | CONFIRMED | `RealFuel.cs:183-187, 253-258, 762, 869` | The can is bought with SHVDN's `WeaponHash.PetrolCan` (`RealFuel.cs:860`). Its joaat is 883325847, the same as `WEAPON_PETROLCAN`. |
| S9 | `Stored.Litres` at Trunk.cs:30, saved as 7th field (261, 279); store at 495; take-back refusal at 508-512 | CONFIRMED | `Trunk.cs:30, 261, 279, 495, 507-512` | |
| S10 | RealFuel leaves the can alone during missions at RealFuel.cs:567-568 | PARTLY | It is actually at `RealFuel.cs:575-576` (`!RealismSuite.Missions.Active`) | Wrong lines only. |
| S11 | If RealFuel didn't start, `CanLitres` reads 0 | CONFIRMED | `RealFuel.cs:256-257` | Setting it also does nothing, so taking a can back would silently lose its petrol too. |
| S12 | Police hot area is a red radius blip, alpha 85/110, `SET_BLIP_DISPLAY 8`, flashing | CONFIRMED | `Police.cs:1436-1471` | |
| S13 | PoliceComputer uses red and yellow area blips at 352 and 363 | CONFIRMED | `PoliceComputer.cs:351-352, 362-363` | It also uses red point blips for criminals (`:382`). |
| S14 | Heists: crew/car Blue, loot Green, objectives Yellow | PARTLY | `Heists/*.cs`, `Squad.cs:99`, `FleecaJob.cs:713` | It leaves out enemy blips, which are **Red** (`FleecaJob.cs:405`, `LifeinvaderJob.cs:443`). Arena uses Orange (`ArenaModule.cs:66`) and Red/Blue teams; Den and LuckyWheel use Yellow. All are point blips, so the report's conclusion stands. |
| S15 | Police description: 12 drawables, hat prop 0, glasses prop 1 | CONFIRMED | `Police.cs:391-399` | It also stores the beard overlay. |
| S16 | **A mask, or a changed shirt or pants, defeats recognition** (Police.cs:440-450); a change at the trunk "clears outfit known" | **WRONG** | `Police.cs:440-475, 966-976, 1533` | Only a **mask** defeats recognition: `Masked()` returns infinity at `:971`, and only when the mask differs from the one the police saw (`mask > 0 && mask != d.Comp[1]`). A changed shirt, pants, shoes, hair, beard, hat or glasses only **adds seconds** (`DisguiseSeconds`: shirt/jacket +8, pants +6, hair +4, beard +4, hat +3, shoes +2, glasses +2). Removing a mask the police saw you wearing adds +12. `OutfitMatches` (11 and 4) only controls the HUD clothing icon (`:1533`). Nothing ever sets `OutfitKnown = false`. So changing at the trunk slows recognition; it doesn't "shake" it. The "line of sight already checked" remark is only the help text at `:195`; there is no out-of-sight check tied to clothing. |
| S17 | HeistKit sets hostile relationship groups at 64-65 | CONFIRMED | `Heists/HeistKit.cs:63-65` | |
| S18 | The Heists phone contact uses SPA II's iFruit list at HeistsModule.cs:346-367 | CONFIRMED | `HeistsModule.cs:344-375` (`SPAII.Mechanic.iFruit`) | |
| S19 | Den spawns peds with `World.CreatePed` and `TASK_START_SCENARIO_IN_PLACE` (236, 714-737) | CONFIRMED | `Gambling/Den.cs:236, 714, 737` | |
| S20 | SPA II is compiled into the DLL (`-recurse:'merged/*.cs'`); LemonUI is referenced | CONFIRMED | `build.sh` | The reference DLL is SHVDN3 **3.9.0** (`/home/claude/realfuel/ref`). |
| S21 | **SPA II's WardrobeList1/2.cs hold 73 lists, about 5,500 `Clothing` entries for M/F/T and freemode, with the same labels**; "no need to extract it again" | PARTLY | I counted 63 + 10 = 73 lists; 4,862 `new Clothing(` + 665 `new ClothingSet(` = 5,527. Up to three components per item (`SPAII.INM/Clothing.cs`). | The counts are right, but it is **not the full story catalogue**: there is **no mask type at all** (`eClothingType` has no Mask; zero `BERD_` labels); **Michael has no hats** (0 `PROPS_P0_H*`, against 3 in the shop); Trevor has 1 hat label (shop 3), Franklin 32 (shop 48); glasses are 30/62/41 against the shop's 51/74/61. `eClothingType.Hat` appears only in the freemode lists. Entries also don't carry the shop's item index, so they can't be matched to the ownership bits without the shop table. |
| S22 | `Wardrobe.cs` has the `Create*WardrobeMenu` menus and a preview camera; `WardrobeMemory` snapshots a look | CONFIRMED | `Wardrobe.cs:3113-3185, 3544, 3856`; `WardrobeMemory.cs` (12 components + helmet/glasses/earrings) | |
| S23 | Nothing in SPA II checks ownership | CONFIRMED (as far as grep shows) | No global, stat or ownership reads in `Wardrobe.cs` / `WardrobeMemory.cs` | |
| S24 | BlipBudget documents the 150 wait; its patch found no match | CONFIRMED | `BlipBudget.cs:1-12, 59` | The scan only covers MEM_PRIVATE readable pages and looks for `25 96` (PUSH_CONST_U8 150). |
| S25 | `Missions.Active` exists | CONFIRMED | `Missions.cs:9` (`Game.IsMissionActive || CustomHeist`) | |

### 1b. Rockstar decompiled scripts

| # | Claim | Verdict | Evidence | Correction / note |
|---|---|---|---|---|
| R1 | blip_controller waits while `GET_NUMBER_OF_ACTIVE_BLIPS() >= 150` at line 319 | CONFIRMED | `blip_controller.c:319-322` | This is Sainan's build, not necessarily Enhanced 1158.16. BlipLimitFix's changelog says some setups have 240. |
| R2 | Door 5 if "boot", door 4 if "bonnet" (fmmc.c:96358-96370) | CONFIRMED | `fmmc.c:96358-96371` | |
| R3 | `func_13118`: `ADD_BLIP_FOR_AREA(centre, width, length)` + `SET_BLIP_ROTATION_WITH_FLOAT(heading)` (762285-762297) | CONFIRMED | `fmmc.c:762285-762297` | |
| R4 | Shaded areas: AREA + SHORT_RANGE + ROTATION_WITH_FLOAT + COLOUR + ALPHA 160 (853596-853600) | CONFIRMED | `fmmc.c:853596-853601` | |
| R5 | Circles: `ADD_BLIP_FOR_RADIUS`, colour 5, alpha 120 (762310-762316) | CONFIRMED | `fmmc.c:762307-762311` | |
| R6 | Integer `SET_BLIP_ROTATION` at fm_content_stash_house.c:396044 | CONFIRMED | same line | |
| R7 | `SET_RADIUS_BLIP_EDGE` in docks_heista; `SET_BLIP_HIDDEN_ON_LEGEND` in blip_controller and am_mp_casino | CONFIRMED | `docks_heista.c:125661`; `blip_controller.c:1024`; `am_mp_casino.c:262501` | |
| R8 | fm_hideout_controler uses point/ped blips, not areas | CONFIRMED | only `ADD_BLIP_FOR_COORD` (5) and `ADD_BLIP_FOR_ENTITY` (2) | |
| R9 | Relationship group hashes (FAMILY 1166638144 ... HILLBILLY -1285976420) | CONFIRMED | I recomputed joaat for all nine; all appear in `fm_hideout_controler.c:12165-12230`, FAMILY at `franklin0.c:52348`, BALLAS at `franklin0.c:3125` | The table is a generic relationship-group index (it also returns player, CIVMALE, COP, army, shark, cat); it isn't a gang-attack list. |
| R10 | "Joining = SET_RELATIONSHIP_BETWEEN_GROUPS(1 or 0, gang, PLAYER) **both ways**, as Rockstar does at franklin0.c:52348-52372" | PARTLY | `franklin0.c:52346-52374` | Rockstar sets FAMILY→player only, one way: 255 (cleared) for Michael and Trevor, 1 for Franklin. Player→LOST is 2, and LOST↔player is 5 for Trevor. This is a per-character helper that the game re-applies itself. See M1. |
| R11 | Gang ped models (families ... armgoon) found as joaat strings | CONFIRMED | each model name found in 1-11 scripts | But see M3: Salvadorans and Marabunta are very likely the same gang. `g_m_m_maragrande_01` appears only in Online `fmmc.c:948006`. |
| R12 | `am_gang_call.c` relationship set-up at 18646-19099 | CONFIRMED (start) | `am_gang_call.c:18646` | |
| R13 | `WORLD_HUMAN_DRUG_DEALER` at gb_contraband_buy.c:441971 | CONFIRMED | same line | |
| R14 | bailbond1.c: `TASK_HANDS_UP` 38160, `UPDATE_TASK_HANDS_UP_DURATION` 38003/38156; group set-up 38012-38037; config flags 167/118/42; reset flag 237; flee/combat attributes; `TASK_ENTER_VEHICLE(...,25000,seat,2.0,1048577,0)` with seat-free and entry-clear checks; speed < 1.75 | CONFIRMED | `bailbond1.c:38003, 38010-38038, 38058-38076, 38156-38160` | |
| R15 | re_snatched.c: dict `random@kidnap_girl`, scene attached to "chassis", `IG_1_*_DRAG_INTO_VAN`, `DRAG_INTO_VAN_BURR`, van gburrito (410) | CONFIRMED | `re_snatched.c:407, 410, 417, 727-733, 8821-8867` | The drag-into-van scene itself (8857) is created at the van's coordinates and heading, not attached; the earlier loop scenes are attached. |
| R16 | `anim@GangOps@Hostage@` with perp_idle, victim_idle, victim_success, perp_fail (fmmc.c:730911-730950, 731077) | CONFIRMED | `fmmc.c:730911, 730938-730939, 731077, 731171` | It is a networked synced scene. Whether it works in SP is UNVERIFIABLE. |
| R17 | Bank hostage dicts `anim@heists@ornate_bank@hostages@*`, `fleeca_bank@hostages@*` in fmmc.c | CONFIRMED | grep of fmmc.c | |
| R18 | Carry: `missfinale_c2mcs_1` (finalec2.c:6033), clips `FIN_C2_MCS_1_CAMMAN` / `_STEVE` (123521-123529) | CONFIRMED | `finalec2.c:6033, 123518-123529` | |
| R19 | Corpse pose `dead/dead_a` (armenian2.c:67636), `dead_d` on a live, invincible, silent ped (carsteal3.c:109646), `TASK_PLAY_ANIM_ADVANCED dead/dead_a` (fmmc.c:64607) | CONFIRMED | same lines; carsteal3 synced-scene flags 145 at `:109646` | |
| R20 | Physical ankle attach, bones 52301/14201 (michael2.c:3169-3170, 49107) | CONFIRMED | `michael2.c:3169-3170, 49106-49107` | In the mission, Michael is hung from a hook entity (`iLocal_4696`). Using it on the player's hand is our own extrapolation. |
| R21 | family5 drag scene `missfam5mcs_6drag` / `fam_5_mcs_6_drag_michael` + cam (98199-98374) | CONFIRMED | `family5.c:98199, 98371, 98374` | |
| R22 | `combat@drag_ped@` not in any script | CONFIRMED (absent) | grep across /tmp/dl and /tmp/rs: no hit | Stays UNVERIFIED. |
| R23 | `carry_to_boot_l/_r` in finale_heist2a | CONFIRMED | `finale_heist2a.c:123762, 123838` | |
| R24 | `missfbi3_party` is the coke-party scene, not a trunk anim (fbi3.c:2784) | CONFIRMED | `fbi3.c:2784, 62069` (`Snort_Coke_B_Male1`) | |
| R25 | Devin's trunk is the cutscene `fin_ext_p1` (finalec2.c:4453, 4830); sound `FINALE_C2_DEVIN_INSIDE_TRUNK_MASTER` (4500, 5210); no ped attached in the boot | CONFIRMED | same lines; `func_285` calls `REQUEST_CUTSCENE`. The only vehicle attaches go to "seat_dside_f" (`:4940-5040`). | |
| R26 | `fin_ext_p1-7` / `cs_devin_dual-7` not in Rockstar scripts | CONFIRMED (absent) | grep: no hit | Community only. |
| R27 | `timetable@floyd@cryingonbed@base` at main.c:154258 | CONFIRMED | `/tmp/rs/main.c:154258` | |
| R28 | Mask anims: `put_on_mask` (fmmc.c:426470); dictionaries by situation (426483-426563) | CONFIRMED | `fmmc.c:426470-426473` (`func_6031`), `426480-426570` | The list is on_foot, standard_car ds/ps/rds/rps, low_car ds/ps, van ds/ps/rds/rps, truck ds/ps, and jeep **rds/rps only**. There is no "take off" clip. See M5 for Rockstar's own SP removal clips. |
| R29 | Shop natives: `SET_PED_COMPONENT_VARIATION` (53370), comp 5 cleared and prop calls (54002-54006), and 134625-134628 | PARTLY | `clothes_shop_sp.c:53370, 54002-54006, 54971` | **134625-134628 is wrong**: those lines are a tunables switch (`Global_262145.f_31661`). Use `:54971` for `SET_PED_PROP_INDEX(ped, anchor, d, t, false, 1)`. Rockstar passes **6** arguments to `SET_PED_PROP_INDEX` and **3** to `CLEAR_PED_PROP(ped, 0, 1)`. |
| R30 | Labels `OUTFIT_P0_0` 354340, `BERD_P0_1_0` 355081, `TORSO_P0_8_0` 358624; same in player_controller.c 22308/23049/26592 | CONFIRMED | same lines | The shop has only 16 BERD (mask/component 1) labels across the three characters (5, 5, 6). |
| R31 | `clothingspecs` at 218885 and 235510; `clothingtie try_tie_*`; `check_out_a`; `mp_clothing@female@*` | PARTLY | `clothes_shop_sp.c:218885, 235510`; `try_tie_*` and `mp_clothing@female@*` present | `check_out_a` is **not** in clothes_shop_sp.c. |
| R32 | Ownership: `func_96` at 12525 reads/writes `Global_114963.f_2370[char /*164*/].f_(4*compType)[...]`; callers at 11621-11629 map set 0→bit 1, set 1→bit 2, !set 2→bit 4 | PARTLY | `clothes_shop_sp.c:11619-11631, 12525-12600` | The callers are right. The layout is simplified: types 0-2 use `.f_0/.f_4/.f_8[iParam3]`, but type 3 picks sub-blocks `.f_12/.f_16/.f_20` with a second index (`iParam2`), so it isn't simply `f_(4*compType)`. The meaning of the sets (available / acquired / new) is still inferred. The same block is used elsewhere (`fbi4.c:121636`, `Global_114963.f_2370.f_539.f_2399`), which supports "saved story globals". |
| R33 | `CombatMeleeManager_Groups` PoolSize 30 (LS Gangsta page) | CONFIRMED | gta5-mods page text | Whether it's needed or possible on Enhanced is UNVERIFIABLE. |

### 1c. External mods and community sources

| # | Claim | Verdict | Evidence | Correction / note |
|---|---|---|---|---|
| E1 | BlipLimitFix [Enhanced] by rootBrz, GPL-3.0, open source, raises the blip_controller threshold (default 1900), tested on Enhanced 1.0.1158.16 | CONFIRMED | `/tmp/ext/gtav-blip-limit-fix/README.md`, `LICENSE`, `config/BlipLimitFix.ini`, gta5-mods page | It is GPL-3.0-**only** with a ScriptHookV linking exception. It is a **C++ ASI** that needs an ASI loader; it is not a .NET script. The range is 150-1997. |
| E2 | Its README "explains why BlipBudget's byte pattern found nothing" (threshold not always 150, structural decoding) | PARTLY | README; `CHANGELOG.md` 1.4.2 | The README never mentions BlipBudget; that link is the report's own inference. It is a plausible cause: a 240 threshold encodes as `25 F0`, not `25 96`. A second possible cause is that BlipBudget only scans MEM_PRIVATE pages, while BlipLimitFix finds the code through the script program registry. Neither has been checked on the user's machine. |
| E3 | Locators `44 88 05 ? ? ? ? 48 89 F9` (GEN9), `48 8D 0D ? ? ? ? 41 8B D6 E8 ? ? ? ? FF 05` (GEN8), registry offset 0xD8 | CONFIRMED | README "How it works" | |
| E4 | Engine pool of 2,000 slots, measured by the author | CONFIRMED (as the author's claim) | README "Engine blip-limit findings" | At full pool the native returned 1,997. The probe started with **226 slots already in use**. |
| E5 | BlipLimitFix dated Oct 2026 | CONFIRMED | git log 2026-10-24 (v1.4.2); page first upload 2026-10-18 | **The gta5-mods download is still v1.3**. The 240-threshold fix is only in 1.4.2 (GitHub/Nexus). The page also has a VirusTotal "suspicious" comment from a moderator and user reports of pattern errors and stutter. |
| E6 | Gang and Turf Mod (lucasvinbr) v2.1.0, Jan 2025, SHVDN3 + LemonUI, MIT; zones keyed by game zone name; tickets; `GangBlipColorSubMenu.cs`, `GangCarColorsSubMenu.cs` | CONFIRMED | `/tmp/ext/GTA5GangMod`: AssemblyVersion 2.1.0.0, commit 2025-01-30, LICENSE MIT, `.csproj` LemonUI, `ZoneManager.cs:138, 329`, `GangWar.cs:1169` | |
| E7 | Gang Mod "shows area blips only for the 5 nearest zones" | PARTLY | `ZoneManager.cs:254-290`, `TurfZone.cs:132-138`, `CustomTurfZone.cs:109-116` | Every zone gets a **point** icon in all-zones mode. Only *custom* zones get a radius circle, for the 5 nearest. Game-named zones never get an area. There are no rectangles. |
| E8 | rubbertoe98 CarryPeople: carrier `missfinale_c2mcs_1/fin_c2_mcs_1_camman` flag 49; carried `nm/firemans_carry` flag 33; attach (0, 0.27, 0.15, 0.63, 0.5, 0.5, 180, ..., 2, false) | CONFIRMED | raw `CarryPeople/cl_carry.lua:7-17, 93` | The repo has **no LICENSE file** (404 on main/master), so "open" means readable, not licensed to copy. |
| E9 | qb-policejob: carry at 316-322 (and 342); escort bone 11816 at 0.45, 0.45, 0 | PARTLY | raw `client/interactions.lua:293, 316-322, 336-342` | qb's carry uses rotation **0.0**, not 180 (`:321`). The escort passes 11816 straight in as a bone *index*, a common FiveM mistake; in C# use `GET_PED_BONE_INDEX(ped, 11816)`. The licence is **GPL-3.0** (checked; the report said "not checked"). `mp_arresting/idle` and `mp_arrest_paired` are confirmed at `:37-55, 115`. |
| E10 | qb-radialmenu trunk: `fin_ext_p1-7/cs_devin_dual-7`, attach `(0, x, y, z, 0, 0, 40, 1,1,1,1,1,1)` at trunk.lua:112-117; class offsets at config.lua:710-733; mid-engine list; GPL-3.0 | CONFIRMED | raw `client/trunk.lua:5-23, 102, 112-117`; `config.lua:710-733`; LICENSE GPL-3.0 | Class 0 is labelled "Coupes" in qb's comment but is Compacts. The report's grouping is right. The list also includes jester, ardent, nero/nero2, vagner and comet2-4. |
| E11 | qb-radialmenu clothing anims: hat on `mp_masks@standard_car@ds@/put_on_mask`, off `missheist_agency2ahelmet/take_off_helmet_stand`, glasses `clothingspecs` (clothing.lua:724-743) | CONFIRMED | raw `client/clothing.lua:724-745` | The glasses clip is `clothingspecs/take_off` (flag 51). |
| E12 | Kidnap (jedijosh920, 2016): passenger kidnap and Altruist sacrifice, no ransom, source not published | CONFIRMED | gta5-mods page (v1.0, 2016-01-05) | |
| E13 | Kidnap V (avivr, 2024): van, iFruitAddon2 contact, torture, buggy and abandoned | CONFIRMED | gta5-mods page; pinned author comment 2026-01-16 | |
| E14 | Carry Ped (ediotTV, 2025): custom anim in dlc.rpf, earlier heist box carry, "modify with credit", fails on scenario peds | CONFIRMED | gta5-mods page (v2.1b) | |
| E15 | Pick Up Bodies (w3irdo, 2024): crashes, can't lift corpses, not open | CONFIRMED | gta5-mods page and comments | |
| E16 | MaskEquipper (danistheman262, 2022), MP Interaction Menu For SP (CruelMasterMC, 2024, LemonUI), source not stated | CONFIRMED | gta5-mods pages | MP Interaction Menu lists no accessory toggles. |
| E17 | LS Gangsta Mod v1.8 Feb 2024: join at blip, 22 territories, "reported broken" | PARTLY | gta5-mods page | Only user comments (missing blips, crashes) support "broken"; the author doesn't say so. |
| E18 | Blip colour IDs and names are from the FiveM blip reference | CONFIRMED (names) | docs.fivem.net/docs/game-references/blips (colour section) | Names match. The page gives no RGB values; see C1-C3. |
| E19 | SHVDN Enhanced "3.9.0.6"; the globals API name is unverified | PARTLY | SHVDN Enhanced's gta5-mods page shows version **1.1.0.6** (14 Jul 2026). The build's reference DLL is 3.9.0. | The API exists in the referenced DLL: `GTA.Native.GlobalVariable.Get(int)`, then `.Read<T>()`, `.IsBitSet(int)`, `.GetStructField(int)`, `.GetArray(int)` (seen in the IL). Users on that page report memory-pattern problems on 1158.x. |
| E20 | Game build | UNVERIFIABLE | Every external source says **1.0.**1158.16; the brief says 2.0.1158.16 | Probably the same build; worth confirming with the user. |

### 1d. Colours and blip arithmetic

| # | Claim | Verdict | Evidence | Correction / note |
|---|---|---|---|---|
| C1 | The proposed turf colours (2, 25, 27, 83, 8, 10, 15, 18, 38, 29, 40, 55, 26, 23, 61) aren't red, orange or yellow | PARTLY | RGB from the forum.cfx.re "Blip colour id to RGB" thread: 2 (113,203,113), 25 (12,123,86), 27 (171,60,230), 83 (168,84,242), 8 (254,122,195), 10 (177,143,131), 15 (106,196,191), 18 (151,202,233), 29 (69,97,171), 26 (122,195,254), 55 (160,160,160), 61 (205,63,152), 23 (231,141,154) | None is a pure red, orange or yellow. But **23 Light Pink (231,141,154) is a salmon-pink** that will look like the police's red hot area at alpha 85-110 over the dark map; drop it. 10 Light Brown is a pinkish tan; acceptable. 38 and 40 look right by name (blue, dark grey), but the thread's author warns that IDs from 35 up may be misaligned in his table. |
| C2 | The avoid list covers the red/orange/yellow shades | PARTLY | same RGB source | Also avoid **36 Beige (251,238,165, pale yellow)**, **51 Salmon (255,139,92, orange)**, **41 Pink Red (242,157,157)**, **56 Brown (132,114,50, olive/dark yellow)** and probably **31 Light Brown (184,155,123, tan)**. IDs 2-5, 49 and 66-69 also show "friendly/enemy" variants, so never call `SET_BLIP_AS_FRIENDLY` on turf blips. Confirm in game with `GET_BLIP_HUD_COLOUR` and `GET_HUD_COLOUR` before fixing the palette. |
| C3 | "25-40 zones × 1-3 rectangles = about 40-80 blips" | **WRONG** | arithmetic | 25×1 = 25 up to 40×3 = 120. Even the middle of the range (about 60-80) needs the rest of the map to be light. BlipLimitFix's own test game already had 226 slots in use before any turf blips (E4). |
| C4 | Area and radius blips count in `GET_NUMBER_OF_ACTIVE_BLIPS` | UNVERIFIABLE (likely yes) | They live in the same blip pool, and the README's measurement counts all but 3 slots | Measure with BlipBudget's log. |
| C5 | SHVDN3 has `ADD_BLIP_FOR_AREA`, `SET_BLIP_ROTATION_WITH_FLOAT`, `SET_BLIP_ROTATION`, `SET_BLIP_HIDDEN_ON_LEGEND`, `SET_RADIUS_BLIP_EDGE` | CONFIRMED | present in the `Hash` enum of the referenced SHVDN 3.9.0 DLL | SHVDN has no area-blip helper. Create with `Function.Call<int>(Hash.ADD_BLIP_FOR_AREA, x, y, z, w, l)` and wrap the handle in a `Blip`. |
| C6 | Option B (blips only while the pause map is open) | UNVERIFIABLE | | as the report says |

---

## 2. Missed or wrong in the report

**M1. The game overwrites gang relationships by itself.** A per-character helper sets FAMILY→player to 255 (Michael, Trevor) or 1 (Franklin), player→LOST to 2, and LOST↔player to 5 for Trevor. It lives in `main.c:7203` (`func_163`, called during player set-up at `main.c:3658`), and the same function is compiled into about 38 mission scripts (franklin0, armenian1/3, agency_heist2, fbi1-5, the michael and trevor missions and others). Joining the Families as Michael or Trevor, or the Lost as Trevor, will be undone on character set-up and whenever those scripts run it. The gang module must re-assert its relationships on a timer (for example every 1-2 s), not only on switch and load. All three protagonists share the single `PLAYER` group, so joining is effectively global and has to be redone per character.

**M2. Police recognition works differently from what the report says.** See S16. Only a mask makes you unrecognisable, and only a mask the police didn't already see. Clothes, hat and glasses changes add 2-8 s each. That is still gameplay value, but the report's "shake a description" loop for feature 8 is overstated. The newly written `TrunkWardrobe.cs` header has the same wording ("compares ... live"), which is accurate, but nothing resets `OutfitKnown`.

**M3. Salvadorans and Marabunta Grande are very likely one gang.** In-game, Marabunta Grande are the Salvadoran (MS-13 parody) gang ([gtabase](https://www.gtabase.com/grand-theft-auto-v/gangs/marabunta-grande): blue and white; El Burro Heights, Vespucci Beach, Grand Senora, the East Vinewood canal). The ambient members are the `g_m_y_salvagoon_*` / `g_m_y_salvaboss_01` models. `g_m_m_maragrande_01` appears only in Online `fmmc.c`. Giving them two turf colours (Blue 38/29 and Electric Blue 26) is probably a design error. Which relationship group the ambient salvagoons actually use (SALVA or MARABUNTE) is UNVERIFIABLE here; read it at runtime with `GET_PED_RELATIONSHIP_GROUP_HASH`.

**M4. SPA II can't be the source for feature 9's lists.** There are no masks at all, no Michael hats, and fewer glasses than the shop (S21). Remembering what has been seen on the ped (the approach `Accessories.cs` now takes) is the only workable owned-items list without the ownership global. Even that needs filters (see M10).

**M5. Rockstar has SP removal animations the report didn't find:**
- Mask off in a car: `misscommon@std_take_off_masks` / `take_off_mask_ds`, flag 0. Rockstar swaps the component at anim time > 0.25 (`fbi4.c:121599-121654`).
- Hat or helmet off on foot: `veh@bike@common@Front@base` / `take_off_helmet_walk`, flag 48, prop removed at > 0.54 (`agency_heist2.c:7005-7044`).

Both are better-grounded than qb's `missheist_agency2ahelmet`. They also mean `put_on_mask` doesn't have to serve for every case.

**M6. The BlipLimitFix version matters.** The gta5-mods download is 1.3. The fix for non-150 thresholds is 1.4.2, which is only on GitHub and Nexus. If users are pointed at gta5-mods, setups with a 240 threshold get "pattern not found". It is also an ASI, so it needs an ASI loader.

**M7. The blip budget is worse than the report assumes** (C3, E4). Treat lifting the threshold (BlipLimitFix 1.4.2, or a BlipBudget rewrite using the registry approach) as a prerequisite for feature 3, not an option. Otherwise use Gang Mod's model (point icons, plus areas only near you).

**M8. The colour avoid list is incomplete.** Colour 23 should come off the palette (C1, C2).

**M9. Main-ride shapes break features 6 and 8 for two characters.**
- Franklin's **bagger** is a motorcycle with no "boot" bone. `Trunk.cs` never offers a trunk on it, so there's no wardrobe and no body storage.
- Trevor's **bodhi2** is a pickup. Its "boot" is the tailgate of an open bed, so "hide the occupant while the lid is shut" would make a body vanish from an open bed.
- On SUVs and hatchbacks the "boot" bone is at the tailgate hinge (roof height), so a root offset taken from it needs a downward correction. Plan per-class or per-model handling and a "no body storage" list that includes pickups.

**M10. The `Accessories.cs` already written conflicts with other modules** (checked because it now exists):
- It opens on `IS_DISABLED_CONTROL_JUST_PRESSED(InteractionMenu)`. That still fires while the Trunk panel calls `DisableAllControlsThisFrame`, so M opens it on top of the trunk panel and the police computer.
- `Blocked()` doesn't check `Trunk.PanelOpen`, `PoliceScript.Busy`, `PoliceComputerScript.ComputerOpen`, `Missions.Active` or the phone. The report asked for "Busy states and the trunk panel".
- `Look()` records whatever is on the head every second. That includes motorcycle helmets (prop 0 while riding) and mission-given hats and masks, which then show up as "owned".
- `SET_PED_PROP_INDEX` is called with 5 arguments and `CLEAR_PED_PROP` with 2. Rockstar passes 6 (`..., false, 1`) and 3 (`..., 0, 1`) (R29). Pass the full set to avoid an undefined last argument.

`TrunkWardrobe.cs` has the same `SET_PED_PROP_INDEX` and `CLEAR_PED_PROP` argument issue at lines 84-85.

**M11. For corpses, `RESURRECT_PED` is an alternative to `CLONE_PED`.** It keeps the same entity, so blood and damage decals probably stay. It is just as UNVERIFIED and should be tested alongside `CLONE_PED`.

**M12. Licences.** rubbertoe98 has no licence (all rights reserved by default), so take numbers from it but don't copy code. qb-policejob is GPL-3.0. Neither can be pasted into the suite unless the suite accepts GPL.

**M13. Pools on Enhanced.** The report names the ped pool but gives no numbers, and none could be verified here. SHVDN Enhanced's page has user reports of "memory pattern not found" for pool access on 1158.x. `Trunk.cs` already uses `World.GetNearbyVehicles` and `World.GetAllBlips`, so pools evidently work on the user's setup, but wars and dealers should cap spawned peds conservatively and check `World.GetAllPeds().Length` at runtime.

**M14. Small citation errors.**
- clothes_shop_sp 134625-134628 is wrong (R29).
- RealFuel 567-568 should be 575-576 (S10).
- `check_out_a` isn't in the shop script (R31).
- "Rockstar sets both ways" is wrong (R10).
- ROADMAP.md points to `docs/FEASIBILITY.md`, but the file is in `plans/`.

---

## 3. Overall assessment

Almost all of the Rockstar script citations are accurate; I found one bad line range and a few small slips. The native names, animation dictionaries and clips, bone IDs, relationship hashes and model names all check out. The community values (carry, trunk pose, class offsets) match their sources, except qb's carry rotation.

The weak points are claims about the suite itself and about the data the build will lean on.

1. **Feature 9 first: holds.** The prop and component slots and the natives are right. The mask animation set is real, and Rockstar has SP removal clips the report missed (M5). "What the character owns" can't come from SPA II (no masks, no Michael hats). Remembering what has been worn works, but must ignore helmets and mission items and must respect other modules' busy states (M10). The police payoff is smaller than claimed: masks block recognition; other items only slow it (M2).
2. **Feature 8 second: holds**, as packed outfits under `MAIN|` keys. It is not available on Franklin's bagger (M9). Ownership via `Global_114963` stays optional and build-specific. SHVDN does have `GlobalVariable` (E19), but SPA II entries can't be mapped to the ownership bits without the shop's index table.
3. **Features 5 and 6 (carry, then trunk): hold.** The carry values are confirmed, and the trunk pose and offsets are confirmed from qb. Add pickups and bikes to the exclusions, correct the Z offset for tall vehicles, and don't hide bodies in open beds (M9).
4. **Feature 4: holds.** The bail-bond follow logic is exactly as cited.
5. **Feature 3: holds technically, but the budget is understated.** The area-blip natives and Rockstar's rectangle helper are confirmed and exist in SHVDN's Hash enum. The count arithmetic is wrong (25-120, not 40-80), and real setups may already be over 150. Make the threshold lift (BlipLimitFix 1.4.2, not the gta5-mods 1.3 download) or "near-only" area blips a precondition. Fix the palette: drop 23, extend the avoid list, verify colours in game (C1, C2). Merge Salvadorans and Marabunta (M3).
6. **Feature 1 (wars) needs one design change before it's built:** the game resets FAMILY and LOST relationships with the player itself, so joined-gang relationships must be re-asserted continuously, not only on switch and load (M1).

The build order is sound. The fixes above fold into it without reordering anything. The only thing that needs to happen earlier is settling the blip strategy before any turf work, as the report already says.
