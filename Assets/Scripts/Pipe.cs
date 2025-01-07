using UnityEngine;

public class Pipe : MonoBehaviour
{
    public float moveSpeed = 2f; // Velocidad de movimiento de los tubos
    public bool stopMovement = false; // Bandera global para detener el movimiento

    private BoxCollider2D triggerZone; // Zona del trigger para detectar el paso

    void Start()
    {
        triggerZone = GetComponent<BoxCollider2D>();
        if (triggerZone == null)
        {
            triggerZone = gameObject.AddComponent<BoxCollider2D>();
        }

        triggerZone.isTrigger = true; // Asegúrate de que sea un trigger
    }

    void Update()
    {
        if (stopMovement)
            return;

        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        if (transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        BirdController bird = other.GetComponent<BirdController>();
        if (bird != null)
        {
            Debug.Log("Trigger detectado! Ocell passant pels pipes.");
            GameManager.instance.OnPlayerPassPipe();
        }
    }

    public void StopPipes()
    {
        stopMovement = true;
    }
}