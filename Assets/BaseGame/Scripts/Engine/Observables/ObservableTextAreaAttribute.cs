using System;
using UnityEngine;

namespace LCPS.SlipForge.Engine
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ObservableTextAreaAttribute : PropertyAttribute
    {
        public int MinLines;
        public int MaxLines;

        public ObservableTextAreaAttribute(int minLines, int maxLines)
        {
            MinLines = minLines;
            MaxLines = maxLines;
        }
    }
}
