using FrameWork;

namespace Tetris {
    public class GameConfig : YamlConfig {
        public float FallInterval { get; set; } = 0.5f;
        public int Width { get; set; } = 10;
        public int Height { get; set; } = 20;
        public BoxLoc newBoxLoc = new BoxLoc {x = 5, y = 19};
    }

    public class BoxLoc {
        public int x { get; set; }
        public int y { get; set; }
    }
}