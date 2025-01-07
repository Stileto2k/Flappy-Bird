using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    public GameOverManager gameOverManager; // Referencia al script GameOverManager

    void Start()
    {
        // Configura el botón para que ejecute el método RestartGame() del script GameOverManager
        GetComponent<Button>().onClick.AddListener(gameOverManager.RestartGame);
    }
}
