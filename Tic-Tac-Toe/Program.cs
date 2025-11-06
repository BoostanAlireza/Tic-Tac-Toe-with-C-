using System;

namespace Tic_Tac_Toe
{
    public class Board
    {
        public int Size { get; }
        char[,] grid;
        public Board(int size)
        {
            Size = size; // Size of the board or Array
            grid = new char[Size, Size]; // 2D array to represent the board
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    grid[i, j] = '-';
        }

        public void Display() {
            for (int i = 0; i < Size; i++) {
                Console.Write("\n");
                for (int j = 0; j < Size; j++) {
                    Console.Write(grid[i, j] + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public bool IsEmpty(int row, int column)
        {
            return grid[row, column] == '-';
        }

        public bool PlaceMark(int row, int column, char mark)
        {
            if (IsEmpty(row, column))
            {
                grid[row, column] = mark;
                return true;
            }
            return false;
        }

        public bool CheckWin(char mark)
        {
            // Rows
            for (int i = 0; i < Size; i++)
            {
                bool rowWin = true;
                for (int j = 0; j < Size; j++)
                {
                    if (grid[i, j] != mark)
                    {
                        rowWin = false;
                        break;
                    }
                }
                if (rowWin) return true;
            }
            // Columns
            for (int j = 0; j < Size; j++)
            {
                bool columnWin = true;
                for (int i = 0; i < Size; i ++)
                {
                    if (grid[i, j] != mark)
                    {
                        columnWin = false;
                        break;
                    }
                }
                if (columnWin) return true;
            }
            // Main Diagonal
            bool mainDiagonalWin = true;
            for (int i = 0; i < Size; i++)
            {
                if (grid[i, i] != mark)
                {
                    mainDiagonalWin = false;
                    break;
                }
            }
            if (mainDiagonalWin) return true;

            bool antiDiagonalWin = true;
            for (int i = 0; i < Size; i++)
            {
                if (grid[i, Size - i - 1] != mark)
                {
                    antiDiagonalWin = false;
                    break;
                }
            }
            if (antiDiagonalWin) return true;

            // No win
            return false;
        }

        // Check whether the board is full and the game is a draw
        public bool IsFull()
        {
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    if (grid[i, j] == '-')
                        return false;
            return true;
        }
    }

    public class Player
    {
        public char Mark { get; set; }
        public bool IsComputer { get; set; }
        private Random random = new Random();

        public Player(char mark, bool isComputer)
        {
            Mark = mark;
            IsComputer = isComputer;
        }

        public (int row, int column) GetMove(Board board)
        {
            int row, column;

            if(!IsComputer)
            {
                while (true)
                {
                    Console.WriteLine("Enter your move(row, column): ");
                    string[] input = Console.ReadLine().Split(' ');

                    if (input.Length != 2)
                    {
                        Console.WriteLine("Invalid input. Please enter two numbers separated by a space.");
                        continue;
                    }
                    
                    if (!int.TryParse(input[0], out row) || !int.TryParse(input[1], out column))
                    {
                        Console.WriteLine("Invalid input. Try again.");
                        continue;
                    }

                    if (row < 0 || row >= board.Size || column < 0 || column >= board.Size)
                    {
                        Console.WriteLine("Your move is out of range. Try again.");
                        continue;
                    }

                    if (!board.IsEmpty(row, column))
                    {
                        Console.WriteLine("Cell is already occupied. Try again.");
                        continue;
                    }

                    break;
                }
            }
            else
            {
                do
                {
                    row = random.Next(0, board.Size);
                    column = random.Next(0, board.Size);
                } while (!board.IsEmpty(row, column));
            }
            return (row, column);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter board size: ");
            int size = Convert.ToInt32(Console.ReadLine());
            Board board = new Board(size);

            Console.WriteLine("Do you want to play first? (y/n)");
            string answer = Console.ReadLine().Trim().ToLower();

            Player human, computer, currentPlayer;

            if (answer == "y")
            {
                 human = new Player('X', false);
                 computer = new Player('O', true);
                 currentPlayer = human;
            }

            else
            {
                 human = new Player('O', false);
                 computer = new Player('X', true);
                 currentPlayer = computer;
            }

            while (true)
            {

                var (row, column) = currentPlayer.GetMove(board);

                bool validMove = board.PlaceMark(row, column, currentPlayer.Mark);

                if (validMove)
                {
                    board.Display();

                    // Switch player
                    currentPlayer = (currentPlayer == human) ? computer : human;
                }

                if (board.CheckWin(human.Mark))
                {
                    Console.WriteLine($"Player {currentPlayer.Mark} Wins.");
                    break;
                }

                if (board.IsFull())
                {
                    Console.WriteLine("It's a draw!");
                    break;
                }

            }
            Console.WriteLine("Game Over. Press any Key to exit.");
            Console.ReadKey();

        }
    }
}
