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

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GetComponent<AudioManager>();
        instance = this;
        Canvas.SetActive(true);
    }
    void Start()
    {
        Time.timeScale = 0;
        audioManager.PlaySound(Sound.Activation.Custom, "Main Theme");
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
    }

    void Win()
    {
        winMenu.SetActive(true);
        Time.timeScale = 0;
    }
}
