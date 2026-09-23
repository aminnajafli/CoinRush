using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text bestScoreText;

    private void Start()
    {
        bestScoreText.text = $"En yüksek skor: {HighScoreStorage.Get()}";
    }

    // Başla butonunun OnClick olayına bağlanır.
    public void OnStartClicked()
    {
        SceneManager.LoadScene("Game");
    }

    // Çıkış butonunun OnClick olayına bağlanır.
    public void OnQuitClicked()
    {
        Application.Quit();

#if UNITY_EDITOR
        // Application.Quit() editörde hiçbir şey yapmaz; test ederken Play modunu
        // kendimiz durduruyoruz ki buton editörde de bir şey yapıyormuş gibi hissettirsin.
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // Editörde bileşenin sağ üstündeki üç nokta menüsünden çalıştırılır (test için).
    [ContextMenu("En yüksek skoru sıfırla")]
    private void ResetHighScore()
    {
        HighScoreStorage.Reset();
    }
}