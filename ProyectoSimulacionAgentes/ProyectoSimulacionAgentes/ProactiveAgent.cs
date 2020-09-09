using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSimulacionAgentes
{
    class ProactiveAgent : Agent
    {
        int turn; //how many turns pass
        List<int> pathX;
        List<int> pathY;
        int step; //in wich step of the path to find the elemnt is
        bool justEmpty;
        bool justFull;

        public ProactiveAgent(int _team, int _xpos, int _ypos, Map _map, Warehouse _myWarehouse)
            : base(_team, _xpos, _ypos, _map, _myWarehouse)
        {
            turn = 0;
            pathX = new List<int>();
            pathY = new List<int>();
            step = 0;
            justEmpty = true; ;
            justFull = false;
        }

        override public void DoSomething()
        {
            turn += 1;
            List<int> xs;
            List<int> ys;

            if (!this.empty)
            {
                //If my wearhouse is in an accesible position
                if (SearchPos(this.xpos, this.ypos, Map.Elements.Warehouse, out xs, out ys))
                {
                    this.empty = true;
                    this.justEmpty = true;
                    this.justFull = false;
                    myWarehouse.StoreResources += 1;
                }

                else
                {
                    //If just find and element in the last turn or every 3 turns look for the shortest path to the wearhouse
                    if (justFull || this.turn % 3 == 0 )
                    {
                        pathX.Clear();
                        pathY.Clear();
                        this.justFull = false;
                        FindShortestPath(Map.Elements.Warehouse, out pathX, out pathY);
                        step = 0;
                    }

                    //if following the path to the wearhouse move to the next step
                    if (pathX.Count > 0 && step < pathX.Count && (int)map.currentMap[pathX[step], pathY[step]] <= 2)
                    {
                        Map.Elements temp = (Map.Elements)map.currentMap[pathX[step], pathY[step]];
                        map.MoveAgent(this, pathX[step], pathY[step]);

                        rememberMark = temp;
                        this.xpos = pathX[step];
                        this.ypos = pathY[step];
                        step += 1;
                    }
                    
                    //Move to an empty available position
                    else if (SearchPosMove(this.xpos, this.ypos, Map.Elements.Mark2, out xs, out ys) || SearchPosMove(this.xpos, this.ypos, Map.Elements.Mark1, out xs, out ys) ||
                            SearchPosMove(this.xpos, this.ypos, Map.Elements.Nothing, out xs, out ys))
                    {
                        int selectPos = pos.Next(xs.Count);
                        Map.Elements temp = (Map.Elements)map.currentMap[xs[selectPos], ys[selectPos]];
                        map.MoveAgent(this, xs[selectPos], ys[selectPos]);

                        rememberMark = temp;
                        this.xpos = xs[selectPos];
                        this.ypos = ys[selectPos];
                    }

                }
            }

            //if is empty
            else
            {
                //If an element is in an accesible position
                if (SearchPos(this.xpos, this.ypos, Map.Elements.Resource, out xs, out ys))
                {
                    int selectPos = pos.Next(xs.Count);
                    map.DeleteResorce(xs[selectPos], ys[selectPos]);
                    this.empty = false;
                    this.justEmpty = false;
                    this.justFull = true;
                }

                else
                {
                    //If just drop and element in the last turn or every 3 turns look for the shortest path to the nearest resource
                    if (justEmpty || this.turn % 3 == 0 )
                    {
                        pathX.Clear();
                        pathY.Clear();
                        justEmpty = false;
                        FindShortestPath(Map.Elements.Resource, out pathX, out pathY);
                        step = 0;
                    }

                    //if following the path to a resorce move to the next step
                    if (pathX.Count > 0 && step < pathX.Count && (int)map.currentMap[pathX[step], pathY[step]] <= 2)
                    {
                        Map.Elements temp = (Map.Elements)map.currentMap[pathX[step], pathY[step]];
                        map.MoveAgent(this, pathX[step], pathY[step]);

                        rememberMark = temp;
                        this.xpos = pathX[step];
                        this.ypos = pathY[step];
                        step += 1;
                    }

                    else if (SearchPosMove(this.xpos, this.ypos, Map.Elements.Mark2, out xs, out ys) || SearchPosMove(this.xpos, this.ypos, Map.Elements.Mark1, out xs, out ys) ||
                            SearchPosMove(this.xpos, this.ypos, Map.Elements.Nothing, out xs, out ys))
                    {       
                        int selectPos = pos.Next(xs.Count);
                        Map.Elements temp = (Map.Elements)map.currentMap[xs[selectPos], ys[selectPos]];
                        map.MoveAgent(this, xs[selectPos], ys[selectPos]);

                        rememberMark = temp;
                        this.xpos = xs[selectPos];
                        this.ypos = ys[selectPos];
                                
                    }                       
        
                }

            }
        }

        //Find the shortest path to the nearest resource or the wearhouse
        private void FindShortestPath(Map.Elements element, out List<int> shortestPathX, out List<int> shortestPathY)
        {
            List<int> shortPathx = new List<int>();
            List<int> shortPathy = new List<int>();

            List<int> currentPathx = new List<int>();
            List<int> currentPathy = new List<int>();

            //A copy of the map to modify
            int[,] newMap = new int[map.currentMap.GetLength(0), map.currentMap.GetLength(0)];

            for (int i = 0; i < map.currentMap.GetLength(0); i++)
            {
                for (int j = 0; j < map.currentMap.GetLength(0); j++)
                {
                    newMap[i, j] = (int)map.currentMap[i, j];
                }

            }

            shortestPathX = new List<int>();
            shortestPathY = new List<int>();

            //Look for the element in de map
            for (int i = 0; i < map.currentMap.GetLength(0); i++)
            {
                for (int j = 0; j < map.currentMap.GetLength(0); j++)
                {
                    if (newMap[i, j] == (int)element)
                    {
                        if (element == Map.Elements.Warehouse)
                        {
                            //found the werahouse
                            if (myWarehouse.Xpos == i && myWarehouse.Ypos == j)
                            {
                                FindShortestPath(newMap, this.xpos, this.ypos, i, j, shortPathx, shortPathy, currentPathx, currentPathy);
                                shortestPathX.AddRange(shortPathx);
                                shortestPathY.AddRange(shortPathy);
                                shortPathx.Clear();
                                shortPathy.Clear();
                                //the firts position in the path is where the agent is 
                                if (shortestPathX.Count() > 0)
                                {
                                    shortestPathX.RemoveAt(0);
                                    shortestPathY.RemoveAt(0);
                                }
                                return;                               
                            }                           
                        }
                        
                        //Looking for a resourse
                        else
                        {
                            FindShortestPath(newMap, this.xpos, this.ypos, i, j, shortPathx, shortPathy, currentPathx, currentPathy);
                            if (shortPathx.Count > 0 && (shortPathx.Count < shortestPathX.Count || shortestPathX.Count == 0))
                            {
                                shortestPathX.AddRange(shortPathx);
                                shortestPathY.AddRange(shortPathy);
                                //the firts position in the path is where the agent is 
                                shortestPathX.RemoveAt(0);
                                shortestPathY.RemoveAt(0);

                            }


                            shortPathx.Clear();
                            shortPathy.Clear();
                        }

                    }

                }

            }
           
        }

        private static void FindShortestPath(int[,] element, int origX, int origY, int destX, int destY,
             List<int> shortPathx, List<int> shortPathy, List<int> currentPathx, List<int> currentPathy)
        {
            if ((origX == destX && origY + 1 == destY) || (origX == destX && origY - 1 == destY) ||
                (origX + 1 == destX && origY == destY) || (origX - 1 == destX && origY == destY) ||
                (origX + 1 == destX && origY - 1 == destY) || (origX - 1 == destX && origY + 1 == destY) ||
                (origX + 1 == destX && origY + 1 == destY) || (origX - 1 == destX && origY - 1 == destY))
            {
                currentPathx.Add(origX);
                currentPathy.Add(origY);

                if (shortPathx.Count == 0 || currentPathx.Count < shortPathx.Count)
                {
                    shortPathx.Clear();
                    shortPathx.AddRange(currentPathx);
                    shortPathy.Clear();
                    shortPathy.AddRange(currentPathy);
                }

                currentPathx.Remove(origX);
                currentPathy.Remove(origY);
            }

            else
            {
                element[origX, origY] = 6;
                currentPathx.Add(origX);
                currentPathy.Add(origY);

                if (origX + 1 < element.GetLength(0) && element[origX + 1, origY] < 3)
                    FindShortestPath(element, origX + 1, origY, destX, destY, shortPathx, shortPathy, currentPathx, currentPathy);

                if (origX - 1 >= 0 && element[origX - 1, origY] < 3)
                    FindShortestPath(element, origX - 1, origY, destX, destY, shortPathx, shortPathy, currentPathx, currentPathy);

                if (origY + 1 < element.GetLength(0) && element[origX, origY + 1] < 3)
                    FindShortestPath(element, origX, origY + 1, destX, destY, shortPathx, shortPathy, currentPathx, currentPathy);

                if (origY - 1 >= 0 && element[origX, origY - 1] < 3)
                    FindShortestPath(element, origX, origY - 1, destX, destY, shortPathx, shortPathy, currentPathx, currentPathy);

                element[origX, origY] = 0;
                currentPathx.Remove(origX);
                currentPathy.Remove(origY);
            }
        }
    }
}

