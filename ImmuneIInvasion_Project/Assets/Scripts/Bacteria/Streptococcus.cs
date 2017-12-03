using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Streptococcus : Bacteria
{
    public bool disperse = true;
    public Cooldown dispersionCooldown;
    public override void Start()
    {
        base.Start();
        pullAction += HandleRadial;
    }
    public override void Update()
    {
        if (!dead && !antiBodyDisable && dispersionCooldown.CountDown())
        {
            disperse = !disperse;
        }
        base.Update();
    }
    void HandleRadial(Vector2 direction, RaycastHit2D hit)
    {
        if (hit.transform != null)
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
            Vector2 dir = -direction;

            if (!disperse)
            {
                dir.y = 0;
                if (hit.transform.GetComponent<Cell>() != null)
                {

                    if (dir.y < 0)
                    {
                        dir.y = 0;
                    }
                    rb.AddForce(direction.normalized * 0.15f);
                }
                else
                {
                    if (dir.y < 0)
                    {
                        dir.y = 0;
                    }
                    rb.AddForce(dir.normalized * 0.11f);
                }
            }
            else
            {
                if (hit.transform.GetComponent<RedCell>() != null)
                {
                    rb.AddForce(dir.normalized * 0.3f);
                }
            }


        }
        else
        {
            Debug.DrawLine(transform.position, (Vector2)transform.position + (direction * visionDst), Color.green);
        }
    }
}
