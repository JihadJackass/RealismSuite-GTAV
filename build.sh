#!/bin/bash
# Builds RealismSuite.dll: our modules (src/) plus the merged mods (merged/).
R=/home/claude/realfuel/ref
mcs -target:library -unsafe -langversion:7 -sdk:4.7.2-api -r:$R/ScriptHookVDotNet3.dll -r:$R/LemonUI.SHVDN3.dll -r:$R/PresentationCore.dll -r:WindowsBase.dll -r:Microsoft.VisualBasic.dll -r:System.dll $EXTRA_REFS \
  -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:System.Core.dll -r:System.Xml.dll -r:System.Xml.Linq.dll \
  -r:System.Web.Extensions.dll -r:Microsoft.CSharp.dll \
  -resource:merged/BusinessV.Defaults.businesses.json,BusinessV.Defaults.businesses.json \
  -resource:merged/iFruitAddon2.Properties.Resources.resources,iFruitAddon2.Properties.Resources.resources \
  -out:build/RealismSuite.dll -recurse:'src/*.cs' -recurse:'merged/*.cs' 2>&1 | grep -E "error"
