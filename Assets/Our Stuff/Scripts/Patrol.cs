using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField] float speed;
    private          float waitTime;
    [SerializeField] float startWaitTime;

    [SerializeField] Transform moveSpot;
    [SerializeField] float     minX;
    [SerializeField] float     maxX;
    [SerializeField] float     minY;
    [SerializeField] float     maxY;

    private void Start()
    {
        waitTime = startWaitTime;
        moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveSpot.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, moveSpot.position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                waitTime = startWaitTime;
            }

            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
}
