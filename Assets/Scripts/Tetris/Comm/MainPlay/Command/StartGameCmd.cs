using FrameWork;

namespace Tetris {
    public class StartGameCmd : AbstractCommand {
        protected override void OnExecute() {
            GetOperation<TetrisLogicOperation>().StartGame();
        }
    }
}