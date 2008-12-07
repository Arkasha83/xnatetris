using System;

namespace XNATetris
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            TetrisGame.getSingleton().Run();
        }
    }
}

