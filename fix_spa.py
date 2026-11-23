#!/usr/bin/env python3
# Turns ILSpy's C# output for SPAII.dll (a VB.NET assembly) into source mcs accepts. Text fixes only; no logic changes.
import re, glob, shutil, os, subprocess, collections
SRC='/home/claude/merge/out/SPAII'; DST='merged/SPAII'
shutil.rmtree(DST, ignore_errors=True)
for p in glob.glob(SRC+'/**/*.cs', recursive=True):
    if '/Properties/' in p: continue
    q=os.path.join(DST, os.path.relpath(p, SRC)); os.makedirs(os.path.dirname(q), exist_ok=True); shutil.copy(p, q)
files=glob.glob(DST+'/**/*.cs', recursive=True)
for p in files:
    s=open(p).read(); t=s
    # VB modules are static classes
    if '[StandardModule]' in t:
        t=re.sub(r'(\[StandardModule\]\s*\n(?:\s*\[[^\]]*\]\s*\n)*\s*(?:internal|public) )sealed class', r'\1static class', t)
    # leftover of VB "End With"
    t=re.sub(r'^[ \t]*@null @?\w+ = null;\n', '', t, flags=re.M)
    t=re.sub(r'^[ \t]*base\._002Ector\(\);\n', '', t, flags=re.M)
    t=t.replace('.get_Exists()', '.Exists')
    # VB closure copy-constructor called with itself (VB passes Nothing here)
    t=re.sub(r'(\b[\w.]+\s+(\w+) = new [\w.]+)\(\2\);', r'\1(null);', t)
    t=re.sub(r'^[ \t]*\[return: TupleElementNames\([^\n]*\n', '', t, flags=re.M)
    t=re.sub(r'\[TupleElementNames\(new string\[\] \{[^}]*\}\)\] ?', '', t)
    # optional ByRef parameters
    t=t.replace('internal bool TryStoreApartmentVehicle(ApartmentClass selectedApt, Vehicle vehicle, ref int uid, ref int storedSlot = -1, int preferredFloor = -1, int expectedSlot = -1)','''internal bool TryStoreApartmentVehicle(ApartmentClass selectedApt, Vehicle vehicle, ref int uid)
		{
			int storedSlot = -1;
			return TryStoreApartmentVehicle(selectedApt, vehicle, ref uid, ref storedSlot);
		}

		internal bool TryStoreApartmentVehicle(ApartmentClass selectedApt, Vehicle vehicle, ref int uid, ref int storedSlot, int preferredFloor = -1, int expectedSlot = -1)''')
    t=t.replace('public static bool StoreVehicle(Mansion m, Vehicle veh, ref int committedUid = 0)','''public static bool StoreVehicle(Mansion m, Vehicle veh)
		{
			int committedUid = 0;
			return StoreVehicle(m, veh, ref committedUid);
		}

		public static bool StoreVehicle(Mansion m, Vehicle veh, ref int committedUid)''')
    # nested types that closures expose through generic types
    for n in ('GuardSlot','TuningOption','SettingsExpectation','OwnedBlip'):
        t=re.sub(r'\bprivate( sealed| abstract)?( class| struct| enum) '+n+r'\b', r'internal\1\2 '+n, t)
    if t!=s: open(p,'w').write(t)
open(DST+'/VBSupport.cs','w').write('''// Hand-written support for the VB-to-C# output.
internal sealed class VB_0024AnonymousType_0<T0, T1>
{
	public T0 b { get; private set; }
	public T1 a { get; private set; }
	public VB_0024AnonymousType_0(T0 b, T1 a) { this.b = b; this.a = a; }
}
internal static class MathF
{
	public const float PI = 3.14159274f;
	public const float E = 2.71828175f;
}
''')
def build():
    out=subprocess.run(['./build.sh'],capture_output=True,text=True).stdout
    open('/tmp/err.txt','w').write(out); return out
for rnd in range(8):
    out=build(); errs=[l for l in out.split('\\n') if 'error CS' in l]
    lines=out.split('\n'); n=sum('error CS' in l for l in lines)
    print('round',rnd,'errors',n)
    if n==0: break
    changed=0
    edits=collections.defaultdict(dict)   # file -> line -> new text
    fields=collections.defaultdict(set)   # file -> names needing a WithEvents backing field
    for l in lines:
        m=re.match(r'(merged/\S+?)\((\d+),\d+\): \(Location of the symbol',l)
        if m: edits[m.group(1)][int(m.group(2))]='widen'
        m=re.match(r'(merged/\S+?)\((\d+),\d+\): error CS005[0-9]',l)
        if m: edits[m.group(1)].setdefault(int(m.group(2)),'narrow')
        m=re.match(r"(merged/\S+?)\(\d+,\d+\): error CS0103: The name `(_\w+)'",l)
        if m: fields[m.group(1)].add(m.group(2))
    for p,d in edits.items():
        L=open(p).read().split('\n')
        for ln,kind in d.items():
            x=L[ln-1]
            if kind=='widen': y=re.sub(r'\b(private|protected)\b','internal',x,count=1)
            else: y=x.replace('public ','internal ',1)
            if y!=x: L[ln-1]=y; changed+=1
        open(p,'w').write('\n'.join(L))
    for p,names in fields.items():
        s=open(p).read()
        for name in names:
            prop=name[1:]
            m=re.search(r'^([ \t]*)(?:public|internal|private|protected)( static)?(?: virtual)? ([\w.<>\[\], ]+?) '+re.escape(prop)+r'\s*\n\s*\{', s, flags=re.M)
            if not m or re.search(r'\b'+re.escape(name)+r';', s[:m.start()][-400:]): continue
            decl='%s[System.Runtime.CompilerServices.AccessedThroughProperty("%s")]\n%sprivate%s %s %s;\n\n' % (m.group(1),prop,m.group(1),m.group(2) or '',m.group(3),name)
            s=s[:m.start()]+decl+s[m.start():]; changed+=1
        open(p,'w').write(s)
    print('  changed',changed)
    if changed==0: break
