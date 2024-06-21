# -*- coding: cp1252 -*-


import unittest
from VioBaseTestCase import VioBaseTestCase
from vio import VIO
from vocollect_core.task.task_runner import TaskRunnerBase
from VioTask import VioTask
from Constants import CONFIG_MSG, SIGN_ON, GET_VARIABLES
from vocollect_core.utilities import pickler
from InstructionProcessor import InstructionProcessorTask
import mock_catalyst

class Test(VioBaseTestCase):
    
    def setUp(self):
        self.clear()
        self._obj = VioTask()
        self._obj.taskRunner = VIO()
        TaskRunnerBase._main_runner = self._obj.taskRunner
        pickler.save_state = False
        
        mock_catalyst.environment_properties["SwVersion.Locale"] = 'en';
        mock_catalyst.environment_properties["Operator.Id"] = 'user';
        mock_catalyst.environment_properties["Device.Id"] = '9999999999';      
    
    def test0_0JobFoundCase(self):
        #-----------------------------
        # Config msg
        self._obj.runState(CONFIG_MSG)

        self.validate_prompts()                
        #-----------------------------
        # Get Sign on
        self.post_dialog_responses()
        self._obj.runState(SIGN_ON)
        
        self.validate_prompts('Welcome!')
        
        #-----------------------------
        # Get variables        
        self._obj.runState(GET_VARIABLES)
        
        self.validate_prompts()
        
        #-----------------------------
        # Config msg
        self._obj.runState(CONFIG_MSG)

        self.validate_prompts()                
        #-----------------------------
        # Get Sign on
        self.post_dialog_responses()
        self._obj.runState(SIGN_ON)
        
        self.validate_prompts('Welcome!')
        
        #-----------------------------
        # Get variables        
        self._obj.runState(GET_VARIABLES)
        
        self.validate_prompts()
        
        #-----------------------------
        # Get instructions
        task = VioTask()
        self._obj = InstructionProcessorTask(self._obj.variables, self._obj.send_to_host, self._obj.vcommands, VIO(), task)
        
        self.post_dialog_responses(
            '1VCONFIRM'
            , 'VCONFIRM'
            , '2VCONFIRM'
            , 'VCONFIRM'
            , '2.2VCONFIRM'
            , 'FVCONFIRM'
            , '22VCONFIRM'
            , '10VCONFIRM'
            , '27VCONFIRM'
            , 'VCONFIRM'
            , '10VCONFIRM'
            , '33VCONFIRM'
            , '44VCONFIRM'
            , 'VCONFIRM'
            , '01'
            , 'VCONFIRM'
            , '01'
            , '02'
            , 'VCOMMAND04'
            , 'VCONFIRM'
            , 'VCOMMAND04'
            , 'VCONFIRM') 
             
        TaskRunnerBase.get_main_runner()._append(self._obj)
        TaskRunnerBase.get_main_runner()._append(task)
        
        try:
            
            self._obj.execute()
            
        except Exception as ex:
            print(ex) 
            pass        
        
        self.validate_prompts(
            'Job?'
            , 'Start checklist message'
            , 'Question with priority'
            , 'Indicate number with confirmation'
            , '2 ?'
            , 'Indicate decimal number with range of 2 to 3 digits'
            , '2.2'
            , 'Indicate alphanumeric that can be skipped'
            , 'F'
            , 'Indicate date with short year, month and day'
            , 'Year'
            , 'Month'
            , 'Day'
            , ' Year 22 , Month 10 , Day 27 , ?'
            , 'Indicate time with hour, minute and second, with confirmation'
            , 'Hour'
            , 'Minute'
            , 'Second'
            , ' Hour 10 , Minute 33 , Second 44 , ?'
            , 'Indicate option with confirmation'
            , 'Option 1 ?'
            , 'Indicate multiple choice with confirmation'
            , 'Option 1'
            , 'Next'
            , 'Option 2'
            , 'Next'
            , ' , Option 1 , Option 2 ?'
            , 'beep'
            , 'beep'
            , 'Job finished with skipped questions. To complete it, say VCOMMAND04. To continue, say VCONFIRM'
            , 'Job finished. To continue, say VCONFIRM'
            , 'Job?')
       
if __name__ == "__main__":
    #import sys;sys.argv = ['', 'Test.testName']
    unittest.main()
