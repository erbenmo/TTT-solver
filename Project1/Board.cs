using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project1
{
	public class TTT
	{
		public int NumColumns { get; set; }
		public int NumRows { get; set; }
		public int pieceCount;
		public List<List<int>> Board { get; set; }

		public const int human = 0;
		public const int computer = 1;
		public const int empty = -1;

		public bool gameover = false;


		public int human_h = -1, human_w = -1;
		public int computer_h = -1, computer_w = -1;

		public TTT(int width, int height)
		{
			NumColumns = width;
			NumRows = height;

			pieceCount = NumColumns * NumRows;

			Board = new List<List<int>>();

			for (int i = 0; i < NumRows; i++)
			{
				List<int> row = new List<int>();
				for (int j = 0; j < NumColumns; j++)
				{
					row.Add(empty);
				}
				Board.Add(row);
			}
		}

		public void ReadhumanInput()
		{
			string[] tokens = Console.ReadLine().Split();
			human_h = int.Parse(tokens[0]);
			human_w = int.Parse(tokens[1]);
		}

		public void printTTT()
		{
			for (int h = 0; h < NumRows; h++)
			{
				for (int w = 0; w < NumColumns; w++)
				{
					if (Board[h][w] == human) Console.Write("X");
					else if (Board[h][w] == computer) Console.Write("O");
					else Console.Write("*");
					Console.Write(" ");
				}
				Console.WriteLine();
			}
			Console.WriteLine();
		}
		public void play()
		{
			printTTT();
			while(!gameover)
			{
				while (true)
				{
					ReadhumanInput();
					
					if(ValidateIndex(human_w, human_h) == false)
					{ //  loop again
					}
					else if (Board[human_h][human_w] == empty)
					{
						Board[human_h][human_w] = human;
						if (checkWin(human_w, human_h))
						{
							Console.WriteLine("You win!\n\n");
							gameover = true;
						}

							
						break;
					}
				}
				pieceCount--;

				if (!gameover && pieceCount == 0)
				{
					Console.WriteLine("Tie\n\n");
					gameover = true;
				}
				printTTT();

				AI();
				pieceCount--;

				if (checkWin(computer_w, computer_h))
				{
					Console.WriteLine("AI wins!\n\n");
					gameover = true;
				}

				if (!gameover && pieceCount == 0)
				{
					Console.WriteLine("Tie\n\n");
					gameover = true;
				}
				printTTT();
			}
		}



		public void AI()
		{
			if (Board[1][1] == empty)
			{
				computer_h = computer_w = 1;
				Board[1][1] = computer;
				return;
			}
			for (int h = 0; h < NumRows; h++)
			{
				for (int w = 0; w < NumColumns; w++)
				{
					if (Board[h][w] != empty)
						continue;

					// try place piece here and see if AI can win immediately
					Board[h][w] = computer;
					if (checkWin(w, h))
					{
						computer_w = w;
						computer_h = h;
						return;
					}
					Board[h][w] = empty;
				}
			}

			for (int h = 0; h < NumRows; h++)
			{
				for (int w = 0; w < NumColumns; w++)
				{
					if (Board[h][w] != empty)
						continue;

					// Check if human can win immediately if she places her piece here
					Board[h][w] = human;
					if (checkWin(w, h))
					{
						computer_w = w;
						computer_h = h;
						Board[h][w] = computer; 

						return;
					}
					Board[h][w] = empty;
				}
			}

			// place a piece randomly...
			Random r = new Random();
			while (true)
			{
				computer_w = r.Next(0, 2);
				computer_h = r.Next(0, 2);

				if (Board[computer_h][computer_w] == empty)
				{
					Board[computer_h][computer_w] = computer;
					return;
				}
			}
			
		}

		public bool checkWin(int w, int h)
		{
			int owner = Board[h][w];
			return checkUpDown(w, h, owner) || checkLeftRight(w, h, owner) ||
					checkLowerLeftUpperRight(w, h, owner) || checkUpperLeftLowerRight(w, h, owner);
		}

		private bool checkUpDown(int w, int h, int owner)
		{
			int i = w, j = 0;
			for (; j < NumRows; j++)
			{
				if (Board[j][i] != owner)
					return false;
			}
			return true;
		}

		private bool checkLeftRight(int w, int h, int owner)
		{
			int i = 0, j = h;
			for (; i < NumColumns; i++)
			{
				if (Board[j][i] != owner)
					return false;
			}
			return true;
		}

		private bool checkLowerLeftUpperRight(int w, int h, int owner)
		{
			int i = w, j = h;
			// move to the UpperRight as far as we can
			while (ValidateIndex(i + 1, j - 1) == true)
			{
				i = i + 1;
				j = j - 1;
			}

			int threshould = System.Math.Min(NumColumns, NumRows);
			int count = 0;
			// Check & Move Lower Left
			while (ValidateIndex(i, j))
			{
				if (Board[j][i] != owner)
					return false;
				i = i - 1;
				j = j + 1;
				count++;
			}

			return (count >= threshould);
		}

		private bool checkUpperLeftLowerRight(int w, int h, int owner)
		{
			int i = w, j = h;
			// move to the UpperLeft as far as we can
			while (ValidateIndex(i - 1, j - 1) == true)
			{
				i = i - 1;
				j = j - 1;
			}

			int threshould = System.Math.Min(NumColumns, NumRows);
			int count = 0;
			// Check & Move Lower Right
			while (ValidateIndex(i, j))
			{
				if (Board[j][i] != owner)
					return false;
				i = i + 1;
				j = j + 1;
				count++;
			}
			return (count >= threshould);
		}

		private bool ValidateIndex(int w, int h)
		{
			if (h >= NumRows || w >= NumColumns || h < 0 || w < 0)
				return false;
			return true;
		}
	}
}
