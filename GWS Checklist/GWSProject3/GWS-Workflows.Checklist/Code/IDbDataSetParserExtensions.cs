// <copyright file="IDbDataSetParserExtensions.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using System;
using System.Data;

namespace Honeywell.GWS.Connector.Library.Workflows.Checklist.Code;

/// <summary>
/// Extensions methods for <see cref="DbDataSetParser"/>.
/// </summary>
public static class IDbDataSetParserExtensions
{
    /// <summary>
    /// Gets the <see cref="string"/> value from the database dataset object in a safe manner.
    /// </summary>
    /// <param name="row">Database row object.</param>
    /// <param name="colName">Column name.</param>
    /// <returns>Null or string value from row.</returns>
    public static string? SafeGetString(this DataRow row, string colName) => row[colName] is DBNull ? null : Convert.ToString(row[colName]);

    /// <summary>
    /// Gets the <see cref="string"/> value from the database reader object in a safe manner.
    /// </summary>
    /// <param name="reader">Database reader object.</param>
    /// <param name="colName">Column name.</param>
    /// <returns>Null or string value from reader.</returns>
    public static string? SafeGetString(this IDataReader reader, string colName) => reader[colName] is DBNull ? null : Convert.ToString(reader[colName]);

    /// <summary>
    /// Gets the <see cref="int"/> value from the database dataset object in a safe manner.
    /// </summary>
    /// <param name="row">Database row object.</param>
    /// <param name="colName">Column name.</param>
    /// <returns>Null or int value from row.</returns>
    public static int? SafeGetInt(this DataRow row, string colName) => row[colName] is DBNull ? null : Convert.ToInt32(row[colName]);

    /// <summary>
    /// Gets the <see cref="short"/> value from the database dataset object in a safe manner.
    /// </summary>
    /// <param name="row">Database row object.</param>
    /// <param name="colName">Column name.</param>
    /// <returns>Null or short value from row.</returns>
    public static short? SafeGetShort(this DataRow row, string colName) => row[colName] is DBNull ? null : Convert.ToInt16(row[colName]);

    /// <summary>
    /// Gets the <see cref="decimal"/> value from the database dataset object in a safe manner.
    /// </summary>
    /// <param name="row">Database row object.</param>
    /// <param name="colName">Column name.</param>
    /// <returns>Null or decimal value from row.</returns>
    public static decimal? SafeGetDecimal(this DataRow row, string colName) => row[colName] is DBNull ? null : Convert.ToDecimal(row[colName]);

    /// <summary>
    /// Gets the <see cref="bool"/> value from the database dataset object in a safe manner.
    /// </summary>
    /// <param name="row">Database row object.</param>
    /// <param name="colName">Column name.</param>
    /// <returns>Null or bool value from row.</returns>
    public static bool? SafeGetBool(this DataRow row, string colName) => row[colName] is DBNull ? null : Convert.ToBoolean(row[colName]);

    /// <summary>
    /// Gets the <see cref="bool"/> value from the database dataset object in a safe manner.
    /// </summary>
    /// <param name="row">Database row object.</param>
    /// <param name="colName">Column name.</param>
    /// <returns>Null or bool value from row.</returns>
    public static DateTime? SafeGetDateTime(this DataRow row, string colName) => row[colName] is DBNull ? null : Convert.ToDateTime(row[colName]);

    /// <summary>
    /// Gets the <see cref="Uri"/> value from the database dataset object in a safe manner.
    /// </summary>
    /// <param name="row">Database row object.</param>
    /// <param name="colName">Column name.</param>
    /// <returns>Null or Uri value from row.</returns>
    public static Uri? SafeGetUri(this DataRow row, string colName) => row[colName] is DBNull ? null : new Uri(row[colName].ToString());
}
