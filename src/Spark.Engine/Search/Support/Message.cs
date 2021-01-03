// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace Spark.Engine.Search.Support
{
    using System.Diagnostics;

    internal static class Message
    {
        internal static void Info(string messageFormat, params object[] messageArgs)
        {
#if DEBUG
            Debug.WriteLine(Error.FormatMessage(messageFormat,messageArgs));
#endif
        }
    }
}
