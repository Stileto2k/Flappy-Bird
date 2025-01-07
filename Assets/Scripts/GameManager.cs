using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Instància única del GameManager
    public TMP_Text scoreText;           // Text que mostra la puntuació
    public TMP_Text bestScoreText;       // Text que mostra el millor puntatge

    [Header("Audio Clips")]
    public AudioClip pointSound;         // So quan es passa entre els tubs
    public AudioClip gameOverSound;      // So quan es mor l'ocell

    private AudioSource audioSource;     // Font d'àudio
    private int score = 0;               // Puntuació actual
    private int bestScore = 0;           // Millor puntuació guardada

    void Awake()
    {
        // Implementem el patró Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Manté el GameManager entre escenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Carreguem el millor puntatge quan es crea el GameManager
        LoadBestScore();
    }

    void Start()
    {
        // Asegurem que els textos estiguin assignats després de carregar la nova escena
        if (scoreText == null || bestScoreText == null)
        {
            FindUIElements();
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        UpdateScore(0);  // Inicialitzem el marcador a 0
        UpdateBestScoreText();  // Actualitzem el millor marcador en el text
    }

    void OnEnable()
    {
        // Ens subscrivim a l'esdeveniment de càrrega d'escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Ens desubscrivim per evitar errors
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // S'executa quan es carrega una nova escena
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindUIElements(); // Tornem a buscar els textos
        ResetScore();     // Reiniciem el marcador
        UpdateBestScoreText(); // Actualitzem el millor marcador
    }

    // Mètode per trobar i assignar els textos de puntuació
    private void FindUIElements()
    {
        TMP_Text[] texts = FindObjectsOfType<TMP_Text>();
        foreach (TMP_Text text in texts)
        {
            if (text.name == "ScoreText") scoreText = text;
            if (text.name == "BestScoreText") bestScoreText = text;
        }

        if (scoreText == null || bestScoreText == null)
        {
            Debug.LogError("Els textos de puntuació no es troben a l'escena actual!");
        }
    }

    // Mètode per actualitzar la puntuació
    public void UpdateScore(int points)
    {
        score += points;

        if (scoreText != null)
            scoreText.text = "Score: " + score;

        // Compara i actualitza el millor puntatge si cal
        if (score > bestScore)
        {
            bestScore = score;
            SaveBestScore(); // Guardem el millor puntatge
            UpdateBestScoreText(); // Actualitzem el text de millor puntuació
        }
    }

    // Mètode per reiniciar la puntuació
    private void ResetScore()
    {
        score = 0; // Reiniciem la puntuació
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    // Mètode per guardar el millor puntatge
    private void SaveBestScore()
    {
        PlayerPrefs.SetInt("BestScore", bestScore);
        PlayerPrefs.Save();
    }

    // Mètode per carregar el millor puntatge
    public void LoadBestScore()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0); // Carrega 0 si no hi ha cap valor guardat
    }

    // Actualitza el text de millor puntuació
    private void UpdateBestScoreText()
    {
        if (bestScoreText != null)
            bestScoreText.text = "Best Score: " + bestScore;
    }

    // Mètode cridat quan l'ocell passa entre els tubs
    public void OnPlayerPassPipe()
    {
        UpdateScore(1); // Suma 1 punt
        PlaySound(pointSound); // Reprodueix el so de puntatge
    }

    // Mètode per reiniciar el joc
    public void RestartGame()
    {
        // Reiniciem l'escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Mètode per mostrar la pantalla de Game Over
    public void ShowGameOver()
    {
        PlaySound(gameOverSound); // Reprodueix el so de game over
        Time.timeScale = 0f; // Pausa el joc
        GameObject.FindObjectOfType<GameOverManager>().ShowGameOver(); // Mostra el menú de game over
    }

    // Mètode per reproduir sons
    public void PlaySound(AudioClip clip)
    {
        if (clip == null) return;
        audioSource.clip = clip;
        audioSource.Play();
    }
}
