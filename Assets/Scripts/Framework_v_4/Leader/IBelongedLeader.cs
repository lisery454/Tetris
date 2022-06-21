namespace FrameWork {
    public interface IBelongedLeader : ICanGetConfig {
        ILeader BelongedLeader { get; set; }
    }
}