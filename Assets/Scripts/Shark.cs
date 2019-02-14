using UnityEngine;

public class Shark : FallingObject
{
    [Header("Visual Effects")]
    public ParticleSystem visualEffect;

    [Header("Sound Effects")]
    public AudioSource onSpawn;
    public AudioSource reachLowerLimit;
    public AudioSource onClick;

    private bool haveBeenClickedThrice;
    private int numberOfClicks;

    private GameObject exhaustedFish;

    public override void Start()
    {
        base.Start();
        fallSpeed = maxSpeed + 0.3f;
    }

    public void SetPrey(GameObject _exhaustedFish)
    {
        exhaustedFish = _exhaustedFish;
    }

    public override void Update()
    {
        transform.Translate(0f, -fallSpeed * Time.deltaTime, 0);
        if (exhaustedFish != null)
            if ((exhaustedFish.transform.position - transform.position).sqrMagnitude < 0.3f && haveBeenClickedThrice == false)
                Destroy(exhaustedFish);
        if (transform.position.y < -3.6f)
            ReachedLowerLimit();
    }


    public override void ReachedLowerLimit()
    {
        if (haveBeenClickedThrice == false && haveBeenScored == false)
        {
            wordManager.AdjustScore(scoreMatchPassed);
            objectText.text = "";
            haveBeenClickedThrice = true;
            haveBeenScored = true;
            reachLowerLimit.Play();
            visualEffect.Stop();
            Destroy(gameObject, 2f);
        }
    }

    public override void SetValuesFromWordManager()
    {
        maxSpeed = wordManager.maxSpeed;
        scoreMatchPassed = wordManager.scoreCorrectPassed;
    }

    public override void OnMouseDown()
    {
        numberOfClicks++;
        if (numberOfClicks == 3 && haveBeenClickedThrice == false)
        {
            objectText.text = "";
            haveBeenClickedThrice = true;
            haveBeenScored = true;
            onClick.Play();
            visualEffect.Stop();
            Destroy(gameObject, 2f);
        }
    }
}
