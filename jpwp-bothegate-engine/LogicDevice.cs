using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace jpwp_bothegate_engine
{
    /// <summary>
    /// Class LogicDevice is the base class for generating logic devices, e.g. gates, voltage sources, voltage sinks and constants.
    /// <list type="bullet">
    /// <listheader>Implemented devices.</listheader>
    ///     <item>
    ///     <description>2-input NAND gate.</description>
    ///     </item>
    ///     <item>
    ///     <description>2-input NOR gate.</description>
    ///     </item>
    ///     <item>
    ///     <description>2-input AND gate.</description>
    ///     </item>
    ///     <item>
    ///     <description>2-input NOR gate.</description>
    ///     </item>
    ///     <item>
    ///     <description>1-input NOT gate.</description>
    ///     </item>
    ///     <item>
    ///     <description>1-output voltage source.</description>
    ///     </item>
    ///     <item>
    ///     <description>1-input voltage sink.</description>
    ///     </item>
    /// </list>
    /// </summary>
    public class LogicDevice
    {
        /// <summary>
        /// Field : List<KeyValuePair<String, bool>> ports
        /// </summary>
        private List<KeyValuePair<String, bool>> ports;
        /// <summary>
        /// Field : int id
        /// </summary>
        private int id;

        public List<KeyValuePair<String, bool>> Ports { get; set; }
        public int Id { get; set; } = 0;
        /// <summary>
        /// Method : transferFunction
        /// </summary>
        public LogicDevice()
        {
            Id = 0;
            Ports = new List<KeyValuePair<String, bool>>();
        }
        public LogicDevice(String[] portNames)
        {
            Id = 0;
            List<KeyValuePair<string, bool>> tPorts = new List<KeyValuePair<string, bool>>();
            for (int portNameIndex = 0; portNameIndex < portNames.Length; portNameIndex++)
            {
                Utils.debugPrint("portNames[" + portNameIndex + "] " + portNames[portNameIndex]);
                tPorts.Add(new KeyValuePair<String, bool>(portNames[portNameIndex], false));
            }
            Ports = tPorts;
        }
        public LogicDevice(String[] portNames, int id)
        {
            Id = id;
            List<KeyValuePair<string, bool>> tPorts = new List<KeyValuePair<string, bool>>();
            for (int portNameIndex = 0; portNameIndex < portNames.Length; portNameIndex++)
            {
                Utils.debugPrint("portNames[" + portNameIndex + "] " + portNames[portNameIndex]);
                tPorts.Add(new KeyValuePair<String, bool>(portNames[portNameIndex], false));
            }
            Ports = tPorts;
        }
        /// <summary>
        /// Method : void driveInputPort(String portName , bool stimulus)
        /// Is used to modify Value in KeyValuPair of Port given by portName
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="stimulus"></param>
        public virtual void driveInputPort(String portName , bool stimulus)
        {
            for (int i = 0; i < Ports.Count; i++)
            {
                if (Ports[i].Key == portName)
                {
                    Ports[i] = new KeyValuePair<String, bool>(Ports[i].Key, stimulus);
                }
            }
        }
        /// <summary>
        /// Method : void transferFunction()
        /// This method is overloaded for every type of LogicDevice. 
        /// When triggered it assigns proper bool value to the output, based on the LogicDevice's inputs.
        /// </summary>
        public virtual void transferFunction()
        {
        }
        /// <summary>
        /// Method : void dumpInfo()
        /// Command Line Interface debug helper. Prints current status of LogicDevice.
        /// </summary>
        public virtual void dumpInfo()
        {
            Utils.infoPrint((Ports.Count-1)+"-input "+this.GetType().Name + "" + this.Id + " device debug dump");
            Utils.debugPrint("Port \t        |        \tValue");
            Utils.debugPrint("-------------------------------------");
            foreach (KeyValuePair<String, bool> kvp in Ports)
            {
                Utils.debugPrint(kvp.Key + "\t        |        \t" + kvp.Value);
            }
            Utils.debugPrint("-------------------------------------");
        }
        /// <summary>
        /// Method : String dumpInfo()
        /// Graphical User Interface debug helper. Returns current status of LogicDevice.
        /// </summary>
        public String dumpAllInfoText()
        {
            String dumpText = "";
            dumpText+=(Ports.Count - 1) + "-input " + this.GetType().Name + "" + this.Id + " device debug dump \n";
            dumpText+= "Port \t        |        \tValue \n";
            dumpText+= "------------------------------------- \n";
            foreach (KeyValuePair<String, bool> kvp in Ports)
            {
                dumpText+=kvp.Key + "\t        |        \t" + kvp.Value + "\n";
            }
            dumpText+="------------------------------------- \n";
            return dumpText;
        }
        /// <summary>
        /// Method : String dumpInfoText()
        /// Graphical User Interface debug helper. Returns current status of LogicDevice.
        /// </summary>
        public String dumpInfoText()
        {
            String infoText = this.GetType().Name + "" + this.Id;
            foreach (KeyValuePair<String, bool> kvp in Ports)
            {
                infoText += " "+kvp.Key;
            }
            return infoText;
        }
        public virtual void toggleState() { }
        /// <summary>
        /// Method : String getOutput()
        /// Returns Ports' name of output node.
        /// </summary>
        public virtual String getOutput()
        {
            int max = 0;
            for (int portIndex = 0; portIndex < Ports.Count; portIndex++)
            {
                if (Regex.IsMatch(Ports[portIndex].Key, @"^\d+$"))
                {
                    if (Convert.ToInt32(Ports[portIndex].Key) >= max)
                    {
                        max = Convert.ToInt32(Ports[portIndex].Key);
                    }
                }
            }
            return Convert.ToString(max);
        }
        /// <summary>
        /// Method : String getRandomInput
        /// Pick a random input (not last) port
        /// </summary>
        /// <returns></returns>
        public virtual String getRandomInput()
        {
            Random rnd = new Random();
            return Ports[rnd.Next(0, Ports.Count - 2)].Key;
        }
    }
    /// <summary>
    /// Class DigitalSource : LogicDevice
    /// DigitalSource is a 1-port 2-state device. It has a unique toggleState() method,
    /// that allows switching its value.
    /// </summary>
    public class DigitalSource : LogicDevice
    {
        public DigitalSource() :base () { }
        public DigitalSource(String[] portNames) : base(portNames) { }
        public DigitalSource(String[] portNames, int id) : base(portNames, id){}
        public override void toggleState()
        {
            Ports[0] = new KeyValuePair<String, bool>(Ports[0].Key, !Ports[0].Value);
        }
        public override void driveInputPort(String portName, bool stimulus)
        {
            base.driveInputPort(portName, stimulus);
        }
}
    /// <summary>
    /// Class DigitalSink : LogicDevice
    /// DigitalSink is a 1-port device that can only be used as end node of the netlist.
    /// It is required for constraining user input to a fixed number of outputs.
    /// </summary>
    public class DigitalSink : LogicDevice
    {
        public DigitalSink() : base() { }
        public DigitalSink(String[] portNames) : base(portNames) { }
        public DigitalSink(String[] portNames, int id) : base(portNames, id) { }
        public override void transferFunction(){}
    }
    /// <summary>
    /// Class NANDGate : LogicDevice
    /// All gates implemented can be 2 or more input gates. Output is always the last node.
    /// </summary>
    public class NANDGate : LogicDevice
    {
        public NANDGate() : base() { }
        public NANDGate(String[] portNames) : base(portNames) {}
        public NANDGate(String[] portNames, int id) : base(portNames, id){}
        public override void transferFunction()
        {
            bool result = Ports[0].Value;
            for (int portIndex = 1; portIndex < Ports.Count - 1; portIndex++)
            {
                result &= Ports[portIndex].Value;
            }
            result = !result;
            Ports[Ports.Count - 1] = new KeyValuePair<String, bool>(Ports[Ports.Count - 1].Key, result);
        }
    }
    public class ANDGate : LogicDevice
    {
        public ANDGate() : base() { }
        public ANDGate(String[] portNames) : base(portNames) { }
        public ANDGate(String[] portNames, int id) : base(portNames, id) { }
        public override void transferFunction()
        {
            bool result = Ports[0].Value;
            for (int portIndex = 1; portIndex < Ports.Count - 1; portIndex++)
            {
                result &= Ports[portIndex].Value;
            }
            Ports[Ports.Count - 1] = new KeyValuePair<String, bool>(Ports[Ports.Count - 1].Key, result);
        }
    }
    public class ORGate : LogicDevice
    {
        public ORGate() : base() { }
        public ORGate(String[] portNames) : base(portNames) { }
        public ORGate(String[] portNames, int id) : base(portNames, id){ }
        public override void transferFunction()
        {
            bool result = Ports[0].Value;
            for (int portIndex = 1; portIndex < Ports.Count - 1; portIndex++)
            {
                result |= Ports[portIndex].Value;
            }
            Ports[Ports.Count - 1] = new KeyValuePair<String, bool>(Ports[Ports.Count - 1].Key, result);
        }
    }
    public class NORGate : LogicDevice
    {
        public NORGate() : base() { }
        public NORGate(String[] portNames) : base(portNames) { }
        public NORGate(String[] portNames, int id) : base(portNames, id) { }
        public override void transferFunction()
        {
            bool result = Ports[0].Value;
            for (int portIndex = 1; portIndex < Ports.Count - 1; portIndex++)
            {
                result |= Ports[portIndex].Value;
            }
            result = !result;
            Ports[Ports.Count - 1] = new KeyValuePair<String, bool>(Ports[Ports.Count - 1].Key, result);
        }
    }
    public class NOTGate : LogicDevice
    {
        public NOTGate() : base() { }
        public NOTGate(String[] portNames) : base(portNames) { }
        public NOTGate(String[] portNames, int id) : base(portNames, id) { }
        public override void transferFunction()
        {
            Ports[1] = new KeyValuePair<String, bool>(Ports[1].Key, !Ports[1].Value);
        }
    }
    /// <summary>
    /// Class DigitalHigh : LogicDevice
    /// This device always transfers its output to true (digital high).
    /// </summary>
    public class DigitalHigh : LogicDevice
    {
        public DigitalHigh() : base() { }
        public DigitalHigh(String[] portNames) : base(portNames) { }
        public DigitalHigh(String[] portNames, int id) : base(portNames, id) { }
        public override void transferFunction()
        {
            Ports[0]= new KeyValuePair<String, bool>(Ports[0].Key, true);
        }
    }
    /// <summary>
    /// Class DigitalLow : LogicDevice
    /// This device always transfers its output to false (digital low).
    /// </summary>
    public class DigitalLow : LogicDevice
    {
        public DigitalLow() : base() { }
        public DigitalLow(String[] portNames) : base(portNames) { }
        public DigitalLow(String[] portNames, int id) : base(portNames, id) { }
        public override void transferFunction()
        {
            Ports[0] = new KeyValuePair<String, bool>(Ports[0].Key, true);
        }
    }
}
