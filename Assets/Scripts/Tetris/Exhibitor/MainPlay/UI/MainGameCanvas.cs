using UnityEngine;
using UnityEngine.UI;

namespace Tetris {
    public class MainGameCanvas : MainPlaySceneExhibitor {
        [SerializeField] private Button backBtn;

        private void Start() {
            backBtn.onClick.AddListener(() => {
                TetrisGame.Instance.ChangeScene("StartUI");
            });
        }
    }
}