using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject mainMenuUI;
    public GameObject pauseMenuUI;
    public GameObject hudUI;
    public GameObject gameOverUI;

    [Header("Referências")]
    public Transform player;
    public ScoreManager scoreManager;
    public CatController catController;

    [Header("Configuração")]
    public float deathYThreshold = -10f;

    private bool isGameOver = false;
    private bool isPaused = false;
    private bool hasGameStarted = false;

    void Start()
    {
        // Começa com menu principal
        mainMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        hudUI.SetActive(false);
        gameOverUI.SetActive(false);

        catController.canMove = false;
        Time.timeScale = 1f; // Jogo roda, mas input travado
    }

    void Update()
    {
        if (!isGameOver && hasGameStarted)
        {
            if (player.position.y < deathYThreshold)
            {
                TriggerGameOver();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                    ResumeGame();
                else
                    PauseGame();
            }
        }
    }

    public void StartGame()
    {
        mainMenuUI.SetActive(false);
        hudUI.SetActive(true);
        catController.canMove = true;
        hasGameStarted = true;
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void TriggerGameOver()
    {
        isGameOver = true;
        catController.canMove = false;

        Time.timeScale = 0f;
        hudUI.SetActive(false);
        gameOverUI.SetActive(true);
        scoreManager.ShowFinalScore();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
