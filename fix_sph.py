# Fixes the decompiled Single Player Heists sources so they compile inside RealismSuite.
import re, os, glob
root = 'merged'
def files(d): return glob.glob(os.path.join(root, d, '**', '*.cs'), recursive=True)
for f in files('SinglePlayerHeists') + files('HKHModHelperNew') + files('SmartGuards'):
    t = open(f, encoding='utf-8-sig').read(); o = t
    t = re.sub(r'^[ \t]*\[(return: |field: )?TupleElementNames\([^\n]*\n', '', t, flags=re.M)
    t = re.sub(r'\[(return: |field: )?TupleElementNames\(new string\[\] \{[^}]*\}\)\] ?', '', t)
    # LemonUI 2.2: UseMouse was replaced by MouseBehavior
    t = re.sub(r'(\w[\w.]*)\.UseMouse = false;', r'\1.MouseBehavior = LemonUI.Menus.MenuMouseBehavior.Disabled;', t)
    # SmartGuards' internal EntityFlickerEffect clashes with SPH's once both are in one assembly
    if os.sep + 'SmartGuards' + os.sep in f:
        t = re.sub(r'\bEntityFlickerEffect\b', 'SmartEntityFlickerEffect', t)
    if t != o: open(f, 'w', encoding='utf-8').write(t)
