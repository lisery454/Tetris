using FrameWork;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris {
    public class MainGameCanvas : Exhibitor {
        [SerializeField] private Button backBtn;
        [SerializeField] private Text scoreText;

        private void Start() {
            backBtn.onClick.AddListener(() => {
                PlaySFX("ClickBtn");
                GotoScene("StartUI");
            });

            AddEventListener<GameEndEvt>(evt => {
                PlaySFX("End");
                GotoScene("EndUI");
            });

            AddEventListener<ScoreUpdateEvt>(evt => { scoreText.text = evt.Score.ToString(); });

            AddEventListener<EliminateEvt>(evt => { PlaySFX("Eliminate", evt.level); });
        }
    }
}