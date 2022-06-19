using UnityEngine;
using UnityEngine.UI;

namespace Tetris {
    public class StartCanvas : StartUISceneExhibitor {
        [SerializeField] private Button startBtn;
        [SerializeField] private Button exitBtn;

        private void Start() {
            startBtn.onClick.AddListener(() => { TetrisGame.Instance.GotoMainGameScene(); });

            exitBtn.onClick.AddListener(() => { TetrisGame.Instance.ExitGame(); });
        }
    }
}