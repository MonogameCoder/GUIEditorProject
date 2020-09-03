using _GUIProject;
using System;

namespace _GUIProject
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new MainWindow())
                game.Run();
        }
    }
}
