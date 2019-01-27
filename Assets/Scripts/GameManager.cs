using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region Timers

    public int clockTimer;
    public float currentTime;
    public int endTime;
    public float timer;
    public float secondsPassedTimer;
    #endregion

    public float sanity = 100;
    public float paranoia = 100;

    public Image bG;

    public Image sanityBar;
    public Image paranoiaBar;

    public GameObject bed;

    public Animator bedAnimator;
    public Animation[] dealAnimations;

    public Animator blackScreen;

    public TMP_Text paranoiaTimer;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    
    void Start()
    {
        currentTime = 0f;
        endTime = 600;
    }

    void Update()
    {
        timer += Time.deltaTime;
        secondsPassedTimer = currentTime;
        currentTime = Mathf.RoundToInt(timer % 60);
        paranoiaTimer.text = string.Format("Time Remaining: \n{0} / {1} ", currentTime, endTime);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            sanity -= 100;
        }

        if(Input.GetKey(KeyCode.Alpha0))
        {
            paranoiaTimer.gameObject.SetActive(true);
        }
        else
        {
            paranoiaTimer.gameObject.SetActive(false);

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            paranoia += 100;
        }

        if(secondsPassedTimer < currentTime)
        {
            paranoia -= 0.5f;
        }

        secondsPassedTimer = currentTime - 1;

        UpdateBars();

        EndGame();
        //Debug.Log(string.Format("Seconds passed: {0}", currentTime));

    }

    void EndGame()
    {
        //If you've survived for 5 minutes, end the game.
        if (currentTime >= endTime)
        {
            StartCoroutine(fadeScreen(1));
        }

        if (paranoia >= 100)
        {
            StartCoroutine(fadeScreen(2));
            Debug.Log("Oh no paranoia");
        }

        if (sanity <= 0)
        {
            StartCoroutine(fadeScreen(3));
            Debug.Log("Oh no sanity");
        }
    }

    void UpdateBars()
    {
        sanityBar.fillAmount = Mathf.InverseLerp(100, 0, sanity);
        paranoiaBar.fillAmount = Mathf.InverseLerp(0, 100, paranoia);

    }

    public void TakeDamage(int damage)
    {
        sanity -= damage;
    }

    IEnumerator fadeScreen(int endScreen)
    {
        blackScreen.Play("FadeAnim");

        yield return new WaitForSeconds(1.2f);

        SceneManager.LoadScene(endScreen);

    }
}
