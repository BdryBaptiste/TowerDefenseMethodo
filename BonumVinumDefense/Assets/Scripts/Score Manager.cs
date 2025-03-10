using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TMP_Text scoreText;
    public TMP_Text highscoreText;
    private int highscore = 0;
    private WaveManager waveManager;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
        if (waveManager == null)
        {
            Debug.LogError("WaveManager non trouvé !");
            return;
        }

        // Charger le meilleur score depuis PlayerPrefs
        highscore = PlayerPrefs.GetInt("Highscore", 0);
        Debug.Log("HighScore chargé : " + highscore);

        UpdateScore();
    }

    public void UpdateScore()
    {
        if (waveManager == null) return;

        int currentWave = waveManager.GetCurrentWaveNumber();
        Debug.Log("Vague actuelle : " + currentWave);

        if (scoreText != null)
            scoreText.text = "Vague Actuelle : " + currentWave.ToString();
        
        if (highscoreText != null)
            highscoreText.text = "Meilleure Vague : " + highscore.ToString();

        if (currentWave > highscore)
        {
            highscore = currentWave;
            PlayerPrefs.SetInt("Highscore", highscore);
            PlayerPrefs.Save();

            Debug.Log("Nouveau Highscore enregistré : " + highscore);

            if (highscoreText != null)
                highscoreText.text = "Meilleure Vague : " + highscore.ToString();
        }
    }
}
