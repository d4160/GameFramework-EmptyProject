using System.Collections;
using System;
using System.Collections.Generic;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class DropdownConstantsAttribute : DrawerAttribute
    {
        public readonly Type SelectFromType;

        public DropdownConstantsAttribute(Type type)
        {
            SelectFromType = type;
        }
    }
}