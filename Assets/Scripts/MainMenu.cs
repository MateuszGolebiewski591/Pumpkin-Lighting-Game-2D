using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Image blackOverlay;
    public void PlayGame()
    {
        StartCoroutine(FadeToGame());
        
    }

    private IEnumerator FadeToGame()
    {
        float t = 0f;
        Color c = blackOverlay.color;
        while (t < 1f)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / 1f);
            blackOverlay.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }
        SceneManager.LoadSceneAsync(1);
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
