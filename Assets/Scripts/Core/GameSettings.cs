using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "CoinRush/Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("Tur")]
    public float roundDuration = 60f;
    public int startingLives = 3;
    public int fruitPoints = 3;

    [Header("Zorluk: tur başı → tur sonu")]
    public float startSpawnInterval = 1.0f;
    public float endSpawnInterval = 0.35f;
    public float startFallSpeed = 3f;
    public float endFallSpeed = 8f;

    [Range(0f, 1f)] public float startBombChance = 0.15f;
    [Range(0f, 1f)] public float endBombChance = 0.40f;

    [Range(0f, 1f)] public float startFruitChance = 0.30f;
    [Range(0f, 1f)] public float endFruitChance = 0.20f;

    [Header("Power-up: yavaşlatma")]
    [Range(0f, 1f)] public float powerUpChance = 0.05f;
    public float powerUpSlowDuration = 5f;
    [Range(0.1f, 1f)] public float powerUpSlowMultiplier = 0.5f;
}