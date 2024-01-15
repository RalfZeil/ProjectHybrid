using UnityEngine;
using TMPro;

public class BeatScore : MonoBehaviour
{
    [Header("Score")]
    public TextMeshProUGUI scoreText;
    public int startScore = 0;

    private int score = 0;

    [Header("Feedback")]
    public GameObject feedbackPrefab;
    public Transform feedbackSpawnPoint;
    public float destroyTime;

    private void Start()
    {
        UpdateScoreText();
        score = startScore;
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    private void InstantiateFeedback(string text, Color color)
    {
        GameObject feedback = Instantiate(feedbackPrefab, feedbackSpawnPoint);
        feedback.GetComponent<TextMeshProUGUI>().text = text;
        feedback.GetComponent<TextMeshProUGUI>().color = color;
        Destroy(feedback, destroyTime); // Destroy the feedback text after 1 second
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
        InstantiateFeedback("+" + points.ToString(), Color.green);
    }

    public void SubtractScore(int points)
    {
        score -= points;
        if (score < 0)
            score = 0; // Ensure the score doesn't go below zero
        UpdateScoreText();
        InstantiateFeedback("-" + points.ToString(), Color.red);
    }
}
