using FrameWork;
using Tetris;
using UnityEngine;
using UnityEngine.UI;

public class EndCanvas : Exhibitor {
    [SerializeField] private Text scoreText;
    [SerializeField] private Button backBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button againBtn;
    [SerializeField] private Text maxScoreText;

    private void Start() {
        againBtn.onClick.AddListener(() => {
            PlaySFX("ClickBtn");
            GotoScene("MainPlay");
        });
        backBtn.onClick.AddListener(() => {
            PlaySFX("ClickBtn");
            GotoScene("StartUI");
        });
        exitBtn.onClick.AddListener(() => {
            PlaySFX("ClickBtn");
            GotoScene("Exit");
        });
        scoreText.text = GetModel<ScoreModel>().Score.ToString();
        maxScoreText.text = GetConfig<RecordConfig>().MaxScore.ToString();
    }
}