using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CoFramework.RefBuild.Editor
{

    public class BuilderPreference : ScriptableSingleton<BuilderPreference>
    { 
        public string defaultNameSpace = "CoFramework.RefBuild.Cache";
        public string defaultPath = "Assets/Project/Scripts/RefBuilds";
        public bool increase = false;
        public bool part = false;
        public bool autoCreatePath = true;
        public bool awakrInit = false;
    }
}