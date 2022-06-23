using FrameWork;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris {
    public class MainGameCanvas : Exhibitor {
        [SerializeField] private Button backBtn;
        [SerializeField] private Text scoreText;

        private void Start() {
            backBtn.onClick.AddListener(() => { GotoScene("StartUI"); });

            AddEventListener<GameEndEvt>(evt => { GotoScene("EndUI"); });

            AddEventListener<ScoreUpdateEvt>(evt => { scoreText.text = evt.Score.ToString(); });
        }
    }
}