namespace FrameWork {
    public class LeaderFactory {
        private readonly IGame belongedGame;

        public LeaderFactory(IGame belongedGame) {
            this.belongedGame = belongedGame;
        }

        public Leader CreateLeader() {
            return new Leader(belongedGame);
        }
    }
}