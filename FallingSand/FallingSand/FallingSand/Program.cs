using System;

namespace FallingSand
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (FSGGame game = new FSGGame())
            {
                game.Run();
            }
        }
    }
#endif
}

