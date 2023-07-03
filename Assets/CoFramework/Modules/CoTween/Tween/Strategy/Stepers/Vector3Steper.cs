using CoFramework.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoFramework.Tween
{
    public class Vector3Steper : TweenSteper<Vector3>
    {
        //public override float GetDistance()
        //{
        //    return (ValueEnd - Current.Value).sqrMagnitude;
        //}

        public override void MoveNext(float step)
        {
            Current.Value = LerpHelper.Lerp(ValueStart, ValueEnd, step);
        }

    }
}
