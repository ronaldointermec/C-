using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Tests.Data
{
    public class GetWorkOrderItem_Start : TheoryData<string, GetWorkOrderItemBase, string>
    {
        public GetWorkOrderItem_Start()
        {
            Add(expectedNotWork, null!, null!);
            Add(expectedBeginPickingOrderWithoutOptionalParamsDialog, beginPickingOrderWithoutOptionalParams, null!);
            Add(expectedBeginPickingOrderDialog, beginPickingOrder, "WC|Break");
            Add(expectedPickingLineDialog, pickingLine, null!);
            Add(expectedPickingLineAskBatchDisabledDialog, pickingLineAskBatchDisabled, "WC|Break");
            Add(expectedPickingLineStockCountingAlwaysDialog, pickingLineStockCountingAlways, "WC|Break");
            Add(expectedPickingLineCanSkipAisleDialog, pickingLineCanSkipAisle, "WC|Break");
            Add(expectedPickingLineCountdownDialog, pickingLineCountdown, "WC|Break");
            Add(expectedPickingLineCDWithPositionNullDialog, pickingLineCDWithPositionNull, "WC|Break");
            Add(expectedPrintLabelsWithoutOptionalParamsDialog, printLabelsWithoutOptionalParams, "WC|Break");
            Add(expectedPrintLabelsDialog, printLabels, "WC|Break");
            Add(expectedValidatePrintingWithoutOptionalParamsDialog, validatePrintingWithoutOptionalParams, "WC|Break");
            Add(expectedValidatePrintingDialog, validatePrinting, "WC|Break");
            Add(expectedPlaceInDockWithoutOptionalParamsDialog, placeInDockWithoutOptionalParams, "WC|Break");
            Add(expectedPlaceInDockDialog, placeInDock, "WC|Break");
            Add(expectedAskQuestionDialog, askQuestion, "WC|Break");
        }

        private readonly string expectedNotWork = @"0,setSendHostFlag,""CODE"",1
1,loadMenuOptions,""MENU""
2,setCommandsConfirm,CONFIRM,CONFIRM,CONFIRM,CONFIRM,,,,,CONFIRM,CONFIRM
3,assignNum,""DLG"",1
4,setCommands
5,say,""No work assigned. To try again, say VCONFIRM"",1,0

";
        private readonly BeginPickingOrder beginPickingOrder = new("A123", "Begin picking order line")
        {
            OrderNumber = "O12345",
            Customer = "Customer",
            ContainerType = "Pallet",
            ContainersCount = 1,
        };

        private readonly string expectedBeginPickingOrderDialog = @"0,setSendHostFlag,""CODE"",1
1,loadMenuOptions,""MENU""
2,setCommandsConfirm,CONFIRM,CONFIRM,CONFIRM,CONFIRM,,,,,CONFIRM,CONFIRM
3,assignNum,""DLG"",1
4,assignStr,""CODE"",""A123""
5,assignStr,""CURRENT_AISLE""
6,assignStr,""CURRENT_POSITION""
7,assignStr,""CUSTOMER"",""Customer""
8,assignNum,""START_TIME"",""#time""
9,label,""LABEL_START_A123""
10,setCommands,,,LABEL_BREAK_A123
11,assignStr,""PROMPT"",""Order O12345""
12,concat,""PROMPT"",""PROMPT"","", Customer""
13,concat,""PROMPT"",""PROMPT"","", in Pallet""
14,concat,""PROMPT"",""PROMPT"","", 1 containers""
15,say,""$PROMPT"",1,1
16,say,""Begin picking order line"",1,1
17,goTo,""LABEL_END_A123""
18,label,""LABEL_BREAK_A123""
19,setCommands,,,,,,,,,LABEL_START_A123
20,getMenu,""BREAK_REASON"",""Break reason?"",,""BREAK"",1,""?"",0,,1
21,setSendHostFlag,""BREAK_REASON"",1
22,setSendHostFlag,""RESPONSETYPE"",1
23,assignNum,""RESPONSETYPE"",5
24,getVariablesOdr
25,setSendHostFlag,""BREAK_REASON"",0
26,setCommands
27,say,""At break. To resume work, say VCONFIRM"",1,0
28,assignNum,""RESPONSETYPE"",6
29,getVariablesOdr
30,goTo,""LABEL_START_A123""
31,label,""LABEL_END_A123""
32,assignNum,""END_TIME"",""#time""
33,assignStr,""STATUS"",""OK""
34,assignNum,""RESPONSETYPE"",0
35,setSendHostFlag,""STATUS"",1
36,setSendHostFlag,""START_TIME"",1
37,setSendHostFlag,""END_TIME"",1
38,setSendHostFlag,""RESPONSETYPE"",1

";
        private readonly BeginPickingOrder beginPickingOrderWithoutOptionalParams = new("A123", string.Empty)
        {
            OrderNumber = "O12345",
        };

        private readonly string expectedBeginPickingOrderWithoutOptionalParamsDialog = @"0,setSendHostFlag,""CODE"",1
1,loadMenuOptions,""MENU""
2,setCommandsConfirm,CONFIRM,CONFIRM,CONFIRM,CONFIRM,,,,,CONFIRM,CONFIRM
3,assignNum,""DLG"",1
4,assignStr,""CODE"",""A123""
5,assignStr,""CURRENT_AISLE""
6,assignStr,""CURRENT_POSITION""
7,assignNum,""START_TIME"",""#time""
8,label,""LABEL_START_A123""
9,setCommands,,,LABEL_BREAK_A123
10,assignStr,""PROMPT"",""Order O12345""
11,say,""$PROMPT"",1,1
12,goTo,""LABEL_END_A123""
13,label,""LABEL_BREAK_A123""
14,setCommands,,,,,,,,,LABEL_START_A123
15,setSendHostFlag,""RESPONSETYPE"",1
16,assignNum,""RESPONSETYPE"",5
17,getVariablesOdr
18,setCommands
19,say,""At break. To resume work, say VCONFIRM"",1,0
20,assignNum,""RESPONSETYPE"",6
21,getVariablesOdr
22,goTo,""LABEL_START_A123""
23,label,""LABEL_END_A123""
24,assignNum,""END_TIME"",""#time""
25,assignStr,""STATUS"",""OK""
26,assignNum,""RESPONSETYPE"",0
27,setSendHostFlag,""STATUS"",1
28,setSendHostFlag,""START_TIME"",1
29,setSendHostFlag,""END_TIME"",1
30,setSendHostFlag,""RESPONSETYPE"",1

";

        private readonly PickingLine pickingLine = new("A123", "Message")
        {
            Customer = "Customer",
            Aisle = "Aisle",
            Slot = "Slot",
            Position = "Position",
            CD = "CD",
            ProductName = "ProductName",
            ProductNumber = "ProductNumber",
            UpcNumber = "UpcNumber",
            OriginalServingCode = "OriginalServingCode",
            OriginalServingPrompt = "OriginalServingPrompt",
            OriginalServingQuantity = 1,
            OriginalServingUpperTolerance = 2.5m,
            OriginalServingLowerTolerance = 1.5m,
            OriginalMaxQuantityAllowedPerPick = 1,
            OriginalUnitsFormat = 1,
            AlternativeServingCode = "AlternativeServingCode",
            AlternativeServingPrompt = "AlternativeServingPrompt",
            AlternativeServingQuantity = 1,
            AlternativeServingUpperTolerance = 2.5m,
            AlternativeServingLowerTolerance = 1.5m,
            AlternativeMaxQuantityAllowedPerPick = 1,
            AlternativeUnitsFormat = 1,
            UnitsQuantity = 1,
            UnitsUpperTolerance = 1.5m,
            UnitsLowerTolerance = 2.5m,
            UnitsMaxQuantityAllowedPerPick = 1,
            AskWeight = true,
            WeightMin = 1.5m,
            WeightMax = 2.5m,
            HowMuchMore = 10,
            StockCounting = StockCountingMode.PartialPicked,
            SpeakProductName = false,
            AskBatch = true,
            Batches = new string[] { "123", "456" },
            CanSkipAisle = false,
            HelpMsg = "HelpMessage",
            CountdownPick = false,
            ProductCDs = new string[] { "1234", "567" },
        };

        private readonly string expectedPickingLineDialog = @"0,setSendHostFlag,""CODE"",1
1,loadMenuOptions,""MENU""
2,setCommandsConfirm,CONFIRM,CONFIRM,CONFIRM,CONFIRM,,,,,CONFIRM,CONFIRM
3,assignNum,""DLG"",1
4,assignStr,""CODE"",""A123""
5,label,""LABEL_START_A123""
6,assignStr,""STATUS""
7,assignStr,""PICKED""
8,assignStr,""SERVING""
9,assignStr,""WEIGHT""
10,assignStr,""STOCK""
11,assignStr,""DOCK""
12,assignStr,""BREAKAGE""
13,assignStr,""BATCH""
14,assignNum,""START_TIME"",""#time""
15,assignStr,""PRODUCT_DESCRIPTION"",""productname""
16,assignStr,""PRODUCT_NUMBER"",""productnumber""
17,assignStr,""UPC_NUMBER"",""upcnumber""
18,assignStr,""QTY_UPPER""
19,assignStr,""QTY_LOWER""
20,assignStr,""MAX_QTY_ALLOWED_PER_PICK""
21,assignStr,""TOTAL_PICKED""
22,assignStr,""TOTAL_WEIGHT""
23,assignStr,""CUSTOMER"",""Customer""
24,setCommands
25,say,""Message"",1,1
26,assignStr,""CURRENT_AISLE""
27,assignStr,""CURRENT_POSITION""
28,setCommands,LABEL_EXCEPTION_LOCATION_A123,LABEL_DOCK_A123,LABEL_BREAK_A123,,,There are 10 lines remaining,,,,,$CUSTOMER
29,doIf,""CURRENT_AISLE"",""Aisle"",""<>"",""STR"",,,,,,,,,,,1
30,assignStr,""CURRENT_AISLE"",""Aisle""
31,assignStr,""CURRENT_POSITION"",""Position""
32,say,""Aisle Aisle"",1,0
33,doEndIf,,,,,,,,,,,,,,,1
34,assignStr,""WHEREAMI"",""Aisle Aisle""
35,setCommand,4,""LABEL_SKIP_SLOT_A123"",CONFIRM
36,say,""Slot Slot"",1,0
37,concat,""WHEREAMI"",""WHEREAMI"","", Slot Slot""
38,setCommands,LABEL_EXCEPTION_CD_A123,LABEL_DOCK_A123,LABEL_BREAK_A123,LABEL_SKIP_SLOT_A123,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
39,getDigits,""DUMMY"",""Position"",,10,0,0,,""1"",""2"",,,1,""CD"",""Incorrect"",1,,,0
40,concat,""WHEREAMI"",""WHEREAMI"","", Position""
41,say,""$PRODUCT_DESCRIPTION"",0,0
42,label,""LABEL_VALIDATE_PRODUCT_CD_A123""
43,getDigits,""PRODUCT_CD"",""Check digit?"",,10,0,0,,""1"",""20"",,,1,,,1,,,0
44,doIf,""PRODUCT_CD"",""1234"",""="",""STR"",,,,,,,,,,,2
45,setSendHostFlag,""PRODUCT_CD"",1
46,goTo,""LABEL_ASK_BATCH_A123""
47,doEndIf,,,,,,,,,,,,,,,2
48,doIf,""PRODUCT_CD"",""567"",""="",""STR"",,,,,,,,,,,3
49,setSendHostFlag,""PRODUCT_CD"",1
50,goTo,""LABEL_ASK_BATCH_A123""
51,doEndIf,,,,,,,,,,,,,,,3
52,concat,""DUMMY"",""Incorrect, "",""PRODUCT_CD""
53,say,""$DUMMY"",0,0
54,goTo,""LABEL_VALIDATE_PRODUCT_CD_A123""
55,label,""LABEL_ASK_BATCH_A123""
56,say,""Select batch"",0,0
57,setSendHostFlag,""BATCH"",1
58,ask,""DUMMY"",""Batch 123?"",,1,1,""y"",""n"",""c""
59,doIf,""DUMMY"",""y"",""="",""STR"",,,,,,,,,,,4
60,assignStr,""BATCH"",""123""
61,goTo,""LABEL_RESET_PICK_A123""
62,doElseIf,""DUMMY"",""c"",""="",""STR"",,,,,,,,,,,4
63,goTo,""LABEL_RESET_PICK_A123""
64,doEndIf,,,,,,,,,,,,,,,4
65,ask,""DUMMY"",""Batch 456?"",,1,1,""y"",""n"",""c""
66,doIf,""DUMMY"",""y"",""="",""STR"",,,,,,,,,,,5
67,assignStr,""BATCH"",""456""
68,goTo,""LABEL_RESET_PICK_A123""
69,doElseIf,""DUMMY"",""c"",""="",""STR"",,,,,,,,,,,5
70,goTo,""LABEL_RESET_PICK_A123""
71,doEndIf,,,,,,,,,,,,,,,5
72,goTo,""LABEL_ASK_BATCH_A123""
73,label,""LABEL_RESET_PICK_A123""
74,assignStr,""PROMPT"",""OriginalServingPrompt""
75,assignNum,""QTY_REQUESTED"",1
76,assignNum,""QTY_REQUESTED_ORIG"",1
77,assignStr,""TOTAL_PICKED""
78,assignStr,""TOTAL_WEIGHT""
79,assignNum,""QTY_UPPER"",1
80,assignNum,""QTY_LOWER"",1
81,assignNum,""MAX_QTY_ALLOWED_PER_PICK"",1
82,assignStr,""SERVING"",""OriginalServingCode""
83,label,""LABEL_PICK_A123""
84,setCommands,LABEL_EXCEPTION_A123,,,LABEL_SKIP_SLOT_A123,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,LABEL_BREAKAGE_A123,$CUSTOMER,LABEL_FORMAT_A123,LABEL_UNITS_A123,,$PRODUCT_NUMBER,$UPC_NUMBER
85,getDigits,""TOTAL_PICKED"",""$PROMPT"",""HelpMessage"",10,0,0,,""1"",""20"",""$QTY_LOWER"",""$QTY_UPPER"",0,,,1,,,0
86,doIf,""TOTAL_PICKED"",""QTY_REQUESTED"",""<"",""NUM"",,,,,,,,,,,6
87,concat,""PROMPT"",""Asked for "",""QTY_REQUESTED""
88,concat,""PROMPT"",""PROMPT"","", you said ""
89,concat,""PROMPT"",""PROMPT"",""TOTAL_PICKED""
90,concat,""PROMPT"",""PROMPT"","". Is this a short?""
91,ask,""DUMMY"",""$PROMPT"",,1,0,""1"",""0""
92,doIf,""DUMMY"",""0"",""="",""NUM"",,,,,,,,,,,7
93,goTo,""LABEL_RESET_PICK_A123""
94,doEndIf,,,,,,,,,,,,,,,7
95,setCommands,,,,,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
96,getMenu,""DUMMY"",""Reason code?"",,""LOWER_QUANTITY"",1,""?"",0,""Incorrect"",1
97,doIf,""DUMMY"",""1"",""="",""NUM"",,,,,,,,,,,8
98,assignStr,""STATUS"",""Empty""
99,doElseIf,""DUMMY"",""2"",""="",""NUM"",,,,,,,,,,,8
100,assignStr,""STATUS"",""Breakage""
101,doElseIf,""DUMMY"",""3"",""="",""NUM"",,,,,,,,,,,8
102,assignStr,""STATUS"",""Completed""
103,doElseIf,""DUMMY"",""4"",""="",""NUM"",,,,,,,,,,,8
104,assignStr,""STATUS"",""EndPallet""
105,doElseIf,""DUMMY"",""5"",""="",""NUM"",,,,,,,,,,,8
106,assignStr,""STATUS"",""OK""
107,doElseIf,""DUMMY"",""6"",""="",""NUM"",,,,,,,,,,,8
108,goTo,""LABEL_RESET_PICK_A123""
109,doEndIf,,,,,,,,,,,,,,,8
110,getDigits,""STOCK"",""Quantity in location"",,10,0,1,""?"",""1"",""20"",,,0,,,1,,,0
111,setSendHostFlag,""STOCK"",1
112,doElse,,,,,,,,,,,,,,,6
113,assignStr,""STATUS"",""OK""
114,doEndIf,,,,,,,,,,,,,,,6
115,label,""LABEL_WEIGHT_A123""
116,getFloat,""TOTAL_WEIGHT"",""Weight?"",,10,0,1,""?"",""1"",""20"",,,0,,,1,,,0
117,doIf,""TOTAL_WEIGHT"",""0"",""<>"",""NUM"",,,,,,,,,,,9
118,doIf,""TOTAL_WEIGHT"",""1.5"",""<"",""NUM"",,,,,,,,,,,10
119,say,""The weight is less than allowed"",0,0
120,goTo,""LABEL_WEIGHT_A123""
121,doEndIf,,,,,,,,,,,,,,,10
122,doIf,""TOTAL_WEIGHT"",""2.5"","">"",""NUM"",,,,,,,,,,,11
123,say,""The weight is more than allowed"",0,0
124,goTo,""LABEL_WEIGHT_A123""
125,doEndIf,,,,,,,,,,,,,,,11
126,doEndIf,,,,,,,,,,,,,,,9
127,setSendHostFlag,""TOTAL_WEIGHT"",1
128,label,""LABEL_CONFIRM_A123""
129,assignNum,""END_TIME"",""#time""
130,assignNum,""RESPONSETYPE"",1
131,setSendHostFlag,""STATUS"",1
132,setSendHostFlag,""START_TIME"",1
133,setSendHostFlag,""END_TIME"",1
134,setSendHostFlag,""TOTAL_PICKED"",1
135,setSendHostFlag,""SERVING"",1
136,setSendHostFlag,""RESPONSETYPE"",1
137,goTo,""LABEL_END_A123""
138,label,""LABEL_FORMAT_A123""
139,setCommands,LABEL_EXCEPTION_A123,,,,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
140,assignStr,""PROMPT"",""AlternativeServingPrompt""
141,assignNum,""QTY_REQUESTED"",1
142,assignNum,""QTY_REQUESTED_ORIG"",1
143,assignNum,""QTY_UPPER"",1
144,assignNum,""QTY_LOWER"",1
145,assignNum,""MAX_QTY_ALLOWED_PER_PICK"",1
146,assignStr,""SERVING"",""AlternativeServingCode""
147,goTo,""LABEL_PICK_A123""
148,label,""LABEL_BREAKAGE_A123""
149,getDigits,""BREAKAGE"",""Breakage quantity?"",,10,0,1,""?"",""1"",""20"",,,0,,,1,,,0
150,setSendHostFlag,""BREAKAGE"",1
151,goTo,""LABEL_PICK_A123""
152,label,""LABEL_UNITS_A123""
153,setCommands,LABEL_EXCEPTION_A123,,,,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
154,assignStr,""PROMPT"",""1 units""
155,assignNum,""QTY_REQUESTED"",1
156,assignNum,""QTY_REQUESTED_ORIG"",1
157,assignNum,""QTY_UPPER"",1
158,assignNum,""QTY_LOWER"",1
159,assignNum,""MAX_QTY_ALLOWED_PER_PICK"",1
160,assignStr,""SERVING"",""UNITS""
161,goTo,""LABEL_PICK_A123""
162,label,""LABEL_EXCEPTION_LOCATION_A123""
163,assignStr,""STATUS"",""BadLocation""
164,assignStr,""SERVING""
165,goTo,""LABEL_CONFIRM_A123""
166,label,""LABEL_EXCEPTION_CD_A123""
167,assignStr,""STATUS"",""NoCheck""
168,assignStr,""SERVING""
169,goTo,""LABEL_CONFIRM_A123""
170,label,""LABEL_EXCEPTION_A123""
171,assignStr,""STATUS"",""Cancelled""
172,assignStr,""SERVING""
173,goTo,""LABEL_CONFIRM_A123""
174,label,""LABEL_SKIP_SLOT_A123""
175,assignStr,""STATUS"",""Postponed""
176,assignStr,""SERVING""
177,goTo,""LABEL_CONFIRM_A123""
178,label,""LABEL_DOCK_A123""
179,assignNum,""RESPONSETYPE"",1
180,assignStr,""STATUS"",""EndPallet""
181,assignNum,""END_TIME"",""#time""
182,setSendHostFlag,""RESPONSETYPE"",1
183,setSendHostFlag,""STATUS"",1
184,setSendHostFlag,""START_TIME"",1
185,setSendHostFlag,""END_TIME"",1
186,goTo,""LABEL_END_A123""
187,label,""LABEL_BREAK_A123""
188,assignStr,""CURRENT_AISLE""
189,assignStr,""CURRENT_POSITION""
190,setCommands,,,,,,,,,LABEL_START_A123
191,setSendHostFlag,""RESPONSETYPE"",1
192,assignNum,""RESPONSETYPE"",5
193,getVariablesOdr
194,setCommands
195,say,""At break. To resume work, say VCONFIRM"",1,0
196,assignNum,""RESPONSETYPE"",6
197,getVariablesOdr
198,goTo,""LABEL_START_A123""
199,label,""LABEL_END_A123""

";

        private readonly PickingLine pickingLineCountdown = new("A123", "Message")
        {
            Customer = "Customer",
            Aisle = "Aisle",
            Slot = "Slot",
            Position = "Position",
            CD = "CD",
            ProductName = "ProductName",
            ProductNumber = "ProductNumber",
            UpcNumber = "UpcNumber",
            OriginalServingCode = "OriginalServingCode",
            OriginalServingPrompt = "OriginalServingPrompt",
            OriginalServingQuantity = 1,
            OriginalServingUpperTolerance = 2.5m,
            OriginalServingLowerTolerance = 1.5m,
            OriginalMaxQuantityAllowedPerPick = 1,
            OriginalUnitsFormat = 1,
            AlternativeServingCode = "AlternativeServingCode",
            AlternativeServingPrompt = "AlternativeServingPrompt",
            AlternativeServingQuantity = 1,
            AlternativeServingUpperTolerance = 2.5m,
            AlternativeServingLowerTolerance = 1.5m,
            AlternativeMaxQuantityAllowedPerPick = 1,
            AlternativeUnitsFormat = 1,
            UnitsQuantity = 1,
            UnitsUpperTolerance = 1.5m,
            UnitsLowerTolerance = 2.5m,
            UnitsMaxQuantityAllowedPerPick = 1,
            AskWeight = true,
            WeightMin = 1.5m,
            WeightMax = 2.5m,
            HowMuchMore = 10,
            StockCounting = StockCountingMode.PartialPicked,
            SpeakProductName = true,
            AskBatch = false,
            Batches = new string[] { "123", "456" },
            CanSkipAisle = false,
            HelpMsg = "HelpMessage",
            CountdownPick = true,
        };

        private readonly string expectedPickingLineCountdownDialog = @"0,setSendHostFlag,""CODE"",1
1,loadMenuOptions,""MENU""
2,setCommandsConfirm,CONFIRM,CONFIRM,CONFIRM,CONFIRM,,,,,CONFIRM,CONFIRM
3,assignNum,""DLG"",1
4,assignStr,""CODE"",""A123""
5,label,""LABEL_START_A123""
6,assignStr,""STATUS""
7,assignStr,""PICKED""
8,assignStr,""SERVING""
9,assignStr,""WEIGHT""
10,assignStr,""STOCK""
11,assignStr,""DOCK""
12,assignStr,""BREAKAGE""
13,assignStr,""BATCH""
14,assignNum,""START_TIME"",""#time""
15,assignStr,""PRODUCT_DESCRIPTION"",""productname""
16,assignStr,""PRODUCT_NUMBER"",""productnumber""
17,assignStr,""UPC_NUMBER"",""upcnumber""
18,assignStr,""QTY_UPPER""
19,assignStr,""QTY_LOWER""
20,assignStr,""MAX_QTY_ALLOWED_PER_PICK""
21,assignStr,""TOTAL_PICKED""
22,assignStr,""TOTAL_WEIGHT""
23,assignStr,""CUSTOMER"",""Customer""
24,setCommands
25,say,""Message"",1,1
26,assignStr,""CURRENT_AISLE""
27,assignStr,""CURRENT_POSITION""
28,setCommands,LABEL_EXCEPTION_LOCATION_A123,LABEL_DOCK_A123,LABEL_BREAK_A123,,,There are 10 lines remaining,,,,,$CUSTOMER
29,doIf,""CURRENT_AISLE"",""Aisle"",""<>"",""STR"",,,,,,,,,,,1
30,assignStr,""CURRENT_AISLE"",""Aisle""
31,assignStr,""CURRENT_POSITION"",""Position""
32,say,""Aisle Aisle"",1,0
33,doEndIf,,,,,,,,,,,,,,,1
34,assignStr,""WHEREAMI"",""Aisle Aisle""
35,setCommand,4,""LABEL_SKIP_SLOT_A123"",CONFIRM
36,say,""Slot Slot"",1,0
37,concat,""WHEREAMI"",""WHEREAMI"","", Slot Slot""
38,setCommands,LABEL_EXCEPTION_CD_A123,LABEL_DOCK_A123,LABEL_BREAK_A123,LABEL_SKIP_SLOT_A123,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
39,getDigits,""DUMMY"",""Position"",,10,0,0,,""1"",""2"",,,1,""CD"",""Incorrect"",1,,,0
40,concat,""WHEREAMI"",""WHEREAMI"","", Position""
41,say,""$PRODUCT_DESCRIPTION"",0,0
42,label,""LABEL_ASK_BATCH_A123""
43,label,""LABEL_RESET_PICK_A123""
44,assignStr,""PROMPT"",""OriginalServingPrompt""
45,assignNum,""QTY_REQUESTED"",1
46,assignNum,""QTY_REQUESTED_ORIG"",1
47,assignStr,""TOTAL_PICKED""
48,assignStr,""TOTAL_WEIGHT""
49,assignNum,""QTY_UPPER"",1
50,assignNum,""QTY_LOWER"",1
51,assignNum,""MAX_QTY_ALLOWED_PER_PICK"",1
52,assignStr,""SERVING"",""OriginalServingCode""
53,label,""LABEL_PICK_A123""
54,setCommands,LABEL_EXCEPTION_A123,,,LABEL_SKIP_SLOT_A123,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,LABEL_BREAKAGE_A123,$CUSTOMER,LABEL_FORMAT_A123,LABEL_UNITS_A123,,$PRODUCT_NUMBER,$UPC_NUMBER
55,getDigits,""PICKED"",""$PROMPT"",""HelpMessage"",10,0,0,,""1"",""20"",,,0,,,1,,,0
56,doIf,""MAX_QTY_ALLOWED_PER_PICK"",,""<>"",""STR"",,,,,,,,,,,2
57,doIf,""PICKED"",""MAX_QTY_ALLOWED_PER_PICK"","">"",""NUM"",,,,,,,,,,,3
58,concat,""DUMMY"",""The maximum quantity allowed per pick is"",""MAX_QTY_ALLOWED_PER_PICK""
59,say,""$DUMMY"",0,1
60,goTo,""LABEL_PICK_A123""
61,doEndIf,,,,,,,,,,,,,,,3
62,doEndIf,,,,,,,,,,,,,,,2
63,doIf,""PICKED"",""QTY_REQUESTED"",""<"",""NUM"",,,,,,,,,,,4
64,setCommands,LABEL_EXCEPTION_A123,,,,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,LABEL_BREAKAGE_A123,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
65,doIf,""PICKED"",""0"",""="",""NUM"",,,,,,,,,,,5
66,add,""TOTAL_PICKED"",""TOTAL_PICKED"",""PICKED""
67,concat,""PROMPT"",""Asked for "",""QTY_REQUESTED_ORIG""
68,concat,""PROMPT"",""PROMPT"","", you said ""
69,concat,""PROMPT"",""PROMPT"",""TOTAL_PICKED""
70,concat,""PROMPT"",""PROMPT"","". Is this a short?""
71,ask,""DUMMY"",""$PROMPT"",,1,0,""1"",""0""
72,doIf,""DUMMY"",""0"",""="",""NUM"",,,,,,,,,,,6
73,goTo,""LABEL_PICK_A123""
74,doEndIf,,,,,,,,,,,,,,,6
75,setCommands,,,,,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
76,getMenu,""DUMMY"",""Reason code?"",,""LOWER_QUANTITY"",1,""?"",0,""Incorrect"",1
77,doIf,""DUMMY"",""1"",""="",""NUM"",,,,,,,,,,,7
78,assignStr,""STATUS"",""Empty""
79,doElseIf,""DUMMY"",""2"",""="",""NUM"",,,,,,,,,,,7
80,assignStr,""STATUS"",""Breakage""
81,doElseIf,""DUMMY"",""3"",""="",""NUM"",,,,,,,,,,,7
82,assignStr,""STATUS"",""Completed""
83,doElseIf,""DUMMY"",""4"",""="",""NUM"",,,,,,,,,,,7
84,assignStr,""STATUS"",""EndPallet""
85,doElseIf,""DUMMY"",""5"",""="",""NUM"",,,,,,,,,,,7
86,assignStr,""STATUS"",""OK""
87,doElseIf,""DUMMY"",""6"",""="",""NUM"",,,,,,,,,,,7
88,goTo,""LABEL_RESET_PICK_A123""
89,doEndIf,,,,,,,,,,,,,,,7
90,doElse,,,,,,,,,,,,,,,5
91,label,""LABEL_WEIGHT_A123""
92,getFloat,""WEIGHT"",""Weight?"",,10,0,1,""?"",""1"",""20"",,,0,,,1,,,0
93,doIf,""SERVING"",""OriginalServingCode"",""="",""STR"",,,,,,,,,,,8
94,multiply,""DUMMY"",""PICKED"",1
95,doElseIf,""SERVING"",""AlternativeServingCode"",""="",""STR"",,,,,,,,,,,8
96,multiply,""DUMMY"",""PICKED"",1
97,doElse,,,,,,,,,,,,,,,8
98,assignNum,""DUMMY"",""PICKED""
99,doEndIf,,,,,,,,,,,,,,,8
100,multiply,""DUMMY"",""DUMMY"",""1.5""
101,doIf,""WEIGHT"",""DUMMY"",""<"",""NUM"",,,,,,,,,,,9
102,say,""The weight is less than allowed"",0,0
103,goTo,""LABEL_WEIGHT_A123""
104,doEndIf,,,,,,,,,,,,,,,9
105,doIf,""SERVING"",""OriginalServingCode"",""="",""STR"",,,,,,,,,,,10
106,multiply,""DUMMY"",""PICKED"",1
107,doElseIf,""SERVING"",""AlternativeServingCode"",""="",""STR"",,,,,,,,,,,10
108,multiply,""DUMMY"",""PICKED"",1
109,doElse,,,,,,,,,,,,,,,10
110,assignNum,""DUMMY"",""PICKED""
111,doEndIf,,,,,,,,,,,,,,,10
112,multiply,""DUMMY"",""DUMMY"",""2.5""
113,doIf,""WEIGHT"",""DUMMY"","">"",""NUM"",,,,,,,,,,,11
114,say,""The weight is more than allowed"",0,0
115,goTo,""LABEL_WEIGHT_A123""
116,doEndIf,,,,,,,,,,,,,,,11
117,add,""TOTAL_WEIGHT"",""TOTAL_WEIGHT"",""WEIGHT""
118,setSendHostFlag,""TOTAL_WEIGHT"",1
119,subtract,""QTY_REQUESTED"",""QTY_REQUESTED"",""PICKED""
120,add,""TOTAL_PICKED"",""TOTAL_PICKED"",""PICKED""
121,concat,""PROMPT"",""Remain"",""QTY_REQUESTED""
122,goTo,""LABEL_PICK_A123""
123,doEndIf,,,,,,,,,,,,,,,5
124,getDigits,""STOCK"",""Quantity in location"",,10,0,1,""?"",""1"",""20"",,,0,,,1,,,0
125,setSendHostFlag,""STOCK"",1
126,doElse,,,,,,,,,,,,,,,4
127,doIf,""QTY_UPPER"",,""="",""NUM"",,,,,,,,,,,12
128,doIf,""PICKED"",""QTY_REQUESTED"","">"",""NUM"",,,,,,,,,,,13
129,say,""Quantity greater than requested"",0,0
130,goTo,""LABEL_PICK_A123""
131,doEndIf,,,,,,,,,,,,,,,13
132,doElse,,,,,,,,,,,,,,,12
133,add,""DUMMY"",""TOTAL_PICKED"",""PICKED""
134,doIf,""DUMMY"",""QTY_UPPER"","">"",""NUM"",,,,,,,,,,,14
135,say,""Quantity greater than requested"",0,0
136,goTo,""LABEL_PICK_A123""
137,doElseIf,""DUMMY"",""QTY_REQUESTED_ORIG"","">"",""NUM"",,,,,,,,,,,14
138,doIf,""DUMMY"",""QTY_UPPER"",""<="",""NUM"",,,,,,,,,,,15
139,concat,""DUMMY"",""DUMMY"","" , correct?""
140,ask,""DUMMY"",""$DUMMY"",,1,0,""1"",""0""
141,doIf,""DUMMY"",""0"",""="",""NUM"",,,,,,,,,,,16
142,goTo,""LABEL_PICK_A123""
143,doEndIf,,,,,,,,,,,,,,,16
144,doEndIf,,,,,,,,,,,,,,,15
145,doEndIf,,,,,,,,,,,,,,,14
146,doEndIf,,,,,,,,,,,,,,,12
147,assignStr,""STATUS"",""OK""
148,subtract,""QTY_REQUESTED"",""QTY_REQUESTED"",""PICKED""
149,add,""TOTAL_PICKED"",""TOTAL_PICKED"",""PICKED""
150,label,""LABEL_WEIGHT_A123""
151,getFloat,""WEIGHT"",""Weight?"",,10,0,1,""?"",""1"",""20"",,,0,,,1,,,0
152,doIf,""SERVING"",""OriginalServingCode"",""="",""STR"",,,,,,,,,,,17
153,multiply,""DUMMY"",""PICKED"",1
154,doElseIf,""SERVING"",""AlternativeServingCode"",""="",""STR"",,,,,,,,,,,17
155,multiply,""DUMMY"",""PICKED"",1
156,doElse,,,,,,,,,,,,,,,17
157,assignNum,""DUMMY"",""PICKED""
158,doEndIf,,,,,,,,,,,,,,,17
159,multiply,""DUMMY"",""DUMMY"",""1.5""
160,doIf,""WEIGHT"",""DUMMY"",""<"",""NUM"",,,,,,,,,,,18
161,say,""The weight is less than allowed"",0,0
162,goTo,""LABEL_WEIGHT_A123""
163,doEndIf,,,,,,,,,,,,,,,18
164,doIf,""SERVING"",""OriginalServingCode"",""="",""STR"",,,,,,,,,,,19
165,multiply,""DUMMY"",""PICKED"",1
166,doElseIf,""SERVING"",""AlternativeServingCode"",""="",""STR"",,,,,,,,,,,19
167,multiply,""DUMMY"",""PICKED"",1
168,doElse,,,,,,,,,,,,,,,19
169,assignNum,""DUMMY"",""PICKED""
170,doEndIf,,,,,,,,,,,,,,,19
171,multiply,""DUMMY"",""DUMMY"",""2.5""
172,doIf,""WEIGHT"",""DUMMY"","">"",""NUM"",,,,,,,,,,,20
173,say,""The weight is more than allowed"",0,0
174,goTo,""LABEL_WEIGHT_A123""
175,doEndIf,,,,,,,,,,,,,,,20
176,add,""TOTAL_WEIGHT"",""TOTAL_WEIGHT"",""WEIGHT""
177,setSendHostFlag,""TOTAL_WEIGHT"",1
178,doEndIf,,,,,,,,,,,,,,,4
179,label,""LABEL_CONFIRM_A123""
180,assignNum,""END_TIME"",""#time""
181,assignNum,""RESPONSETYPE"",1
182,setSendHostFlag,""STATUS"",1
183,setSendHostFlag,""START_TIME"",1
184,setSendHostFlag,""END_TIME"",1
185,setSendHostFlag,""TOTAL_PICKED"",1
186,setSendHostFlag,""SERVING"",1
187,setSendHostFlag,""RESPONSETYPE"",1
188,goTo,""LABEL_END_A123""
189,label,""LABEL_FORMAT_A123""
190,setCommands,LABEL_EXCEPTION_A123,,,,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
191,assignStr,""PROMPT"",""AlternativeServingPrompt""
192,assignNum,""QTY_REQUESTED"",1
193,assignNum,""QTY_REQUESTED_ORIG"",1
194,assignNum,""QTY_UPPER"",1
195,assignNum,""QTY_LOWER"",1
196,assignNum,""MAX_QTY_ALLOWED_PER_PICK"",1
197,assignStr,""SERVING"",""AlternativeServingCode""
198,goTo,""LABEL_PICK_A123""
199,label,""LABEL_BREAKAGE_A123""
200,getDigits,""BREAKAGE"",""Breakage quantity?"",,10,0,1,""?"",""1"",""20"",,,0,,,1,,,0
201,setSendHostFlag,""BREAKAGE"",1
202,goTo,""LABEL_PICK_A123""
203,label,""LABEL_UNITS_A123""
204,setCommands,LABEL_EXCEPTION_A123,,,,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
205,assignStr,""PROMPT"",""1 units""
206,assignNum,""QTY_REQUESTED"",1
207,assignNum,""QTY_REQUESTED_ORIG"",1
208,assignNum,""QTY_UPPER"",1
209,assignNum,""QTY_LOWER"",1
210,assignNum,""MAX_QTY_ALLOWED_PER_PICK"",1
211,assignStr,""SERVING"",""UNITS""
212,goTo,""LABEL_PICK_A123""
213,label,""LABEL_EXCEPTION_LOCATION_A123""
214,assignStr,""STATUS"",""BadLocation""
215,assignStr,""SERVING""
216,goTo,""LABEL_CONFIRM_A123""
217,label,""LABEL_EXCEPTION_CD_A123""
218,assignStr,""STATUS"",""NoCheck""
219,assignStr,""SERVING""
220,goTo,""LABEL_CONFIRM_A123""
221,label,""LABEL_EXCEPTION_A123""
222,assignStr,""STATUS"",""Cancelled""
223,assignStr,""SERVING""
224,goTo,""LABEL_CONFIRM_A123""
225,label,""LABEL_SKIP_SLOT_A123""
226,assignStr,""STATUS"",""Postponed""
227,assignStr,""SERVING""
228,goTo,""LABEL_CONFIRM_A123""
229,label,""LABEL_DOCK_A123""
230,assignNum,""RESPONSETYPE"",1
231,assignStr,""STATUS"",""EndPallet""
232,assignNum,""END_TIME"",""#time""
233,setSendHostFlag,""RESPONSETYPE"",1
234,setSendHostFlag,""STATUS"",1
235,setSendHostFlag,""START_TIME"",1
236,setSendHostFlag,""END_TIME"",1
237,goTo,""LABEL_END_A123""
238,label,""LABEL_BREAK_A123""
239,assignStr,""CURRENT_AISLE""
240,assignStr,""CURRENT_POSITION""
241,setCommands,,,,,,,,,LABEL_START_A123
242,getMenu,""BREAK_REASON"",""Break reason?"",,""BREAK"",1,""?"",0,,1
243,setSendHostFlag,""BREAK_REASON"",1
244,setSendHostFlag,""RESPONSETYPE"",1
245,assignNum,""RESPONSETYPE"",5
246,getVariablesOdr
247,setSendHostFlag,""BREAK_REASON"",0
248,setCommands
249,say,""At break. To resume work, say VCONFIRM"",1,0
250,assignNum,""RESPONSETYPE"",6
251,getVariablesOdr
252,goTo,""LABEL_START_A123""
253,label,""LABEL_END_A123""

";

        private readonly PickingLine pickingLineAskBatchDisabled = new("A123", "Message")
        {
            Customer = "Customer",
            Aisle = "Aisle",
            Slot = "Slot",
            Position = "Position",
            CD = "CD",
            ProductName = "ProductName",
            ProductNumber = "ProductNumber",
            UpcNumber = "UpcNumber",
            OriginalServingCode = "OriginalServingCode",
            OriginalServingPrompt = "OriginalServingPrompt",
            OriginalServingQuantity = 1,
            OriginalServingUpperTolerance = 2.5m,
            OriginalServingLowerTolerance = 1.5m,
            OriginalMaxQuantityAllowedPerPick = 1,
            OriginalUnitsFormat = 1,
            AlternativeServingCode = "AlternativeServingCode",
            AlternativeServingPrompt = "AlternativeServingPrompt",
            AlternativeServingQuantity = 1,
            AlternativeServingUpperTolerance = 2.5m,
            AlternativeServingLowerTolerance = 1.5m,
            AlternativeMaxQuantityAllowedPerPick = 1,
            AlternativeUnitsFormat = 1,
            UnitsQuantity = 1,
            UnitsUpperTolerance = 1.5m,
            UnitsLowerTolerance = 2.5m,
            UnitsMaxQuantityAllowedPerPick = 1,
            AskWeight = true,
            WeightMin = 1.5m,
            WeightMax = 2.5m,
            HowMuchMore = 10,
            StockCounting = StockCountingMode.PartialPicked,
            SpeakProductName = true,
            AskBatch = false,
            Batches = new string[] { "123", "456" },
            CanSkipAisle = false,
            HelpMsg = "HelpMessage",
            CountdownPick = false,
        };

        private readonly string expectedPickingLineAskBatchDisabledDialog = @"0,setSendHostFlag,""CODE"",1
1,loadMenuOptions,""MENU""
2,setCommandsConfirm,CONFIRM,CONFIRM,CONFIRM,CONFIRM,,,,,CONFIRM,CONFIRM
3,assignNum,""DLG"",1
4,assignStr,""CODE"",""A123""
5,label,""LABEL_START_A123""
6,assignStr,""STATUS""
7,assignStr,""PICKED""
8,assignStr,""SERVING""
9,assignStr,""WEIGHT""
10,assignStr,""STOCK""
11,assignStr,""DOCK""
12,assignStr,""BREAKAGE""
13,assignStr,""BATCH""
14,assignNum,""START_TIME"",""#time""
15,assignStr,""PRODUCT_DESCRIPTION"",""productname""
16,assignStr,""PRODUCT_NUMBER"",""productnumber""
17,assignStr,""UPC_NUMBER"",""upcnumber""
18,assignStr,""QTY_UPPER""
19,assignStr,""QTY_LOWER""
20,assignStr,""MAX_QTY_ALLOWED_PER_PICK""
21,assignStr,""TOTAL_PICKED""
22,assignStr,""TOTAL_WEIGHT""
23,assignStr,""CUSTOMER"",""Customer""
24,setCommands
25,say,""Message"",1,1
26,assignStr,""CURRENT_AISLE""
27,assignStr,""CURRENT_POSITION""
28,setCommands,LABEL_EXCEPTION_LOCATION_A123,LABEL_DOCK_A123,LABEL_BREAK_A123,,,There are 10 lines remaining,,,,,$CUSTOMER
29,doIf,""CURRENT_AISLE"",""Aisle"",""<>"",""STR"",,,,,,,,,,,1
30,assignStr,""CURRENT_AISLE"",""Aisle""
31,assignStr,""CURRENT_POSITION"",""Position""
32,say,""Aisle Aisle"",1,0
33,doEndIf,,,,,,,,,,,,,,,1
34,assignStr,""WHEREAMI"",""Aisle Aisle""
35,setCommand,4,""LABEL_SKIP_SLOT_A123"",CONFIRM
36,say,""Slot Slot"",1,0
37,concat,""WHEREAMI"",""WHEREAMI"","", Slot Slot""
38,setCommands,LABEL_EXCEPTION_CD_A123,LABEL_DOCK_A123,LABEL_BREAK_A123,LABEL_SKIP_SLOT_A123,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
39,getDigits,""DUMMY"",""Position"",,10,0,0,,""1"",""2"",,,1,""CD"",""Incorrect"",1,,,0
40,concat,""WHEREAMI"",""WHEREAMI"","", Position""
41,say,""$PRODUCT_DESCRIPTION"",0,0
42,label,""LABEL_ASK_BATCH_A123""
43,label,""LABEL_RESET_PICK_A123""
44,assignStr,""PROMPT"",""OriginalServingPrompt""
45,assignNum,""QTY_REQUESTED"",1
46,assignNum,""QTY_REQUESTED_ORIG"",1
47,assignStr,""TOTAL_PICKED""
48,assignStr,""TOTAL_WEIGHT""
49,assignNum,""QTY_UPPER"",1
50,assignNum,""QTY_LOWER"",1
51,assignNum,""MAX_QTY_ALLOWED_PER_PICK"",1
52,assignStr,""SERVING"",""OriginalServingCode""
53,label,""LABEL_PICK_A123""
54,setCommands,LABEL_EXCEPTION_A123,,,LABEL_SKIP_SLOT_A123,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,LABEL_BREAKAGE_A123,$CUSTOMER,LABEL_FORMAT_A123,LABEL_UNITS_A123,,$PRODUCT_NUMBER,$UPC_NUMBER
55,getDigits,""TOTAL_PICKED"",""$PROMPT"",""HelpMessage"",10,0,0,,""1"",""20"",""$QTY_LOWER"",""$QTY_UPPER"",0,,,1,,,0
56,doIf,""TOTAL_PICKED"",""QTY_REQUESTED"",""<"",""NUM"",,,,,,,,,,,2
57,concat,""PROMPT"",""Asked for "",""QTY_REQUESTED""
58,concat,""PROMPT"",""PROMPT"","", you said ""
59,concat,""PROMPT"",""PROMPT"",""TOTAL_PICKED""
60,concat,""PROMPT"",""PROMPT"","". Is this a short?""
61,ask,""DUMMY"",""$PROMPT"",,1,0,""1"",""0""
62,doIf,""DUMMY"",""0"",""="",""NUM"",,,,,,,,,,,3
63,goTo,""LABEL_RESET_PICK_A123""
64,doEndIf,,,,,,,,,,,,,,,3
65,setCommands,,,,,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
66,getMenu,""DUMMY"",""Reason code?"",,""LOWER_QUANTITY"",1,""?"",0,""Incorrect"",1
67,doIf,""DUMMY"",""1"",""="",""NUM"",,,,,,,,,,,4
68,assignStr,""STATUS"",""Empty""
69,doElseIf,""DUMMY"",""2"",""="",""NUM"",,,,,,,,,,,4
70,assignStr,""STATUS"",""Breakage""
71,doElseIf,""DUMMY"",""3"",""="",""NUM"",,,,,,,,,,,4
72,assignStr,""STATUS"",""Completed""
73,doElseIf,""DUMMY"",""4"",""="",""NUM"",,,,,,,,,,,4
74,assignStr,""STATUS"",""EndPallet""
75,doElseIf,""DUMMY"",""5"",""="",""NUM"",,,,,,,,,,,4
76,assignStr,""STATUS"",""OK""
77,doElseIf,""DUMMY"",""6"",""="",""NUM"",,,,,,,,,,,4
78,goTo,""LABEL_RESET_PICK_A123""
79,doEndIf,,,,,,,,,,,,,,,4
80,getDigits,""STOCK"",""Quantity in location"",,10,0,1,""?"",""1"",""20"",,,0,,,1,,,0
81,setSendHostFlag,""STOCK"",1
82,doElse,,,,,,,,,,,,,,,2
83,assignStr,""STATUS"",""OK""
84,doEndIf,,,,,,,,,,,,,,,2
85,label,""LABEL_WEIGHT_A123""
86,getFloat,""TOTAL_WEIGHT"",""Weight?"",,10,0,1,""?"",""1"",""20"",,,0,,,1,,,0
87,doIf,""TOTAL_WEIGHT"",""0"",""<>"",""NUM"",,,,,,,,,,,5
88,doIf,""TOTAL_WEIGHT"",""1.5"",""<"",""NUM"",,,,,,,,,,,6
89,say,""The weight is less than allowed"",0,0
90,goTo,""LABEL_WEIGHT_A123""
91,doEndIf,,,,,,,,,,,,,,,6
92,doIf,""TOTAL_WEIGHT"",""2.5"","">"",""NUM"",,,,,,,,,,,7
93,say,""The weight is more than allowed"",0,0
94,goTo,""LABEL_WEIGHT_A123""
95,doEndIf,,,,,,,,,,,,,,,7
96,doEndIf,,,,,,,,,,,,,,,5
97,setSendHostFlag,""TOTAL_WEIGHT"",1
98,label,""LABEL_CONFIRM_A123""
99,assignNum,""END_TIME"",""#time""
100,assignNum,""RESPONSETYPE"",1
101,setSendHostFlag,""STATUS"",1
102,setSendHostFlag,""START_TIME"",1
103,setSendHostFlag,""END_TIME"",1
104,setSendHostFlag,""TOTAL_PICKED"",1
105,setSendHostFlag,""SERVING"",1
106,setSendHostFlag,""RESPONSETYPE"",1
107,goTo,""LABEL_END_A123""
108,label,""LABEL_FORMAT_A123""
109,setCommands,LABEL_EXCEPTION_A123,,,,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
110,assignStr,""PROMPT"",""AlternativeServingPrompt""
111,assignNum,""QTY_REQUESTED"",1
112,assignNum,""QTY_REQUESTED_ORIG"",1
113,assignNum,""QTY_UPPER"",1
114,assignNum,""QTY_LOWER"",1
115,assignNum,""MAX_QTY_ALLOWED_PER_PICK"",1
116,assignStr,""SERVING"",""AlternativeServingCode""
117,goTo,""LABEL_PICK_A123""
118,label,""LABEL_BREAKAGE_A123""
119,getDigits,""BREAKAGE"",""Breakage quantity?"",,10,0,1,""?"",""1"",""20"",,,0,,,1,,,0
120,setSendHostFlag,""BREAKAGE"",1
121,goTo,""LABEL_PICK_A123""
122,label,""LABEL_UNITS_A123""
123,setCommands,LABEL_EXCEPTION_A123,,,,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
124,assignStr,""PROMPT"",""1 units""
125,assignNum,""QTY_REQUESTED"",1
126,assignNum,""QTY_REQUESTED_ORIG"",1
127,assignNum,""QTY_UPPER"",1
128,assignNum,""QTY_LOWER"",1
129,assignNum,""MAX_QTY_ALLOWED_PER_PICK"",1
130,assignStr,""SERVING"",""UNITS""
131,goTo,""LABEL_PICK_A123""
132,label,""LABEL_EXCEPTION_LOCATION_A123""
133,assignStr,""STATUS"",""BadLocation""
134,assignStr,""SERVING""
135,goTo,""LABEL_CONFIRM_A123""
136,label,""LABEL_EXCEPTION_CD_A123""
137,assignStr,""STATUS"",""NoCheck""
138,assignStr,""SERVING""
139,goTo,""LABEL_CONFIRM_A123""
140,label,""LABEL_EXCEPTION_A123""
141,assignStr,""STATUS"",""Cancelled""
142,assignStr,""SERVING""
143,goTo,""LABEL_CONFIRM_A123""
144,label,""LABEL_SKIP_SLOT_A123""
145,assignStr,""STATUS"",""Postponed""
146,assignStr,""SERVING""
147,goTo,""LABEL_CONFIRM_A123""
148,label,""LABEL_DOCK_A123""
149,assignNum,""RESPONSETYPE"",1
150,assignStr,""STATUS"",""EndPallet""
151,assignNum,""END_TIME"",""#time""
152,setSendHostFlag,""RESPONSETYPE"",1
153,setSendHostFlag,""STATUS"",1
154,setSendHostFlag,""START_TIME"",1
155,setSendHostFlag,""END_TIME"",1
156,goTo,""LABEL_END_A123""
157,label,""LABEL_BREAK_A123""
158,assignStr,""CURRENT_AISLE""
159,assignStr,""CURRENT_POSITION""
160,setCommands,,,,,,,,,LABEL_START_A123
161,getMenu,""BREAK_REASON"",""Break reason?"",,""BREAK"",1,""?"",0,,1
162,setSendHostFlag,""BREAK_REASON"",1
163,setSendHostFlag,""RESPONSETYPE"",1
164,assignNum,""RESPONSETYPE"",5
165,getVariablesOdr
166,setSendHostFlag,""BREAK_REASON"",0
167,setCommands
168,say,""At break. To resume work, say VCONFIRM"",1,0
169,assignNum,""RESPONSETYPE"",6
170,getVariablesOdr
171,goTo,""LABEL_START_A123""
172,label,""LABEL_END_A123""

";

        private readonly PickingLine pickingLineCDWithPositionNull = new("A123", "Message")
        {
            Customer = "Customer",
            Aisle = "Aisle",
            CD = "123",
            ProductName = "ProductName",
            ProductNumber = "ProductNumber",
            UpcNumber = "UpcNumber",
        };

        private readonly string expectedPickingLineCDWithPositionNullDialog = @"0,setSendHostFlag,""CODE"",1
1,loadMenuOptions,""MENU""
2,setCommandsConfirm,CONFIRM,CONFIRM,CONFIRM,CONFIRM,,,,,CONFIRM,CONFIRM
3,assignNum,""DLG"",1
4,assignStr,""CODE"",""A123""
5,label,""LABEL_START_A123""
6,assignStr,""STATUS""
7,assignStr,""PICKED""
8,assignStr,""SERVING""
9,assignStr,""WEIGHT""
10,assignStr,""STOCK""
11,assignStr,""DOCK""
12,assignStr,""BREAKAGE""
13,assignStr,""BATCH""
14,assignNum,""START_TIME"",""#time""
15,assignStr,""PRODUCT_DESCRIPTION"",""productname""
16,assignStr,""PRODUCT_NUMBER"",""productnumber""
17,assignStr,""UPC_NUMBER"",""upcnumber""
18,assignStr,""QTY_UPPER""
19,assignStr,""QTY_LOWER""
20,assignStr,""MAX_QTY_ALLOWED_PER_PICK""
21,assignStr,""TOTAL_PICKED""
22,assignStr,""TOTAL_WEIGHT""
23,assignStr,""CUSTOMER"",""Customer""
24,setCommands
25,say,""Message"",1,1
26,assignStr,""CURRENT_AISLE""
27,assignStr,""CURRENT_POSITION""
28,setCommands,LABEL_EXCEPTION_LOCATION_A123,LABEL_DOCK_A123,LABEL_BREAK_A123,,,Information not available,,,,,$CUSTOMER
29,doIf,""CURRENT_AISLE"",""Aisle"",""<>"",""STR"",,,,,,,,,,,1
30,assignStr,""CURRENT_AISLE"",""Aisle""
31,assignStr,""CURRENT_POSITION""
32,say,""Aisle Aisle"",1,0
33,doEndIf,,,,,,,,,,,,,,,1
34,assignStr,""WHEREAMI"",""Aisle Aisle""
35,setCommands,LABEL_EXCEPTION_CD_A123,LABEL_DOCK_A123,LABEL_BREAK_A123,LABEL_SKIP_SLOT_A123,,Information not available,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
36,getDigits,""DUMMY"",""Check digit?"",,10,0,0,,""1"",""3"",,,1,""123"",""Incorrect"",1,,,0
37,label,""LABEL_ASK_BATCH_A123""
38,label,""LABEL_RESET_PICK_A123""
39,assignStr,""STATUS"",""OK""
40,label,""LABEL_CONFIRM_A123""
41,assignNum,""END_TIME"",""#time""
42,assignNum,""RESPONSETYPE"",1
43,setSendHostFlag,""STATUS"",1
44,setSendHostFlag,""START_TIME"",1
45,setSendHostFlag,""END_TIME"",1
46,setSendHostFlag,""TOTAL_PICKED"",1
47,setSendHostFlag,""SERVING"",1
48,setSendHostFlag,""RESPONSETYPE"",1
49,goTo,""LABEL_END_A123""
50,label,""LABEL_FORMAT_A123""
51,say,""Not allowed"",0,0
52,goTo,""LABEL_PICK_A123""
53,label,""LABEL_BREAKAGE_A123""
54,getDigits,""BREAKAGE"",""Breakage quantity?"",,10,0,1,""?"",""1"",""20"",,,0,,,1,,,0
55,setSendHostFlag,""BREAKAGE"",1
56,goTo,""LABEL_PICK_A123""
57,label,""LABEL_UNITS_A123""
58,say,""Not allowed"",0,0
59,goTo,""LABEL_PICK_A123""
60,label,""LABEL_EXCEPTION_LOCATION_A123""
61,assignStr,""STATUS"",""BadLocation""
62,assignStr,""SERVING""
63,goTo,""LABEL_CONFIRM_A123""
64,label,""LABEL_EXCEPTION_CD_A123""
65,assignStr,""STATUS"",""NoCheck""
66,assignStr,""SERVING""
67,goTo,""LABEL_CONFIRM_A123""
68,label,""LABEL_EXCEPTION_A123""
69,assignStr,""STATUS"",""Cancelled""
70,assignStr,""SERVING""
71,goTo,""LABEL_CONFIRM_A123""
72,label,""LABEL_SKIP_SLOT_A123""
73,assignStr,""STATUS"",""Postponed""
74,assignStr,""SERVING""
75,goTo,""LABEL_CONFIRM_A123""
76,label,""LABEL_DOCK_A123""
77,assignNum,""RESPONSETYPE"",1
78,assignStr,""STATUS"",""EndPallet""
79,assignNum,""END_TIME"",""#time""
80,setSendHostFlag,""RESPONSETYPE"",1
81,setSendHostFlag,""STATUS"",1
82,setSendHostFlag,""START_TIME"",1
83,setSendHostFlag,""END_TIME"",1
84,goTo,""LABEL_END_A123""
85,label,""LABEL_BREAK_A123""
86,assignStr,""CURRENT_AISLE""
87,assignStr,""CURRENT_POSITION""
88,setCommands,,,,,,,,,LABEL_START_A123
89,getMenu,""BREAK_REASON"",""Break reason?"",,""BREAK"",1,""?"",0,,1
90,setSendHostFlag,""BREAK_REASON"",1
91,setSendHostFlag,""RESPONSETYPE"",1
92,assignNum,""RESPONSETYPE"",5
93,getVariablesOdr
94,setSendHostFlag,""BREAK_REASON"",0
95,setCommands
96,say,""At break. To resume work, say VCONFIRM"",1,0
97,assignNum,""RESPONSETYPE"",6
98,getVariablesOdr
99,goTo,""LABEL_START_A123""
100,label,""LABEL_END_A123""

";

        private readonly PickingLine pickingLineCanSkipAisle = new("A123", "Message")
        {
            Customer = "Customer",
            Aisle = "Aisle",
            Slot = "Slot",
            Position = "Position",
            CD = "CD",
            ProductName = "ProductName",
            ProductNumber = "ProductNumber",
            UpcNumber = "UpcNumber",
            OriginalServingCode = "OriginalServingCode",
            OriginalServingPrompt = "OriginalServingPrompt",
            OriginalServingQuantity = 1,
            OriginalServingUpperTolerance = 2.5m,
            OriginalServingLowerTolerance = 1.5m,
            OriginalMaxQuantityAllowedPerPick = 1,
            OriginalUnitsFormat = 1,
            AlternativeServingCode = "AlternativeServingCode",
            AlternativeServingPrompt = "AlternativeServingPrompt",
            AlternativeServingQuantity = 1,
            AlternativeServingUpperTolerance = 2.5m,
            AlternativeServingLowerTolerance = 1.5m,
            AlternativeMaxQuantityAllowedPerPick = 1,
            AlternativeUnitsFormat = 1,
            UnitsQuantity = 1,
            UnitsUpperTolerance = 1.5m,
            UnitsLowerTolerance = 2.5m,
            UnitsMaxQuantityAllowedPerPick = 1,
            AskWeight = true,
            WeightMin = 1.5m,
            WeightMax = 2.5m,
            HowMuchMore = 10,
            StockCounting = StockCountingMode.Always,
            SpeakProductName = false,
            AskBatch = false,
            Batches = new string[] { "123", "456" },
            CanSkipAisle = true,
            HelpMsg = "HelpMessage",
            CountdownPick = false,
            ProductCDs = new string[] { "1234", "567" },
        };

        private readonly string expectedPickingLineCanSkipAisleDialog = @"0,setSendHostFlag,""CODE"",1
1,loadMenuOptions,""MENU""
2,setCommandsConfirm,CONFIRM,CONFIRM,CONFIRM,CONFIRM,,,,,CONFIRM,CONFIRM
3,assignNum,""DLG"",1
4,assignStr,""CODE"",""A123""
5,label,""LABEL_START_A123""
6,assignStr,""STATUS""
7,assignStr,""PICKED""
8,assignStr,""SERVING""
9,assignStr,""WEIGHT""
10,assignStr,""STOCK""
11,assignStr,""DOCK""
12,assignStr,""BREAKAGE""
13,assignStr,""BATCH""
14,assignNum,""START_TIME"",""#time""
15,assignStr,""PRODUCT_DESCRIPTION"",""productname""
16,assignStr,""PRODUCT_NUMBER"",""productnumber""
17,assignStr,""UPC_NUMBER"",""upcnumber""
18,assignStr,""QTY_UPPER""
19,assignStr,""QTY_LOWER""
20,assignStr,""MAX_QTY_ALLOWED_PER_PICK""
21,assignStr,""TOTAL_PICKED""
22,assignStr,""TOTAL_WEIGHT""
23,assignStr,""CUSTOMER"",""Customer""
24,setCommands
25,say,""Message"",1,1
26,assignStr,""CURRENT_AISLE""
27,assignStr,""CURRENT_POSITION""
28,setCommands,LABEL_EXCEPTION_LOCATION_A123,LABEL_DOCK_A123,LABEL_BREAK_A123,,,There are 10 lines remaining,,,,,$CUSTOMER
29,setCommand,14,""LABEL_SKIP_AISLE_A123"",CONFIRM
30,doIf,""CURRENT_AISLE"",""Aisle"",""<>"",""STR"",,,,,,,,,,,1
31,assignNum,""SKIPPING_AISLE"",0
32,assignStr,""CURRENT_AISLE"",""Aisle""
33,assignStr,""CURRENT_POSITION"",""Position""
34,say,""Aisle Aisle"",1,0
35,doElseIf,""SKIPPING_AISLE"",""1"",""="",""NUM"",,,,,,,,,,,1
36,doIf,""CURRENT_POSITION"",""Position"",""<>"",""STR"",,,,,,,,,,,2
37,goTo,""LABEL_SKIP_SLOT_A123""
38,doElse,,,,,,,,,,,,,,,2
39,assignNum,""SKIPPING_AISLE"",0
40,say,""Aisle Aisle"",1,0
41,doEndIf,,,,,,,,,,,,,,,2
42,doEndIf,,,,,,,,,,,,,,,1
43,assignStr,""WHEREAMI"",""Aisle Aisle""
44,setCommand,4,""LABEL_SKIP_SLOT_A123"",CONFIRM
45,say,""Slot Slot"",1,0
46,concat,""WHEREAMI"",""WHEREAMI"","", Slot Slot""
47,setCommands,LABEL_EXCEPTION_CD_A123,LABEL_DOCK_A123,LABEL_BREAK_A123,LABEL_SKIP_SLOT_A123,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
48,setCommand,14,""LABEL_SKIP_AISLE_A123"",CONFIRM
49,getDigits,""DUMMY"",""Position"",,10,0,0,,""1"",""2"",,,1,""CD"",""Incorrect"",1,,,0
50,concat,""WHEREAMI"",""WHEREAMI"","", Position""
51,say,""$PRODUCT_DESCRIPTION"",0,0
52,label,""LABEL_VALIDATE_PRODUCT_CD_A123""
53,getDigits,""PRODUCT_CD"",""Check digit?"",,10,0,0,,""1"",""20"",,,1,,,1,,,0
54,doIf,""PRODUCT_CD"",""1234"",""="",""STR"",,,,,,,,,,,3
55,setSendHostFlag,""PRODUCT_CD"",1
56,goTo,""LABEL_ASK_BATCH_A123""
57,doEndIf,,,,,,,,,,,,,,,3
58,doIf,""PRODUCT_CD"",""567"",""="",""STR"",,,,,,,,,,,4
59,setSendHostFlag,""PRODUCT_CD"",1
60,goTo,""LABEL_ASK_BATCH_A123""
61,doEndIf,,,,,,,,,,,,,,,4
62,concat,""DUMMY"",""Incorrect, "",""PRODUCT_CD""
63,say,""$DUMMY"",0,0
64,goTo,""LABEL_VALIDATE_PRODUCT_CD_A123""
65,label,""LABEL_ASK_BATCH_A123""
66,label,""LABEL_RESET_PICK_A123""
67,assignStr,""PROMPT"",""OriginalServingPrompt""
68,assignNum,""QTY_REQUESTED"",1
69,assignNum,""QTY_REQUESTED_ORIG"",1
70,assignStr,""TOTAL_PICKED""
71,assignStr,""TOTAL_WEIGHT""
72,assignNum,""QTY_UPPER"",1
73,assignNum,""QTY_LOWER"",1
74,assignNum,""MAX_QTY_ALLOWED_PER_PICK"",1
75,assignStr,""SERVING"",""OriginalServingCode""
76,label,""LABEL_PICK_A123""
77,setCommands,LABEL_EXCEPTION_A123,,,LABEL_SKIP_SLOT_A123,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,LABEL_BREAKAGE_A123,$CUSTOMER,LABEL_FORMAT_A123,LABEL_UNITS_A123,,$PRODUCT_NUMBER,$UPC_NUMBER
78,getDigits,""TOTAL_PICKED"",""$PROMPT"",""HelpMessage"",10,0,0,,""1"",""20"",""$QTY_LOWER"",""$QTY_UPPER"",0,,,1,,,0
79,doIf,""TOTAL_PICKED"",""QTY_REQUESTED"",""<"",""NUM"",,,,,,,,,,,5
80,concat,""PROMPT"",""Asked for "",""QTY_REQUESTED""
81,concat,""PROMPT"",""PROMPT"","", you said ""
82,concat,""PROMPT"",""PROMPT"",""TOTAL_PICKED""
83,concat,""PROMPT"",""PROMPT"","". Is this a short?""
84,ask,""DUMMY"",""$PROMPT"",,1,0,""1"",""0""
85,doIf,""DUMMY"",""0"",""="",""NUM"",,,,,,,,,,,6
86,goTo,""LABEL_RESET_PICK_A123""
87,doEndIf,,,,,,,,,,,,,,,6
88,setCommands,,,,,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
89,getMenu,""DUMMY"",""Reason code?"",,""LOWER_QUANTITY"",1,""?"",0,""Incorrect"",1
90,doIf,""DUMMY"",""1"",""="",""NUM"",,,,,,,,,,,7
91,assignStr,""STATUS"",""Empty""
92,doElseIf,""DUMMY"",""2"",""="",""NUM"",,,,,,,,,,,7
93,assignStr,""STATUS"",""Breakage""
94,doElseIf,""DUMMY"",""3"",""="",""NUM"",,,,,,,,,,,7
95,assignStr,""STATUS"",""Completed""
96,doElseIf,""DUMMY"",""4"",""="",""NUM"",,,,,,,,,,,7
97,assignStr,""STATUS"",""EndPallet""
98,doElseIf,""DUMMY"",""5"",""="",""NUM"",,,,,,,,,,,7
99,assignStr,""STATUS"",""OK""
100,doElseIf,""DUMMY"",""6"",""="",""NUM"",,,,,,,,,,,7
101,goTo,""LABEL_RESET_PICK_A123""
102,doEndIf,,,,,,,,,,,,,,,7
103,doElse,,,,,,,,,,,,,,,5
104,assignStr,""STATUS"",""OK""
105,doEndIf,,,,,,,,,,,,,,,5
106,label,""LABEL_WEIGHT_A123""
107,getFloat,""TOTAL_WEIGHT"",""Weight?"",,10,0,1,""?"",""1"",""20"",,,0,,,1,,,0
108,doIf,""TOTAL_WEIGHT"",""0"",""<>"",""NUM"",,,,,,,,,,,8
109,doIf,""TOTAL_WEIGHT"",""1.5"",""<"",""NUM"",,,,,,,,,,,9
110,say,""The weight is less than allowed"",0,0
111,goTo,""LABEL_WEIGHT_A123""
112,doEndIf,,,,,,,,,,,,,,,9
113,doIf,""TOTAL_WEIGHT"",""2.5"","">"",""NUM"",,,,,,,,,,,10
114,say,""The weight is more than allowed"",0,0
115,goTo,""LABEL_WEIGHT_A123""
116,doEndIf,,,,,,,,,,,,,,,10
117,doEndIf,,,,,,,,,,,,,,,8
118,setSendHostFlag,""TOTAL_WEIGHT"",1
119,setCommands,,,,,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
120,getDigits,""STOCK"",""Quantity in location"",,10,0,1,""?"",""1"",""20"",,,0,,,1,,,0
121,setSendHostFlag,""STOCK"",1
122,label,""LABEL_CONFIRM_A123""
123,assignNum,""END_TIME"",""#time""
124,assignNum,""RESPONSETYPE"",1
125,setSendHostFlag,""STATUS"",1
126,setSendHostFlag,""START_TIME"",1
127,setSendHostFlag,""END_TIME"",1
128,setSendHostFlag,""TOTAL_PICKED"",1
129,setSendHostFlag,""SERVING"",1
130,setSendHostFlag,""RESPONSETYPE"",1
131,goTo,""LABEL_END_A123""
132,label,""LABEL_FORMAT_A123""
133,setCommands,LABEL_EXCEPTION_A123,,,,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
134,assignStr,""PROMPT"",""AlternativeServingPrompt""
135,assignNum,""QTY_REQUESTED"",1
136,assignNum,""QTY_REQUESTED_ORIG"",1
137,assignNum,""QTY_UPPER"",1
138,assignNum,""QTY_LOWER"",1
139,assignNum,""MAX_QTY_ALLOWED_PER_PICK"",1
140,assignStr,""SERVING"",""AlternativeServingCode""
141,goTo,""LABEL_PICK_A123""
142,label,""LABEL_BREAKAGE_A123""
143,getDigits,""BREAKAGE"",""Breakage quantity?"",,10,0,1,""?"",""1"",""20"",,,0,,,1,,,0
144,setSendHostFlag,""BREAKAGE"",1
145,goTo,""LABEL_PICK_A123""
146,label,""LABEL_UNITS_A123""
147,setCommands,LABEL_EXCEPTION_A123,,,,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
148,assignStr,""PROMPT"",""1 units""
149,assignNum,""QTY_REQUESTED"",1
150,assignNum,""QTY_REQUESTED_ORIG"",1
151,assignNum,""QTY_UPPER"",1
152,assignNum,""QTY_LOWER"",1
153,assignNum,""MAX_QTY_ALLOWED_PER_PICK"",1
154,assignStr,""SERVING"",""UNITS""
155,goTo,""LABEL_PICK_A123""
156,label,""LABEL_EXCEPTION_LOCATION_A123""
157,assignStr,""STATUS"",""BadLocation""
158,assignStr,""SERVING""
159,goTo,""LABEL_CONFIRM_A123""
160,label,""LABEL_EXCEPTION_CD_A123""
161,assignStr,""STATUS"",""NoCheck""
162,assignStr,""SERVING""
163,goTo,""LABEL_CONFIRM_A123""
164,label,""LABEL_EXCEPTION_A123""
165,assignStr,""STATUS"",""Cancelled""
166,assignStr,""SERVING""
167,goTo,""LABEL_CONFIRM_A123""
168,label,""LABEL_SKIP_AISLE_A123""
169,assignNum,""SKIPPING_AISLE"",1
170,goTo,""LABEL_SKIP_SLOT_A123""
171,label,""LABEL_SKIP_SLOT_A123""
172,assignStr,""STATUS"",""Postponed""
173,assignStr,""SERVING""
174,goTo,""LABEL_CONFIRM_A123""
175,label,""LABEL_DOCK_A123""
176,assignNum,""RESPONSETYPE"",1
177,assignStr,""STATUS"",""EndPallet""
178,assignNum,""END_TIME"",""#time""
179,setSendHostFlag,""RESPONSETYPE"",1
180,setSendHostFlag,""STATUS"",1
181,setSendHostFlag,""START_TIME"",1
182,setSendHostFlag,""END_TIME"",1
183,goTo,""LABEL_END_A123""
184,label,""LABEL_BREAK_A123""
185,assignStr,""CURRENT_AISLE""
186,assignStr,""CURRENT_POSITION""
187,setCommands,,,,,,,,,LABEL_START_A123
188,getMenu,""BREAK_REASON"",""Break reason?"",,""BREAK"",1,""?"",0,,1
189,setSendHostFlag,""BREAK_REASON"",1
190,setSendHostFlag,""RESPONSETYPE"",1
191,assignNum,""RESPONSETYPE"",5
192,getVariablesOdr
193,setSendHostFlag,""BREAK_REASON"",0
194,setCommands
195,say,""At break. To resume work, say VCONFIRM"",1,0
196,assignNum,""RESPONSETYPE"",6
197,getVariablesOdr
198,goTo,""LABEL_START_A123""
199,label,""LABEL_END_A123""

";

        private readonly PickingLine pickingLineStockCountingAlways = new("A123", "Message")
        {
            Customer = "Customer",
            Aisle = "Aisle",
            Slot = "Slot",
            Position = "Position",
            CD = "CD",
            ProductName = "ProductName",
            ProductNumber = "ProductNumber",
            UpcNumber = "UpcNumber",
            OriginalServingCode = "OriginalServingCode",
            OriginalServingPrompt = "OriginalServingPrompt",
            OriginalServingQuantity = 1,
            OriginalServingUpperTolerance = 2.5m,
            OriginalServingLowerTolerance = 1.5m,
            OriginalMaxQuantityAllowedPerPick = 1,
            OriginalUnitsFormat = 1,
            AlternativeServingCode = "AlternativeServingCode",
            AlternativeServingPrompt = "AlternativeServingPrompt",
            AlternativeServingQuantity = 1,
            AlternativeServingUpperTolerance = 2.5m,
            AlternativeServingLowerTolerance = 1.5m,
            AlternativeMaxQuantityAllowedPerPick = 1,
            AlternativeUnitsFormat = 1,
            UnitsQuantity = 1,
            UnitsUpperTolerance = 1.5m,
            UnitsLowerTolerance = 2.5m,
            UnitsMaxQuantityAllowedPerPick = 1,
            AskWeight = true,
            WeightMin = 1.5m,
            WeightMax = 2.5m,
            HowMuchMore = 10,
            StockCounting = StockCountingMode.Always,
            SpeakProductName = false,
            AskBatch = false,
            Batches = new string[] { "123", "456" },
            CanSkipAisle = false,
            HelpMsg = "HelpMessage",
            CountdownPick = false,
            ProductCDs = new string[] { "1234", "567" },
        };

        private readonly string expectedPickingLineStockCountingAlwaysDialog = @"0,setSendHostFlag,""CODE"",1
1,loadMenuOptions,""MENU""
2,setCommandsConfirm,CONFIRM,CONFIRM,CONFIRM,CONFIRM,,,,,CONFIRM,CONFIRM
3,assignNum,""DLG"",1
4,assignStr,""CODE"",""A123""
5,label,""LABEL_START_A123""
6,assignStr,""STATUS""
7,assignStr,""PICKED""
8,assignStr,""SERVING""
9,assignStr,""WEIGHT""
10,assignStr,""STOCK""
11,assignStr,""DOCK""
12,assignStr,""BREAKAGE""
13,assignStr,""BATCH""
14,assignNum,""START_TIME"",""#time""
15,assignStr,""PRODUCT_DESCRIPTION"",""productname""
16,assignStr,""PRODUCT_NUMBER"",""productnumber""
17,assignStr,""UPC_NUMBER"",""upcnumber""
18,assignStr,""QTY_UPPER""
19,assignStr,""QTY_LOWER""
20,assignStr,""MAX_QTY_ALLOWED_PER_PICK""
21,assignStr,""TOTAL_PICKED""
22,assignStr,""TOTAL_WEIGHT""
23,assignStr,""CUSTOMER"",""Customer""
24,setCommands
25,say,""Message"",1,1
26,assignStr,""CURRENT_AISLE""
27,assignStr,""CURRENT_POSITION""
28,setCommands,LABEL_EXCEPTION_LOCATION_A123,LABEL_DOCK_A123,LABEL_BREAK_A123,,,There are 10 lines remaining,,,,,$CUSTOMER
29,doIf,""CURRENT_AISLE"",""Aisle"",""<>"",""STR"",,,,,,,,,,,1
30,assignStr,""CURRENT_AISLE"",""Aisle""
31,assignStr,""CURRENT_POSITION"",""Position""
32,say,""Aisle Aisle"",1,0
33,doEndIf,,,,,,,,,,,,,,,1
34,assignStr,""WHEREAMI"",""Aisle Aisle""
35,setCommand,4,""LABEL_SKIP_SLOT_A123"",CONFIRM
36,say,""Slot Slot"",1,0
37,concat,""WHEREAMI"",""WHEREAMI"","", Slot Slot""
38,setCommands,LABEL_EXCEPTION_CD_A123,LABEL_DOCK_A123,LABEL_BREAK_A123,LABEL_SKIP_SLOT_A123,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
39,getDigits,""DUMMY"",""Position"",,10,0,0,,""1"",""2"",,,1,""CD"",""Incorrect"",1,,,0
40,concat,""WHEREAMI"",""WHEREAMI"","", Position""
41,say,""$PRODUCT_DESCRIPTION"",0,0
42,label,""LABEL_VALIDATE_PRODUCT_CD_A123""
43,getDigits,""PRODUCT_CD"",""Check digit?"",,10,0,0,,""1"",""20"",,,1,,,1,,,0
44,doIf,""PRODUCT_CD"",""1234"",""="",""STR"",,,,,,,,,,,2
45,setSendHostFlag,""PRODUCT_CD"",1
46,goTo,""LABEL_ASK_BATCH_A123""
47,doEndIf,,,,,,,,,,,,,,,2
48,doIf,""PRODUCT_CD"",""567"",""="",""STR"",,,,,,,,,,,3
49,setSendHostFlag,""PRODUCT_CD"",1
50,goTo,""LABEL_ASK_BATCH_A123""
51,doEndIf,,,,,,,,,,,,,,,3
52,concat,""DUMMY"",""Incorrect, "",""PRODUCT_CD""
53,say,""$DUMMY"",0,0
54,goTo,""LABEL_VALIDATE_PRODUCT_CD_A123""
55,label,""LABEL_ASK_BATCH_A123""
56,label,""LABEL_RESET_PICK_A123""
57,assignStr,""PROMPT"",""OriginalServingPrompt""
58,assignNum,""QTY_REQUESTED"",1
59,assignNum,""QTY_REQUESTED_ORIG"",1
60,assignStr,""TOTAL_PICKED""
61,assignStr,""TOTAL_WEIGHT""
62,assignNum,""QTY_UPPER"",1
63,assignNum,""QTY_LOWER"",1
64,assignNum,""MAX_QTY_ALLOWED_PER_PICK"",1
65,assignStr,""SERVING"",""OriginalServingCode""
66,label,""LABEL_PICK_A123""
67,setCommands,LABEL_EXCEPTION_A123,,,LABEL_SKIP_SLOT_A123,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,LABEL_BREAKAGE_A123,$CUSTOMER,LABEL_FORMAT_A123,LABEL_UNITS_A123,,$PRODUCT_NUMBER,$UPC_NUMBER
68,getDigits,""TOTAL_PICKED"",""$PROMPT"",""HelpMessage"",10,0,0,,""1"",""20"",""$QTY_LOWER"",""$QTY_UPPER"",0,,,1,,,0
69,doIf,""TOTAL_PICKED"",""QTY_REQUESTED"",""<"",""NUM"",,,,,,,,,,,4
70,concat,""PROMPT"",""Asked for "",""QTY_REQUESTED""
71,concat,""PROMPT"",""PROMPT"","", you said ""
72,concat,""PROMPT"",""PROMPT"",""TOTAL_PICKED""
73,concat,""PROMPT"",""PROMPT"","". Is this a short?""
74,ask,""DUMMY"",""$PROMPT"",,1,0,""1"",""0""
75,doIf,""DUMMY"",""0"",""="",""NUM"",,,,,,,,,,,5
76,goTo,""LABEL_RESET_PICK_A123""
77,doEndIf,,,,,,,,,,,,,,,5
78,setCommands,,,,,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
79,getMenu,""DUMMY"",""Reason code?"",,""LOWER_QUANTITY"",1,""?"",0,""Incorrect"",1
80,doIf,""DUMMY"",""1"",""="",""NUM"",,,,,,,,,,,6
81,assignStr,""STATUS"",""Empty""
82,doElseIf,""DUMMY"",""2"",""="",""NUM"",,,,,,,,,,,6
83,assignStr,""STATUS"",""Breakage""
84,doElseIf,""DUMMY"",""3"",""="",""NUM"",,,,,,,,,,,6
85,assignStr,""STATUS"",""Completed""
86,doElseIf,""DUMMY"",""4"",""="",""NUM"",,,,,,,,,,,6
87,assignStr,""STATUS"",""EndPallet""
88,doElseIf,""DUMMY"",""5"",""="",""NUM"",,,,,,,,,,,6
89,assignStr,""STATUS"",""OK""
90,doElseIf,""DUMMY"",""6"",""="",""NUM"",,,,,,,,,,,6
91,goTo,""LABEL_RESET_PICK_A123""
92,doEndIf,,,,,,,,,,,,,,,6
93,doElse,,,,,,,,,,,,,,,4
94,assignStr,""STATUS"",""OK""
95,doEndIf,,,,,,,,,,,,,,,4
96,label,""LABEL_WEIGHT_A123""
97,getFloat,""TOTAL_WEIGHT"",""Weight?"",,10,0,1,""?"",""1"",""20"",,,0,,,1,,,0
98,doIf,""TOTAL_WEIGHT"",""0"",""<>"",""NUM"",,,,,,,,,,,7
99,doIf,""TOTAL_WEIGHT"",""1.5"",""<"",""NUM"",,,,,,,,,,,8
100,say,""The weight is less than allowed"",0,0
101,goTo,""LABEL_WEIGHT_A123""
102,doEndIf,,,,,,,,,,,,,,,8
103,doIf,""TOTAL_WEIGHT"",""2.5"","">"",""NUM"",,,,,,,,,,,9
104,say,""The weight is more than allowed"",0,0
105,goTo,""LABEL_WEIGHT_A123""
106,doEndIf,,,,,,,,,,,,,,,9
107,doEndIf,,,,,,,,,,,,,,,7
108,setSendHostFlag,""TOTAL_WEIGHT"",1
109,setCommands,,,,,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
110,getDigits,""STOCK"",""Quantity in location"",,10,0,1,""?"",""1"",""20"",,,0,,,1,,,0
111,setSendHostFlag,""STOCK"",1
112,label,""LABEL_CONFIRM_A123""
113,assignNum,""END_TIME"",""#time""
114,assignNum,""RESPONSETYPE"",1
115,setSendHostFlag,""STATUS"",1
116,setSendHostFlag,""START_TIME"",1
117,setSendHostFlag,""END_TIME"",1
118,setSendHostFlag,""TOTAL_PICKED"",1
119,setSendHostFlag,""SERVING"",1
120,setSendHostFlag,""RESPONSETYPE"",1
121,goTo,""LABEL_END_A123""
122,label,""LABEL_FORMAT_A123""
123,setCommands,LABEL_EXCEPTION_A123,,,,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
124,assignStr,""PROMPT"",""AlternativeServingPrompt""
125,assignNum,""QTY_REQUESTED"",1
126,assignNum,""QTY_REQUESTED_ORIG"",1
127,assignNum,""QTY_UPPER"",1
128,assignNum,""QTY_LOWER"",1
129,assignNum,""MAX_QTY_ALLOWED_PER_PICK"",1
130,assignStr,""SERVING"",""AlternativeServingCode""
131,goTo,""LABEL_PICK_A123""
132,label,""LABEL_BREAKAGE_A123""
133,getDigits,""BREAKAGE"",""Breakage quantity?"",,10,0,1,""?"",""1"",""20"",,,0,,,1,,,0
134,setSendHostFlag,""BREAKAGE"",1
135,goTo,""LABEL_PICK_A123""
136,label,""LABEL_UNITS_A123""
137,setCommands,LABEL_EXCEPTION_A123,,,,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
138,assignStr,""PROMPT"",""1 units""
139,assignNum,""QTY_REQUESTED"",1
140,assignNum,""QTY_REQUESTED_ORIG"",1
141,assignNum,""QTY_UPPER"",1
142,assignNum,""QTY_LOWER"",1
143,assignNum,""MAX_QTY_ALLOWED_PER_PICK"",1
144,assignStr,""SERVING"",""UNITS""
145,goTo,""LABEL_PICK_A123""
146,label,""LABEL_EXCEPTION_LOCATION_A123""
147,assignStr,""STATUS"",""BadLocation""
148,assignStr,""SERVING""
149,goTo,""LABEL_CONFIRM_A123""
150,label,""LABEL_EXCEPTION_CD_A123""
151,assignStr,""STATUS"",""NoCheck""
152,assignStr,""SERVING""
153,goTo,""LABEL_CONFIRM_A123""
154,label,""LABEL_EXCEPTION_A123""
155,assignStr,""STATUS"",""Cancelled""
156,assignStr,""SERVING""
157,goTo,""LABEL_CONFIRM_A123""
158,label,""LABEL_SKIP_SLOT_A123""
159,assignStr,""STATUS"",""Postponed""
160,assignStr,""SERVING""
161,goTo,""LABEL_CONFIRM_A123""
162,label,""LABEL_DOCK_A123""
163,assignNum,""RESPONSETYPE"",1
164,assignStr,""STATUS"",""EndPallet""
165,assignNum,""END_TIME"",""#time""
166,setSendHostFlag,""RESPONSETYPE"",1
167,setSendHostFlag,""STATUS"",1
168,setSendHostFlag,""START_TIME"",1
169,setSendHostFlag,""END_TIME"",1
170,goTo,""LABEL_END_A123""
171,label,""LABEL_BREAK_A123""
172,assignStr,""CURRENT_AISLE""
173,assignStr,""CURRENT_POSITION""
174,setCommands,,,,,,,,,LABEL_START_A123
175,getMenu,""BREAK_REASON"",""Break reason?"",,""BREAK"",1,""?"",0,,1
176,setSendHostFlag,""BREAK_REASON"",1
177,setSendHostFlag,""RESPONSETYPE"",1
178,assignNum,""RESPONSETYPE"",5
179,getVariablesOdr
180,setSendHostFlag,""BREAK_REASON"",0
181,setCommands
182,say,""At break. To resume work, say VCONFIRM"",1,0
183,assignNum,""RESPONSETYPE"",6
184,getVariablesOdr
185,goTo,""LABEL_START_A123""
186,label,""LABEL_END_A123""

";

        private readonly PrintLabels printLabelsWithoutOptionalParams = new("A123", string.Empty);

        private readonly string expectedPrintLabelsWithoutOptionalParamsDialog = @"0,setSendHostFlag,""CODE"",1
1,loadMenuOptions,""MENU""
2,setCommandsConfirm,CONFIRM,CONFIRM,CONFIRM,CONFIRM,,,,,CONFIRM,CONFIRM
3,assignNum,""DLG"",1
4,assignStr,""CODE"",""A123""
5,assignStr,""STATUS""
6,assignNum,""START_TIME"",""#time""
7,assignStr,""PRINTER""
8,label,""LABEL_SELECT_LABELS_A123""
9,setCommands,,,,,LABEL_SELECT_PRINTER_A123,,,,LABEL_CANCEL_A123
10,doIf,""PRINTER"",,""="",""STR"",,,,,,,,,,,1
11,concat,""PROMPT"",""Printer"",""unknown""
12,doElse,,,,,,,,,,,,,,,1
13,concat,""PROMPT"",""Printer"",""PRINTER""
14,doEndIf,,,,,,,,,,,,,,,1
15,concat,""PROMPT"",""PROMPT"","", How many labels?""
16,getDigits,""LABELS"",""$PROMPT"",,10,0,1,""?"",""1"",""2"",,,0,,,1,,,0
17,setSendHostFlag,""RESPONSETYPE"",1
18,setSendHostFlag,""STATUS"",1
19,setSendHostFlag,""START_TIME"",1
20,setSendHostFlag,""END_TIME"",1
21,setSendHostFlag,""LABELS"",1
22,setSendHostFlag,""PRINTER"",1
23,assignStr,""STATUS"",""OK""
24,goTo,""LABEL_END_A123""
25,label,""LABEL_SELECT_PRINTER_A123""
26,setCommands,,,,,,,,,LABEL_SELECT_LABELS_A123
27,say,""Printers not available"",0,0
28,goTo,""LABEL_SELECT_LABELS_A123""
29,label,""LABEL_CANCEL_A123""
30,assignStr,""STATUS"",""Cancelled""
31,label,""LABEL_END_A123""
32,assignNum,""RESPONSETYPE"",2
33,assignNum,""END_TIME"",""#time""

";

        private readonly PrintLabels printLabels = new("A123", string.Empty)
        {
            Copies = 1,
            DefaultPrinter = 2,
            Printers = new[] { 1, 2, 3 }
        };

        private readonly string expectedPrintLabelsDialog = @"0,setSendHostFlag,""CODE"",1
1,loadMenuOptions,""MENU""
2,setCommandsConfirm,CONFIRM,CONFIRM,CONFIRM,CONFIRM,,,,,CONFIRM,CONFIRM
3,assignNum,""DLG"",1
4,assignStr,""CODE"",""A123""
5,assignStr,""STATUS""
6,assignNum,""START_TIME"",""#time""
7,assignNum,""PRINTER"",2
8,label,""LABEL_SELECT_LABELS_A123""
9,setCommands,,,,,LABEL_SELECT_PRINTER_A123,,,,LABEL_CANCEL_A123
10,doIf,""PRINTER"",,""="",""STR"",,,,,,,,,,,1
11,concat,""PROMPT"",""Printer"",""unknown""
12,doElse,,,,,,,,,,,,,,,1
13,concat,""PROMPT"",""Printer"",""PRINTER""
14,doEndIf,,,,,,,,,,,,,,,1
15,concat,""PROMPT"",""PROMPT"","", to print, say VCONFIRM""
16,say,""$PROMPT"",1,0
17,assignNum,""LABELS"",1
18,setSendHostFlag,""RESPONSETYPE"",1
19,setSendHostFlag,""STATUS"",1
20,setSendHostFlag,""START_TIME"",1
21,setSendHostFlag,""END_TIME"",1
22,setSendHostFlag,""LABELS"",1
23,setSendHostFlag,""PRINTER"",1
24,assignStr,""STATUS"",""OK""
25,goTo,""LABEL_END_A123""
26,label,""LABEL_SELECT_PRINTER_A123""
27,setCommands,,,,,,,,,LABEL_SELECT_LABELS_A123
28,assignNum,""PRINTER_1_CODE"",1
29,assignNum,""PRINTER_2_CODE"",2
30,assignNum,""PRINTER_3_CODE"",3
31,label,""LABEL_PRINTER_A123""
32,getDigits,""PRINTER"",""Printer?"",,10,0,0,,""1"",""2"",,,0,,,1,,,0
33,doIf,""PRINTER"",""PRINTER_1_CODE"",""="",""NUM"",,,,,,,,,,,2
34,goTo,""LABEL_SELECT_LABELS_A123""
35,doEndIf,,,,,,,,,,,,,,,2
36,doIf,""PRINTER"",""PRINTER_2_CODE"",""="",""NUM"",,,,,,,,,,,3
37,goTo,""LABEL_SELECT_LABELS_A123""
38,doEndIf,,,,,,,,,,,,,,,3
39,doIf,""PRINTER"",""PRINTER_3_CODE"",""="",""NUM"",,,,,,,,,,,4
40,goTo,""LABEL_SELECT_LABELS_A123""
41,doEndIf,,,,,,,,,,,,,,,4
42,say,""Unknown printer"",0,0
43,goTo,""LABEL_PRINTER_A123""
44,label,""LABEL_CANCEL_A123""
45,assignStr,""STATUS"",""Cancelled""
46,label,""LABEL_END_A123""
47,assignNum,""RESPONSETYPE"",2
48,assignNum,""END_TIME"",""#time""

";

        private readonly ValidatePrinting validatePrintingWithoutOptionalParams = new("A123", string.Empty);

        private readonly string expectedValidatePrintingWithoutOptionalParamsDialog = @"0,setSendHostFlag,""CODE"",1
1,loadMenuOptions,""MENU""
2,setCommandsConfirm,CONFIRM,CONFIRM,CONFIRM,CONFIRM,,,,,CONFIRM,CONFIRM
3,assignNum,""DLG"",1
4,assignStr,""CODE"",""A123""
5,setCommands,LABEL_EXCEPTION_A123
6,assignNum,""START_TIME"",""#time""
7,ask,""DUMMY"",""Correct printing?"",,1,0,""1"",""0""
8,doIf,""DUMMY"",""0"",""="",""NUM"",,,,,,,,,,,1
9,say,""Retrying..."",0,0
10,assignStr,""STATUS"",""Retry""
11,goTo,""LABEL_END_A123""
12,doEndIf,,,,,,,,,,,,,,,1
13,setCommands
14,assignStr,""STATUS"",""OK""
15,goTo,""LABEL_END_A123""
16,label,""LABEL_EXCEPTION_A123""
17,assignStr,""STATUS"",""Cancelled""
18,label,""LABEL_END_A123""
19,assignNum,""RESPONSETYPE"",3
20,setSendHostFlag,""RESPONSETYPE"",1
21,setSendHostFlag,""STATUS"",1
22,setSendHostFlag,""START_TIME"",1
23,setSendHostFlag,""END_TIME"",1
24,assignNum,""END_TIME"",""#time""

";

        private readonly ValidatePrinting validatePrinting = new("A123", string.Empty)
        {
            ValidationCodes = new[] { "123", "456" },
            VoiceLength = 3,
        };

        private readonly string expectedValidatePrintingDialog = @"0,setSendHostFlag,""CODE"",1
1,loadMenuOptions,""MENU""
2,setCommandsConfirm,CONFIRM,CONFIRM,CONFIRM,CONFIRM,,,,,CONFIRM,CONFIRM
3,assignNum,""DLG"",1
4,assignStr,""CODE"",""A123""
5,setCommands,LABEL_EXCEPTION_A123
6,assignNum,""START_TIME"",""#time""
7,ask,""DUMMY"",""Correct printing?"",,1,0,""1"",""0""
8,doIf,""DUMMY"",""0"",""="",""NUM"",,,,,,,,,,,1
9,say,""Retrying..."",0,0
10,assignStr,""STATUS"",""Retry""
11,goTo,""LABEL_END_A123""
12,doEndIf,,,,,,,,,,,,,,,1
13,setCommands
14,assignStr,""STATUS"",""OK""
15,assignNum,""LABELS_COUNT"",2
16,setSendHostFlag,""LABELS_1_LABEL"",0
17,assignStr,""LABELS_1_CODE"",""123""
18,assignStr,""LABELS_1_LABEL"",""123""
19,assignNum,""LABELS_1_VALIDATED"",0
20,setSendHostFlag,""LABELS_2_LABEL"",0
21,assignStr,""LABELS_2_CODE"",""456""
22,assignStr,""LABELS_2_LABEL"",""456""
23,assignNum,""LABELS_2_VALIDATED"",0
24,say,""Validate labels"",0,0
25,setCommands,,,,,,,,,LABEL_END_A123
26,label,""LABEL_VALIDATE""
27,doIf,""LABELS_COUNT"",""0"",""="",""NUM"",,,,,,,,,,,2
28,say,""Validation completed"",0,0
29,goTo,""LABEL_END_A123""
30,doEndIf,,,,,,,,,,,,,,,2
31,concat,""PROMPT"","", remain "",""LABELS_COUNT""
32,getDigits,""DUMMY"",""$PROMPT"",,10,0,0,,""3"",""3"",,,1,,,1,,,0
33,doIf,""DUMMY"",""LABELS_1_CODE"",""="",""STR"",,,,,,,,,,,3
34,doIf,""LABELS_1_VALIDATED"",""0"",""="",""NUM"",,,,,,,,,,,4
35,assignNum,""LABELS_1_VALIDATED"",1
36,decrement,""LABELS_COUNT""
37,setSendHostFlag,""LABELS_1_LABEL"",1
38,goTo,""LABEL_VALIDATE""
39,doEndIf,,,,,,,,,,,,,,,4
40,doEndIf,,,,,,,,,,,,,,,3
41,doIf,""DUMMY"",""LABELS_2_CODE"",""="",""STR"",,,,,,,,,,,5
42,doIf,""LABELS_2_VALIDATED"",""0"",""="",""NUM"",,,,,,,,,,,6
43,assignNum,""LABELS_2_VALIDATED"",1
44,decrement,""LABELS_COUNT""
45,setSendHostFlag,""LABELS_2_LABEL"",1
46,goTo,""LABEL_VALIDATE""
47,doEndIf,,,,,,,,,,,,,,,6
48,doEndIf,,,,,,,,,,,,,,,5
49,say,""Incorrect"",0,0
50,goTo,""LABEL_VALIDATE""
51,label,""LABEL_EXCEPTION_A123""
52,assignStr,""STATUS"",""Cancelled""
53,label,""LABEL_END_A123""
54,assignNum,""RESPONSETYPE"",3
55,setSendHostFlag,""RESPONSETYPE"",1
56,setSendHostFlag,""STATUS"",1
57,setSendHostFlag,""START_TIME"",1
58,setSendHostFlag,""END_TIME"",1
59,assignNum,""END_TIME"",""#time""

";

        private readonly PlaceInDock placeInDockWithoutOptionalParams = new("A123", string.Empty);

        private readonly string expectedPlaceInDockWithoutOptionalParamsDialog = @"0,setSendHostFlag,""CODE"",1
1,loadMenuOptions,""MENU""
2,setCommandsConfirm,CONFIRM,CONFIRM,CONFIRM,CONFIRM,,,,,CONFIRM,CONFIRM
3,assignNum,""DLG"",1
4,assignStr,""CODE"",""A123""
5,label,""LABEL_DOCK_A123""
6,assignStr,""STATUS""
7,assignNum,""START_TIME"",""#time""
8,setCommands,LABEL_DOCK_EXCEPTION_A123
9,say,""Place in dock"",0,0
10,getDigits,""DOCK"",""Specify destination"",,10,0,0,,""1"",""20"",,,0,,,1,,,0
11,setSendHostFlag,""DOCK"",1
12,assignStr,""STATUS"",""OK""
13,goTo,""LABEL_DOCK_END_A123""
14,label,""LABEL_DOCK_EXCEPTION_A123""
15,assignStr,""STATUS"",""Cancelled""
16,label,""LABEL_DOCK_END_A123""
17,assignNum,""END_TIME"",""#time""
18,assignNum,""RESPONSETYPE"",4
19,setSendHostFlag,""STATUS"",1
20,setSendHostFlag,""DOCK"",1
21,setSendHostFlag,""START_TIME"",1
22,setSendHostFlag,""END_TIME"",1
23,setSendHostFlag,""RESPONSETYPE"",1
24,assignStr,""CURRENT_AISLE""

";

        private readonly PlaceInDock placeInDock = new("A123", string.Empty)
        {
            CD = "123",
            Dock = "Dock 1",
        };

        private readonly string expectedPlaceInDockDialog = @"0,setSendHostFlag,""CODE"",1
1,loadMenuOptions,""MENU""
2,setCommandsConfirm,CONFIRM,CONFIRM,CONFIRM,CONFIRM,,,,,CONFIRM,CONFIRM
3,assignNum,""DLG"",1
4,assignStr,""CODE"",""A123""
5,label,""LABEL_DOCK_A123""
6,assignStr,""STATUS""
7,assignNum,""START_TIME"",""#time""
8,setCommands,LABEL_DOCK_EXCEPTION_A123
9,say,""Place in dock"",0,0
10,getDigits,""DUMMY"",""Dock Dock 1"",,10,0,0,,""1"",""3"",,,1,""123"",""Incorrect"",1,,,0
11,assignStr,""DOCK"",""Dock 1""
12,setSendHostFlag,""DOCK"",1
13,assignStr,""STATUS"",""OK""
14,goTo,""LABEL_DOCK_END_A123""
15,label,""LABEL_DOCK_EXCEPTION_A123""
16,assignStr,""STATUS"",""Cancelled""
17,label,""LABEL_DOCK_END_A123""
18,assignNum,""END_TIME"",""#time""
19,assignNum,""RESPONSETYPE"",4
20,setSendHostFlag,""STATUS"",1
21,setSendHostFlag,""DOCK"",1
22,setSendHostFlag,""START_TIME"",1
23,setSendHostFlag,""END_TIME"",1
24,setSendHostFlag,""RESPONSETYPE"",1
25,assignStr,""CURRENT_AISLE""

";
        private readonly AskQuestion askQuestion = new("A123", "Question");

        private readonly string expectedAskQuestionDialog = @"0,setSendHostFlag,""CODE"",1
1,loadMenuOptions,""MENU""
2,setCommandsConfirm,CONFIRM,CONFIRM,CONFIRM,CONFIRM,,,,,CONFIRM,CONFIRM
3,assignNum,""DLG"",1
4,assignStr,""CODE"",""A123""
5,setCommands
6,assignStr,""STATUS""
7,assignNum,""START_TIME"",""#time""
8,ask,""DUMMY"",""Question"",,1,0,""1"",""0""
9,doIf,""DUMMY"",""0"",""="",""NUM"",,,,,,,,,,,1
10,assignStr,""STATUS"",""Cancelled""
11,doElse,,,,,,,,,,,,,,,1
12,assignStr,""STATUS"",""OK""
13,doEndIf,,,,,,,,,,,,,,,1
14,assignNum,""END_TIME"",""#time""
15,assignNum,""RESPONSETYPE"",8
16,setSendHostFlag,""STATUS"",1
17,setSendHostFlag,""START_TIME"",1
18,setSendHostFlag,""END_TIME"",1
19,setSendHostFlag,""RESPONSETYPE"",1

";
    }
}
