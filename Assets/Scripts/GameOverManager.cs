using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public TMP_Text gameOverText; // Texto de Game Over
    public TMP_Text pressText;    // Texto de "Presionar cualquier tecla"
    public GameObject pipePrefab; // Referencia al prefab de los tubos
    public GameObject restartButton; // Referencia al botón de reiniciar
    private PipeSpawner pipeSpawner; // Referencia al PipeSpawner
    private bool isGameOver = false;

    void Start()
    {
        pipeSpawner = FindObjectOfType<PipeSpawner>(); // Encuentra el PipeSpawner en la escena
        CreatePipes(); // Crea los tubos iniciales al inicio

        // Inicialmente oculta los textos y el botón de reiniciar
        gameOverText.gameObject.SetActive(false);
        pressText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);

        // Añade el evento OnClick para el botón de reiniciar
        restartButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(RestartGame);
    }

    // Método para mostrar los mensajes de Game Over
    public void ShowGameOver()
    {
        isGameOver = true; // Marca el juego como terminado
        Time.timeScale = 0f; // Pausa el tiempo del juego
        gameOverText.gameObject.SetActive(true); // Muestra el mensaje de Game Over
        pressText.gameObject.SetActive(true);    // Muestra el mensaje de "Presionar"
        restartButton.gameObject.SetActive(true); // Muestra el botón de reiniciar

        // Actualiza el texto del Best Score al mostrar Game Over
        if (GameManager.instance != null)
        {
            GameManager.instance.LoadBestScore();
            GameManager.instance.PlaySound(GameManager.instance.gameOverSound); // Reproduce el sonido
        }
    }

    // Método para ocultar los mensajes de Game Over
    public void HideGameOver()
    {
        gameOverText.gameObject.SetActive(false);
        pressText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
    }

    // Método para reiniciar la partida
    public void RestartGame()
    {
        Time.timeScale = 1f; // Restaura el tiempo del juego

        // Limpia el estado del juego
        isGameOver = false;
        HideGameOver(); // Oculta los mensajes de Game Over

        // Resetea los componentes del juego
        ResetGame();

        // Recarga la escena actual para reiniciar el nivel desde cero
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Método para crear los tubos iniciales cuando comienza el juego
    private void CreatePipes()
    {
        if (pipePrefab != null && pipeSpawner != null)
        {
            pipeSpawner.SpawnPipe(); // Llame a SpawnPipe() aquí
        }
    }

    // Método para resetear los estados del juego
    private void ResetGame()
    {
        pipeSpawner.ResetSpawner();
        BirdController bird = FindObjectOfType<BirdController>();
        if (bird != null)
        {
            bird.ResetBird(); // Reestablece el estado del pájaro
        }
    }
}
