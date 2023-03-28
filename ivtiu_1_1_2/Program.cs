using System;
class Position
{
    private int PosX;
    private int PosY;

    private int MaxPosX;
    private int MaxPosY;

    public string Mark;

    public Position()
    {
        PosX = 0;
        PosY = 0;
        Mark = string.Empty;
    }
    public Position(int x, int y, int maxX, int maxY, string mark)
    {
        PosX = x;
        PosY = y;
        MaxPosX = maxX;
        MaxPosY = maxY;
        Mark = mark;
    }

    public int getPosX() { return this.PosX; }

    public int getPosY() { return this.PosY; }

    public void UpX()
    {
        if (this.PosX + 1 <= MaxPosX)
            this.PosX++;
    }

    public void UpY()
    {
        if (this.PosY + 1 <= MaxPosY)
            this.PosY++;
    }

    public void DownX()
    {
        if (this.PosX - 1 >= 0)
            this.PosX--;
    }

    public void DownY()
    {
        if (this.PosY - 1 >= 0)
            this.PosY--;
    }
}

class Grid
{
    private int CoinsCount = 0;
    private int MovesCount = 0;

    private int PlayerPosX;
    private int PlayerPosY;
    private string PlayerMark;

    private int GridSize;
    public string GridMark;

    private string[,] MainArea;
    private bool[,] CoinArea;

    public Grid(int playerPosX, int playerPosY, string playerMark, int gridSize, string gridMark)
    {
        PlayerPosX = playerPosX;
        PlayerPosY = playerPosY;
        PlayerMark = playerMark;

        GridSize = gridSize;
        GridMark = gridMark;

        MainArea = new string[GridSize, GridSize];
        CoinArea = new bool[GridSize, GridSize];

        for (int i = 0; i < GridSize; i++)
            for (int j = 0; j < GridSize; j++)
                CoinArea[i, j] = false;
    }

    public void moveUp()
    { this.MovesCount++; }
    public void print()
    {
        for (int i = GridSize - 1; i >= 0; i--)
        {
            for (int j = 0; j < GridSize; j++)
            {
                if (CoinArea[i, j])
                {
                    MainArea[i, j] = "(C) ";
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else if (j == PlayerPosY && i == PlayerPosX)
                {
                    MainArea[i, j] = PlayerMark;
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    MainArea[i, j] = GridMark;
                    Console.ForegroundColor = ConsoleColor.Magenta;
                }

                Console.Write(MainArea[i, j], Console.ForegroundColor);
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine("\n");
        }
    }

    public void update(int playerPosX, int playerPosY)
    {
        PlayerPosX = playerPosX;
        PlayerPosY = playerPosY;
    }

    public void setRandomCoins()
    {
        Random rand = new Random();
        bool flag = false;

        for (int i = 0; i < GridSize; i++)
            for (int j = 0; j < GridSize; j++)
            {
                if (j != PlayerPosY || i != PlayerPosX)
                {
                    if ((CoinArea[i, j] = (rand.Next(1, 40) == 1)))
                        flag = true;
                }
            }

        if (!flag)
            setRandomCoins();
    }

    public void check()
    {
        if (CoinArea[PlayerPosX, PlayerPosY] == true)
        {
            CoinArea[PlayerPosX, PlayerPosY] = false;
            CoinsCount++;
            setRandomCoins();
        }
    }

    public void printCoins()
    { Console.WriteLine($"coins: {CoinsCount}"); }

    public void printMoves()
    { Console.WriteLine($"moves: {MovesCount}"); }
}
internal class Program
{
    static void Main()
    {
        Position Player = new Position(6, 6, 12, 12, "(P) ");
        Grid Area = new Grid(Player.getPosX(), Player.getPosY(), Player.Mark, 13, "[-] ");
        ConsoleKeyInfo Input;

        Area.setRandomCoins();

        do
        {
            Console.Clear();
            Area.print();
            Area.printCoins();
            Area.printMoves();
            Console.WriteLine("ESC - exit");

            do
            {
                Input = Console.ReadKey();
            }
            while (Input.Key != ConsoleKey.DownArrow
                  && Input.Key != ConsoleKey.UpArrow
                  && Input.Key != ConsoleKey.LeftArrow
                  && Input.Key != ConsoleKey.RightArrow
                  && Input.Key != ConsoleKey.Escape);

            switch (Input.Key)
            {
                case ConsoleKey.RightArrow:
                    Player.UpY();
                    break;

                case ConsoleKey.LeftArrow:
                    Player.DownY();
                    break;

                case ConsoleKey.UpArrow:
                    Player.UpX();
                    break;

                case ConsoleKey.DownArrow:
                    Player.DownX();
                    break;

                default: break;
            }
            Area.moveUp();
            Area.update(Player.getPosX(), Player.getPosY());
            Area.check();
        }
        while (Input.Key != ConsoleKey.Escape);
    }
}

