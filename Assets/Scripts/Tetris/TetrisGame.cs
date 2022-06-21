using System.Collections;
using DG.Tweening;
using FrameWork;
using UnityEngine;
using UnityEngine.UI;


namespace Tetris {
    public class TetrisGame : Game<TetrisGame> {
        public Leader startUILeader { get; private set; }
        public Leader tetrisGameLeader { get; private set; }

        [SerializeField] private RectTransform sceneLoadPicTransform;
        [SerializeField] private GameObject sceneLoadSquarePrefab;
        [SerializeField] private GameObject Barrier;
        private GameObject[,] sceneLoadSquareList;
        private int Column = 10;
        private int Row = 6;


        protected override void Awake() {
            base.Awake();
            //配置
            AddConfig(ConfigRWer.ReadConfig<GameConfig>("Config/GameConfig.yaml"));

            //添加进入和离开场景的动作
            BeforeLoadScene.Add("StartUI", BeforeGotoStartUIScene);
            BeforeLoadScene.Add("MainPlay", BeforeGotoMainGameScene);
            AfterLoadScene.Add("StartUI", AfterGotoStartUIScene);
            AfterLoadScene.Add("MainPlay", AfterGotoMainGameScene);

            //初始化场景变换的动画
            InitLoadScenePic();

            //转到开始场景
            ChangeScene("StartUI");
        }

        private void InitLoadScenePic() {
            Barrier.SetActive(false);

            sceneLoadSquareList = new GameObject[Column, Row];
            var width = sceneLoadSquarePrefab.GetComponent<RectTransform>().rect.width;
            for (var x = 0; x < Column; x++) {
                for (var y = 0; y < Row; y++) {
                    var o = Instantiate(sceneLoadSquarePrefab, sceneLoadPicTransform, true);
                    o.transform.position += Vector3.right * x * width;
                    o.transform.position += Vector3.up * y * width;
                    sceneLoadSquareList[x, y] = o;
                    o.transform.localScale = Vector3.zero;
                    o.SetActive(false);
                    o.GetComponent<Image>().color = Color.HSVToRGB(x * y * 1f / Column / Row, 0.5f, 0.9f);
                }
            }
        }

        #region 进入和离开场景的动作

        private IEnumerator BeforeGotoMainGameScene() {
            DeleteAllLeader();
            InitTetrisGameLeader();
            yield return StartCoroutine(LoadPicOn());
        }

        private IEnumerator BeforeGotoStartUIScene() {
            DeleteAllLeader();
            InitStartUILeader();
            yield return StartCoroutine(LoadPicOn());
        }

        private IEnumerator AfterGotoMainGameScene() {
            yield return StartCoroutine(LoadPicOff());
            tetrisGameLeader.SendCommand<StartGameCmd>();
        }

        private IEnumerator AfterGotoStartUIScene() {
            yield return StartCoroutine(LoadPicOff());
        }

        #endregion
        
        private void InitStartUILeader() {
            startUILeader = new Leader();
        }

        private void InitTetrisGameLeader() {
            tetrisGameLeader = new Leader();
            tetrisGameLeader.Register(new TetrisGameModel());
            tetrisGameLeader.Register(new TetrisLogicOperation());
        }

        private void DeleteAllLeader() {
            startUILeader = null;
            tetrisGameLeader = null;
        }

        private IEnumerator LoadPicOn() {
            Barrier.SetActive(true);
            var waitForSeconds = new WaitForSeconds(0.3f / Column / Row);
            for (var x = 0; x < Column; x++) {
                for (var y = 0; y < Row; y++) {
                    sceneLoadSquareList[x, y].SetActive(true);
                }
            }

            for (var x = 0; x < Column; x++) {
                for (var y = 0; y < Row; y++) {
                    sceneLoadSquareList[x, y].transform.DOScale(1f, 0.2f);
                    yield return waitForSeconds;
                }
            }

            yield return new WaitForSeconds(0.2f);
        }

        private IEnumerator LoadPicOff() {
            var waitForSeconds = new WaitForSeconds(0.3f / Column / Row);

            for (var x = 0; x < Column; x++) {
                for (var y = 0; y < Row; y++) {
                    sceneLoadSquareList[x, y].transform.DOScale(0f, 0.2f);
                    yield return waitForSeconds;
                }
            }

            for (var x = 0; x < Column; x++) {
                for (var y = 0; y < Row; y++) {
                    sceneLoadSquareList[x, y].SetActive(false);
                }
            }

            yield return new WaitForSeconds(0.2f);
            Barrier.SetActive(false);
        }
    }
}