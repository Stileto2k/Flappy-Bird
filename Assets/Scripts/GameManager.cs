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
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject); // Manté el GameManager entre escenes
    }

    void Start()
    {
        // Asegurem que els textos estiguin assignats després de carregar la nova escena
        if (scoreText == null || bestScoreText == null)
        {
            FindUIElements(); // Busquem els textos en la nova escena si no estan assignats
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        LoadBestScore(); // Carreguem el millor puntatge guardat
        UpdateScore(0);  // Actualitzem la puntuació a 0 al començar
    }

    // Mètode per trobar i assignar els textos de puntuació si no estan assignats
    private void FindUIElements()
    {
        scoreText = FindObjectOfType<TMP_Text>(); // Assignem el text de la puntuació (si només hi ha un, el trobem)
        bestScoreText = FindObjectOfType<TMP_Text>(); // Assignem el text del millor puntatge (assumim que també n'hi ha un)
        
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
            if (bestScoreText != null)
                bestScoreText.text = "Best Score: " + bestScore;
            SaveBestScore(); // Guardem el millor puntatge
        }
    }

    // Mètode cridat quan l'ocell passa entre els tubs
    public void OnPlayerPassPipe()
    {
        UpdateScore(1); // Suma 1 punt
        PlaySound(pointSound); // Reprodueix el so de puntatge
    }

    // Mètode per guardar el millor puntatge
    private void SaveBestScore()
    {
        PlayerPrefs.SetInt("BestScore", bestScore);
        PlayerPrefs.Save(); // Guardem les dades persistentment
    }

    // Mètode per carregar el millor puntatge
    private void LoadBestScore()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0); // Si no hi ha cap valor guardat, carrega 0
        if (bestScoreText != null)
            bestScoreText.text = "Best Score: " + bestScore;
    }

    // Mètode per reiniciar el joc
    public void RestartGame()
    {
        score = 0; // Reiniciem la puntuació
        UpdateScore(0); // Actualitzem la puntuació en pantalla

        // Reiniciem l'estat de l'ocell i els tubs
        ResetBird();
        ResetPipeSpawner();

        // Reiniciem el temps (en cas que es pausés en game over)
        Time.timeScale = 1f;

        // Carreguem l'escena actual per reiniciar el nivell
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Mètode per reiniciar l'ocell
    private void ResetBird()
    {
        BirdController bird = FindObjectOfType<BirdController>();
        if (bird != null)
        {
            bird.ResetBird(); // Reiniciem l'estat de l'ocell
        }
    }

    // Mètode per reiniciar el spawner de tubs
    private void ResetPipeSpawner()
    {
        PipeSpawner pipeSpawner = FindObjectOfType<PipeSpawner>();
        if (pipeSpawner != null)
        {
            pipeSpawner.ResetSpawner(); // Reiniciem el spawner de tubs
        }
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
        audioSource.clip = clip;
        audioSource.Play();
    }
}
