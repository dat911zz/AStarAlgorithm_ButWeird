using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AStarAlgorithm
{
    class AStartAlgorithm_Eng
    {
        public void Run()
        {
            int TotalCost = 0;
            List<Tile> ResultList = new List<Tile>();
            //List<string> map = new List<string>
            //{
            //    "|--------------|",
            //    "|A             |",
            //    "|--| |---------|",
            //    "|              |",
            //    "|   |--| |---| |",
            //    "|   |     |    |",
            //    "|---|     |    |",
            //    "|              |",
            //    "|--| |------   |",
            //    "|              |",
            //    "|   |-----|----|",
            //    "|         |    |",
            //    "|---|       |B |",
            //    "|--------------|"
            //};
            //List<string> map = new List<string>
            //{
            //    "A - ",
            //    "- --",
            //    "   -",
            //    "    ",
            //    " -- ",
            //    "    ",
            //    "    ",
            //    "    ",


            //};
            List<string> map = new List<string>
            {   "|--------------------------------------|",
                "|A   |           |  |B     |  |        |",
                "|--| |-------| | |  |  |        |-| |  |",
                "|  |         | | |  |--|--|--|  |   |  |",
                "|    ||      | | |  |     |  |  | |-|  |",
                "|-----| |----| | |     |     |--|    | |",
                "|       |      | |  |--------| |---| |-|",
                "| |--|  | |----| |  |    |   |-|       |",
                "|    |--|      | |  |  |   |   |-| | | |",
                "|--| |-------| | |     | |---|-|   |-| |",
                "|  |         | | |  |----|   | | |     |",
                "|    |------ | | |  |    | | |   | | | |",
                "|-----|   || | | |     | | | | | |-| | |",
                "|       |    | | |  |--|   |   | | | | |",
                "|  | |-------| | |    |--------| | | | |",
                "|  |   |   |   | |  | |   | |  | |     |",
                "|  | |   |   | | |      |   |  | |--|  |",
                "|---------|-|--| |  |-----| |  |    |  |",
                "|                |        |      |  |  |",
                "|---|  |-|---| |-|  |---| |----| |-----|",
                "|                   |                  |",
                "|--------------------------------------|"
            };
            var start = new Tile();
            start.Y = map.FindIndex(x => x.Contains("A"));
            start.X = map[start.Y].IndexOf("A");


            var finish = new Tile();
            finish.Y = map.FindIndex(x => x.Contains("B"));
            finish.X = map[finish.Y].IndexOf("B");

            start.SetDistance(finish.X, finish.Y);

            var activeTiles = new List<Tile>();
            activeTiles.Add(start);
            var visitedTiles = new List<Tile>();

            while (activeTiles.Any())
            {
                var checkTile = activeTiles.OrderBy(x => x.CostDistance).First();

                //Start drawing map when we found the destination
                if (checkTile.X == finish.X && checkTile.Y == finish.Y)
                {
                    //We found the destination and we can be sure (Because the the OrderBy above)
                    //That it's the most low cost option. 
                    var tile = checkTile;
                    //Console.WriteLine("Retracing steps backwards...");
                    Console.WriteLine("Thực hiện quay lui...");
                    Console.WriteLine("Tọa độ truy ngược về điểm bắt đầu ([X : Y]):");
                    while (true)
                    {
                        Console.WriteLine($"{tile.X} : {tile.Y}");
                        if (tile.X == finish.X && tile.Y == finish.Y)
                        {
                            TotalCost = tile.CostDistance;
                        }
                        if (map[tile.Y][tile.X] == ' ')
                        {
                            var newMapRow = map[tile.Y].ToCharArray();
                            newMapRow[tile.X] = '*';
                            map[tile.Y] = new string(newMapRow);
                        }
                        ResultList.Add(new Tile() { X = tile.X, Y = tile.Y, Cost = tile.Cost, Distance = tile.Distance});
                        tile = tile.Parent;
                        if (tile == null)
                        {
                            //Console.WriteLine("Map looks like :");
                            Console.WriteLine("\n\nTổng chi phí từ A -> B: {0}", TotalCost);
                            Console.WriteLine("Bản đồ sau khi chạy thuật toán A*:");
                            map.ForEach(x =>
                            {
                                var charArr = x.ToCharArray();
                                foreach (var ch in charArr)
                                {
                                    if (ch == '*')
                                    {
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.BackgroundColor = ConsoleColor.Green;
                                        Console.Write(ch);
                                        Console.ResetColor();
                                    }
                                    else
                                    {
                                        Console.Write(ch);
                                    }
                                }
                                Console.WriteLine();
                            });
                            Console.WriteLine("Done!");
                            ResultList.Reverse();
                            List<Tile> listFollowX = new List<Tile>();
                            List<Tile> listFollowY = new List<Tile>();
                            //ResultList.ForEach(x =>
                            //{
                            //    if (ResultList.IndexOf(x) == 0)
                            //    {
                            //        //Console.WriteLine("[{0}:{1}] {2}", x.X, x.Y, ResultList.IndexOf(x));
                            //    }
                            //    else
                            //    {
                            //        if (ResultList[ResultList.IndexOf(x)].X != ResultList[ResultList.IndexOf(x)-1].X)
                            //        {
                            //            //listFollowX.Add(x);
                            //            Console.WriteLine("[{0}:{1}]", x.X, x.Y);
                            //        }
                            //    }
                            //});
                            HeuristicCalculatorForAllConers(map, start, finish);
                            return;
                        }
                 
                    }
                }

                visitedTiles.Add(checkTile);
                activeTiles.Remove(checkTile);

                var walkableTiles = GetWalkableTiles(map, checkTile, finish);

                foreach (var walkableTile in walkableTiles)
                {
                    //We have already visited this tile so we don't need to do so again!
                    if (visitedTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                        continue;

                    //It's already in the active list, but that's OK, maybe this new tile has a better value (e.g. We might zigzag earlier but this is now straighter). 
                    if (activeTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                    {
                        var existingTile = activeTiles.First(x => x.X == walkableTile.X && x.Y == walkableTile.Y);
                        if (existingTile.CostDistance > checkTile.CostDistance)
                        {
                            activeTiles.Remove(existingTile);
                            activeTiles.Add(walkableTile);
                        }
                    }
                    else
                    {
                        //We've never seen this tile before so add it to the list. 
                        activeTiles.Add(walkableTile);
                    }
                }
            }

            //Console.WriteLine("No Path Found!");
            Console.WriteLine("Không tìm thấy đường!");
        }
        private List<Tile> GetWalkableTiles(List<string> map, Tile currentTile, Tile targetTile)
        {
            var possibleTiles = new List<Tile>()
            {
                new Tile { X = currentTile.X, Y = currentTile.Y - 1, Parent = currentTile, Cost = currentTile.Cost + 1 },
                new Tile { X = currentTile.X, Y = currentTile.Y + 1, Parent = currentTile, Cost = currentTile.Cost + 1},
                new Tile { X = currentTile.X - 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
                new Tile { X = currentTile.X + 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
            };

            possibleTiles.ForEach(tile => tile.SetDistance(targetTile.X, targetTile.Y));

            var maxX = map.First().Length - 1;
            var maxY = map.Count - 1;

            return possibleTiles
                    .Where(tile => tile.X >= 0 && tile.X <= maxX)
                    .Where(tile => tile.Y >= 0 && tile.Y <= maxY)
                    .Where(tile => map[tile.Y][tile.X] == ' ' || map[tile.Y][tile.X] == 'B')
                    .ToList();
        }
        private void HeuristicCalculatorForAllConers(List<string> map, Tile start, Tile finish)
        {
            List<Tile> listCorner = new List<Tile>();
            Array2D arr2D = new Array2D(map);
            arr2D.Show2DArray();
            //Find corner          
            listCorner.Add(new Tile() { X = start.X, Y = start.Y });
            listCorner.AddRange(new List<Tile>(FindCorners(arr2D)));
            listCorner.Add(new Tile() { X = finish.X, Y = finish.Y });
            //Show list of corner
            Console.Write("\nTính Heuristic: ");
            listCorner.ForEach(x =>
            {
                x.SetDistance(finish.X, finish.Y);
                Console.Write("\n[{0}:{1}] = {2}", x.X, x.Y, x.Distance);
            });
        }
        private List<Tile> FindCorners(Array2D a)
        {
            List<Tile> tmpList = new List<Tile>();
            int countPath = 0;
            #region cmt
            for (int i = 0; i < a.m; i++)
            {
                for (int j = 0; j < a.n; j++)
                {
                    //Check các điểm trong 
                    if (a.arr[i, j] == 1)
                    {
                        if (j - 1 > 0 && j + 1 < a.n - 1)
                        {
                            // 1
                            if (a.arr[i, j - 1] == 1 && a.arr[i, j + 1] != 1)
                            {
                                if (a.arr[i + 1, j] == 1 || a.arr[i - 1, j] == 1)
                                {
                                    countPath++;
                                }
                            }
                            // 2
                            if (a.arr[i - 1, j] == 1 && a.arr[i + 1, j] != 1)
                            {
                                if (a.arr[i, j + 1] == 1 || a.arr[i, j - 1] == 1)
                                {
                                    countPath++;
                                }
                            }
                            // 3
                            if (a.arr[i, j - 1] != 1 && a.arr[i, j + 1] == 1)
                            {
                                if (a.arr[i + 1, j] == 1 || a.arr[i - 1, j] == 1)
                                {
                                    countPath++;
                                }
                            }
                            // 4
                            if (a.arr[i - 1, j] != 1 && a.arr[i + 1, j] == 1)
                            {
                                if (a.arr[i, j + 1] == 1 || a.arr[i, j - 1] == 1)
                                {
                                    countPath++;
                                }
                            }
                        }
                    }
                    if (countPath == 2)
                    {
                        //Console.WriteLine(" {0}:{1}", i, j);
                        ////a.arr[i, j]++;
                        tmpList.Add(new Tile() { X = i, Y = j });
                    }
                    countPath = 0;
                }
            }
            tmpList.ForEach(x => {
                Console.Write($" [{x.X}:{x.Y}]");
            });
            tmpList.ForEach(x => {
                a.arr[x.X, x.Y]++;
            });
            Console.WriteLine();
            //a.Show2DArray();
            #endregion
            return tmpList;
        }


    }
    class Tile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Cost { get; set; }
        public int Distance { get; set; }
        public int CostDistance => Cost + Distance;
        public Tile Parent { get; set; }
        
        //The distance is essentially the estimated distance, ignoring walls to our target. 
        //So how many tiles left and right, up and down, ignoring walls, to get there. 
        public void SetDistance(int targetX, int targetY)//Manhattan Distance
        {
            this.Distance = Math.Abs(targetX - X) + Math.Abs(targetY - Y);
        }
    }
    class Array2D
    {
        public int m, n;
        public int[,] arr;
        public Array2D()
        {

        }
        public Array2D(List<string> map)
        {
            m = map.Count;
            n = map[0].Length;
            arr = new int[m, n];
            for (int i = 0; i < map.Count; i++)
            {
                var tmp = map[i].ToCharArray();
                for (int j = 0; j < map[0].Length; j++)
                {
                    if (tmp[j].Equals('*') || tmp[j].Equals('A') || tmp[j].Equals('B'))
                    {
                        arr[i, j] = 1;
                    }
                    else
                    {
                        if (tmp[j].Equals('|') || tmp[j].Equals('-'))
                        {
                            arr[i, j] = -1;
                        }
                        else
                        {
                            arr[i, j] = 0;
                        }
                    }
                }
            }
        }
        public void Show2DArray()
        {
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (arr[i, j] == 1)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                    }
                    if (arr[i, j] == -1)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                    }
                    Console.Write(" {0,2}", arr[i, j]);
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }
    }
}
