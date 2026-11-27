using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float tiempoAtaque = 0;
    public float distanciaCaja;
    public Vector2 tamanno;
    public LayerMask capaEnemigo;
    Vector3 movimiento;
    Animator animator;
    AudioSource audioSource;
    public GameObject efectoMordisco;
    public int dano = 25;

    void Start(){
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update(){
        Movimiento();
        Animaciones();
        Ataque();
    }

    void Movimiento(){
        movimiento = new Vector3();

        if (Input.GetKey(KeyCode.D))
        {
            movimiento += new Vector3(1,0,0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movimiento -= new Vector3(1,0,0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            movimiento += new Vector3(0,1,0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movimiento -= new Vector3(0,1,0);
        }

        movimiento.Normalize();
    }

    void Animaciones(){
        animator.SetBool("movimiento", movimiento.sqrMagnitude > 0);

        if (movimiento.y > 0)
        {
            animator.SetInteger("direccion", 0);
        }
        else if (movimiento.y < 0)
        {
            animator.SetInteger("direccion", 1);
        }
        else if (movimiento.x < 0)
        {
            animator.SetInteger("direccion", 2);
        }
        else if (movimiento.x > 0)
        {
            animator.SetInteger("direccion", 3);
        }
    }

    void Ataque(){
        if (Input.GetKeyDown(KeyCode.Space))
        { 
            audioSource.Play();
            tiempoAtaque = 0.2f;
            Golpear();
        }

        if (tiempoAtaque > 0)
        {
            tiempoAtaque -= Time.deltaTime;
        }
        if (tiempoAtaque <= 0)
        {
            efectoMordisco.SetActive(false);
        }

        animator.SetBool("atacando", tiempoAtaque > 0);
    }

    void Golpear(){
        Vector2 vector = Vector2.up;
        int direccion = animator.GetInteger("direccion");

        if (direccion == 1)
        {
            vector = Vector2.down;
        }
        else if (direccion == 2)
        {
            vector = Vector2.left;
        }
        else if (direccion == 3)
        {
            vector = Vector2.right;
        }
        Vector3 centro = GetComponent<Collider2D>().bounds.center;
        efectoMordisco.transform.position = centro + (Vector3)vector * distanciaCaja;
        efectoMordisco.SetActive(true);
        RaycastHit2D rc = Physics2D.BoxCast(GetComponent<Collider2D>().bounds.center, tamanno, 0, vector, distanciaCaja, capaEnemigo);

        if (rc)
        {
            VidaEnemigo enemigo = rc.collider.GetComponent<VidaEnemigo>();
            VidaBoss boss = rc.collider.GetComponent<VidaBoss>();

            if(enemigo != null)
            {
                enemigo.RecibirDano(dano);
            }
            else if (boss != null)
            {
                boss.RecibirDano(dano);
            }
        }
        else
        {
            Debug.Log("No hay enemigo");
        }
    }

    void OnDrawGizmos(){
        Vector3 vector = Vector3.up;

        if (animator != null)
        {
            int direccion = animator.GetInteger("direccion");

            if (direccion == 1)
            {
                vector = Vector3.down;
            }
            else if (direccion == 2)
            {
                vector = Vector3.left;
            }
            else if (direccion == 3)
            {
                vector = Vector3.right;
            }
        }

        Gizmos.color = Color.red;
        Vector3 centro = GetComponent<Collider2D>().bounds.center;
        Gizmos.DrawWireCube(centro + vector * distanciaCaja, tamanno);
    }
    
    void FixedUpdate(){
        transform.position += movimiento * speed * Time.deltaTime;
    }

    public void BloquearJugador()
    {
        movimiento = Vector3.zero;
        tiempoAtaque = 0f;

        animator.SetBool("movimiento", false);
        animator.SetBool("atacando", false);

        efectoMordisco.SetActive(false);
        audioSource.Stop();

        enabled = false;
    }

    public void MejorarDano()
    {
        dano += 5;
    }
    public void MejorarVelocidad()
    {
        speed += 0.5f;
    }

}
