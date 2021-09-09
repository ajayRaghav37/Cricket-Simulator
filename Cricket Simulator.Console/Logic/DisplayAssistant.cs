using System;
using System.Collections.Generic;
using System.Linq;
using Cricket_Simulator.Common;
using Cricket_Simulator.Entities;
using Cricket_Simulator.Enums;
using Repositories.TableEntities;
using static System.Console;
using static Cricket_Simulator.Console.ConsoleHelper;
using static Cricket_Simulator.Console.Logic.SimulationHelper;
using static Cricket_Simulator.Logic.Utilities;
using static Logging.Logger;

namespace Cricket_Simulator.Console.Logic
{
    public static class DisplayAssistant
    {
        public static void DisplayPlayers(List<Teams> teams, List<Squads> squads, List<Players> players)
        {
            LogEntry(inputParams: new object[] { teams, squads, players });

            int[] columnWidths = GetColumnWidths(4, 0, 4, 6, 4, 0, 4);

            if (players == null)
                throw new ArgumentNullException(nameof(players));

            if (squads == null)
                throw new ArgumentNullException(nameof(squads));

            WriteLine();

            List<int> playersInTeam = new List<int>();

            for (int i = 0; i < teams.Count; i++)
                playersInTeam.Add(squads.Count(item => item.TeamId == teams[i].TeamId));

            int mostPlayersInTeam = playersInTeam.Max();

            for (int i = 0; i <= mostPlayersInTeam; i++)
            {
                for (int j = 0; j < teams.Count; j++)
                {
                    if (i == 0)
                    {
                        string teamTitle = teams[j].TeamName.ToUpper() + " (" + teams[j].ShortName + ")";

                        if (j == 0)
                            Write(teamTitle.PadRight(columnWidths[0] + columnWidths[1] + columnWidths[2] + columnWidths[3]));
                        else
                            Write(teamTitle);
                    }
                    else if (i > playersInTeam[j])
                    {
                        if (j == 0)
                            Write(string.Empty.PadRight(columnWidths[0] + columnWidths[1] + columnWidths[2] + columnWidths[3]));
                    }
                    else
                    {
                        int playerId = squads.FindAll(item => item.TeamId == teams[j].TeamId)[i - 1].PlayerId;

                        Players player = players.Find(item => item.PlayerId == playerId);

                        string playerSuffix = string.Empty;

                        if (teams[j].Captain == playerId)
                            playerSuffix = " (c)";
                        else if (teams[j].ViceCaptain == playerId)
                            playerSuffix = " (vc)";

                        Write((i + ". ").PadRight(columnWidths[j * 4 + 0]) + (player.PlayerName + playerSuffix).PadRight(columnWidths[j * 4 + 1]) + GetRoleString(player.Role).PadRight(columnWidths[j * 4 + 2]) + string.Empty.PadRight((1 - j) * columnWidths[3]));
                    }
                }

                if (i == 0)
                    WriteLine();

                WriteLine();
            }

            LogExit();
        }

        public static void DisplayEndOfOver(Player batsmanStrike, Player batsmanNonStrike, Player bowler, Player lastBowler, string teamShortName, int totalRuns, int totalWickets, int overNumber, int ballsInCurrentOver, int totalRunsBeforeThisOver, int totalWicketsBeforeThisOver, int[] columnWidths)
        {
            LogEntry(inputParams: new object[] { batsmanStrike, batsmanNonStrike, bowler, lastBowler, teamShortName, totalRuns, totalWickets, overNumber, ballsInCurrentOver, totalRunsBeforeThisOver, totalWicketsBeforeThisOver, columnWidths });

            if (bowler == null)
                throw new ArgumentNullException(nameof(bowler));

            if (columnWidths == null)
                throw new ArgumentNullException(nameof(columnWidths));

            string overSummary = "Over " + overNumber + " (";

            if (totalRuns - totalRunsBeforeThisOver > 0)
                overSummary += GetQuantifiedString("run", totalRuns - totalRunsBeforeThisOver);
            else if (ballsInCurrentOver == 6)
            {
                overSummary += "Maiden";
                bowler.Bowling.Maidens++;
            }
            else
            {
                overSummary += "0 runs";
            }

            if (totalWickets - totalWicketsBeforeThisOver > 0)
                overSummary += ", " + GetQuantifiedString("wicket", totalWickets - totalWicketsBeforeThisOver);

            overSummary += ")";

            WriteLine("\n" + overSummary.PadRight(columnWidths[0] + columnWidths[1]) + (teamShortName + " " + totalRuns + "/" + totalWickets).PadLeft(columnWidths[2]) + "\n");
            DisplayCurrentPlayersStats(batsmanStrike, batsmanNonStrike, bowler, lastBowler);

            Write("\n\n");

            LogExit();
        }

        public static void DisplayCurrentPlayersStats(Player batsmanStrike, Player batsmanNonStrike, Player bowler, Player lastBowler)
        {
            LogEntry(inputParams: new object[] { batsmanStrike, batsmanNonStrike, bowler, lastBowler });

            int[] columnWidths = GetColumnWidths(14, 0, 6, 14, 0);

            if (bowler == null)
                throw new ArgumentNullException(nameof(bowler));

            Player batsmanToPrint = batsmanNonStrike ?? batsmanStrike;

            Write((batsmanToPrint.ShortName.PadRight(columnWidths[0]) + batsmanToPrint.Batting.Display.PadLeft(columnWidths[1])).PadRight(columnWidths[0] + columnWidths[1] + columnWidths[2]));

            Write(bowler.ShortName.PadRight(columnWidths[3]) + bowler.Bowling.Display.PadLeft(columnWidths[4]) + "\n");

            batsmanToPrint = (batsmanToPrint == batsmanNonStrike ? batsmanStrike : batsmanNonStrike);

            if (batsmanToPrint != null)
                Write((batsmanToPrint.ShortName.PadRight(columnWidths[0]) + batsmanToPrint.Batting.Display.PadLeft(columnWidths[1])).PadRight(columnWidths[0] + columnWidths[1] + columnWidths[2]));
            else
                Write(string.Empty.PadRight(columnWidths[0] + columnWidths[1] + columnWidths[2]));

            if (lastBowler != null)
                Write(lastBowler.ShortName.PadRight(columnWidths[3]) + lastBowler.Bowling.Display.PadLeft(columnWidths[4]));

            LogExit();
        }

        public static void DisplayScoreCard(List<Teams> teams, int battingTeamIndex, List<Player> battingPlayers, List<Player> bowlingPlayers, List<Extra> extras, List<FallOfWicket> fallOfWickets)
        {
            LogEntry(inputParams: new object[] { teams, battingTeamIndex, battingPlayers, bowlingPlayers, extras, fallOfWickets });

            if (teams == null)
                throw new ArgumentNullException(nameof(teams));

            WriteLine("\n" + "FIRST INNINGS SCORECARD".PadCenter(USABLE_WIDTH) + "\n");

            DisplayBattingCard(teams[battingTeamIndex], battingPlayers, GetOversString(bowlingPlayers.Sum(item => item.Bowling.Balls)), extras);

            DisplayBowlingCard(teams[1 - battingTeamIndex], bowlingPlayers, fallOfWickets);

            LogExit();
        }

        private static void DisplayBowlingCard(Teams team, List<Player> bowlingPlayers, List<FallOfWicket> fallOfWickets)
        {
            LogEntry(inputParams: new object[] { team, bowlingPlayers, fallOfWickets });

            int[] columnWidths = GetColumnWidths(0, 5, 5, 5, 5, 5, 8, 5, 5);

            WriteLine();

            WriteLine((team.TeamName + " Bowling").PadRight(columnWidths[0]) + "O".PadLeft(columnWidths[1]) + "M".PadLeft(columnWidths[2]) + "D".PadLeft(columnWidths[3]) + "R".PadLeft(columnWidths[4]) + "W".PadLeft(columnWidths[5]) + "Econ".PadLeft(columnWidths[6]) + GetExtraTypeNotation(ExtraType.NoBall).PadLeft(columnWidths[7]) + GetExtraTypeNotation(ExtraType.Wide).PadLeft(columnWidths[8]) + "\n");

            foreach (Player bowler in bowlingPlayers)
                WriteLine(bowler.ShortName.PadRight(columnWidths[0]) + bowler.Bowling.Overs.PadLeft(columnWidths[1]) + bowler.Bowling.Maidens.ToString().PadLeft(columnWidths[2]) + bowler.Bowling.DotBalls.ToString().PadLeft(columnWidths[3]) + bowler.Bowling.Runs.ToString().PadLeft(columnWidths[4]) + bowler.Bowling.Wickets.ToString().PadLeft(columnWidths[5]) + string.Format("{0:0.00}", bowler.Bowling.Economy).PadLeft(columnWidths[6]) + bowler.Bowling.NoBalls.ToString().PadLeft(columnWidths[7]) + bowler.Bowling.WideBalls.ToString().PadLeft(columnWidths[8]));

            if (fallOfWickets.Count > 0)
                DisplayFallOfWickets(fallOfWickets);

            LogExit();
        }

        private static void DisplayFallOfWickets(List<FallOfWicket> fallOfWickets)
        {
            LogEntry(inputParams: fallOfWickets);

            int[] columnWidths = GetColumnWidths(0, 6, 0);

            WriteLine("\nFall of wickets\n");

            for (int i = 0; i < fallOfWickets.Count; i++)
            {
                if (i % 2 == 1)
                    WriteLine(fallOfWickets[i].Display);
                else
                    Write(fallOfWickets[i].Display.PadRight(columnWidths[0] + columnWidths[1]));
            }

            LogExit();
        }

        private static void DisplayBattingCard(Teams team, List<Player> battingPlayers, string overs, List<Extra> extras)
        {
            LogEntry(inputParams: new object[] { team, battingPlayers, overs, extras });

            int[] columnWidths = GetColumnWidths(14, 0, 5, 5, 5, 5, 9);

            WriteLine((team.TeamName + " Batting").PadRight(columnWidths[0] + columnWidths[1]) + "R".PadLeft(columnWidths[2]) + "B".PadLeft(columnWidths[3]) + "4s".PadLeft(columnWidths[4]) + "6s".PadLeft(columnWidths[5]) + "SR".PadLeft(columnWidths[6]) + "\n");

            foreach (Player batsman in battingPlayers.FindAll(item => !string.IsNullOrEmpty(item.Batting.Status)))
                Write(batsman.ShortName.PadRight(columnWidths[0]) + batsman.Batting.Status.PadRight(columnWidths[1]) + batsman.Batting.Runs.ToString().PadLeft(columnWidths[2]) + batsman.Batting.BallsFaced.ToString().PadLeft(columnWidths[3]) + batsman.Batting.Fours.ToString().PadLeft(columnWidths[4]) + batsman.Batting.Sixes.ToString().PadLeft(columnWidths[5]) + string.Format("{0:0.00}", batsman.Batting.StrikeRate).PadLeft(columnWidths[6]) + "\n");

            Write("\n" + "Extras".PadRight(columnWidths[0]));
            int totalExtras = extras.Sum(item => item.Runs);
            Write(GetExtrasBreakup(extras, totalExtras).PadRight(columnWidths[1]));
            WriteLine(extras.Sum(item => item.Runs).ToString().PadLeft(columnWidths[2]));

            Write("Total".PadRight(columnWidths[0]));
            int wickets = battingPlayers.Count(item => item.Batting.Status.NotIn(null, "not out"));
            Write(("(" + (wickets == 10 ? "All out" : GetQuantifiedString("wicket", wickets)) + "; " + overs + " overs" + ")").PadRight(columnWidths[1]));
            int totalRuns = battingPlayers.Sum(item => item.Batting.Runs) + totalExtras;
            WriteLine(totalRuns.ToString().PadLeft(columnWidths[2]) + " (" + string.Format("{0:0.00}", GetRunRate(totalRuns, overs)) + " runs per over)");

            List<Player> didNotBat = battingPlayers.FindAll(item => string.IsNullOrEmpty(item.Batting.Status));
            if (didNotBat.Count > 0)
            {
                Write("\nDid not bat: ");

                for (int i = 0; i < didNotBat.Count; i++)
                {
                    Write(didNotBat[i].ShortName);
                    if (i < didNotBat.Count - 1)
                        Write(", ");
                }

                WriteLine();
            }

            LogExit();
        }

        private static string GetExtrasBreakup(List<Extra> extras, int totalExtras)
        {
            LogEntry(inputParams: new object[] { extras, totalExtras });

            if (totalExtras > 0)
            {
                string extrasBreakup = "(";
                int progressiveExtras = 0;

                for (int i = 0; i < 5; i++)
                {
                    int runsThroughExtra = extras.FindAll(item => item.ExtraType == (ExtraType)i).Sum(item => item.Runs);

                    if (runsThroughExtra == 0)
                        continue;

                    progressiveExtras += runsThroughExtra;
                    extrasBreakup += GetExtraTypeNotation((ExtraType)i) + " " + runsThroughExtra;

                    if (progressiveExtras < totalExtras)
                        extrasBreakup += ", ";
                    else
                    {
                        extrasBreakup += ")";

                        LogExit(extrasBreakup);
                        return extrasBreakup;
                    }
                }
            }

            LogExit(string.Empty);
            return string.Empty;
        }
    }
}
