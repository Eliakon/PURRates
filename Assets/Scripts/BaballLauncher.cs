using UnityEngine;
using System.Collections;

public class BaballLauncher : MonoBehaviour
{
    [SerializeField]
    public RectTransform baball;
    [SerializeField]
    private float animationDuration = 2;
    [SerializeField]
    private float bounce = 100;
    [SerializeField]
    private float attackRange = 200;

    private bool attacking = false;
    private Vector2 anchoredPosition;
    private Vector2 position;
    private Purrate target;
    private Vector2 targetPosition;
    private float currentAnimationTime;

    public bool Attacking
    {
        get
        {
            return attacking;
        }
    }

    private void Start()
    {
        anchoredPosition = baball.anchoredPosition;
    }

    public bool CanAttack(Purrate cat)
    {
        var catPosition = SwitchToRectTransform(baball, cat.baballLauncher.baball);
        var diffX = Mathf.Abs(catPosition.x - baball.anchoredPosition.x);
        var diffY = Mathf.Abs(catPosition.y - baball.anchoredPosition.y);
        return diffX <= attackRange && diffY <= attackRange;
    }

    public void LaunchAttack(Purrate cat)
    {
        if (CanAttack(cat) && cat.Life > 0)
        {
            target = cat;
            targetPosition = SwitchToRectTransform(baball, cat.baballLauncher.baball);
            position = baball.anchoredPosition;
            currentAnimationTime = 0;
            attacking = true;
            gameObject.SetActive(true);
        }
    }

    public void StopAttack()
    {
        gameObject.SetActive(false);
        baball.anchoredPosition = anchoredPosition;
        attacking = false;
    }

    private void Update()
    {
        if (attacking)
        {
            var t = currentAnimationTime / animationDuration;
            var posX = Mathf.Lerp(position.x, targetPosition.x, t);
            var posY = Mathf.Lerp(position.y, targetPosition.y, t);
            posY += Mathf.Sin(t * Mathf.PI) * bounce;
            baball.anchoredPosition = new Vector2(posX, posY);

            if (currentAnimationTime >= animationDuration)
            {
                StopAttack();
                target.Strike();

                if (target.Life > 0)
                {
                    LaunchAttack(target);
                    return;
                }
            }

            currentAnimationTime += Time.deltaTime;
        }
    }

    private static Vector2 SwitchToRectTransform(RectTransform from, RectTransform to)
    {
        Vector2 localPoint;
        Vector2 fromPivotDerivedOffset = new Vector2(from.rect.width * 0.5f + from.rect.xMin, from.rect.height * 0.5f + from.rect.yMin);
        Vector2 screenP = RectTransformUtility.WorldToScreenPoint(null, from.position);
        screenP += fromPivotDerivedOffset;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(to, screenP, null, out localPoint);
        Vector2 pivotDerivedOffset = new Vector2(to.rect.width * 0.5f + to.rect.xMin, to.rect.height * 0.5f + to.rect.yMin);
        return -1 * (to.anchoredPosition + localPoint - pivotDerivedOffset);
    }
}
