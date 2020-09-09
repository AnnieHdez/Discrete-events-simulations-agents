using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSimulacionAgentes
{
    class Map
    {
        private Map.Elements[,] map;
        private int currentNumResources; //Keep count of how many resorces are in the map at all times
        private List<int> availablePositions = new List<int>(); //Keep track of all empty position at the map at all times
        private Random position = new Random();

        public Map(int N, int numAgents, int numTeams, int numResources, out List<Warehouse> warehouses)
        {
            map = new Map.Elements[N, N];
            currentNumResources = numResources;
           // numResources = _numResources;
           // numAgents = _numAgents;
           // numTeams = _numTeams;

            if (numTeams + numAgents + numResources > N * N)
                throw new ArgumentException("Can't be more objects than space in the map");

            if (numAgents < numTeams)
                throw new ArgumentException("Can't be more teams than agents");

            //All positions are empty
            for (int i = 0; i < N * N; i++)
                availablePositions.Add(i);

            Initialize(numAgents, numTeams, numResources, out warehouses);
        }

        //Place the  Agents, Resources and Warehouses at random positions in the map
        private void Initialize( int numAgents, int numTeams, int numResources, out List<Warehouse> warehouses)
        {
            int rand = position.Next(availablePositions.Count - 1);

            warehouses = new List<Warehouse>();

            for (int i = 0; i < numTeams; i++)
            {
                int pos = availablePositions[rand];
                availablePositions.RemoveAt(rand);
                map[(int)pos / map.GetLength(0), pos % map.GetLength(0)] = Elements.Warehouse;
                rand = position.Next(availablePositions.Count - 1);
                warehouses.Add(new Warehouse(i, (int)pos / map.GetLength(0), pos % map.GetLength(0)));
            }

            for (int i = 0; i < numAgents; i++)
            {
                int pos = availablePositions[rand];
                availablePositions.RemoveAt(rand);
                map[(int)pos / map.GetLength(0), pos % map.GetLength(0)] = Elements.Agent;
                rand = position.Next(availablePositions.Count);
            }

            for (int i = 0; i < numResources; i++)
            {
                int pos = availablePositions[rand];
                availablePositions.RemoveAt(rand);
                map[(int)pos / map.GetLength(0), pos % map.GetLength(0)] = Elements.Resource;
                rand = position.Next(availablePositions.Count);
            }
        }

        public Map.Elements[,] currentMap
        {
            get
            {
                return map;
            }
        }

        //Place the new resorces at random positions at the map at each turn
        public void PlaceNewResorces()
        {
            //quantity of new Resources to place in the map, less than half the availables positions
            int numNewResources = position.Next((availablePositions.Count / 3)-1);

            int rand = position.Next(availablePositions.Count);

            for (int i = 0; i < numNewResources; i++)
            {
                int pos = availablePositions[rand];
                availablePositions.RemoveAt(rand);
                map[(int)pos / map.GetLength(0), pos % map.GetLength(0)] = Elements.Resource;
                rand = position.Next(availablePositions.Count);
            }

            this.Print();
        }
    
        //the different kinds of elements that can be place in the map
        public enum Elements
        {
            Nothing = 0,
            Mark1 = 1, //Marks that make the reactive agents in the map
            Mark2 = 2,
            Agent = 3,
            Resource = 4,
            Warehouse = 5

        }

        //Delete a especific resource from the map
        public void DeleteResorce(int x, int y)
        {
            if (x < 0 || x > map.GetLength(0) - 1 || y < 0 || y > map.GetLength(0) - 1)
                throw new ArgumentOutOfRangeException("the position isn't inside the map");

            if (map[x, y] != Elements.Resource)
                throw new ArgumentException("Isn't a resorce at that position");

            map[x, y] = 0;
            availablePositions.Add(map.GetLength(0) * x + y);
            this.Print();
        }

        //Move a agent in the map
        public void MoveAgent(Agent agent, int x, int y)
        {
            if (x < 0 || x > map.GetLength(0) - 1 || y < 0 || y > map.GetLength(0) - 1)
                throw new ArgumentOutOfRangeException("the position isn't inside the map");

            if ((int)map[x, y] > 2)
                throw new ArgumentException("Can't move to a busy position");

            int temp = (int)map[x, y];
            //Place the mark that was before the agent move
            map[agent.Xpos, agent.Ypos] = agent.rememberMark;

            map[x, y] = Elements.Agent;
           
            availablePositions.Add(map.GetLength(0) * agent.Xpos + agent.Ypos);
            availablePositions.Remove(map.GetLength(0) * x + y);
            this.Print();
        }

        public void Print()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(0); j++)
                {
                    Console.Write(map[i, j] + ", ");
                    for (int k = 0; k < 9-map[i,j].ToString().Length; k++)
                    {
                        Console.Write(" ");
                    }
                }

                Console.WriteLine();
            }
            Console.WriteLine('\n');
            Console.WriteLine('\n');
        }
    }
}
