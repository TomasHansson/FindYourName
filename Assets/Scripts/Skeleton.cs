using UnityEngine;

public class Skeleton : FallingObject
{
    [Header("Visual Effects")]
    public ParticleSystem visualEffect;

    [Header("Sound Effects")]
    public AudioSource onSpawn;
    public AudioSource reachLowerLimit;
    public AudioSource onClick;

    public override void ReachedLowerLimit()
    {
        if (!haveBeenClicked)
        {
            wordManager.AdjustScore(scoreMatchPassed);
            objectText.text = "";
            haveBeenClicked = true;
            reachLowerLimit.Play();
            visualEffect.Stop();
            Destroy(gameObject, 2f);
            GameObject[] activeMatches = GameObject.FindGameObjectsWithTag("Match");
            foreach (GameObject match in activeMatches)
                if (match.transform.position.y > this.transform.position.y && match.GetComponent<Match>().GetHaveBeenClicked() == false)
                    Destroy(match);
        }
    }

    public override void SetValuesFromWordManager()
    {
        minSpeed = wordManager.minSpeed;
        maxSpeed = wordManager.maxSpeed;
        scoreMatchClicked = wordManager.scoreMatchClicked;
        scoreMatchPassed = wordManager.scoreMatchPassed;
    }

    public override void OnMouseDown()
    {
        if (!haveBeenClicked)
        {
            wordManager.AdjustScore(scoreMatchClicked);
            objectText.text = "";
            haveBeenClicked = true;
            onClick.Play();
            visualEffect.Stop();
            Destroy(gameObject, 2f);
        }
    }
}