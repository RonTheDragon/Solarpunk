using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int TreesGrown;
    public int TreesTotal;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Time.timeScale = 0;
    }

    private void Update()
    {
        if (TreesTotal == TreesGrown)
        {
            Win();
        }
    }

    public void TreeGrown()
    {
        TreesGrown++;
        UpdateTreesUI();
    }

    public void AddTree()
    {
        TreesTotal++;
        UpdateTreesUI();
    }

    void UpdateTreesUI()
    {

    }

    void Win()
    {
        //Open Menu
        Time.timeScale = 0;
    }
}
