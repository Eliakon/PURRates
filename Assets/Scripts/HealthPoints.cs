using UnityEngine;
using System.Collections;

public class HealthPoints : MonoBehaviour
{
    [SerializeField]
    private RectTransform health;
    [SerializeField]
    private float animationDuration = 0.2f;

    private float currentAnimationTime;
    private float fromValue;
    private float toValue;
    private bool animated;
    
    public void SetHealth(int healthValue, int maxHealthValue)
    {
        fromValue = health.anchorMax.x;
        toValue = (float)healthValue / (float)maxHealthValue;
        currentAnimationTime = 0;
        animated = true;
    }

    private void Update()
    {
        if (animated)
        {
            var t = currentAnimationTime / animationDuration;
            health.anchorMax = new Vector2(Mathf.Lerp(fromValue, toValue, t), health.anchorMax.y);

            if (currentAnimationTime >= animationDuration)
            {
                health.anchorMax = new Vector2(toValue, health.anchorMax.y);
                animated = false;
            }

            currentAnimationTime += Time.deltaTime;
        }
    }
}
