using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class Shadows : MonoBehaviour
{
    public static Shadows me;
    public GameObject ShadownPrefab;
    public List<GameObject> pool=new List<GameObject>();
    private float counter;
    public float speed;
    //public Color color;

    private void Awake()
    {
        me = this;
    }

    public GameObject GetShadows()
    {
        for (int i = 0;  i < pool.Count;i++)
        {
            if(!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                pool[i].transform.position = transform.position;
                //pool[i].transform.rotation = transform.rotation;
                pool[i].GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
                pool[i].GetComponent<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX;
                //pool[i].GetComponent<Solid>().color = color;
                return pool[i];
            } 
        }
        GameObject obj = Instantiate(ShadownPrefab, transform.position, transform.rotation) as GameObject;
        obj.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
        //obj.GetComponent<Solid>().color = color;
        pool.Add(obj);
        return obj;
    }

    public void ActiveShadowEffect()
    {
        counter+=speed*Time.deltaTime;
        if(counter>1)
        {
            GetShadows();
            counter = 0;
        }
    }
}
