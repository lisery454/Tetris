using FrameWork;
using UnityEngine.SceneManagement;


namespace Tetris {
    public class TetrisGame : Game<TetrisGame> {
        public Leader startUILeader { get; private set; }
        public Leader tetrisGameLeader { get; private set; }

        protected override void Awake() {
            base.Awake();
            NewStartUILeader();
            NewTetrisGameLeader();
        }


        public void GotoMainGameScene() {
            NewTetrisGameLeader();
            SceneManager.LoadScene("MainPlay");
        }

        public void GotoStartUIScene() {
            NewStartUILeader();
            SceneManager.LoadScene("StartUI");
        }

        public void ExitGame() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }


        private void NewStartUILeader() {
            startUILeader = new Leader();
        }

        private void NewTetrisGameLeader() {
            tetrisGameLeader = new Leader();
            tetrisGameLeader.Register(new TetrisGameModel());
            tetrisGameLeader.Register(new TetrisLogicOperation());
        }
    }
}