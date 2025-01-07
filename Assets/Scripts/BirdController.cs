using UnityEngine;

public class BirdController : MonoBehaviour
{
    public float flapStrength = 10f;  // Fuerza con la que el pájaro salta
    private Rigidbody2D rb;           // Componente Rigidbody2D del pájaro
    private Animator animator;        // Componente Animator del pájaro

    private float rotacionMaxima = 45f; // Ángulo máximo de rotación hacia arriba
    private float velocidadRotacion = 5f; // Velocidad de rotación al cambiar de ángulo

    private bool isDead = false; // Bandera para verificar si el pájaro ha muerto

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtenemos el Rigidbody2D del pájaro
        animator = GetComponent<Animator>(); // Obtenemos el Animator del pájaro
    }

    void Update()
    {
        if (isDead)
            return; // Si está muerto, no hacemos nada más

        if (Input.GetKeyDown(KeyCode.Space)) // Si presionamos espacio o pantalla (en dispositivos móviles)
        {
            Flap(); // Hacemos que el pájaro "salte"
        }

        // Calcula la rotación suave del pájaro basándose en la velocidad vertical
        float rotacion = Mathf.LerpAngle(transform.eulerAngles.z, Mathf.Sign(rb.velocity.y) * rotacionMaxima, velocidadRotacion * Time.deltaTime);

        // Aplica la rotación al pájaro
        transform.rotation = Quaternion.Euler(0, 0, rotacion);
    }

    void Flap()
    {
        rb.velocity = Vector2.up * flapStrength; // Aplica una fuerza hacia arriba al Rigidbody2D
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead)
            return; // Si ya está muerto, no hacemos nada

        isDead = true; // Marcamos que el pájaro ha muerto
        animator.SetTrigger("DieTrigger"); // Activamos la animación de morir
        rb.velocity = Vector2.zero; // Detenemos el movimiento del pájaro
        rb.simulated = false; // Deshabilitamos la física del pájaro

        GameObject.FindObjectOfType<GameOverManager>().ShowGameOver(); // Muestra la pantalla de Game Over
    }

    // Función para resetear el estado del pájaro
    public void ResetBird()
    {
        isDead = false;
        rb.simulated = true;
        rb.velocity = Vector2.zero;
        transform.rotation = Quaternion.Euler(0, 0, 0); // Reset rotation
    }
}
