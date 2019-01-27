using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    Animator animator;

    string myName;

    float timer = 0;

    int health = 100;

    SpriteRenderer mySprite;

    AudioSource myAudio;

    #region Change in inspector
    public int sanityDamage;

    public float threat;

    public float minThreatPerRate;
    public float maxThreatPerRate;

    public float threatRate = 5.5f;
    public float sanityRate = 0.2f;
    #endregion

    float defeatTimer = 7;

    void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
        myAudio = GetComponent<AudioSource>();
        myName = gameObject.name;
        threat = 0;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > threatRate)
        {
            threat += Mathf.RoundToInt(Random.Range(minThreatPerRate, maxThreatPerRate));
            timer = 0;

            //Debug.Log(string.Format("Threat is currently at {0}", threat));
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("THREAT 4"))
        {
            GameManager.instance.sanity -= sanityRate;
            
        }

        if(health <= 10)
        {
            StartCoroutine(SetHealthBack());
            threat = 0;
        }

        //After an X amount of time generate threat

        //After reaching a threat threshold execute

        //If clicked on and a minor amount of threat is generated you are considered 

        //PARANOID!!!!!!!!

        animator.SetFloat("Threat", threat);
        animator.SetInteger("Health", health);

    }

    void OnMouseDown()
    {

        if(threat == 100)
        {
            GameManager.instance.TakeDamage(sanityDamage);
        }

        if(threat <= 40)
        {
            GameManager.instance.paranoia += 20;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("THREAT 4"))
        {
            health -= 10;
            StartCoroutine(DamageFlasher());
        }

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("THREAT 4"))
        {
            DealWithYourParanoia();
        }

    }

    public void DealWithYourParanoia()
    {
        threat = 0;

        GameManager.instance.bedAnimator.SetTrigger("Deal with " + myName);
    }

    IEnumerator DamageFlasher()
    {
        mySprite.color = Color.red;
        yield return null;
        mySprite.color = Color.white;

    }

    IEnumerator SetHealthBack()
    {
        yield return new WaitForSeconds(defeatTimer);
        health = 100;
    }

    public void playAudio()
    {
        myAudio.PlayOneShot(myAudio.clip);
    }

    public void stopAudio()
    {
        myAudio.Stop();

    }
}
