using FrameWork;

namespace Tetris {
    public class RotBoxCmd : Command{
        protected override void OnExecute() {
            GetOperation<TetrisLogicOperation>().Rotate();
        }
    }
}