﻿/* 
 * Copyright (c) 2014, Furore (info@furore.com) and contributors
 * See the file CONTRIBUTORS for details.
 * 
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Spark.Engine.Auxiliary
{
    public static class ResourceVisitor
    {
        public static void VisitByType(object item, Visitor action, params Type[] filter)
        {
            // This is a filter that returns true if the property in pInfo is a subtype
            // of one of the types given in the filter. Because of this, scan() returns
            // all Elements in item that are of the types in filter, or subclasses.
            void Visitor(Element elem, string path)
            {
                foreach (var t in filter)
                {
                    var type = elem.GetType();
                    if (t.IsAssignableFrom(type))
                    {
                        action(elem, path);
                    }
                }
            }

            Scan(item, null, Visitor);
        }

        private static bool PropertyFilter(MemberInfo mem, object arg)
        {
            // We prefilter on properties, so this cast is always valid
            var prop = (PropertyInfo) mem;

            // Return true if the property is either an Element or an IEnumerable<Element>.
            var isElementProperty = typeof(Element).IsAssignableFrom(prop.PropertyType);
            var collectionInterface = prop.PropertyType.GetInterface("IEnumerable`1");
            var isElementCollection = false;
            var hasIndexParameters = prop.GetIndexParameters().Length > 0;

            if (collectionInterface != null)
            {
                var firstGenericArg = collectionInterface.GetGenericArguments()[0];
                isElementCollection = typeof(Element).IsAssignableFrom(firstGenericArg);
            }

            return (isElementProperty || isElementCollection) && hasIndexParameters == false;
        }

        private static string JoinPath(string old, string part)
        {
            return !string.IsNullOrEmpty(old) ? old + "." + part : part;
        }

        private static void Scan(object item, string path, Visitor visitor)
        {
            if (item == null)
            {
                return;
            }

            path ??= string.Empty;

            // Scan the object 'item' and find all properties of type Element of IEnumerable<Element>
            var result = item.GetType()
                .FindMembers(MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public, PropertyFilter, null);

            // Do a depth-first traversal of the properties and their contents
            foreach (PropertyInfo property in result)
            {
                // If this member is an IEnumerable<Element>, go inside and recurse
                if (property.PropertyType.GetInterface("IEnumerable`1") != null)
                {
                    // Since we filter for Properties of Element or IEnumerable<Element>
                    // this cast should always work
                    var list = (IEnumerable<Element>) property.GetValue(item, null);

                    if (list != null)
                    {
                        var index = 0;
                        foreach (var element in list)
                        {
                            var propertyPath = JoinPath(path, property.Name + "[" + index.ToString() + "]");

                            if (element != null)
                            {
                                visitor(element, propertyPath);
                                Scan(element, propertyPath, visitor);
                            }
                        }
                    }
                }

                // If this member is an Element, go inside and recurse
                else
                {
                    var propertyPath = JoinPath(path, property.Name);

                    var propValue = (Element) property.GetValue(item);

                    // Look into the property to find nested elements
                    if (propValue != null)
                    {
                        visitor(propValue, propertyPath);
                        Scan(propValue, propertyPath, visitor);
                    }
                }
            }
        }
    }
}
