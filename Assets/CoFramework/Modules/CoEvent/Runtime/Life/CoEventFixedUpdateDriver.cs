using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoFramework.Events
{
    internal class CoEventFixedUpdateDriver : MonoBehaviour
    {
        void FixedUpdate()
        {

            this.Operator<IFixedUpdate>().Send();
        }
    }
}
