using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Shark : MonoBehaviour {

    [Header("Visual Effects")]
    public ParticleSystem visualEffect;

    [Header("Sound Effects")]
    public AudioSource onSpawn;
    public AudioSource reachLowerLimit;
    public AudioSource onClick;

    private float fallSpeed;
    private float maxSpeed;

    private int scoreCorrectPassed;

    private bool haveBeenClickedThrice = false;
    private int numberOfClicks = 0;
    private bool haveBeenScored = false;

    private WordManager wordManager;

    private GameObject exhaustedFish;

    void Start()
    {
        wordManager = GameObject.FindGameObjectWithTag("Word Manager").GetComponent<WordManager>();
        SetValuesFromWordManager();
        fallSpeed = maxSpeed + 0.3f;
        IEnumerable<GameObject> exhaustedFishes = GameObject.FindGameObjectsWithTag("Exhausted Fish").AsEnumerable();
        exhaustedFishes = exhaustedFishes.OrderBy(exhaustedFish => (exhaustedFish.transform.position - transform.position).sqrMagnitude);
        foreach (GameObject _exhaustedFish in exhaustedFishes)
        {
            if (_exhaustedFish.GetComponent<ExhaustedFish>().GetHunted() == false)
            {
                exhaustedFish = _exhaustedFish;
                exhaustedFish.GetComponent<ExhaustedFish>().SetHunted(true);
                break;
            }
        }
    }

    void Update()
    {
        transform.Translate(0f, -fallSpeed * Time.deltaTime, 0);
        if (exhaustedFish != null)
            if ((exhaustedFish.transform.position - transform.position).sqrMagnitude < 0.3f && haveBeenClickedThrice == false)
                Destroy(exhaustedFish);
        if (transform.position.y < -3.6f)
        {
            if (haveBeenClickedThrice == false && haveBeenScored == false)
            {
                wordManager.SetScore(scoreCorrectPassed);
                GetComponent<TextMeshProUGUI>().text = "";
                haveBeenClickedThrice = true;
                haveBeenScored = true;
                reachLowerLimit.Play();
                visualEffect.Stop();
                Destroy(gameObject, 2f);
            }
        }
    }

    private void SetValuesFromWordManager()
    {
        maxSpeed = wordManager.maxSpeed;
        scoreCorrectPassed = wordManager.scoreCorrectPassed;
    }

    private void OnMouseDown()
    {
        numberOfClicks++;
        if (numberOfClicks == 3 && haveBeenClickedThrice == false)
        {
            GetComponent<TextMeshProUGUI>().text = "";
            haveBeenClickedThrice = true;
            haveBeenScored = true;
            onClick.Play();
            visualEffect.Stop();
            Destroy(gameObject, 2f);
        }
    }
}
