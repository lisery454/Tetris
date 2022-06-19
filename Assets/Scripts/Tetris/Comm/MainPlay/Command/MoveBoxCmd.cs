using FrameWork;

namespace Tetris {
    public enum MoveDir {
        Down,
        Left,
        Right
    }


    public class MoveBoxCmd : AbstractCommand {
        private MoveDir moveDir { get; set; }

        public MoveBoxCmd(MoveDir moveDir) {
            this.moveDir = moveDir;
        }

        protected override void OnExecute() {
            GetOperation<TetrisLogicOperation>().MoveBox(moveDir);
        }
    }
}