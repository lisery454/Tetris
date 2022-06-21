using UnityEngine;
using UnityEngine.UI;

namespace Tetris {
    public class StartCanvas : StartUISceneExhibitor {
        [SerializeField] private Button startBtn;
        [SerializeField] private Button exitBtn;

        private void Start() {
            startBtn.onClick.AddListener(() => { BelongedLeader.BelongedGame.GotoScene("MainPlay"); });

            exitBtn.onClick.AddListener(() => { BelongedLeader.BelongedGame.ExitGame(); });
        }
    }
}