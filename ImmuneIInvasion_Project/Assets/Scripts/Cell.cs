using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cell : MonoBehaviour
{
    Transform start;
    Transform end;
    public int segments;
    public float visionDst;
    public LayerMask hittable;
    [HideInInspector]
    public Rigidbody2D rb;
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        start = GameManager.GetStart;
        end = GameManager.GetEnd;
    }
    public virtual void ScrollCell()
    {
        if (transform.position.y > end.position.y)
        {
            Vector2 pos = transform.position;
            pos.y = start.position.y + 0.1f;
            transform.position = pos;
        }

    }
    public virtual void RadialDetection()
    {
        for (int i = 0; i < segments; i++)
        {
            Vector2 direction = GetDirection(a * i);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, visionDst, hittable);

            if (hit.transform != null)
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);
                Vector2 dir = -direction;
                dir.y = 0;
                if (hit.transform.GetComponent<Cell>() != null)
                {

                    if (dir.y < 0)
                    {
                        dir.y = 0;
                    }
                    rb.AddForce(dir.normalized * 0.5f);
                }
                else
                {
                    if (dir.y < 0)
                    {
                        dir.y = 0;
                    }
                    rb.AddForce(dir.normalized * 0.7f);
                }

            }
            else
            {
                Debug.DrawLine(transform.position, (Vector2)transform.position + (direction * visionDst), Color.green);
            }
        }
    }
    public Vector2 GetDirection(float angle)
    {
        float x = Mathf.Sin(Mathf.Deg2Rad * ((angle) - transform.rotation.eulerAngles.z));
        float y = Mathf.Cos(Mathf.Deg2Rad * ((angle) - transform.rotation.eulerAngles.z));
        return new Vector2(x, y);
    }
    public float a
    {
        get
        {
            return 360f / segments;
        }
    }
    public IEnumerator DeathSequence(float speed = 1f)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if(renderer.color.a < 0.1f)
        {
            if(transform.childCount > 0)
                renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
        Color c = renderer.color;
        Color inv = renderer.color;
        inv.a = 0;
        float af = 1f;
        while (renderer.color.a > 0.01f)
        {
            af -= Time.deltaTime * speed;
            if (af <= 0)
            {
                af = 0;
            }
            c.a = af;
            renderer.color = c;
            yield return null;
        }
        Destroy(gameObject);
    }
}
