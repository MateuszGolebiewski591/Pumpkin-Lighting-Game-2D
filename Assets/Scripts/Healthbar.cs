using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Image healthbar;

    void start()
    {
        healthbar.fillAmount = 5 / 10;
    }

    public void UpdateHealth(int newHealth)
    {
        healthbar.fillAmount = newHealth*1f / 10;
    }
}
