using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static int screenWidth = 32;
    static int screenHeight = 16;
    static int snakeSpeed = 100;
    static Random random = new Random();

    static Pixel head;
    static List<Pixel> snake = new List<Pixel>();
    static int score = 0;

    static Pixel obstacle;

    static void Main()
    {
        InitializeGame();
        while (true)
        {
            if (Console.KeyAvailable)
            {
                ProcessInput(Console.ReadKey(true).Key);
            }
            MoveSnake();
            CheckCollision();
            DrawFrame();
            Thread.Sleep(snakeSpeed);
        }
    }

    static void InitializeGame()
    {
        Console.WindowHeight = screenHeight;
        Console.WindowWidth = screenWidth;
        Console.CursorVisible = false;

        head = new Pixel(screenWidth / 2, screenHeight / 2, ConsoleColor.Red);
        snake.Add(head);

        PlaceObstacle();
    }

    static void ProcessInput(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.UpArrow:
                if (head.Direction != Direction.Down)
                    head.Direction = Direction.Up;
                break;
            case ConsoleKey.DownArrow:
                if (head.Direction != Direction.Up)
                    head.Direction = Direction.Down;
                break;
            case ConsoleKey.LeftArrow:
                if (head.Direction != Direction.Right)
                    head.Direction = Direction.Left;
                break;
            case ConsoleKey.RightArrow:
                if (head.Direction != Direction.Left)
                    head.Direction = Direction.Right;
                break;
        }
    }

    static void MoveSnake()
    {
        for (int i = snake.Count - 1; i >= 1; i--)
        {
            snake[i].X = snake[i - 1].X;
            snake[i].Y = snake[i - 1].Y;
        }

        switch (head.Direction)
        {
            case Direction.Up:
                head.Y--;
                break;
            case Direction.Down:
                head.Y++;
                break;
            case Direction.Left:
                head.X--;
                break;
            case Direction.Right:
                head.X++;
                break;
        }

        // Wrap around boundaries
        if (head.X < 0) head.X = screenWidth - 1;
        if (head.X >= screenWidth) head.X = 0;
        if (head.Y < 0) head.Y = screenHeight - 1;
        if (head.Y >= screenHeight) head.Y = 0;
    }

    static void CheckCollision()
    {
        if (head.X == obstacle.X && head.Y == obstacle.Y)
        {
            score++;
            PlaceObstacle();
        }
        // Check collision with self
        for (int i = 1; i < snake.Count; i++)
        {
            if (head.X == snake[i].X && head.Y == snake[i].Y)
            {
                EndGame();
            }
        }
    }

    static void DrawFrame()
    {
        Console.Clear();
        DrawSnake();
        DrawObstacle();
        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Score: " + score);
    }

    static void DrawSnake()
    {
        foreach (var pixel in snake)
        {
            Console.SetCursorPosition(pixel.X, pixel.Y);
            Console.ForegroundColor = pixel.Color;
            Console.Write("■");
        }
    }

    static void DrawObstacle()
    {
        Console.SetCursorPosition(obstacle.X, obstacle.Y);
        Console.ForegroundColor = obstacle.Color;
        Console.Write("*");
    }

    static void PlaceObstacle()
    {
        int x, y;
        do
        {
            x = random.Next(0, screenWidth);
            y = random.Next(0, screenHeight);
        } while (snake.Exists(p => p.X == x && p.Y == y));

        obstacle = new Pixel(x, y, ConsoleColor.Cyan);
    }

    static void EndGame()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
        Console.WriteLine("Game Over");
        Console.SetCursorPosition(screenWidth / 5, screenHeight / 2 + 1);
        Console.WriteLine("Your Score: " + score);
        Environment.Exit(0);
    }
}

enum Direction { Up, Down, Left, Right }

class Pixel
{
    public int X { get; set; }
    public int Y { get; set; }
    public ConsoleColor Color { get; set; }
    public Direction Direction { get; set; }

    public Pixel(int x, int y, ConsoleColor color)
    {
        X = x;
        Y = y;
        Color = color;
    }
}
