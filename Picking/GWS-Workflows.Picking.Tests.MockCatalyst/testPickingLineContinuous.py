# -*- coding: cp1252 -*-

import unittest
from VioBaseTestCase import VioBaseTestCase
from vio import VIO
from vocollect_core.task.task_runner import TaskRunnerBase
from VioTask import VioTask
from Constants import CONFIG_MSG, SIGN_ON, GET_VARIABLES
from vocollect_core.utilities import pickler
from InstructionProcessor import InstructionProcessorTask
import time
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
    
    #@unittest.skip("demonstrating skipping")
    def test01PickingLine(self):
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
            'VCONFIRM' #'Order 001 , Customer , in container'
            , 'VCONFIRM' #'Order header message' 
            , 'VCONFIRM'
            , 'VCONFIRM'
            , 'VCOMMAND03'
            , 'VYES'
            , '01'
            , 'VYES'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , 'VCOMMAND07'
            , 'VCOMMAND08'
            , 'VCOMMAND11'
            , 'VCOMMAND06'
            , '123'
            , 'VCONFIRM'
            , '1VCONFIRM'
            , '7VCONFIRM'
            , '3VCONFIRM'
            , 'VCOMMAND04'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , '123'
            , 'VCOMMAND10'
            , 'VYES'
            , '2VCONFIRM'
            , 'VYES'
            , '2VCONFIRM'
            , 'VYES'
            , '2VCONFIRM'
            , 'VYES'
            , 'VCONFIRM'
            , '123'
            , 'VCOMMAND01'
            , 'VYES'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , '123'
            , '4VCONFIRM'
            , '5VCONFIRM'
            , 'VYES'
            , '30VCONFIRM'
            , 'VYES'
            , '20VCONFIRM'
            , 'VYES'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , '123'
            , 'VCOMMAND12'
            , 'VYES'
            , '49VCONFIRM'
            , 'VCONFIRM'
            , '123'
            , 'VCOMMAND13'
            , '240VCONFIRM'
            , 'VCONFIRM'
            , '123'
            , 'VCOMMAND13'
            , '210VCONFIRM'
            , 'VYES'
            , 'VCOMMAND02'
            , 'VYES'
            , '1VCONFIRM'
            , 'VYES'
            , 'VYES'
            , '123'
            , '456'
            , '1VCONFIRM'
            , '3VCONFIRM' # 'Unknown printer , How many labels?'
            , 'VCONFIRM' # '3 ?'
            , 'VCOMMAND05' # 'Unknown printer , How many labels?'
            , '7VCONFIRM' # 'Printer?'
            , '2VCONFIRM' # 'Printer?'
            , '4VCONFIRM' # 'Printer 2 , How many labels?'
            , 'VCONFIRM' # '4 ?'
            , 'VYES' # 'Correct printing?'
            , 'VNO' # 'Correct printing?'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , '123'
            , '4VCONFIRM'
            , 'VYES'
            , 'VNO'
            , 'VSIGNOFF')
             
        TaskRunnerBase.get_main_runner()._append(self._obj)
        TaskRunnerBase.get_main_runner()._append(task)
        
        try:
            
            self._obj.execute()
            
        except Exception as ex:
            print(ex) 
            pass        
        
        self.validate_prompts(
              'Order 001 , Customer , in container'
            , 'Order header message' # (1.1) Test resting in picking line
            , 'First line'
            , 'Aisle 1'
            , 'Slot A'
            , 'VCOMMAND03, correct?'
            , 'Rest reason?'
            , 'WC ?'
            , 'At rest. To resume work, say VCONFIRM'
            , 'First line'
            , 'Aisle 1'
            , 'Slot A'
            , '01'
            , 'Aisle 1 , Slot A' # Where am I command
            , '01'
            , 'test 1.1' # Product command
            , '01'
            , 'Customer' # Customer command
            , '01'
            , 'There are 2 lines remaining'
            , '01' # Pending command
            , 'test 1.1' # (1.2) Pronounce the product name
            , '3 boxes'
            , 'Error Code 301.  Entered value is out of range.  Minimum value is 2 and maximum value is 4.'
            , 'Error Code 301.  Entered value is out of range.  Minimum value is 2 and maximum value is 4.'
            , '3 boxes'
            , 'Slot A'
            , 'VCOMMAND04, correct?' # (1.2) Test skipping picking line
            , 'Slot A'
            , '03'
            , '3 boxes' 
            , 'VCOMMAND10, correct?' # (1.3) Test declaring breakage
            , 'Breakage quantity?'
            , '2 ?'
            , '3 boxes' # (1.3) Test picking less quantity
            , '2 is less than the amount requested. Are you sure?'
            , 'Quantity in location' # Test stoc counting when quantity is partial
            , '2 ?'
            , 'Slot B'
            , '01'
            , '3 boxes' # (1.4) Test declaring exception in picking line
            , 'VCOMMAND01, correct?'
            , 'Aisle 2'
            , 'Slot A'
            , '01'
            , '4 boxes'
            , 'Weight?' # (1.5) Test picking line with wheight
            , '5 ?'
            , 'The minimum weight is 10.0 and the maximum weight is 25.0'
            , 'Weight?'
            , '30 ?'
            , 'The minimum weight is 10.0 and the maximum weight is 25.0'
            , 'Weight?'
            , '20 ?'
            , 'Aisle 3'
            , 'Slot A'
            , '01'
            , '4 boxes' 
            , '49 packages' # (1.6) Test change to alternative preparation format
            , 'Slot A'
            , '03'
            , '4 boxes'
            , '240 units' # (1.7) Test change to units preparation format
            , 'Slot A'
            , '04'
            , '4 boxes'
            , '240 units'
            , '210 is less than the amount requested. Are you sure?' # (1.8) Test picking less units than requested
            , 'Slot A'
            , 'VCOMMAND02, correct?' # (1.9) Test end pallet
            , 'Printer 1 , How many labels?'
            , '1 ?'
            , 'Correct printing?'
            , 'Validate labels'
            , ', remain 2'
            , ', remain 1'
            , 'Validation completed'
            , 'Place in dock'
            , 'Specify destination'
            , 'Printer unknown , How many labels?'
            , '3 ?'
            , 'Printer unknown , How many labels?'
            , 'Printer?'
            , 'Unknown printer'
            , 'Printer?'
            , 'Printer 2 , How many labels?'
            , '4 ?'
            , 'Correct printing?' # (1.9.4) Valid printing
            , 'Correct printing?' # (1.9.5) Invalid printing
            , 'Retrying...'
            , 'Aisle 3' 
            , 'Slot A'
            , '05'
            , '4 boxes' # (1.9) Confirm line quantity
            , 'Place in dock'
            , 'Dock 1' # (1.10) Test placing in dock with dock informed
            , 'This is a question'
            , 'This is another question'
            , 'Order 001 , Customer , in container'
            , 'Exiting the system. See you next time'
            , 'Order 001 , Customer , in container'
            )
    
    def test02PickingLine_AskBatch(self):
        time.sleep(2) # Sleep for 2 seconds
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
            'VCONFIRM'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , '123'
            , 'VNO'
            , 'VYES'
            , '3VCONFIRM'
            , 'VCONFIRM'
            , '123'
            , 'VCANCEL'
            , '3VCONFIRM'
            , 'VSIGNOFF')
             
        TaskRunnerBase.get_main_runner()._append(self._obj)
        TaskRunnerBase.get_main_runner()._append(task)
        
        try:
            
            self._obj.execute()
            
        except Exception as ex:
            print(ex) 
            pass        
        
        self.validate_prompts(
              'Order 001 , Customer , in container'
            , 'Aisle 1'
            , 'Slot A'
            , '01'
            , 'Select batch'
            , 'Batch 1?' # Test select second option
            , 'Batch 2?'
            , '3 boxes'
            , 'Slot A'
            , '02'
            , 'Select batch' # Test cancel selection
            , 'Batch 1?'
            , '3 boxes'
            , 'Order 001 , Customer , in container'
            , 'Exiting the system. See you next time'
            , 'Order 001 , Customer , in container'
            )
    
    def test03PickingLine_AskPackage(self):
        time.sleep(2) # Sleep for 2 seconds
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
            'VCONFIRM'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , '123'
            , '3VCONFIRM'
            , 'VYES'
            , 'VCONFIRM'
            , '123'
            , '3VCONFIRM'
            , 'VNO'
            , 'VSIGNOFF')
             
        TaskRunnerBase.get_main_runner()._append(self._obj)
        TaskRunnerBase.get_main_runner()._append(task)
        
        try:
            
            self._obj.execute()
            
        except Exception as ex:
            print(ex) 
            pass        
        
        self.validate_prompts(
              'Order 001 , Customer , in container'
            , 'Aisle 1'
            , 'Slot A'
            , '01'
            , '3 boxes'
            , 'Contains packaging?' # Says yes
            , 'Slot A'
            , '02'
            , '3 boxes'
            , 'Contains packaging?' # Says no
            , 'Order 001 , Customer , in container'
            , 'Exiting the system. See you next time'
            , 'Order 001 , Customer , in container'
            )
    
    def test04PickingLine_ValidateLocation(self):
        time.sleep(2) # Sleep for 2 seconds
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
            'VCONFIRM'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , '123'
            , '3VCONFIRM'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , '3VCONFIRM'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , '123'
            , '3VCONFIRM'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , '3VCONFIRM'
            , 'VSIGNOFF')
             
        TaskRunnerBase.get_main_runner()._append(self._obj)
        TaskRunnerBase.get_main_runner()._append(task)
        
            
        try:
            
            self._obj.execute()
            
        except Exception as ex:
            print(ex) 
            pass   
        
        
        self.validate_prompts(
              'Order 001 , Customer , in container'
            , 'Test 4.1: Slot not supplied'
            , 'Aisle 1'
            , '01'
            , '3 boxes'
            , 'Test 4.2: Control digit not supplied'
            , 'Aisle 1'
            , 'Slot A'
            , '02'
            , '3 boxes'
            , 'Test 4.3: Position not supplied'
            , 'Aisle 1'
            , 'Control digit?'
            , '3 boxes'
            , 'Test 4.4: Position and Control Digit not supplied'
            , 'Aisle 1'
            , 'Slot A'
            , '3 boxes'
            , 'Order 001 , Customer , in container'
            , 'Exiting the system. See you next time'
            , 'Order 001 , Customer , in container'
            )
    
    def test05PickingLine_ValidateProductWithCDs(self):
        time.sleep(2) # Sleep for 2 seconds
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
            'VCONFIRM'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , '123'
            , '111VCONFIRM'
            , '123VCONFIRM'
            , '3VCONFIRM'           
            , 'VSIGNOFF')
             
        TaskRunnerBase.get_main_runner()._append(self._obj)
        TaskRunnerBase.get_main_runner()._append(task)
        
            
        try:
            
            self._obj.execute()
            
        except Exception as ex:
            print(ex) 
            pass   
        
        
        self.validate_prompts(
              'Order 001 , Customer , in container'
            , 'Test 5.1: List of codes to be validated'
            , 'Aisle 1'
            , 'Slot A'
            , '01'
            , 'test 5.1'
            , 'Control digit?'
            , 'Incorrect, 111'
            , 'Control digit?'
            , '3 boxes'           
            , 'Order 001 , Customer , in container'
            , 'Exiting the system. See you next time'
            , 'Order 001 , Customer , in container'
            )
    
    def test06PickingLine_PickingQuantityPartially(self):
        time.sleep(2) # Sleep for 2 seconds
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
            'VCONFIRM'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , 'VCONFIRM'
            , '123'
            , '1VCONFIRM'
            , '1VCONFIRM'
            , '1VCONFIRM'
            , 'VCONFIRM'
            , '123'
            , '2VCONFIRM'
            , '0VCONFIRM'
            , '01'
            , 'VYES'
            , '2VCONFIRM'
            , 'VYES'
            , 'VCONFIRM'
            , '123'
            , '2VCONFIRM'
            , '1VCONFIRM'
            , '1VCONFIRM'
            , '1VCONFIRM'
            , 'VCONFIRM'
            , '123'
            , '4VCONFIRM'
            , 'VYES'
            , 'VCONFIRM'
            , '123'
            , '7VCONFIRM'
            , '3VCONFIRM'
            , 'VCONFIRM'
            , '123'
            , '2VCONFIRM'
            , '11VCONFIRM'
            , 'VYES'
            , '1VCONFIRM'
            , '6.5VCONFIRM'
            , 'VYES'
            , 'VCONFIRM'
            , '123'
            , '3VCONFIRM'
            , '10VCONFIRM'
            , 'VYES'
            , '50VCONFIRM'
            , 'VYES'
            , '15VCONFIRM'
            , 'VYES'
            , 'VSIGNOFF')
             
        TaskRunnerBase.get_main_runner()._append(self._obj)
        TaskRunnerBase.get_main_runner()._append(task)
        
        try:
            
            self._obj.execute()
            
        except Exception as ex:
            print(ex) 
            pass        
        
        self.validate_prompts(
              'Order 001 , Customer , in container'
            , 'Aisle 1'
            , 'Slot A'
            , '01'
            , '3 boxes'
            , 'Remain 2'
            , 'Remain 1'
            , 'Slot A'           
            , '02'
            , '3 boxes' 
            , 'Remain 1'
            , '2 is less than the amount requested. Reason?'
            , 'Empty ?'
            , 'Quantity in location' # Stock counting when quantity is less than requested
            , '2 ?'
            , 'Slot A'
            , '03'
            , '3 boxes'
            , 'The maximum quantity allowed per pick is 1'
            , '3 boxes'
            , 'Remain 2'
            , 'Remain 1'
            , 'Slot A'
            , '04'
            , '3 boxes'
            , '4 , correct?' # Quantity greater but within ranges
            , 'Slot A'
            , '05'
            , '3 boxes'
            , 'Quantity greater than requested' # Quantity greater
            , '3 boxes'
            , 'Slot A'
            , '06'
            , '3 boxes'
            , 'Weight?'
            , '11 ?'
            , 'Remain 1'
            , 'Weight?'
            , '6.5 ?'
            , 'Slot A'
            , '07'
            , '3 boxes'
            , 'Weight?'
            , '10 ?'
            , 'The minimum weight is 15.0'
            , 'Weight?'
            , '50 ?'
            , 'The maximum weight is 45.0'
            , 'Weight?'
            , '15 ?'
            , 'Order 001 , in container , 3 containers'
            , 'Exiting the system. See you next time'
            , 'Order 001 , in container , 3 containers'
            )
    
    def test07MultiOrder(self):

        time.sleep(2) # Sleep for 2 seconds
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
                                      'VCONFIRM'
                                    , 'VCONFIRM'
                                    , 'VCONFIRM'
                                    , '123'
                                    , '3VCONFIRM'
                                    , 'VCONFIRM'
                                    , '123'
                                    , '3VCONFIRM'
                                    , 'VCONFIRM'
                                    , '123'
                                    , '3VCONFIRM'
                                    , 'VCONFIRM'
                                    , '123'
                                    , '3VCONFIRM'
                                    , '1VCONFIRM'
                                    , 'VYES'
                                    , 'VYES'
                                    , '123'
                                    , '456'
                                    , 'VCONFIRM'
                                    , '123'
                                    , 'VCONFIRM'
                                    , 'VCONFIRM'
                                    , '123'
                                    , '4VCONFIRM'
                                    , 'VCONFIRM'
                                    , 'VCONFIRM'
                                    , 'VCONFIRM'
                                    , '123'
                                    , '4VCONFIRM'
                                    , 'VCONFIRM'
                                    , '123'
                                    , 'VSIGNOFF') 
        
        
        TaskRunnerBase.get_main_runner()._append(self._obj)
        TaskRunnerBase.get_main_runner()._append(task)
        
        try:
            
            self._obj.execute()
            
        except Exception as ex:
            print(ex) 
            pass  
        
        self.validate_prompts('Order 001 , in container , 3 containers'
                                , 'Aisle 1'
                                , 'Slot A'
                                , '01'
                                , '3 boxes in A'
                                , 'Slot A'
                                , '02'
                                , '3 boxes in B' 
                                , 'Slot A'
                                , '02'
                                , '3 boxes in C' 
                                , 'Slot B'
                                , '01'
                                , '3 boxes in B'
                                , 'Printer 1 , How many labels?'
                                , '1 ?'
                                , 'Correct printing?'
                                , 'Validate labels'
                                , ', remain 2'
                                , ', remain 1'
                                , 'Validation completed'
                                , 'Container B to dock'
                                , 'Place in dock'
                                , 'Dock 2'
                                , 'Aisle 2'
                                , 'Slot A'
                                , '01'
                                , '4 boxes in C'
                                , 'Container C to dock'
                                , 'Place in dock'
                                , 'Dock 3'                                
                                , 'Aisle 3'
                                , 'Slot A'
                                , '01'
                                , '4 boxes in A'
                                , 'Container A to dock'
                                , 'Place in dock'
                                , 'Dock 1'
                                , 'Order 001'
                                , 'Exiting the system. See you next time'
                                , 'Order 001')

    
    def test08StockCouting(self):

        time.sleep(2) # Sleep for 2 seconds
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
                                      'VCONFIRM'
                                    , 'VCONFIRM'
                                    , 'VCONFIRM'
                                    , 'VCONFIRM'
                                    , 'VCONFIRM'
                                    , '123'
                                    , '10VCONFIRM'
                                    , 'VYES'
                                    , 'VCOMMAND04'
                                    , 'VYES'
                                    , 'VCOMMAND01'
                                    , 'VYES'
                                    , 'VCONFIRM'
                                    , '123'
                                    , '15VCONFIRM'
                                    , 'VYES'
                                    , 'VCONFIRM'
                                    , 'VCONFIRM'
                                    , 'VSIGNOFF') 
        
        
        TaskRunnerBase.get_main_runner()._append(self._obj)
        TaskRunnerBase.get_main_runner()._append(task)
        
        try:
            
            self._obj.execute()
            
        except Exception as ex:
            print(ex) 
            pass  
        
        self.validate_prompts('Order 001'
                                , 'Order header message'
                                , 'First line stock counting'
                                , 'Aisle 1'
                                , 'Slot A'
                                , '01'
                                , 'Quantity in location'
                                , '10 ?'
                                , 'Slot A'
                                , 'VCOMMAND04, correct?' # (1.2) Test skipping line
                                , 'Slot A'
                                , 'VCOMMAND01, correct?' # (1.2) Test declaring exception line 
                                , 'Slot B'
                                , '01'
                                , 'Quantity in location'
                                , '15 ?'
                                , 'Empty work. Used to inform a message and get new work when operator say VCONFIRM'
                                , 'No work assigned. To try again, say VCONFIRM'
                                , 'No work assigned. To try again, say VCONFIRM'
                                , 'Exiting the system. See you next time'
                                , 'No work assigned. To try again, say VCONFIRM')
    
if __name__ == "__main__":
    #import sys;sys.argv = ['', 'Test.testName']
    unittest.main()
