using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Lance.Common
{
    public static class CoreWrapperEx
    {
        private static readonly Type CompilerType = typeof(CompilerGeneratedAttribute);

        public static Type GetDeclaringType(this MethodInfo methodInfo) { return methodInfo.ReflectedType; }

        public static MethodInfo GetMethodInfoEx(this Delegate delegateEx)
        {
            var method = delegateEx.Method;
            return method;
        }

        public static Type[] GetInterfacesEx(this Type type) { return type.GetInterfaces(); }

        public static bool IsInterfaceEx(this Type type) { return type.IsInterface; }

        public static bool IsValueTypeEx(this Type type) { return type.IsValueType; }

        public static Type GetDeclaringType(this MemberInfo memberInfo) { return memberInfo.ReflectedType; }

        public static Type GetBaseType(this Type type) { return type.BaseType; }

        public static IEnumerable<Attribute> GetCustomAttributes(this Type type, bool inherit) { return Attribute.GetCustomAttributes(type, inherit); }

        public static bool ContainsCustomAttribute(this MemberInfo memberInfo, Type customAttribute, bool inherit = false)
        {
            return Attribute.IsDefined(memberInfo, customAttribute, inherit);
        }

        public static bool ContainsCustomAttribute(this FieldInfo memberInfo, Type customAttribute, bool inherit = false)
        {
            return Attribute.IsDefined(memberInfo, customAttribute, inherit);
        }

        public static bool IsGenericTypeEx(this Type type) { return type.IsGenericType; }

        public static Type[] GetGenericArgumentsEx(this Type type) { return type.GetGenericArguments(); }

        public static MemberInfo[] FindWritablePropertiesWithCustomAttribute(this Type contract, Type customAttributeType)
        {
            var propertyList = new FasterList<MemberInfo>(8);

            do
            {
                var propertyInfos = contract.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance);

                for (var i = 0; i < propertyInfos.Length; i++)
                {
                    var propertyInfo = propertyInfos[i];

                    if (propertyInfo.CanWrite && propertyInfo.ContainsCustomAttribute(customAttributeType))
                        propertyList.Add(propertyInfo);
                }

                contract = contract.GetBaseType();
            } while (contract != null);

            if (propertyList.Count > 0)
                return propertyList.ToArray();

            return null;
        }

        public static bool IsCompilerGenerated(this Type t)
        {
            var attr = Attribute.IsDefined(t, typeof(CompilerGeneratedAttribute));

            return attr;
        }

        public static bool IsCompilerGenerated(this MemberInfo memberInfo)
        {
            var attr = Attribute.IsDefined(memberInfo, CompilerType);

            return attr;
        }
    }
}