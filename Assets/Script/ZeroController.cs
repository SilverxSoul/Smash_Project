using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroController : MonoBehaviour
{
    Animator animator;
    AudioSource audiosource;
    SpriteRenderer spriterender;
    private int ComboIndex;
    public bool test;
    [SerializeField] private AudioClip[] soundAttack;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    Rigidbody2D rb;
    float horizontal;
    float vertical;
    Vector2 direction;
    public enum State
    {
        Idle,
        Attacking,
        Dashing,
        Jumping,
        Falling
    }

    bool attackAble;
    [SerializeField] private State ZeroState;
    public bool ActiveShadow;
    //Thời gian dash hiện tại
    private float dashTime;
    //Thời gian nhảy hiện tại
    private float jumpTime;
    //thời gian tối đa của một lần dash
    [SerializeField]private float maxDashTime;
    [SerializeField] private float maxJumpTime;
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
        dashTime = 0;
    }

    // Update is called once per frame
    void Update()
    {

        
    }
    private void FixedUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Attack();
        }
        if (ZeroState != State.Dashing )
        {
            Direction();
            Movement(direction);
        }

        if (ActiveShadow)
        {
            Shadows.me.ActiveShadowEffect();
        }
        if (Input.GetKeyDown(KeyCode.Z))//GetKeyDown trả về true tại ngay tại frame mà khoảng khắc bạn nhấn key đó. Qua frame sau thì GetKeyDown lại trở lại false vì lúc đó bạn đang giữ nút đó xuống nhưng là sang trạng thái là giữ chứ không phải là khoảng khắc bạn nhấn xuống.
                                        //Thế nên update mới thực hiện if này một lần, qua frame sau thì update vẫn gọi đến nó nhưng lúc này GetKeyDown đã trả về false rồi nên hàm if mới bị bỏ qua chứ không phải là update không có gọi hàm if này ở frame sau!!
                                        //Tóm lại Getkeydown là ám chỉ tại khoảng khắc bạn nhấn nút chứ không phải là bạn đang giữ nút đó.
        {
            dashTime = Time.time;
            Dash();
        }
        if (ZeroState == State.Dashing && (Time.time - dashTime) >= maxDashTime && Input.GetKey(KeyCode.Z))// GetKey là ám chỉ bạn đang giữ nút đó, nếu tại frame đó bạn thực sự đang giữ key đó thì sẽ trả về true, còn các trường hợp còn lại thì là false.
        {
            dashTime = 0;
            StopDash();
            ZeroState = State.Idle;
        }
        if (Input.GetKeyUp(KeyCode.Z))//Tương tự GetKeyDown nhưng là ám chỉ tại khoảng khắc bạn thả nút thì sẽ trả về true còn các trường hợp khác đều là false.
        {
            dashTime = 0;
            StopDash();
            ZeroState = State.Idle;
        }
        if (Input.GetKeyDown(KeyCode.X) && ZeroState != State.Jumping)
        {
            jumpTime = Time.time;
            Jump();
        }
        if (ZeroState == State.Jumping && (Time.time - jumpTime) <= maxJumpTime && Input.GetKey(KeyCode.X) )
        {
            Jump();
        }
        if (rb.velocity.y < 0 && ZeroState != State.Falling)
        {
            Debug.Log(rb.velocity.y);
            jumpTime = 0;
            Falling();
        }
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

            if (direction != Vector2.zero && ZeroState==State.Idle)
            {
                
                animator.SetBool("IsWalking", true);
            }
            else
            {
                animator.SetBool("IsWalking", false);
            }

            rb.velocity = new Vector2(Math.Sign(direction.x) * speed, rb.velocity.y);// nếu không sửa cái này thì nó sẽ lúc fall sẽ bị chuẩn hóa thành 1 khiến việc fall luôn bị fall không còn tự nhiên theo thông thường!!!
        }
        else rb.velocity = new Vector2(0,rb.velocity.y);

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
            direction= new Vector2(0,rb.velocity.y);
        }
    }
    void Dash()
    {
        ZeroState = State.Dashing;
        if(spriterender.flipX)//nếu spriterender.flipX = true thì nhân vật đang quay mặt về bên phải
        {
            rb.velocity = Vector2.right * speed * 4;
        }
        else
        {
            rb.velocity = Vector2.left* speed * 4;
        }
        ActiveShadow = true;
        animator.SetBool("IsDashing", true);
    }
    void StopDash()
    {
        ActiveShadow = false;
        animator.SetBool("IsDashing", false);
        rb.velocity = Vector2.zero;
    }

    void Jump()
    {
        ZeroState = State.Jumping;
        rb.AddForce(Vector2.up* jumpForce,ForceMode2D.Impulse);
        animator.SetTrigger("Jump");
    }
    void Falling()
    {
        ZeroState = State.Falling;
        animator.ResetTrigger("Jump");//chủ động tắt trigger thay vì để unity tự động tắt, nếu thiếu cái này animation dễ bị hiện tượng là vẫn kích hoạt animation Jump!!
        animator.SetTrigger("Fall");
        
    }

    void Landing()
    {
        ZeroState = State.Idle;
        animator.SetTrigger("Landing");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && ZeroState == State.Falling)
        {
            Landing();
        }
    }
    
}
