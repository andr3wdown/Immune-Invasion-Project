using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staphylococcus : Bacteria
{
    [SerializeField]
    bool disperse = false;
    bool sticking = false;
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
            if (disperse)
            {
                sticking = false;
            }
        }
        if (antiBodyDisable)
        {
            sticking = false;
        }
        rb.isKinematic = sticking;
        if (sticking)
        {
            rb.velocity = Vector2.zero;
        }
        base.Update();
    }
    public void HandleRadial(Vector2 direction, RaycastHit2D hit)
    {
        
        if (hit.transform != null)
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);      
            Vector2 dir = -direction;
            if (!sticking)
            {
                if (!disperse)
                {
                    dir.y = 0;
                    if (hit.transform.GetComponent<Cell>() == null)
                    {

                        if (dir.y < 0)
                        {
                            dir.y = 0;
                        }
                        rb.AddForce(direction.normalized * 0.2f);
                    }
                    else
                    {
                        if (dir.y < 0)
                        {
                            dir.y = 0;
                        }
                        rb.AddForce(dir.normalized * 0.2f);
                    }
                }
                else
                {
                    if (hit.transform.GetComponent<RedCell>() != null)
                    {
                        rb.AddForce(direction.normalized * 0.4f);
                    }
                    if (hit.transform.GetComponent<Cell>() == null)
                    {

                        if (dir.y < 0)
                        {
                            dir.y = 0;
                        }
                        rb.AddForce(dir.normalized * 0.2f);
                    }
                }
            }
        }
        else
        {
            Debug.DrawLine(transform.position, (Vector2)transform.position + (direction * visionDst), Color.green);
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(!dead && !antiBodyDisable)
        {
            if(collision.transform.GetComponent<Staphylococcus>() != null 
                && collision.transform.GetComponent<Staphylococcus>().sticking 
                && !disperse 
                || !disperse && collision.transform.GetComponent<Cell>() == null)
            {
                sticking = true;
            }
        }
    }
}
