using FrameWork;
using UnityEngine;

namespace Tetris {
    public class BoxManager : MainPlaySceneExhibitor {
        private float zeroX = -4.5f, zeroY = -8.5f;
        [SerializeField] private Box BoxPrefab;
        private Box currentBox;

        private void Start() {
            AddEventListener<CreateBoxEvt>(CreateBox).UnregisterWhenGameObjectDestroyed(gameObject);
            AddEventListener<MoveBoxEvt>(MoveBox).UnregisterWhenGameObjectDestroyed(gameObject);

            SendCommand<NextBoxCmd>();
        }

        private void CreateBox(CreateBoxEvt evt) {
            currentBox = Instantiate(BoxPrefab, new Vector3(zeroX + evt.LocX, zeroY + evt.LocY, 0), Quaternion.identity);
        }

        private void MoveBox(MoveBoxEvt evt) {
            currentBox.transform.Translate(evt.DeltaX, evt.DeltaY, 0);
        }
    }
}