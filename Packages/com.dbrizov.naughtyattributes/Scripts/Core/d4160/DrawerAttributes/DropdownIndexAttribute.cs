using System.Collections;
using System;
using System.Collections.Generic;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class DropdownIndexAttribute : DrawerAttribute
    {
        public string ValuesName { get; private set; }

        public DropdownIndexAttribute(string valuesName)
        {
            ValuesName = valuesName;
        }
    }
}