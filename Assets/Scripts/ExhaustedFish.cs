using UnityEngine;

public class ExhaustedFish : FallingObject
{
    [Header("Sound Effects")]
    public AudioSource onSpawn;
    public AudioSource reachLowerLimit;
    public AudioSource onClick;

    public override void Start()
    {
        base.Start();
        fallSpeed -= 0.1f;
    }

    public override void ReachedLowerLimit()
    {
        if (!haveBeenScored)
        {
            wordManager.AdjustScore(scoreMatchPassed);
            objectText.text = "";
            haveBeenClicked = true;
            haveBeenScored = true;
            reachLowerLimit.Play();
            Destroy(gameObject, 2f);
        }
    }

    public override void SetValuesFromWordManager()
    {
        minSpeed = wordManager.minSpeed;
        maxSpeed = wordManager.maxSpeed;
        scoreMatchPassed = wordManager.scoreCorrectPress;
    }

    public override void OnMouseDown()
    {
        if (!haveBeenClicked)
        {
            haveBeenClicked = true;
            onClick.Play();
            fallSpeed += 0.3f;
        }
    }
}