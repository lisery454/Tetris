namespace FrameWork {
    public interface ISoil : INode { }

    public abstract class AbstractSoil : ISoil {
        public IStem BelongedStem { get; set; }
        public abstract void Init();
    }

    public interface ICanGetSoil {
        T GetSoil<T>() where T : class, ISoil;
    }
}