using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CoFramework.EngineEditor
{


    public class ModuleWindow : EditorWindow
    {
        [MenuItem("CoFramework", menuItem = "CoFramework/ModulesDepends")]
        static void Open()
        {
            var window = GetWindow<ModuleWindow>("Modules Dependence");
            window.minSize = new Vector2(400, 200);
            window.Show();
        }



        Dictionary<Type, bool> folded = new Dictionary<Type, bool>();
        Dictionary<Type, Type[]> depends = new Dictionary<Type, Type[]>();
        Dictionary<Type, Type> parameters = new Dictionary<Type, Type>();
        private void OnEnable()
        {
            Assembly[] ass = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < ass.Length; i++)
            {
                var assType = ass[i].GetTypes();
                foreach (var type in assType)
                {
                    if (type.GetInterface(nameof(IModule)) != null)
                    {
                        folded.Add(type, true);
                        var depend = type.GetCustomAttribute<ModuleDependsAttribute>();
                        depends.Add(type, depend == null ? new Type[0] : depend.Depends);
                        parameters.Add(type, depend?.Parameter);
                    }
                }
            }

        }

        Vector2 scrollViewPosition = Vector2.zero;
        string tagTemp = string.Empty;

        private void OnGUI()
        {
            scrollViewPosition = GUILayout.BeginScrollView(scrollViewPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            GUILayout.BeginVertical();
            foreach (var kvp in folded)
            {
                tagTemp = string.Empty;
                if (Application.isPlaying)
                    tagTemp = Framework.HasModule(kvp.Key) ? "---(Running)" : "---(Rest)";
                else tagTemp = parameters[kvp.Key] == null ? "--(NullCreateParameter)" : $"--({parameters[kvp.Key].Name})";

                GUILayout.Label($"[{kvp.Key.Name}]" + tagTemp);


                foreach (var depend in depends[kvp.Key])
                {
                    EditorGUILayout.LabelField("            " + depend.Name + "(Depend)", EditorStyles.wordWrappedLabel);
                }

            }
            GUILayout.EndVertical();

            GUILayout.EndScrollView();
        }
    }
}