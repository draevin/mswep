using System;

namespace MapLogic
{
    public class Node
    {
        public NodeState state;
        public bool isCovered;
        public bool isMarked;
        public int adjacentCount = 0;

        public Node(NodeState newState)
        {
            state = newState;
            isCovered = true;
        }

        public void DrawNode()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            string nodeOutput;
            if (isCovered)
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;

                if (isMarked)
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write("*");
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("]");
                }
                else
                {
                    Console.Write("[ ]");
                }
            }
            else
            {
                switch (state)
                {
                    case NodeState.Empty:
                        nodeOutput = "   ";
                        break;
                    case NodeState.Adjacent:
                        nodeOutput = $" {adjacentCount.ToString()} ";
                        switch (adjacentCount)
                        {
                            case 1:
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                            case 2:
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                break;
                            case 3:
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            case 4:
                                Console.ForegroundColor = ConsoleColor.DarkBlue;
                                break;
                            case 5:
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                break;
                            case 6:
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                break;
                            case 7:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            case 8:
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                break;
                        }
                        break;
                    case NodeState.Mine:
                        nodeOutput = " X ";
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        break;
                    default:
                        nodeOutput = "   ";
                        break;
                }

                Console.Write(nodeOutput);
            }

            Console.ResetColor();
        }


    }
}
