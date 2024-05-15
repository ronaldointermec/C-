using GWSProject1.Modules;
using Honeywell.GWS.Connector.SDK;
using Honeywell.GWS.Connector.SDK.Interfaces;
using Honeywell.VIO.SDK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace GWSProject1
{
    [Description("GWSProject1")]
    public class Workflow : WorkflowBase, IWorkflowWithData, IWorkflowWithCommands
    {
        private readonly IBehavior<IBehaviorSettings> _behavior;

        public Workflow(IBehavior<IBehaviorSettings> behavior) => _behavior = behavior;

        public override async ValueTask<SignOn> ConnectAsync(IDevice device)
        {
            var signOn = new SignOn();

            var res = await _behavior.SignOnAsync(device.Operator);


            if (res)
                signOn.NoPassword("Access granted!");
            else
                signOn.NotAllowed("Access denied!", "Please contact your supervisor");

            return signOn;
        }

        public override ValueTask<VariableSet> GetVariableSetAsync(Dictionary<string, string> data, IDevice device)
        {
            var vars = new VariableSet();

            vars.Add(Vars.DLG, "00", true);
            vars.Add(Vars.ID);
            vars.Add(Vars.NUMBER1);
            vars.Add(Vars.NUMBER2);
            vars.Add(Vars.OPERATION);
            vars.Add(Vars.RESULT);
            vars.Add(Vars.PROMPT);
            vars.Add(Vars.DUMMY);
            vars.Add(Vars.START);
            vars.Add(Vars.END);

            return new ValueTask<VariableSet>(vars);
        }

        public override ValueTask BuildInstructionsAsync(InstructionSet instr, Dictionary<string, string> data, IDevice device)
        {
            if (!data.ContainsKey(Vars.DLG))
                throw new InvalidOperationException("Missing device state");

            if (!Enum.TryParse(data[Vars.DLG], out Dialogs dialog))
                throw new InvalidOperationException($"Unknown state '{data[Vars.DLG]}'");

            switch (dialog)
            {
                case Dialogs.Start:
                    Start(instr, data, device);
                    break;
                case Dialogs.GetNextOperation:
                    GetNextOperation(instr, data, device);
                    break;
                case Dialogs.SendOperationResult:
                    SendOperationResult(instr, data, device);
                    break;

            }

            return default;
        }

        private void Start(InstructionSet instr, Dictionary<string, string> data, IDevice device)
        {
            instr.Say($"Welcome to Honeywell Voice. My setting is {_behavior.Settings.MySetting}", requireReady: true, priorityPrompt: true);
            instr.AssignNum(Vars.DLG, Dialogs.GetNextOperation);
        }
        private void GetNextOperation(InstructionSet instr, Dictionary<string, string> data, IDevice device)
        {
            var op = _behavior.GetOperation();

            if (op is null)
            {
                instr.Say("No more operations available", true, true, "To retry, say VCONFIRM");
                return;
            }

            instr.AssignNum(Vars.START, "#time");

            instr.Label(Labels.Begin);

            if (!string.IsNullOrEmpty(op.Message))
            {
                instr.Say($"{op.Message}, to continue say VCONFIRM", true);

            }

            //instr.SetCommand(1, op.Message, false);
            instr.SetCommands(
                command01: op.Message,
                command02: Vars.RESULT,
                command03: Labels.Restart,
                command04: "HOST"
                );

            instr.AssignNum(Vars.ID, op.Id);

            if (op.Operator1.HasValue)
            {
                instr.AssignNum(Vars.NUMBER1, op.Operator1.Value);
            }
            else
            {
                instr.GetDigits(
                     Vars.NUMBER1,
                     "Say first number",
                     confirmationPrompt: ", correct?",
                     minLength: "1",
                     maxLength: "3",
                     minRange: "2",
                     maxRange: "777",
                     barcodeEnabled: true
          );
            }

            if (op.Operator2.HasValue)
            {
                instr.AssignNum(Vars.NUMBER2, op.Operator2.Value);
            }
            else
            {
                instr.GetDigits(
                    Vars.NUMBER2,
                    "Say second number",
                    //confirmationPrompt: ", correct?",
                    //minLength: "1", maxLength: "3",
                    //minRange: "2", maxRange: "777",
                    //barcodeEnabled: true
                    //mustEqual: $"${Vars.NUMBER1}",wrongPrompt:"Wrong", includeCollectedDataInWrongPrompt: true
                    anchorWordsKey: "FORMAT", anchorWordVariableName: Vars.DUMMY,
                    imageUrl: "https://picsum.photos/700"
                    );

                instr.DoIf(Vars.DUMMY, "Dozens", Operation.EQ, CompareType.Str, _ => instr.Multiply(Vars.NUMBER2, Vars.NUMBER2, 12));
            }


            instr.AssignNum(Vars.OPERATION, op.Operation);

            instr.DoIf(Vars.OPERATION, (int)OperationType.Add, Operation.EQ, ifb =>
            {
                {
                    instr.Add(Vars.RESULT, Vars.NUMBER1, Vars.NUMBER2);
                }

                ifb.DoElseIf(Vars.OPERATION, (int)OperationType.Subtract, Operation.EQ);
                {
                    instr.Subtract(Vars.RESULT, Vars.NUMBER1, Vars.NUMBER2);
                }

                ifb.DoElseIf(Vars.OPERATION, (int)OperationType.Multiply, Operation.EQ);
                {
                    instr.Multiply(Vars.RESULT, Vars.NUMBER1, Vars.NUMBER2);
                }

                ifb.DoElse();
                {
                    instr.DoIf(Vars.NUMBER2, 0, Operation.EQ, ifb2 =>
                    {
                        instr.BeepError();
                        {
                            instr.Say("Dividing by zero is not a good idea");

                            if (op.Operator2.HasValue)
                            {
                                instr.AssignNum(Vars.RESULT, 0);
                            }
                            else
                            {
                                instr.GoTo(Labels.Restart);
                            }

                        }

                        ifb2.DoElse();
                        {
                            instr.Divide(Vars.RESULT, Vars.NUMBER1, Vars.NUMBER2);
                        }

                    });

                }

            });

            instr.AssignNum(Vars.END, "#time");

            // Send ODR
            instr.SetSendHostFlag(true, Vars.START, Vars.END);
            instr.GetVariablesOdr();
            instr.SetSendHostFlag(false, Vars.START, Vars.END);

            // Speak Result            
            instr.Subtract(Vars.DUMMY, Vars.END, Vars.START);
            instr.Concat(Vars.PROMPT, "The result is", Vars.RESULT);
            instr.Concat(Vars.PROMPT, ", in ");
            instr.Concat(Vars.PROMPT, Vars.DUMMY);
            instr.Concat(Vars.PROMPT, " seconds, to continue say VCONFIRM");

            instr.Say($"${Vars.PROMPT}", true, true);

            // Send Result
            instr.SetSendHostFlag(true, Vars.ID, Vars.RESULT);
            instr.AssignNum(Vars.DLG, Dialogs.SendOperationResult);
            instr.GoTo(Labels.End);

            // Restart Command 
            instr.Label(Labels.Restart);
            instr.Say("Operation restarted");
            instr.GoTo(Labels.Begin);

            instr.Label(Labels.End);
        }

        private void SendOperationResult(InstructionSet instr, Dictionary<string, string> data, IDevice device)
        {
            if (!data.TryGetValue(Vars.ID, out int id))
            {
                throw new InvalidOperationException($"Variable '{nameof(Vars.ID)}' not found or has not a valid value");
            }

            if (!data.TryGetValue(Vars.RESULT, out float result))
            {
                throw new InvalidOperationException($"Variable '{nameof(Vars.RESULT)}' not found or has not a valid value");
            }


            instr.SetSendHostFlag(false, Vars.ID, Vars.RESULT);

            try
            {
                _behavior.SetOperationResult(id, result);
                instr.Say("Operation confirmed");
            }
            catch (Exception)
            {

                instr.BeepError();
                instr.Say("Could not confirm the operation");
            }

            instr.AssignNum(Vars.DLG, Dialogs.GetNextOperation);
            GetNextOperation(instr, data, device);

        }

        public override ValueTask<AnchorWordsOptions> GetAnchorWordsAsync(IDevice device)
        {
            var anchorWords = new AnchorWordsOptions();
            anchorWords.AddOption("FORMAT", "Units");
            anchorWords.AddOption("FORMAT", "Dozens");
            return new(anchorWords);

        }

        public ValueTask ProcessDataAsync(Dictionary<string, string> data, IDevice device)
        {
            if (!data.TryGetValue(Vars.START, out int start))
            {
                return default;
            }
            if (!data.TryGetValue(Vars.END, out int end))
            {
                return default;
            }
            // Casting int to datetime 
            var startDate = new DateTime(2009, 1, 1).AddSeconds(start);
            var endDate = new DateTime(2009, 1, 1).AddSeconds(end);

            _behavior.Log($"Operation started at {startDate} and ended at {endDate}", LogLevel.Information);

            var elapsed = end - start;

            if (!device.Bag.TryGetValue(Bag.Elapsed, out int total))
            {
                device.Bag.Add(Bag.Elapsed, elapsed);

            }
            else
            {
                total += elapsed;
                device.Bag[Bag.Elapsed] = total;
            }
            return default;
        }

        public ValueTask<CommandRequest> BuildCommandInstructionsAsync(int command, Dictionary<string, string> data, IDevice device)
        {
            var cmd = new CommandRequest();

            switch (command)
            {
                case 4:
                    {
                        if (device.Bag.TryGetValue(Bag.Elapsed, out int elapsed))
                        {
                            cmd.Say($"Total elapsed time is {elapsed}");
                        }
                        else
                        {
                            cmd.Say("No information available");
                        }
                        break;
                    }
            }

            return new(cmd);


        }
    }
}