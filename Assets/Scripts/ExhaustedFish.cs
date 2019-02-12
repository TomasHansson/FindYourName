using TMPro;
using UnityEngine;

public class ExhaustedFish : MonoBehaviour
{
    [Header("Sound Effects")]
    public AudioSource onSpawn;
    public AudioSource reachLowerLimit;
    public AudioSource onClick;

    private float fallSpeed;
    private float minSpeed;
    private float maxSpeed;

    private int scorePassed;

    private bool haveBeenClicked = false;
    private bool haveBeenScored = false;

    private WordManager wordManager;

    private bool hunted = false;

    void Start()
    {
        wordManager = GameObject.FindGameObjectWithTag("Word Manager").GetComponent<WordManager>();
        SetValuesFromWordManager();
        fallSpeed = Random.Range(minSpeed - 0.1f, maxSpeed - 0.1f);
    }

    void Update()
    {
        transform.Translate(0f, -fallSpeed * Time.deltaTime, 0);
        if (transform.position.y < -3.6f)
        {
            if (haveBeenScored == false)
            {
                wordManager.SetScore(scorePassed);
                GetComponent<TextMeshProUGUI>().text = "";
                haveBeenClicked = true;
                haveBeenScored = true;
                reachLowerLimit.Play();
                Destroy(gameObject, 2f);
            }
        }
    }

    private void SetValuesFromWordManager()
    {
        minSpeed = wordManager.minSpeed;
        maxSpeed = wordManager.maxSpeed;
        scorePassed = wordManager.scoreCorrectPress;
    }

    private void OnMouseDown()
    {
        if (haveBeenClicked == false)
        {
            haveBeenClicked = true;
            onClick.Play();
            fallSpeed += 0.3f;
        }
    }

    public void SetHunted(bool _hunted)
    {
        hunted = _hunted;
    }

    public bool GetHunted()
    {
        return hunted;
    }
}
