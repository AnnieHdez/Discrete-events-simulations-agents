using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoSimulacionAgentes;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            ProyectoSimulacionAgentes.Program p = new ProyectoSimulacionAgentes.Program();
            p.Run(6, 6, 10, 2, 2, 6);
            Console.ReadLine();
        }
    }
}

