using System.Collections;
using DG.Tweening;
using FrameWork;
using UnityEngine;
using UnityEngine.UI;


namespace Tetris {
    public class TetrisGame : Game {
        [SerializeField] private RectTransform sceneLoadPicMaskTransform;
        [SerializeField] private GameObject sceneLoadSquarePrefab;
        [SerializeField] private GameObject Barrier;
        private GameObject[,] sceneLoadSquareList;
        private int Column = 10;
        private int Row = 6;


        protected override void Awake() {
            base.Awake();
            //配置
            ConfigController.AddConfig("Config/GameConfig.yaml", YamlConfig.ReadConfig<GameConfig>);
            ConfigController.AddConfig("Config/KeyConfig.yaml", YamlConfig.ReadConfig<KeyConfig>);
            ConfigController.AddConfig("Config/RecordConfig.yaml", YamlConfig.ReadConfig<RecordConfig>);


            //默认加载动画
            DefaultBeforeLoadSceneAnim = DefaultBeforeSceneLoad;
            DefaultAfterLoadSceneAnim = DefaultAfterSceneLoad;

            //在开始加载场景时
            OnBeforeLoadScene.Add("MainPlay", () => {
                NodeController.Register(new TetrisGameModel());
                NodeController.Register(new ScoreModel());
                NodeController.Register(new TetrisLogicOperation());
            });
            OnBeforeLoadScene.Add("EndUI", () => {
                var scoreModel = NodeController.GetNode<ScoreModel>();
                NodeController.UnRegisterAll();
                NodeController.RegisterWithoutInit(scoreModel);
            });

            //初始化场景变换的动画
            InitLoadScenePic();

            //当场景切换到MainPlay时， 根据分辨率修正场景中 摄像机范围大小
            GameViewListener.OnViewChange["MainPlay"] = (width, height) => {
                var main = Camera.main;

                if (main != null) {
                    if (height * 1f / width > 9f / 16f) {
                        Debug.Log($"{height},  {width}");
                        Debug.Log(main.orthographicSize);
                        main.orthographicSize = 11f * (height * 16f) / (width * 9f);
                        Debug.Log(main.orthographicSize);
                    }
                    else
                        main.orthographicSize = 11f;
                }
            };

            //结束时保存Config
            OnExitGame += () => {
                ConfigController.SaveConfig<GameConfig>("Config/GameConfig.yaml", YamlConfig.WriteConfig);
                ConfigController.SaveConfig<KeyConfig>("Config/KeyConfig.yaml", YamlConfig.WriteConfig);
                ConfigController.SaveConfig<RecordConfig>("Config/RecordConfig.yaml", YamlConfig.WriteConfig);
            };

            //加载声音
            SoundManager.LoadSoundClips("SoundClip", handle => {
                //背景音
                SoundManager.PlayGlobalSound("BG");
                //转到开始场景
                GotoScene("StartUI");
            });
        }

        private void InitLoadScenePic() {
            Barrier.SetActive(false);

            sceneLoadSquareList = new GameObject[Column, Row];
            var width = sceneLoadSquarePrefab.GetComponent<RectTransform>().rect.width;
            for (var x = 0; x < Column; x++) {
                for (var y = 0; y < Row; y++) {
                    var o = Instantiate(sceneLoadSquarePrefab, sceneLoadPicMaskTransform, false);
                    o.GetComponent<RectTransform>().anchoredPosition +=
                        Vector2.right * x * width + Vector2.up * y * width;

                    sceneLoadSquareList[x, y] = o;
                    o.transform.localScale = Vector3.zero;
                    o.SetActive(false);
                    o.GetComponent<Image>().color =
                        Color.HSVToRGB((x + 1) * (y + 1) * 1f / (Column + 1) / (Row + 1), 0.5f, 0.9f);
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