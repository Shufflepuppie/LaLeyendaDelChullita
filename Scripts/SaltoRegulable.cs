using Unity.VisualScripting;
using UnityEngine;

public class SaltoRegulable : MonoBehaviour
{
    public PlayerSoundController playerSoundController;

    [Header("Referencias")]
    private Rigidbody2D rb2D;
    private Animator animator;

    [Header("Salto")]
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private Vector2 dimesionesCaja;
    [SerializeField] private LayerMask queEsSuelo;

    private bool enSuelo;
    private bool saltar;

    [Header("SaltoRegulable")]
    [Range(0, 1)]
    [SerializeField] private float multiplicadorCancelarSalto;
    [SerializeField] private float multiplicadorGravedad;

    private float escalaGravedad;
    private bool botonSaltoArriba = true;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        escalaGravedad = rb2D.gravityScale;
    }

    void Update()
    {
        if (Input.GetButton("Jump"))
        {
            saltar = true;
        }

        if (Input.GetButtonUp("Jump"))
        {
            BotonSaltoArriba();
        }

        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimesionesCaja, 0, queEsSuelo);
        animator.SetBool("enSuelo", enSuelo);
    }

    private void FixedUpdate()
    {
        if (saltar && botonSaltoArriba && enSuelo)
        {
            Saltar();
        }

        if (rb2D.linearVelocityY < 0 && !enSuelo)
        {
            rb2D.gravityScale = escalaGravedad * multiplicadorGravedad;
        }
        else
        {
            rb2D.gravityScale = escalaGravedad;
        }

        saltar = false;
    }

    private void Saltar()
    {
        rb2D.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        enSuelo = false;
        playerSoundController.playSaltar();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(controladorSuelo.position, dimesionesCaja);
    }

    private void BotonSaltoArriba()
    {
        if (rb2D.linearVelocityY > 0)
        {
            rb2D.AddForce(Vector2.down * rb2D.linearVelocityY * (1 - multiplicadorCancelarSalto), ForceMode2D.Impulse);
        }

        botonSaltoArriba = true;
        saltar = false;
    }

    public void Rebote()
    {
        rb2D.AddForce(new Vector2(0f, fuerzaSalto));
    }
}
