// <copyright file="Behavior.cs" company="Honeywell | Safety and Productivity Solutions">
// Copyright (c) Honeywell, 2023. All rights reserved.
// </copyright>

using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Models.Interfaces;
using Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.Behaviors;
#if !NETFRAMEWORK
using Honeywell.GWS.Connector.Library.Workflows.Picking.Services.Interfaces;
#endif
using Honeywell.GWS.Connector.SDK;
using Honeywell.GWS.Connector.SDK.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Modules.InMemory.Continuous;

/// <summary>
/// Base implementation for Memory Continuous ConnectorBehavior.
/// </summary>
public class Behavior : PickingBehaviorBase, IPickingContinuousBehavior
{
    private readonly Queue<IGetWorkOrderItem> _assignments = new();
    private readonly Queue<IGetWorkOrderItem> _emptyAssignments = new();

    private int currentTest = 1;

#if NETFRAMEWORK
    /// <summary>
    /// Initializes a new instance of the <see cref="Behavior"/> class.
    /// </summary>
    /// <param name="settings">Settings instance.</param>
    public Behavior(PickingBehaviorSettings settings)
        : base(settings)
    {
    }
#else
    /// <summary>
    /// Initializes a new instance of the <see cref="Behavior"/> class.
    /// </summary>
    /// <param name="settings">Settings instance.</param>
    /// <param name="serverLog">ServerLog service instance.</param>
    public Behavior(PickingBehaviorSettings settings, IServerLog serverLog)
        : base(settings, serverLog)
    {
    }
#endif

    /// <inheritdoc/>
    public override ValueTask<ConnectResult> ConnectAsync(string operatorName, IDevice device)
    {
        switch (currentTest)
        {
            // Test 1: Picking line
            case 1:
                new List<IGetWorkOrderItem>(new IGetWorkOrderItem[]
                {
                    new BeginPickingOrder("1", "Order header message") { OrderNumber = "001", Customer = "Customer", ContainerType = "container" },
                    new PickingLine("1.1", "First line")
                    {
                        Aisle = "1",
                        Slot = "A",
                        Position = "01",
                        CD = "123",
                        ProductName = "Test 1.1",
                        ProductNumber = "ITEM 1.1",
                        UpcNumber = "UPC 1.1",
                        OriginalServingCode = "BOXES",
                        OriginalServingPrompt = "3 boxes",
                        OriginalServingQuantity = 3,
                        OriginalServingUpperTolerance = 50,
                        OriginalServingLowerTolerance = 50,
                        SpeakProductName = true,
                        HowMuchMore = 2,
                    },
                    new PickingLine("1.2")
                    {
                        Aisle = "1",
                        Slot = "A",
                        Position = "02",
                        CD = "123",
                        ProductName = "Test 1.2",
                        ProductNumber = "ITEM 1.2",
                        UpcNumber = "UPC 1.2",
                        OriginalServingCode = "BOXES",
                        OriginalServingPrompt = "3 boxes",
                        OriginalServingQuantity = 3,
                        OriginalServingUpperTolerance = 25,
                        OriginalServingLowerTolerance = 100,
                    },
                    new PickingLine("1.3")
                    {
                        Aisle = "1",
                        Slot = "A",
                        Position = "03",
                        CD = "123",
                        ProductName = "Test 1.3",
                        ProductNumber = "ITEM 1.3",
                        UpcNumber = "UPC 1.3",
                        OriginalServingCode = "BOXES",
                        OriginalServingPrompt = "3 boxes",
                        OriginalServingQuantity = 3,
                        OriginalServingUpperTolerance = 25,
                        OriginalServingLowerTolerance = 100,
                        StockCounting = StockCountingMode.PartialPicked,
                    },
                    new PickingLine("1.4")
                    {
                        Aisle = "1",
                        Slot = "B",
                        Position = "01",
                        CD = "123",
                        ProductName = "Test 1.4",
                        ProductNumber = "ITEM 1.4",
                        UpcNumber = "UPC 1.4",
                        OriginalServingCode = "BOXES",
                        OriginalServingPrompt = "3 boxes",
                        OriginalServingQuantity = 3,
                        OriginalServingUpperTolerance = 25,
                        OriginalServingLowerTolerance = 100,
                    },
                    new PickingLine("1.5")
                    {
                        Aisle = "2",
                        Slot = "A",
                        Position = "01",
                        CD = "123",
                        ProductName = "Test 1.5",
                        ProductNumber = "ITEM 1.5",
                        UpcNumber = "UPC 1.5",
                        OriginalServingCode = "BOXES",
                        OriginalServingPrompt = "4 boxes",
                        OriginalServingQuantity = 4,
                        OriginalServingUpperTolerance = 25,
                        OriginalServingLowerTolerance = 100,
                        AskWeight = true,
                        WeightMin = 10.0M,
                        WeightMax = 25.0M,
                    },
                    new PickingLine("1.6")
                    {
                        Aisle = "3",
                        Slot = "A",
                        Position = "01",
                        CD = "123",
                        ProductName = "Test 1.6",
                        ProductNumber = "ITEM 1.6",
                        UpcNumber = "UPC 1.6",
                        OriginalServingCode = "BOXES",
                        OriginalServingPrompt = "4 boxes",
                        OriginalServingQuantity = 4,
                        OriginalServingUpperTolerance = 25,
                        OriginalServingLowerTolerance = 100,
                        AlternativeServingCode = "packages",
                        AlternativeServingQuantity = 49,
                        AlternativeServingUpperTolerance = 20,
                        AlternativeServingLowerTolerance = 100,
                        AlternativeServingPrompt = "49 packages",
                        UnitsQuantity = 240,
                        UnitsLowerTolerance = 100,
                        UnitsUpperTolerance = 0,
                    },
                    new PickingLine("1.7")
                    {
                        Aisle = "3",
                        Slot = "A",
                        Position = "03",
                        CD = "123",
                        ProductName = "Test 1.7",
                        ProductNumber = "ITEM 1.7",
                        UpcNumber = "UPC 1.7",
                        OriginalServingCode = "BOXES",
                        OriginalServingPrompt = "4 boxes",
                        OriginalServingQuantity = 4,
                        OriginalServingUpperTolerance = 25,
                        OriginalServingLowerTolerance = 100,
                        AlternativeServingCode = "packages",
                        AlternativeServingQuantity = 49,
                        AlternativeServingUpperTolerance = 20,
                        AlternativeServingLowerTolerance = 100,
                        AlternativeServingPrompt = "49 packages",
                        UnitsQuantity = 240,
                        UnitsLowerTolerance = 100,
                        UnitsUpperTolerance = 0,
                    },
                    new PickingLine("1.8")
                    {
                        Aisle = "3",
                        Slot = "A",
                        Position = "04",
                        CD = "123",
                        ProductName = "Test 1.8",
                        ProductNumber = "ITEM 1.8",
                        UpcNumber = "UPC 1.8",
                        OriginalServingCode = "BOXES",
                        OriginalServingPrompt = "4 boxes",
                        OriginalServingQuantity = 4,
                        OriginalServingUpperTolerance = 25,
                        OriginalServingLowerTolerance = 100,
                        AlternativeServingCode = "packages",
                        AlternativeServingQuantity = 49,
                        AlternativeServingUpperTolerance = 20,
                        AlternativeServingLowerTolerance = 100,
                        AlternativeServingPrompt = "49 packages",
                        UnitsQuantity = 240,
                        UnitsLowerTolerance = 100,
                        UnitsUpperTolerance = 0,
                    },
                    new PickingLine("1.9")
                    {
                        Aisle = "3",
                        Slot = "A",
                        Position = "05",
                        CD = "123",
                        ProductName = "Test 1.9",
                        ProductNumber = "ITEM 1.9",
                        UpcNumber = "UPC 1.9",
                        OriginalServingCode = "BOXES",
                        OriginalServingPrompt = "4 boxes",
                        OriginalServingQuantity = 4,
                    },
                    new PrintLabels("1.9.0") { DefaultPrinter = 1 },
                    new ValidatePrinting("1.9.1") { ValidationCodes = new List<string> { "123", "456" }, VoiceLength = 3 },
                    new PlaceInDock("1.9.2") { },
                    new PrintLabels("1.9.3") { },
                    new PrintLabels("1.9.3.1") { Printers = new List<int> { 1, 2, 3 } },
                    new ValidatePrinting("1.9.4") { },
                    new ValidatePrinting("1.9.5") { },
                    new PickingLine("1.9.6")
                    {
                        Aisle = "3",
                        Slot = "A",
                        Position = "05",
                        CD = "123",
                        ProductName = "Test 1.9",
                        ProductNumber = "ITEM 1.9",
                        UpcNumber = "UPC 1.9",
                        OriginalServingCode = "BOXES",
                        OriginalServingPrompt = "4 boxes",
                        OriginalServingQuantity = 4,
                    },
                    new PlaceInDock("1.10") { Dock = "1" },
                    new AskQuestion("1.11", "This is a question"),
                    new AskQuestion("1.12", "This is another question"),
                    new BeginPickingOrder("2", null) { OrderNumber = "001", Customer = "Customer", ContainerType = "container" },
                }).ForEach(o => _assignments.Enqueue(o));
                currentTest = 2;
                break;

            // Test 2: Picking line - Ask batch
            case 2:
                new List<IGetWorkOrderItem>(new IGetWorkOrderItem[]
                {
                new BeginPickingOrder("2", null) { OrderNumber = "001", Customer = "Customer", ContainerType = "container" },
                new PickingLine("2.1")
                {
                    Aisle = "1",
                    Slot = "A",
                    Position = "01",
                    CD = "123",
                    AskBatch = true,
                    Batches = new List<string> { "1", "2" },
                    ProductName = "Test 2.1",
                    ProductNumber = "ITEM 2.1",
                    UpcNumber = "UPC 2.1",
                    OriginalServingCode = "BOXES",
                    OriginalServingPrompt = "3 boxes",
                    OriginalServingQuantity = 3,
                    OriginalServingUpperTolerance = 25,
                    OriginalServingLowerTolerance = 100,
                },
                new PickingLine("2.2")
                {
                    Aisle = "1",
                    Slot = "A",
                    Position = "02",
                    CD = "123",
                    AskBatch = true,
                    Batches = new List<string> { "1", "2" },
                    ProductName = "Test 2.2",
                    ProductNumber = "ITEM 2.2",
                    UpcNumber = "UPC 2.2",
                    OriginalServingCode = "BOXES",
                    OriginalServingPrompt = "3 boxes",
                    OriginalServingQuantity = 3,
                    OriginalServingUpperTolerance = 25,
                    OriginalServingLowerTolerance = 100,
                },
                new BeginPickingOrder("3", null) { OrderNumber = "001", Customer = "Customer", ContainerType = "container" },
                }).ForEach(o => _assignments.Enqueue(o));
                currentTest = 3;
                break;

            // Test 3: Picking line - Ask if the line contains package
            case 3:
                new List<IGetWorkOrderItem>(new IGetWorkOrderItem[]
                    {
                    new BeginPickingOrder("3", null) { OrderNumber = "001", Customer = "Customer", ContainerType = "container" },
                    new PickingLine("3.1")
                    {
                        Aisle = "1",
                        Slot = "A",
                        Position = "01",
                        CD = "123",
                        ProductName = "Test 3.1",
                        ProductNumber = "ITEM 3.1",
                        UpcNumber = "UPC 3.1",
                        OriginalServingCode = "BOXES",
                        OriginalServingPrompt = "3 boxes",
                        OriginalServingQuantity = 3,
                        OriginalServingUpperTolerance = 25,
                        OriginalServingLowerTolerance = 100,
                    },
                    new PickingLine("3.2")
                    {
                        Aisle = "1",
                        Slot = "A",
                        Position = "02",
                        CD = "123",
                        ProductName = "Test 3.2",
                        ProductNumber = "ITEM 3.2",
                        UpcNumber = "UPC 3.2",
                        OriginalServingCode = "BOXES",
                        OriginalServingPrompt = "3 boxes",
                        OriginalServingQuantity = 3,
                        OriginalServingUpperTolerance = 25,
                        OriginalServingLowerTolerance = 100,
                    },
                    new BeginPickingOrder("4", null) { OrderNumber = "001", Customer = "Customer", ContainerType = "container" },
                    }).ForEach(o => _assignments.Enqueue(o));
                currentTest = 4;
                break;

            // Test 4: Picking line - Validate location
            case 4:
                new List<IGetWorkOrderItem>(new IGetWorkOrderItem[]
                {
                new BeginPickingOrder("4", null) { OrderNumber = "001", Customer = "Customer", ContainerType = "container" },
                new PickingLine("4.1", "Test 4.1: Slot not supplied")
                {
                    Aisle = "1",
                    Position = "01",
                    ProductName = "Test 4.1",
                    ProductNumber = "ITEM 4.1",
                    UpcNumber = "UPC 4.1",
                    CD = "123",
                    OriginalServingCode = "BOXES",
                    OriginalServingPrompt = "3 boxes",
                    OriginalServingQuantity = 3,
                    OriginalServingUpperTolerance = 25,
                    OriginalServingLowerTolerance = 100,
                },
                new PickingLine("4.2", "Test 4.2: Control digit not supplied")
                {
                    Aisle = "1",
                    Slot = "A",
                    Position = "02",
                    ProductName = "Test 4.2",
                    ProductNumber = "ITEM 4.2",
                    UpcNumber = "UPC 4.2",
                    OriginalServingCode = "BOXES",
                    OriginalServingPrompt = "3 boxes",
                    OriginalServingQuantity = 3,
                    OriginalServingUpperTolerance = 25,
                    OriginalServingLowerTolerance = 100,
                },
                new PickingLine("4.3", "Test 4.3: Position not supplied")
                {
                    Aisle = "1",
                    CD = "123",
                    ProductName = "Test 4.3",
                    ProductNumber = "ITEM 4.3",
                    UpcNumber = "UPC 4.3",
                    OriginalServingCode = "BOXES",
                    OriginalServingPrompt = "3 boxes",
                    OriginalServingQuantity = 3,
                    OriginalServingUpperTolerance = 25,
                    OriginalServingLowerTolerance = 100,
                },
                new PickingLine("4.4", "Test 4.4: Position and Control Digit not supplied")
                {
                    Aisle = "1",
                    Slot = "A",
                    ProductName = "Test 4.4",
                    ProductNumber = "ITEM 4.4",
                    UpcNumber = "UPC 4.4",
                    OriginalServingCode = "BOXES",
                    OriginalServingPrompt = "3 boxes",
                    OriginalServingQuantity = 3,
                    OriginalServingUpperTolerance = 25,
                    OriginalServingLowerTolerance = 100,
                },
                new BeginPickingOrder("5", null) { OrderNumber = "001", Customer = "Customer", ContainerType = "container" },
                }).ForEach(o => _assignments.Enqueue(o));
                currentTest = 5;
                break;

            // Test 5: Picking line - Validate product
            case 5:
                new List<IGetWorkOrderItem>(new IGetWorkOrderItem[]
                {
                new BeginPickingOrder("5", null) { OrderNumber = "001", Customer = "Customer", ContainerType = "container" },
                new PickingLine("5.1", "Test 5.1: List of codes to be validated")
                {
                    Aisle = "1",
                    Position = "01",
                    Slot = "A",
                    ProductName = "Test 5.1",
                    ProductNumber = "ITEM 5.1",
                    UpcNumber = "UPC 5.1",
                    CD = "123",
                    ProductCDs = new List<string> { "123", "456" },
                    OriginalServingCode = "BOXES",
                    OriginalServingPrompt = "3 boxes",
                    OriginalServingQuantity = 3,
                    OriginalServingUpperTolerance = 25,
                    OriginalServingLowerTolerance = 100,
                },
                new BeginPickingOrder("6", null) { OrderNumber = "001", Customer = "Customer", ContainerType = "container" },
                }).ForEach(o => _assignments.Enqueue(o));
                currentTest = 6;
                break;

            // Test 6: Picking quantity partially
            case 6:
                new List<IGetWorkOrderItem>(new IGetWorkOrderItem[]
                {
                new BeginPickingOrder("6", null) { OrderNumber = "001", Customer = "Customer", ContainerType = "container" },
                new PickingLine("6.1")
                {
                    Aisle = "1",
                    Slot = "A",
                    Position = "01",
                    CD = "123",
                    ProductName = "Test 6.1",
                    ProductNumber = "ITEM 6.1",
                    UpcNumber = "UPC 6.1",
                    OriginalServingCode = "BOXES",
                    OriginalServingPrompt = "3 boxes",
                    OriginalServingQuantity = 3,
                    OriginalServingUpperTolerance = 25,
                    OriginalServingLowerTolerance = 100,
                    CountdownPick = true,
                },
                new PickingLine("6.2")
                {
                    Aisle = "1",
                    Slot = "A",
                    Position = "02",
                    CD = "123",
                    ProductName = "Test 6.2",
                    ProductNumber = "ITEM 6.2",
                    UpcNumber = "UPC 6.2",
                    OriginalServingCode = "BOXES",
                    OriginalServingPrompt = "3 boxes",
                    OriginalServingQuantity = 3,
                    OriginalServingUpperTolerance = 25,
                    OriginalServingLowerTolerance = 100,
                    CountdownPick = true,
                    StockCounting = StockCountingMode.PartialPicked,
                },
                new PickingLine("6.3")
                {
                    Aisle = "1",
                    Slot = "A",
                    Position = "03",
                    CD = "123",
                    ProductName = "Test 6.3",
                    ProductNumber = "ITEM 6.3",
                    UpcNumber = "UPC 6.3",
                    OriginalServingCode = "BOXES",
                    OriginalServingPrompt = "3 boxes",
                    OriginalServingQuantity = 3,
                    OriginalServingUpperTolerance = 25,
                    OriginalServingLowerTolerance = 100,
                    CountdownPick = true,
                    OriginalMaxQuantityAllowedPerPick = 1,
                },
                new PickingLine("6.4")
                {
                    Aisle = "1",
                    Slot = "A",
                    Position = "04",
                    CD = "123",
                    ProductName = "Test 6.4",
                    ProductNumber = "ITEM 6.4",
                    UpcNumber = "UPC 6.4",
                    OriginalServingCode = "BOXES",
                    OriginalServingPrompt = "3 boxes",
                    OriginalServingQuantity = 3,
                    OriginalServingUpperTolerance = 100,
                    OriginalServingLowerTolerance = 100,
                    CountdownPick = true,
                },
                new PickingLine("6.5")
                {
                    Aisle = "1",
                    Slot = "A",
                    Position = "05",
                    CD = "123",
                    ProductName = "Test 6.5",
                    ProductNumber = "ITEM 6.5",
                    UpcNumber = "UPC 6.5",
                    OriginalServingCode = "BOXES",
                    OriginalServingPrompt = "3 boxes",
                    OriginalServingQuantity = 3,
                    OriginalServingUpperTolerance = 100,
                    OriginalServingLowerTolerance = 100,
                    CountdownPick = true,
                },
                new PickingLine("6.6")
                {
                    Aisle = "1",
                    Slot = "A",
                    Position = "06",
                    CD = "123",
                    ProductName = "Test 6.6",
                    ProductNumber = "ITEM 6.6",
                    UpcNumber = "UPC 6.6",
                    OriginalServingCode = "BOXES",
                    OriginalServingPrompt = "3 boxes",
                    OriginalServingQuantity = 3,
                    OriginalServingUpperTolerance = 100,
                    OriginalServingLowerTolerance = 100,
                    CountdownPick = true,
                    AskWeight = true,
                    WeightMax = 1.5M,
                    WeightMin = 0.5M,
                    OriginalUnitsFormat = 10,
                },
                new PickingLine("6.7")
                {
                    Aisle = "1",
                    Slot = "A",
                    Position = "07",
                    CD = "123",
                    ProductName = "Test 6.7",
                    ProductNumber = "ITEM 6.7",
                    UpcNumber = "UPC 6.7",
                    OriginalServingCode = "BOXES",
                    OriginalServingPrompt = "3 boxes",
                    OriginalServingQuantity = 3,
                    OriginalServingUpperTolerance = 100,
                    OriginalServingLowerTolerance = 100,
                    CountdownPick = true,
                    AskWeight = true,
                    WeightMax = 1.5M,
                    WeightMin = 0.5M,
                    OriginalUnitsFormat = 10,
                },
                new BeginPickingOrder("7", null) { OrderNumber = "001", ContainerType = "container", ContainersCount = 3 },
                }).ForEach(o => _assignments.Enqueue(o));
                currentTest = 7;
                break;

            // Test 7: Multi order picking line
            case 7:
                new List<IGetWorkOrderItem>(new IGetWorkOrderItem[]
                {
                new BeginPickingOrder("7", null) { OrderNumber = "001", ContainerType = "container", ContainersCount = 3 },
                new PickingLine("7.1")
                {
                    Customer = "Customer 1",
                    Aisle = "1",
                    Slot = "A",
                    Position = "01",
                    CD = "123",
                    ProductName = "Test  7.1",
                    ProductNumber = "ITEM 7.1",
                    UpcNumber = "UPC 7.1",
                    OriginalServingCode = "BOXES",
                    OriginalServingPrompt = "3 boxes in A",
                    OriginalServingQuantity = 3,
                    OriginalServingUpperTolerance = 25,
                    OriginalServingLowerTolerance = 100,
                },
                new PickingLine("7.2")
                {
                    Customer = "Customer 2",
                    Aisle = "1",
                    Slot = "A",
                    Position = "02",
                    CD = "123",
                    ProductName = "Test 7.2",
                    ProductNumber = "ITEM 7.2",
                    UpcNumber = "UPC 7.2",
                    OriginalServingCode = "BOXES",
                    OriginalServingPrompt = "3 boxes in B",
                    OriginalServingQuantity = 3,
                    OriginalServingUpperTolerance = 25,
                    OriginalServingLowerTolerance = 100,
                },
                new PickingLine("7.3")
                {
                    Customer = "Customer 3",
                    Aisle = "1",
                    Slot = "A",
                    Position = "02",
                    CD = "123",
                    ProductName = "Test 7.3",
                    ProductNumber = "ITEM 7.3",
                    UpcNumber = "UPC 7.3",
                    OriginalServingCode = "BOXES",
                    OriginalServingPrompt = "3 boxes in C",
                    OriginalServingQuantity = 3,
                    OriginalServingUpperTolerance = 25,
                    OriginalServingLowerTolerance = 100,
                },
                new PickingLine("7.4")
                {
                    Customer = "Customer 2",
                    Aisle = "1",
                    Slot = "B",
                    Position = "01",
                    CD = "123",
                    ProductName = "Test 7.4",
                    ProductNumber = "ITEM 7.4",
                    UpcNumber = "UPC 7.4",
                    OriginalServingCode = "BOXES",
                    OriginalServingPrompt = "3 boxes in B",
                    OriginalServingQuantity = 3,
                    OriginalServingUpperTolerance = 25,
                    OriginalServingLowerTolerance = 100,
                },
                new PrintLabels("7.5.1") { DefaultPrinter = 1 },
                new ValidatePrinting("7.5.2") { ValidationCodes = new List<string> { "123", "456" }, VoiceLength = 3 },
                new PlaceInDock("7.5", "Container B to dock") { Dock = "2", CD = "123" },
                new PickingLine("7.6")
                {
                    Customer = "Customer 3",
                    Aisle = "2",
                    Slot = "A",
                    Position = "01",
                    CD = "123",
                    ProductName = "Test 7.6",
                    ProductNumber = "ITEM 7.6",
                    UpcNumber = "UPC 7.6",
                    OriginalServingCode = "BOXES",
                    OriginalServingPrompt = "4 boxes in C",
                    OriginalServingQuantity = 4,
                    OriginalServingUpperTolerance = 25,
                    OriginalServingLowerTolerance = 100,
                },
                new PlaceInDock("7.7", "Container C to dock") { Dock = "3" },
                new PickingLine("7.8")
                {
                    Customer = "Customer 1",
                    Aisle = "3",
                    Slot = "A",
                    Position = "01",
                    CD = "123",
                    ProductName = "Test 7.8",
                    ProductNumber = "ITEM 7.8",
                    UpcNumber = "UPC 7.8",
                    OriginalServingCode = "BOXES",
                    OriginalServingPrompt = "4 boxes in A",
                    OriginalServingQuantity = 4,
                    OriginalServingUpperTolerance = 25,
                    OriginalServingLowerTolerance = 100,
                    AlternativeServingCode = "packages",
                    AlternativeServingQuantity = 49,
                    AlternativeServingUpperTolerance = 20,
                    AlternativeServingLowerTolerance = 100,
                    AlternativeServingPrompt = "49 packages in A",
                    UnitsQuantity = 240,
                    UnitsLowerTolerance = 100,
                    UnitsUpperTolerance = 0,
                },
                new PlaceInDock("7.9", "Container A to dock") { Dock = "1" },
                new BeginPickingOrder("8", "Order header message") { OrderNumber = "001" },
                }).ForEach(o => _assignments.Enqueue(o));
                currentTest = 8;
                break;

            // Test 8: Stock counting
            case 8:
                new List<IGetWorkOrderItem>(new IGetWorkOrderItem[]
                {
                new BeginPickingOrder("8", "Order header message") { OrderNumber = "001" },
                new PickingLine("8.1", "First line stock counting")
                {
                    Aisle = "1",
                    Slot = "A",
                    Position = "01",
                    CD = "123",
                    ProductName = "Test  8.1",
                    ProductNumber = "ITEM 8.1",
                    UpcNumber = "UPC 8.1",
                    StockCounting = StockCountingMode.Always,
                },
                new PickingLine("8.2")
                {
                    Aisle = "1",
                    Slot = "A",
                    Position = "02",
                    CD = "123",
                    ProductName = "Test 8.2",
                    ProductNumber = "ITEM 8.2",
                    UpcNumber = "UPC 8.2",
                    StockCounting = StockCountingMode.Always,
                },
                new PickingLine("8.3")
                {
                    Aisle = "1",
                    Slot = "A",
                    Position = "03",
                    CD = "123",
                    ProductName = "Test 8.3",
                    ProductNumber = "ITEM 8.3",
                    UpcNumber = "UPC 8.3",
                    StockCounting = StockCountingMode.Always,
                },
                new PickingLine("8.4")
                {
                    Aisle = "1",
                    Slot = "B",
                    Position = "01",
                    CD = "123",
                    ProductName = "Test 8.4",
                    ProductNumber = "ITEM 8.4",
                    UpcNumber = "UPC 8.4",
                    StockCounting = StockCountingMode.Always,
                },
                }).ForEach(o => _assignments.Enqueue(o));

                _emptyAssignments.Enqueue(new EmptyWork("Empty work. Used to inform a message and get new work when operator say VCONFIRM"));

                currentTest = 1;
                break;
        }

        return new ValueTask<ConnectResult>(new ConnectResult(true, null, null));
    }

    /// <inheritdoc/>
    public override ValueTask<DisconnectResult> DisconnectAsync(string operatorName, string device, bool force)
    {
        _assignments.Clear();

        return new ValueTask<DisconnectResult>(new DisconnectResult(true, null));
    }

    /// <inheritdoc/>
    public override Task BeginBreakAsync(string operatorName, string device, BeginBreak res)
    {
        Log($"BeginBreak: {res.Code} - {res.Reason}", LogLevel.Information);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public override Task EndBreakAsync(string operatorName, string device)
    {
        Log($"EndBreak: {operatorName}", LogLevel.Information);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task<IGetWorkOrderItem?> GetWorkOrderAsync(string operatorName, string device)
    {
        if (_assignments.Any())
            return Task.FromResult<IGetWorkOrderItem?>(_assignments.Dequeue());

        if (_emptyAssignments.Any())
            return Task.FromResult<IGetWorkOrderItem?>(_emptyAssignments.Dequeue());

        return Task.FromResult<IGetWorkOrderItem?>(null);
    }

    /// <inheritdoc/>
    public override Task<string?> RegisterOperatorStartAsync(string operatorName, string device)
    {
        if (operatorName == "802")
            return Task.FromResult<string?>("Registered");
        else
            return Task.FromResult((string?)null);
    }

    /// <inheritdoc/>
    public Task<IGetWorkOrderItem?> SetWorkOrderAsync(string operatorName, string device, ISetWorkOrderItem res)
    {
        var ok = false;

        switch (res)
        {
            case BeginPickingOrderResult:
                Log($"SetWorkOrder[BeginPickingOrder]: {res.Code} [{res.Started} - {res.Finished}]: {res.Status}", LogLevel.Information);
                break;
            case PickingLineResult pickingLineResult:
                ok = CheckPickingLineResult(pickingLineResult);
                break;
            case PlaceInDockResult placeInDock:
                ok = CheckPlaceInDockResult(placeInDock);
                break;
            case PrintLabelsResult printLabelsResult:
                ok = CheckPrintLabelsResult(printLabelsResult);
                break;
            case ValidatePrintingResult validatePrintingResult:
                ok = CheckValidatePrintingResult(validatePrintingResult);
                break;
            case AskQuestionResult askQuestionResult:
                ok = CheckAskQuestionResult(askQuestionResult);
                break;
        }

        if (!ok)
            throw new InvalidOperationException($"Invalid test: SetWorkOrder: {res.Code} [{res.Started} - {res.Finished}]: {res.Status}");

        if (_assignments.Any())
            return Task.FromResult<IGetWorkOrderItem?>(_assignments.Dequeue());
        else
            return Task.FromResult((IGetWorkOrderItem?)null);
    }

    private bool CheckPickingLineResult(PickingLineResult res)
    {
        Log($"SetWorkOrder[PickingLine]: {res.Code} [{res.Started} - {res.Finished}]: {res.Status}", LogLevel.Information);
        Log($"Picked: {res.Picked.ToString() ?? "(no)"} {res.ServingCode} - Weight: {res.Weight?.ToString() ?? "(no)"} - Dock: {res.Dock?.ToString() ?? "(no)"} - ProductCD: {res.ProductCD?.ToString() ?? "(no)"} - Batch: {res.Batch} - BreakageQuantity: {res.Breakage?.ToString() ?? "(no)"} - Stock: {res.Stock?.ToString() ?? "(no)"}", LogLevel.Information);

        return res.Code switch
        {
            "1.1" => res.Status == "OK" && res.Picked == 3 && res.ServingCode == "BOXES" && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "1.2" => res.Status == "Postponed" && res.Picked == null && res.ServingCode == null && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "1.3" => res.Status == "OK" && res.Picked == 2 && res.Breakage == 2 && res.ServingCode == "BOXES" && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "1.4" => res.Status == "Cancelled" && res.Picked == null && res.ServingCode == null && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "1.5" => res.Status == "OK" && res.Picked == 4 && res.ServingCode == "BOXES" && res.Weight == 20 && res.Dock == null && res.Started <= res.Finished,
            "1.6" => res.Status == "OK" && res.Picked == 49 && res.ServingCode == "packages" && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "1.7" => res.Status == "OK" && res.Picked == 240 && res.ServingCode == "UNITS" && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "1.8" => res.Status == "OK" && res.Picked == 210 && res.ServingCode == "UNITS" && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "1.9" => res.Status == "EndPallet" && !res.Picked.HasValue && res.ServingCode == null && !res.Weight.HasValue && res.Started <= res.Finished,
            "1.9.6" => res.Status == "OK" && res.Picked == 4 && res.ServingCode == "BOXES" && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "2.1" => res.Status == "OK" && res.Picked == 3 && res.ServingCode == "BOXES" && res.Weight == null && res.Batch == "2" && res.Dock == null && res.Started <= res.Finished,
            "2.2" => res.Status == "OK" && res.Picked == 3 && res.ServingCode == "BOXES" && res.Weight == null && string.IsNullOrEmpty(res.Batch) && res.Started <= res.Finished,
            "3.1" => res.Status == "OK" && res.Picked == 3 && res.ServingCode == "BOXES" && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "3.2" => res.Status == "OK" && res.Picked == 3 && res.ServingCode == "BOXES" && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "4.1" => res.Status == "OK" && res.Picked == 3 && res.ServingCode == "BOXES" && res.ProductCD == null && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "4.2" => res.Status == "OK" && res.Picked == 3 && res.ServingCode == "BOXES" && res.ProductCD == null && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "4.3" => res.Status == "OK" && res.Picked == 3 && res.ServingCode == "BOXES" && res.ProductCD == null && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "4.4" => res.Status == "OK" && res.Picked == 3 && res.ServingCode == "BOXES" && res.ProductCD == null && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "5.1" => res.Status == "OK" && res.Picked == 3 && res.ServingCode == "BOXES" && res.ProductCD == "123" && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "6.1" => res.Status == "OK" && res.Picked == 3 && res.ServingCode == "BOXES" && res.ProductCD == null && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "6.2" => res.Status == "Empty" && res.Picked == 2 && res.ServingCode == "BOXES" && res.Stock == 2 && res.ProductCD == null && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "6.3" => res.Status == "OK" && res.Picked == 3 && res.ServingCode == "BOXES" && res.ProductCD == null && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "6.4" => res.Status == "OK" && res.Picked == 4 && res.ServingCode == "BOXES" && res.ProductCD == null && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "6.5" => res.Status == "OK" && res.Picked == 3 && res.ServingCode == "BOXES" && res.ProductCD == null && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "6.6" => res.Status == "OK" && res.Picked == 3 && res.ServingCode == "BOXES" && res.ProductCD == null && res.Weight == 17.5M && res.Dock == null && res.Started <= res.Finished,
            "6.7" => res.Status == "OK" && res.Picked == 3 && res.ServingCode == "BOXES" && res.ProductCD == null && res.Weight == 15 && res.Dock == null && res.Started <= res.Finished,
            "7.1" => res.Status == "OK" && res.Picked == 3 && res.ServingCode == "BOXES" && res.ProductCD == null && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "7.2" => res.Status == "OK" && res.Picked == 3 && res.ServingCode == "BOXES" && res.ProductCD == null && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "7.3" => res.Status == "OK" && res.Picked == 3 && res.ServingCode == "BOXES" && res.ProductCD == null && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "7.4" => res.Status == "OK" && res.Picked == 3 && res.ServingCode == "BOXES" && res.ProductCD == null && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "7.6" => res.Status == "OK" && res.Picked == 4 && res.ServingCode == "BOXES" && res.ProductCD == null && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "7.8" => res.Status == "OK" && res.Picked == 4 && res.ServingCode == "BOXES" && res.ProductCD == null && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "8.1" => res.Status == "OK" && res.Stock == 10 && res.Picked == null && res.ServingCode == null && res.ProductCD == null && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "8.2" => res.Status == "Postponed" && res.Picked == null && res.ServingCode == null && res.ProductCD == null && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "8.3" => res.Status == "BadLocation" && res.Picked == null && res.ServingCode == null && res.ProductCD == null && res.Weight == null && res.Dock == null && res.Started <= res.Finished,
            "8.4" => res.Status == "OK" && res.Stock == 15 && res.Picked == null && res.ServingCode == null && res.ProductCD == null && res.Weight == null && res.Dock == null && res.Started <= res.Finished,

            _ => false,
        };
    }

    private bool CheckPlaceInDockResult(PlaceInDockResult res)
    {
        Log($"SetWorkOrder[PlaceInDock]: {res.Code} [{res.Started} - {res.Finished}]: Dock: {res.Dock} - Status: {res.Status}", LogLevel.Information);

        return res.Code switch
        {
            "1.9.2" => res.Status == "OK" && res.Dock == "1",
            "1.10" => res.Status == "OK" && res.Dock == "1",
            "7.5" => res.Status == "OK" && res.Dock == "2",
            "7.7" => res.Status == "OK" && res.Dock == "3",
            "7.9" => res.Status == "OK" && res.Dock == "1",
            _ => false,
        };
    }

    private bool CheckAskQuestionResult(AskQuestionResult res)
    {
        Log($"SetWorkOrder[AskQuestionResult]: {res.Code} [{res.Started} - {res.Finished}]: Status: {res.Status}", LogLevel.Information);

        return res.Code switch
        {
            "1.11" => res.Status == "OK",
            "1.12" => res.Status == "Cancelled",
            _ => false,
        };
    }

    private bool CheckPrintLabelsResult(PrintLabelsResult res)
    {
        Log($"SetWorkOrder[PrintLabelsResult]: {res.Code} [{res.Started} - {res.Finished}]: Status: {res.Status} - LabelsToPrint: {res.LabelsToPrint?.ToString() ?? "(no)"} - Printer: {res.Printer?.ToString() ?? "(no)"}", LogLevel.Information);

        return res.Code switch
        {
            "1.9.0" => res.Status == "OK" && res.LabelsToPrint == 1 && res.Printer == 1,
            "1.9.3" => res.Status == "OK" && res.LabelsToPrint == 3 && !res.Printer.HasValue,
            "1.9.3.1" => res.Status == "OK" && res.LabelsToPrint == 4 && res.Printer == 2,
            "7.5.1" => res.Status == "OK" && res.LabelsToPrint == 1 && res.Printer == 1,
            _ => false,
        };
    }

    private bool CheckValidatePrintingResult(ValidatePrintingResult res)
    {
        Log($"SetWorkOrder[ValidatePrintingResult]: {res.Code} [{res.Started} - {res.Finished}]: Status: {res.Status} - ReadLabels: {string.Join(",", res.ReadLabels)}", LogLevel.Information);

        return res.Code switch
        {
            "1.9.1" => res.Status == "OK" && res.ReadLabels.SequenceEqual(new[] { "123", "456" }),
            "1.9.4" => res.Status == "OK" && !res.ReadLabels.Any(),
            "1.9.5" => res.Status == "Retry" && !res.ReadLabels.Any(),
            "7.5.2" => res.Status == "OK" && res.ReadLabels.SequenceEqual(new[] { "123", "456" }),
            _ => false,
        };
    }
}