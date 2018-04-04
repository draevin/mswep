using System;

namespace MapLogic
{
    public class NodeMap
    {
        public int sideLength, setMines, sweptMines, moves;
        private int maxMines;

        public Node[,] nodes;
        private Random rand = new Random();

        private NodeState setState;
        public NodeMap(int size)
        {
            nodes = new Node[size, size];
            sideLength = size;

            maxMines = (4 * (sideLength * sideLength)) / 10;
            setMines = 0;

            MapStates();
        }

        public void MapStates()
        {
            GenerateMines();
            SetAdjacency();
        }

        public void GenerateMines()
        {
            int randInt;

            for (int yCoord = 0; yCoord < sideLength; yCoord++)
            {
                for (int xCoord = 0; xCoord < sideLength; xCoord++)
                {
                    setState = NodeState.Empty;

                    if (setMines <= maxMines)
                    {
                        randInt = (rand.Next() % (2 * sideLength / 3));

                        if (randInt == 0)
                        {
                            setState = NodeState.Mine;
                            setMines++;
                        }
                    }

                    nodes[xCoord, yCoord] = new Node(setState);

                }
            }
        }

        public void SetAdjacency()
        {
            bool upperEdge, lowerEdge, leftEdge, rightEdge;

            int count;

            for (int yCoord = 0; yCoord < sideLength; yCoord++)
            {
                upperEdge = lowerEdge = false;

                if (yCoord == 0)
                    upperEdge = true;
                if (yCoord == sideLength - 1)
                    lowerEdge = true;

                for (int xCoord = 0; xCoord < sideLength; xCoord++)
                {
                    count = 0;

                    leftEdge = rightEdge = false;

                    if (xCoord == 0)
                        leftEdge = true;
                    if (xCoord == sideLength - 1)
                        rightEdge = true;

                    if (upperEdge)
                    {
                        if (leftEdge)
                        {
                            count += AddAdjacentCount(xCoord + 1, yCoord);
                            count += AddAdjacentCount(xCoord + 1, yCoord + 1);
                            count += AddAdjacentCount(xCoord, yCoord + 1);

                        }
                        else if (rightEdge)
                        {
                            count += AddAdjacentCount(xCoord, yCoord + 1);
                            count += AddAdjacentCount(xCoord - 1, yCoord + 1);
                            count += AddAdjacentCount(xCoord - 1, yCoord);
                        }
                        else
                        {
                            count += AddAdjacentCount(xCoord + 1, yCoord);
                            count += AddAdjacentCount(xCoord + 1, yCoord + 1);
                            count += AddAdjacentCount(xCoord, yCoord + 1);
                            count += AddAdjacentCount(xCoord - 1, yCoord + 1);
                            count += AddAdjacentCount(xCoord - 1, yCoord);
                        }
                    }
                    else if (lowerEdge)
                    {
                        if (leftEdge)
                        {
                            count += AddAdjacentCount(xCoord, yCoord - 1);
                            count += AddAdjacentCount(xCoord + 1, yCoord - 1);
                            count += AddAdjacentCount(xCoord + 1, yCoord);
                        }
                        else if (rightEdge)
                        {
                            count += AddAdjacentCount(xCoord, yCoord - 1);
                            count += AddAdjacentCount(xCoord - 1, yCoord);
                            count += AddAdjacentCount(xCoord - 1, yCoord - 1);
                        }
                        else
                        {
                            count += AddAdjacentCount(xCoord - 1, yCoord);
                            count += AddAdjacentCount(xCoord - 1, yCoord - 1);
                            count += AddAdjacentCount(xCoord, yCoord - 1);
                            count += AddAdjacentCount(xCoord + 1, yCoord - 1);
                            count += AddAdjacentCount(xCoord + 1, yCoord);
                        }
                    }
                    else
                    {
                        if (leftEdge)
                        {
                            count += AddAdjacentCount(xCoord, yCoord - 1);
                            count += AddAdjacentCount(xCoord + 1, yCoord - 1);
                            count += AddAdjacentCount(xCoord + 1, yCoord);
                            count += AddAdjacentCount(xCoord + 1, yCoord + 1);
                            count += AddAdjacentCount(xCoord, yCoord + 1);
                        }
                        else if (rightEdge)
                        {
                            count += AddAdjacentCount(xCoord, yCoord - 1);
                            count += AddAdjacentCount(xCoord, yCoord + 1);
                            count += AddAdjacentCount(xCoord - 1, yCoord + 1);
                            count += AddAdjacentCount(xCoord - 1, yCoord);
                            count += AddAdjacentCount(xCoord - 1, yCoord - 1);
                        }
                        else
                        {
                            count += AddAdjacentCount(xCoord, yCoord - 1);
                            count += AddAdjacentCount(xCoord + 1, yCoord - 1);
                            count += AddAdjacentCount(xCoord + 1, yCoord);
                            count += AddAdjacentCount(xCoord + 1, yCoord + 1);
                            count += AddAdjacentCount(xCoord, yCoord + 1);
                            count += AddAdjacentCount(xCoord - 1, yCoord + 1);
                            count += AddAdjacentCount(xCoord - 1, yCoord);
                            count += AddAdjacentCount(xCoord - 1, yCoord - 1);
                        }
                    }

                    if (count > 0 && nodes[xCoord, yCoord].state != NodeState.Mine)
                    {
                        nodes[xCoord, yCoord].state = NodeState.Adjacent;
                        nodes[xCoord, yCoord].adjacentCount = count;
                    }
                }
            }
        }

        private int AddAdjacentCount(int x, int y)
        {
            if (nodes[x, y].state == NodeState.Mine)
                return 1;

            return 0;
        }

        public void ClearConsecutiveEmpties(int xCoord, int yCoord)
        {
            bool upperEdge, lowerEdge, leftEdge, rightEdge;


            upperEdge = lowerEdge = false;

            if (yCoord == 0)
                upperEdge = true;
            if (yCoord == sideLength - 1)
                lowerEdge = true;


            leftEdge = rightEdge = false;

            if (xCoord == 0)
                leftEdge = true;
            if (xCoord == sideLength - 1)
                rightEdge = true;

            if (upperEdge)
            {
                if (leftEdge)
                {
                    RevealEmptyOrAdjacent(xCoord + 1, yCoord);
                    RevealEmptyOrAdjacent(xCoord + 1, yCoord + 1);
                    RevealEmptyOrAdjacent(xCoord, yCoord + 1);

                }
                else if (rightEdge)
                {
                    RevealEmptyOrAdjacent(xCoord, yCoord + 1);
                    RevealEmptyOrAdjacent(xCoord - 1, yCoord + 1);
                    RevealEmptyOrAdjacent(xCoord - 1, yCoord);
                }
                else
                {
                    RevealEmptyOrAdjacent(xCoord + 1, yCoord);
                    RevealEmptyOrAdjacent(xCoord + 1, yCoord + 1);
                    RevealEmptyOrAdjacent(xCoord, yCoord + 1);
                    RevealEmptyOrAdjacent(xCoord - 1, yCoord + 1);
                    RevealEmptyOrAdjacent(xCoord - 1, yCoord);
                }
            }
            else if (lowerEdge)
            {
                if (leftEdge)
                {
                    RevealEmptyOrAdjacent(xCoord, yCoord - 1);
                    RevealEmptyOrAdjacent(xCoord + 1, yCoord - 1);
                    RevealEmptyOrAdjacent(xCoord + 1, yCoord);
                }
                else if (rightEdge)
                {
                    RevealEmptyOrAdjacent(xCoord, yCoord - 1);
                    RevealEmptyOrAdjacent(xCoord - 1, yCoord);
                    RevealEmptyOrAdjacent(xCoord - 1, yCoord - 1);
                }
                else
                {
                    RevealEmptyOrAdjacent(xCoord - 1, yCoord);
                    RevealEmptyOrAdjacent(xCoord - 1, yCoord - 1);
                    RevealEmptyOrAdjacent(xCoord, yCoord - 1);
                    RevealEmptyOrAdjacent(xCoord + 1, yCoord - 1);
                    RevealEmptyOrAdjacent(xCoord + 1, yCoord);
                }
            }
            else
            {
                if (leftEdge)
                {
                    RevealEmptyOrAdjacent(xCoord, yCoord - 1);
                    RevealEmptyOrAdjacent(xCoord + 1, yCoord - 1);
                    RevealEmptyOrAdjacent(xCoord + 1, yCoord);
                    RevealEmptyOrAdjacent(xCoord + 1, yCoord + 1);
                    RevealEmptyOrAdjacent(xCoord, yCoord + 1);
                }
                else if (rightEdge)
                {
                    RevealEmptyOrAdjacent(xCoord, yCoord - 1);
                    RevealEmptyOrAdjacent(xCoord, yCoord + 1);
                    RevealEmptyOrAdjacent(xCoord - 1, yCoord + 1);
                    RevealEmptyOrAdjacent(xCoord - 1, yCoord);
                    RevealEmptyOrAdjacent(xCoord - 1, yCoord - 1);
                }
                else
                {
                    RevealEmptyOrAdjacent(xCoord, yCoord - 1);
                    RevealEmptyOrAdjacent(xCoord + 1, yCoord - 1);
                    RevealEmptyOrAdjacent(xCoord + 1, yCoord);
                    RevealEmptyOrAdjacent(xCoord + 1, yCoord + 1);
                    RevealEmptyOrAdjacent(xCoord, yCoord + 1);
                    RevealEmptyOrAdjacent(xCoord - 1, yCoord + 1);
                    RevealEmptyOrAdjacent(xCoord - 1, yCoord);
                    RevealEmptyOrAdjacent(xCoord - 1, yCoord - 1);
                }


            }
        }

        private void RevealEmptyOrAdjacent(int xCheck, int yCheck)
        {
            if (nodes[xCheck, yCheck].state == NodeState.Empty && nodes[xCheck, yCheck].isCovered)
            {
                nodes[xCheck, yCheck].isCovered = false;
                ClearConsecutiveEmpties(xCheck, yCheck);
            }
            else if (nodes[xCheck, yCheck].state == NodeState.Adjacent && nodes[xCheck, yCheck].isCovered)
            {
                nodes[xCheck, yCheck].isCovered = false;
            }
        }

        public bool WinState()
        {
            int markedMines = 0;
            int emptiesCovered = 0;

            for (int yCoord = 0; yCoord < sideLength; yCoord++)
            {
                for (int xCoord = 0; xCoord < sideLength; xCoord++)
                {
                    if (nodes[xCoord, yCoord].isMarked && nodes[xCoord,yCoord].state == NodeState.Mine)
                        markedMines++;

                    if (nodes[xCoord, yCoord].isCovered && nodes[xCoord, yCoord].state != NodeState.Mine)
                        emptiesCovered++;
                }
            }

            if (markedMines == setMines || emptiesCovered == 0)
                return true;

            return false;
        }

        public bool LoseState()
        {
            for (int yCoord = 0; yCoord < sideLength; yCoord++)
            {
                for (int xCoord = 0; xCoord < sideLength; xCoord++)
                {
                    if (!nodes[xCoord, yCoord].isCovered && nodes[xCoord, yCoord].state == NodeState.Mine)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
