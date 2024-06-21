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
        mock_catalyst.environment_properties["Operator.Id"] = '7767';
        mock_catalyst.environment_properties["Device.Id"] = '9999999999';      
    
    def test0_0JobNotFoundCase(self):
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
            '66VCONFIRM') 
             
        TaskRunnerBase.get_main_runner()._append(self._obj)
        TaskRunnerBase.get_main_runner()._append(task)
        
        try:
            
            self._obj.execute()
            
        except Exception as ex:
            print(ex) 
            pass        
        
        self.validate_prompts(
            'Job?'
            , 'Job 66 not found')

    def test1_0messageCase(self):
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
            '1VCONFIRM' #paso 1
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , 'VCOMMAND01'
            , 'VCONFIRM'
            , '1VCONFIRM'
            , 'VCONFIRM'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , 'VCOMMAND02'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , '1VCONFIRM'
            , 'VCOMMAND02'
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
            'Job?' #paso 1
            , 'Start checklist messages'
            , 'Priority message'
            , 'Message with confirmation, to continue say VCONFIRM'
            , 'Message with additional information, to continue say VCONFIRM'
            , 'VCOMMAND03, correct?'
            , 'Message with confirmation, to continue say VCONFIRM'
            , 'Message with additional information, to continue say VCONFIRM'
            , 'VCOMMAND01, correct?'
            , 'Leaving job'
            , 'Job?'
            , 'Message with additional information, to continue say VCONFIRM'
            , 'Message that can be omitted, to continue say VCONFIRM'
            , 'VCOMMAND03, correct?'
            , 'Message with additional information, to continue say VCONFIRM'
            , 'Message that can be omitted, to continue say VCONFIRM'
            , 'VCOMMAND02, correct?'
            , 'Job finished with skipped questions. To complete it, say no more . To continue, say VCONFIRM'
            , 'Job?'
            , 'Message that can be omitted, to continue say VCONFIRM'
            , 'VCOMMAND02, correct?'
            , 'Job finished with skipped questions. To complete it, say no more . To continue, say VCONFIRM'
            , 'Job finished. To continue, say VCONFIRM'
            , 'Job?')

    def test2_0QuestionsCase(self):
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
            '2VCONFIRM' #paso 2
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , 'VCOMMAND01'
            , 'VCONFIRM'
            , '2VCONFIRM'
            , 'VCONFIRM'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , 'VCOMMAND02'
            , 'VCONFIRM'
            , 'VCONFIRM') 
             
        TaskRunnerBase.get_main_runner()._append(self._obj)
        TaskRunnerBase.get_main_runner()._append(task)
        
        try:
            
            self._obj.execute()
            
        except Exception as ex:
            print(ex) 
            pass        
        
        self.validate_prompts(
            'Job?' #paso 2
            , 'Start checklist questions'
            , 'Question without priority'
            , 'Question with priority'
            , 'VCOMMAND03, correct?'
            , 'Question without priority'
            , 'Question with priority'
            , 'VCOMMAND01, correct?'
            , 'Leaving job'
            , 'Job?'
            , 'Question with priority'
            , 'Question that can be omitted'
            , 'VCOMMAND03, correct?'
            , 'Question with priority'
            , 'Question that can be omitted'
            , 'VCOMMAND02, correct?'
            , 'Job finished with skipped questions. To complete it, say no more . To continue, say VCONFIRM'
            , 'Job?')

    def test3_0IntegerCase(self):
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
            '3VCONFIRM' #paso 3
            , 'VCOMMAND03'
            , '123456VCONFIRM'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , '12VCONFIRM'            
            , '12345678VCONFIRM'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , '1238764VCONFIRM'
            , '1234VCONFIRM'
            , 'VCONFIRM'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , '123VCONFIRM'
            , 'VCONFIRM'
            , '12VCONFIRM'
            , '9VCONFIRM'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , '18VCONFIRM'
            , '15VCONFIRM'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , '14VCONFIRM'
            , 'VCOMMAND02'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , '3VCONFIRM'
            , 'VCOMMAND03') 
             
        TaskRunnerBase.get_main_runner()._append(self._obj)
        TaskRunnerBase.get_main_runner()._append(task)
        
        try:
            
            self._obj.execute()
            
        except Exception as ex:
            print(ex) 
            pass        
        
        self.validate_prompts(
            'Job?' #paso 3
            , 'Start checklist integers'
            , 'Indicate number'
            , '123456'
            , 'Indicate number with scanner'
            , 'VCOMMAND03, correct?'
            , 'Indicate number'
            , '12'
            , 'Indicate number with scanner'
            , '12345678'
            , 'Indicate number with confirmation'
            , 'VCOMMAND03, correct?'
            , 'Indicate number with scanner'
            , '1238764'
            , 'Indicate number with confirmation'
            , '1234 ?'
            , 'Indicate number with range of 2 to 3 digits'
            , 'VCOMMAND03, correct?'
            , 'Indicate number with confirmation'
            , '123 ?'
            , 'Indicate number with range of 2 to 3 digits'
            , '12'
            , 'Indicate number with a minimum of 10 to 30'
            , 'Error Code 301.  Entered value is out of range.  Minimum value is 10 and maximum value is 30.'
            , 'VCOMMAND03, correct?'
            , 'Indicate number with range of 2 to 3 digits'
            , '18'
            , 'Indicate number with a minimum of 10 to 30'            
            , '15'
            , 'Indicate number that can be omitted'
            , 'VCOMMAND03, correct?'
            , 'Indicate number with a minimum of 10 to 30'
            , '14'
            , 'Indicate number that can be omitted'
            , 'VCOMMAND02, correct?'
            , 'Job finished with skipped questions. To complete it, say no more . To continue, say VCONFIRM'
            , 'Job?'
            , 'Indicate number that can be omitted')

    def test4_0DecimalCase(self):
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
            '4VCONFIRM' #paso 4
            , 'VCOMMAND03'
            , '1.2VCONFIRM'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , '3.2VCONFIRM'

            , '12345678VCONFIRM'
            , 'VCOMMAND01'
            , 'VCONFIRM'
            , '4VCONFIRM'
            , 'VCOMMAND03'
            
            , '1.2VCONFIRM'
            , 'VCONFIRM'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , '1.23VCONFIRM'
            , 'VCONFIRM'

            , '2.3VCONFIRM'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , '3.5VCONFIRM'

            , '12.3VCONFIRM'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , '14.3VCONFIRM'
            
            , 'VCOMMAND02'
            , 'VCONFIRM'
            , 'VCONFIRM') 
             
        TaskRunnerBase.get_main_runner()._append(self._obj)
        TaskRunnerBase.get_main_runner()._append(task)
        
        try:
            
            self._obj.execute()
            
        except Exception as ex:
            print(ex) 
            pass        
        
        self.validate_prompts(
            'Job?' #paso 4
            , 'Start checklist decimal numbers'
            , 'Indicate decimal number'
            , '1.2'
            , 'Indicate decimal number with scanner'
            , 'VCOMMAND03, correct?'
            , 'Indicate decimal number'
            , '3.2'
            , 'Indicate decimal number with scanner'

            , '12345678'
            , 'Indicate decimal number with confirmation'
            , 'VCOMMAND01, correct?'
            , 'Leaving job'
            , 'Job?'
            , 'Indicate decimal number with confirmation'
            , '1.2 ?'
            , 'Indicate decimal number with range of 2 to 3 digits'
            , 'VCOMMAND03, correct?'
            , 'Indicate decimal number with confirmation'
            , '1.23 ?'
            , 'Indicate decimal number with range of 2 to 3 digits'

            , '2.3'
            , 'Indicate decimal number with a minimum of 10 to 30'
            , 'VCOMMAND03, correct?'
            , 'Indicate decimal number with range of 2 to 3 digits'
            , '3.5'
            , 'Indicate decimal number with a minimum of 10 to 30'

            , '12.3'
            , 'Indicate decimal number that can be omitted'
            , 'VCOMMAND03, correct?'
            , 'Indicate decimal number with a minimum of 10 to 30'
            , '14.3'
            , 'Indicate decimal number that can be omitted'

            , 'VCOMMAND02, correct?'
            , 'Job finished with skipped questions. To complete it, say no more . To continue, say VCONFIRM'
            , 'Job?')

    def test5_0AlphanumericCase(self):
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
            '5VCONFIRM' #paso 5
            , 'ASFVCONFIRM'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , 'DFGVCONFIRM'

            , 'ASDF1234VCONFIRM'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , 'GHFB893SVCONFIRM'

            , 'A1VCONFIRM'
            , 'VNO'
            , 'AB23VCONFIRM'
            , 'VCONFIRM'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , 'BC12VCONFIRM'
            , 'VCONFIRM'

            , 'VCOMMAND01'
            , 'VCONFIRM'
            , '5VCONFIRM'
            , 'A1CVCONFIRM'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , 'BCDVCONFIRM'

            , 'VCOMMAND02'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , '5VCONFIRM'
            , 'A123BVCONFIRM'
            , 'VCONFIRM') 
             
        TaskRunnerBase.get_main_runner()._append(self._obj)
        TaskRunnerBase.get_main_runner()._append(task)
        
        try:
            
            self._obj.execute()
            
        except Exception as ex:
            print(ex) 
            pass        
        
        self.validate_prompts(
            'Job?' #paso 5
            , 'Start checklist alphanumeric values'
            , 'Indicate alphanumeric'
            , 'ASF'
            , 'Indicate alphanumeric with scanner'
            , 'VCOMMAND03, correct?'
            , 'Indicate alphanumeric'
            , 'DFG'
            , 'Indicate alphanumeric with scanner'

            , 'ASDF1234'
            , 'Indicate alphanumeric with confirmation'
            , 'VCOMMAND03, correct?'
            , 'Indicate alphanumeric with scanner'
            , 'GHFB893S'
            , 'Indicate alphanumeric with confirmation'

            , '<Spell>A1</Spell> ?'
            , 'Indicate alphanumeric with confirmation'
            , '<Spell>AB23</Spell> ?'
            , 'Indicate alphanumeric with range of 2 to 3 digits'
            , 'VCOMMAND03, correct?'
            , 'Indicate alphanumeric with confirmation'
            , '<Spell>BC12</Spell> ?'
            , 'Indicate alphanumeric with range of 2 to 3 digits'

            , 'VCOMMAND01, correct?'
            , 'Leaving job'
            , 'Job?'
            , 'Indicate alphanumeric with range of 2 to 3 digits'
            , 'A1C'
            , 'Indicate alphanumeric that can be omitted'
            , 'VCOMMAND03, correct?'
            , 'Indicate alphanumeric with range of 2 to 3 digits'
            , 'BCD'
            , 'Indicate alphanumeric that can be omitted'

            , 'VCOMMAND02, correct?'
            , 'Job finished with skipped questions. To complete it, say no more . To continue, say VCONFIRM'
            , 'Job?'
            , 'Indicate alphanumeric that can be omitted'
            , 'A123B'
            , 'Job finished. To continue, say VCONFIRM'
            , 'Job?')

    def test6_0DateCase(self):
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
            '6VCONFIRM' #paso 6
            , '01VCONFIRM'
            , '12VCONFIRM'
            , '1999VCONFIRM'
            , 'VNO'
            , '30VCONFIRM'
            , '02VCONFIRM'
            , '1957VCONFIRM'
            , '31VCONFIRM'
            , '04VCONFIRM'
            , '1957VCONFIRM'
            , '31VCONFIRM'
            , '06VCONFIRM'
            , '1957VCONFIRM'
            , '31VCONFIRM'
            , '09VCONFIRM'
            , '1957VCONFIRM'
            , '31VCONFIRM'
            , '11VCONFIRM'
            , '1957VCONFIRM'

            , '23VCONFIRM'
            , '03VCONFIRM'
            , '1957VCONFIRM'
            , 'VCONFIRM'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , '02VCONFIRM'
            , '12VCONFIRM'
            , '1999VCONFIRM'
            , 'VCONFIRM'

            , '01VCONFIRM'
            , '02VCONFIRM'
            , '94VCONFIRM'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , '02VCONFIRM'
            , '10VCONFIRM'
            , '74VCONFIRM'

            , 'VCOMMAND01'
            , 'VCONFIRM'
            , '06VCONFIRM'
            , '1985VCONFIRM'
            , '10VCONFIRM'
            , '31VCONFIRM'
            , '85VCONFIRM'
            , '10VCONFIRM'
            , '31VCONFIRM'
            , '1995VCONFIRM'
            , '04VCONFIRM'
            , '1423VCONFIRM'
            , '10VCONFIRM'
            , '01VCONFIRM'
            , '1846VCONFIRM'
            , '1956VCONFIRM'
            , '03VCONFIRM'
            , '55VCONFIRM'
            , '07VCONFIRM'
            , '12VCONFIRM'
            , '27VCONFIRM'
            , 'VCOMMAND01'
            , 'VCONFIRM'
            , '06VCONFIRM'
            , '27VCONFIRM'
            , '04VCONFIRM'
            , '1969VCONFIRM'
            , '89VCONFIRM'
            , '03VCONFIRM'
            , '23VCONFIRM'
            , 'VCOMMAND02'
            , 'VCONFIRM'
            , 'VCONFIRM') 
             
        TaskRunnerBase.get_main_runner()._append(self._obj)
        TaskRunnerBase.get_main_runner()._append(task)
        
        try:
            
            self._obj.execute()
            
        except Exception as ex:
            print(ex) 
            pass        
        
        self.validate_prompts(
            'Job?' #paso 6
            , 'Start checklist dates'
            , 'Indicate date with day, month and long year, with confirmation'
            , 'Day'
            , 'Month'
            , 'Year'
            , ' Day 01 , Month 12 , Year 1999 , ?'
            , 'Indicate date with day, month and long year, with confirmation'
            , 'Day'
            , 'Month'
            , 'Year'
            , 'Invalid date'
            , 'Indicate date with day, month and long year, with confirmation'
            , 'Day'
            , 'Month'
            , 'Year'
            , 'Invalid date'
            , 'Indicate date with day, month and long year, with confirmation'
            , 'Day'
            , 'Month'
            , 'Year'
            , 'Invalid date'
            , 'Indicate date with day, month and long year, with confirmation'
            , 'Day'
            , 'Month'
            , 'Year'
            , 'Invalid date'
            , 'Indicate date with day, month and long year, with confirmation'
            , 'Day'
            , 'Month'
            , 'Year'
            , 'Invalid date'
            , 'Indicate date with day, month and long year, with confirmation'
            , 'Day'
            , 'Month'
            , 'Year'

            , ' Day 23 , Month 03 , Year 1957 , ?'
            , 'Indicate date with day, month and short year'
            , 'Day'
            , 'VCOMMAND03, correct?'
            , 'Indicate date with day, month and long year, with confirmation'
            , 'Day'
            , 'Month'
            , 'Year'
            , ' Day 02 , Month 12 , Year 1999 , ?'
            , 'Indicate date with day, month and short year'
            , 'Day'

            , 'Month'
            , 'Year'
            , ' Day 01 , Month 02 , Year 94 ,'
            , 'Indicate date with long year, month and day'
            , 'Year'
            , 'VCOMMAND03, correct?'
            , 'Indicate date with day, month and short year'
            , 'Day'
            , 'Month'
            , 'Year'
            , ' Day 02 , Month 10 , Year 74 ,'
            , 'Indicate date with long year, month and day'
            , 'Year'

            , 'VCOMMAND01, correct?'
            , 'Leaving job'
            , 'Job?'
            , 'Indicate date with long year, month and day'
            , 'Year'
            , 'Month'
            , 'Day'
            , ' Year 1985 , Month 10 , Day 31 ,'
            , 'Indicate date with short year, month and day'
            , 'Year'
            , 'Month'
            , 'Day'
            , ' Year 85 , Month 10 , Day 31 ,'
            , 'Indicate date with long year and month'
            , 'Year'
            , 'Month'
            , ' Year 1995 , Month 04 ,'
            , 'Indicate date with short year and month'
            , 'Year'
            , 'Month'
            , ' Year 14 , Month 10 ,'
            , 'Indicate date with month and long year'
            , 'Month'
            , 'Year'
            , 'Error Code 301.  Entered value is out of range.  Minimum value is 1900 and maximum value is 2100.'
            , ' Month 01 , Year 1956 ,'
            , 'Indicate date with month and short year'
            , 'Month'
            , 'Year'
            , ' Month 03 , Year 55 ,'
            , 'Indicate date with month and day'
            , 'Month'
            , 'Day'
            , ' Month 07 , Day 12 ,'
            , 'Indicate date with day and month'
            , 'Day'
            , 'Month'
            , 'VCOMMAND01, correct?'
            , 'Leaving job'
            , 'Job?'
            , 'Indicate date with day and month'
            , 'Day'
            , 'Month'
            , ' Day 27 , Month 04 ,'
            , 'Indicate date with long year'
            , 'Year'
            , ' Year 1969 ,'
            , 'Indicate date with short year'
            , 'Year'
            , ' Year 89 ,'
            , 'Indicate date with month'
            , 'Month'
            , ' Month 03 ,'
            , 'Indicate date with day'
            , 'Day'
            , ' Day 23 ,'
            , 'Indicate date that can be omitted'
            , 'Day'
            , 'VCOMMAND02, correct?'
            , 'Job finished with skipped questions. To complete it, say no more . To continue, say VCONFIRM'
            , 'Job?')

    def test7_0TimeCase(self):
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
            '7VCONFIRM' #paso 7
            , '23VCONFIRM'
            , '59VCONFIRM'
            , '59VCONFIRM'
            , 'VNO'
            , '22VCONFIRM'
            , '59VCONFIRM'
            , '59VCONFIRM'
            , 'VCONFIRM'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , '15VCONFIRM'
            , '45VCONFIRM'

            , '32VCONFIRM'
            , 'VCONFIRM'
            , '32VCONFIRM'
            , '22VCONFIRM'
            , '49VCONFIRM'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , '14VCONFIRM'
            , '28VCONFIRM'

            , '51VCONFIRM'
            , '29VCONFIRM'
            , 'VCOMMAND01'
            , 'VCONFIRM'
            , '7VCONFIRM'
            , '2VCONFIRM'
            , '02VCONFIRM'
            , '08VCONFIRM'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , '15VCONFIRM'

            , '60VCONFIRM'
            , '32VCONFIRM'
            , '01VCONFIRM'
            , '01VCONFIRM'
            , 'VCOMMAND02'
            , 'VCONFIRM'
            , 'VCONFIRM') 
             
        TaskRunnerBase.get_main_runner()._append(self._obj)
        TaskRunnerBase.get_main_runner()._append(task)
        
        try:
            
            self._obj.execute()
            
        except Exception as ex:
            print(ex) 
            pass        
        
        self.validate_prompts(
            'Job?' #paso 7
            , 'Start checklist times'
            , 'Indicate time with hour, minute and second, with confirmation'
            , 'Hour'
            , 'Minute'
            , 'Second'
            , ' Hour 23 , Minute 59 , Second 59 , ?'
            , 'Indicate time with hour, minute and second, with confirmation'
            , 'Hour'
            , 'Minute'
            , 'Second'
            , ' Hour 22 , Minute 59 , Second 59 , ?'
            , 'Indicate time with hour and minute'
            , 'Hour'
            , 'VCOMMAND03, correct?'
            , 'Indicate time with hour, minute and second, with confirmation'
            , 'Hour'
            , 'Minute'
            , 'Second'
            , ' Hour 15 , Minute 45 , Second 32 , ?'
            , 'Indicate time with hour and minute'
            , 'Hour'

            , 'Error Code 301.  Entered value is out of range.  Minimum value is 0 and maximum value is 23.'
            , 'Minute'
            , ' Hour 22 , Minute 49 ,'
            , 'Indicate time with minute and second'
            , 'Minute'
            , 'VCOMMAND03, correct?'
            , 'Indicate time with hour and minute'
            , 'Hour'
            , 'Minute'
            , ' Hour 14 , Minute 28 ,'
            , 'Indicate time with minute and second'
            , 'Minute'

            , 'Second'
            , ' Minute 51 , Second 29 ,'
            , 'Indicate time with hour'
            , 'Hour'
            , 'VCOMMAND01, correct?'
            , 'Leaving job'
            , 'Job?'
            , 'Indicate time with hour'
            , 'Hour'
            , 'Error Code 304.  Entered value must be at least 2 digits.'
            , ' Hour 02 ,'
            , 'Indicate time with minute'
            , 'Minute'
            , ' Minute 08 ,'
            , 'Indicate time with second'
            , 'Second'
            , 'VCOMMAND03, correct?'
            , 'Indicate time with minute'
            , 'Minute'
            , ' Minute 15 ,'
            , 'Indicate time with second'
            , 'Second'

            , 'Error Code 301.  Entered value is out of range.  Minimum value is 0 and maximum value is 59.'
            , ' Second 32 ,'
            , 'Indicate time that can be omitted'
            , 'Hour'
            , 'Minute'
            , 'Second'
            , 'VCOMMAND02, correct?'
            , 'Job finished with skipped questions. To complete it, say no more . To continue, say VCONFIRM'
            , 'Job?')

    def test8_0SelectCase(self):
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
            '8VCONFIRM' #paso 8
            , '01'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , '03'

            , '02'
            , 'VNO'
            , '03'
            , 'VCONFIRM'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , '01'
            , 'VCONFIRM'

            , 'VCOMMAND02'
            , 'VCONFIRM'
            , 'VCONFIRM') 
             
        TaskRunnerBase.get_main_runner()._append(self._obj)
        TaskRunnerBase.get_main_runner()._append(task)
        
        try:
            
            self._obj.execute()
            
        except Exception as ex:
            print(ex) 
            pass        
        
        self.validate_prompts(
            'Job?' #paso 8
            , 'Start checklist selection'
            , 'Indicate option'
            , 'Option 1'
            , 'Indicate option with confirmation'
            , 'VCOMMAND03, correct?'
            , 'Indicate option'
            , 'Option 3'
            , 'Indicate option with confirmation'

            , 'Option 2 ?'
            , 'Indicate option with confirmation'
            , 'Option 3 ?'
            , 'Indicate option that can be omitted'
            , 'VCOMMAND03, correct?'
            , 'Indicate option with confirmation'
            , 'Option 1 ?'
            , 'Indicate option that can be omitted'

            , 'VCOMMAND02, correct?'
            , 'Job finished with skipped questions. To complete it, say no more . To continue, say VCONFIRM'
            , 'Job?')

    def test9_0SelectMultipleCase(self):
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
            '9VCONFIRM' #paso 9
            , '02'
            , '06'
            , '01'
            , 'VCOMMAND04'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , '03'
            , 'VCOMMAND04'

            , '01'
            , '02'
            , '03'
            , 'VCOMMAND04'
            , 'VNO'
            , '01'
            , '02'
            , 'VCOMMAND04'
            , 'VCONFIRM'
            , 'VCOMMAND03'
            , 'VCONFIRM'
            , '03'
            , '02'
            , '01'
            , 'VCOMMAND04'
            , 'VCONFIRM'

            , '01'
            , 'VCOMMAND02'
            , 'VCONFIRM'
            , 'VCONFIRM') 
             
        TaskRunnerBase.get_main_runner()._append(self._obj)
        TaskRunnerBase.get_main_runner()._append(task)
        
        try:
            
            self._obj.execute()
            
        except Exception as ex:
            print(ex) 
            pass        
        
        self.validate_prompts(
            'Job?' #paso 9
            , 'Start checklist multiple choice'
            , 'Indicate multiple choice'
            , 'Option 2'
            , 'Next'
            , '06 Wrong'
            , 'Next'
            , 'Option 1'
            , 'Next'
            , ' , Option 2 , Option 1'
            , 'Indicate multiple choice with confirmation'
            , 'VCOMMAND03, correct?'
            , 'Indicate multiple choice'
            , 'Option 3'
            , 'Next'
            , ' , Option 3'
            , 'Indicate multiple choice with confirmation'

            , 'Option 1'
            , 'Next'
            , 'Option 2'
            , 'Next'
            , 'Option 3'
            , 'Next'
            , ' , Option 1 , Option 2 , Option 3 ?'
            , 'Indicate multiple choice with confirmation'
            , 'Option 1'
            , 'Next'
            , 'Option 2'
            , 'Next'
            , ' , Option 1 , Option 2 ?'
            , 'Indicate option that can be omitted'
            , 'VCOMMAND03, correct?'
            , 'Indicate multiple choice with confirmation'
            , 'Option 3'
            , 'Next'
            , 'Option 2'
            , 'Next'
            , 'Option 1'
            , 'Next'
            , ' , Option 3 , Option 2 , Option 1 ?'
            , 'Indicate option that can be omitted'

            , 'Option 1'
            , 'Next'
            , 'VCOMMAND02, correct?'
            , 'Job finished with skipped questions. To complete it, say no more . To continue, say VCONFIRM'
            , 'Job?')

if __name__ == "__main__":
    #import sys;sys.argv = ['', 'Test.testName']
    unittest.main()
