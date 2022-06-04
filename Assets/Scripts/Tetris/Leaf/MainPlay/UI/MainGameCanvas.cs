using UnityEngine;
using UnityEngine.UI;

namespace Tetris.UI {
    public class MainGameCanvas : MainPlaySceneLeaf {
        [SerializeField] private Button backBtn;

        private void Start() {
            backBtn.onClick.AddListener(() => { TetrisPlant.Instance.GotoStartUIScene(); });
        }
    }
}