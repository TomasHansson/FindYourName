using UnityEngine;
using UnityEngine.UI;

public class Infoboard : MonoBehaviour {

    public Toggle dontShowAgain;
    private int levelNr;

	void Start () {
        levelNr = GameObject.FindGameObjectWithTag("Word Manager").GetComponent<WordManager>().levelNr;
        if (PlayerPrefs.GetInt("DontShowAgainLevel" + levelNr, 0) == 0)
            Time.timeScale = 0;
        else
            gameObject.SetActive(false);
	}
	
    public void ContinueButtonClicked()
    {
        Time.timeScale = 1;
        if (dontShowAgain.isOn)
            PlayerPrefs.SetInt("DontShowAgainLevel" + levelNr, 1);
        gameObject.SetActive(false);
    }
}