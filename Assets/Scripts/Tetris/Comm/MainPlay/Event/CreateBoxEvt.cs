using FrameWork;

namespace Tetris {
    public class CreateBoxEvt : AbstractEvent {
        public int LocX, LocY;

        public CreateBoxEvt(int locX, int locY) {
            LocX = locX;
            LocY = locY;
        }
    }
}