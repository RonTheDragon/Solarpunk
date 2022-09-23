using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasCloud : MonoBehaviour
{
    public int MaxHp;
    int Hp;

    public GameObject TheCloud;

    public float RespawnCooldown;

    float respawnTimeLeft;

    Vector2 StartingScale;

    // Start is called before the first frame update
    void Start()
    {
        Hp = MaxHp;
        StartingScale = TheCloud.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Hp < 0) { TheCloud.SetActive(false); respawnTimeLeft = RespawnCooldown;  Hp = MaxHp; }
        if (respawnTimeLeft > 0) { respawnTimeLeft -= Time.deltaTime; } else if (TheCloud.activeSelf==false)
        {
            TheCloud.SetActive(true);
            TheCloud.transform.localScale = StartingScale;
        }
    }

    public void Sucked()
    {
        Hp--;
        TheCloud.transform.localScale -= TheCloud.transform.localScale * 0.1f;
    }

  
}
