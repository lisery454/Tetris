using FrameWork;

namespace Tetris {
    public class ScoreUpdateEvt : Event {
        public int Score { get; set; }

        public ScoreUpdateEvt(int score) {
            Score = score;
        }
    }
}