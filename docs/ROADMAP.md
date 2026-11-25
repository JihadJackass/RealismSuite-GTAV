# Roadmap

What's planned for the bundle next. Each item says what it should do in game, how it ties into what's already there, and where it stands. The research behind it is in `docs/research/` (a feasibility study, and a second pass that checked its claims).

Status: **planned**, **researching**, **in progress**, **done**.

Suggested build order: clothes menu, trunk wardrobe, carrying, bodies in trunks, kidnapping, then the turf map and joining a gang, then wars, then dealers.

---

## Gangs and territory

### Joining a gang
**Status:** built and reviewed, untested in game

Each gang has a hangout in its home neighbourhood, shown on the map in its colour, with a leader standing at it. Talk to him (E) to join; it costs $5,000 by default. He greets you, thanks you for joining, and insults you if you leave or turn up wearing a rival's colours. The hangout menu works with the mouse and with WASD. A fixed hangout spot can be set per gang in `Gangs.ini` (`Hangout=x, y, z`). Joinable: Families, Ballas, Vagos, Marabunta Grande (with the Salvadorans) and the Lost MC. The Aztecas have no street-gang group of their own in the game, so they can't be joined. Gangs, colours, cars and home turf are all in `Gangs.ini`.

Original plan:

GTA V doesn't put you in a gang, so you have to join one first, through a contact, before any of the gang features open up. Your gang decides:
- your crew colour on the map
- the cars your members drive
- who your rivals are

You can only be in one gang at a time. Leaving should cost something (respect, a fight, a payout).

Notes from the research:
- The game sets the player's relationship with the Families and the Lost itself, in `main` and in about 38 mission scripts, and it keeps doing it. Your gang's friendliness has to be re-applied on a timer, not just once on joining.
- The Salvadorans and Marabunta Grande are almost certainly the same gang in game, so they get one colour, not two.

### Gang wars and turf (San Andreas style)
**Status:** built and reviewed, untested in game

How it works now:
- **Starting a war:** kill three of a rival gang's members in their own turf within 90 seconds. In neutral turf, kill three of any gang's members there and you fight that gang for it.
- **The fight:** three waves of attackers, on foot and from the second wave by car, plus two of your homies as backup.
- **Winning and losing:** clear the waves to take the neighbourhood. Die, get arrested or stay out of it for 30 seconds and the war is lost.
- **Defending:** once a game day there's a chance a rival moves on your turf. You get three in-game hours to get there, or it's theirs.
- **Interruptions:** a story mission or a character switch calls a war off without anyone losing turf; a defence goes back to a pending attack with a fresh deadline. Attackers who run out of the zone, or get stuck out of sight and out of reach for a minute, count as gone.

Original plan:

- The city is split into turf owned by the gangs.
- Starting trouble in a rival's turf, or attacking it on purpose, starts a war: waves of rival members show up, and holding out takes the turf.
- Rivals attack your turf back, and turf you don't defend gets lost.
- Wars and turf should work with the police system: a war draws police the way any big shootout does.

### Turf colours on the map
**Status:** built and reviewed, untested in game

Turf is the game's own neighbourhoods, sampled from the map once and drawn as see-through rectangles in the owner's colour. Neighbouring zones held by the same gang are merged into as few rectangles as possible; `Gangs.log` says how many were drawn. The list of neighbourhoods is in `Gangs.ini`, and `Gangs.log` lists every zone code found on the map.
- Turf only shows for a character who is in a gang, and goes away when he leaves. Hangouts stay on the map so you can find where to join.
- The rectangles are short-range, as Rockstar's own area icons are. Long-range ones were what put a ring of coloured dots round the edge of the minimap.

Original plan:

- Each gang's turf is shaded on the map in its own colour.
- No red, orange or yellow. The police system uses those (search areas, heat), so gangs get green, pink, purple, blue, teal and so on.
- The colour matches the gang's cars.
- Keep the number of map icons in mind. The game stops adding shop and mission icons past 150 icons on the map (see BlipBudget), so turf can't be hundreds of separate squares. Even a coarse map (25 to 40 zones, 1 to 3 shapes each) is 25 to 120 icons, and a busy game may already be over 150 without any turf. Lifting that limit comes first (BlipLimitFix, below).
- Shapes: Rockstar draws turf-like areas as rotated, see-through rectangles (area icons with a rotation), as in its Online mission script. Custom outlines on the pause map aren't possible in story mode.
- Colours, to check in game: green for the Families, purple for the Ballas, light blue for the Aztecas, blue for the Marabunta, grey for the Lost. The Vagos are yellow in the game, which the police own, so they're light brown instead. Avoid the reddish and yellowish map colours as well: light pink (looks like the police's red), pale yellow, orange-salmon, pink-red, olive brown and tan.

### Drug dealing
**Status:** built and reviewed, untested in game

How it works now:
- **Hiring:** at your gang's hangout, for neighbourhoods you hold. Each dealer costs $2,500, with up to 3 per neighbourhood.
- **Earnings:** each dealer earns $40 an in-game hour, and the money waits at the hangout.
- **Losing them:** a dealer is gone if he's killed or his neighbourhood is lost.

Original plan:

- You hire dealers to work corners on your turf.
- Dealers earn money over time and hold the corner for your gang: more dealers, firmer control.
- Rivals and police can hit your corners. A corner left alone in a rival area becomes a target.
- Money goes through the same wallet as everything else.

---

## Bodies, people and the trunk

### Kidnapping
**Status:** built, untested in game

How it works now:
- **Hands up:** aim a gun at someone on foot and they put their hands up.
- **Grab:** press K to grab them; holding K lets them go.
- **Getting in:** they follow you and take a back seat. Press H to put them in the trunk instead.
- **The hand-over:** drive to the hand-over spot that appears on the map and wait there, with nobody around, to collect a ransom.
- **Police:** being seen grabbing someone is reported the same way as other crimes.

Original plan:

For gang wars and heists: grab someone, get them into your car, take them somewhere and hold them for ransom or information. Should tie into the police system (witnesses, a wanted level for being seen).

### Grabbing and dragging bodies
**Status:** built, untested in game

Press H to pick up a knocked-down person or a body over your shoulder, and H again to put them down. You walk while carrying, can't run, aim or get into a car, and drop them if you're hit. A dead body is swapped for a stand-in posed as dead (Rockstar's own trick), so it can be carried with the same animation.

Original plan:

Pick up or drag a body by hand to move it out of sight.
- Carrying a living (or knocked-out) person over the shoulder works: the game has the animation, used in a story mission, and other mods use it.
- A dead body can't be animated. The options are Rockstar's own trick (a live stand-in posed as dead, swapped for the body) or dragging the body by the ankles with a physics attachment.

### Bodies in the trunk
**Status:** built, untested in game

How it works now:
- **In and out:** carry someone to a car's trunk and press H to put them in; H at the trunk takes them out.
- **Out of sight:** the body is hidden while the lid is shut.
- **Kept with the car:** it stays with that car if you drive away or save, keyed the same way as the trunk stash.
- **Open beds:** pickups with an open bed carry the body in the bed, in plain view.
- **No trunk:** motorcycles and mid-engine cars refuse.
- **Police:** a body hidden before anyone finds it isn't reported.
- **Fine-tuning:** each car model's spot can be adjusted in `Bodies.ini`.

Original plan:

Put a body (or a kidnapped person) in the trunk of a car and take it out again. For heists and gang wars. Works with the trunk stash that's already there.
- Hide the body while the lid is shut, so it doesn't poke through the car, and keep it as a stash entry when the car despawns or the game saves.
- Not every car has a usable trunk: mid-engine cars, Franklin's Bagger (a motorcycle) and pickups with an open bed (Trevor's Bodhi) need their own handling. On SUVs the trunk point sits at roof height and needs lowering.

### Jerry cans in the trunk
**Status:** done

Jerry cans could already be stored in the trunk stash, but the petrol in them was tracked separately and got mixed up (store a full can, buy a new one, and the stored can's petrol was lost). Now each can keeps its own petrol: storing it takes the petrol with it, taking it back gives that petrol back, and the trunk list shows how many litres each can holds. You can only carry one can at a time, so the trunk won't hand you a second one.

---

## Clothes

### Changing clothes at your own car's trunk
**Status:** done (untested in game)

At your main ride's trunk (never a random car), the trunk panel has a WARDROBE button (or W):
- **Pack a copy of what I'm wearing:** stores an outfit, up to 8 per car.
- **Change into an outfit:** puts it on, and what you had on goes into the trunk in its place. R removes an outfit.
- **Missions:** not during story missions, since those set your clothes.

Franklin's Bagger is a motorcycle with no trunk, so his wardrobe is in his Buffalo.

What it does for the police: a changed shirt, pants, hat or glasses gives you a few extra seconds before a cop recognises you. It slows them down; it doesn't wipe the description. Only a mask they haven't seen you in makes you unrecognisable.

### Interaction menu (M)
**Status:** done (untested in game)

Story mode has no menu on M, so this is modelled on GTA Online's.
- **Quick GPS:** a waypoint to the nearest main ride, safehouse, petrol station, LS Customs, Ammu-Nation, clothes store, barber, tattoo parlour, convenience store, car wash or hospital, or clear it.
- **Accessories:** hat, glasses, and mask on the freemode models. On/off, or swap for anything that character has worn before. Mission clothes and bike helmets aren't remembered, and nothing changes during a mission. Rockstar's mask animation and sound, upper body only, by seat in a car.
- **Vehicle:** engine (through RealFuel, so an empty tank still won't start), hood, trunk, each door, close all, windows.
- **Controls:** W/S and A/D on foot, the arrows in a vehicle (so WASD keeps driving), Enter or click, Backspace or right click to go back, M to close. Mouse throughout.
- **Story characters:** component 1 is their beard, not a mask. The first version of this menu treated it as a mask and shaved them; it now leaves it alone, and puts back once any beard it took off. Story masks go on the hat prop, so the hat row ("Headwear") covers them.
- **Later:** walk styles; offering clothes you've bought but never worn (that list sits in the save at a build-specific place).

### Help prompts and the Brief
**Status:** done (untested in game)

Our prompts used to be re-sent every frame, and each one went into the pause menu's Brief. They now go through one helper that does what Rockstar's context prompts do: keep the message out of the Brief and show it on a short timer, so it disappears by itself when you walk away. The merged mods (Diamond Casino, Gambling Dens, HKH, SPA II) still have their own per-frame prompts.

---

## Smaller things noted along the way

- **Map icon limit.** The game's `blip_controller` script stops adding shop, activity and mission icons once 150 icons are on the map. BlipBudget's attempt to patch it found nothing, because it searched for fixed bytes. **BlipLimitFix [Enhanced]** (open source, GPL-3.0, tested on Enhanced 1.0.1158.16) decodes the script's code instead and raises the limit to a configurable number. It's an ASI plugin, so it's installed beside the game rather than merged into the suite: version 1.4.2 from GitHub (the gta5-mods download is older), limit 1900. Check `BlipLimitFix.log` beside the game for `PATCHED:`.
- **LS Customs icons not showing.** All five exist on the map while you're off a mission, yet don't appear for you. BlipBudget now logs each one's display settings, to find out why.
