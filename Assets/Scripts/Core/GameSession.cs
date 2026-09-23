using System;
using UnityEngine;

public struct RoundResult
{
    public int Score;
    public int BestScore;
    public bool IsNewBest;
}

public class GameSession : MonoBehaviour
{
    [SerializeField] private GameSettings settings;
    [SerializeField] private BasketController basket;

    public event Action<int> ScoreChanged;
    public event Action<int> LivesChanged;
    public event Action<float> TimeChanged;
    public event Action<RoundResult> GameEnded;

    public bool IsPlaying { get; private set; }

    // 0 = tur başı, 1 = tur sonu. zorluk bu değere göre artar.
    public float Progress => Mathf.Clamp01(1f - timeLeft / settings.roundDuration);

    private int score;
    private int lives;
    private float timeLeft;

    private void OnEnable()
    {
        basket.ItemCaught += HandleItemCaught;
    }

    private void OnDisable()
    {
        basket.ItemCaught -= HandleItemCaught;
    }

    private void Start()
    {
        score = 0;
        lives = settings.startingLives;
        timeLeft = settings.roundDuration;
        IsPlaying = true;
        basket.ControlEnabled = true;

        ScoreChanged?.Invoke(score);
        LivesChanged?.Invoke(lives);
        TimeChanged?.Invoke(timeLeft);
    }

    private void Update()
    {
        if (!IsPlaying)
        {
            return;
        }

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            TimeChanged?.Invoke(timeLeft);
            EndRound();
            return;
        }

        TimeChanged?.Invoke(timeLeft);
    }

    private void HandleItemCaught(FallingItem item)
    {
        if (!IsPlaying)
        {
            return;
        }

        switch (item.Type)
        {
            case ItemType.Coin:
                score += item.Points;
                ScoreChanged?.Invoke(score);
                return;

            case ItemType.Fruit:
                score += settings.fruitPoints;
                ScoreChanged?.Invoke(score);
                return;

            case ItemType.Bomb:
                lives--;
                LivesChanged?.Invoke(lives);

                if (lives <= 0)
                {
                    EndRound();
                }
                return;
        }
    }

    private void EndRound()
    {
        IsPlaying = false;
        basket.ControlEnabled = false;

        bool isNewBest = HighScoreStorage.TrySet(score);

        GameEnded?.Invoke(new RoundResult
        {
            Score = score,
            BestScore = HighScoreStorage.Get(),
            IsNewBest = isNewBest
        });
    }
}