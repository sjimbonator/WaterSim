﻿using System;

namespace WaterSim
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Creating Window");
            using Game game = new Game(1920, 1080, "LearnOpenTK");

            //Run takes a double, which is how many frames per second it should strive to reach.
            //You can leave that out and it'll just update as fast as the hardware will allow it.
            game.Run(60.0);

        }
    }
}
