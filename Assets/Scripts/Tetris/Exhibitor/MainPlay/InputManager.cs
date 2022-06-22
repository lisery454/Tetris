using FrameWork;
using UnityEngine;

namespace Tetris {
    public class InputManager : Exhibitor {
        private bool IsAble = true;

        protected override void Awake() {
            base.Awake();
            AddEventListener<GameEndEvt>(evt => { IsAble = false; });
        }

        private void Update() {
            if (!IsAble) return;
            
            if (Input.GetKeyDown(KeyCode.A)) {
                SendCommand(new MoveBoxCmd(MoveDir.Left));
            }
            else if (Input.GetKeyDown(KeyCode.D)) {
                SendCommand(new MoveBoxCmd(MoveDir.Right));
            }
            else if (Input.GetKeyDown(KeyCode.S)) {
                SendCommand(new MoveBoxCmd(MoveDir.Down));
            }
            else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) {
                SendCommand<RotBoxCmd>();
            }
        }
    }
}