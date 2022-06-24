using FrameWork;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris {
    public class SettingCanvas : Exhibitor {
        private GameConfig gameConfig;
        private KeyConfig keyConfig;

        [SerializeField] private SliderText LimitHeightSlider;
        [SerializeField] private SliderText FallSpeedSlider;

        [SerializeField] private KeyCodeText LeftKeyCodeText;
        [SerializeField] private KeyCodeText RightKeyCodeText;
        [SerializeField] private KeyCodeText DownKeyCodeText;
        [SerializeField] private KeyCodeText RotKeyCodeText;


        [SerializeField] private Button saveBtn;
        [SerializeField] private Button backBtn;


        protected override void Awake() {
            base.Awake();
            gameConfig = GetConfig<GameConfig>();
            keyConfig = GetConfig<KeyConfig>();
        }

        private void Start() {
            LimitHeightSlider.SetMaxAndMin(gameConfig.LimitHeightMax, gameConfig.LimitHeightMin);
            LimitHeightSlider.SliderValue = gameConfig.LimitHeight;
            FallSpeedSlider.SetMaxAndMin(gameConfig.FallSpeedMax, gameConfig.FallSpeedMin);
            FallSpeedSlider.SliderValue = (int) Mathf.Round(1f / gameConfig.FallInterval);
            
            LeftKeyCodeText.KeyCode = keyConfig.KeyCode_Left;
            RightKeyCodeText.KeyCode = keyConfig.KeyCode_Right;
            DownKeyCodeText.KeyCode = keyConfig.KeyCode_Down;
            RotKeyCodeText.KeyCode = keyConfig.KeyCode_Rot;

            LeftKeyCodeText.keyNameText.text = "Left";
            RightKeyCodeText.keyNameText.text = "Right";
            DownKeyCodeText.keyNameText.text = "Down";
            RotKeyCodeText.keyNameText.text = "Rot";


            backBtn.onClick.AddListener(() => { GotoScene("StartUI"); });
            saveBtn.onClick.AddListener(() => {
                gameConfig.LimitHeight = (int) LimitHeightSlider.SliderValue;
                gameConfig.FallInterval = 1f / FallSpeedSlider.SliderValue;


                keyConfig.KeyCode_Left = LeftKeyCodeText.KeyCode;
                keyConfig.KeyCode_Right = RightKeyCodeText.KeyCode;
                keyConfig.KeyCode_Down = DownKeyCodeText.KeyCode;
                keyConfig.KeyCode_Rot = RotKeyCodeText.KeyCode;

                SaveConfig<GameConfig>("Config/GameConfig.yaml", YamlConfig.WriteConfig);
                SaveConfig<KeyConfig>("Config/KeyConfig.yaml", YamlConfig.WriteConfig);
                
                GotoScene("StartUI");
            });
        }
    }
}