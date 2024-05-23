// <copyright file="Vars.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

namespace Honeywell.GWS.Connector.Library.Workflows.Picking;

/// <summary>
/// Extension class to set device variable names.
/// </summary>
public static class Vars
{
    /// <summary>
    /// Keeps track of the dialog state machine.
    /// </summary>
    public static readonly string DLG = "DLG";

    /// <summary>
    /// Contains selected menu option.
    /// </summary>
    public static readonly string MENU = "MENU";

    /// <summary>
    /// Contains the response type to confirm specific result.
    /// </summary>
    public static readonly string RESPONSETYPE = "RESPONSETYPE";

    /// <summary>
    /// Contains the status of the work performed.
    /// </summary>
    public static readonly string STATUS = "STATUS";

    /// <summary>
    /// Contains the start time of the work.
    /// </summary>
    public static readonly string START_TIME = "START_TIME";

    /// <summary>
    /// Contains the end time of the work.
    /// </summary>
    public static readonly string END_TIME = "END_TIME";

    /// <summary>
    /// Dummy variable used for helping composing instructions set.
    /// </summary>
    public static readonly string DUMMY = "DUMMY";

    /// <summary>
    /// Prompt variable used to prompt large messages.
    /// </summary>
    public static readonly string PROMPT = "PROMPT";

    /// <summary>
    /// Contains the dock selected.
    /// </summary>
    public static readonly string DOCK = "DOCK";

    /// <summary>
    /// Contains the quantity of labels to print.
    /// </summary>
    public static readonly string LABELS = "LABELS";

    /// <summary>
    /// Contains the batch selected.
    /// </summary>
    public static readonly string BATCH = "BATCH";

    /// <summary>
    /// Contains the quantity picked.
    /// </summary>
    public static readonly string PICKED = "PICKED";

    /// <summary>
    /// Contains the total quantity picked.
    /// </summary>
    public static readonly string TOTAL_PICKED = "TOTAL_PICKED";

    /// <summary>
    /// Contains the weight selected.
    /// </summary>
    public static readonly string WEIGHT = "WEIGHT";

    /// <summary>
    /// Contains the product description.
    /// </summary>
    public static readonly string PRODUCT_DESCRIPTION = "PRODUCT_DESCRIPTION";

    /// <summary>
    /// Contains the product number.
    /// </summary>
    public static readonly string PRODUCT_NUMBER = "PRODUCT_NUMBER";

    /// <summary>
    /// Contains the UPC number.
    /// </summary>
    public static readonly string UPC_NUMBER = "UPC_NUMBER";

    /// <summary>
    /// Contains the total weight selected.
    /// </summary>
    public static readonly string TOTAL_WEIGHT = "TOTAL_WEIGHT";

    /// <summary>
    /// Contains the quantity maximum quantity allowed per pick.
    /// </summary>
    public static readonly string MAX_QTY_ALLOWED_PER_PICK = "MAX_QTY_ALLOWED_PER_PICK";

    /// <summary>
    /// Contains the customer of the order.
    /// </summary>
    public static readonly string CUSTOMER = "CUSTOMER";

    /// <summary>
    /// Contains the last position confirmed.
    /// </summary>
    public static readonly string CURRENT_POSITION = "CURRENT_POSITION";

    /// <summary>
    /// Contains the last aisle confirmed.
    /// </summary>
    public static readonly string CURRENT_AISLE = "CURRENT_AISLE";

    /// <summary>
    /// Contains the selected serving code.
    /// </summary>
    public static readonly string SERVING = "SERVING";

    /// <summary>
    /// Contains the quantity of stock counted.
    /// </summary>
    public static readonly string STOCK = "STOCK";

    /// <summary>
    /// Contains the quantity of breakage.
    /// </summary>
    public static readonly string BREAKAGE = "BREAKAGE";

    /// <summary>
    /// Contains the upper quantity allowed to pick.
    /// </summary>
    public static readonly string QTY_UPPER = "QTY_UPPER";

    /// <summary>
    /// Contains the lower quantity allowed to pick.
    /// </summary>
    public static readonly string QTY_LOWER = "QTY_LOWER";

    /// <summary>
    /// Used to indicate whether the line has package.
    /// </summary>
    public static readonly string SKIPPING_AISLE = "SKIPPING_AISLE";

    /// <summary>
    /// Contains the last position confirmed which includes aisle and position.
    /// </summary>
    public static readonly string WHEREAMI = "WHEREAMI";

    /// <summary>
    /// Contains the check digit to validate product.
    /// </summary>
    public static readonly string PRODUCT_CD = "PRODUCT_CD";

    /// <summary>
    /// Contains the quantity to be picked.
    /// </summary>
    public static readonly string QTY_REQUESTED = "QTY_REQUESTED";

    /// <summary>
    /// Used to save quantity temporarily.
    /// </summary>
    public static readonly string QTY_REQUESTED_ORIG = "QTY_REQUESTED_ORIG";

    /// <summary>
    /// Contains work identifier.
    /// </summary>
    public static readonly string CODE = "CODE";

    /// <summary>
    /// Contains the current printer.
    /// </summary>
    public static readonly string PRINTER = "PRINTER";

    /// <summary>
    /// Contains the number of labels to print.
    /// </summary>
    public static readonly string LABELS_COUNT = "LABELS_COUNT";

    /// <summary>
    /// Contains the break reason.
    /// </summary>
    public static readonly string BREAK_REASON = "BREAK_REASON";
}