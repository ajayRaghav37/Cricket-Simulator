using System;
using System.Linq;
using Cricket_Simulator.Common;
using static System.Console;
using static Logging.Logger;

namespace Cricket_Simulator.Console
{
    public static class ConsoleHelper
    {
        public const int USABLE_WIDTH = 72;

        public static string GetUserChoice(string prompt, params string[] options)
        {
            LogEntry(inputParams: new object[] { prompt, options });

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            string userChoice = options[GetUserChoiceIndex(prompt, options) - 1];

            LogExit(returnValue: userChoice);

            return userChoice;
        }

        public static int GetUserNumber(int minValue, int maxValue)
        {
            LogEntry(inputParams: new object[] { minValue, maxValue });

            int userChoice;

            while (!ReadValue(out userChoice).IsBetween(minValue, maxValue))
            {
                ForegroundColor = ConsoleColor.Yellow;
                WriteLine("Selection not within expected range. Try again.\n");
                ResetColor();
            }

            LogExit(returnValue: userChoice);
            return userChoice;
        }

        public static int GetUserChoiceIndex(string prompt, params string[] options)
        {
            LogEntry(inputParams: new object[] { prompt, options });

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            WriteLine(prompt);

            for (int i = 1; i <= options?.Length; i++)
                Write("\n" + i + ". " + options[i - 1]);

            Write("\n\nYour choice: ");

            int userChoiceIndex = GetUserNumber(1, options.Length) - 1;

            LogExit(returnValue: userChoiceIndex);
            return userChoiceIndex;
        }

        private static int ReadValue(out int item)
        {
            LogEntry();

            while (!int.TryParse(ReadLine(), out item))
            {
                ForegroundColor = ConsoleColor.Yellow;
                WriteLine("Invalid input. Try again.\n");
                ResetColor();
            }

            LogExit(returnValue: item);
            return item;
        }

        public static int[] GetColumnWidths(params int[] columnWidths)
        {
            LogEntry(inputParams: columnWidths);

            if (columnWidths == null)
                throw new ArgumentNullException(nameof(columnWidths));

            int numZeroWidths = columnWidths.Count(item => item == 0);
            int replacementWidth = (USABLE_WIDTH - columnWidths.Sum()) / numZeroWidths;
            int adjustment = (USABLE_WIDTH - columnWidths.Sum()) % numZeroWidths;

            for (int i = 0; i < columnWidths.Length; i++)
                if (columnWidths[i] == 0)

                {
                    columnWidths[i] = replacementWidth;

                    if (adjustment > 0)
                    {
                        columnWidths[i]++;
                        adjustment--;
                    }
                }

            LogExit(returnValue: columnWidths);
            return columnWidths;
        }
    }
}
