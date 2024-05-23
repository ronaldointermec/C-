// <copyright file="DbDataReaderExtensions.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using System;
using System.Data;
using System.Linq;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Code;

/// <summary>
/// Extension methods for <see cref="DbDataReaderParser"/>.
/// </summary>
public static class DbDataReaderExtensions
{
    /// <summary>
    /// Gets the <see cref="string"/> value from the database reader object in a safe manner.
    /// </summary>
    /// <param name="reader">Database reader object.</param>
    /// <param name="colName">Column name.</param>
    /// <returns>Null or string value from reader.</returns>
    public static string? SafeGetString(this IDataReader reader, string colName) => reader[colName] is DBNull ? null : Convert.ToString(reader[colName]);

    /// <summary>
    /// Gets the <see cref="int"/> value from the database reader object in a safe manner.
    /// </summary>
    /// <param name="reader">Database reader object.</param>
    /// <param name="colName">Column name.</param>
    /// <returns>Null or string value from reader.</returns>
    public static int? SafeGetInt(this IDataReader reader, string colName) => reader[colName] is DBNull ? null : Convert.ToInt32(reader[colName]);

    /// <summary>
    /// Gets the <see cref="decimal"/> value from the database reader object in a safe manner.
    /// </summary>
    /// <param name="reader">Database reader object.</param>
    /// <param name="colName">Column name.</param>
    /// <returns>Null or string value from reader.</returns>
    public static decimal? SafeGetDecimal(this IDataReader reader, string colName) => reader[colName] is DBNull ? null : Convert.ToDecimal(reader[colName]);

    /// <summary>
    /// Gets the <see cref="bool"/> value from the database reader object in a safe manner.
    /// </summary>
    /// <param name="reader">Database reader object.</param>
    /// <param name="colName">Column name.</param>
    /// <returns>Null or string value from reader.</returns>
    public static bool? SafeGetBool(this IDataReader reader, string colName) => reader[colName] is DBNull ? null : Convert.ToBoolean(reader[colName]);

    /// <summary>
    /// Gets the <see cref="string"/> array value from the database reader object in a safe manner.
    /// </summary>
    /// <param name="reader">Database reader object.</param>
    /// <param name="colName">Column name.</param>
    /// <returns>Null or string value from reader.</returns>
    public static string[] SafeGetStringArray(this IDataReader reader, string colName) => reader[colName] is DBNull ? new string[] { } : Convert.ToString(reader[colName]).Split('|');

    /// <summary>
    /// Gets the <see cref="int"/> array value from the database reader object in a safe manner.
    /// </summary>
    /// <param name="reader">Database reader object.</param>
    /// <param name="colName">Column name.</param>
    /// <returns>Null or string value from reader.</returns>
    public static int[] SafeGetIntArray(this IDataReader reader, string colName) => reader[colName] is DBNull ? new int[] { } : Convert.ToString(reader[colName]).Split('|').Select(int.Parse).ToArray();

    /// <summary>
    /// Gets the <see cref="Uri"/> value from the database reader object in a safe manner.
    /// </summary>
    /// <param name="reader">Database reader object.</param>
    /// <param name="colName">Column name.</param>
    /// <returns>Null or string value from reader.</returns>
    public static Uri? SafeGetUri(this IDataReader reader, string colName) => reader[colName] is DBNull ? null : new Uri(reader[colName].ToString());
}
