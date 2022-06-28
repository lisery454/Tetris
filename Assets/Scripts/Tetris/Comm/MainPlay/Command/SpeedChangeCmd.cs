using FrameWork;

namespace Tetris {
    public class SpeedChangeCmd : Command {
        private bool isSpeedUp { get; }

        public SpeedChangeCmd(bool isSpeedUp) {
            this.isSpeedUp = isSpeedUp;
        }


        protected override void OnExecute() {
            GetNode<TetrisLogicOperation>().SpeedChange(isSpeedUp);
        }
    }
}