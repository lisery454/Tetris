using FrameWork;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris {
    public class StartCanvas : Exhibitor {
        [SerializeField] private Button startBtn;
        [SerializeField] private Button settingBtn;
        [SerializeField] private Button exitBtn;

        private void Start() {
            startBtn.onClick.AddListener(() => { GotoScene("MainPlay"); });

            settingBtn.onClick.AddListener(() => { GotoScene("Setting"); });

            exitBtn.onClick.AddListener(() => { GotoScene("Exit"); });
        }
    }
}