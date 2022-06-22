using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Image hpImage;
    public Image hpEffectImage;

    [HideInInspector]
    public float hp;
    [SerializeField] private float maxHp=120;
    [SerializeField] private float hurtSpeed = 0.005f;

    private void Start()
    {
        hp = maxHp;

    }

    private void Update()
    {
        hpImage.fillAmount = hp / maxHp;
        if(hpEffectImage.fillAmount>hpImage.fillAmount)
        {
            hpEffectImage.fillAmount -= hurtSpeed;
        }
        else
        {
            hpEffectImage.fillAmount = hpImage.fillAmount;
        }
    }
}
