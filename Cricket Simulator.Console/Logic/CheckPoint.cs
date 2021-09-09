using static System.Console;
using static Cricket_Simulator.Console.ConsoleHelper;
using static Logging.Logger;
namespace Cricket_Simulator.Console.Logic
{
    public static class CheckPoint
    {
        public static int GetCheckPoint()
        {
            LogEntry();

            WriteLine();

            int checkPoint = GetUserChoiceIndex("Choose your prompt level.", "Ball", "Over", "Wicket", "Inning", "Match");

            LogExit(returnValue: checkPoint);
            return checkPoint;
        }
    }
}
