using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace HorseRacing
{
    internal class Program
    {
        private const int FinishLine = 30;
        private static readonly object _lock = new();
        private static readonly List<int> FinishOrder = new();
        private static readonly Dictionary<int, int> HorsePositions = new();
        private static readonly Dictionary<int, TimeSpan> HorseTimes = new();

        static void Main()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Horse Race").Centered().Color(Color.Orange1));
            AnsiConsole.MarkupLine("[bold yellow]Welcome to the Horse Race Simulator[/]");

            int horseCount = AskHorseCount();
            if (horseCount <= 0) return;

            for (int i = 1; i <= horseCount; i++)
                HorsePositions[i] = 0;

            var threads = new Thread[horseCount];
            var stopwatches = new Stopwatch[horseCount];

            for (int i = 0; i < horseCount; i++)
            {
                int horseNumber = i + 1;
                stopwatches[i] = Stopwatch.StartNew();

                threads[i] = new Thread(() =>
                {
                    RunHorse(horseNumber);
                    stopwatches[horseNumber - 1].Stop();

                    lock (_lock)
                    {
                        HorseTimes[horseNumber] = stopwatches[horseNumber - 1].Elapsed;
                    }
                });
                threads[i].Start();
            }

            var table = RenderRace();

            var live = AnsiConsole.Live(table)
                .AutoClear(false)
                .Overflow(VerticalOverflow.Crop)
                .Cropping(VerticalOverflowCropping.Top);

            live.Start(ctx =>
            {
                while (FinishOrder.Count < horseCount)
                {
                    lock (_lock)
                    {
                        table.Rows.Clear();

                        foreach (var kvp in HorsePositions.OrderBy(h => h.Key))
                        {
                            var horseNum = kvp.Key;
                            var pos = kvp.Value;
                            string track = new string('=', pos) + ">";
                            table.AddRow($"Horse #{horseNum}", track);
                        }
                    }
                    ctx.Refresh();
                    Thread.Sleep(100);
                }
            });

            foreach (var thread in threads)
                thread.Join();

            ShowResults();
        }

        static int AskHorseCount()
        {
            AnsiConsole.Markup("[green]Enter number of horses:[/] ");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int count) && count > 1 && count <10)
                return count;

            AnsiConsole.MarkupLine("[red]Invalid input. Exiting...[/]");
            return 0;
        }

        static void RunHorse(int horseNumber)
        {
            var rnd = new Random(horseNumber * DateTime.Now.Millisecond);

            while (true)
            {
                Thread.Sleep(rnd.Next(100, 200));

                lock (_lock)
                {
                    if (HorsePositions[horseNumber] < FinishLine)
                        HorsePositions[horseNumber]++;
                    else
                        break;
                }
            }

            lock (_lock)
            {
                if (!FinishOrder.Contains(horseNumber))
                    FinishOrder.Add(horseNumber);
            }
        }

        static Table RenderRace()
        {
            var table = new Table().Border(TableBorder.None);
            table.AddColumn(new TableColumn("[u]Horse[/]").LeftAligned());
            table.AddColumn(new TableColumn("[u]Track[/]").LeftAligned());

            lock (_lock)
            {
                foreach (var kvp in HorsePositions.OrderBy(h => h.Key))
                {
                    var horseNum = kvp.Key;
                    var pos = kvp.Value;
                    string track = new string('=', pos) + ">";
                    table.AddRow($"Horse #{horseNum}", track);
                }
            }

            return table;
        }

        static void ShowResults()
        {
            AnsiConsole.MarkupLine("\n[bold green]Final Standings:[/]");
            var table = new Table().Centered();
            table.AddColumn("Place");
            table.AddColumn("Horse");
            table.AddColumn("Time (seconds)");

            for (int i = 0; i < FinishOrder.Count; i++)
            {
                var horseNum = FinishOrder[i];
                string time = HorseTimes.ContainsKey(horseNum)
                    ? HorseTimes[horseNum].TotalSeconds.ToString("0.00")
                    : "N/A";

                table.AddRow($"{i + 1}", $"Horse #{horseNum}", time);
            }

            AnsiConsole.Write(table);
        }
    }
}
