using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteCell : Cell
{
    public bool playerControl = true;
    public static List<WhiteCell> allWhiteCells = new List<WhiteCell>();
    public float speed;
    public float succForce = 2f;
    public float succRange;
    public GameObject attractionField;
    bool gettingInput = false;
    public LayerMask bacteriaLayer;
    private void Update()
    {
        if (playerControl)
        {
            GetInput();
        }      
        RadialDetection();
        ScrollCell();
    }
    void GetInput()
    {
        gettingInput = Input.GetKey(KeyCode.Mouse0);
        if (gettingInput)
        {
            Vector2 inputVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            inputVector -= (Vector2)transform.position;
            rb.AddForce(inputVector.normalized * speed);
        }
        bool gettingInput2 = Input.GetKey(KeyCode.Mouse1);
        if (gettingInput2)
        {
            SUCC();
        }
        attractionField.SetActive(gettingInput2);
    }
    public void SUCC()
    {
        Collider2D[] bacteriaInRange = Physics2D.OverlapCircleAll(transform.position, succRange, bacteriaLayer);
        for(int i = 0; i < bacteriaInRange.Length; i++)
        {
            Vector2 dir = transform.position - bacteriaInRange[i].transform.position;
            bacteriaInRange[i].GetComponent<Rigidbody2D>().AddForce(dir.normalized * succForce);
        }
       
    }
    public override void RadialDetection()
    {
        if (!gettingInput)
        {
            base.RadialDetection();
        }
        else
        {
            for (int i = 0; i < segments; i++)
            {
                Vector2 direction = GetDirection(a * i);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, visionDst, hittable);

                if (hit.transform != null)
                {
                    Debug.DrawLine(transform.position, hit.point, Color.red);
                    if (hit.transform.GetComponent<RedCell>() != null)
                    {
                        Vector2 d = direction;
                        d.y = 0;
                        hit.transform.GetComponent<Rigidbody2D>().AddForce(d.normalized * 1f);
                    }
                    else if (hit.transform.GetComponent<WhiteCell>() != null)
                    {
                        Vector2 dir = -direction;
                        dir.y = 0;
                        if (dir.y < 0)
                        {
                            dir.y = 0;
                        }
                        rb.AddForce(dir.normalized * 0.5f);
                    }


                }
                else
                {
                    Debug.DrawLine(transform.position, (Vector2)transform.position + (direction * visionDst), Color.green);
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, succRange);
    }
    public static void DeployAntibodies(GameObject antibody)
    {
        /*Vector2 mult = Vector2.zero;
        for(int i = 0; i < allWhiteCells.Count; i++)
        {
            mult += (Vector2)allWhiteCells[i].transform.position;
        }
        mult /= allWhiteCells.Count;
        Instantiate(antibody, mult, Quaternion.identity);*/
        for(int i = 0; i < allWhiteCells.Count; i++)
        {
            GameObject go = Instantiate(antibody, allWhiteCells[i].transform.position, allWhiteCells[i].transform.rotation);
            go.transform.SetParent(allWhiteCells[i].transform);
        }
    }
    private void OnEnable()
    {
        allWhiteCells.Add(this);
    }
    private void OnDisable()
    {
        allWhiteCells.Remove(this);
    }
}
