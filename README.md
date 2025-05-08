### **Test Machine Code Structure**  
This codebase consists of two main components:  

1. **Test Platform**  
   - Location: `SolveWare_TesterBranch`  
   - Main configuration file: `LaserX_TesterLibrary/SystemConfig/StationConfigs.xml`  

2. **Test Plugin**  
   - Location: `TestPlugin/TestPlugin_Demo`  
   - The plugin utilizes hardware resources from the test platform to perform actions, including:  
     - Motion control  
     - Instrument control  
     - Test module invocation  

#### **Hardware Resource Configuration**  
   - **3.1 Hardware Resources**  
     - Config path: `InstrumentConfigs` node in `StationConfigs.xml`  
   - **3.2 Hardware Communication**  
     - Config path: `InstrumentChassisConfigs` node in `StationConfigs.xml`  
   - **3.3 Hardware Instrument Library**  
     - Location: `SolveWare_BurnInInstruments`  
   - **3.4 Test Module Library**  
     - Location: `SolveWare_TestPackage`  

#### **Integration Guide for New Test Modules**  
For adding new test modules, refer to the reference implementation:  
- `Mirror Tuning/ModuleAndRecipe_MTuning/TestModule_MTuning.cs`  
- `Mirror Tuning/ModuleAndRecipe_MTuning/TestRecipe_MTuning.cs`  

These files demonstrate:  
- Instrument invocation methods  
- Inter-triggering functionality  
- Other core interactions  
