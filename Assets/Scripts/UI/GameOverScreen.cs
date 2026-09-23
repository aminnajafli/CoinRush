using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameSession session;
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text bestScoreText;

    private void Awake()
    {
        panel.SetActive(false);
    }

    private void OnEnable()
    {
        session.GameEnded += Show;
    }

    private void OnDisable()
    {
        session.GameEnded -= Show;
    }

    private void Show(RoundResult result)
    {
        scoreText.text = $"Skor: {result.Score}";
        bestScoreText.text = result.IsNewBest
            ? $"Yeni rekor: {result.BestScore}"
            : $"En yüksek skor: {result.BestScore}";

        panel.SetActive(true);
    }

    // butonların OnClick olaylarına bağlanır.
    public void OnRestartClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMenuClicked()
    {
        SceneManager.LoadScene("Menu");
    }
}