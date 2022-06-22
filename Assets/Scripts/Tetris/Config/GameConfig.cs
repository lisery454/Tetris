﻿using System.Collections.Generic;
using FrameWork;

namespace Tetris {
    public class GameConfig : YamlConfig {
        public float FallInterval { get; set; } = 0.5f;
        public int Width { get; set; } = 10;
        public int Height { get; set; } = 20;
        public IntTuple NewBoxLoc { get; set; } = new IntTuple(5, 18);
        public int LimitHeight { get; set; } = 12;
        public int LimitHeightMin { get; set; } = 3;
        public int LimitHeightMax { get; set; } = 16;

        public List<List<IntTuple>> BoxGroupPrefabs { get; set; } = new List<List<IntTuple>> {
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

        public List<FloatTuple> BoxGroupSRS { get; private set; } = new List<FloatTuple>() {
            new FloatTuple(0.5f, -0.5f),
            new FloatTuple(1f, 0f),
            new FloatTuple(-1f, 0f),
            new FloatTuple(0.5f, 0.5f),
            new FloatTuple(0f, 0f),
            new FloatTuple(0f, 0f),
            new FloatTuple(0f, 0f),
        };

        public TupleMatrix RotMatrix = new TupleMatrix(0, -1, 1, 0);
    }
}