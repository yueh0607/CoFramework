﻿using UnityEngine;
namespace CoFramework.RefBuild
{

    public class ScriptMark : MonoBehaviour
    {

#if UNITY_EDITOR

        public Component buildTarget = null;

        public string buildProperty = null;

#endif

    }
}
