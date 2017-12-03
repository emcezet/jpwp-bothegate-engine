using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace jpwp_bothegate_engine
{
    public class LogicDevice
    {
        private int inputPorts;
        private int outputPorts;

        public int getInputPorts
        {
            get => default(int);
            set
            {
            }
        }

        public int setInputPorts
        {
            get => default(int);
            set
            {
            }
        }

        public int getOutputPorts
        {
            get => default(int);
            set
            {
            }
        }

        public int setOutputPorts
        {
            get => default(int);
            set
            {
            }
        }

        public void triggerTransferFunction()
        {
            throw new System.NotImplementedException();
        }
    }

    public class NANDGate : LogicDevice
    {
        public void transferFunction()
        {
            throw new System.NotImplementedException();
        }
    }

    public class ORGate : LogicDevice
    {
        public void transferFunction()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Wire : LogicDevice
    {
        public void transferFunction()
        {
            throw new System.NotImplementedException();
        }
    }
}