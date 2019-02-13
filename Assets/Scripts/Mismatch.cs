using UnityEngine;

public class Mismatch : FallingObject
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
            wordManager.AdjustScore(scoreMisMatchClicked);
            haveBeenClicked = true;
            objectText.color = Color.black;
            onClick.Play();
        }
    }

    public override void ReachedLowerLimit()
    {
        if (!haveBeenClicked)
        {
            wordManager.AdjustScore(scoreMisMatchPassed);
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
        scoreMisMatchClicked = wordManager.scoreIncorrectPress;
        scoreMisMatchPassed = wordManager.scoreIncorrectPassed;
    }
}