using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    protected TextMeshProUGUI textMeshPro;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        this.textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateText(bool isPlayer, int score)
    {
        if (isPlayer) this.textMeshPro.text = $"Player Score: {score}";
        else this.textMeshPro.text = $"Opponent Score: {score}";
    }
}
