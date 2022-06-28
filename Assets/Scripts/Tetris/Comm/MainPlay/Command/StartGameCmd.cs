using FrameWork;

namespace Tetris {
    public class StartGameCmd : Command {
        protected override void OnExecute() {
            GetNode<TetrisLogicOperation>().StartGame();
        }
    }
}