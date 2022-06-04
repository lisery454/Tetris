using UnityEngine;

namespace Tetris {
    public class InputManager : MainPlaySceneLeaf {
        private void Update() {
            if (Input.GetKeyDown(KeyCode.A)) {
                SendNerveWave(new MoveBoxNW(MoveCommand.Left));
            }
            else if (Input.GetKeyDown(KeyCode.D)) {
                SendNerveWave(new MoveBoxNW(MoveCommand.Right));
            }
            else if (Input.GetKeyDown(KeyCode.S)) {
                SendNerveWave(new MoveBoxNW(MoveCommand.Down));
            }
        }
    }
}