using UnityEngine;

namespace Tetris {
    public class InputManager : MainPlaySceneExhibitor {
        private void Update() {
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