using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance;

    public AudioSource hitVFX;
    public AudioSource missVFX;

    public TMPro.TextMeshProUGUI comboScoreText;
    public static int comboScore;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        comboScore = 0;
    }

    public static void Hit()
    {
        Instance.hitVFX.PlayOneShot(Instance.hitVFX.clip);
        comboScore++;
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
    }
}
