using FrameWork;

namespace Tetris {
    public class RotBoxCmd : AbstractCommand{
        protected override void OnExecute() {
            GetOperation<TetrisLogicOperation>().Rotate();
        }
    }
}