using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bacteria : Cell
{
    [Space(10)]
    [Header("Bacteria properties")]
    public static List<Bacteria> allBact = new List<Bacteria>();
    public GameObject offspring;
    public Cooldown disableCooldown;
    [HideInInspector]
    public bool dead = false;
    [HideInInspector]
    public bool antiBodyDisable = false;
    public Sprite disabledSprite;
    Sprite originalSprite;
    [HideInInspector]
    public float nutrition = 0;
    public float vitality = 5;
    RedCell currentCell;
    public float plusDamage = 1.7f;
    public float minusDamage = 3f;

    public delegate void PullAction(Vector2 direction, RaycastHit2D hit);
    public PullAction pullAction;

    public override void Start()
    {
        if (!allBact.Contains(this))
        {
            allBact.Add(this);
        }
        base.Start();
        originalSprite = GetComponent<SpriteRenderer>().sprite;
    }
    public virtual void Update()
    {
        if (!dead && !antiBodyDisable)
        {

            RadialDetection();
            ScrollCell();
        }
        if (antiBodyDisable && disableCooldown.CountDown())
        {
            antiBodyDisable = false;
        }
        GetComponent<SpriteRenderer>().sprite = antiBodyDisable ? disabledSprite : originalSprite;
    }
    public void DecreaseVitality(float rate = 11f)
    {
        vitality -= Time.deltaTime * rate;
        if (vitality <= 0)
        {
            dead = true;
            StartCoroutine(DeathSequence());
            vitality = 0;
        }
    }
    public virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (!dead && !antiBodyDisable)
        {
            if (collision.transform.GetComponent<RedCell>() != null)
            {
                /* if (!disperse && dispersionCooldown.CountDown())
                 {
                     disperse = !disperse;
                 }*/
                currentCell = collision.transform.GetComponent<RedCell>();
                currentCell.DecreaseVitality(minusDamage);
                nutrition += Time.deltaTime * plusDamage;
                if (nutrition >= 5f)
                {
                    Duplicate();
                    nutrition = 0;
                }
            }
        }
        if (!dead)
        {
            if (collision.transform.GetComponent<WhiteCell>() != null)
            {
                DecreaseVitality();
            }
        }
    }
    public virtual void Duplicate()
    {
        Instantiate(offspring, transform.position, transform.rotation);
    }
    private void OnEnable()
    {
        allBact.Add(this);
    }
    private void OnDisable()
    {
        if (FindObjectOfType<GameManager>() != null)
        {
            GameManager.IncrementAntibodyMeter();
        }
        allBact.Remove(this);
    }
    public void Disable()
    {
        antiBodyDisable = true;
    }
    public override void RadialDetection()
    {
        for (int i = 0; i < segments; i++)
        {
            Vector2 direction = GetDirection(a * i);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, visionDst, hittable);
            pullAction(direction, hit);         
        }

    }
}
