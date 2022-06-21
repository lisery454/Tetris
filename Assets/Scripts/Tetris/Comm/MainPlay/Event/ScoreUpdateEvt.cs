using FrameWork;

namespace Tetris {
    public class ScoreUpdateEvt : AbstractEvent {
        public int Score { get; set; }

        public ScoreUpdateEvt(int score) {
            Score = score;
        }
    }
}