# Our behaviour changes to Single Player Heists (run after fix_sph.py on a fresh decompile).
import re
def patch(path, a, b, count=1):
    s = open(path, encoding='utf-8').read()
    n = s.count(a)
    assert n == count, (path, a[:80], n)
    open(path, 'w', encoding='utf-8').write(s.replace(a, b))
base = 'merged/SinglePlayerHeists/'

# 1. Lester's garment factory (id2_14) belongs to the story: don't force its map state at startup.
#    Also drop the "Single Player Heists Loaded / Initialization Stage" pop-ups.
p = base + 'SinglePlayerHeists.UtilsMisc/ManageInitialization.cs'
s = open(p, encoding='utf-8').read()
s = re.sub(r'\t+Function\.Call\(Hash\.(REMOVE|REQUEST)_IPL, "id2_14[^"]*"\);\n', '', s)
s = re.sub(r'\t+Notification\.Show\("~y~Single Player Heists~w~ Loaded\\nInitialization Stage: [^"]*"\);\n', '', s)
open(p, 'w', encoding='utf-8').write(s)
assert 'id2_14' not in s and 'Initialization Stage' not in s

# 2. Stilt House job: it loaded 47 interiors for the whole game at startup, including story interiors
#    (Lester's house, Franklin's house, LS Customs, the bank). The same "load every interior" idea broke
#    Lester's house windows before. Load nothing at startup; at heist start load only the stilt houses.
p = base + 'SinglePlayerHeists.HitNRun/TheSilthouseJob.cs'
s = open(p, encoding='utf-8').read()
a = s.index('public TheSilthouseJob()'); b = s.index('\n\t\t}\n', a)
ctor = re.sub(r'\t+Function\.Call\(Hash\.(REQUEST|REMOVE)_IPL, "[^"]*"\);\n', '', s[a:b])
s = s[:a] + ctor + s[b:]
story = ['v_janitor', 'v_franklinshouse', 'v_genbank', 'v_carmod', 'v_rockclub', 'v_comedy', 'v_clotheshi',
         'burton_subway', 'bt1_04_carpark', 'v_studio_lo', 'v_apart_midspaz', 'v_ranch']
for name in story:
    s = re.sub(r'\t+Function\.Call\(Hash\.REQUEST_IPL, "[^"]*' + name + r'[^"]*"\);\n', '', s)
open(p, 'w', encoding='utf-8').write(s)

# 3. Lester's first text about the new heists: never in the middle of a story mission or cutscene.
patch(base + 'SinglePlayerHeists/Main.cs',
      'if (LesterHasToldPlayerOfNewHeists == 0 && Game.GameTime > GameTimeToTriggerUINotification)',
      'if (LesterHasToldPlayerOfNewHeists == 0 && Game.GameTime > GameTimeToTriggerUINotification && !Game.IsMissionActive && Game.Player.CanControlCharacter && !Function.Call<bool>(Hash.IS_CUTSCENE_ACTIVE, Array.Empty<InputArgument>()))')
print('sph patches applied')

# 4. Phone contacts: no second plain "Lester" next to the story one, and no mod-name labels.
patch(base + 'SinglePlayerHeists.UtilsMisc/LesterContact.cs', 'new iFruitContact("Lester")', 'new iFruitContact("Lester (Jobs)")')
patch(base + 'SinglePlayerHeists.UtilsMisc/OrderAVehicle.cs', 'new iFruitContact("SPH Arcade Vehicles")', 'new iFruitContact("Arcade Vehicles")')
patch(base + 'SinglePlayerHeists.UtilsMisc/SPH_SettingsMenu.cs', 'new iFruitContact("SPH Settings")', 'new iFruitContact("Heist Settings")')
print('contact names patched')
