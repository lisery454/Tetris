using System;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Tetris {
    public class TetrisGameModel : AbstractModel {
        public override void Init() {
            var gameConfig = TetrisGame.Instance.GetConfig<GameConfig>();
            StaticBoxInfos = new StaticBoxInfo[gameConfig.Width, gameConfig.Height];
            for (var w = 0; w < gameConfig.Width; w++) {
                for (var h = 0; h < gameConfig.Height; h++) {
                    StaticBoxInfos[w, h] = new StaticBoxInfo();
                }
            }

            DynamicBoxInfos = null;
            DynamicBoxGroupRotCenter = null;
        }

        public StaticBoxInfo[,] StaticBoxInfos; //静止方块的信息
        public List<DynamicBoxInfo> DynamicBoxInfos; //运动方块的信息
        public FloatTuple DynamicBoxGroupRotCenter; //运动方块组的旋转中心
    }

    public class StaticBoxInfo {
        public Color Color { get; set; }
        public bool IsBox { get; set; }

        public StaticBoxInfo() {
            Color = Color.white;
            IsBox = false;
        }

        public StaticBoxInfo(Color color) {
            Color = color;
            IsBox = false;
        }
    }

    public class DynamicBoxInfo : ICloneable {
        public Color Color { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public DynamicBoxInfo(int x, int y) {
            X = x;
            Y = y;
            Color = Color.green;
        }

        public DynamicBoxInfo(Color color, int x, int y) {
            Color = color;
            X = x;
            Y = y;
        }

        public DynamicBoxInfo() { }

        public object Clone() {
            return new DynamicBoxInfo(Color, X, Y);
        }
    }
}