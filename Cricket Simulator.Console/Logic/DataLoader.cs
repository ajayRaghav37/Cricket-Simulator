using System;
using System.Collections.Generic;
using System.Linq;
using Repositories;
using Repositories.TableEntities;
using static System.Console;
using static Logging.Logger;

namespace Cricket_Simulator.Console.Logic
{
    public static class DataLoader
    {
        public static List<Teams> LoadTeams()
        {
            LogEntry();

            List<Teams> teams = null;

            while (teams == null)
            {
                Write("Loading teams");

                try
                {
                    teams = TeamsRepository.GetAll().Result;
                }
                catch
                {
                    ForegroundColor = ConsoleColor.Red;
                    Write("\nCould not connect to the database. Press enter to try again.");
                    ResetColor();
                    ReadLine();
                    WriteLine();
                }
            }

            LogExit(returnValue: teams);
            return teams;
        }

        public static List<Squads> LoadSquads(List<Teams> teams)
        {
            LogEntry(inputParams: teams);

            List<Squads> sqauds = SquadsRepository.GetAll(teams.Select(item => item.TeamId)).Result;

            LogExit(returnValue: sqauds);
            return sqauds;
        }

        public static List<Players> LoadPlayers(List<Squads> squads)
        {
            LogEntry(inputParams: squads);

            List<Players> players = PlayerRepository.GetAll(squads.Select(item => item.PlayerId)).Result;

            LogExit(returnValue: players);
            return players;
        }
    }
}
