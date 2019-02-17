using UnityEngine;

public class Match : FallingObject
{
    [Header("Visual Effects")]
    public ParticleSystem playOnDestory;

    [Header("Sound Effects")]
    public AudioSource onClick;
    public AudioSource reachLowerLimit;

    public override void OnMouseDown()
    {
        if (!haveBeenClicked)
        {
            wordManager.AdjustScore(scoreMatchClicked);
            playOnDestory.Play();
            haveBeenClicked = true;
            objectText.text = "";
            onClick.Play();
            Destroy(gameObject, 2f);
        }
    }

    public override void ReachedLowerLimit()
    {
        if (!haveBeenClicked)
        {
            wordManager.AdjustScore(scoreMatchPassed);
            ParticleSystem.MainModule mainModule = playOnDestory.main;
            mainModule.startColor = Color.black;
            playOnDestory.Play();
            haveBeenClicked = true;
            objectText.text = "";
            reachLowerLimit.Play();
            Destroy(gameObject, 2f);
        }
    }

    public override void SetValuesFromWordManager()
    {
        minSpeed = wordManager.minSpeed;
        maxSpeed = wordManager.maxSpeed;
        scoreMatchClicked = wordManager.scoreMatchClicked;
        scoreMatchPassed = wordManager.scoreMatchPassed;
    }

    public bool GetHaveBeenClicked()
    {
        return haveBeenClicked;
    }
}