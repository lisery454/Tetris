using FrameWork;
using UnityEngine;

namespace Tetris {
    public class BoxManager : MainPlaySceneExhibitor {
        private float zeroX = -4.5f, zeroY = -8.5f;
        [SerializeField] private Box BoxPrefab;
        private Box[,] boxMatrix;
        private GameConfig gameConfig;


        protected override void Awake() {
            base.Awake();
            gameConfig = TetrisGame.Instance.GetConfig<GameConfig>();
            boxMatrix = new Box[gameConfig.Width, gameConfig.Height];
            for (var w = 0; w < gameConfig.Width; w++) {
                for (var h = 0; h < gameConfig.Height; h++) {
                    var box = Instantiate(BoxPrefab);
                    box.transform.position = new Vector3(zeroX + w, zeroY + h);
                    boxMatrix[w, h] = box;
                    box.SetBoxColorAndInfo(Color.green, false);
                }
            }
        }

        private void Start() {
            AddEventListener<UpdateBoxViewEvt>(UpdateBoxView).UnregisterWhenGameObjectDestroyed(gameObject);
        }

        private void UpdateBoxView(UpdateBoxViewEvt e) {
            for (var w = 0; w < gameConfig.Width; w++) {
                for (var h = 0; h < gameConfig.Height; h++) {
                    var boxInfo = e.BoxInfos[w, h];
                    boxMatrix[w, h].SetBoxColorAndInfo(boxInfo.Color, boxInfo.IsBox);
                }
            }
        }
    }
}