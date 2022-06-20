using UnityEngine;
using UnityEngine.UI;

namespace Tetris {
    public class MainGameCanvas : MainPlaySceneExhibitor {
        [SerializeField] private Button backBtn;
        [SerializeField] private Text tipText;

        private void Start() {
            backBtn.onClick.AddListener(() => { TetrisGame.Instance.ChangeScene("StartUI"); });

            AddEventListener<FailEvt>(evt => { tipText.text = "You Fail!"; });
        }
    }
}