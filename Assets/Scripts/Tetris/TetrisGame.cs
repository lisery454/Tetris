using FrameWork;


namespace Tetris {
    public class TetrisGame : Game<TetrisGame> {
        public Leader startUILeader { get; private set; }
        public Leader tetrisGameLeader { get; private set; }

        protected override void Awake() {
            base.Awake();

            ConfigRWer.WriteConfig(new GameConfig(), "Assets/Yaml/GameConfig.yaml");
            AddConfig(ConfigRWer.ReadConfig<GameConfig>("Assets/Yaml/GameConfig.yaml"));

            OnGotoScenes.Add("StartUI", OnGotoStartUIScene);
            OnGotoScenes.Add("MainPlay", OnGotoMainGameScene);

            InitStartUILeader();
            InitTetrisGameLeader();
        }

        private void OnGotoMainGameScene() {
            DeleteAllLeader();
            InitTetrisGameLeader();
        }

        private void OnGotoStartUIScene() {
            DeleteAllLeader();
            InitStartUILeader();
        }

        private void InitStartUILeader() {
            startUILeader = new Leader();
        }

        private void InitTetrisGameLeader() {
            tetrisGameLeader = new Leader();
            tetrisGameLeader.Register(new TetrisGameModel());
            tetrisGameLeader.Register(new TetrisLogicOperation());
        }

        private void DeleteAllLeader() {
            startUILeader = null;
            tetrisGameLeader = null;
        }
    }
}