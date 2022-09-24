using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] int water;
    public int WaterNeeded = 30;
    bool TreeFullGrown;

    GameObject Tree;
    SpriteRenderer TreeSprite;
    public Sprite[] sprites;
    // Start is called before the first frame update


    void Start()
    {
        GameManager.instance.AddTreeToTotal();
        water = 0;
        if (Tree == null)
        {
            Tree = transform.GetChild(0).gameObject;
            TreeSprite = Tree.GetComponent<SpriteRenderer>();
            TreeSprite.sprite = sprites[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (water >= WaterNeeded && !TreeFullGrown)
        {
          //  DeadTree.SetActive(false);
            //GoodTree.SetActive(true);
            GameManager.instance.TreeGrown();
            TreeFullGrown = true;
            TreeSprite.sprite = sprites[sprites.Length-1];
        }
    }

    public void GetWatered()
    {
        if (!TreeFullGrown)
        water++;
        UpdateTreeSprite();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy" && !TreeFullGrown)
        {
            water = 0;
            UpdateTreeSprite();
        }
    }

    void UpdateTreeSprite()
    {
        Debug.Log(water);
        int wot = 0;
        for (int i = 0; i < sprites.Length-1; i++)
        {
            if (water <= wot)
            {
                TreeSprite.sprite = sprites[i];
                return;
            }
            else
            {
                wot += (WaterNeeded / sprites.Length);
            }
        }
    }
}
