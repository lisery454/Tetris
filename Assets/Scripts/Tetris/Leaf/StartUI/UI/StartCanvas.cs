using UnityEngine;
using UnityEngine.UI;

namespace Tetris {
    public class StartCanvas : StartUISceneLeaf {
        [SerializeField] private Button startBtn;
        [SerializeField] private Button exitBtn;

        private void Start() {
            startBtn.onClick.AddListener(() => {
                TetrisPlant.Instance.GotoMainGameScene();
            });
            
            exitBtn.onClick.AddListener(() => {
                TetrisPlant.Instance.ExitGame();
            });
        }
    }
}