using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    Vector3 movimiento;
    Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
       animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
        animator.SetBool("movimiento", movimiento.sqrMagnitude > 0);
        
    }
    
    void FixedUpdate()
    {
        transform.position += movimiento * speed * Time.deltaTime;
    }
}
