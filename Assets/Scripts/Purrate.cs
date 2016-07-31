using UnityEngine;
using System.Collections;

public class Purrate : MonoBehaviour
{
    private const string selectedParameterName = "Selected";

    [SerializeField]
    public RectTransform rectTransform;
    [SerializeField]
    public PurrateSkinManager skinManager;
    [SerializeField]
    private Animator selectionAnimator;
    [SerializeField]
    private float animationDuration = 5;
    [SerializeField]
    public BaballLauncher baballLauncher;
    [SerializeField]
    private HealthPoints healthPoints;
    [SerializeField]
    private int life = 9;

    private int maxLife;
    private bool selected = false;
    private bool moving = false;
    private Vector2 position;
    private Vector2 targetPosition;
    private float currentAnimationTime;

    public int Life
    {
        get
        {
            return life;
        }
    }

    private void Start()
    {
        maxLife = life;
    }

    public void Select(bool select)
    {
        selected = select;
        selectionAnimator.SetBool(selectedParameterName, selected);
    }

    public void Move(Vector3 targetPos)
    {
        position = rectTransform.localPosition;
        targetPosition = targetPos;
        currentAnimationTime = 0;
        moving = true;
    }

    public void LaunchAttack(Purrate enemy)
    {
        baballLauncher.LaunchAttack(enemy);
    }

    public void Strike()
    {
        life -= 1;
        healthPoints.SetHealth(life, maxLife);
    }

    private void Update()
    {
        if (moving)
        {
            var t = currentAnimationTime / animationDuration;
            var posX = Mathf.Lerp(position.x, targetPosition.x, t);
            var posY = Mathf.Lerp(position.y, targetPosition.y, t);
            rectTransform.localPosition = new Vector3(posX, posY, 0);

            if (currentAnimationTime >= animationDuration)
            {
                moving = false;
            }

            currentAnimationTime += Time.deltaTime;
        }
    }
}
