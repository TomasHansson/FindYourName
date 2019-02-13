using UnityEngine;
using TMPro;

public class Word : MonoBehaviour {

    [Header("Visual Effects")]
    public ParticleSystem playOnDestory;

    [Header("Sound Effects")]
    public AudioSource clickCorrect;
    public AudioSource passCorrect;
    public AudioSource clickIncorrect;

    private float fallSpeed;
    private float minSpeed;
    private float maxSpeed;

    private int scoreCorrectPress;
    private int scoreIncorrectPress;
    private int scoreIncorrectPassed;
    private int scoreCorrectPassed;

    private bool haveBeenClicked = false;
    private bool haveBeenScored = false;

    private TextMeshProUGUI word;
    private WordManager wordManager;

    void Start () {
        wordManager = GameObject.FindGameObjectWithTag("Word Manager").GetComponent<WordManager>();
        SetValuesFromWordManager();
        word = GetComponent<TextMeshProUGUI>();
        fallSpeed = Random.Range(minSpeed, maxSpeed);
    }
	
	void Update () {
        transform.Translate(0f, -fallSpeed * Time.deltaTime, 0);
        if (transform.position.y < -3.6f)
        {
            if (word.text != wordManager.GetCurrentName() && haveBeenClicked == false && haveBeenScored == false)
            {
                wordManager.AdjustScore(scoreIncorrectPassed);
                word.text = "";
                haveBeenClicked = true;
                haveBeenScored = true;
                playOnDestory.Play();
                passCorrect.Play();
                Destroy(gameObject, 2f);
            }
            else if (word.text == wordManager.GetCurrentName() && haveBeenClicked == false)
            {
                wordManager.AdjustScore(scoreCorrectPassed);
                word.text = "";
                haveBeenClicked = true;
                clickIncorrect.Play();
                var main = playOnDestory.main;
                main.startColor = Color.black;
                playOnDestory.Play();
                Destroy(gameObject, 2f);
            }
            else if (word.text != wordManager.GetCurrentName() && haveBeenClicked == true)
            {
                word.text = "";
                Destroy(gameObject, 2f);
            }
        }
	}

    private void SetValuesFromWordManager()
    {
        minSpeed = wordManager.minSpeed;
        maxSpeed = wordManager.maxSpeed;
        scoreCorrectPress = wordManager.scoreCorrectPress;
        scoreIncorrectPress = wordManager.scoreIncorrectPress;
        scoreIncorrectPassed = wordManager.scoreIncorrectPassed;
        scoreCorrectPassed = wordManager.scoreCorrectPassed;
    }

    private void OnMouseDown()
    {
        if (word.text == wordManager.GetCurrentName() && haveBeenClicked == false)
        {
            wordManager.AdjustScore(scoreCorrectPress);
            haveBeenClicked = true;
            haveBeenScored = true;
            playOnDestory.Play();
            clickCorrect.Play();
            word.text = "";
            Destroy(gameObject, 2f);
        }
        else if (haveBeenClicked == false)
        {
            wordManager.AdjustScore(scoreIncorrectPress);
            haveBeenClicked = true;
            haveBeenScored = true;
            word.color = Color.black;
            clickIncorrect.Play();
        }
    }

    public bool GetHaveBeenClicked()
    {
        return haveBeenClicked;
    }
}
