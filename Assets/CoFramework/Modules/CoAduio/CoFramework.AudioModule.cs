using CoFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoFramework.Audio
{


    public class AudioModule : IModule
    {
       

        public void OnCreate(CreateParameters parameters)
        {

        }
        public void OnDestroy()
        {

        }

        public void OnUpdate()
        {

        }


        public float GlobalVolume { get; set; } = 1;


     

    }
}