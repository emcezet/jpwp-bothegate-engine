using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace jpwp_bothegate_engine
{
    /// <summary>
    /// Class LogicSystem.
    /// This is the basic class to use LogicDevice. Each System is a group of devices connected in a set fashion.
    /// Approach similar to SPICE3 was used, where we understand that a pair of nodes is connected, if they have the same name.
    /// </summary>
    public class LogicSystem
    {
        private List<LogicDevice> netlist;
        public List<LogicDevice> Netlist { get; set; }
        public LogicSystem()
        {
            List<LogicDevice> tnetlist = new List<LogicDevice>();
            Netlist = tnetlist;
        }
        public LogicSystem(List<LogicDevice> tnetlist)
        {
            Netlist = tnetlist;
        }
        /// <summary>
        /// Replace this pseduorandom algorithm with a more sophisiticated for better expierience in Skirmish Mode.
        /// </summary>
        /// <param name="noInputs"></param>
        /// <param name="noOutputs"></param>
        public LogicSystem(int noInputs, int noOutputs)
        {
            List<LogicDevice> ttnetlist = new List<LogicDevice>();
            int idCounter = 0;
            String[] portNames = { Convert.ToString(idCounter) };
            for (int i = 0; i < noInputs; i++)
            {
                ttnetlist.Add(new DigitalSource(portNames, idCounter));
                idCounter++;
            }
            for (int i = 0; i < noOutputs; i++)
            {
                ttnetlist.Add(new DigitalSink(portNames, idCounter));
                idCounter++;
            }
            //List<int> sources = getDigitalSourcesIndex();
            //List<int> sinks = getDigitalSinksIndex();
            Random rnd = new Random();
            for (int sinkIndex = noInputs; sinkIndex < noInputs+noOutputs; sinkIndex++)
            {
                String outputPortName = Convert.ToString(sinkIndex);
                
                String[] portNamesGates = { Convert.ToString(rnd.Next(0,noInputs)), Convert.ToString(rnd.Next(0, noInputs)), outputPortName };
                switch (rnd.Next(1,5))
                {
                    case 1:
                        ttnetlist.Add(new NANDGate(portNamesGates, idCounter));
                        break;
                    case 2:
                        ttnetlist.Add(new ANDGate(portNamesGates, idCounter));
                        break;
                    case 3:
                        ttnetlist.Add(new ORGate(portNamesGates, idCounter));
                        break;
                    case 4:
                        ttnetlist.Add(new NORGate(portNamesGates, idCounter));
                        break;
                    case 5:
                        ttnetlist.Add(new NOTGate(portNamesGates, idCounter));
                        break;
                    default:
                        break;
                }
                idCounter++;
            }
            Netlist = ttnetlist;
            }
        public LogicSystem(JsonNetlist tnetlist)
        {
            List<LogicDevice> ttnetlist = new List<LogicDevice>();
            for (int tnetlistIndex = 0; tnetlistIndex < tnetlist.LogicDevices.Count; tnetlistIndex++)
            {
                LogicDevice dev = new LogicDevice();
                dev = tnetlist.LogicDevices[tnetlistIndex];
                String[] portNames = new String[dev.Ports.Count];
                for (int ports = 0; ports < dev.Ports.Count; ports++)
                {
                    portNames[ports] = dev.Ports[ports].Key;
                }
                switch (tnetlist.Type[tnetlistIndex].ToLower())
                {
                    case "nand2":
                        ttnetlist.Add(new NANDGate(portNames, dev.Id));
                        break;
                    case "and2":
                        ttnetlist.Add(new ANDGate(portNames, dev.Id));
                        break;
                    case "or2":
                        ttnetlist.Add(new ORGate(portNames, dev.Id));
                        break;
                    case "nor2":
                        ttnetlist.Add(new NORGate(portNames, dev.Id));
                        break;
                    case "v":
                        ttnetlist.Add(new DigitalSource(portNames, dev.Id));
                        break;
                    case "s":
                        ttnetlist.Add(new DigitalSink(portNames, dev.Id));
                        break;
                    case "not":
                        ttnetlist.Add(new NOTGate(portNames, dev.Id));
                        break;
                    case "high":
                        ttnetlist.Add(new DigitalHigh(portNames, dev.Id));
                        break;
                    case "low":
                        ttnetlist.Add(new DigitalLow(portNames, dev.Id));
                        break;
                    default:
                        break;
                        }
                Netlist = ttnetlist;
            }
        }
        /// <summary>
        /// Method : void loadNetlist(String fileName)
        /// Created for CLI debugging. Further use not recommended.
        /// </summary>
        /// <param name="fileName"></param>
        public void loadNetlist(String fileName)
        {
            String[] netlistLines;
            int lineNumber=0;
            String[] portNames = new String[] { };
            String[] typeAndId = new String[] { };
            String type ="";
            int id=0;
            String[] spacedLine = new String[] { };
            netlistLines = System.IO.File.ReadAllLines(fileName);
            foreach (var line in netlistLines)
            {
                Utils.infoPrint("Line of Netlist:" + lineNumber + " | " + line);
                if ( lineNumber == 0)
                {
                    Utils.debugPrint("No processing on line 0.");
                }
                else
                {
                    spacedLine = line.Split(' ','\t');
                    typeAndId = Regex.Split(spacedLine[0], "(?=[0-9])");
                    type = typeAndId[0];
                    id = Convert.ToInt32(typeAndId[1]);
                    Array.Resize(ref portNames, spacedLine.Length - 1);
                    for (int i = 1; i < spacedLine.Length; i++)
                    {
                        //Utils.debugPrint("spacedLine \t" + spacedLine[i]);
                        portNames[i-1] = spacedLine[i];
                    }
                    //Utils.debugPrint("portNames.Length"+portNames.Length);
                    switch (type.ToLower())
                    {
                        case "nand":
                            Netlist.Add(new NANDGate(portNames, id));
                            break;
                        case "nor":
                            //
                            break;
                        default:
                            Utils.infoPrint("Unknown type of logic device.");
                            break;
                    }
                }
                lineNumber++;
            }
        }

        public List<int> getDigitalSourcesIndex()
        {
            List<int> result = new List<int>();
            for (int deviceIndex = 0; deviceIndex < Netlist.Count; deviceIndex++)
            {
                if(Netlist[deviceIndex].GetType().Name == "DigitalSource")
                {
                    result.Add(deviceIndex);
                }
            }
            return result;
        }
        public List<bool> getDigitalSourcesValue()
        {
            List<bool> result = new List<bool>();
            List<int> index = getDigitalSourcesIndex();
            for (int deviceIndex = 0; deviceIndex < index.Count; deviceIndex++)
            {
                result.Add(Netlist[index[deviceIndex]].Ports[0].Value);
            }
            return result;
        }
        public List<int> getDigitalSinksIndex()
        {
            List<int> result = new List<int>();
            for (int deviceIndex = 0; deviceIndex < Netlist.Count; deviceIndex++)
            {
                if (Netlist[deviceIndex].GetType().Name == "DigitalSink")
                {
                    result.Add(deviceIndex);
                }
            }
            return result;
        }
        public List<bool> getDigitalSinksValue()
        {
            List<bool> result = new List<bool>();
            List<int> index = getDigitalSinksIndex();
            for (int deviceIndex = 0; deviceIndex < index.Count; deviceIndex++)
            {
                result.Add(Netlist[index[deviceIndex]].Ports[0].Value);
            }
            return result;
        }
        public int getInputVectorDecimal(List<bool> vector)
        {
            List<String> vectorS = new List<String>();
            for (int vectorIndex = 0; vectorIndex < vector.Count; vectorIndex++)
            {
                vectorS.Add(Convert.ToString(Convert.ToInt32(vector[vectorIndex])));
            }
            String resultS = "";
            for (int vectorIndex = 0; vectorIndex < vectorS.Count; vectorIndex++)
            {
                resultS += vectorS[vectorS.Count-1-vectorIndex];
            }
            int result = Convert.ToInt32(resultS,2);
            return result;
        }
        public List<bool> getIncrementedInputVector(int input)
        {
            String vectorS = Convert.ToString(input+1, 2);
            List<bool> inputVector = new List<bool>();
            for (int vectorIndex = 0; vectorIndex < vectorS.Length; vectorIndex++)
            {
                int temp = Convert.ToInt32(vectorS[vectorIndex]);
                inputVector.Add(Convert.ToBoolean(temp));
            }
            return inputVector;
        }
        public List<bool> intToListBoolPadded (int input)
        {
            String vectorS = Convert.ToString(input, 2);
            List<bool> inputVector = new List<bool>();
            for (int vectorIndex = 0; vectorIndex < vectorS.Length; vectorIndex++)
            {
                int temp = vectorS[vectorS.Length - 1 - vectorIndex] - '0';
                inputVector.Add(Convert.ToBoolean(temp));
            }
            List<bool> padder = getDigitalSourcesValue();
            for (int vectorIndex = inputVector.Count; vectorIndex < padder.Count; vectorIndex++)
            {
                inputVector.Add(false);
            }
            return inputVector;
        }
        public List<bool> lsbToMsb(List<bool> input)
        {
            List<bool> output = new List<bool>();
            for (int inputIndex = input.Count-1; inputIndex >= 0 ; inputIndex--)
            {
                output.Add(input[inputIndex]);
            }
            return output;
        }
        public List<bool> incrementBoolList(List<bool> input)
        {
            List<bool> output = new List<bool>();
            int decInput = getInputVectorDecimal(input);
            output = getIncrementedInputVector(decInput); //smaller than input
            for (int inputIndex = output.Count; inputIndex < input.Count; inputIndex++)
            {
                output.Add(input[inputIndex]);
            }
            return output;
        }
        public void driveSources(List<bool> inputVector)
        {
            List<int> sourceIndexes = getDigitalSourcesIndex();
            for (int sources = 0; sources < sourceIndexes.Count; sources++)
            {
                Netlist[sourceIndexes[sources]].driveInputPort(Netlist[sourceIndexes[sources]].Ports[0].Key,inputVector[sources]);
            }
        }
        /// <summary>
        /// Registering devices is key for CLI use of this project.
        /// Similar to Linux device driver mount, the LogicSystem implements mechanisms for automatic adding and
        /// substracting LogicDevices from the netlist. Even though Port's nodes names are Strings, the auto
        /// methods (getAvailableId, getAvailablePorts,registerDevice,unregisterDevice) work only with ints for
        /// sorting purposes.
        /// </summary>
        /// <returns></returns>
        public int getAvailableId()
        {
            int max = 0;
            for (int idIndex = 0; idIndex < Netlist.Count; idIndex++)
            {
                if (Netlist[idIndex].Id >= max)
                {
                    max = Netlist[idIndex].Id;
                }
            }
            return max + 1;
        }
        public String[] getAvailablePorts(int count)
        {
            String[] availablePorts = new String[count];
            int max = 0;
            for (int deviceIndex = 0; deviceIndex < Netlist.Count; deviceIndex++)
            {
                for (int portIndex = 0; portIndex < Netlist[deviceIndex].Ports.Count; portIndex++)
                {
                    if (Regex.IsMatch(Netlist[deviceIndex].Ports[portIndex].Key, @"^\d+$"))
                    {
                        if( Convert.ToInt32(Netlist[deviceIndex].Ports[portIndex].Key) >= max)
                        {
                            max = Convert.ToInt32(Netlist[deviceIndex].Ports[portIndex].Key);
                        }
                    }
                }
            }
            for (int availableIndex = 0; availableIndex < count; availableIndex++)
            {
                availablePorts[availableIndex] = Convert.ToString(max + availableIndex + 1);
            }
            return availablePorts;
        }
        public void registerDevice(String type)
        {
            LogicDevice dev = new LogicDevice();
            switch (type.ToLower())
            {
                case "nand2":
                    dev = new NANDGate(getAvailablePorts(3), getAvailableId());
                    break;
                case "and2":
                    dev = new ANDGate(getAvailablePorts(3), getAvailableId());
                    break;
                case "nor2":
                    dev = new NORGate(getAvailablePorts(3), getAvailableId());
                    break;
                case "or2":
                    dev = new ORGate(getAvailablePorts(3), getAvailableId());
                    break;
                case "not":
                    dev = new NOTGate(getAvailablePorts(2), getAvailableId());
                    break;
                case "v":
                    dev = new DigitalSource(getAvailablePorts(1), getAvailableId());
                    break;
                case "s":
                    dev = new DigitalSink(getAvailablePorts(1), getAvailableId());
                    break;
                case "high":
                    dev = new DigitalHigh(getAvailablePorts(1), getAvailableId());
                    break;
                case "low":
                    dev = new DigitalLow(getAvailablePorts(1), getAvailableId());
                    break;
                default:
                    break;
            }
            Netlist.Add(dev);
        }
        public void unregisterDevice(LogicDevice dev)
        {
            Netlist.Remove(dev);
        }
        /// <summary>
        /// Connect methods provide mechanisms to alter the netlist, so that two nodes are connected.
        /// As mentioned before, a connection is a pair of Ports with the same name.
        /// </summary>
        /// <param name="connector"></param>
        /// <param name="connectorPort"></param>
        /// <param name="connectee"></param>
        public void connectPorts(LogicDevice connector, String connectorPort, LogicDevice connectee)
        {
            KeyValuePair<String, bool> tPort = new KeyValuePair<String, bool>(connectorPort, false);
            Netlist[Netlist.IndexOf(connector)].Ports[Netlist[Netlist.IndexOf(connector)].Ports.IndexOf(tPort)] = tPort;
        }
        public void connectNodes(String connectorNode, int connectorDeviceId, String connecteeNode, int connecteeDeviceId)
        {
            KeyValuePair<String, bool> tPort = new KeyValuePair<String, bool>(connectorNode, false);
            int connectorIndex = -1;
            int connecteeIndex = -1;
            for (int deviceIndex = 0; deviceIndex < Netlist.Count; deviceIndex++)
            {
                if(Netlist[deviceIndex].Id == connectorDeviceId)
                {
                    connectorIndex = deviceIndex;
                }
                if (Netlist[deviceIndex].Id == connecteeDeviceId)
                {
                    connecteeIndex = deviceIndex;
                }
            }
            int connectorPortIndex = -1;
            int connecteePortIndex = -1;
            for (int portIndex = 0; portIndex < Netlist[connectorIndex].Ports.Count; portIndex++)
            {
                if (Netlist[connectorIndex].Ports[portIndex].Key == connectorNode)
                {
                    connectorPortIndex = portIndex;
                }
            }
            for (int portIndex = 0; portIndex < Netlist[connecteeIndex].Ports.Count; portIndex++)
            {
                if (Netlist[connecteeIndex].Ports[portIndex].Key == connecteeNode)
                {
                    connecteePortIndex = portIndex;
                }
            }
            Netlist[connecteeIndex].Ports[connecteePortIndex] = tPort;
        }
        /// <summary>
        /// Method : resetSystem()
        /// Drive all devices to false (low). Mind that constants are also reset! This is not a problem, because of
        /// hitAllTransferFunctions() method implemented below.
        /// </summary>
        public void resetSystem()
        {
            for (int driveeIndex = 0; driveeIndex < Netlist.Count; driveeIndex++)
            {
                for (int portIndex = 0; portIndex < Netlist[driveeIndex].Ports.Count; portIndex++)
                {
                    Netlist[driveeIndex].Ports[portIndex] = new KeyValuePair<string, bool>(Netlist[driveeIndex].Ports[portIndex].Key, false);
                }
            }
        }
        public int getNetlistIndexByDeviceId(int devId)
        {
            int result = -1;
            for (int netlistIndex = 0; netlistIndex < Netlist.Count; netlistIndex++)
            {
                if(Netlist[netlistIndex].Id == devId)
                {
                    result = netlistIndex;
                }
            }
            return result;
        }
        public void drcNetlist()
        {
            for (int deviceIndex = 0; deviceIndex < Netlist.Count; deviceIndex++)
            {
               for (int portIndex = 0; portIndex < Netlist[deviceIndex].Ports.Count; portIndex++)
                {

                }
            }
        }
        /// <summary>
        /// dump* methods are used for printing information about Netlist or LogicDevices
        /// </summary>
        public void dumpAllInfos()
        {
            for (int i = 0; i < Netlist.Count; i++)
            {
                Netlist[i].dumpInfo();
            }
        }
        public String [] dumpAllInfosText()
        {
            String[] dumpText = new String[Netlist.Count];
            for (int i = 0; i < Netlist.Count; i++)
            {
                dumpText[i]=Netlist[i].dumpAllInfoText();
            }
            return dumpText;
        }
        public String[] dumpNetlist()
        {
            String[] netlistText = new String[Netlist.Count];
            for (int i = 0; i < Netlist.Count; i++)
            {
                netlistText[i]=Netlist[i].dumpInfoText()+"\n";
            }
            return netlistText;
        }
        public String dumpInputVector()
        {
            List<bool> vector = getDigitalSourcesValue();
            List<String> vectorS = new List<String>();
            for (int vectorIndex = 0; vectorIndex < vector.Count; vectorIndex++)
            {
                vectorS.Add(Convert.ToString(Convert.ToInt32(vector[vectorIndex])));
            }
            String resultS = "";
            for (int vectorIndex = 0; vectorIndex < vectorS.Count; vectorIndex++)
            {
                resultS += vectorS[vectorIndex];
            }
            return resultS;
        }
        public String dumpOutputVector()
        {
            List<bool> vector = getDigitalSinksValue();
            List<String> vectorS = new List<String>();
            for (int vectorIndex = 0; vectorIndex < vector.Count; vectorIndex++)
            {
                vectorS.Add(Convert.ToString(Convert.ToInt32(vector[vectorIndex])));
            }
            String resultS = "";
            for (int vectorIndex = 0; vectorIndex < vectorS.Count; vectorIndex++)
            {
                resultS += vectorS[vectorIndex];
            }
            return resultS;
        }
        public bool isThePortInTheDev(int devId, String portName)
        {
            bool result = false ;
            int index = getNetlistIndexByDeviceId(devId);
                for (int portIndex = 0; portIndex < Netlist[getNetlistIndexByDeviceId(devId)].Ports.Count; portIndex++)
                {
                    if (Netlist[getNetlistIndexByDeviceId(devId)].Ports[portIndex].Key == portName)
                    {
                        result = true;
                    }
                }
            return false;
        }
        public void hitAllTransferFunctions(int count)
        {
            for (int j = 0; j < count; j++)
            {
                for (int i = 0; i < Netlist.Count; i++)
                {
                    Netlist[i].transferFunction();
                }
            }

        }
        public int getStagesCount()
        {
            List<int> branch = new List<int>();
            for (int netlistIndex = 0; netlistIndex < Netlist.Count; netlistIndex++)
            {
                for (int portIndex = 0; portIndex < Netlist[netlistIndex].Ports.Count; portIndex++)
                {
                    int branchEntry = 0;
                    String matchValue = Netlist[netlistIndex].Ports[portIndex].Key;
                    for (int netlistIndexMatch = 0; netlistIndexMatch < Netlist.Count; netlistIndexMatch++)
                    {
                        if (isThePortInTheDev(Netlist[netlistIndexMatch].Id, matchValue))
                        {
                            branchEntry++;
                        }
                    }
                }
            }
            int result = -1;
            for (int listIndex = 0; listIndex < branch.Count; listIndex++)
            {
                if(branch[listIndex] >= result)
                {
                    result = branch[listIndex];
                }
            }
            return result;
        }
        /// <summary>
        /// This is the most important method of them all. The basic assumption is that
        /// N-stage logic purely combinational no-delay circuit will reach its final state
        /// after N-'ticks' since applying stimulus to its inputs, where a tick is a twofold operation of:
        /// updating outputs via transferFunctions, driving inputs from outputs of previous stages.
        /// 
        /// Having failed to implement it based on this remark, lets multiply the cycles by 10.
        /// </summary>
        public void gotoSteadyState()
        {
            int cyclesUntilStable = getStagesCount()+10;
            for (int cycleIndex = 0; cycleIndex < cyclesUntilStable; cycleIndex++)
            {
                hitAllTransferFunctions(1);
                driveDevices();
            }
        }
        public void driveDevices()
        {
            for (int driverIndex = 0; driverIndex < Netlist.Count; driverIndex++)
            {
                if (Netlist[driverIndex].GetType().Name != "DigitalSink")
                {
                    KeyValuePair<String, bool> output = new KeyValuePair<String, bool>(Netlist[driverIndex].Ports.Last().Key, Netlist[driverIndex].Ports.Last().Value);
                    for (int driveeIndex = 0; driveeIndex < Netlist.Count; driveeIndex++)
                    {
                        for (int portIndex = 0; portIndex < Netlist[driveeIndex].Ports.Count; portIndex++)
                        {
                            if (Netlist[driveeIndex].Ports[portIndex].Key == output.Key)
                            {
                                Netlist[driveeIndex].Ports[portIndex] = new KeyValuePair<string, bool>(Netlist[driveeIndex].Ports[portIndex].Key, output.Value);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Use every possible combination of registered voltage sources to generate the truth table of the LogicSystem.
        /// Example. NAND Gate:
        /// Dictionary<int,String> 
        /// (0,"0"),
        /// (1,"0"),
        /// (2,"0"),
        /// (3,"1")
        /// </summary>
        public Dictionary<int,String> runTest()
        {
            resetSystem();
            Dictionary<int, String> testResult = new Dictionary<int, string>();
            for (int stateIndex = 0; stateIndex < Math.Pow(2,getDigitalSourcesIndex().Count ); stateIndex++)
            {
                List<bool> stimulus = lsbToMsb(intToListBoolPadded(stateIndex));
                List<bool> t = new List<bool> {true} ;
                driveSources(stimulus);
                gotoSteadyState();
                testResult.Add(stateIndex, dumpOutputVector());
            }
            return testResult;
        }

    }
}