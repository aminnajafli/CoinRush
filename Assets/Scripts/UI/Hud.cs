using TMPro;
using UnityEngine;

public class Hud : MonoBehaviour
{
    [SerializeField] private GameSession session;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text livesText;

    private int shownSecond = -1;

    private void OnEnable()
    {
        session.ScoreChanged += OnScoreChanged;
        session.LivesChanged += OnLivesChanged;
        session.TimeChanged += OnTimeChanged;
    }

    private void OnDisable()
    {
        session.ScoreChanged -= OnScoreChanged;
        session.LivesChanged -= OnLivesChanged;
        session.TimeChanged -= OnTimeChanged;
    }

    private void OnScoreChanged(int score)
    {
        scoreText.text = $"Skor: {score}";
    }

    private void OnLivesChanged(int lives)
    {
        livesText.text = $"Can: {lives}";
    }

    private void OnTimeChanged(float timeLeft)
    {
        // yazıyı sadece saniye değişince güncelle; her karede string üretmek çöp yaratır.
        int second = Mathf.CeilToInt(timeLeft);

        if (second == shownSecond)
        {
            return;
        }

        shownSecond = second;
        timeText.text = second.ToString();
    }
}