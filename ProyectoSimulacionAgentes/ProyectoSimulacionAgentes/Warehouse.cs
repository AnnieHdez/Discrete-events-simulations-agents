using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSimulacionAgentes
{
    class Warehouse
    {
        private int storeResources; //number of resources store at the wearhouse 
        private int owner; //Team to wich the wearhouse belongs to
        private int xpos;
        private int ypos;

        public Warehouse(int _owner, int _xpos, int _ypos)
        {
            storeResources = 0;
            owner = _owner;
            xpos = _xpos;
            ypos = _ypos;
        }

        public int StoreResources
        {
            get
            {
                return storeResources;
            }

            set
            {
                storeResources = value;
            }
        }

        public int Owner
        {
            get
            {
                return owner;
            }
        }

        public int Xpos
        {
            get
            {
                return xpos;
            }
        }

        public int Ypos
        {
            get
            {
                return ypos;
            }
        }

    }
}
