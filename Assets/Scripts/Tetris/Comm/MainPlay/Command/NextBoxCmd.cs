using FrameWork;

namespace Tetris {
    public class NextBoxCmd : AbstractCommand {
        protected override void OnExecute() {
            GetOperation<TetrisLogicOperation>().CreateNewBoxOnTop();
        }
    }
}