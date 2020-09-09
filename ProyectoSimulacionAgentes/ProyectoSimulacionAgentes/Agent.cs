using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSimulacionAgentes
{
    abstract class Agent
    {
        internal int xpos;
        internal int ypos;
        internal int team;
        internal bool empty;
        internal bool foundWH; //state of the agent to know if already found it wearhouse
        internal static Map map;
        internal Random pos = new Random();
        internal static Warehouse myWarehouse;
        public Map.Elements rememberMark; //remember what was at the position when the agent move to place the mark again

        public Agent(int _team, int _xpos, int _ypos, Map _map, Warehouse _myWarehouse)
        {
            team = _team;
            xpos = _xpos;
            ypos = _ypos;
            empty = true;
            foundWH = false;
            map = _map;
            myWarehouse = _myWarehouse;
            rememberMark = Map.Elements.Nothing; // The position where the agent is was empty
        }

        virtual public void DoSomething()
        { }

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

        //Return all availables positions (the 4 where the agent can move)  where is the wanted element
        internal static bool SearchPosMove(int x, int y, Map.Elements element, out List<int> xs, out List<int> ys)
        {
            bool found = false;
            xs = new List<int>();
            ys = new List<int>();

            if (x + 1 < map.currentMap.GetLength(0) && map.currentMap[x + 1, y] == element)
            {
                //If looking for a wearhouse check that is the one of the agent team 
                if (element == Map.Elements.Warehouse)
                {
                    if (myWarehouse.Xpos == x + 1 && myWarehouse.Ypos == y)
                    {
                        found = true;
                        xs.Add(x + 1);
                        ys.Add(y);
                    }
                }

                else
                {
                    found = true;
                    xs.Add(x + 1);
                    ys.Add(y);
                }
            }

            if (x - 1 >= 0 && map.currentMap[x - 1, y] == element)
            {
                //If looking for a wearhouse check that is the one of the agent team 
                if (element == Map.Elements.Warehouse)
                {
                    if (myWarehouse.Xpos == x - 1 && myWarehouse.Ypos == y)
                    {
                        found = true;
                        xs.Add(x - 1);
                        ys.Add(y);
                    }
                }

                else
                {
                    found = true;
                    xs.Add(x - 1);
                    ys.Add(y);
                }
            }

            if (y + 1 < map.currentMap.GetLength(0) && map.currentMap[x, y + 1] == element)
            {
                //If looking for a wearhouse check that is the one of the agent team 
                if (element == Map.Elements.Warehouse)
                {
                    if (myWarehouse.Xpos == x && myWarehouse.Ypos == y + 1)
                    {
                        found = true;
                        xs.Add(x);
                        ys.Add(y + 1);
                    }
                }

                else
                {
                    found = true;
                    xs.Add(x);
                    ys.Add(y + 1);
                }
            }

            if (y - 1 >= 0 && map.currentMap[x, y - 1] == element)
            {
                //If looking for a wearhouse check that is the one of the agent team 
                if (element == Map.Elements.Warehouse)
                {
                    if (myWarehouse.Xpos == x && myWarehouse.Ypos == y - 1)
                    {
                        found = true;
                        xs.Add(x);
                        ys.Add(y - 1);
                    }
                }

                else
                {
                    found = true;
                    xs.Add(x);
                    ys.Add(y - 1);
                }
            }

            return found;
        }

        //Return all availables positions (the 8 from where the agent can look)  where is the wanted element
        internal static bool SearchPos(int x, int y, Map.Elements element, out List<int> xs, out List<int> ys)
        {
            bool found = SearchPosMove(x, y, element, out xs, out ys);

            if (x + 1 < map.currentMap.GetLength(0) && y + 1 < map.currentMap.GetLength(0) && map.currentMap[x + 1, y + 1] == element)
            {
                //If looking for a wearhouse check that is the one of the agent team 
                if (element == Map.Elements.Warehouse)
                {
                    if (myWarehouse.Xpos == x + 1 && myWarehouse.Ypos == y + 1)
                    {
                        found = true;
                        xs.Add(x + 1);
                        ys.Add(y + 1);
                    }
                }

                else
                {
                    found = true;
                    xs.Add(x + 1);
                    ys.Add(y + 1);
                }
            }

            if (x - 1 >= 0 && y - 1 >= 0 && map.currentMap[x - 1, y - 1] == element)
            {
                //If looking for a wearhouse check that is the one of the agent team 
                if (element == Map.Elements.Warehouse)
                {
                    if (myWarehouse.Xpos == x - 1 && myWarehouse.Ypos == y - 1)
                    {
                        found = true;
                        xs.Add(x - 1);
                        ys.Add(y - 1);
                    }
                }

                else
                {
                    found = true;
                    xs.Add(x - 1);
                    ys.Add(y - 1);
                }
            }

            if (x - 1 >= 0 && y + 1 < map.currentMap.GetLength(0) && map.currentMap[x - 1, y + 1] == element)
            {
                //If looking for a wearhouse check that is the one of the agent team 
                if (element == Map.Elements.Warehouse)
                {
                    if (myWarehouse.Xpos == x - 1 && myWarehouse.Ypos == y + 1)
                    {
                        found = true;
                        xs.Add(x - 1);
                        ys.Add(y + 1);
                    }
                }

                else
                {
                    found = true;
                    xs.Add(x - 1);
                    ys.Add(y + 1);
                }
            }

            if (x + 1 < map.currentMap.GetLength(0) && y - 1 >= 0 && map.currentMap[x + 1, y - 1] == element)
            {
                //If looking for a wearhouse check that is the one of the agent team 
                if (element == Map.Elements.Warehouse)
                {
                    if (myWarehouse.Xpos == x + 1 && myWarehouse.Ypos == y - 1)
                    {
                        found = true;
                        xs.Add(x + 1);
                        ys.Add(y - 1);
                    }
                }

                else
                {
                    found = true;
                    xs.Add(x + 1);
                    ys.Add(y - 1);
                }
            }

            return found;
        }
    }
}
