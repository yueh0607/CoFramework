using PlasticGui;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CoFramework.EngineEditor
{


    public class CoFrameworkWindow : EditorWindow
    {
        [MenuItem("CoFramework", menuItem = "CoFramework/About")]
        static void Open()
        {
            var window = GetWindow<CoFrameworkWindow>("CoFramework");
            window.minSize = new Vector2(600, 400);
            window.Show();
        }


        Texture logo;
        private void OnEnable()
        {
            logo = AssetDatabase.LoadAssetAtPath<Texture>("Assets/CoFramework/Core/Editor/Icons/logo.png");
        }

        private void OnGUI()
        {
            if (logo == null) Debug.Log("null");
            //GUILayout.Label(logo, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            // 获取顶端区域的矩形范围
            Rect rect = GUILayoutUtility.GetRect(0, position.width, 0, logo.height);

            // 绘制图片
            GUI.DrawTexture(rect, logo, ScaleMode.ScaleAndCrop, true);
            string introducation = "CoFramework是一款敏捷游戏开发框架，宗旨为“简单”，“实用”，“灵活”。" +
                "框架采用模块化的开发方式，内核极简化，在模块之间发生依赖和关系，不求有功但求无过，" +
                "提供一系列常用模块的基础上为用户提供模块拓展方式。";
            GUILayout.TextArea(introducation, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));


            if (GUILayout.Button("Github - CoFramework"))
            {
                Application.OpenURL("https://github.com/yueh0607/CoFramework/tree/main");
            }
        }
    }
}