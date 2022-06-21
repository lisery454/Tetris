using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Tetris {
    public class BoxManager : MainPlaySceneExhibitor {
        [SerializeField] private Box BoxPrefab;
        [SerializeField] private GameObject LimitLine;
        [SerializeField] private Transform ShowNextBoxTransform;
        [SerializeField] private float zeroX = -4.5f, zeroY = -8.5f;


        private Box[,] boxMatrix;
        private List<Box> ShowNextBoxList;
        private GameConfig gameConfig;


        protected override void Awake() {
            base.Awake();
            gameConfig = TetrisGame.Instance.GetConfig<GameConfig>();

            boxMatrix = new Box[gameConfig.Width, gameConfig.Height];
            ShowNextBoxList = new List<Box>();

            for (var w = 0; w < gameConfig.Width; w++) {
                for (var h = 0; h < gameConfig.Height; h++) {
                    var box = Instantiate(BoxPrefab, transform, true);
                    box.transform.position = new Vector3(zeroX + w, zeroY + h);
                    boxMatrix[w, h] = box;
                    box.SetBoxColorAndInfo(Color.green, false);
                }
            }

            var LimitLinePos = LimitLine.transform.position;
            LimitLinePos += Vector3.up * gameConfig.LimitHeight;
            LimitLine.transform.position = LimitLinePos;
            
            AddEventListener<UpdateBoxViewEvt>(UpdateBoxView).UnregisterWhenGameObjectDestroyed(gameObject);
            AddEventListener<NextBoxGroupEvt>(ShowNextBoxGroup).UnregisterWhenGameObjectDestroyed(gameObject);
        }
        

        private void UpdateBoxView(UpdateBoxViewEvt e) {
            for (var w = 0; w < gameConfig.Width; w++) {
                for (var h = 0; h < gameConfig.Height; h++) {
                    var boxInfo = e.StaticBoxInfo[w, h];
                    boxMatrix[w, h].SetBoxColorAndInfo(boxInfo.Color, boxInfo.IsBox);
                }
            }

            foreach (var info in e.DynamicBoxInfo) {
                boxMatrix[info.X, info.Y].SetBoxColorAndInfo(info.Color, true);
            }
        }

        private void ShowNextBoxGroup(NextBoxGroupEvt e) {
            foreach (var box in ShowNextBoxList) {
                Destroy(box.gameObject);
            }

            ShowNextBoxList.Clear();

            foreach (var info in e.NextBoxGroup) {
                var box = Instantiate(BoxPrefab, ShowNextBoxTransform, false);
                box.transform.position += new Vector3(info.X - gameConfig.NewBoxLoc.X, info.Y - gameConfig.NewBoxLoc.Y);
                box.SetBoxColorAndInfo(info.Color, true);

                ShowNextBoxList.Add(box);
            }
        }
    }
}