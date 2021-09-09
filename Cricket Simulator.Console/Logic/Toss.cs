using System;
using System.Collections.Generic;
using Repositories.TableEntities;
using static System.Console;
using static Cricket_Simulator.Configuration.Likelihoods;
using static Cricket_Simulator.Console.ConsoleHelper;
using static Cricket_Simulator.Logic.Utilities;
using static Logging.Logger;

namespace Cricket_Simulator.Console.Logic
{
    public static class Toss
    {
        public static int GetBattingTeamIndex(List<Teams> teams, List<Players> players)
        {
            LogEntry(inputParams: new object[] { teams, players });

            if (players == null)
                throw new ArgumentNullException(nameof(players));

            WriteLine("Time for the toss\n");

            int userSpinChoice = GetUserChoiceIndex("Who shall spin the coin?", players.Find(item => item.PlayerId == teams[0].Captain).PlayerName, players.Find(item => item.PlayerId == teams[1].Captain).PlayerName);

            WriteLine();

            int userTossChoice = GetUserChoiceIndex("Make a call for " + players.Find(item => item.PlayerId == teams[1 - userSpinChoice].Captain).PlayerName, "Heads", "Tails");

            int tossResult = GetRandomResultFromLikelihoods(HEADS_OR_TAILS);

            int teamWin = (userTossChoice == tossResult) ? (1 - userSpinChoice) : userSpinChoice;

            WriteLine();

            int userDecision = GetUserChoiceIndex("It's " + (tossResult == 0 ? "heads. " : "tails. ") + players.Find(item => item.PlayerId == teams[teamWin].Captain).PlayerName + " has won the toss. What do you want him to choose?", "Batting", "Fielding");

            WriteLine("\n" + teams[teamWin].TeamName + " has won the toss and elected to " + (userDecision == 0 ? "bat" : "field") + " first.");

            if (teamWin == userDecision)
            {
                LogExit(returnValue: 0);
                return 0;
            }
            else
            {
                LogExit(returnValue: 1);
                return 1;
            }
        }
    }
}
