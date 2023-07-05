using UnityEditor;

namespace CoFramework.EngineEditor
{
    public class ObjectWindow : EditorWindow
    {
        [MenuItem("CoFramework", menuItem = "CoFramework/ModulesDepends")]

        static void Open()
        {
            var window = GetWindow<ObjectWindow>();
            window.Show();
        }



        private void OnGUI()
        {

        }
    }
}