using System;
using FrameWork;

namespace Tetris {
    public class TetrisGameModel : AbstractModel {
        public override void Init() { }

        private static int Width = 10;
        private static int Height = 20;
        public Tuple<int, int> newBoxLoc = new Tuple<int, int>(5, 19);
        public bool[,] BoxLoc = new bool[Width, Height];
    }
}