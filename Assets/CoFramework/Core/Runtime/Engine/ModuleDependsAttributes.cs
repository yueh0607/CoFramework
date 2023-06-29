using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoFramework
{
    public class ModuleDependsAttribute : Attribute
    {

        public Type[] Depends { get; private set; } 
        public Type Parameter { get; private set; }

        public ModuleDependsAttribute() { Depends = new Type[0]; Parameter = null; }
        public ModuleDependsAttribute(Type Parameter,params Type[] depends)
        {
            this.Parameter = Parameter;
            Depends= depends;
        }

    
    }
}
