using TMPro;
using UnityEngine;

public abstract class FallingObject : MonoBehaviour
{
    protected float fallSpeed;
    protected float minSpeed;
    protected float maxSpeed;

    protected int scoreMatchClicked;
    protected int scoreMisMatchClicked;
    protected int scoreMisMatchPassed;
    protected int scoreMatchPassed;

    protected bool haveBeenClicked;
    protected bool haveBeenScored;

    protected TextMeshProUGUI objectText;
    protected WordManager wordManager;

    public virtual void Start()
    {
        wordManager = GameObject.FindGameObjectWithTag("Word Manager").GetComponent<WordManager>();
        objectText = GetComponent<TextMeshProUGUI>();
        SetValuesFromWordManager();
        fallSpeed = Random.Range(minSpeed, maxSpeed);
    }

    public virtual void Update()
    {
        transform.Translate(0f, -fallSpeed * Time.deltaTime, 0);
        if (transform.position.y < -3.6f)
            ReachedLowerLimit();
    }

    public abstract void ReachedLowerLimit();
    public abstract void SetValuesFromWordManager();
    public abstract void OnMouseDown();
}