using System.Collections;
using DG.Tweening;
using FrameWork;
using UnityEngine;
using UnityEngine.UI;


namespace Tetris {
    public class TetrisGame : Game {
        [SerializeField] private RectTransform sceneLoadPicTransform;
        [SerializeField] private GameObject sceneLoadSquarePrefab;
        [SerializeField] private GameObject Barrier;
        private GameObject[,] sceneLoadSquareList;
        private int Column = 10;
        private int Row = 6;


        protected override void Awake() {
            base.Awake();
            //配置
            AddConfig(YamlConfigRWer.ReadConfig<GameConfig>("Config/GameConfig.yaml"));

            //默认加载动画
            DefaultBeforeLoadSceneAnim = DefaultBeforeSceneLoad;
            DefaultAfterLoadSceneAnim = DefaultAfterSceneLoad;

            //在开始加载场景时
            OnStartLoadScene.Add("StartUI", () => { LeaderFactory.CreateLeader("StartUI"); });

            OnStartLoadScene.Add("MainPlay", () => {
                var leader = LeaderFactory.CreateLeader("MainPlay");
                leader.Register(new TetrisGameModel());
                leader.Register(new ScoreModel());
                leader.Register(new TetrisLogicOperation());
            });

            OnStartLoadScene.Add("EndUI", () => {
                var leader = LeaderFactory.CreateLeader("EndUI");
                leader.RegisterWithoutInit(LeaderFactory.GetLeader("MainPlay").GetModel<ScoreModel>());
            });
            
            OnStartLoadScene.Add("Setting", () => { LeaderFactory.CreateLeader("Setting"); });


            //初始化场景变换的动画
            InitLoadScenePic();

            //转到开始场景
            GotoScene("StartUI");
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

        private IEnumerator DefaultBeforeSceneLoad() {
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

        private IEnumerator DefaultAfterSceneLoad() {
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