using System;

namespace FrameWork {
    public class IntTuple : ICloneable {
        public int X { get; set; }
        public int Y { get; set; }

        public IntTuple(int x, int y) {
            X = x;
            Y = y;
        }

        public IntTuple() { }

        public static explicit operator FloatTuple(IntTuple a) {
            return new FloatTuple(a.X, a.Y);
        }

        public object Clone() {
            return new IntTuple(X, Y);
        }
    }

    public class FloatTuple : ICloneable {
        public float X { get; set; }
        public float Y { get; set; }

        public FloatTuple(float x, float y) {
            X = x;
            Y = y;
        }

        public FloatTuple() { }

        public static FloatTuple operator -(FloatTuple f1, FloatTuple f2) {
            return new FloatTuple(f1.X - f2.X, f1.Y - f2.Y);
        }

        public static FloatTuple operator +(FloatTuple f1, FloatTuple f2) {
            return new FloatTuple(f1.X + f2.X, f1.Y + f2.Y);
        }

        public static explicit operator IntTuple(FloatTuple a) {
            return new IntTuple((int) Math.Round(a.X), (int) Math.Round(a.Y));
        }

        public object Clone() {
            return new FloatTuple(X, Y);
        }
    }
}