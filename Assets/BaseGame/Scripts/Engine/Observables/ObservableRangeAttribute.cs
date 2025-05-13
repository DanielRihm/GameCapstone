using UnityEngine;
using System;

namespace LCPS.SlipForge.Engine
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ObservableRangeAttribute : PropertyAttribute
    {
        public float Min;
        public float Max;

        public ObservableRangeAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }
}
