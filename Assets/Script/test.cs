using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float ForceForDefault;
    [SerializeField] private float ForceForImpulse;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.X) )
        {
            Debug.Log("Pressed x");
            Jump1();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Pressed z");
            Jump2();
        }
    }
    void Jump1()
    {
        Vector2 jumpForce = new Vector2(0, ForceForDefault);
        Debug.Log("Jump");
        rb.AddForce(jumpForce);
    }
    private void Jump2()
    {
        Vector2 jumpForce = new Vector2(0, ForceForImpulse);
        Debug.Log("Jump");
        rb.AddForce(jumpForce,ForceMode2D.Impulse);
    }
}
