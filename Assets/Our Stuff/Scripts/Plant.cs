using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    int water;
    GameObject DeadTree;
    GameObject GoodTree;
    // Start is called before the first frame update
    void Start()
    {
        water = 0;
        if (DeadTree == null)
        {
            DeadTree = transform.GetChild(0).gameObject;
            GoodTree = transform.GetChild(1).gameObject;
            DeadTree.SetActive(true);
            GoodTree.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (water > 30)
        {
            DeadTree.SetActive(false);
            GoodTree.SetActive(true);
        }
    }

    public void GetWatered()
    {
        water++;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            water = 0;
        }
    }
}
