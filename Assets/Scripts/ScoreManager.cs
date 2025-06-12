using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;
    public StarManager starManager;

    private float startX;
    private int score = 0;

    void Start()
    {
        if (player != null)
            startX = player.position.x;
    }
void LateUpdate()
{
    if (player == null) return;

    float distance = player.position.x - startX;
    score = Mathf.Max(0, Mathf.FloorToInt(distance));
    score = Mathf.Max(0, Mathf.FloorToInt(distance * starManager.GetMultiplier()));


    scoreText.text = score.ToString("D7");
}

    public void ShowFinalScore()
    {
        finalScoreText.text = score.ToString("D7");
    }
}
