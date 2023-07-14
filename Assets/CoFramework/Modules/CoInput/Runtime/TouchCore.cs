using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Input = UnityEngine.Input;
namespace CoFramework.Inputs
{
    public class TouchCore 
    {
        public Dictionary<int, Touch> Touches { get; private set; } = new Dictionary<int, Touch>();
        public void DriveUpdate()
        {
            for(int i =0;i<Input.touchCount;i++)
            {
                
            }
        }

    }
}
