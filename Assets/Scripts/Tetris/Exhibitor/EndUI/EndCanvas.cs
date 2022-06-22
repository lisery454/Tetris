using FrameWork;
using Tetris;
using UnityEngine;
using UnityEngine.UI;

public class EndCanvas : Exhibitor {
    [SerializeField] private Text scoreText;
    [SerializeField] private Button backBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button againBtn;

    private void Start() {
        againBtn.onClick.AddListener(() => { BelongedLeader.BelongedGame.GotoScene("MainPlay"); });
        backBtn.onClick.AddListener(() => { BelongedLeader.BelongedGame.GotoScene("StartUI"); });
        exitBtn.onClick.AddListener(() => { BelongedLeader.BelongedGame.ExitGame(); });
        scoreText.text = GetModel<ScoreModel>().Score.ToString();
    }
}