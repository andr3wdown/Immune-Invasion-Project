using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int ANTIBODY_MAX = 50;
    public Text redCellCounter;
    public Text bacteriaCounter;
    public Image antibodyMeter;
    public Transform start;
    public Transform end;
    static GameManager instance;
    public GameObject antibody;
    int antibodyMeterCount = 0;
    public GameObject antibodiesMessage;
    public GameObject endScreen;
    public GameObject retry;
    public GameObject next;
    public Text endText;
    public Text endDesc;
    public bool mainMenu = false;
    bool stopped = false;
    float startTimer = 2f;
    public Image hpBar;
    public Image infectionBar;
    void OnEnable()
    {
        RedCell.allRedCells = new List<RedCell>();
        WhiteCell.allWhiteCells = new List<WhiteCell>();
        Bacteria.allBact = new List<Bacteria>();
        instance = this; 
        Time.timeScale = 1;
    }
    private void Update()
    {
        
        if (!mainMenu)
        {
            float ratio = (float)(RedCell.allRedCells.Count - 50) / (float)(108 - 50);
            hpBar.fillAmount = ratio;
            ratio = (float)Bacteria.allBact.Count / 300f;
            infectionBar.fillAmount = ratio;
            redCellCounter.text = "Until critical: " + (RedCell.allRedCells.Count - 50 < 0 ? 0 : RedCell.allRedCells.Count - 50).ToString();
            bacteriaCounter.text = "Infection: " + Mathf.Round(ratio * 100) + "%";
            bool bodiesReady = antibodyMeterCount >= ANTIBODY_MAX;
            antibodiesMessage.SetActive(bodiesReady);
            if (bodiesReady && Input.GetKeyDown(KeyCode.Space))
            {
                antibodyMeterCount = 0;
                WhiteCell.DeployAntibodies(antibody);
                antibodyMeter.fillAmount = (float)antibodyMeterCount / (float)ANTIBODY_MAX;
            }
            startTimer -= Time.deltaTime * 0.2f;
            if (RedCell.allRedCells.Count <= 50 && !stopped && startTimer <= 0)
            {
                stopped = true;
                
                StartCoroutine(EndSequence(false));
                //Time.timeScale = 0;
            }
            if(Bacteria.allBact.Count <= 0 && !stopped && startTimer <= 0)
            {
                stopped = true;
                
                StartCoroutine(EndSequence(true));
                //Time.timeScale = 0;
            }
        }
    
    }
    IEnumerator EndSequence(bool victory)
    {      
        
        for(int i = 0; i < WhiteCell.allWhiteCells.Count; i++)
        {
            WhiteCell.allWhiteCells[i].playerControl = false;
        }
        yield return new WaitForSeconds(3f);
        Time.timeScale = 0;
        endScreen.SetActive(true);
        if (SceneManager.GetActiveScene().buildIndex + 1 > 3)
        {
            next.SetActive(false);
            retry.SetActive(!victory);
            endText.text = victory ? "You beat the game!" : "You Lost!";
            endDesc.text = victory ? "Congratulations! Thanks for playing!" : "Amount of red cells fell to a critical level!";
        }
        else
        {
            next.SetActive(victory);
            retry.SetActive(!victory);
            endText.text = victory ? "You Won!" : "You Lost!";
            endDesc.text = victory ? "You eliminated all of the bacteria!" : "Amount of red cells fell to a critical level!";
        }
        
        
    }
    public static Transform GetStart
    {
        get
        {
            return instance.start;
        }
    }
    public static Transform GetEnd
    {
        get
        {
            return instance.end;
        }
    }
    public static void IncrementAntibodyMeter()
    {
        instance.antibodyMeterCount++;
        if(instance.antibodyMeterCount > instance.ANTIBODY_MAX)
        {
            instance.antibodyMeterCount = instance.ANTIBODY_MAX;
        }
        if(instance != null && instance.antibodyMeter != null)
        {
            instance.antibodyMeter.fillAmount = (float)instance.antibodyMeterCount / (float)instance.ANTIBODY_MAX;
        }
 
    }


}
