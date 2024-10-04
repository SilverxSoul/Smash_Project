using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solid : MonoBehaviour
{
    private SpriteRenderer myRenderer;
    private Shader myMaterial;
    public Color color;

    // Start is called before the first frame update
    void Start()
    {
        myRenderer=GetComponent<SpriteRenderer>(); 
        myMaterial = Shader.Find("GUI/Text Shader"); 

    }
    void ColorSprite()
    {
        myRenderer.material.shader=myMaterial;
        myRenderer.color = color;
    }
    // Update is called once per frame
    void Update()
    {
        ColorSprite();
    }

    public void Finish()
    {
        gameObject.SetActive(false);
    }
}
