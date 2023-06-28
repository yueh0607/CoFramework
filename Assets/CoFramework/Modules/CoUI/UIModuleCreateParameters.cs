﻿using CoFramework;
using CoFramework.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoFramework.UI
{
    public class UIModuleCreateParameters : CreateParameters
    {
        public bool AutoLoadDefaultEventSystem = true;
        public Action<CoTask> InitAction = null;
        public string UIRootLocation = "UIRoot";
        public bool NeedUICamera = false;
        public string UICameraLocation = "UICamera";
    }
}