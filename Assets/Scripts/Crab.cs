using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Crab : FallingObject
{
    [Header("Visual Effects")]
    public ParticleSystem visualEffect;

    [Header("Sound Effects")]
    public AudioSource onSpawn;
    public AudioSource reachLowerLimit;
    public AudioSource onClick;

    public override void ReachedLowerLimit()
    {
        if (haveBeenClicked == false && haveBeenScored == false)
        {
            wordManager.AdjustScore(scoreMatchPassed);
            objectText.text = "";
            haveBeenClicked = true;
            haveBeenScored = true;
            reachLowerLimit.Play();
            visualEffect.Stop();
            Destroy(gameObject, 2f);
            IEnumerable<GameObject> activeMatches = GameObject.FindGameObjectsWithTag("Word").AsEnumerable();
            activeMatches = activeMatches.OrderBy(match => (match.transform.position - transform.position).sqrMagnitude);
            int matchesToRemove = 2;
            foreach (GameObject match in activeMatches)
            {
                if (matchesToRemove == 0 || activeMatches.Count() == 0)
                    break;
                if (match.GetComponent<Word>().GetHaveBeenClicked() == false && matchesToRemove > 0)
                {
                    Destroy(match);
                    matchesToRemove--;
                }
            }
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