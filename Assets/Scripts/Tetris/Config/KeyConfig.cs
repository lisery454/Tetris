using FrameWork;
using UnityEngine;

namespace Tetris {
    public class KeyConfig : YamlConfig {
        public KeyCode KeyCode_Left { get; set; } = KeyCode.A;
        public KeyCode KeyCode_Right { get; set; } = KeyCode.D;
        public KeyCode KeyCode_Down { get; set; } = KeyCode.Space;
        public KeyCode KeyCode_Rot { get; set; } = KeyCode.W;
        public KeyCode KeyCode_SpeedUp { get; set; } = KeyCode.S;
    }
}