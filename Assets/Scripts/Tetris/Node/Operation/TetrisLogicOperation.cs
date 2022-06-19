using System;
using FrameWork;

namespace Tetris {
    public class TetrisLogicOperation : AbstractOperation {
        private TetrisGameModel _model;

        public override void Init() {
            _model = GetModel<TetrisGameModel>();
        }

        public void MoveBox(MoveDir moveDir) {
            switch (moveDir) {
                case MoveDir.Down:
                    TriggerEvent(new MoveBoxEvt(0, -1));
                    break;
                case MoveDir.Left:
                    TriggerEvent(new MoveBoxEvt(-1, 0));
                    break;
                case MoveDir.Right:
                    TriggerEvent(new MoveBoxEvt(1, 0));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(moveDir), moveDir, null);
            }
        }

        public void CreateNewBoxOnTop() {
            //_model.BoxLoc[_model.newBoxLoc.Item1, _model.newBoxLoc.Item2] = true;
            //TriggerEvent(new CreateBoxEvt(_model.newBoxLoc.Item1, _model.newBoxLoc.Item2));
        }
    }
}