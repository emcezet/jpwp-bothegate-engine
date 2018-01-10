using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jpwp_bothegate_engine
{
    /// <summary>
    /// Class JsonNetlist.
    /// Wrapper for Netlist to take type into consideration. XML could've been a better binding scheme (Future update).
    /// </summary>
    public class JsonNetlist
    {
        private List<String> type;
        private List<LogicDevice> logicDevices;
        public List<LogicDevice> LogicDevices { get; set; }
        public List<String> Type { get; set; }
        public JsonNetlist(List<String> ttype, List<LogicDevice> tlogicDevices)
        {
            Type = ttype;
            LogicDevices = tlogicDevices;
        }  
    }
}
