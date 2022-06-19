using FrameWork;

namespace Tetris {
    public class MoveBoxEvt : AbstractEvent {
        public int DeltaX, DeltaY;

        public MoveBoxEvt(int deltaX, int deltaY) {
            DeltaX = deltaX;
            DeltaY = deltaY;
        }
    }
}