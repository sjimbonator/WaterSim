using System;

namespace WaterSim
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Creating Window");
            using Game game = new Game(1920, 1080, "Terrain Generator");
            game.Run(60.0);
        }
    }
}
