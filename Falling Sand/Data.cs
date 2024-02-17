
using System.Windows.Shapes;

namespace Falling_Sand
{
    public static class Data
    {

        public static double canvasSize = 400;

        public static int matrixSize = 100;

        public static int sandSize = (int)canvasSize / matrixSize;

        public static Rectangle[,] matrix = new Rectangle[matrixSize, matrixSize];
    }
}
