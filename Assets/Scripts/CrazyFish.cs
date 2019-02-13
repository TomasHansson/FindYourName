using UnityEngine;

public class CrazyFish : FallingObject
{
    [Header("Visual Effects")]
    public ParticleSystem visualEffect;

    [Header("Sound Effects")]
    public AudioSource onSpawn;
    public AudioSource reachLowerLimit;
    public AudioSource onClick;

    public override void ReachedLowerLimit()
    {
        if (!haveBeenClicked && !haveBeenScored)
        {
            wordManager.AdjustScore(scoreMatchPassed);
            objectText.text = "";
            haveBeenClicked = true;
            haveBeenScored = true;
            reachLowerLimit.Play();
            visualEffect.Stop();
            Destroy(gameObject, 2f);
            GameObject[] activeMatches = GameObject.FindGameObjectsWithTag("Word");
            if (activeMatches != null)
                Destroy(activeMatches[Random.Range(0, activeMatches.Length)]);
        }
    }

    public override void SetValuesFromWordManager()
    {
        minSpeed = wordManager.minSpeed;
        maxSpeed = wordManager.maxSpeed;
        scoreMatchClicked = wordManager.scoreCorrectPress;
        scoreMatchPassed = wordManager.scoreCorrectPassed;
    }

    public override void OnMouseDown()
    {
        if (!haveBeenClicked)
        {
            wordManager.AdjustScore(scoreMatchClicked);
            objectText.text = "";
            haveBeenClicked = true;
            haveBeenScored = true;
            onClick.Play();
            visualEffect.Stop();
            Destroy(gameObject, 2f);
        }
    }
}