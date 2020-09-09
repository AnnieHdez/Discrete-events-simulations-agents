using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSimulacionAgentes
{
    class ReactiveAgent : Agent
    {
        public ReactiveAgent(int _team, int _xpos, int _ypos, Map _map, Warehouse _myWarehouse)
            : base(_team, _xpos, _ypos, _map, _myWarehouse)
        { }

        override public void DoSomething()
        {
            List<int> xs;
            List<int> ys;

            if (this.empty)
            {
                //if is a resource at a reachable position, get one random
                if (SearchPos(this.xpos, this.ypos, Map.Elements.Resource, out xs, out ys))
                {
                    int selectPos = pos.Next(xs.Count);
                    map.DeleteResorce(xs[selectPos], ys[selectPos]);
                    this.empty = false;
                    return;
                }
                
                //if is an empty position at wich the agent can move, move to one random
                else if (SearchPosMove(this.xpos, this.ypos, Map.Elements.Nothing, out xs, out ys) || SearchPosMove(this.xpos, this.ypos, Map.Elements.Mark1, out xs, out ys) ||
                        SearchPosMove(this.xpos, this.ypos, Map.Elements.Mark2, out xs, out ys))
                {
                    int selectPos = pos.Next(xs.Count);
                    Map.Elements temp = (Map.Elements)map.currentMap[xs[selectPos], ys[selectPos]];
                    map.MoveAgent(this, xs[selectPos], ys[selectPos]);

                    //if already found the wearhouse mark the path taken
                    if (foundWH)
                        map.currentMap[this.xpos, this.ypos] = Map.Elements.Mark2;

                    rememberMark = temp;
                    this.xpos = xs[selectPos];
                    this.ypos = ys[selectPos];
                    return;
                }
            }

            else if (!this.empty)
            {
                //if the agent's wearhouse is at a reachable position, drop the resource
                if (SearchPos(this.xpos, this.ypos, Map.Elements.Warehouse, out xs, out ys))
                {
                    this.empty = true;
                    myWarehouse.StoreResources += 1;
                    foundWH = true;
                    return;
                }

                //if is an empty position at wich the agent can move, move to one random
                else if (SearchPosMove(this.xpos, this.ypos, Map.Elements.Mark2, out xs, out ys) || SearchPosMove(this.xpos, this.ypos, Map.Elements.Mark1, out xs, out ys) ||
                       SearchPosMove(this.xpos, this.ypos, Map.Elements.Nothing, out xs, out ys))
                {
                    int selectPos = pos.Next(xs.Count);
                    Map.Elements temp = (Map.Elements)map.currentMap[xs[selectPos], ys[selectPos]];
                    map.MoveAgent(this, xs[selectPos], ys[selectPos]);

                    if (rememberMark == Map.Elements.Mark2)
                        map.currentMap[this.xpos, this.ypos] = Map.Elements.Mark1;

                    else if ((rememberMark == Map.Elements.Mark1))
                        map.currentMap[this.xpos, this.ypos] = Map.Elements.Nothing;

                    rememberMark = temp;
                    this.xpos = xs[selectPos];
                    this.ypos = ys[selectPos];
                    return;
                }
            }
        }
    }
}
