/*
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 *
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.githubusercontent.com/ewoutkramer/fhir-net-api/master/LICENSE
 */

namespace Spark.Engine.Search.Support
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal static class ReflectionHelper
    {
        public static bool CanBeTreatedAsType(this Type CurrentType, Type TypeToCompareWith)
        {
            // Always return false if either Type is null
            if (CurrentType == null || TypeToCompareWith == null)
                return false;


            // Return the result of the assignability test
            return TypeToCompareWith.IsAssignableFrom(CurrentType);
        }


        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        public static T GetAttributeOnEnum<T>(this Enum enumVal)
            where T : Attribute
        {
            var type = enumVal.GetType();
#if PORTABLE45
            var memInfo = type.GetTypeInfo().GetDeclaredField(enumVal.ToString());
#else
            var memInfo = type.GetMember(enumVal.ToString())[0];
#endif
            var attributes = memInfo.GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes.First() : null;
        }


        public static IEnumerable<PropertyInfo> FindPublicProperties(Type t)
        {
            if (t == null) throw Error.ArgumentNull("t");

#if PORTABLE45
			return t.GetRuntimeProperties(); //(BindingFlags.Instance | BindingFlags.Public);
            // return t.GetTypeInfo().DeclaredProperties.Union(t.GetTypeInfo().BaseType.GetTypeInfo().DeclaredProperties); //(BindingFlags.Instance | BindingFlags.Public);
#else
            return t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
#endif
        }

        public static PropertyInfo FindPublicProperty(Type t, string name)
        {
            if (t == null) throw Error.ArgumentNull("t");
            if (name == null) throw Error.ArgumentNull("name");

#if PORTABLE45
            return t.GetRuntimeProperty(name);
#else
            return t.GetProperty(name, BindingFlags.Instance | BindingFlags.Public);
#endif
        }

        internal static MethodInfo FindPublicStaticMethod(Type t, string name, params Type[] arguments)
        {
            if (t == null) throw Error.ArgumentNull("t");
            if (name == null) throw Error.ArgumentNull("name");

#if PORTABLE45
            return t.GetRuntimeMethod(name,arguments);
#else
            return t.GetMethod(name, arguments);
#endif
        }

        internal static bool HasDefaultPublicConstructor(Type t)
        {
            if (t == null) throw Error.ArgumentNull("t");

#if PORTABLE45
			if (t.GetTypeInfo().IsValueType)
                return true;
#else
            if (t.IsValueType)
                return true;
#endif

            return (GetDefaultPublicConstructor(t) != null);
        }

        internal static ConstructorInfo GetDefaultPublicConstructor(Type t)
        {
#if PORTABLE45
            return t.GetTypeInfo().DeclaredConstructors.FirstOrDefault(s => s.GetParameters().Length == 0 && s.IsPublic && !s.IsStatic);
#else
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public;

            return t.GetConstructors(bindingFlags).SingleOrDefault(c => !c.GetParameters().Any());
#endif
        }

        public static bool IsNullableType(Type type)
        {
            if (type == null) throw Error.ArgumentNull("type");

#if PORTABLE45
			return (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
#else
            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
#endif
        }

        public static Type GetNullableArgument(Type type)
        {
            if (type == null) throw Error.ArgumentNull("type");

            if (IsNullableType(type))
            {
#if PORTABLE45
                return type.GenericTypeArguments[0];
#else
                return type.GetGenericArguments()[0];
#endif
            }
            else
                throw Error.Argument("type", "Type {0} is not a Nullable<T>", type.Name);
        }

        public static bool IsTypedCollection(Type type)
        {
            return type.IsArray || ImplementsGenericDefinition(type, typeof(ICollection<>));
        }


        public static IList CreateGenericList(Type itemType)
        {
            return (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));
        }


        public static bool IsClosedGenericType(Type type)
        {
#if PORTABLE45
			return type.GetTypeInfo().IsGenericType && !type.GetTypeInfo().ContainsGenericParameters;
#else
            return type.IsGenericType && !type.ContainsGenericParameters;
#endif
        }


        public static bool IsOpenGenericTypeDefinition(Type type)
        {
#if PORTABLE45
			return type.GetTypeInfo().IsGenericTypeDefinition;
#else
            return type.IsGenericTypeDefinition;
#endif
        }

        public static bool IsConstructedFromGenericTypeDefinition(Type type, Type genericBase)
        {
            return type.GetGenericTypeDefinition() == genericBase;
        }

        /// <summary>
        /// Gets the type of the typed collection's items.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The type of the typed collection's items.</returns>
        public static Type GetCollectionItemType(Type type)
        {
            if (type == null) throw Error.ArgumentNull("type");

            if (type.IsArray)
            {
                return type.GetElementType();
            }

            if (ImplementsGenericDefinition(type, typeof(ICollection<>), out var genericListType))
            {
                //EK: If I look at ImplementsGenericDefinition, I don't think this can actually occur.
                //if (genericListType.IsGenericTypeDefinition)
                //throw Error.Argument("type", "Type {0} is not a collection.", type.Name);

                return genericListType.GetGenericArguments()[0];
            }
            else if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                return null;
            }
            else
            {
                throw Error.Argument("type", "Type {0} is not a collection.", type.Name);
            }
        }

        public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition)
        {
            return ImplementsGenericDefinition(type, genericInterfaceDefinition, out _);
        }

        public static bool ImplementsGenericDefinition(
            Type type,
            Type genericInterfaceDefinition,
            out Type implementingType)
        {
            if (type == null) throw Error.ArgumentNull("type");
            if (genericInterfaceDefinition == null) throw Error.ArgumentNull("genericInterfaceDefinition");

            if (!genericInterfaceDefinition.IsInterface || !genericInterfaceDefinition.IsGenericTypeDefinition)
                throw Error.Argument(
                    "genericInterfaceDefinition",
                    "'{0}' is not a generic interface definition.",
                    genericInterfaceDefinition.Name);
            if (type.IsInterface)
            {
                if (type.IsGenericType)
                {
                    var interfaceDefinition = type.GetGenericTypeDefinition();

                    if (genericInterfaceDefinition == interfaceDefinition)
                    {
                        implementingType = type;
                        return true;
                    }
                }
            }

            foreach (var i in type.GetInterfaces())
            {
                if (i.IsGenericType)
                {
                    var interfaceDefinition = i.GetGenericTypeDefinition();

                    if (genericInterfaceDefinition == interfaceDefinition)
                    {
                        implementingType = i;
                        return true;
                    }
                }
            }

            implementingType = null;
            return false;
        }

        #region << Extension methods to make the handling of PCL easier >>

#if PORTABLE45
		internal static bool IsDefined(this Type t, Type attributeType, bool inherit)
		{
			return t.GetTypeInfo().IsDefined(attributeType, inherit);
		}

		internal static bool IsAssignableFrom(this Type t, Type otherType)
		{
			return t.GetTypeInfo().IsAssignableFrom(otherType.GetTypeInfo());
		}
#endif

        internal static bool IsEnum(this Type t)
        {
#if PORTABLE45
			return t.GetTypeInfo().IsEnum;
#else
            return t.IsEnum;
#endif
        }

        #endregion

        internal static T GetAttribute<T>(MemberInfo member)
            where T : Attribute
        {
            var attr = Attribute.GetCustomAttribute(member, typeof(T));
            return (T)attr;
        }

        internal static ICollection<T> GetAttributes<T>(MemberInfo member)
            where T : Attribute
        {
            var attr = Attribute.GetCustomAttributes(member, typeof(T));
            return (ICollection<T>)attr.Select(a => (T)a);
        }


        internal static IEnumerable<FieldInfo> FindEnumFields(Type t)
        {
            if (t == null) throw Error.ArgumentNull("t");

            return t.GetFields(BindingFlags.Public | BindingFlags.Static);
        }

        internal static bool IsArray(object value)
        {
            if (value == null) throw Error.ArgumentNull("value");

            return value.GetType().IsArray;
        }
    }
}
