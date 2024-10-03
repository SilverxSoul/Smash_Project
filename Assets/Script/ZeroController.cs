using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroController : MonoBehaviour
{
    Animator animator;
    AudioSource audiosource;
    SpriteRenderer spriterender;
    private int ComboIndex;
    [SerializeField] private AudioClip[] soundAttack;
    [SerializeField] private float speed;
    Rigidbody2D rb;
    float horizontal;
    float vertical;
    Vector2 direction;
    public enum State
    {
        Idle,
        Attacking,
        Dashing
    }

    bool attackAble;
    [SerializeField] private State ZeroState;
    // Start is called before the first frame update
    void Start()
    {
        animator=GetComponent<Animator>();
        ComboIndex = 1;
        attackAble = true;
        audiosource=GetComponent<AudioSource>();
        spriterender=GetComponent<SpriteRenderer>();
        rb=GetComponent<Rigidbody2D>();
        ZeroState = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Attack();
        }
        Direction();
        Movement(direction);


    }

    void Attack()
    {
        if(attackAble)
        {
            animator.SetTrigger("Attack" + ComboIndex);
            attackAble = false;
            audiosource.clip = soundAttack[ComboIndex-1];
            audiosource.Play();
            ZeroState = State.Attacking;
        }
        
    }
    
   public void SetCombo()
    {
        
        if(ComboIndex<3)
        {
            ComboIndex = ComboIndex + 1;
            attackAble = true;
            
        }
        
    }

    public void ResetCombo()
    {
        ComboIndex = 1;
        attackAble = true;
        ZeroState = State.Idle;
    }

    void Movement(Vector2 direction)
    {
        if( ZeroState !=State.Attacking)
        {
            //bo switch nay cung duoc vi Direction da flip roi !!!
            switch (direction.x)
            {
                case 1:
                    spriterender.flipX = true;
                    break;
                case -1:
                    spriterender.flipX = false;
                    break;
            }

            if (direction != Vector2.zero)
            {
                
                animator.SetBool("IsWalking", true);
            }
            else
            {
                animator.SetBool("IsWalking", false);
            }

            rb.velocity = direction.normalized * speed;
        }
        else rb.velocity = Vector2.zero;

    }
    void Direction()
    {

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            direction = Vector2.right;
            spriterender.flipX = true;//neu ban khong muon cho attack bi flip khi chua hoan thanh combo thi dung them
                                      //dong nay o Direction vi cai Direction nay no se thay doi huong chung cho toan bo
                                      //cac action khac va chi dung cai flip tai cac hanh dong ban muon flip cu the
                                      //nhu Movement cha han!!
                                      

        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            direction = Vector2.left;
            spriterender.flipX = false;
        }
      
        else
        {
            direction= Vector2.zero;
        }
    }
}
