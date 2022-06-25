using FrameWork;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris {
    public class StartCanvas : Exhibitor {
        [SerializeField] private Button startBtn;
        [SerializeField] private Button settingBtn;
        [SerializeField] private Button exitBtn;

        private void Start() {
            startBtn.onClick.AddListener(() => {
                PlaySFX("ClickBtn");
                GotoScene("MainPlay");
            });

            settingBtn.onClick.AddListener(() => {
                PlaySFX("ClickBtn");
                GotoScene("Setting");
            });

            exitBtn.onClick.AddListener(() => {
                PlaySFX("ClickBtn");
                GotoScene("Exit");
            });
        }
    }
}