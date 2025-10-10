using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameEndScreen : MonoBehaviour
{
    [SerializeField] private Image blackOverlay;
    [SerializeField] private TextMeshProUGUI endText;


    void Start()
    { 
        blackOverlay.color = new Color(0, 0, 0, 1f);
        endText.gameObject.SetActive(false);
        StartCoroutine(FadeIn());        
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        Color c = blackOverlay.color;
        while (t < 2f)
        {
            t += Time.deltaTime;
            float time = t / 2f;
            time = time * time * time;
            float alpha = Mathf.Lerp(1f, 0f, time);
            blackOverlay.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }
    }

    public void ShowEndScreen(string message)
    {
        StartCoroutine(FadeToBlackAndShowMessage(message));
    }

    private IEnumerator FadeToBlackAndShowMessage(string message)
    {
        float t = 0f;
        Color c = blackOverlay.color;
        while (t < 2f)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / 2f);
            blackOverlay.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        endText.text = message;
        endText.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MainMenu");
    }
}

