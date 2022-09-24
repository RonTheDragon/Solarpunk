using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] int water;
    public int WaterNeeded = 30;
    bool TreeFullGrown;

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
        if (water > WaterNeeded && !TreeFullGrown)
        {
            DeadTree.SetActive(false);
            GoodTree.SetActive(true);
            GameManager.instance.TreesGrown++;
            TreeFullGrown = true;
        }
    }

    public void GetWatered()
    {
        if (!TreeFullGrown)
        water++;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy" && !TreeFullGrown)
        {
            water = 0;
        }
    }
}
