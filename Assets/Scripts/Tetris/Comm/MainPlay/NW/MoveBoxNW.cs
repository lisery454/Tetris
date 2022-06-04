using FrameWork;

namespace Tetris {
    public enum MoveCommand {
        Down,
        Left,
        Right
    }


    public class MoveBoxNW : AbstractNerveWave {
        private MoveCommand MoveCommand { get; set; }

        public MoveBoxNW(MoveCommand moveCommand) {
            MoveCommand = moveCommand;
        }

        protected override void OnExecute() {
            GetRoot<TetrisLogicRoot>().PrintInput(MoveCommand);
        }
    }
}