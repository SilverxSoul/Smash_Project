using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroController : MonoBehaviour
{
    Animator animator;
    AudioSource audiosource;
    private int ComboIndex;
    [SerializeField] private AudioClip[] soundAttack;
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
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Attack();
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
            Debug.Log(ComboIndex);
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
    }

}
