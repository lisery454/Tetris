using FrameWork;

namespace Tetris {
    public class ScoreModel : Model {
        public override void Init() {
            Score = 0;
        }

        public int Score { get; set; }
    }
}