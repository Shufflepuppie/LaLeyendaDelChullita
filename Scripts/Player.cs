using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSoundController playerSoundController;
    private float velocidad = 5f;
    public int vida = 3;

    public float fuerzaRebote = 6f;
    public float longitudRaycast = 0.1f;
    public LayerMask capaSuelo;

    private bool enSuelo;
    private bool recibiendoDanio;
    public bool muerto;
    public Animator animator;
    private Rigidbody2D rb;

    public float idleTimeout = 3f;   // tiempo para entrar en Idle
    private float idleTimer = 0f;    // contador de inactividad

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerSoundController.playInicio();
    }

    void Update()
    {
        if (!muerto)
        {
            float inputX = Input.GetAxisRaw("Horizontal");

            // Movimiento
            float velocidadX = inputX * velocidad * Time.deltaTime;
            if (!recibiendoDanio)
                transform.position += new Vector3(velocidadX, 0, 0);

            // Girar personaje
            if (inputX < 0)
                transform.localScale = new Vector3(-1, 1, 1);
            else if (inputX > 0)
                transform.localScale = new Vector3(1, 1, 1);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, longitudRaycast, capaSuelo);
            enSuelo = hit.collider != null;

            animator.SetBool("enSuelo", enSuelo);
            animator.SetBool("recibeDanio", recibiendoDanio);

            // -----------------------------
            // CONTROL DE INACTIVIDAD
            // -----------------------------
            if (inputX != 0)
            {
                idleTimer = 0f;
                animator.SetBool("isIdle", false);
                animator.SetFloat("movement", Mathf.Abs(inputX));
            }
            else
            {
                idleTimer += Time.deltaTime;

                if (idleTimer >= idleTimeout)
                {
                    animator.SetBool("isIdle", true);
                    animator.SetFloat("movement", 0f);
                }
            }
        }

        animator.SetBool("muerto", muerto);
    }

    public void RecibeDanio(Vector2 direccion, int cantDanio)
    {
        if (!recibiendoDanio)
        {
            playerSoundController.playDanio();
            recibiendoDanio = true;
            vida -= cantDanio;

            if (vida <= 0)
            {
                playerSoundController.playMuerte();
                muerto = true;

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.GameOver();
                }
            }

            if (!muerto)
            {
                Vector2 rebote = new Vector2(transform.position.x - direccion.x, 0.2f).normalized;
                rb.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse);
            }
        }
    }

    public void DesactivaDanio()
    {
        recibiendoDanio = false;
        rb.linearVelocity = Vector2.zero;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitudRaycast);
    }
}
