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
    public class Workflow : WorkflowBase
    {
        private readonly IBehavior<IBehaviorSettings> _behavior;

        public Workflow(IBehavior<IBehaviorSettings> behavior) => _behavior = behavior;

        public override ValueTask<SignOn> ConnectAsync(IDevice device)
        {
            return base.ConnectAsync(device);
        }

        public override ValueTask<VariableSet> GetVariableSetAsync(Dictionary<string, string> data, IDevice device)
        {
            var vars = new VariableSet();

            vars.Add(Vars.DLG, "00", true);

            vars.Add(Vars.NUMBER1);
            vars.Add(Vars.NUMBER2);
            vars.Add(Vars.OPERATION);
            vars.Add(Vars.RESULT);


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
                    instr.Say($"Welcome to Honeywell Voice. My setting is {_behavior.Settings.MySetting}", requireReady: true, priorityPrompt: true);

                    var op = _behavior.GetOperation();

                    if (op is null)
                    {
                        instr.Say("No more operations available", true, true, "To retry, say VCONFIRM");
                        return default;

                    }
                    if (!string.IsNullOrEmpty(op.Message))
                    { 
                    
                    instr.Say($"{op.Message}, to continue say VCONFIRM", true);
                    }
                    break;
            }

            return default;
        }
    }
}