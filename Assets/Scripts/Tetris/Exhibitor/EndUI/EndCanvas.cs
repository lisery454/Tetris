using Tetris;
using UnityEngine;
using UnityEngine.UI;

public class EndCanvas : EndUISceneExhibitor {
    [SerializeField] private Text scoreText;
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button againBtn;

    private void Start() {
        againBtn.onClick.AddListener(() => { BelongedLeader.BelongedGame.GotoScene("MainPlay"); });
        exitBtn.onClick.AddListener(() => { BelongedLeader.BelongedGame.ExitGame(); });
        scoreText.text = GetModel<ScoreModel>().Score.ToString();
    }
}