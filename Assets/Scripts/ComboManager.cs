using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance;

    public AudioSource hitVFX;
    public AudioSource missVFX;

    public TMPro.TextMeshProUGUI comboScoreText;
    public TMPro.TextMeshProUGUI totalScoreText;

    public static int comboScore;
    public static int totalScore;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        comboScore = 0;
        totalScore = 0;
    }

    public static void Hit()
    {
        Instance.hitVFX.PlayOneShot(Instance.hitVFX.clip);
        comboScore++;
        totalScore++;
        totalScore += (int)((double)comboScore / 5 * 1.7);
    }

    public static void Miss()
    {
        Instance.missVFX.PlayOneShot(Instance.missVFX.clip);
        comboScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        comboScoreText.text = comboScore.ToString();
        totalScoreText.text = totalScore.ToString();

    }
}
