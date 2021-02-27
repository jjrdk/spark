﻿// /*
//  * Copyright (c) 2014, Furore (info@furore.com) and contributors
//  * See the file CONTRIBUTORS for details.
//  *
//  * This file is licensed under the BSD 3-Clause license
//  * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
//  */

namespace Spark.Engine.Search.Support
{
    using System;
    using System.Globalization;

    /// <summary>
    ///     Utility class for creating and unwrapping <see cref="Exception" /> instances.
    /// </summary>
    internal static class Error
    {
        /// <summary>
        ///     Formats the specified resource string using <see cref="M:CultureInfo.CurrentCulture" />.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>The formatted string.</returns>
        internal static string FormatMessage(string format, params object[] args) =>
            string.Format(CultureInfo.CurrentCulture, format, args);

        /// <summary>
        ///     Creates an <see cref="ArgumentException" /> with the provided properties.
        /// </summary>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception" />.</returns>
        internal static ArgumentException Argument(string messageFormat, params object[] messageArgs) =>
            new ArgumentException(FormatMessage(messageFormat, messageArgs));

        /// <summary>
        ///     Creates an <see cref="ArgumentException" /> with the provided properties.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that caused the current exception.</param>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception" />.</returns>
        internal static ArgumentException Argument(
            string parameterName,
            string messageFormat,
            params object[] messageArgs) =>
            new ArgumentException(FormatMessage(messageFormat, messageArgs), parameterName);

        /// <summary>
        ///     Creates an <see cref="ArgumentNullException" /> with the provided properties.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that caused the current exception.</param>
        /// <returns>The logged <see cref="Exception" />.</returns>
        internal static ArgumentException ArgumentNull(string parameterName) =>
            new ArgumentNullException(parameterName);

        /// <summary>
        ///     Creates an <see cref="ArgumentNullException" /> with the provided properties.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that caused the current exception.</param>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception" />.</returns>
        internal static ArgumentNullException ArgumentNull(
            string parameterName,
            string messageFormat,
            params object[] messageArgs) =>
            new ArgumentNullException(parameterName, FormatMessage(messageFormat, messageArgs));

        /// <summary>
        ///     Creates an <see cref="ArgumentException" /> with a default message.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that caused the current exception.</param>
        /// <returns>The logged <see cref="Exception" />.</returns>
        internal static ArgumentException ArgumentNullOrEmpty(string parameterName) =>
            Argument(parameterName, "The argument '{0}' is null or empty.", parameterName);


        /// <summary>
        ///     Creates an <see cref="InvalidOperationException" />.
        /// </summary>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception" />.</returns>
        internal static InvalidOperationException InvalidOperation(string messageFormat, params object[] messageArgs) =>
            new InvalidOperationException(FormatMessage(messageFormat, messageArgs));

        /// <summary>
        ///     Creates an <see cref="InvalidOperationException" />.
        /// </summary>
        /// <param name="innerException">Inner exception</param>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception" />.</returns>
        internal static InvalidOperationException InvalidOperation(
            Exception innerException,
            string messageFormat,
            params object[] messageArgs) =>
            new InvalidOperationException(FormatMessage(messageFormat, messageArgs), innerException);

        /// <summary>
        ///     Creates an <see cref="NotSupportedException" />.
        /// </summary>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception" />.</returns>
        internal static NotSupportedException NotSupported(string messageFormat, params object[] messageArgs) =>
            new NotSupportedException(FormatMessage(messageFormat, messageArgs));

        /// <summary>
        ///     Creates an <see cref="FormatException" /> with the provided properties.
        /// </summary>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="pos">Optional line position information for the message</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception" />.</returns>
        internal static FormatException Format(string messageFormat, IPostitionInfo pos, params object[] messageArgs)
        {
            var message = pos != null
                ? $"At line {pos.LineNumber}, pos {pos.LinePosition}: {FormatMessage(messageFormat, messageArgs)}"
                : FormatMessage(messageFormat, messageArgs);

            return new FormatException(message);
        }

        /// <summary>
        ///     Creates an <see cref="NotImplementedException" />.
        /// </summary>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception" />.</returns>
        internal static NotImplementedException NotImplemented(string messageFormat, params object[] messageArgs) =>
            new NotImplementedException(FormatMessage(messageFormat, messageArgs));

        /// <summary>
        ///     Creates an <see cref="NotImplementedException" />.
        /// </summary>
        /// <returns></returns>
        internal static NotImplementedException NotImplemented() => new NotImplementedException();
    }
}