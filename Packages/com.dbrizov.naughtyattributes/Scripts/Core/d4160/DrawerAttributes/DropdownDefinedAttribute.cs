using System.Collections;
using System;
using System.Collections.Generic;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class DropdownDefinedAttribute : DrawerAttribute
    {
        public readonly object[] ValuesArray;

        public DropdownDefinedAttribute(params object[] definedValues)
        {
            ValuesArray = definedValues;
        }
    }
}