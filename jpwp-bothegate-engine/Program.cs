using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jpwp_bothegate_engine
{
    class Program
    {
        static void Main(string[] args)
        {
            Utils.infoPrint("ENTRY POINT OF APP: ");
            Utils.infoPrint("JPWP BOTHEGATE ENGINE v 0.1. ");
            //List tests
            if ( Convert.ToBoolean(0) )
            {
                /*
                SortedList<int, bool> mylist = new SortedList<int, bool>();
                mylist.Add(10, false);
                mylist.Add(20, true);
                mylist.Add(30, false);
                mylist.Add(40, true);
                mylist.Add(50, false);
                for (int i = 0; i < 5; i++)
                {
                    System.Console.WriteLine("mylist.Keys[" + i + "]: " + mylist.Keys[i]);
                    System.Console.WriteLine("mylist[20] " + mylist[20]);
                }
                mylist[20] = false;
                for (int i = 0; i < 5; i++)
                {
                    System.Console.WriteLine("mylist.Keys[" + i + "]: " + mylist.Keys[i]);
                    System.Console.WriteLine("mylist[20] " + mylist[20]);
                }*/
                List<KeyValuePair<String, bool>> list2 = new List<KeyValuePair<string, bool>>();
                KeyValuePair<String, bool> kvp = new KeyValuePair<string, bool>("hey",false);
                list2.Add(kvp);
                list2.Add(kvp);
                List<String> strings = new List<string>();
                List<bool> bools = new List<bool>(
                    (from tkvp in list2 where tkvp.Key == "hey" select tkvp.Value).ToList()
                    );
                for (int i = 0; i < bools.Count; i++)
                {
                    System.Console.WriteLine("List entry at index" + i+ ":"+ bools[i]);
                    //System.Console.WriteLine("Key at index" + i + ":" + list2[i].Key);
                    //System.Console.WriteLine("Value at index" + i + ":" + list2[i].Value);
                    System.Console.WriteLine("Value at key" + i + ":" + bools[i]
                        
                        );
                }
                Utils.debugPrint("last:"+bools.Last().GetType());
            }
            if ( Convert.ToBoolean(0) )
            {
                nandTest();
            }
            if (Convert.ToBoolean(0))
            {
                Utils.infoPrint("LogicSystem class debug starts.");
                LogicSystem sys = new LogicSystem();
                sys.loadNetlist("netlist_0x00.txt"); //temp hardcode
                sys.dumpAllInfos();
                sys.Netlist[0].driveInputPort("0", true);
                sys.Netlist[0].driveInputPort("1", true);
                for (int i = 0; i < 4; i++)
                {
                    sys.hitAllTransferFunctions(1);
                    sys.driveDevices();
                }
                sys.dumpAllInfos();
                Utils.debugPrint("Available id: " + sys.getAvailableId());
                String[] availablePorts = sys.getAvailablePorts(5);
                for (int i = 0; i < availablePorts.Length; i++)
                {
                    Utils.debugPrint("Available ports: " + availablePorts[i]);
                }
                
            }
            if (Convert.ToBoolean(0))
            {
                Utils.drcErrWrnMsg("nand 8", 0);
                Utils.drcErrWrnMsg("nand 8", 1);
                Utils.drcErrWrnMsg("nand 8", 2);
                Utils.drcErrWrnMsg("nand 8", 10);
                Utils.drcErrWrnMsg("nand 8", 532);
                Utils.drcErrWrnMsg("nand 8", 1024);
            }
            if (Convert.ToBoolean(1))
            {
                Utils.infoPrint("LogicSystem class debug starts.");
                LogicSystem sys = new LogicSystem();
                sys.registerDevice("digitalsource");
                sys.Netlist[0].toggleState();

                sys.registerDevice("nand2");
                sys.registerDevice("nand2");
                sys.dumpAllInfos();
                
                //sys.registerDevice("nand2");
                //sys.registerDevice("nand2");

                //sys.Netlist[0].driveInputPort("1", true);
                //sys.Netlist[0].driveInputPort("2", true);
                //sys.dumpAllInfos();
                //sys.resetSystem();
                //sys.dumpAllInfos();

                sys.dumpAllInfos();
            }
        }


        static void nandTest()
        {
            //Create a NAND Gate
            //Testcases
            bool[] stimulus = new bool[]
            {
            false, false,
            false, true,
            true,  false,
            true,  true
            };
            String[] names = new String[] { "inputA", "inputB", "outputC"};
            Utils.debugPrint("names.length: " + names.Length);
            NANDGate gate = new NANDGate(names);
            Utils.infoPrint("NAND gate test.");
            Utils.infoPrint("-------------------------------------");
            Utils.clearReport();
            for (int i = 0; i < 4; i++)
            {
                gate.driveInputPort(names[0], stimulus[2 * i]);
                gate.driveInputPort(names[1], stimulus[2 * i + 1]);
                //Utils.infoPrint("Initial state:");
                //gate.dumpInfo();
                gate.transferFunction();
                Utils.infoPrint("After trigger call.");
                gate.dumpInfo();
                Utils.fdebugPrint(  Convert.ToInt32(gate.Ports[0].Value) + " " +
                                    Convert.ToInt32(gate.Ports[1].Value) + " " +
                                    Convert.ToInt32(gate.Ports[2].Value) );
            }
         }
    }
}
