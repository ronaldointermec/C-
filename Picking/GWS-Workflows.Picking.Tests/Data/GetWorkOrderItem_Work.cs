using Honeywell.GWS.Connector.Library.Workflows.Picking.Models;

namespace Honeywell.GWS.Connector.Library.Workflows.Picking.Tests.Data
{
    public class GetWorkOrderItem_Work : TheoryData<string, GetWorkOrderItemBase>
    {
        public GetWorkOrderItem_Work()
        {
            Add(expectedNotWork, null!);
            Add(expectedBeginPickingOrderWithoutOptionalParamsDialog, beginPickingOrderWithoutOptionalParams);
            Add(expectedBeginPickingOrderDialog, beginPickingOrder);
            Add(expectedPickingLineDialog, pickingLine);
            Add(expectedPrintLabelsWithoutOptionalParamsDialog, printLabelsWithoutOptionalParams);
            Add(expectedPrintLabelsDialog, printLabels);
            Add(expectedValidatePrintingWithoutOptionalParamsDialog, validatePrintingWithoutOptionalParams);
            Add(expectedValidatePrintingDialog, validatePrinting);
            Add(expectedPlaceInDockWithoutOptionalParamsDialog, placeInDockWithoutOptionalParams);
            Add(expectedPlaceInDockDialog, placeInDock);
            Add(expectedAskQuestionDialog, askQuestion);
        }

        private readonly string expectedNotWork = @"0,setCommands
1,say,""No work assigned. To try again, say VCONFIRM"",1,0

";
        private readonly BeginPickingOrder beginPickingOrder = new("A123", "Begin picking order line")
        {
            OrderNumber = "O12345",
            Customer = "Customer",
            ContainerType = "Pallet",
            ContainersCount = 1,
        };

        private readonly string expectedBeginPickingOrderDialog = @"0,assignStr,""CODE"",""A123""
1,assignStr,""CURRENT_AISLE""
2,assignStr,""CURRENT_POSITION""
3,assignStr,""CUSTOMER"",""Customer""
4,assignNum,""START_TIME"",""#time""
5,label,""LABEL_START_A123""
6,setCommands,,,LABEL_BREAK_A123
7,assignStr,""PROMPT"",""Order O12345""
8,concat,""PROMPT"",""PROMPT"","", Customer""
9,concat,""PROMPT"",""PROMPT"","", in Pallet""
10,concat,""PROMPT"",""PROMPT"","", 1 containers""
11,say,""$PROMPT"",1,1
12,say,""Begin picking order line"",1,1
13,goTo,""LABEL_END_A123""
14,label,""LABEL_BREAK_A123""
15,setCommands,,,,,,,,,LABEL_START_A123
16,getMenu,""BREAK_REASON"",""Break reason?"",,""BREAK"",1,""?"",0,,1
17,setSendHostFlag,""BREAK_REASON"",1
18,setSendHostFlag,""RESPONSETYPE"",1
19,assignNum,""RESPONSETYPE"",5
20,getVariablesOdr
21,setSendHostFlag,""BREAK_REASON"",0
22,setCommands
23,say,""At break. To resume work, say VCONFIRM"",1,0
24,assignNum,""RESPONSETYPE"",6
25,getVariablesOdr
26,goTo,""LABEL_START_A123""
27,label,""LABEL_END_A123""
28,assignNum,""END_TIME"",""#time""
29,assignStr,""STATUS"",""OK""
30,assignNum,""RESPONSETYPE"",0
31,setSendHostFlag,""STATUS"",1
32,setSendHostFlag,""START_TIME"",1
33,setSendHostFlag,""END_TIME"",1
34,setSendHostFlag,""RESPONSETYPE"",1

";
        private readonly BeginPickingOrder beginPickingOrderWithoutOptionalParams = new("A123", string.Empty)
        {
            OrderNumber = "O12345",
        };

        private readonly string expectedBeginPickingOrderWithoutOptionalParamsDialog = @"0,assignStr,""CODE"",""A123""
1,assignStr,""CURRENT_AISLE""
2,assignStr,""CURRENT_POSITION""
3,assignNum,""START_TIME"",""#time""
4,label,""LABEL_START_A123""
5,setCommands,,,LABEL_BREAK_A123
6,assignStr,""PROMPT"",""Order O12345""
7,say,""$PROMPT"",1,1
8,goTo,""LABEL_END_A123""
9,label,""LABEL_BREAK_A123""
10,setCommands,,,,,,,,,LABEL_START_A123
11,getMenu,""BREAK_REASON"",""Break reason?"",,""BREAK"",1,""?"",0,,1
12,setSendHostFlag,""BREAK_REASON"",1
13,setSendHostFlag,""RESPONSETYPE"",1
14,assignNum,""RESPONSETYPE"",5
15,getVariablesOdr
16,setSendHostFlag,""BREAK_REASON"",0
17,setCommands
18,say,""At break. To resume work, say VCONFIRM"",1,0
19,assignNum,""RESPONSETYPE"",6
20,getVariablesOdr
21,goTo,""LABEL_START_A123""
22,label,""LABEL_END_A123""
23,assignNum,""END_TIME"",""#time""
24,assignStr,""STATUS"",""OK""
25,assignNum,""RESPONSETYPE"",0
26,setSendHostFlag,""STATUS"",1
27,setSendHostFlag,""START_TIME"",1
28,setSendHostFlag,""END_TIME"",1
29,setSendHostFlag,""RESPONSETYPE"",1

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

        private readonly string expectedPickingLineDialog = @"0,assignStr,""CODE"",""A123""
1,label,""LABEL_START_A123""
2,assignStr,""STATUS""
3,assignStr,""PICKED""
4,assignStr,""SERVING""
5,assignStr,""WEIGHT""
6,assignStr,""STOCK""
7,assignStr,""DOCK""
8,assignStr,""BREAKAGE""
9,assignStr,""BATCH""
10,assignNum,""START_TIME"",""#time""
11,assignStr,""PRODUCT_DESCRIPTION"",""productname""
12,assignStr,""PRODUCT_NUMBER"",""productnumber""
13,assignStr,""UPC_NUMBER"",""upcnumber""
14,assignStr,""QTY_UPPER""
15,assignStr,""QTY_LOWER""
16,assignStr,""MAX_QTY_ALLOWED_PER_PICK""
17,assignStr,""TOTAL_PICKED""
18,assignStr,""TOTAL_WEIGHT""
19,assignStr,""CUSTOMER"",""Customer""
20,setCommands
21,say,""Message"",1,1
22,assignStr,""CURRENT_AISLE""
23,assignStr,""CURRENT_POSITION""
24,setCommands,LABEL_EXCEPTION_LOCATION_A123,LABEL_DOCK_A123,LABEL_BREAK_A123,,,There are 10 lines remaining,,,,,$CUSTOMER
25,doIf,""CURRENT_AISLE"",""Aisle"",""<>"",""STR"",,,,,,,,,,,1
26,assignStr,""CURRENT_AISLE"",""Aisle""
27,assignStr,""CURRENT_POSITION"",""Position""
28,say,""Aisle Aisle"",1,0
29,doEndIf,,,,,,,,,,,,,,,1
30,assignStr,""WHEREAMI"",""Aisle Aisle""
31,setCommand,4,""LABEL_SKIP_SLOT_A123"",CONFIRM
32,say,""Slot Slot"",1,0
33,concat,""WHEREAMI"",""WHEREAMI"","", Slot Slot""
34,setCommands,LABEL_EXCEPTION_CD_A123,LABEL_DOCK_A123,LABEL_BREAK_A123,LABEL_SKIP_SLOT_A123,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
35,getDigits,""DUMMY"",""Position"",,10,0,0,,""1"",""2"",,,1,""CD"",""Incorrect"",1,,,0
36,concat,""WHEREAMI"",""WHEREAMI"","", Position""
37,say,""$PRODUCT_DESCRIPTION"",0,0
38,label,""LABEL_VALIDATE_PRODUCT_CD_A123""
39,getDigits,""PRODUCT_CD"",""Check digit?"",,10,0,0,,""1"",""20"",,,1,,,1,,,0
40,doIf,""PRODUCT_CD"",""1234"",""="",""STR"",,,,,,,,,,,2
41,setSendHostFlag,""PRODUCT_CD"",1
42,goTo,""LABEL_ASK_BATCH_A123""
43,doEndIf,,,,,,,,,,,,,,,2
44,doIf,""PRODUCT_CD"",""567"",""="",""STR"",,,,,,,,,,,3
45,setSendHostFlag,""PRODUCT_CD"",1
46,goTo,""LABEL_ASK_BATCH_A123""
47,doEndIf,,,,,,,,,,,,,,,3
48,concat,""DUMMY"",""Incorrect, "",""PRODUCT_CD""
49,say,""$DUMMY"",0,0
50,goTo,""LABEL_VALIDATE_PRODUCT_CD_A123""
51,label,""LABEL_ASK_BATCH_A123""
52,say,""Select batch"",0,0
53,setSendHostFlag,""BATCH"",1
54,ask,""DUMMY"",""Batch 123?"",,1,1,""y"",""n"",""c""
55,doIf,""DUMMY"",""y"",""="",""STR"",,,,,,,,,,,4
56,assignStr,""BATCH"",""123""
57,goTo,""LABEL_RESET_PICK_A123""
58,doElseIf,""DUMMY"",""c"",""="",""STR"",,,,,,,,,,,4
59,goTo,""LABEL_RESET_PICK_A123""
60,doEndIf,,,,,,,,,,,,,,,4
61,ask,""DUMMY"",""Batch 456?"",,1,1,""y"",""n"",""c""
62,doIf,""DUMMY"",""y"",""="",""STR"",,,,,,,,,,,5
63,assignStr,""BATCH"",""456""
64,goTo,""LABEL_RESET_PICK_A123""
65,doElseIf,""DUMMY"",""c"",""="",""STR"",,,,,,,,,,,5
66,goTo,""LABEL_RESET_PICK_A123""
67,doEndIf,,,,,,,,,,,,,,,5
68,goTo,""LABEL_ASK_BATCH_A123""
69,label,""LABEL_RESET_PICK_A123""
70,assignStr,""PROMPT"",""OriginalServingPrompt""
71,assignNum,""QTY_REQUESTED"",1
72,assignNum,""QTY_REQUESTED_ORIG"",1
73,assignStr,""TOTAL_PICKED""
74,assignStr,""TOTAL_WEIGHT""
75,assignNum,""QTY_UPPER"",1
76,assignNum,""QTY_LOWER"",1
77,assignNum,""MAX_QTY_ALLOWED_PER_PICK"",1
78,assignStr,""SERVING"",""OriginalServingCode""
79,label,""LABEL_PICK_A123""
80,setCommands,LABEL_EXCEPTION_A123,,,LABEL_SKIP_SLOT_A123,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,LABEL_BREAKAGE_A123,$CUSTOMER,LABEL_FORMAT_A123,LABEL_UNITS_A123,,$PRODUCT_NUMBER,$UPC_NUMBER
81,getDigits,""TOTAL_PICKED"",""$PROMPT"",""HelpMessage"",10,0,0,,""1"",""20"",""$QTY_LOWER"",""$QTY_UPPER"",0,,,1,,,0
82,doIf,""TOTAL_PICKED"",""QTY_REQUESTED"",""<"",""NUM"",,,,,,,,,,,6
83,concat,""PROMPT"",""Asked for "",""QTY_REQUESTED""
84,concat,""PROMPT"",""PROMPT"","", you said ""
85,concat,""PROMPT"",""PROMPT"",""TOTAL_PICKED""
86,concat,""PROMPT"",""PROMPT"","". Is this a short?""
87,ask,""DUMMY"",""$PROMPT"",,1,0,""1"",""0""
88,doIf,""DUMMY"",""0"",""="",""NUM"",,,,,,,,,,,7
89,goTo,""LABEL_RESET_PICK_A123""
90,doEndIf,,,,,,,,,,,,,,,7
91,setCommands,,,,,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
92,getMenu,""DUMMY"",""Reason code?"",,""LOWER_QUANTITY"",1,""?"",0,""Incorrect"",1
93,doIf,""DUMMY"",""1"",""="",""NUM"",,,,,,,,,,,8
94,assignStr,""STATUS"",""Empty""
95,doElseIf,""DUMMY"",""2"",""="",""NUM"",,,,,,,,,,,8
96,assignStr,""STATUS"",""Breakage""
97,doElseIf,""DUMMY"",""3"",""="",""NUM"",,,,,,,,,,,8
98,assignStr,""STATUS"",""Completed""
99,doElseIf,""DUMMY"",""4"",""="",""NUM"",,,,,,,,,,,8
100,assignStr,""STATUS"",""EndPallet""
101,doElseIf,""DUMMY"",""5"",""="",""NUM"",,,,,,,,,,,8
102,assignStr,""STATUS"",""OK""
103,doElseIf,""DUMMY"",""6"",""="",""NUM"",,,,,,,,,,,8
104,goTo,""LABEL_RESET_PICK_A123""
105,doEndIf,,,,,,,,,,,,,,,8
106,getDigits,""STOCK"",""Quantity in location"",,10,0,1,""?"",""1"",""20"",,,0,,,1,,,0
107,setSendHostFlag,""STOCK"",1
108,doElse,,,,,,,,,,,,,,,6
109,assignStr,""STATUS"",""OK""
110,doEndIf,,,,,,,,,,,,,,,6
111,label,""LABEL_WEIGHT_A123""
112,getFloat,""TOTAL_WEIGHT"",""Weight?"",,10,0,1,""?"",""1"",""20"",,,0,,,1,,,0
113,doIf,""TOTAL_WEIGHT"",""0"",""<>"",""NUM"",,,,,,,,,,,9
114,doIf,""TOTAL_WEIGHT"",""1.5"",""<"",""NUM"",,,,,,,,,,,10
115,say,""The weight is less than allowed"",0,0
116,goTo,""LABEL_WEIGHT_A123""
117,doEndIf,,,,,,,,,,,,,,,10
118,doIf,""TOTAL_WEIGHT"",""2.5"","">"",""NUM"",,,,,,,,,,,11
119,say,""The weight is more than allowed"",0,0
120,goTo,""LABEL_WEIGHT_A123""
121,doEndIf,,,,,,,,,,,,,,,11
122,doEndIf,,,,,,,,,,,,,,,9
123,setSendHostFlag,""TOTAL_WEIGHT"",1
124,label,""LABEL_CONFIRM_A123""
125,assignNum,""END_TIME"",""#time""
126,assignNum,""RESPONSETYPE"",1
127,setSendHostFlag,""STATUS"",1
128,setSendHostFlag,""START_TIME"",1
129,setSendHostFlag,""END_TIME"",1
130,setSendHostFlag,""TOTAL_PICKED"",1
131,setSendHostFlag,""SERVING"",1
132,setSendHostFlag,""RESPONSETYPE"",1
133,goTo,""LABEL_END_A123""
134,label,""LABEL_FORMAT_A123""
135,setCommands,LABEL_EXCEPTION_A123,,,,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
136,assignStr,""PROMPT"",""AlternativeServingPrompt""
137,assignNum,""QTY_REQUESTED"",1
138,assignNum,""QTY_REQUESTED_ORIG"",1
139,assignNum,""QTY_UPPER"",1
140,assignNum,""QTY_LOWER"",1
141,assignNum,""MAX_QTY_ALLOWED_PER_PICK"",1
142,assignStr,""SERVING"",""AlternativeServingCode""
143,goTo,""LABEL_PICK_A123""
144,label,""LABEL_BREAKAGE_A123""
145,getDigits,""BREAKAGE"",""Breakage quantity?"",,10,0,1,""?"",""1"",""20"",,,0,,,1,,,0
146,setSendHostFlag,""BREAKAGE"",1
147,goTo,""LABEL_PICK_A123""
148,label,""LABEL_UNITS_A123""
149,setCommands,LABEL_EXCEPTION_A123,,,,,There are 10 lines remaining,$WHEREAMI,$PRODUCT_DESCRIPTION,,,$CUSTOMER,,,,$PRODUCT_NUMBER,$UPC_NUMBER
150,assignStr,""PROMPT"",""1 units""
151,assignNum,""QTY_REQUESTED"",1
152,assignNum,""QTY_REQUESTED_ORIG"",1
153,assignNum,""QTY_UPPER"",1
154,assignNum,""QTY_LOWER"",1
155,assignNum,""MAX_QTY_ALLOWED_PER_PICK"",1
156,assignStr,""SERVING"",""UNITS""
157,goTo,""LABEL_PICK_A123""
158,label,""LABEL_EXCEPTION_LOCATION_A123""
159,assignStr,""STATUS"",""BadLocation""
160,assignStr,""SERVING""
161,goTo,""LABEL_CONFIRM_A123""
162,label,""LABEL_EXCEPTION_CD_A123""
163,assignStr,""STATUS"",""NoCheck""
164,assignStr,""SERVING""
165,goTo,""LABEL_CONFIRM_A123""
166,label,""LABEL_EXCEPTION_A123""
167,assignStr,""STATUS"",""Cancelled""
168,assignStr,""SERVING""
169,goTo,""LABEL_CONFIRM_A123""
170,label,""LABEL_SKIP_SLOT_A123""
171,assignStr,""STATUS"",""Postponed""
172,assignStr,""SERVING""
173,goTo,""LABEL_CONFIRM_A123""
174,label,""LABEL_DOCK_A123""
175,assignNum,""RESPONSETYPE"",1
176,assignStr,""STATUS"",""EndPallet""
177,assignNum,""END_TIME"",""#time""
178,setSendHostFlag,""RESPONSETYPE"",1
179,setSendHostFlag,""STATUS"",1
180,setSendHostFlag,""START_TIME"",1
181,setSendHostFlag,""END_TIME"",1
182,goTo,""LABEL_END_A123""
183,label,""LABEL_BREAK_A123""
184,assignStr,""CURRENT_AISLE""
185,assignStr,""CURRENT_POSITION""
186,setCommands,,,,,,,,,LABEL_START_A123
187,getMenu,""BREAK_REASON"",""Break reason?"",,""BREAK"",1,""?"",0,,1
188,setSendHostFlag,""BREAK_REASON"",1
189,setSendHostFlag,""RESPONSETYPE"",1
190,assignNum,""RESPONSETYPE"",5
191,getVariablesOdr
192,setSendHostFlag,""BREAK_REASON"",0
193,setCommands
194,say,""At break. To resume work, say VCONFIRM"",1,0
195,assignNum,""RESPONSETYPE"",6
196,getVariablesOdr
197,goTo,""LABEL_START_A123""
198,label,""LABEL_END_A123""

";
        private readonly PrintLabels printLabelsWithoutOptionalParams = new("A123", string.Empty);

        private readonly string expectedPrintLabelsWithoutOptionalParamsDialog = @"0,assignStr,""CODE"",""A123""
1,assignStr,""STATUS""
2,assignNum,""START_TIME"",""#time""
3,assignStr,""PRINTER""
4,label,""LABEL_SELECT_LABELS_A123""
5,setCommands,,,,,LABEL_SELECT_PRINTER_A123,,,,LABEL_CANCEL_A123
6,doIf,""PRINTER"",,""="",""STR"",,,,,,,,,,,1
7,concat,""PROMPT"",""Printer"",""unknown""
8,doElse,,,,,,,,,,,,,,,1
9,concat,""PROMPT"",""Printer"",""PRINTER""
10,doEndIf,,,,,,,,,,,,,,,1
11,concat,""PROMPT"",""PROMPT"","", How many labels?""
12,getDigits,""LABELS"",""$PROMPT"",,10,0,1,""?"",""1"",""2"",,,0,,,1,,,0
13,setSendHostFlag,""RESPONSETYPE"",1
14,setSendHostFlag,""STATUS"",1
15,setSendHostFlag,""START_TIME"",1
16,setSendHostFlag,""END_TIME"",1
17,setSendHostFlag,""LABELS"",1
18,setSendHostFlag,""PRINTER"",1
19,assignStr,""STATUS"",""OK""
20,goTo,""LABEL_END_A123""
21,label,""LABEL_SELECT_PRINTER_A123""
22,setCommands,,,,,,,,,LABEL_SELECT_LABELS_A123
23,say,""Printers not available"",0,0
24,goTo,""LABEL_SELECT_LABELS_A123""
25,label,""LABEL_CANCEL_A123""
26,assignStr,""STATUS"",""Cancelled""
27,label,""LABEL_END_A123""
28,assignNum,""RESPONSETYPE"",2
29,assignNum,""END_TIME"",""#time""

";

        private readonly PrintLabels printLabels = new("A123", string.Empty)
        {
            Copies = 1,
            DefaultPrinter = 2,
            Printers = new[] { 1, 2, 3 }
        };

        private readonly string expectedPrintLabelsDialog = @"0,assignStr,""CODE"",""A123""
1,assignStr,""STATUS""
2,assignNum,""START_TIME"",""#time""
3,assignNum,""PRINTER"",2
4,label,""LABEL_SELECT_LABELS_A123""
5,setCommands,,,,,LABEL_SELECT_PRINTER_A123,,,,LABEL_CANCEL_A123
6,doIf,""PRINTER"",,""="",""STR"",,,,,,,,,,,1
7,concat,""PROMPT"",""Printer"",""unknown""
8,doElse,,,,,,,,,,,,,,,1
9,concat,""PROMPT"",""Printer"",""PRINTER""
10,doEndIf,,,,,,,,,,,,,,,1
11,concat,""PROMPT"",""PROMPT"","", to print, say VCONFIRM""
12,say,""$PROMPT"",1,0
13,assignNum,""LABELS"",1
14,setSendHostFlag,""RESPONSETYPE"",1
15,setSendHostFlag,""STATUS"",1
16,setSendHostFlag,""START_TIME"",1
17,setSendHostFlag,""END_TIME"",1
18,setSendHostFlag,""LABELS"",1
19,setSendHostFlag,""PRINTER"",1
20,assignStr,""STATUS"",""OK""
21,goTo,""LABEL_END_A123""
22,label,""LABEL_SELECT_PRINTER_A123""
23,setCommands,,,,,,,,,LABEL_SELECT_LABELS_A123
24,assignNum,""PRINTER_1_CODE"",1
25,assignNum,""PRINTER_2_CODE"",2
26,assignNum,""PRINTER_3_CODE"",3
27,label,""LABEL_PRINTER_A123""
28,getDigits,""PRINTER"",""Printer?"",,10,0,0,,""1"",""2"",,,0,,,1,,,0
29,doIf,""PRINTER"",""PRINTER_1_CODE"",""="",""NUM"",,,,,,,,,,,2
30,goTo,""LABEL_SELECT_LABELS_A123""
31,doEndIf,,,,,,,,,,,,,,,2
32,doIf,""PRINTER"",""PRINTER_2_CODE"",""="",""NUM"",,,,,,,,,,,3
33,goTo,""LABEL_SELECT_LABELS_A123""
34,doEndIf,,,,,,,,,,,,,,,3
35,doIf,""PRINTER"",""PRINTER_3_CODE"",""="",""NUM"",,,,,,,,,,,4
36,goTo,""LABEL_SELECT_LABELS_A123""
37,doEndIf,,,,,,,,,,,,,,,4
38,say,""Unknown printer"",0,0
39,goTo,""LABEL_PRINTER_A123""
40,label,""LABEL_CANCEL_A123""
41,assignStr,""STATUS"",""Cancelled""
42,label,""LABEL_END_A123""
43,assignNum,""RESPONSETYPE"",2
44,assignNum,""END_TIME"",""#time""

";

        private readonly ValidatePrinting validatePrintingWithoutOptionalParams = new("A123", string.Empty);

        private readonly string expectedValidatePrintingWithoutOptionalParamsDialog = @"0,assignStr,""CODE"",""A123""
1,setCommands,LABEL_EXCEPTION_A123
2,assignNum,""START_TIME"",""#time""
3,ask,""DUMMY"",""Correct printing?"",,1,0,""1"",""0""
4,doIf,""DUMMY"",""0"",""="",""NUM"",,,,,,,,,,,1
5,say,""Retrying..."",0,0
6,assignStr,""STATUS"",""Retry""
7,goTo,""LABEL_END_A123""
8,doEndIf,,,,,,,,,,,,,,,1
9,setCommands
10,assignStr,""STATUS"",""OK""
11,goTo,""LABEL_END_A123""
12,label,""LABEL_EXCEPTION_A123""
13,assignStr,""STATUS"",""Cancelled""
14,label,""LABEL_END_A123""
15,assignNum,""RESPONSETYPE"",3
16,setSendHostFlag,""RESPONSETYPE"",1
17,setSendHostFlag,""STATUS"",1
18,setSendHostFlag,""START_TIME"",1
19,setSendHostFlag,""END_TIME"",1
20,assignNum,""END_TIME"",""#time""

";

        private readonly ValidatePrinting validatePrinting = new("A123", string.Empty)
        {
            ValidationCodes = new[] { "123", "456" },
            VoiceLength = 3,
        };

        private readonly string expectedValidatePrintingDialog = @"0,assignStr,""CODE"",""A123""
1,setCommands,LABEL_EXCEPTION_A123
2,assignNum,""START_TIME"",""#time""
3,ask,""DUMMY"",""Correct printing?"",,1,0,""1"",""0""
4,doIf,""DUMMY"",""0"",""="",""NUM"",,,,,,,,,,,1
5,say,""Retrying..."",0,0
6,assignStr,""STATUS"",""Retry""
7,goTo,""LABEL_END_A123""
8,doEndIf,,,,,,,,,,,,,,,1
9,setCommands
10,assignStr,""STATUS"",""OK""
11,assignNum,""LABELS_COUNT"",2
12,setSendHostFlag,""LABELS_1_LABEL"",0
13,assignStr,""LABELS_1_CODE"",""123""
14,assignStr,""LABELS_1_LABEL"",""123""
15,assignNum,""LABELS_1_VALIDATED"",0
16,setSendHostFlag,""LABELS_2_LABEL"",0
17,assignStr,""LABELS_2_CODE"",""456""
18,assignStr,""LABELS_2_LABEL"",""456""
19,assignNum,""LABELS_2_VALIDATED"",0
20,say,""Validate labels"",0,0
21,setCommands,,,,,,,,,LABEL_END_A123
22,label,""LABEL_VALIDATE""
23,doIf,""LABELS_COUNT"",""0"",""="",""NUM"",,,,,,,,,,,2
24,say,""Validation completed"",0,0
25,goTo,""LABEL_END_A123""
26,doEndIf,,,,,,,,,,,,,,,2
27,concat,""PROMPT"","", remain "",""LABELS_COUNT""
28,getDigits,""DUMMY"",""$PROMPT"",,10,0,0,,""3"",""3"",,,1,,,1,,,0
29,doIf,""DUMMY"",""LABELS_1_CODE"",""="",""STR"",,,,,,,,,,,3
30,doIf,""LABELS_1_VALIDATED"",""0"",""="",""NUM"",,,,,,,,,,,4
31,assignNum,""LABELS_1_VALIDATED"",1
32,decrement,""LABELS_COUNT""
33,setSendHostFlag,""LABELS_1_LABEL"",1
34,goTo,""LABEL_VALIDATE""
35,doEndIf,,,,,,,,,,,,,,,4
36,doEndIf,,,,,,,,,,,,,,,3
37,doIf,""DUMMY"",""LABELS_2_CODE"",""="",""STR"",,,,,,,,,,,5
38,doIf,""LABELS_2_VALIDATED"",""0"",""="",""NUM"",,,,,,,,,,,6
39,assignNum,""LABELS_2_VALIDATED"",1
40,decrement,""LABELS_COUNT""
41,setSendHostFlag,""LABELS_2_LABEL"",1
42,goTo,""LABEL_VALIDATE""
43,doEndIf,,,,,,,,,,,,,,,6
44,doEndIf,,,,,,,,,,,,,,,5
45,say,""Incorrect"",0,0
46,goTo,""LABEL_VALIDATE""
47,label,""LABEL_EXCEPTION_A123""
48,assignStr,""STATUS"",""Cancelled""
49,label,""LABEL_END_A123""
50,assignNum,""RESPONSETYPE"",3
51,setSendHostFlag,""RESPONSETYPE"",1
52,setSendHostFlag,""STATUS"",1
53,setSendHostFlag,""START_TIME"",1
54,setSendHostFlag,""END_TIME"",1
55,assignNum,""END_TIME"",""#time""

";

        private readonly PlaceInDock placeInDockWithoutOptionalParams = new("A123", string.Empty);

        private readonly string expectedPlaceInDockWithoutOptionalParamsDialog = @"0,assignStr,""CODE"",""A123""
1,label,""LABEL_DOCK_A123""
2,assignStr,""STATUS""
3,assignNum,""START_TIME"",""#time""
4,setCommands,LABEL_DOCK_EXCEPTION_A123
5,say,""Place in dock"",0,0
6,getDigits,""DOCK"",""Specify destination"",,10,0,0,,""1"",""20"",,,0,,,1,,,0
7,setSendHostFlag,""DOCK"",1
8,assignStr,""STATUS"",""OK""
9,goTo,""LABEL_DOCK_END_A123""
10,label,""LABEL_DOCK_EXCEPTION_A123""
11,assignStr,""STATUS"",""Cancelled""
12,label,""LABEL_DOCK_END_A123""
13,assignNum,""END_TIME"",""#time""
14,assignNum,""RESPONSETYPE"",4
15,setSendHostFlag,""STATUS"",1
16,setSendHostFlag,""DOCK"",1
17,setSendHostFlag,""START_TIME"",1
18,setSendHostFlag,""END_TIME"",1
19,setSendHostFlag,""RESPONSETYPE"",1
20,assignStr,""CURRENT_AISLE""

";

        private readonly PlaceInDock placeInDock = new("A123", string.Empty)
        {
            CD = "123",
            Dock = "Dock 1",
        };

        private readonly string expectedPlaceInDockDialog = @"0,assignStr,""CODE"",""A123""
1,label,""LABEL_DOCK_A123""
2,assignStr,""STATUS""
3,assignNum,""START_TIME"",""#time""
4,setCommands,LABEL_DOCK_EXCEPTION_A123
5,say,""Place in dock"",0,0
6,getDigits,""DUMMY"",""Dock Dock 1"",,10,0,0,,""1"",""3"",,,1,""123"",""Incorrect"",1,,,0
7,assignStr,""DOCK"",""Dock 1""
8,setSendHostFlag,""DOCK"",1
9,assignStr,""STATUS"",""OK""
10,goTo,""LABEL_DOCK_END_A123""
11,label,""LABEL_DOCK_EXCEPTION_A123""
12,assignStr,""STATUS"",""Cancelled""
13,label,""LABEL_DOCK_END_A123""
14,assignNum,""END_TIME"",""#time""
15,assignNum,""RESPONSETYPE"",4
16,setSendHostFlag,""STATUS"",1
17,setSendHostFlag,""DOCK"",1
18,setSendHostFlag,""START_TIME"",1
19,setSendHostFlag,""END_TIME"",1
20,setSendHostFlag,""RESPONSETYPE"",1
21,assignStr,""CURRENT_AISLE""

";
        private readonly AskQuestion askQuestion = new("A123", "Question");

        private readonly string expectedAskQuestionDialog = @"0,assignStr,""CODE"",""A123""
1,setCommands
2,assignStr,""STATUS""
3,assignNum,""START_TIME"",""#time""
4,ask,""DUMMY"",""Question"",,1,0,""1"",""0""
5,doIf,""DUMMY"",""0"",""="",""NUM"",,,,,,,,,,,1
6,assignStr,""STATUS"",""Cancelled""
7,doElse,,,,,,,,,,,,,,,1
8,assignStr,""STATUS"",""OK""
9,doEndIf,,,,,,,,,,,,,,,1
10,assignNum,""END_TIME"",""#time""
11,assignNum,""RESPONSETYPE"",8
12,setSendHostFlag,""STATUS"",1
13,setSendHostFlag,""START_TIME"",1
14,setSendHostFlag,""END_TIME"",1
15,setSendHostFlag,""RESPONSETYPE"",1

";
    }
}
