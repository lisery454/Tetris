using FrameWork;

namespace Tetris {
    public class SpeedChangeCmd : Command {
        private bool isSpeedUp { get; }

        public SpeedChangeCmd(bool isSpeedUp) {
            this.isSpeedUp = isSpeedUp;
        }


        protected override void OnExecute() {
            GetOperation<TetrisLogicOperation>().SpeedChange(isSpeedUp);
        }
    }
}