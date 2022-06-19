using FrameWork;
using UnityEngine;

namespace Tetris {
    public class TetrisGameModel : AbstractModel {
        public override void Init() {
            var GameConfig = TetrisGame.Instance.GetConfig<GameConfig>();
            BoxInfos = new BoxInfo[GameConfig.Width, GameConfig.Height];
        }
        
        public BoxInfo[,] BoxInfos;
    }

    public struct BoxInfo {
        public Color Color;
        public bool IsBox;
    }
}