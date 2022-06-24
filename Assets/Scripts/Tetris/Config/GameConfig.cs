using System.Collections.Generic;
using FrameWork;

namespace Tetris {
    public class GameConfig : YamlConfig {
        public float FallInterval { get; set; }
        public float FallSpeedMin { get; set; } //每秒落几格
        public float FallSpeedMax { get; set; } //每秒落几格
        public int Width { get; set; }
        public int Height { get; set; }
        public IntTuple NewBoxLoc { get; set; }
        public int LimitHeight { get; set; }
        public int LimitHeightMin { get; set; }
        public int LimitHeightMax { get; set; }

        public List<List<IntTuple>> BoxGroupPrefabs { get; set; }

        public List<FloatTuple> BoxGroupSRS { get; private set; }

        public TupleMatrix RotMatrix { get; set; }

        public float SpeedUpFactor { get; set; }
        public float SpeedUpFactorMin { get; set; }
        public float SpeedUpFactorMax { get; set; }

        public GameConfig() {
            FallInterval = 0.5f;
            FallSpeedMin = 1f; //每秒落几格
            FallSpeedMax = 10f; //每秒落几格
            Width = 10;
            Height = 20;
            NewBoxLoc = new IntTuple(5, 18);
            LimitHeight = 12;
            LimitHeightMin = 3;
            LimitHeightMax = 16;

            BoxGroupPrefabs = new List<List<IntTuple>> {
                new List<IntTuple> {
                    new IntTuple(-1, 0), new IntTuple(0, 0), new IntTuple(1, 0), new IntTuple(2, 0)
                },
                new List<IntTuple> {
                    new IntTuple(0, 1), new IntTuple(0, 0), new IntTuple(1, 0), new IntTuple(2, 0)
                },
                new List<IntTuple> {
                    new IntTuple(0, 1), new IntTuple(0, 0), new IntTuple(-1, 0), new IntTuple(-2, 0)
                },
                new List<IntTuple> {
                    new IntTuple(0, 1), new IntTuple(0, 0), new IntTuple(1, 0), new IntTuple(1, 1)
                },
                new List<IntTuple> {
                    new IntTuple(-1, 1), new IntTuple(0, 1), new IntTuple(0, 0), new IntTuple(1, 0)
                },
                new List<IntTuple> {
                    new IntTuple(-1, 0), new IntTuple(0, 0), new IntTuple(0, 1), new IntTuple(1, 1)
                },
                new List<IntTuple> {
                    new IntTuple(-1, 0), new IntTuple(0, 0), new IntTuple(0, 1), new IntTuple(1, 0)
                }
            };

            BoxGroupSRS = new List<FloatTuple>() {
                new FloatTuple(0.5f, -0.5f),
                new FloatTuple(1f, 0f),
                new FloatTuple(-1f, 0f),
                new FloatTuple(0.5f, 0.5f),
                new FloatTuple(0f, 0f),
                new FloatTuple(0f, 0f),
                new FloatTuple(0f, 0f),
            };

            RotMatrix = new TupleMatrix(0, -1, 1, 0);

            SpeedUpFactor = 3f;
            SpeedUpFactorMin = 2f;
            SpeedUpFactorMax = 8f;
        }
    }
}