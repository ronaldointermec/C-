// <copyright file="PickingLine.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Properties;
using Honeywell.GWS.Connector.SDK;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Models;

/// <summary>
/// Implementation class for Picking Line work.
/// </summary>
public class PickingLine : GetWorkOrderItemBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PickingLine"/> class.
    /// </summary>
    public PickingLine()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PickingLine"/> class.
    /// </summary>
    /// <param name="code">Work identifier.</param>
    /// <param name="message">Optional message.</param>
    public PickingLine(string code, string? message = null)
        : base(code, message)
    {
    }

    /// <summary>
    /// Gets the order customer.
    /// </summary>
    public virtual string? Customer { get; init; }

    /// <summary>
    /// Gets the pending lines.
    /// </summary>
    public virtual int? HowMuchMore { get; init; }

    /// <summary>
    /// Gets the aisle.
    /// </summary>
    public virtual string Aisle { get; init; } = null!;

    /// <summary>
    /// Gets the slot.
    /// </summary>
    public virtual string? Slot { get; init; }

    /// <summary>
    /// Gets the position.
    /// </summary>
    public virtual string? Position { get; init; }

    /// <summary>
    /// Gets the check digit to validate position.
    /// </summary>
    public virtual string? CD { get; init; }

    /// <summary>
    /// Gets the product name.
    /// </summary>
    public virtual string ProductName { get; init; } = null!;

    /// <summary>
    /// Gets the product number.
    /// </summary>
    public virtual string ProductNumber { get; init; } = null!;

    /// <summary>
    /// Gets the UPC number.
    /// </summary>
    public virtual string UpcNumber { get; init; } = null!;

    /// <summary>
    /// Gets the spoken name of the product.
    /// </summary>
    public virtual bool? SpeakProductName { get; init; }

    /// <summary>
    /// Gets the original serving code.
    /// </summary>
    public virtual string? OriginalServingCode { get; init; }

    /// <summary>
    /// Gets the original serving prompt.
    /// </summary>
    public virtual string? OriginalServingPrompt { get; init; }

    /// <summary>
    /// Gets the original serving quantity.
    /// </summary>
    public virtual int? OriginalServingQuantity { get; init; }

    /// <summary>
    /// Gets the original units format.
    /// </summary>
    public virtual int? OriginalUnitsFormat { get; init; }

    /// <summary>
    /// Gets the original serving upper tolerance.
    /// </summary>
    public virtual decimal? OriginalServingUpperTolerance { get; init; }

    /// <summary>
    /// Gets the original serving lower tolerance.
    /// </summary>
    public virtual decimal? OriginalServingLowerTolerance { get; init; }

    /// <summary>
    /// Gets the maximum quantity allowed per pick when original format is selected.
    /// </summary>
    public virtual int? OriginalMaxQuantityAllowedPerPick { get; init; }

    /// <summary>
    /// Gets the alternative serving code.
    /// </summary>
    public virtual string? AlternativeServingCode { get; init; }

    /// <summary>
    /// Gets the alternative serving prompt.
    /// </summary>
    public virtual string? AlternativeServingPrompt { get; init; }

    /// <summary>
    /// Gets the alternative serving quantity.
    /// </summary>
    public virtual int? AlternativeServingQuantity { get; init; }

    /// <summary>
    /// Gets the alternative units format.
    /// </summary>
    public virtual int? AlternativeUnitsFormat { get; init; }

    /// <summary>
    /// Gets the alternative serving upper tolerance.
    /// </summary>
    public virtual decimal? AlternativeServingUpperTolerance { get; init; }

    /// <summary>
    /// Gets the alternative serving lower tolerance.
    /// </summary>
    public virtual decimal? AlternativeServingLowerTolerance { get; init; }

    /// <summary>
    /// Gets the maximum quantity allowed per pick when alternative format is selected.
    /// </summary>
    public virtual int? AlternativeMaxQuantityAllowedPerPick { get; init; }

    /// <summary>
    /// Gets the quantity in units format.
    /// </summary>
    public virtual int? UnitsQuantity { get; init; }

    /// <summary>
    /// Gets the upper tolerance when units format is selected.
    /// </summary>
    public virtual decimal? UnitsUpperTolerance { get; init; }

    /// <summary>
    /// Gets the lower tolerance when units format is selected.
    /// </summary>
    public virtual decimal? UnitsLowerTolerance { get; init; }

    /// <summary>
    /// Gets the maximum quantity allowed per pick when units format is selected.
    /// </summary>
    public virtual int? UnitsMaxQuantityAllowedPerPick { get; init; }

    /// <summary>
    /// Gets a value indicating whether partial picking is allowed.
    /// </summary>
    public bool? CountdownPick { get; init; }

    /// <summary>
    /// Gets the help message used in picking prompt.
    /// </summary>
    public virtual string? HelpMsg { get; init; }

    /// <summary>
    /// Gets whether to ask for weight.
    /// </summary>
    public virtual bool? AskWeight { get; init; }

    /// <summary>
    /// Gets the minimum weight accepted.
    /// </summary>
    /// <remarks>When <see cref="CountdownPick"/> is enabled, refers to each unit instead of the whole pick operation.</remarks>
    public virtual decimal? WeightMin { get; init; }

    /// <summary>
    /// Gets the maximum weight accepted.
    /// </summary>
    /// <remarks>When <see cref="CountdownPick"/> is enabled, refers to each unit instead of the whole pick operation.</remarks>
    public virtual decimal? WeightMax { get; init; }

    /// <summary>
    /// Gets the stock counting mode (0: Do not count stock, 1: Count stock only if the quantity picked is partial. 3: Count stock).
    /// </summary>
    public virtual StockCountingMode StockCounting { get; init; }

    /// <summary>
    /// Gets the list of check digits to validate product.
    /// </summary>
    public virtual IEnumerable<string>? ProductCDs { get; init; }

    /// <summary>
    /// Gets a value indicating whether the operator can skip aisle.
    /// </summary>
    public virtual bool? CanSkipAisle { get; init; }

    /// <summary>
    /// Gets whether to ask for batch.
    /// </summary>
    public virtual bool? AskBatch { get; init; }

    /// <summary>
    /// Gets the list of allowed batches.
    /// </summary>
    public virtual IEnumerable<string>? Batches { get; init; }

    /// <summary>
    /// Gets an image uri to be displayed while validating location.
    /// </summary>
    public virtual Uri? ValidateLocationImage { get; init; }

    /// <summary>
    /// Gets an image uri to be displayed while validating product.
    /// </summary>
    public virtual Uri? ValidateProductImage { get; init; }

    /// <summary>
    /// Gets an image uri to be displayed while picking quantity.
    /// </summary>
    public virtual Uri? PickQuantityImage { get; init; }

    /// <summary>
    /// Gets an image uri to be displayed while asking for batches.
    /// </summary>
    public virtual Uri? BatchesImage { get; init; }

    /// <summary>
    /// Gets an image uri to be displayed while asking for stock left.
    /// </summary>
    public virtual Uri? StockCountingImage { get; init; }

    /// <summary>
    /// Gets an image uri to be displayed while placing in dock (only Batch mode).
    /// </summary>
    public virtual Uri? PlaceInDockImage { get; init; }

    /// <inheritdoc/>
    public override void BuildDialog(InstructionSet i, IPickingBehavior behavior, IDevice device)
    {
        i.Label($"{Labels.START}_{Code}");

        // Variables initialization
        i.AssignStr(Vars.STATUS, string.Empty);
        i.AssignStr(Vars.PICKED, string.Empty);
        i.AssignStr(Vars.SERVING, string.Empty);
        i.AssignStr(Vars.WEIGHT, string.Empty);
        i.AssignStr(Vars.STOCK, string.Empty);
        i.AssignStr(Vars.DOCK, string.Empty);
        i.AssignStr(Vars.BREAKAGE, string.Empty);
        i.AssignStr(Vars.BATCH, string.Empty);
        i.AssignNum(Vars.START_TIME, "#time");
        i.AssignStr(Vars.PRODUCT_DESCRIPTION, ProductName.Trim().ToLower());
        i.AssignStr(Vars.PRODUCT_NUMBER, ProductNumber.Trim().ToLower());
        i.AssignStr(Vars.UPC_NUMBER, UpcNumber.Trim().ToLower());
        i.AssignStr(Vars.QTY_UPPER, string.Empty);
        i.AssignStr(Vars.QTY_LOWER, string.Empty);
        i.AssignStr(Vars.MAX_QTY_ALLOWED_PER_PICK, string.Empty);
        i.AssignStr(Vars.TOTAL_PICKED, string.Empty);
        i.AssignStr(Vars.TOTAL_WEIGHT, string.Empty);

        // Customer information
        if (!string.IsNullOrEmpty(Customer))
            i.AssignStr(Vars.CUSTOMER, Customer);

        // Optional message
        if (!string.IsNullOrEmpty(Message))
        {
            i.SetCommands();
            i.Say(Message, true, true);

            i.AssignStr(Vars.CURRENT_AISLE, string.Empty);
            i.AssignStr(Vars.CURRENT_POSITION, string.Empty);
        }

        CheckLocation(i, device);

        ValidateProduct(i, device);

        i.Label($"{Labels.ASK_BATCH}_{Code}");

        if (AskBatch == true && (Batches?.Any() == true))
        {
            AskForBatch(i, device);
        }

        i.Label($"{Labels.RESET_PICK}_{Code}");

        if (OriginalServingQuantity.HasValue)
        {
            PickQuantity(i, device);
        }
        else
        {
            i.AssignStr(Vars.STATUS, "OK");
        }

        if (StockCounting == StockCountingMode.Always)
        {
            StockCountingDialog(i, device);
        }

        ConfirmLine(i, behavior);

        ChangeFormat(i, device);

        DeclareBreakage(i, device);

        ChangeToUnitsFormat(i, device);

        Exceptions(i);

        SkipAisle(i);

        SkipLine(i);

        PlaceInDock(i, behavior, device);

        BreakDialog(i, behavior, device);

        // End line
        i.Label($"{Labels.END}_{Code}");
    }

    private void CheckLocation(InstructionSet i, IDevice device)
    {
        i.SetCommands(command01: $"{Labels.EXCEPTION_LOCATION}_{Code}", command02: $"{Labels.DOCK}_{Code}", command03: $"{Labels.BREAK}_{Code}", command06: GetHowMuchMore(device), command11: $"${Vars.CUSTOMER}");

        if (CanSkipAisle == true)
        {
            i.SetCommand(14, $"{Labels.SKIP_AISLE}_{Code}", true);

            i.DoIf(Vars.CURRENT_AISLE, Aisle, Operation.NE, CompareType.Str, ifb =>
            {
                // New aisle: we assign new aisle to current aisle.
                {
                    i.AssignNum(Vars.SKIPPING_AISLE, false);
                    AssignAndSayCurrentLocation(i, device);
                }

                // Same aisle: we check if the aisle is skipped
                ifb.DoElseIf(Vars.SKIPPING_AISLE, true, Operation.EQ);
                {
                    i.DoIf(Vars.CURRENT_POSITION, Position, Operation.NE, CompareType.Str, ifPosition =>
                    {
                        // Skip the aisle and location if position is not equal.
                        {
                            i.GoTo($"{Labels.SKIP_SLOT}_{Code}");
                        }

                        // The position is the same than current position.
                        ifPosition.DoElse();
                        {
                            i.AssignNum(Vars.SKIPPING_AISLE, false);
                            i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_Aisle), Aisle), true, imageUrl: ValidateLocationImage?.AbsoluteUri);
                        }
                    });
                }
            });
        }
        else
        {
            // The operator cannot skip the aisle
            i.DoIf(Vars.CURRENT_AISLE, Aisle, Operation.NE, CompareType.Str, _ => AssignAndSayCurrentLocation(i, device));
        }

        i.AssignStr(Vars.WHEREAMI, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_Aisle), Aisle));

        // Validate slot
        if (!string.IsNullOrEmpty(Slot))
        {
            i.SetCommand(4, $"{Labels.SKIP_SLOT}_{Code}", true);

            i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_Slot), Slot), true, imageUrl: ValidateLocationImage?.AbsoluteUri);

            i.Concat(Vars.WHEREAMI, $", {device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_Slot), Slot)}");
        }

        i.SetCommands(command01: $"{Labels.EXCEPTION_CD}_{Code}", command02: $"{Labels.DOCK}_{Code}", command03: $"{Labels.BREAK}_{Code}", command04: $"{Labels.SKIP_SLOT}_{Code}", command06: GetHowMuchMore(device), command07: $"${Vars.WHEREAMI}", command08: $"${Vars.PRODUCT_DESCRIPTION}", command11: $"${Vars.CUSTOMER}", command15: $"${Vars.PRODUCT_NUMBER}", command16: $"${Vars.UPC_NUMBER}");

        if (CanSkipAisle == true)
            i.SetCommand(14, $"{Labels.SKIP_AISLE}_{Code}", true);

        if (string.IsNullOrEmpty(Position))
        {
            // Ask for check digit if position is null or empty and exists check digit.
            if (!string.IsNullOrEmpty(CD))
                i.GetDigits(Vars.DUMMY, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_CD)), mustEqual: CD, maxLength: CD?.Length.ToString(), wrongPrompt: device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Error_Incorrect)), imageUrl: ValidateLocationImage?.AbsoluteUri, barcodeEnabled: true);
        }
        else
        {
            // Prompt position. If check digit is provided it must be validated.
            if (!string.IsNullOrEmpty(CD))
                i.GetDigits(Vars.DUMMY, Position, mustEqual: CD, maxLength: CD?.Length.ToString(), wrongPrompt: device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Error_Incorrect)), imageUrl: ValidateLocationImage?.AbsoluteUri, barcodeEnabled: true);
            else
                i.Say(Position, requireReady: true, imageUrl: ValidateLocationImage?.AbsoluteUri);

            i.Concat(Vars.WHEREAMI, $", {Position}");
        }

        void AssignAndSayCurrentLocation(InstructionSet i, IDevice device)
        {
            i.AssignStr(Vars.CURRENT_AISLE, Aisle);
            i.AssignStr(Vars.CURRENT_POSITION, Position);
            i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_Aisle), Aisle), true, imageUrl: ValidateLocationImage?.AbsoluteUri);
        }
    }

    private void ValidateProduct(InstructionSet i, IDevice device)
    {
        if (ProductCDs?.Any() ?? false)
        {
            i.Say($"${Vars.PRODUCT_DESCRIPTION}");

            // Label to request control digit
            i.Label($"{Labels.VALIDATE_PRODUCT_CD}_{Code}");

            i.GetDigits(Vars.PRODUCT_CD, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_CD)), imageUrl: ValidateProductImage?.AbsoluteUri, barcodeEnabled: true);

            foreach (var cd in ProductCDs)
            {
                i.DoIf(Vars.PRODUCT_CD, cd, Operation.EQ, CompareType.Str, _ =>
                {
                    i.SetSendHostFlag(true, Vars.PRODUCT_CD);

                    // Skip out of loop
                    i.GoTo($"{Labels.ASK_BATCH}_{Code}");
                });
            }

            // Incorrect DUMMY message
            i.Concat(Vars.DUMMY, $"{device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Error_Incorrect))}, ", Vars.PRODUCT_CD);
            i.Say($"${Vars.DUMMY}");

            // Skip back to confirm the check digit again
            i.GoTo($"{Labels.VALIDATE_PRODUCT_CD}_{Code}");
        }
        else
        {
            if (SpeakProductName == true)
                i.Say($"${Vars.PRODUCT_DESCRIPTION}");
        }
    }

    private void AskForBatch(InstructionSet i, IDevice device)
    {
        i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_SelectBatch)));
        i.SetSendHostFlag(true, Vars.BATCH);

        // Proposes a list of batches to select.
        foreach (var batch in Batches!)
        {
            i.Ask(Vars.DUMMY, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_ConfirmBatch), batch.Length > 4 ? batch.Substring(batch.Length - 4) : batch), priorityPrompt: true, yesValue: "y", noValue: "n", cancelValue: "c", imageUrl: BatchesImage?.AbsoluteUri);

            i.DoIf(Vars.DUMMY, "y", Operation.EQ, CompareType.Str, ifBatch =>
            {
                {
                    i.AssignStr(Vars.BATCH, batch);
                    i.GoTo($"{Labels.RESET_PICK}_{Code}");
                }

                ifBatch.DoElseIf(Vars.DUMMY, "c", Operation.EQ, CompareType.Str);
                {
                    i.GoTo($"{Labels.RESET_PICK}_{Code}");
                }
            });
        }

        i.GoTo($"{Labels.ASK_BATCH}_{Code}");
    }

    private void PickQuantity(InstructionSet i, IDevice device)
    {
        // Initialize vars
        i.AssignStr(Vars.PROMPT, OriginalServingPrompt);
        i.AssignNum(Vars.QTY_REQUESTED, OriginalServingQuantity!.Value);
        i.AssignNum(Vars.QTY_REQUESTED_ORIG, OriginalServingQuantity.Value);
        i.AssignStr(Vars.TOTAL_PICKED, string.Empty);
        i.AssignStr(Vars.TOTAL_WEIGHT, string.Empty);

        // Assign original quantity tolerances.
        if (OriginalServingUpperTolerance.HasValue)
            i.AssignNum(Vars.QTY_UPPER, (int)Math.Round(OriginalServingQuantity.Value + (OriginalServingUpperTolerance.Value * OriginalServingQuantity.Value / 100)));
        if (OriginalServingLowerTolerance.HasValue)
            i.AssignNum(Vars.QTY_LOWER, (int)Math.Round(OriginalServingQuantity.Value - (OriginalServingLowerTolerance.Value * OriginalServingQuantity.Value / 100)));
        if (OriginalMaxQuantityAllowedPerPick.HasValue)
            i.AssignNum(Vars.MAX_QTY_ALLOWED_PER_PICK, OriginalMaxQuantityAllowedPerPick.Value);

        i.AssignStr(Vars.SERVING, OriginalServingCode);

        i.Label($"{Labels.PICK}_{Code}");

        // Removed the Dock and Take a break commands
        i.SetCommands(command01: $"{Labels.EXCEPTION}_{Code}", command04: $"{Labels.SKIP_SLOT}_{Code}", command06: GetHowMuchMore(device), command07: $"${Vars.WHEREAMI}", command08: $"${Vars.PRODUCT_DESCRIPTION}", command10: $"{Labels.BREAKAGE}_{Code}", command11: $"${Vars.CUSTOMER}", command12: $"{Labels.FORMAT}_{Code}", command13: $"{Labels.UNITS}_{Code}", command15: $"${Vars.PRODUCT_NUMBER}", command16: $"${Vars.UPC_NUMBER}");

        if (CountdownPick == true)
        {
            // Operator can confirm quantity partially until he says the requested quantity or zero.
            i.GetDigits(Vars.PICKED, $"${Vars.PROMPT}", helpMsg: HelpMsg, imageUrl: PickQuantityImage?.AbsoluteUri);

            // Checks if maximum quantity allowed per pick is set and if it is greater than quantity picked.
            i.DoIf(Vars.MAX_QTY_ALLOWED_PER_PICK, string.Empty, Operation.NE, CompareType.Str, _ =>
            {
                i.DoIf(Vars.PICKED, Vars.MAX_QTY_ALLOWED_PER_PICK, Operation.GT, CompareType.Num, _ =>
                {
                    i.Concat(Vars.DUMMY, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_MaxQuantityAllowed)), Vars.MAX_QTY_ALLOWED_PER_PICK);

                    i.Say($"${Vars.DUMMY}", priorityPrompt: true);

                    i.GoTo($"{Labels.PICK}_{Code}");
                });
            });

            // Checks if quantity es less than requested.
            i.DoIf(Vars.PICKED, Vars.QTY_REQUESTED, Operation.LT, CompareType.Num, ifPickedLessThanRequested =>
            {
                {
                    // Removed the Skip, Format and Unit commands
                    i.SetCommands(command01: $"{Labels.EXCEPTION}_{Code}", command06: GetHowMuchMore(device), command07: $"${Vars.WHEREAMI}", command08: $"${Vars.PRODUCT_DESCRIPTION}", command10: $"{Labels.BREAKAGE}_{Code}", command11: $"${Vars.CUSTOMER}", command15: $"${Vars.PRODUCT_NUMBER}", command16: $"${Vars.UPC_NUMBER}");

                    // If quantity is zero, the operator is asked for reason.
                    i.DoIf(Vars.PICKED, 0, Operation.EQ, ifPickedEqualZero =>
                    {
                        {
                            i.Add(Vars.TOTAL_PICKED, Vars.PICKED);

                            var asked = device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_RequiredQuantity));
                            var picked = device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_PickedQuantity));
                            var isShort = device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_IsShort));

                            i.Concat(Vars.PROMPT, asked, Vars.QTY_REQUESTED_ORIG);
                            i.Concat(Vars.PROMPT, picked, Vars.TOTAL_PICKED, isShort);

                            i.Ask(Vars.DUMMY, $"${Vars.PROMPT}", priorityPrompt: true);
                            i.DoIf(Vars.DUMMY, false, Operation.EQ, _ => i.GoTo($"{Labels.PICK}_{Code}"));

                            // Removed Exception and Breakage comands.
                            i.SetCommands(command06: GetHowMuchMore(device), command07: $"${Vars.WHEREAMI}", command08: $"${Vars.PRODUCT_DESCRIPTION}", command11: $"${Vars.CUSTOMER}", command15: $"${Vars.PRODUCT_NUMBER}", command16: $"${Vars.UPC_NUMBER}");

                            i.GetMenu(Vars.DUMMY, Menus.LOWER_QUANTITY, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_LowerQuantityReason)), confirmationPrompt: "?", wrongPrompt: device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Error_Incorrect)));
                            i.DoIf(Vars.DUMMY, 1, Operation.EQ, ifLowerQtyReason =>
                            {
                                i.AssignStr(Vars.STATUS, "Empty");
                                ifLowerQtyReason.DoElseIf(Vars.DUMMY, 2, Operation.EQ);
                                i.AssignStr(Vars.STATUS, "Breakage");
                                ifLowerQtyReason.DoElseIf(Vars.DUMMY, 3, Operation.EQ);
                                i.AssignStr(Vars.STATUS, "Completed");
                                ifLowerQtyReason.DoElseIf(Vars.DUMMY, 4, Operation.EQ);
                                i.AssignStr(Vars.STATUS, "EndPallet");
                                ifLowerQtyReason.DoElseIf(Vars.DUMMY, 5, Operation.EQ);
                                i.AssignStr(Vars.STATUS, "OK");
                                ifLowerQtyReason.DoElseIf(Vars.DUMMY, 6, Operation.EQ);
                                i.GoTo($"{Labels.RESET_PICK}_{Code}");
                            });
                        }

                        // The quantity is greater than zero and less than requested
                        ifPickedEqualZero.DoElse();
                        {
                            if (AskWeight == true)
                                AskWeightDialog(i, device);

                            // Pending quantity
                            i.Subtract(Vars.QTY_REQUESTED, Vars.PICKED);
                            i.Add(Vars.TOTAL_PICKED, Vars.PICKED);

                            i.Concat(Vars.PROMPT, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_PendingQuantity)), Vars.QTY_REQUESTED);

                            i.GoTo($"{Labels.PICK}_{Code}");
                        }
                    });

                    if (StockCounting == StockCountingMode.PartialPicked)
                    {
                        i.GetDigits(Vars.STOCK, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_QuantityInLocation)), confirmationPrompt: "?", imageUrl: StockCountingImage?.AbsoluteUri);

                        i.SetSendHostFlag(true, Vars.STOCK);
                    }
                }

                // When quantity is equal or greater than requested.
                ifPickedLessThanRequested.DoElse();
                {
                    // Checks upper tolerance
                    i.DoIf(Vars.QTY_UPPER, string.Empty, Operation.EQ, CompareType.Num, ifQtyUpper =>
                    {
                        {
                            i.DoIf(Vars.PICKED, Vars.QTY_REQUESTED, Operation.GT, CompareType.Num, _ =>
                            {
                                i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_QuantityGreater)));
                                i.GoTo($"{Labels.PICK}_{Code}");
                            });
                        }

                        // The upper tolerance has value
                        ifQtyUpper.DoElse();
                        {
                            i.Add(Vars.DUMMY, Vars.TOTAL_PICKED, Vars.PICKED);

                            // Check if the quantity picked is greater than the upper tolerance
                            i.DoIf(Vars.DUMMY, Vars.QTY_UPPER, Operation.GT, CompareType.Num, ifQtyM =>
                            {
                                {
                                    i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_QuantityGreater)));
                                    i.GoTo($"{Labels.PICK}_{Code}");
                                }

                                // Check if the quantity is greater than the quantity requested
                                ifQtyM.DoElseIf(Vars.DUMMY, Vars.QTY_REQUESTED_ORIG, Operation.GT, CompareType.Num);
                                {
                                    i.DoIf(Vars.DUMMY, Vars.QTY_UPPER, Operation.LE, CompareType.Num, _ =>
                                    {
                                        i.Concat(Vars.DUMMY, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_Confirm)));

                                        i.Ask(Vars.DUMMY, $"${Vars.DUMMY}", priorityPrompt: true);
                                        i.DoIf(Vars.DUMMY, false, Operation.EQ, _ =>
                                        {
                                            i.GoTo($"{Labels.PICK}_{Code}");
                                        });
                                    });
                                }
                            });
                        }
                    });

                    i.AssignStr(Vars.STATUS, "OK");

                    i.Subtract(Vars.QTY_REQUESTED, Vars.PICKED);
                    i.Add(Vars.TOTAL_PICKED, Vars.PICKED);

                    if (AskWeight == true)
                        AskWeightDialog(i, device);
                }
            });
        }
        else
        {
            // Picking quantity partially is not allowed. If quantity is less than requested, the operator is asked to confirm it.
            i.GetDigits(Vars.TOTAL_PICKED, $"${Vars.PROMPT}", minRange: $"${Vars.QTY_LOWER}", maxRange: $"${Vars.QTY_UPPER}", helpMsg: HelpMsg, imageUrl: PickQuantityImage?.AbsoluteUri);

            i.DoIf(Vars.TOTAL_PICKED, Vars.QTY_REQUESTED, Operation.LT, CompareType.Num, ifb =>
            {
                {
                    var asked = device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_RequiredQuantity));
                    var picked = device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_PickedQuantity));
                    var isShort = device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_IsShort));

                    i.Concat(Vars.PROMPT, asked, Vars.QTY_REQUESTED);
                    i.Concat(Vars.PROMPT, picked, Vars.TOTAL_PICKED, isShort);

                    i.Ask(Vars.DUMMY, $"${Vars.PROMPT}", priorityPrompt: true);
                    i.DoIf(Vars.DUMMY, false, Operation.EQ, _ => i.GoTo($"{Labels.RESET_PICK}_{Code}"));

                    // Removed the Exception, Skip slot, Breakage, Change unit of measure and Units  commands
                    i.SetCommands(command06: GetHowMuchMore(device), command07: $"${Vars.WHEREAMI}", command08: $"${Vars.PRODUCT_DESCRIPTION}", command11: $"${Vars.CUSTOMER}", command15: $"${Vars.PRODUCT_NUMBER}", command16: $"${Vars.UPC_NUMBER}");

                    i.GetMenu(Vars.DUMMY, Menus.LOWER_QUANTITY, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_LowerQuantityReason)), confirmationPrompt: "?", wrongPrompt: device.Translate(DialogResources.ResourceManager, nameof(DialogResources.Error_Incorrect)));
                    i.DoIf(Vars.DUMMY, 1, Operation.EQ, ifLowerQtyReason =>
                    {
                        i.AssignStr(Vars.STATUS, "Empty");
                        ifLowerQtyReason.DoElseIf(Vars.DUMMY, 2, Operation.EQ);
                        i.AssignStr(Vars.STATUS, "Breakage");
                        ifLowerQtyReason.DoElseIf(Vars.DUMMY, 3, Operation.EQ);
                        i.AssignStr(Vars.STATUS, "Completed");
                        ifLowerQtyReason.DoElseIf(Vars.DUMMY, 4, Operation.EQ);
                        i.AssignStr(Vars.STATUS, "EndPallet");
                        ifLowerQtyReason.DoElseIf(Vars.DUMMY, 5, Operation.EQ);
                        i.AssignStr(Vars.STATUS, "OK");
                        ifLowerQtyReason.DoElseIf(Vars.DUMMY, 6, Operation.EQ);
                        i.GoTo($"{Labels.RESET_PICK}_{Code}");
                    });

                    if (StockCounting == StockCountingMode.PartialPicked)
                    {
                        i.GetDigits(Vars.STOCK, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_QuantityInLocation)), confirmationPrompt: "?", imageUrl: StockCountingImage?.AbsoluteUri);

                        i.SetSendHostFlag(true, Vars.STOCK);
                    }
                }

                ifb.DoElse();
                {
                    i.AssignStr(Vars.STATUS, "OK");
                }
            });

            if (AskWeight == true)
            {
                i.Label($"{Labels.WEIGHT}_{Code}");

                i.GetFloat(Vars.TOTAL_WEIGHT, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_Weight)), confirmationPrompt: "?", imageUrl: PickQuantityImage?.AbsoluteUri);

                i.DoIf(Vars.TOTAL_WEIGHT, 0, Operation.NE, _ =>
                {
                    if (WeightMin.HasValue)
                    {
                        i.DoIf(Vars.TOTAL_WEIGHT, WeightMin.GetValueOrDefault().ToString(CultureInfo.InvariantCulture), Operation.LT, CompareType.Num, _ =>
                        {
                            i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_MinWeightAllowed)));
                            i.GoTo($"{Labels.WEIGHT}_{Code}");
                        });
                    }

                    if (WeightMax.HasValue)
                    {
                        i.DoIf(Vars.TOTAL_WEIGHT, WeightMax.GetValueOrDefault().ToString(CultureInfo.InvariantCulture), Operation.GT, CompareType.Num, _ =>
                        {
                            i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_MaxWeightAllowed)));
                            i.GoTo($"{Labels.WEIGHT}_{Code}");
                        });
                    }
                });

                i.SetSendHostFlag(true, Vars.TOTAL_WEIGHT);
            }
        }

        void AskWeightDialog(InstructionSet i, IDevice device)
        {
            i.Label($"{Labels.WEIGHT}_{Code}");

            i.GetFloat(Vars.WEIGHT, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_Weight)), confirmationPrompt: "?", imageUrl: PickQuantityImage?.AbsoluteUri);

            // Checks if weight per unit is within the accepted ranges.
            if (WeightMin.HasValue)
                CheckMinWeightPerUnit(i, device);

            if (WeightMax.HasValue)
                CheckMaxWeightPerUnit(i, device);

            i.Add(Vars.TOTAL_WEIGHT, Vars.WEIGHT);

            i.SetSendHostFlag(true, Vars.TOTAL_WEIGHT);
        }

        void CheckMinWeightPerUnit(InstructionSet i, IDevice device)
        {
            i.DoIf(Vars.SERVING, OriginalServingCode, Operation.EQ, CompareType.Str, ifServing =>
            {
                i.Multiply(Vars.DUMMY, Vars.PICKED, OriginalUnitsFormat.GetValueOrDefault());
                ifServing.DoElseIf(Vars.SERVING, AlternativeServingCode, Operation.EQ, CompareType.Str);
                i.Multiply(Vars.DUMMY, Vars.PICKED, AlternativeUnitsFormat.GetValueOrDefault());
                ifServing.DoElse();
                i.AssignNum(Vars.DUMMY, Vars.PICKED);
            });

            i.Multiply(Vars.DUMMY, WeightMin.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));

            i.DoIf(Vars.WEIGHT, Vars.DUMMY, Operation.LT, CompareType.Num, _ =>
            {
                i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_MinWeightAllowed)));
                i.GoTo($"{Labels.WEIGHT}_{Code}");
            });
        }

        void CheckMaxWeightPerUnit(InstructionSet i, IDevice device)
        {
            i.DoIf(Vars.SERVING, OriginalServingCode, Operation.EQ, CompareType.Str, ifServing =>
            {
                i.Multiply(Vars.DUMMY, Vars.PICKED, OriginalUnitsFormat.GetValueOrDefault());
                ifServing.DoElseIf(Vars.SERVING, AlternativeServingCode, Operation.EQ, CompareType.Str);
                i.Multiply(Vars.DUMMY, Vars.PICKED, AlternativeUnitsFormat.GetValueOrDefault());
                ifServing.DoElse();
                i.AssignNum(Vars.DUMMY, Vars.PICKED);
            });

            i.Multiply(Vars.DUMMY, WeightMax.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));

            i.DoIf(Vars.WEIGHT, Vars.DUMMY, Operation.GT, CompareType.Num, _ =>
            {
                i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_MaxWeightAllowed)));
                i.GoTo($"{Labels.WEIGHT}_{Code}");
            });
        }
    }

    private void StockCountingDialog(InstructionSet i, IDevice device)
    {
        i.SetCommands(command06: GetHowMuchMore(device), command07: $"${Vars.WHEREAMI}", command08: $"${Vars.PRODUCT_DESCRIPTION}", command11: $"${Vars.CUSTOMER}", command15: $"${Vars.PRODUCT_NUMBER}", command16: $"${Vars.UPC_NUMBER}");

        i.GetDigits(Vars.STOCK, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_QuantityInLocation)), confirmationPrompt: "?", imageUrl: StockCountingImage?.AbsoluteUri);

        i.SetSendHostFlag(true, Vars.STOCK);
    }

    private void ConfirmLine(InstructionSet i, IPickingBehavior behavior)
    {
        i.Label($"{Labels.CONFIRM}_{Code}");

        i.AssignNum(Vars.END_TIME, "#time");
        i.AssignNum(Vars.RESPONSETYPE, ResponseTypes.PickingLineResult);

        i.SetSendHostFlag(true, Vars.STATUS, Vars.START_TIME, Vars.END_TIME, Vars.TOTAL_PICKED, Vars.SERVING);

        // If the behavior is defined as a Batch type, the confirmation will be done asynchronously by ODR, on the other hand, if it is Continuous type, it will be made synchronously by LUT
        if (behavior is IPickingBatchBehavior)
        {
            i.GetVariablesOdr();
            i.SetSendHostFlag(false, Vars.STATUS, Vars.START_TIME, Vars.END_TIME, Vars.TOTAL_PICKED, Vars.SERVING, Vars.TOTAL_WEIGHT, Vars.STOCK, Vars.BREAKAGE);
        }
        else
        {
            i.SetSendHostFlag(true, Vars.RESPONSETYPE);
        }

        i.GoTo($"{Labels.END}_{Code}");
    }

    private void ChangeFormat(InstructionSet i, IDevice device)
    {
        // The operator can change the format of preparation to an available alternative format.
        i.Label($"{Labels.FORMAT}_{Code}");
        if (AlternativeServingQuantity.HasValue)
        {
            // Removed Format and Units commands.
            i.SetCommands(command01: $"{Labels.EXCEPTION}_{Code}", command06: GetHowMuchMore(device), command07: $"${Vars.WHEREAMI}", command08: $"${Vars.PRODUCT_DESCRIPTION}", command11: $"${Vars.CUSTOMER}", command15: $"${Vars.PRODUCT_NUMBER}", command16: $"${Vars.UPC_NUMBER}");
            i.AssignStr(Vars.PROMPT, AlternativeServingPrompt);
            i.AssignNum(Vars.QTY_REQUESTED, AlternativeServingQuantity.Value);
            i.AssignNum(Vars.QTY_REQUESTED_ORIG, AlternativeServingQuantity.Value);
            if (AlternativeServingUpperTolerance.HasValue)
                i.AssignNum(Vars.QTY_UPPER, (int)Math.Round(AlternativeServingQuantity.Value + (AlternativeServingUpperTolerance.Value * AlternativeServingQuantity.Value / 100)));
            else
                i.AssignStr(Vars.QTY_UPPER, string.Empty);

            if (AlternativeServingLowerTolerance.HasValue)
                i.AssignNum(Vars.QTY_LOWER, (int)Math.Round(AlternativeServingQuantity.Value - (AlternativeServingLowerTolerance.Value * AlternativeServingQuantity.Value / 100)));
            else
                i.AssignStr(Vars.QTY_LOWER, string.Empty);

            if (AlternativeMaxQuantityAllowedPerPick.HasValue)
                i.AssignNum(Vars.MAX_QTY_ALLOWED_PER_PICK, AlternativeMaxQuantityAllowedPerPick.Value);
            else
                i.AssignStr(Vars.MAX_QTY_ALLOWED_PER_PICK, string.Empty);

            i.AssignStr(Vars.SERVING, AlternativeServingCode);
        }
        else
        {
            i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_NotAllowed)));
        }

        i.GoTo($"{Labels.PICK}_{Code}");
    }

    private void DeclareBreakage(InstructionSet i, IDevice device)
    {
        // The operator can declare a breakage.
        i.Label($"{Labels.BREAKAGE}_{Code}");

        i.GetDigits(Vars.BREAKAGE, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_BreakageQuantity)), confirmationPrompt: "?");

        i.SetSendHostFlag(true, Vars.BREAKAGE);

        i.GoTo($"{Labels.PICK}_{Code}");
    }

    private void ChangeToUnitsFormat(InstructionSet i, IDevice device)
    {
        // The operator can make the picking with units format if it is available.
        i.Label($"{Labels.UNITS}_{Code}");
        if (UnitsQuantity.HasValue)
        {
            // Removed Format and Units commands.
            i.SetCommands(command01: $"{Labels.EXCEPTION}_{Code}", command06: GetHowMuchMore(device), command07: $"${Vars.WHEREAMI}", command08: $"${Vars.PRODUCT_DESCRIPTION}", command11: $"${Vars.CUSTOMER}", command15: $"${Vars.PRODUCT_NUMBER}", command16: $"${Vars.UPC_NUMBER}");
            i.AssignStr(Vars.PROMPT, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_Units), UnitsQuantity));
            i.AssignNum(Vars.QTY_REQUESTED, UnitsQuantity.Value);
            i.AssignNum(Vars.QTY_REQUESTED_ORIG, UnitsQuantity.Value);
            if (UnitsUpperTolerance.HasValue)
                i.AssignNum(Vars.QTY_UPPER, (int)Math.Round(UnitsQuantity.Value + (UnitsUpperTolerance.Value * UnitsQuantity.Value / 100)));
            else
                i.AssignStr(Vars.QTY_UPPER, string.Empty);

            if (UnitsLowerTolerance.HasValue)
                i.AssignNum(Vars.QTY_LOWER, (int)Math.Round(UnitsQuantity.Value - (UnitsLowerTolerance.Value * UnitsQuantity.Value / 100)));
            else
                i.AssignStr(Vars.QTY_LOWER, string.Empty);

            if (UnitsMaxQuantityAllowedPerPick.HasValue)
                i.AssignNum(Vars.MAX_QTY_ALLOWED_PER_PICK, UnitsMaxQuantityAllowedPerPick.Value);
            else
                i.AssignStr(Vars.MAX_QTY_ALLOWED_PER_PICK, string.Empty);

            i.AssignStr(Vars.SERVING, "UNITS");
        }
        else
        {
            i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_NotAllowed)));
        }

        i.GoTo($"{Labels.PICK}_{Code}");
    }

    private void Exceptions(InstructionSet i)
    {
        i.Label($"{Labels.EXCEPTION_LOCATION}_{Code}");
        {
            i.AssignStr(Vars.STATUS, "BadLocation");
            i.AssignStr(Vars.SERVING, string.Empty);
        }

        i.GoTo($"{Labels.CONFIRM}_{Code}");

        i.Label($"{Labels.EXCEPTION_CD}_{Code}");
        {
            i.AssignStr(Vars.STATUS, "NoCheck");
            i.AssignStr(Vars.SERVING, string.Empty);
        }

        i.GoTo($"{Labels.CONFIRM}_{Code}");

        i.Label($"{Labels.EXCEPTION}_{Code}");
        {
            i.AssignStr(Vars.STATUS, "Cancelled");
            i.AssignStr(Vars.SERVING, string.Empty);
        }

        i.GoTo($"{Labels.CONFIRM}_{Code}");
    }

    private void SkipAisle(InstructionSet i)
    {
        if (CanSkipAisle == true)
        {
            i.Label($"{Labels.SKIP_AISLE}_{Code}");

            i.AssignNum(Vars.SKIPPING_AISLE, 1); // We mark and continue to skip the current line
            i.GoTo($"{Labels.SKIP_SLOT}_{Code}");
        }
    }

    private void SkipLine(InstructionSet i)
    {
        i.Label($"{Labels.SKIP_SLOT}_{Code}");

        i.AssignStr(Vars.STATUS, "Postponed");
        i.AssignStr(Vars.SERVING, string.Empty);
        i.GoTo($"{Labels.CONFIRM}_{Code}");
    }

    private void PlaceInDock(InstructionSet i, IPickingBehavior behavior, IDevice device)
    {
        i.Label($"{Labels.DOCK}_{Code}");

        if (behavior is IPickingContinuousBehavior)
        {
            i.AssignNum(Vars.RESPONSETYPE, ResponseTypes.PickingLineResult);
            i.AssignStr(Vars.STATUS, "EndPallet");
            i.AssignNum(Vars.END_TIME, "#time");
            i.SetSendHostFlag(true, Vars.RESPONSETYPE, Vars.STATUS, Vars.START_TIME, Vars.END_TIME);
            i.GoTo($"{Labels.END}_{Code}");
        }
        else
        {
            i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PlaceInDock_PlaceInDock)));

            i.SetCommands(command09: $"{Labels.DOCK_END}_{Code}", command11: $"${Vars.CUSTOMER}");
            i.GetDigits(Vars.LABELS, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PrintLabels_Labels)), maxLength: "2");
            i.AssignNum(Vars.RESPONSETYPE, ResponseTypes.PrintLabelsBatchResult);

            i.Label($"{Labels.PRINT_LABELS}_{Code}");
            {
                i.SetSendHostFlag(true, Vars.LABELS);
                i.GetVariablesOdr();
                i.SetSendHostFlag(false, Vars.LABELS);

                i.SetCommands(command01: $"{Labels.DOCK_END}_{Code}", command11: $"${Vars.CUSTOMER}");

                i.Ask(Vars.DUMMY, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PrintLabels_ConfirmResult)), true, false, priorityPrompt: true);

                i.DoIf(Vars.DUMMY, false, Operation.EQ, _ =>
                {
                    i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PrintLabels_Retrying)));
                    i.GoTo($"{Labels.PRINT_LABELS}_{Code}");
                });

                i.SetCommands();
                i.GetDigits(Vars.DOCK, device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PlaceInDock_Location)), maxLength: "2", imageUrl: PlaceInDockImage?.AbsoluteUri);
                i.AssignNum(Vars.RESPONSETYPE, ResponseTypes.PickingLineResult);
                i.AssignStr(Vars.STATUS, "EndPallet");
                i.AssignNum(Vars.END_TIME, "#time");

                i.SetSendHostFlag(true, Vars.STATUS, Vars.DOCK, Vars.START_TIME, Vars.END_TIME);
                i.GetVariablesOdr();
                i.SetSendHostFlag(false, Vars.STATUS, Vars.DOCK, Vars.START_TIME, Vars.END_TIME);

                i.Say(device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PlaceInDock_PalletPlaced)));

                i.GoTo($"{Labels.DOCK_END}_{Code}");
            }

            // End of place in dock
            i.Label($"{Labels.DOCK_END}_{Code}");
            i.AssignStr(Vars.CURRENT_AISLE, string.Empty);

            i.GoTo($"{Labels.START}_{Code}"); // Back to the beginning of the line
        }
    }

    private void BreakDialog(InstructionSet i, IPickingBehavior behavior, IDevice device)
    {
        // Ask for break reason.
        i.Label($"{Labels.BREAK}_{Code}");
        {
            i.AssignStr(Vars.CURRENT_AISLE, string.Empty);
            i.AssignStr(Vars.CURRENT_POSITION, string.Empty);

            // If the user says 'cancel' it returns at the beginning
            i.SetCommands(command09: $"{Labels.START}_{Code}");

            SharedDialogs.Break(i, behavior.Settings.BreakOptions?.Any() ?? false, device);
        }

        i.GoTo($"{Labels.START}_{Code}");
    }

    private string GetHowMuchMore(IDevice device) => HowMuchMore.HasValue ? device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_HowMuchMore), HowMuchMore) : device.Translate(DialogResources.ResourceManager, nameof(DialogResources.PickingLine_MissingData));
}