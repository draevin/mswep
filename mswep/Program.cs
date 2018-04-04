using MapLogic;
using System;

namespace mswep
{
    class Program
    {
        static NodeMap map;
        static int selectedX, selectedY;
        static bool markingSelection = false;

        static void Main(string[] args)
        {
            Console.SetWindowSize(100, 30);
            GameStateEnum gameState;
            do
            {
                gameState = Menu();
                if (gameState == GameStateEnum.Instructions)
                {
                    Instructions();
                }
                else if (gameState == GameStateEnum.Play)
                {
                    Play();
                }
            } while (gameState != GameStateEnum.Exit);
        }

        static void Instructions()
        {
            Console.Clear();
            Console.WriteLine("-----------------------------");
            Console.WriteLine("----------- MSWEP -----------");
            Console.WriteLine("-------- How To Play --------");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("");
            Console.WriteLine("Select coordinates to uncover");
            Console.WriteLine("using the following format: ");
            Console.WriteLine("   \"x.y\"");
            Console.WriteLine("Mark suspected mines using a");
            Console.WriteLine("\"*\" before the coordinates:");
            Console.WriteLine("   \"*x.y\"");
            Console.WriteLine("Numerals indicate the number");
            Console.WriteLine("of adjacent mines. To win,");
            Console.WriteLine("mark all mines, or isolate");
            Console.WriteLine("them by uncovering all of the");
            Console.WriteLine("surrounding empty cells");
            Console.WriteLine("-----------------------------");
            Console.WriteLine(" [press any key to continue] ");
            Console.WriteLine("-----------------------------");
            Console.ReadKey();

        }

        static void Play()
        {

            bool successfulParse, firstTry;
            int size;
            do
            {
                Console.Clear();
                Console.WriteLine("Input size (note: size must be at least 10):");
                successfulParse = Int32.TryParse(Console.ReadLine(), out size) && size >= 10;
            } while (!successfulParse);

            map = new NodeMap(size);
            firstTry = true;

            do
            {
                selectedX = selectedY = 0;

                markingSelection = false;

                SelectNode();

                while (firstTry)
                {
                    if (map.nodes[selectedX, selectedY].state == NodeState.Mine)
                    {
                        map = new NodeMap(size);
                    }
                    else
                    {
                        firstTry = false;
                    }
                }

                if (markingSelection)
                {
                    if (map.nodes[selectedX, selectedY].isMarked)
                    {
                        map.nodes[selectedX, selectedY].isMarked = false;
                    }
                    else
                    {
                        map.nodes[selectedX, selectedY].isMarked = true;
                    }
                }
                else
                {
                    if (!map.nodes[selectedX, selectedY].isMarked)
                    {
                        map.nodes[selectedX, selectedY].isCovered = false;

                        if (map.nodes[selectedX, selectedY].state == NodeState.Empty)
                        {
                            map.ClearConsecutiveEmpties(selectedX, selectedY);
                        }
                    }
                }

            } while (!map.WinState() && !map.LoseState());

            Console.Clear();
            DrawMap();
            Console.WriteLine();

            if (map.WinState())
            {
                for (int yCoord = 0; yCoord < map.sideLength; yCoord++)
                {
                    for (int xCoord = 0; xCoord < map.sideLength; xCoord++)
                    {
                        if (map.nodes[xCoord, yCoord].isCovered && map.nodes[xCoord, yCoord].state == NodeState.Mine)
                        {
                            map.nodes[xCoord, yCoord].isMarked = true;
                        }
                    }
                }

                Console.Clear();
                DrawMap();

                Console.WriteLine();
                Console.WriteLine("Mines?");
                Console.WriteLine("Swept.");
                Console.WriteLine();
                Console.WriteLine($"{map.setMines} of them, to be exact.");
                Console.WriteLine($"And in only {map.moves} moves!");
                Console.WriteLine();
                Console.WriteLine("[press any key to continue]");
            }

            if (map.LoseState())
            {
                for (int yCoord = 0; yCoord < map.sideLength; yCoord++)
                {
                    for (int xCoord = 0; xCoord < map.sideLength; xCoord++)
                    {
                        map.nodes[xCoord, yCoord].isCovered = false;
                    }
                }

                Console.Clear();
                DrawMap();

                Console.WriteLine();
                Console.WriteLine("|     | |");
                Console.WriteLine();
                Console.WriteLine("| |   | _");
                Console.WriteLine();
                if (map.sweptMines > 0)
                {
                    Console.WriteLine($"You swept {map.sweptMines} of {map.setMines} mines in {map.moves} moves,");
                    Console.WriteLine("but somehow still managed to blow yourself up.");
                }
                else
                {
                    Console.WriteLine("You didn't sweep a single mine!");
                    Console.WriteLine("Maybe take up space pinball, instead.");
                }

                Console.WriteLine();
                Console.WriteLine("[press any key to continue]");
            }

            Console.ReadKey();
        }

        static void DrawMap()
        {
            Console.Write("       ");
            Console.Write($"Mines Placed: {map.setMines}     Est. Mines Remaining: {map.setMines - map.sweptMines}");
            Console.Write("\r\n\r\n");

            Console.Write("       ");
            for (int coord = 0; coord < map.sideLength; coord++)
            {
                if (coord < 10)
                {
                    Console.Write($" {coord} ");
                }
                else
                {
                    Console.Write(coord + " ");
                }
            }            
            Console.Write("\r\n\r\n");
            for (int yCoord = 0; yCoord < map.sideLength; yCoord++)
            {
                if (yCoord < 10)
                {
                    Console.Write($"  {yCoord}    ");
                }
                else
                {
                    Console.Write($" {yCoord}    ");
                }

                for (int xCoord = 0; xCoord < map.sideLength; xCoord++)
                {
                    map.nodes[xCoord, yCoord].DrawNode();

                    if (xCoord == map.sideLength - 1)
                    {
                        Console.Write("\r\n");
                    }
                }
            }
        }

        static void SelectNode()
        {
            bool successfulSelection;

            do
            {
                Console.Clear();
                DrawMap();

                Console.WriteLine();
                Console.WriteLine("Select coordinates:");
                string selectionInput = Console.ReadLine();

                if (selectionInput == "reveal")
                {
                    for (int yCoord = 0; yCoord < map.sideLength; yCoord++)
                    {
                        for (int xCoord = 0; xCoord < map.sideLength; xCoord++)
                        {
                            map.nodes[xCoord, yCoord].isCovered = false;
                        }
                    }
                }

                if (selectionInput == "hide")
                {
                    for (int yCoord = 0; yCoord < map.sideLength; yCoord++)
                    {
                        for (int xCoord = 0; xCoord < map.sideLength; xCoord++)
                        {
                            map.nodes[xCoord, yCoord].isCovered = true;
                        }
                    }
                }

                if (selectionInput.StartsWith("*"))
                {
                    markingSelection = true;
                    selectionInput = selectionInput.TrimStart('*');
                }
                string[] selectionCoords = selectionInput.Split('.');

                if (selectionCoords.Length != 2)
                {
                    successfulSelection = false;
                }
                else
                {
                    successfulSelection = Int32.TryParse(selectionCoords[0], out selectedX) && Int32.TryParse(selectionCoords[1], out selectedY);
                    successfulSelection &= selectedX < map.sideLength;
                    successfulSelection &= selectedY < map.sideLength;
                }

            } while (!successfulSelection);

            map.moves++;

            if (markingSelection && map.nodes[selectedX, selectedY].state == NodeState.Mine)
                map.sweptMines++;
        }

        static GameStateEnum Menu()
        {
            bool successfulSelection = false;
            GameStateEnum selection = GameStateEnum.Exit;

            do
            {
                Console.Clear();
                Console.WriteLine("-----------------------------");
                Console.WriteLine("----------- MSWEP -----------");
                Console.WriteLine("-----------------------------");
                Console.WriteLine("--         [P]lay!         --");
                Console.WriteLine("--     [I]nstructions?     --");
                Console.WriteLine("--        [E]xit...        --");
                Console.WriteLine("-----------------------------");
                char menuItem = Convert.ToChar(Console.Read()).ToString().ToUpper().ToCharArray()[0];
                if (menuItem == 'P')
                {
                    selection = GameStateEnum.Play;
                    successfulSelection = true;
                }
                else if (menuItem == 'I')
                {
                    selection = GameStateEnum.Instructions;
                    successfulSelection = true;
                }
                else if (menuItem == 'E')
                {
                    selection = GameStateEnum.Exit;
                    successfulSelection = true;
                }
            } while (!successfulSelection);

            return selection;
        }
    }
}
