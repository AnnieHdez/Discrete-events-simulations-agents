using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSimulacionAgentes
{
    public class Program
    {   
        /// <summary>
        /// Class that run the simulation
        /// </summary>
        /// <param name="numProactiveAgents">Number of proactive agents to place</param>
        /// <param name="numReactiveAgents">Number of reactive agents to place</param>
        /// <param name="numTurns">Number of turns to run the simulation</param>
        /// <param name="numProactiveTeams">Number of teams to divide the proactive agents</param>
        /// <param name="numReactiveTeams">Number of teams to divide the reactive agents</param>
        /// <param name="size">Size of the map</param>
        public void Run(int numProactiveAgents, int numReactiveAgents, int numTurns, int numProactiveTeams,  int numReactiveTeams, int size)
        {
            Random ran = new Random();
            List<Agent> agents = new List<Agent>();
            List<Warehouse> warehouses;
            int reactives = numReactiveAgents;
            int proactives = numProactiveAgents;
            int numInitResourses = ran.Next(size + size/2); //Number of resources to place at the start in the map

            Map mymap = new Map(size, numProactiveAgents + numReactiveAgents, numProactiveTeams + numReactiveTeams, numInitResourses, out warehouses);
            mymap.Print();

            //Create the agents in the positions in the map
            for (int i = 0; i < mymap.currentMap.GetLength(0); i++)
            {
                for (int j = 0; j < mymap.currentMap.GetLength(0); j++)
                {
                    if (mymap.currentMap[i, j] == Map.Elements.Agent)
                    {
                        int team;
                        if (proactives > 0 && reactives > 0)
                        {
                            int random = ran.Next(2);
                            if(random == 0)
                            {
                                team = ran.Next(numProactiveTeams);
                                agents.Add(new ProactiveAgent(team, i, j, mymap, warehouses[team]));
                                proactives -= 1;
                            }
                            else
                            {
                                team = ran.Next(numProactiveTeams, numProactiveTeams+numReactiveTeams);
                                agents.Add(new ReactiveAgent(team, i, j, mymap, warehouses[team]));
                                reactives -= 1;
                            }
                        }

                        else if (proactives > 0)
                        {
                            team = ran.Next(numProactiveTeams);
                            agents.Add(new ProactiveAgent(team, i, j, mymap, warehouses[team]));
                        }

                        else
                        {
                            team = ran.Next(numProactiveTeams, numProactiveTeams + numReactiveTeams);
                            agents.Add(new ReactiveAgent(team, i, j, mymap, warehouses[team]));
                        }

                    }
                }
            }

            List<Agent> turnAgents = new List<Agent>();

            //Run all the agents in a random order
            for (int i = 0; i < numTurns; i++)
            {
                Console.WriteLine("Turno número " + i);
                turnAgents.AddRange(agents);

                for (int j = 0; j < numProactiveAgents + numReactiveAgents; j++)
                {
                    int pos = ran.Next(turnAgents.Count);
                    Agent currentAgent = turnAgents[pos];
                    turnAgents.RemoveAt(pos);
                    currentAgent.DoSomething();
                }

                mymap.PlaceNewResorces();
            }

            //Find the wining team
            Warehouse win = warehouses[0];
            for (int i = 0; i < warehouses.Count; i++)
            {
                if (warehouses[i].StoreResources >= win.StoreResources)
                    win = warehouses[i];
            }

            if(win.Owner>=numProactiveTeams)
                Console.WriteLine("Ganó el equipo " + win.Owner +" de agentes reactivos con "  + win.StoreResources + " recursos almacenados");
            else
                Console.WriteLine("Ganó el equipo " + win.Owner + " de agentes proactivos con " + win.StoreResources + " recursos almacenados");
        }
    }
}
