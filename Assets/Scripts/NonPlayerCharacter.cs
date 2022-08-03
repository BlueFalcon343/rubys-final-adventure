using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NonPlayerCharacter : MonoBehaviour
{
    public float displayTime = 4.0f;
    public GameObject dialogBox;
    public GameObject winBox;
    
    private int robotCount;
    float timerDisplay;

    // Start is called before the first frame update
    void Start()
    {
       dialogBox.SetActive(false);
       winBox.SetActive(false);
       timerDisplay = -1.0f; 
       
       
    }

    // Update is called once per frame
    void Update()
    {
        robotCount = EnemyController.count;
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
                winBox.SetActive(false);
            }
        }
    }

    public void DisplayDialog()
    {
        timerDisplay = displayTime;
        if (robotCount < 8)
        {
            dialogBox.SetActive(true);
        }
        if (robotCount >= 8)
        {
            winBox.SetActive(true);
            StartCoroutine(Teleport());
        }
    }

    IEnumerator Teleport()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("SecondScene");
    }    
}
