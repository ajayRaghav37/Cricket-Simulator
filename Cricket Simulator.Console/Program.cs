using Cricket_Simulator.Console.Logic;
using static Logging.Logger;

namespace Cricket_Simulator.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            LogEntry(inputParams: args);

            Simulation.Start();

            LogExit();
        }
    }
}
