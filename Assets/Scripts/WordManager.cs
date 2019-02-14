using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WordManager : MonoBehaviour {

    [Header("Background Music")]
    public AudioSource backGroundMusic;

    [Header("Text Elements")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI goalText;
    public TextMeshProUGUI scoreValuesText;
    public TextMeshProUGUI objectsLeftText;
    public TextMeshProUGUI numberOfObjectsText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI currentNameText;

    [Header("Spawnable Prefabs")]
    public GameObject namePrefab;
    public GameObject skeletonPrefab;
    public GameObject crabPrefab;
    public GameObject crazyFishPrefab;
    public GameObject exFishPrefab;
    public GameObject sharkPrefab;

    [Header("Spawnlocations")]
    public Transform[] spawnLocations;

    [Header("Set Level Variables")]
    public int numberOfObjects;
    public int activateSpawnLocations;
    public int goal;
    public int levelNr;
    public float minSpeed;
    public float maxSpeed;
    public int scoreCorrectPress;
    public int scoreIncorrectPress;
    public int scoreIncorrectPassed;
    public int scoreCorrectPassed;

    [Header("Spawnrates in % (100 Total)")]
    public int match;
    public int randomMismatch;
    public int crazyFish;
    public int crab;
    public int skeleton;
    public int exFishAndShark;

    [Header("Score Board")]
    public GameObject scoreBoard;
    public TextMeshProUGUI outcome;
    public TextMeshProUGUI levelScore;
    public TextMeshProUGUI levelHighscore;
    public GameObject newHighscore;
    public GameObject lastLevel;
    public Button nextLevel;

    private int score;
    private int highScore;
    private int numberOfObjectsLeft;

    // Used only in Practice Mode
    private int matches = 0;
    private int clickedMatch = 0;
    private int mismatches = 0;
    private int clickedMismatch = 0;

    // Used only in Endless Mode
    private int itemsSpawned = 0;
    private int spawnRateIncrease = 1;

    private List<string> words = new List<string>();

    private readonly int finalLevelNr = 15;

    void Start() {
        SetupInitialValues();
        SelectWordOption();
        if (PlayerPrefs.GetInt("MusicEnabled") == 0)
            backGroundMusic.Stop();
        if (PlayerPrefs.GetString("PlayMode", "Practice") == "Practice")
            InvokeRepeating("AddObjectPractice", 0.5f, 1);
        else if (PlayerPrefs.GetString("PlayMode", "Practice") == "Campaign")
            InvokeRepeating("AddObject", 0.5f, 1);
        else if (PlayerPrefs.GetString("PlayMode", "Practice") == "Endless")
            InvokeRepeating("AddObjectEndless", 0.5f, 1);
    }

    private void SetupInitialValues()
    {
        highScore = PlayerPrefs.GetInt("HighScoreLevel" + levelNr, 0);
        highScoreText.text = "Highscore: " + highScore;
        goalText.text = "Goal: " + goal;
        scoreValuesText.text = "CM: " + scoreCorrectPress + " CMM: " + scoreIncorrectPress + " MMP: " + scoreIncorrectPassed + " MP: " + scoreCorrectPassed;
        numberOfObjectsText.text = "Number of Objects: " + numberOfObjects;
        numberOfObjectsLeft = numberOfObjects;
        objectsLeftText.text = "Objects Left: " + numberOfObjectsLeft;
        if (PlayerPrefs.GetString("PlayMode", "Practice") == "Campaign")
            levelText.text = "Level: " + levelNr;
    }

    public string GetCurrentName()
    {
        return currentNameText.text;
    }

    public void AdjustScore(int adjustment)
    {
        if (PlayerPrefs.GetString("PlayMode", "Practice") == "Practice")
        {
            if (adjustment == 1) // The scoring on the practice level is set to give 1 point for clicking on a match.
                clickedMatch++;
            else if (adjustment == 2) // The scoring on the practice level is set to give 2 points for clicking on a mismatch.
                clickedMismatch++;
            // All other results are disregarded on the practice level and as such are set to 0.
        }
        else
        {
            score += adjustment;
            scoreText.text = "Score: " + score;
            if (PlayerPrefs.GetString("PlayMode", "Practice") == "Endless" && adjustment == -3)
            // The scoring on the endless level is set to reduce points by 3 for allowing a match (or a creature) to pass by. When this happens, the numbers of objects left
            // will be reduced by one. If it reaches zero, the endless level comes to an end - ie, the level ends whenever the player has missed a certain amount of objects
            // euqal to the value given to the public int numberOfObjects.
            {
                if (numberOfObjectsLeft >= 0)
                    objectsLeftText.text = "Objects Left: " + --numberOfObjectsLeft;
                if (numberOfObjectsLeft == 0)
                {
                    CancelInvoke("AddObjectEndless");
                    StartCoroutine(EndOfRoundEndless());
                }
            }
        }
    }

    private void AddObject()
    {
        objectsLeftText.text = "Objects Left: " + --numberOfObjectsLeft;
        if (numberOfObjectsLeft == 0)
        {
            CancelInvoke("AddObject");
            StartCoroutine(EndOfRound());
        }
        SpawnObject(Random.Range(0, 100));
    }

    private void AddObjectPractice()
    {
        objectsLeftText.text = "Objects Left: " + --numberOfObjectsLeft;
        if (numberOfObjectsLeft == 0)
        {
            CancelInvoke("AddObjectPractice");
            StartCoroutine(EndOfRoundPractice());
        }
        SpawnObject(Random.Range(0, 100));
    }

    private void AddObjectEndless()
    {
        if (itemsSpawned % 10 == 0)
        {
            match = Random.Range(30, 50);
            randomMismatch = Random.Range(15, 25);
            crazyFish = Random.Range(0 + spawnRateIncrease, 10 + spawnRateIncrease);
            crab = Random.Range(0 + spawnRateIncrease, 10 + spawnRateIncrease);
            skeleton = Random.Range(0 + spawnRateIncrease, 10 + spawnRateIncrease);
            exFishAndShark = Random.Range(0 + spawnRateIncrease, 10 + spawnRateIncrease);
            minSpeed += 0.1f;
            maxSpeed += 0.1f;
        }
        if (itemsSpawned % 20 == 0 && itemsSpawned != 0 && spawnRateIncrease <= 5)
        {
            CancelInvoke("AddObjectEndless");
            InvokeRepeating("AddObjectEndless", (1 - 0.1f * spawnRateIncrease), (1 - 0.1f * spawnRateIncrease));
            spawnRateIncrease++;
        }
        SpawnObject(Random.Range(0, match + randomMismatch + crazyFish + crab + skeleton + exFishAndShark));
        itemsSpawned++;
    }

    private void SpawnObject(int chance)
    {
        string randomWord;
        if (chance < match && match != 0)
        {
            matches++;
            randomWord = currentNameText.text;
            GameObject newInstance = Instantiate(namePrefab, spawnLocations[Random.Range(0, activateSpawnLocations)]);
            newInstance.GetComponent<TextMeshProUGUI>().text = randomWord;
        }
        else if (chance < match + randomMismatch && randomMismatch != 0)
        {
            mismatches++;
            randomWord = words[Random.Range(0, words.Count)];
            GameObject newInstance = Instantiate(namePrefab, spawnLocations[Random.Range(0, activateSpawnLocations)]);
            newInstance.GetComponent<TextMeshProUGUI>().text = randomWord;
        }
        else if (chance < match + randomMismatch + crazyFish && crazyFish != 0)
            Instantiate(crazyFishPrefab, spawnLocations[Random.Range(0, activateSpawnLocations)]);
        else if (chance < match + randomMismatch + crazyFish + crab && crab != 0)
            Instantiate(crabPrefab, spawnLocations[Random.Range(0, activateSpawnLocations)]);
        else if (chance < match + randomMismatch + crazyFish + crab + skeleton && skeleton != 0)
            Instantiate(skeletonPrefab, spawnLocations[Random.Range(0, activateSpawnLocations)]);
        else if (chance < match + randomMismatch + crazyFish + crab + skeleton + exFishAndShark && exFishAndShark != 0)
        {
            Transform spawnTransform = spawnLocations[Random.Range(0, activateSpawnLocations)];
            GameObject exhaustedFish = Instantiate(exFishPrefab, spawnTransform);
            StartCoroutine(SpawnShark(spawnTransform, exhaustedFish));
        }
    }

    IEnumerator SpawnShark(Transform spawnTransform, GameObject exhaustedFish)
    {
        yield return new WaitForSeconds(0.8f);
        GameObject shark = Instantiate(sharkPrefab, spawnTransform);
        shark.GetComponent<Shark>().SetPrey(exhaustedFish);
    }

    IEnumerator EndOfRound()
    {
        yield return new WaitForSeconds(5f);
        scoreBoard.SetActive(true);
        levelScore.text = "Score: " + score;
        levelHighscore.text = "Highscore: " + highScore;
        if (score < goal)
        {
            nextLevel.interactable = false;
            outcome.text = "Unfortunately, you didn't reach the goal and will have to retry.";
        }
        if (score >= goal && levelNr > PlayerPrefs.GetInt("HighestCompletedLevel", 0))
        {
            PlayerPrefs.SetInt("HighestCompletedLevel", levelNr);
        }
        if (score > highScore)
        {
            newHighscore.SetActive(true);
            PlayerPrefs.SetInt("HighScoreLevel" + levelNr, score);
        }
        if (levelNr == finalLevelNr)
        {
            if (score > goal)
                lastLevel.SetActive(true);
            nextLevel.interactable = false;
        }
    }

    IEnumerator EndOfRoundPractice()
    {
        yield return new WaitForSeconds(5f);
        scoreBoard.SetActive(true);
        decimal matchPercentage = System.Math.Round(((decimal)clickedMatch / (decimal)matches) * 100, 1);
        decimal mismatchPercentage = System.Math.Round(((decimal)(mismatches - clickedMismatch) / (decimal)mismatches) * 100, 1);
        levelScore.text = "You clicked on " + clickedMatch + "/" + matches + " matches (" + matchPercentage + "%).";
        levelHighscore.text = "You allowed " + (mismatches - clickedMismatch) + "/" + mismatches + " mismatches (" + mismatchPercentage + "%) to pass.";
    }

    IEnumerator EndOfRoundEndless()
    {
        yield return new WaitForSeconds(5f);
        scoreBoard.SetActive(true);
        levelScore.text = "Score: " + score;
        levelHighscore.text = "Highscore: " + highScore;
        if (score > highScore)
        {
            newHighscore.SetActive(true);
            PlayerPrefs.SetInt("HighScoreLevel" + levelNr, score);
        }
    }

    public void RestartButtonPressed()
    {
        SceneManager.LoadScene(levelNr);
    }

    public void NextLevelButtonPressed()
    {
        SceneManager.LoadScene(levelNr + 1);
    }

    public void MainMenyButtonPressed()
    {
        SceneManager.LoadScene(0);
    }

    private void SelectWordOption()
    {
        if (PlayerPrefs.GetString("WordOptions", "Letters") == "Letters")
            SelectLetters();
        else if (PlayerPrefs.GetString("WordOptions", "Letters") == "Names")
            SelectNames();
        else if (PlayerPrefs.GetString("WordOptions", "Letters") == "Colors")
            SelectColors();
        else if (PlayerPrefs.GetString("WordOptions", "Letters") == "Animals")
            SelectAnimals();
        else if (PlayerPrefs.GetString("WordOptions", "Letters") == "Custom Names")
            SelectCustomNames();
        RemoveSelectedFromList();
    }

    private void SelectLetters()
    {
        words = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        if (PlayerPrefs.GetString("CustomLetter", "-") != "-")
            currentNameText.text = PlayerPrefs.GetString("CustomLetter");
        else
            currentNameText.text = words[Random.Range(0, words.Count)];
    }

    private void SelectNames()
    {
        words = new List<string> { "RUSSEL", "KATHRYN", "ROGER", "WALTER", "MARGARET", "SAMUEL", "TERESA", "JOE", "JEREMY", "MARK", "KAREN", "STEVE", "MILDRED",
                                   "IRENE", "RACHEL", "JOSE", "CAROLYN", "JOSEPH", "MICHAEL", "MARTIN", "KENNETH", "JUAN", "CRAIG", "BRANDON", "MARTHA", "ADAM",
                                   "TIMOTHY", "ANDREA", "MICHELLE", "EDWARD", "ARTHUR", "SHAWN", "BRUCE", "PHILLIP", "SIMON", "JULIE", "BOBBY", "JASON",
                                   "DOROTHY", "NICOLE", "BENJAMIN", "DORIS", "LAWRENCE", "ANTHONY", "BILLY", "AMANDA", "PHILIP", "HARRY", "FRANK", "JOYCE" };
        if (PlayerPrefs.GetString("ChosenName", "-") != "-")
        {
            words.Add(PlayerPrefs.GetString("ChosenName"));
            currentNameText.text = PlayerPrefs.GetString("ChosenName");
        }
        else
            currentNameText.text = words[Random.Range(0, words.Count)];
    }

    private void SelectColors()
    {
        words = new List<string> { "Maroon", "Brown", "Olive", "Teal", "Navy", "Black", "Red", "Orange", "Yellow", "Lime", "Green", "Cyan", "Blue", "Purple",
                                    "Magenta", "Grey", "Pink", "Apricot", "Beige", "Mint", "Lavender", "White" };
        currentNameText.text = words[Random.Range(0, words.Count)];
    }

    private void SelectAnimals()
    {
        words = new List<string> { "Alligator", "Ant", "Bear", "Bee", "Bird", "Camel", "Cat", "Cheetah", "Chicken", "Chimpanzee", "Cow", "Crocodile", "Deer", "Dog",
                                    "Doplhin", "Duck", "Eagle", "Elephant", "Fish", "Fly", "Fox", "Frog", "Giraffe", "Goat", "Goldfish", "Hamster", "Horse", "Kangaroo",
                                    "Kitten", "Lion", "Lobster", "Monkey", "Octopus", "Owl", "Panda", "Pig", "Rabbit", "Rat", "Scorpion", "Seal", "Shark", "Sheep",
                                    "Snail", "Snake", "Spider", "Squirrel", "Tiger", "Turtle", "Wolf", "Zebra" };
        currentNameText.text = words[Random.Range(0, words.Count)];
    }

    private void SelectCustomNames()
    {
        bool chosenNameInCustomNames = false;
        for (int i = 1; i <= PlayerPrefs.GetInt("NumberOfCustomNames", 0); i++)
        {
            words.Add(PlayerPrefs.GetString("CustomName" + i));
            if (PlayerPrefs.GetString("CustomName" + i) == PlayerPrefs.GetString("ChosenName"))
                chosenNameInCustomNames = true;
        }
        if (chosenNameInCustomNames == false && PlayerPrefs.GetString("ChosenName", "-") != "-")
            words.Add(PlayerPrefs.GetString("ChosenName"));
        if (PlayerPrefs.GetString("ChosenName", "-") != "-")
            currentNameText.text = PlayerPrefs.GetString("ChosenName");
        else
            currentNameText.text = words[Random.Range(0, words.Count)];
    }

    private void RemoveSelectedFromList()
    {
        int index = words.IndexOf(currentNameText.text);
        words.RemoveAt(index);
    }
}
