using FrameWork;
using UnityEngine;

namespace Tetris {
    public class TetrisLogicRoot : AbstractRoot {
        public override void Init() { }

        //for debug
        public void PrintInput(MoveCommand moveCommand) {
            Debug.Log(moveCommand.ToString());
        }
    }
}