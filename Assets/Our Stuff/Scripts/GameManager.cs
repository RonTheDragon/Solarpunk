using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int TreesGrown;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Time.timeScale = 0;
    }
}
