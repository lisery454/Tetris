namespace FrameWork {
    public interface IMatrix {
    }


    public class TupleMatrix : IMatrix {
        public TupleMatrix() { }

        //  0_0  0_1
        //  1_0  1_1
        public int A_0_0 { get; set; }
        public int A_0_1 { get; set; }
        public int A_1_0 { get; set; }
        public int A_1_1 { get; set; }

        //A * B
        public IntTuple Multiply(IntTuple B) {
            return new IntTuple(
                A_0_0 * B.X + A_0_1 * B.Y, 
                A_1_0 * B.X + A_1_1 * B.Y);
        }
        
        //A * B
        public FloatTuple Multiply(FloatTuple B) {
            return new FloatTuple(
                A_0_0 * B.X + A_0_1 * B.Y, 
                A_1_0 * B.X + A_1_1 * B.Y);
        }

        public TupleMatrix(int a00, int a01, int a10, int a11) {
            A_0_0 = a00;
            A_0_1 = a01;
            A_1_0 = a10;
            A_1_1 = a11;
        }
    }
}