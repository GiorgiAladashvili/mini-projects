using System.Data;

namespace კუნძულები
{
	internal class Program
	{
		static void Main(string[] args)
		{
			int[,] sea = new int[Random.Shared.Next(10, 20), Random.Shared.Next(10, 20)];
			GenerateIslands(sea);
			PrintSea(sea);
			Console.WriteLine($"\nWe have {GetIslandsCount(sea)} islands");
			
        }

		static bool isSafe(int[,] sea, int row, int col,
					   bool[,] visited)
		{
			int ROW = GetRow(sea);
			int COL = GetCol(sea);
			return (row >= 0) && (row < ROW) && (col >= 0)
				&& (col < COL)
				&& (sea[row, col] == 1 && !visited[row, col]);
		}

		static void DFS(int[,] sea, int row, int col,
					bool[,] visited)
		{
			int[] rowNbr
				= new int[] { -1, 0, 0, 1};
			int[] colNbr
				= new int[] { 0, -1, 1,  0};

			visited[row, col] = true;

			for (int k = 0; k < 4; ++k)
				if (isSafe(sea, row + rowNbr[k], col + colNbr[k],
						   visited))
					DFS(sea, row + rowNbr[k], col + colNbr[k],
						visited);
		}
		static int GetIslandsCount(int[,] sea)
		{
			int ROW = GetRow(sea);
			int COL = GetCol(sea);
			bool[,] visited = new bool[ROW, COL];

			int count = 0;
			for (int i = 0; i < ROW; ++i)
				for (int j = 0; j < COL; ++j)
					if (sea[i, j] == 1 && !visited[i, j])
					{
						DFS(sea, i, j, visited);
						++count;
					}
			return count;
		}


		static int GetRow(int[,] sea)
		{
			int row = sea.GetLength(0);
			return row;
		}

		static int GetCol(int[,] sea)
		{
			int col = sea.GetLength(1);
			return col;
		}

		static void GenerateIslands(int[,] sea)
		{
			for (int i = 0; i < sea.GetLength(0); i++)
			{
				for (int j = 0; j < sea.GetLength(1); j++)
				{
					sea[i, j] = Random.Shared.Next(10) > 7 ? 1 : 0;
				}
			}
		}

		static void PrintSea(int[,] sea)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			for (int i = 0; i < sea.GetLength(0); i++)
			{
				for (int j = 0; j < sea.GetLength(1); j++)
				{
					if (sea[i, j] == 0)
					{
						Console.BackgroundColor = ConsoleColor.Blue;
					}
					else
					{
						Console.BackgroundColor = ConsoleColor.Green;
					}

					Console.Write($" {sea[i, j]} ");
				}

				Console.WriteLine();
			}
			Console.ResetColor();
		}
	}
}