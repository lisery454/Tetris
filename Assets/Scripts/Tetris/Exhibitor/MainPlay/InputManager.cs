using FrameWork;
using UnityEngine;

namespace Tetris {
    public class InputManager : Exhibitor {
        private bool IsAble = true;

        private KeyConfig keyConfig;

        protected override void Awake() {
            base.Awake();
            keyConfig = GetConfig<KeyConfig>();
            AddEventListener<GameEndEvt>(evt => { IsAble = false; });
        }

        private void Update() {
            if (!IsAble) return;

            if (Input.GetKeyDown(keyConfig.KeyCode_Left)) {
                SendCommand(new MoveBoxCmd(MoveDir.Left));
            }

            if (Input.GetKeyDown(keyConfig.KeyCode_Right)) {
                SendCommand(new MoveBoxCmd(MoveDir.Right));
            }

            if (Input.GetKeyDown(keyConfig.KeyCode_Down)) {
                SendCommand(new MoveBoxCmd(MoveDir.Down));
            }

            if (Input.GetKeyDown(keyConfig.KeyCode_Rot)) {
                SendCommand<RotBoxCmd>();
            }
        }
    }
}