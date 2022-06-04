using FrameWork;
using UnityEngine.SceneManagement;


namespace Tetris {
    public class TetrisPlant : Plant<TetrisPlant> {
        public Stem StartUIStem { get; private set; }
        public Stem TetrisGameStem { get; private set; }

        protected override void Awake() {
            base.Awake();
            StartUIStem = new Stem();
        }


        public void GotoMainGameScene() {
            TetrisGameStem = new Stem();
            TetrisGameStem.Register(new TetrisLogicRoot());
            TetrisGameStem.Register(new TetrisGameSoil());
            SceneManager.LoadScene("MainPlay");
        }

        public void GotoStartUIScene() {
            StartUIStem = new Stem();
            SceneManager.LoadScene("StartUI");
        }

        public void ExitGame() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}