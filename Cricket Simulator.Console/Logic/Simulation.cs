using System.Collections.Generic;
using static Logging.Logger;
using Repositories.TableEntities;
using static System.Console;
using static Cricket_Simulator.Console.Logic.CheckPoint;
using static Cricket_Simulator.Console.Logic.DataLoader;
using static Cricket_Simulator.Console.Logic.DisplayAssistant;
using static Cricket_Simulator.Console.Logic.TeamSelection;
using static Cricket_Simulator.Console.Logic.Toss;

namespace Cricket_Simulator.Console.Logic
{
    public static class Simulation
    {
        public static void Start()
        {
            LogEntry();

            List<Teams> teams = LoadTeams();

            if (!SelectTeams(teams))
                return;

            Write("Loading players");

            List<Squads> squads = LoadSquads(teams);

            List<Players> players = LoadPlayers(squads);

            WriteLine();

            DisplayPlayers(teams, squads, players);

            if (!SelectPlayers(teams, squads, players))
                return;

            DisplayPlayers(teams, squads, players);

            WriteLine();

            SelectLeaders(teams, squads, players);

            int battingTeamIndex = GetBattingTeamIndex(teams, players);

            int checkPoint = GetCheckPoint();

            int firstInningsScore = FirstInnings.Start(teams, squads, players, battingTeamIndex, checkPoint);

            if (checkPoint < 4)
                ReadKey();

            //Second inning

            ReadKey();

            LogExit();
        }
    }
}
