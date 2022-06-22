using FrameWork;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris {
    public class StartCanvas : Exhibitor {
        [SerializeField] private Button startBtn;
        [SerializeField] private Button settingBtn;
        [SerializeField] private Button exitBtn;

        private void Start() {
            startBtn.onClick.AddListener(() => { BelongedLeader.BelongedGame.GotoScene("MainPlay"); });
            
            settingBtn.onClick.AddListener(() => { BelongedLeader.BelongedGame.GotoScene("Setting"); });
            
            exitBtn.onClick.AddListener(() => { BelongedLeader.BelongedGame.ExitGame(); });
        }
    }
}