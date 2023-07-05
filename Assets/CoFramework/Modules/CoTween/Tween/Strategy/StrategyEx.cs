using CoFramework.RefBuild;
using System;
using UnityEngine;

namespace CoFramework.Tween
{
    public static class StrategyEx
    {

        internal static ISteper GetSteper<T>() where T : IEquatable<T>
        {
            Type type = typeof(T);

            if (type == typeof(int)) return Framework.GlobalAllocate<IntSteper>();
            if (type == typeof(float)) return Framework.GlobalAllocate<FloatSteper>();
            if (type == typeof(double)) return Framework.GlobalAllocate<DoubleSteper>();
            if (type == typeof(Vector2)) return Framework.GlobalAllocate<Vector2Steper>();
            if (type == typeof(Vector3)) return Framework.GlobalAllocate<Vector3Steper>();
            if (type == typeof(Color)) return Framework.GlobalAllocate<ColorSteper>();

            throw new InvalidOperationException($"Do not Have Steper<{type}>!");
        }

        internal static ISteper GetSteperWithParm<T>(T start, T end, BindableProperty<T> port) where T : IEquatable<T>
        {
            var stepter = (TweenSteper<T>)GetSteper<T>();
            stepter.ValueStart = start;
            stepter.ValueEnd = end;
            stepter.Current = port;
            return stepter;
        }


    }
}
