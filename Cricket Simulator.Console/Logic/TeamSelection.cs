using System;
using System.Collections.Generic;
using System.Linq;
using Repositories.TableEntities;
using static System.Console;
using static Cricket_Simulator.Console.ConsoleHelper;
using static Cricket_Simulator.Console.Logic.DisplayAssistant;
using static Cricket_Simulator.Console.Logic.SimulationHelper;
using static Logging.Logger;

namespace Cricket_Simulator.Console.Logic
{
    public static class TeamSelection
    {
        public static bool SelectTeams(List<Teams> teams)
        {
            LogEntry(inputParams: teams);

            if (teams?.Count > 2)
            {
                WriteLine("\n");

                for (int i = 0; i < teams.Count; i++)
                    WriteLine((i + 1) + ". " + teams[i].TeamName);

                List<Teams> selectedTeams = new List<Teams>();

                Write("\nSelect first team to play: ");
                selectedTeams.Add(teams[GetUserNumber(1, teams.Count) - 1]);

                Teams team;

                do
                {
                    Write("Select second team to play: ");
                    team = teams[GetUserNumber(1, teams.Count) - 1];
                } while (selectedTeams.Contains(team));

                selectedTeams.Add(team);

                teams.Clear();

                teams.AddRange(selectedTeams);
            }
            else if (teams?.Count < 2)
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine("Unable to find at least two teams to play");
                ResetColor();
                ReadKey();

                LogExit(returnValue: false);
                return false;
            }

            WriteLine();

            LogExit(returnValue: true);
            return true;
        }
        public static bool SelectPlayers(List<Teams> teams, List<Squads> squads, List<Players> players)
        {
            LogEntry(inputParams: new object[] { teams, squads, players });
            RemoveDuplicatePlayers(teams, squads, players);

            for (int i = 0; i < 2; i++)
            {
                if (squads.Count(item => item.TeamId == teams[i].TeamId) < 22)
                {
                    if (squads.Count(item => item.TeamId == teams[i].TeamId) < 11)
                    {
                        ForegroundColor = ConsoleColor.Red;
                        WriteLine("\nUnable to find at least eleven players to play for " + teams[i].TeamName);
                        ResetColor();
                        ReadKey();

                        LogExit(returnValue: false);
                        return false;
                    }

                    if (!TrySelectXiByRemoval(teams[i], squads, players))
                    {
                        i--;
                        continue;
                    }
                }
                else if (!TrySelectXiByAddition(teams[i], squads, players))
                {
                    i--;
                    continue;
                }
            }

            LogExit(returnValue: true);
            return true;
        }

        private static bool TrySelectXiByAddition(Teams team, List<Squads> squads, List<Players> players)
        {
            LogEntry(inputParams: new object[] { team, squads, players });

            WriteLine("\nSelect the playing XI for " + team.TeamName);

            List<Players> currentPlayers = players.FindAll(item => squads.FindAll(s => s.TeamId == team.TeamId).Select(s => s.PlayerId).Contains(item.PlayerId));

            List<Players> playersToAdd = new List<Players>();

            while (playersToAdd.Count < 11)
            {
                Players playerToAdd = currentPlayers[GetUserNumber(1, currentPlayers.Count) - 1];
                playersToAdd.Add(playerToAdd);
            }

            if (!IsValidPlayingXI(playersToAdd))
            {
                LogExit(returnValue: false);
                return false;
            }

            squads.RemoveAll(item => !playersToAdd.Select(p => p.PlayerId).Contains(item.PlayerId));
            playersToAdd.ForEach(item => currentPlayers.Remove(item));
            currentPlayers.ForEach(item => players.Remove(item));

            FixDuplicateCallingNames(playersToAdd);

            LogExit(returnValue: true);
            return true;
        }

        private static bool TrySelectXiByRemoval(Teams team, List<Squads> squads, List<Players> players)
        {
            LogEntry(inputParams: new object[] { team, squads, players });

            WriteLine("\nRemove few players from squad to select playing XI for " + team.TeamName);

            List<Players> currentPlayers = players.FindAll(item => squads.FindAll(s => s.TeamId == team.TeamId).Select(s => s.PlayerId).Contains(item.PlayerId));
            List<Players> playersToRemove = new List<Players>();
            while (currentPlayers.Count - playersToRemove.Count > 11)
            {
                Players playerToRemove = currentPlayers[GetUserNumber(1, currentPlayers.Count) - 1];
                playersToRemove.Add(playerToRemove);
            }

            playersToRemove.ForEach(item => currentPlayers.Remove(item));

            if (!IsValidPlayingXI(currentPlayers))
            {
                LogExit(returnValue: false);
                return false;
            }

            squads.RemoveAll(item => playersToRemove.Select(p => p.PlayerId).Contains(item.PlayerId));
            playersToRemove.ForEach(item => players.Remove(item));

            FixDuplicateCallingNames(currentPlayers);

            LogExit(returnValue: true);
            return true;
        }

        private static void RemoveDuplicatePlayers(List<Teams> teams, List<Squads> squads, List<Players> players)
        {
            LogEntry(inputParams: new object[] { teams, squads, players });

            if (squads.Select(item => item.PlayerId).Distinct().Count() != squads.Count)
            {
                List<int> playerIds = new List<int>();

                foreach (Squads squadItem in squads)
                {
                    if (playerIds.Contains(squadItem.PlayerId))
                    {
                        ForegroundColor = ConsoleColor.Yellow;
                        WriteLine(players.Find(item => item.PlayerId == squadItem.PlayerId).PlayerName + " is present in both the teams.");
                        ResetColor();
                        Write("\nIf you want him to play for " + teams[0].TeamName + " in this game, then press Y: ");
                        if (ReadKey().Key == ConsoleKey.Y)
                            squads.RemoveAll(item => item.PlayerId == squadItem.PlayerId && item.TeamId != teams[0].TeamId);
                        else
                            squads.RemoveAll(item => item.PlayerId == squadItem.PlayerId && item.TeamId == teams[0].TeamId);
                        WriteLine();
                    }
                    else
                        playerIds.Add(squadItem.PlayerId);
                }
            }

            LogExit();
        }

        private static void FixDuplicateCallingNames(List<Players> players)
        {
            LogEntry(inputParams: players);

            if (players.Select(item => item.CallingName).Distinct().Count() == players.Count)
            {
                LogExit();
                return;
            }

            foreach (Players player in players)
            {
                List<Players> duplicateCallingNamePlayers = players.FindAll(item => item.CallingName == player.CallingName && item != player);

                if (duplicateCallingNamePlayers.Count > 0)
                {
                    player.CallingName = player.ShortName;
                    duplicateCallingNamePlayers.ForEach(item => item.CallingName = item.ShortName);
                }
            }

            LogExit();
        }

        public static void SelectLeaders(List<Teams> teams, List<Squads> squads, List<Players> players)
        {
            LogEntry(inputParams: new object[] { teams, squads, players });

            if (players == null)
                throw new ArgumentNullException(nameof(players));

            if (squads == null)
                throw new ArgumentNullException(nameof(squads));

            bool teamsChanged = false;

            for (int i = 0; i < 2; i++)
            {
                bool changeCaptain = false;
                bool changeViceCaptain = false;

                if (!squads.FindAll(item => item.TeamId == teams[i].TeamId).Select(item => item.PlayerId).Contains(teams[i].Captain) || PromptChangeLeader(teams[i].TeamName))
                    changeCaptain = true;

                if (changeCaptain)
                {
                    Write("Select a captain for " + teams[i].TeamName + ": ");
                    teams[i].Captain = squads.FindAll(item => item.TeamId == teams[i].TeamId).Select(item => item.PlayerId).ElementAt(GetUserNumber(1, 11) - 1);
                    teamsChanged = true;
                }

                if (!squads.FindAll(item => item.TeamId == teams[i].TeamId).Select(item => item.PlayerId).Contains(teams[i].ViceCaptain) || teams[i].ViceCaptain == teams[i].Captain || PromptChangeLeader(teams[i].TeamName, true))
                    changeViceCaptain = true;

                if (changeViceCaptain)
                {
                    ChangeViceCaptain(teams, squads, players, i);
                    teamsChanged = true;
                }

                WriteLine();
            }

            if (teamsChanged)
                DisplayPlayers(teams, squads, players);

            LogExit();
        }

        private static bool PromptChangeLeader(string teamName, bool isViceCaptain = false)
        {
            LogEntry(inputParams: new object[] { teamName, isViceCaptain });
            bool result = false;

            Write("If you want to change " + (isViceCaptain ? "vice" : string.Empty) + " captain for " + teamName + ", then press Y: ");

            if (ReadKey().Key == ConsoleKey.Y)
                result = true;

            WriteLine();

            LogExit(returnValue: result);
            return result;
        }

        private static void ChangeViceCaptain(List<Teams> teams, List<Squads> squads, List<Players> players, int i)
        {
            LogEntry(inputParams: new object[] { teams, squads, players, i });

            while (true)
            {
                Write("Select a vice captain for " + teams[i].TeamName + ": ");
                teams[i].ViceCaptain = squads.FindAll(item => item.TeamId == teams[i].TeamId).Select(item => item.PlayerId).ElementAt(GetUserNumber(1, 11) - 1);

                if (teams[i].ViceCaptain == teams[i].Captain)
                {
                    ForegroundColor = ConsoleColor.Yellow;
                    WriteLine(players.Find(item => item.PlayerId == teams[i].ViceCaptain).PlayerName + " is already the captain. Please choose a different vice captain.\n");
                    ResetColor();
                }
                else
                    break;
            }

            LogExit();
        }

        private static bool IsValidPlayingXI(List<Players> players)
        {
            LogEntry(inputParams: players);

            int numBowlers = 0;
            bool hasKeeper = false;

            foreach (Players player in players)
            {
                string roleString = GetRoleString(player.Role).ToLower();
                if (roleString.Contains("o"))
                    numBowlers++;
                if (roleString.Contains("w"))
                    hasKeeper = true;
            }

            if (numBowlers >= 5 && hasKeeper)
            {
                LogExit(returnValue: true);
                return true;
            }
            else
            {
                ForegroundColor = ConsoleColor.Yellow;
                WriteLine("Not a valid playing XI. Select at least 5 bowlers and a wicketkeeper. Try again.");
                ResetColor();
                LogExit(returnValue: false);
                return false;
            }
        }
    }
}
