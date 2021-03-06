using FrameWork;

namespace Tetris {
    public enum MoveDir {
        Down,
        Left,
        Right
    }


    public class MoveBoxCmd : Command {
        private MoveDir moveDir { get; set; }

        public MoveBoxCmd(MoveDir moveDir) {
            this.moveDir = moveDir;
        }

        protected override void OnExecute() {
            GetNode<TetrisLogicOperation>().MoveBox(moveDir);
        }
    }
}