// <copyright file="DbDataReaderParser.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;
using System;
using System.Data;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Code;

/// <summary>
/// Default implementation for <see cref="IDbDataReaderParser"/>.
/// </summary>
public class DbDataReaderParser : IDbDataReaderParser
{
    /// <inheritdoc/>
    public virtual IGetWorkOrderItem? Parse(IDataReader reader)
    {
        if (reader["Code"] is DBNull)
            return new EmptyWork(Convert.ToString(reader["Message"]));

        return reader["Type"] switch
        {
            nameof(BeginPickingOrder) => new BeginPickingOrder(Convert.ToString(reader["Code"]), Convert.ToString(reader["Message"]))
            {
                Customer = reader.SafeGetString("Customer"),
                ContainersCount = reader.SafeGetInt("ContainersCount"),
                ContainerType = reader.SafeGetString("ContainerType"),
                OrderNumber = Convert.ToString(reader["OrderNumber"]),
                Image = reader.SafeGetUri("Image"),
            },
            nameof(PickingLine) => new PickingLine(Convert.ToString(reader["Code"]), Convert.ToString(reader["Message"]))
            {
                Customer = reader.SafeGetString("Customer"),
                Aisle = Convert.ToString(reader["Aisle"]),
                Slot = reader.SafeGetString("Slot"),
                Position = reader.SafeGetString("Position"),
                CD = Convert.ToString(reader["CD"]),
                ProductName = Convert.ToString(reader["ProductName"]),
                ProductNumber = Convert.ToString(reader["ProductNumber"]),
                UpcNumber = Convert.ToString(reader["UpcNumber"]),
                OriginalServingCode = reader.SafeGetString("OriginalServingCode"),
                OriginalServingPrompt = reader.SafeGetString("OriginalServingPrompt"),
                OriginalServingQuantity = reader.SafeGetInt("OriginalServingQuantity"),
                OriginalServingUpperTolerance = reader.SafeGetDecimal("OriginalServingUpperTolerance"),
                OriginalServingLowerTolerance = reader.SafeGetDecimal("OriginalServingLowerTolerance"),
                OriginalMaxQuantityAllowedPerPick = reader.SafeGetInt("OriginalMaxQuantityAllowedPerPick"),
                OriginalUnitsFormat = reader.SafeGetInt("OriginalUnitsFormat"),
                AlternativeServingCode = reader.SafeGetString("AlternativeServingCode"),
                AlternativeServingPrompt = reader.SafeGetString("AlternativeServingPrompt"),
                AlternativeServingQuantity = reader.SafeGetInt("AlternativeServingQuantity"),
                AlternativeServingUpperTolerance = reader.SafeGetDecimal("AlternativeServingUpperTolerance"),
                AlternativeServingLowerTolerance = reader.SafeGetDecimal("AlternativeServingLowerTolerance"),
                AlternativeMaxQuantityAllowedPerPick = reader.SafeGetInt("AlternativeMaxQuantityAllowedPerPick"),
                AlternativeUnitsFormat = reader.SafeGetInt("AlternativeUnitsFormat"),
                UnitsQuantity = reader.SafeGetInt("UnitsQuantity"),
                UnitsUpperTolerance = reader.SafeGetDecimal("UnitsUpperTolerance"),
                UnitsLowerTolerance = reader.SafeGetDecimal("UnitsLowerTolerance"),
                UnitsMaxQuantityAllowedPerPick = reader.SafeGetInt("UnitsMaxQuantityAllowedPerPick"),
                AskWeight = reader.SafeGetBool("AskWeight"),
                WeightMin = reader.SafeGetDecimal("WeightMin"),
                WeightMax = reader.SafeGetDecimal("WeightMax"),
                HowMuchMore = reader.SafeGetInt("HowMuchMore"),
                StockCounting = reader["StockCounting"] is DBNull ? StockCountingMode.No : (StockCountingMode)Convert.ToInt32(reader["StockCounting"]),
                SpeakProductName = reader.SafeGetBool("SpeakProductName"),
                AskBatch = reader.SafeGetBool("AskBatch"),
                Batches = reader.SafeGetStringArray("Batches"),
                CanSkipAisle = reader.SafeGetBool("CanSkipAisle"),
                HelpMsg = reader.SafeGetString("HelpMsg"),
                CountdownPick = reader.SafeGetBool("CountdownPick"),
                ProductCDs = reader.SafeGetStringArray("ProductCDs"),
                ValidateLocationImage = reader.SafeGetUri("ValidateLocationImage"),
                ValidateProductImage = reader.SafeGetUri("ValidateProductImage"),
                BatchesImage = reader.SafeGetUri("BatchesImage"),
                PickQuantityImage = reader.SafeGetUri("PickQuantityImage"),
                StockCountingImage = reader.SafeGetUri("StockCountingImage"),
                PlaceInDockImage = reader.SafeGetUri("PlaceInDockImage"),
            },
            nameof(PlaceInDock) => new PlaceInDock(Convert.ToString(reader["Code"]), Convert.ToString(reader["Message"]))
            {
                Dock = reader.SafeGetString("Dock"),
                CD = reader.SafeGetString("CD"),
                Image = reader.SafeGetUri("Image"),
            },
            nameof(PrintLabels) => new PrintLabels(Convert.ToString(reader["Code"]), Convert.ToString(reader["Message"]))
            {
                DefaultPrinter = reader.SafeGetInt("DefaultPrinter"),
                Copies = reader.SafeGetInt("Copies"),
                Printers = reader.SafeGetIntArray("Printers"),
                CopiesImage = reader.SafeGetUri("CopiesImage"),
                PrinterImage = reader.SafeGetUri("PrinterImage"),
            },
            nameof(ValidatePrinting) => new ValidatePrinting(Convert.ToString(reader["Code"]), Convert.ToString(reader["Message"]))
            {
                ValidationCodes = reader.SafeGetStringArray("ValidationCodes"),
                VoiceLength = reader.SafeGetInt("VoiceLength"),
                ConfirmImage = reader.SafeGetUri("ConfirmImage"),
                ValidateLabelsImage = reader.SafeGetUri("ValidateLabelsImage"),
            },
            nameof(AskQuestion) => new AskQuestion(Convert.ToString(reader["Code"]), Convert.ToString(reader["Message"]))
            {
                Image = reader.SafeGetUri("Image"),
            },
            _ => null,
        };
    }
}