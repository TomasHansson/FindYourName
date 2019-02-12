using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MenyManager : MonoBehaviour {

    [Header("UI Elements")]
    public Button startButton;
    public Button musicButton;
    public Dropdown playModeOptions;
    public Dropdown wordOptions;
    public Text highestCompletedLevel;
    public InputField chooseLevel;
    
    [Header("Specific Letter")]
    public Text specificLetter;
    public InputField specificLetterInput;

    [Header("Specific Name")]
    public Text specificName;
    public InputField specificNameInput;

    [Header("Custom Names")]
    public Text customNames;
    public InputField customNamesInput;
    public Text customNamesInfo;
    public Button customNamesSaveButton;
    public Button customNamesClearButton;
    public Text customNamesReminderText;

    [Header("Reset Panel")]
    public GameObject resetPanel;

    private bool tempHideCustomNames = false;
    private bool tempHideSpecificLetter = false;
    private bool tempHideSpecificName = false;

    private readonly int maxCustomNames = 21;

    void Start()
    {
        SelectWordOptionFromPlayerPrefs();
        SelectMusicOptionFromPlayerPrefs();
        SelectPlayModeFromPlayerPrefs();
        if (PlayerPrefs.GetInt("HighestCompletedLevel", 0) == 0)
        {
            chooseLevel.interactable = false;
            highestCompletedLevel.text = "Highest Completed Level: -";
        }
        else
            highestCompletedLevel.text = "Highest Completed Level: " + PlayerPrefs.GetInt("HighestCompletedLevel");
    }

    private void SelectWordOptionFromPlayerPrefs()
    {
        switch (PlayerPrefs.GetString("WordOptions", "Letters"))
        {
            case "Letters":
                wordOptions.value = 0;
                break;
            case "Names":
                wordOptions.value = 1;
                break;
            case "Colors":
                wordOptions.value = 2;
                break;
            case "Animals":
                wordOptions.value = 3;
                break;
            case "Custom Names":
                wordOptions.value = 4;
                break;
        }
    }

    public void WordOptionsChanged()
    {
        string chosenOption = "Letters";
        startButton.interactable = true;
        DeactivateGameModeObjects();
        switch (wordOptions.value)
        {
            case 0:
                chosenOption = "Letters";
                ActivateSpecificLetterObjects(true);
                specificLetter.text = "Current Chosen Letter: " + PlayerPrefs.GetString("CustomLetter", "-");
                break;
            case 1:
                chosenOption = "Names";
                ActivateSpecificNameObjects(true);
                specificName.text = "Current Chosen Name: " + System.Environment.NewLine + PlayerPrefs.GetString("ChosenName", "-");
                break;
            case 2:
                chosenOption = "Colors";
                break;
            case 3:
                chosenOption = "Animals";
                break;
            case 4:
                chosenOption = "Custom Names";
                ActivateCustomNamesObjects(true);
                ReadCustomNames();
                break;
        }
        PlayerPrefs.SetString("WordOptions", chosenOption);
    }

    private void DeactivateGameModeObjects()
    {
        ActivateCustomNamesObjects(false);
        ActivateSpecificLetterObjects(false);
        ActivateSpecificNameObjects(false);
    }

    private void ActivateCustomNamesObjects(bool activeState)
    {
        customNames.gameObject.SetActive(activeState);
        customNamesInput.gameObject.SetActive(activeState);
        customNamesInfo.gameObject.SetActive(activeState);
        customNamesSaveButton.gameObject.SetActive(activeState);
        customNamesClearButton.gameObject.SetActive(activeState);
        customNamesReminderText.gameObject.SetActive(activeState);
    }

    private void ActivateSpecificLetterObjects(bool activeState)
    {
        specificLetter.gameObject.SetActive(activeState);
        specificLetterInput.gameObject.SetActive(activeState);
    }

    private void ActivateSpecificNameObjects(bool activeState)
    {
        specificName.gameObject.SetActive(activeState);
        specificNameInput.gameObject.SetActive(activeState);
    }

    private enum Level
    {
        Practice = 16,
        Endless = 17
    }

    public void StartButtonPressed()
    {
        if (PlayerPrefs.GetString("PlayMode", "Practice") == "Practice")
            SceneManager.LoadScene((int)Level.Practice);
        else if (PlayerPrefs.GetString("PlayMode", "Practice") == "Campaign")
        {
            if (chooseLevel.text == null || chooseLevel.text == "")
                SceneManager.LoadScene(1);
            else
                SceneManager.LoadScene(int.Parse(chooseLevel.text));
        }
        else if (PlayerPrefs.GetString("PlayMode", "Practice") == "Endless")
            SceneManager.LoadScene((int)Level.Endless);
    }

    public void ChooseLevelInputValueChanged()
    {
        if (chooseLevel.text != null && chooseLevel.text != "")
        {
            bool allNumbers = true;
            foreach (char number in chooseLevel.text)
            {
                if (!"0123456789".Contains(number.ToString()))
                    allNumbers = false;
            }
            if (!allNumbers && chooseLevel.text.Length == 1)
                chooseLevel.text = "";
            else if (!allNumbers && chooseLevel.text.Length == 2)
                chooseLevel.text = chooseLevel.text.Substring(0, 1);
            if (chooseLevel.text == "0")
                chooseLevel.text = "";
        }
        if (chooseLevel.text != null && chooseLevel.text != "")
        {
            if (int.Parse(chooseLevel.text) > PlayerPrefs.GetInt("HighestCompletedLevel", 0))
                chooseLevel.text = PlayerPrefs.GetInt("HighestCompletedLevel", 0).ToString();
        }
    }

    public void MusicButtonPressed()
    {
        if (PlayerPrefs.GetInt("MusicEnabled", 1) == 1)
        {
            PlayerPrefs.SetInt("MusicEnabled", 0);
            musicButton.GetComponentInChildren<Text>().text = "Enable Music";
        }
        else if (PlayerPrefs.GetInt("MusicEnabled", 1) == 0)
        {
            PlayerPrefs.SetInt("MusicEnabled", 1);
            musicButton.GetComponentInChildren<Text>().text = "Disable Music";
        }
    }

    private void SelectMusicOptionFromPlayerPrefs()
    {
        if (PlayerPrefs.GetInt("MusicEnabled", 1) == 1)
            musicButton.GetComponentInChildren<Text>().text = "Disable Music";
        else
            musicButton.GetComponentInChildren<Text>().text = "Enable Music";
    }

    public void SpecificLetterInputChanged()
    {
        if (specificLetterInput.text != "" && char.IsLetter(specificLetterInput.text[0]))
        {
            PlayerPrefs.SetString("CustomLetter", specificLetterInput.text.ToUpper());
            specificLetter.text = "Current Chosen Letter: " + specificLetterInput.text;
        }
        else if (specificLetterInput.text != "" && !char.IsLetter(specificLetterInput.text[0]))
            specificLetterInput.text = "";
    }

    public void SpecificNameInputChanged()
    {
        bool allLetters = true;
        foreach (char letter in specificNameInput.text)
        {
            if (!char.IsLetter(letter))
                allLetters = false;
        }
        if (!allLetters && specificNameInput.text.Length == 1)
            specificNameInput.text = "";
        else if (!allLetters && specificNameInput.text.Length > 1)
            specificNameInput.text = specificNameInput.text.Substring(0, specificNameInput.text.Length - 1);
        if (specificNameInput.text != "")
        {
            PlayerPrefs.SetString("ChosenName", specificNameInput.text.ToUpper());
            specificName.text = "Current Chosen Name: " + System.Environment.NewLine + specificNameInput.text;
        }
    }

    public void CustomNamesInputValueChanged()
    {
        bool allLetters = true;
        foreach (char letter in customNamesInput.text)
        {
            if (!char.IsLetter(letter))
                allLetters = false;
        }
        if (!allLetters && customNamesInput.text.Length == 1)
            customNamesInput.text = "";
        else if (!allLetters && customNamesInput.text.Length > 1)
            customNamesInput.text = customNamesInput.text.Substring(0, customNamesInput.text.Length - 1);
    }

    public void CustomNamesInsertButtonClicked()
    {
        if (customNamesInput.text != "" && PlayerPrefs.GetInt("NumberOfCustomNames", 0) < maxCustomNames)
        {
            List<string> tempList = new List<string>();
            for (int i = 1; i <= PlayerPrefs.GetInt("NumberOfCustomNames", 0); i++)
            {
                tempList.Add(PlayerPrefs.GetString("CustomName" + i));
            }
            bool nonDuplicate = true;
            foreach (string word in tempList)
            {
                if (word == customNamesInput.text.ToUpper())
                    nonDuplicate = false;
            }
            if (nonDuplicate)
            {
                PlayerPrefs.SetInt("NumberOfCustomNames", PlayerPrefs.GetInt("NumberOfCustomNames", 0) + 1);
                PlayerPrefs.SetString("CustomName" + PlayerPrefs.GetInt("NumberOfCustomNames", 0), customNamesInput.text.ToUpper());
                ReadCustomNames();
            }
        }
    }

    public void CustomNamesClearButtonClicked()
    {
        PlayerPrefs.SetInt("NumberOfCustomNames", 0);
        customNames.text = "Current Custom Names: ";
        customNamesInfo.text = "You Can Save a Total of " + maxCustomNames + " Custom Names. There Are Currently " + PlayerPrefs.GetInt("NumberOfCustomNames", 0) + " Names Saved. A minimum of 5 are required to play and duplicates cannot be saved.";
        startButton.interactable = false;
    }

    private void ReadCustomNames()
    {
        int inputs = 1;
        string readFromPlayerPrefs = "";
        for (int i = 1; i <= PlayerPrefs.GetInt("NumberOfCustomNames", 0); i++)
        {
            if (inputs % 3 == 0)
                readFromPlayerPrefs += PlayerPrefs.GetString("CustomName" + i) + System.Environment.NewLine;
            else
                readFromPlayerPrefs += PlayerPrefs.GetString("CustomName" + i) + "  ";
            inputs++;
        }
        customNames.text = "Current Custom Names: " + System.Environment.NewLine + readFromPlayerPrefs;
        customNamesInfo.text = "You Can Save a Total of " + maxCustomNames + " Custom Names. There Are Currently " + PlayerPrefs.GetInt("NumberOfCustomNames", 0) + " Names Saved. A minimum of 5 are required to play and duplicates cannot be saved.";
        startButton.interactable = PlayerPrefs.GetInt("NumberOfCustomNames", 0) >= 5 ? true : false;
    }

    public void ResetButtonClicked()
    {
        if (customNames.gameObject.activeSelf == true)
        {
            tempHideCustomNames = true;
            ActivateCustomNamesObjects(false);
        }
        if (specificLetter.gameObject.activeSelf == true)
        {
            tempHideSpecificLetter = true;
            ActivateSpecificLetterObjects(false);
        }
        if (specificName.gameObject.activeSelf == true)
        {
            tempHideSpecificName = true;
            ActivateSpecificNameObjects(false);
        }
        resetPanel.SetActive(true);
    }

    public void ConfirmButtonClicked()
    {
        for (int i = 1; i < 16; i++)
        {
            PlayerPrefs.SetInt("HighScoreLevel" + i, 0);
            PlayerPrefs.SetInt("DontShowAgainLevel" + i, 0);
        }
        PlayerPrefs.SetInt("HighestCompletedLevel", 0);
        highestCompletedLevel.text = "Highest Completed Level: -";
        chooseLevel.interactable = false;
        chooseLevel.text = "";
        resetPanel.SetActive(false);
        ReactivateHiddenObjects();
    }

    public void CancelButtonClicked()
    {
        resetPanel.SetActive(false);
        ReactivateHiddenObjects();
    }

    private void ReactivateHiddenObjects()
    {
        if (tempHideCustomNames)
        {
            ActivateCustomNamesObjects(true);
            tempHideCustomNames = false;
        }
        if (tempHideSpecificLetter)
        {
            ActivateSpecificLetterObjects(true);
            tempHideSpecificLetter = false;
        }
        if (tempHideSpecificName)
        {
            ActivateSpecificNameObjects(true);
            tempHideSpecificName = false;
        }
    }

    public void SelectPlayModeFromPlayerPrefs()
    {
        switch (PlayerPrefs.GetString("PlayMode", "Pratice"))
        {
            case "Practice":
                playModeOptions.value = 0;
                break;
            case "Campaign":
                playModeOptions.value = 1;
                break;
            case "Endless":
                playModeOptions.value = 2;
                break;
        }
    }

    public void PlayModeValueChanged()
    {
        string chosenOption = "Letters";
        chooseLevel.interactable = false;
        switch (playModeOptions.value)
        {
            case 0:
                chosenOption = "Practice";
                chooseLevel.text = "";
                break;
            case 1:
                chosenOption = "Campaign";
                chooseLevel.interactable = PlayerPrefs.GetInt("HighestCompletedLevel", 0) > 0 ? true : false;
                break;
            case 2:
                chosenOption = "Endless";
                chooseLevel.text = "";
                break;
        }
        PlayerPrefs.SetString("PlayMode", chosenOption);
    }
}
