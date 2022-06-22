using FrameWork;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris {
    public class SettingCanvas : Exhibitor {
        private GameConfig gameConfig;

        [SerializeField] private SliderText LimitHeightSlider;
        [SerializeField] private Button saveBtn;
        [SerializeField] private Button backBtn;


        protected override void Awake() {
            base.Awake();
            gameConfig = GetConfig<GameConfig>();
        }

        private void Start() {
            LimitHeightSlider.SetMaxAndMin(gameConfig.LimitHeightMax, gameConfig.LimitHeightMin);
            LimitHeightSlider.SliderValue = gameConfig.LimitHeight;


            backBtn.onClick.AddListener(() => { BelongedLeader.BelongedGame.GotoScene("StartUI"); });
            saveBtn.onClick.AddListener(() => {
                gameConfig.LimitHeight = (int) LimitHeightSlider.SliderValue;
                BelongedLeader.BelongedGame.SaveConfig<GameConfig>();
            });
        }
    }
}