using UnityEngine;
using UnityEngine.UI;

namespace Tetris {
    public class MainGameCanvas : MainPlaySceneExhibitor {
        [SerializeField] private Button backBtn;
        [SerializeField] private Text scoreText;

        private void Start() {
            backBtn.onClick.AddListener(() => { TetrisGame.Instance.GotoScene("StartUI"); });

            AddEventListener<GameEndEvt>(evt => { TetrisGame.Instance.GotoScene("EndUI"); });

            AddEventListener<ScoreUpdateEvt>(evt => { scoreText.text = evt.Score.ToString(); });
        }
    }
}