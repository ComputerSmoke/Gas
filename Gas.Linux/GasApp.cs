using Stride.Engine;

namespace Gas
{
    class GasApp
    {
        static void Main(string[] args)
        {
            using (var game = new Game())
            {
                game.Run();
            }
        }
    }
}
