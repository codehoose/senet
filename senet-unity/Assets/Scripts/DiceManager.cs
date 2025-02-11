using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using EventHandler = System.EventHandler;
using EventArgs = System.EventArgs;

public class DiceManager : MonoBehaviour
{
    [SerializeField] private GameObject rollDieButton;
    [SerializeField] private Image animatedDie;
    [SerializeField] private Sprite[] dieImages;
    [SerializeField] private Image lastRollImage;
    [SerializeField] private bool doAnimation = true;

    public int LastRoll { get; private set; }

    public bool RollDieButtonVisible => rollDieButton.activeInHierarchy;

    public void ShowRollButton() => rollDieButton.SetActive(true);

    public event EventHandler DiceRolled;

    void Start()
    {
        rollDieButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            rollDieButton.SetActive(false);
            animatedDie.gameObject.SetActive(true);
            StartCoroutine(RollTheDie());
        });
    }

    IEnumerator RollTheDie()
    {
        float waitTime = 1 / 128f;
        while (doAnimation && waitTime < 1.5f)
        {
            animatedDie.sprite = dieImages[Random.Range(0, dieImages.Length)];
            waitTime *= 2f;
            yield return new WaitForSeconds(waitTime);
        }

        int newRoll = Random.Range(0, dieImages.Length);
        LastRoll = newRoll < 4 ? newRoll + 1 : 6;

        animatedDie.sprite = dieImages[newRoll];
        lastRollImage.sprite = animatedDie.sprite;

        if (doAnimation)
        {
            yield return new WaitForSeconds(2f);
        }
        animatedDie.gameObject.SetActive(false);
        lastRollImage.gameObject.SetActive(true);

        DiceRolled?.Invoke(this, EventArgs.Empty);
    }
}
