using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemySoundController enemySoundController;
    public Transform player;
    public float detectionRadius = 5f;
    public float speed = 2f;

    private Rigidbody2D rb;
    private Animator animator;

    private bool playerDetected = false;
    private bool isChasing = false;
    private bool playerVivo;

    void Start()
    {
        playerVivo = true;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (playerVivo)
        {
            Movimiento();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!playerVivo) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Player playerScript = collision.gameObject.GetComponent<Player>();
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            // Normal del contacto
            ContactPoint2D contacto = collision.contacts[0];

            bool golpeDesdeArriba =
                contacto.normal.y < -0.5f &&        // el jugador viene desde arriba
                playerRb.linearVelocity.y < 0f;     // está cayendo

            if (golpeDesdeArriba)
            {
                // 💥 ENEMIGO APLASTADO
                Morir();

                // pequeño rebote del jugador
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 8f);
            }
            else
            {
                // 💢 JUGADOR RECIBE DAÑO
                if (!playerScript.muerto)
                {
                    Vector2 direccionDanio = new Vector2(transform.position.x, 0);
                    playerScript.RecibeDanio(direccionDanio, 1);

                    playerVivo = !playerScript.muerto;
                    if (!playerVivo)
                    {
                        isChasing = false;
                    }
                }
            }
        }
    }

    // -----------------------
    // ESTE MÉTODO SE LLAMA DESDE LA ANIMACIÓN "WakeUp"
    // -----------------------
    public void StartChasing()
    {
        isChasing = true;
        animator.SetBool("isChasing", true);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    private void Movimiento()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // -----------------------
        // DETECCIÓN DEL JUGADOR
        // -----------------------
        if (distanceToPlayer <= detectionRadius && !playerDetected)
        {
            playerDetected = true;

            animator.SetBool("isAwake", true);   // pasa de Idle a WakeUp
        }

        // -----------------------
        // PERSECUCIÓN
        // -----------------------
        if (isChasing)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 movement = new Vector2(direction.x, 0);

            rb.MovePosition(rb.position + movement * speed * Time.deltaTime);

            // girar enemigo
            if (movement.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);
            else if (movement.x > 0)
                transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void Morir()
    {
        enemySoundController.playFuckGhosts();

        playerVivo = false;
        isChasing = false;

        // animación de muerte
        if (animator != null)
            animator.SetTrigger("die");

        // quitar colisiones
        GetComponent<Collider2D>().enabled = false;

        // dejar que caiga / se aplaste
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 2f;

        // destruir después de un tiempo
        Destroy(gameObject, 1.5f);
    }


}

