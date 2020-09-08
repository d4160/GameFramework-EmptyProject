using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class RequireTypeAttribute : DrawerAttribute
    {
        public Type RequiredType { get; private set; }
        public Type[] GenericArguments { get; private set; }

        public RequireTypeAttribute(Type type, params Type[] genericArguments)
        {
            RequiredType = type;
            GenericArguments = genericArguments;
        }
    }
}