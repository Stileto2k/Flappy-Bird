using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Agrega esta línea para permitir el uso de TMP_Text

public class StartGameButton : MonoBehaviour
{
    public TMP_Text startButtonText; // Referencia al componente TMP_Text del botón

    // Esto se puede hacer en el editor de Unity
    public void OnStartGameButtonClick()
    {
        startButtonText.text = "Iniciando..."; // Cambia el texto del botón
        SceneManager.LoadScene("GameScene"); // Carga la escena del juego
    }
}
