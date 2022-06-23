using FrameWork;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris {
    public class SettingCanvas : Exhibitor {
        private GameConfig gameConfig;

        [SerializeField] private SliderText LimitHeightSlider;
        [SerializeField] private SliderText FallSpeedSlider;
        [SerializeField] private Button saveBtn;
        [SerializeField] private Button backBtn;


        protected override void Awake() {
            base.Awake();
            gameConfig = GetConfig<GameConfig>();
        }

        private void Start() {
            LimitHeightSlider.SetMaxAndMin(gameConfig.LimitHeightMax, gameConfig.LimitHeightMin);
            LimitHeightSlider.SliderValue = gameConfig.LimitHeight;
            FallSpeedSlider.SetMaxAndMin(gameConfig.FallSpeedMax, gameConfig.FallSpeedMin);
            FallSpeedSlider.SliderValue = (int) Mathf.Round(1f / gameConfig.FallInterval);


            backBtn.onClick.AddListener(() => { GotoScene("StartUI"); });
            saveBtn.onClick.AddListener(() => {
                gameConfig.LimitHeight = (int) LimitHeightSlider.SliderValue;
                gameConfig.FallInterval = 1f / FallSpeedSlider.SliderValue;

                SaveConfig<GameConfig>("Config/GameConfig.yaml", YamlConfig.WriteConfig);
                GotoScene("StartUI");
            });
        }
    }
}