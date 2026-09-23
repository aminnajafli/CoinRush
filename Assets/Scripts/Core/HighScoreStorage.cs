using UnityEngine;

public static class HighScoreStorage
{
    private const string Key = "coinrush.highscore";

    public static int Get()
    {
        return PlayerPrefs.GetInt(Key, 0);
    }

    // skor rekor ise kaydeder ve true döner.
    public static bool TrySet(int score)
    {
        if (score <= Get())
        {
            return false;
        }

        PlayerPrefs.SetInt(Key, score);
        PlayerPrefs.Save();
        return true;
    }

    // testler ve hata ayıklama için.
    public static void Reset()
    {
        PlayerPrefs.DeleteKey(Key);
    }
}