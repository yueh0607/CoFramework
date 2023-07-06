using UnityEditor;

namespace CoFramework.Utility.RefBuild.Editor
{

    public class BuilderPreference : ScriptableSingleton<BuilderPreference>
    {
        public string defaultNameSpace = "CoFramework.Utility.RefBuild.Cache";
        public string defaultPath = "Assets/Project/Scripts/RefBuilds";
        public bool increase = false;
        public bool part = false;
        public bool autoCreatePath = true;
        public bool awakrInit = false;
    }
}