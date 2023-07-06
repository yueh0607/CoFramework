using CoFramework.Utility;
using UnityEngine;

namespace CoFramework.Tween
{
    public class Vector2Steper : TweenSteper<Vector2>
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
