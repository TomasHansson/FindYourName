using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Crab : MonoBehaviour {

    [Header("Visual Effects")]
    public ParticleSystem visualEffect;

    [Header("Sound Effects")]
    public AudioSource onSpawn;
    public AudioSource reachLowerLimit;
    public AudioSource onClick;

    private float fallSpeed;
    private float minSpeed;
    private float maxSpeed;

    private int scoreCorrectPress;
    private int scoreCorrectPassed;

    private bool haveBeenClicked = false;
    private bool haveBeenScored = false;

    private WordManager wordManager;

    void Start()
    {
        wordManager = GameObject.FindGameObjectWithTag("Word Manager").GetComponent<WordManager>();
        SetValuesFromWordManager();
        fallSpeed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        transform.Translate(0f, -fallSpeed * Time.deltaTime, 0);
        if (transform.position.y < -3.6f)
        {
            if (haveBeenClicked == false && haveBeenScored == false)
            {
                wordManager.SetScore(scoreCorrectPassed);
                GetComponent<TextMeshProUGUI>().text = "";
                haveBeenClicked = true;
                haveBeenScored = true;
                reachLowerLimit.Play();
                visualEffect.Stop();
                Destroy(gameObject, 2f);
                IEnumerable<GameObject> words = GameObject.FindGameObjectsWithTag("Word").AsEnumerable();
                words = words.OrderBy(word => (word.transform.position - transform.position).sqrMagnitude);
                int itemsToRemove = 2;
                foreach (GameObject word in words)
                {
                    if (itemsToRemove == 0 || words.Count() == 0)
                        break;
                    if (word.GetComponent<Word>().GetHaveBeenClicked() == false && itemsToRemove > 0)
                    {
                        Destroy(word);
                        itemsToRemove--;
                    }
                }
            }
        }
    }

    private void SetValuesFromWordManager()
    {
        minSpeed = wordManager.minSpeed;
        maxSpeed = wordManager.maxSpeed;
        scoreCorrectPress = wordManager.scoreCorrectPress;
        scoreCorrectPassed = wordManager.scoreCorrectPassed;
    }

    private void OnMouseDown()
    {
        if (haveBeenClicked == false)
        {
            wordManager.SetScore(scoreCorrectPress);
            GetComponent<TextMeshProUGUI>().text = "";
            haveBeenClicked = true;
            haveBeenScored = true;
            onClick.Play();
            visualEffect.Stop();
            Destroy(gameObject, 2f);
        }
    }
}
