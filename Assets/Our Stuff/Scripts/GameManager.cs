using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public        int         TreesGrown;
    public        int         TreesTotal;
    public GameObject Canvas;

    [SerializeField] GameObject winMenu;
    [SerializeField] TMP_Text treesCounter;

    private void Awake()
    {
        instance = this;
        Canvas.SetActive(true);
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
        treesCounter.text = TreesGrown + "/" + TreesTotal;
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
