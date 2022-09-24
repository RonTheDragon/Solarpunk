using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public        int         TreesGrown;
    public        int         TreesTotal;

    [SerializeField] GameObject winMenu;

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

    public void AddTreeToTotal()
    {
        TreesTotal++;
        UpdateTreesUI();
    }

    void UpdateTreesUI()
    {
        //display X/Y
        //X is trees grown
        //Y is the trees total

    }

    void Win()
    {
        winMenu.SetActive(true);
        Time.timeScale = 0;
    }
}
