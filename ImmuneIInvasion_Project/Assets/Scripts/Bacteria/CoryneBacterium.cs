using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoryneBacterium : Bacteria
{
 
    [SerializeField]
    bool disperse = false;
    bool sticking = false;
    bool attracting = false;
    public Cooldown dispersionCooldown;
    public Cooldown attractionCooldown;
    public float attractionRange;
    public LayerMask attractable;
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
        if (!dead && !antiBodyDisable && attractionCooldown.CountDown())
        {
            attracting = !attracting;
        }
        if (antiBodyDisable)
        {
            sticking = false;
        }
        rb.isKinematic = sticking;
        if (sticking)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
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
        if (attracting)
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, attractionRange, attractable);
            if (cols.Length > 0)
            {
                for (int i = 0; i < cols.Length; i++)
                {
                    Vector2 dir = cols[i].transform.position - transform.position;
                    dir.Normalize();
                    cols[i].GetComponent<Rigidbody2D>().AddForce(dir * (GetComponent<Bacteria>() != null ? 0.08f : 0.3f));
                }
            }
        }
  
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!dead && !antiBodyDisable)
        {
            if (collision.transform.GetComponent<CoryneBacterium>() != null
                && collision.transform.GetComponent<CoryneBacterium>().sticking
                && !disperse
                || !disperse && collision.transform.GetComponent<Cell>() == null)
            {
                sticking = true;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attractionRange);
    }
}
