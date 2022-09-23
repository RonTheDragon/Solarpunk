using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    int water;
    GameObject Tree;
    // Start is called before the first frame update
    void Start()
    {
        water = 0;
        if (Tree == null)
        {
            Tree = transform.GetChild(0).gameObject;
            Tree.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (water > 30)
        {
            Tree.SetActive(true);
        }
    }

    public void GetWatered()
    {
        water++;
    }
}
