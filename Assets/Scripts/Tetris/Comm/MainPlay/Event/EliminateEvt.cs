using FrameWork;

namespace Tetris {
    public class EliminateEvt : Event {
        public int level;

        public EliminateEvt(int level) {
            this.level = level;
        }
    }
}