using CoFramework.Utility;
using UnityEngine;

namespace CoFramework.Tween
{
    public class ColorSteper : TweenSteper<Color>
    {

        public override void MoveNext(float step)
        {
            Current.Value = LerpHelper.Lerp(ValueStart, ValueEnd, step);
        }

    }
}
