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

    [SerializeField] bool CustomPatrol;
    [SerializeField] Transform[] Positions;

    Vector3 StartPosition;

    private void Start()
    {
        StartPosition = transform.position;
        waitTime = startWaitTime;
        Move();
    }

    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveSpot.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, moveSpot.position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                Move();
                waitTime = startWaitTime;
            }

            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    void Move()
    {
        if (CustomPatrol)
        {
            moveSpot.position = Positions[Random.Range(0, Positions.Length)].position;
        }
        else
        moveSpot.position = new Vector2(StartPosition.x+Random.Range(minX, maxX), StartPosition.y + Random.Range(minY, maxY));
    }
}
