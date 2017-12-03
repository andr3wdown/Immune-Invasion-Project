using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCell : Cell
{
    const float MAX_VITALITY = 30f;
    public static List<RedCell> allRedCells = new List<RedCell>();
    float vitality = 30f;
    public float minAcc, maxAcc;
    public Cooldown moveCooldown;
    bool isDead = false;
    public Color deathColor;
    [SerializeField]
    Color originalColor;
    public override void Start()
    {
        base.Start();
        originalColor = GetComponent<SpriteRenderer>().color;
    }
    private void Update()
    {
        if (!isDead)
        {
            if (moveCooldown.CountDown())
            {
                RandomMovement();
            }
            RadialDetection();          
        }
   
        ScrollCell();
    }
   
	void RandomMovement()
    {
        Vector2 movementVector = new Vector2(Random.Range(-1f, 1f), Random.Range(0f, 0.3f));
        movementVector.Normalize();
        rb.AddForce(movementVector * Random.Range(minAcc * 10, maxAcc * 10));
    }
    public void DecreaseVitality(float rate = 3f)
    {
        vitality -= Time.deltaTime * rate;
        Color c = Color.Lerp(originalColor, deathColor, 1f - vitality / MAX_VITALITY);
        GetComponent<SpriteRenderer>().color = c;
        if(vitality <= 0)
        {
            if (!isDead)
                StartCoroutine(DeathSequence());

            isDead = true;
            
            vitality = 0;
        }
    }
    private void OnEnable()
    {
        allRedCells.Add(this);
    }
    private void OnDisable()
    {
        allRedCells.Remove(this);
    }
}
