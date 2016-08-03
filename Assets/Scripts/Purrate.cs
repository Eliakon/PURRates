using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Purrate : MonoBehaviour
{
    private const string selectedParameterName = "Selected";
    private const string hitParameterName = "Hit";
    private const string dieParameterName = "Die";

    [SerializeField]
    public RectTransform rectTransform;
    [SerializeField]
    public PurrateSkinManager skinManager;
    [SerializeField]
    private Animator selectionAnimator;
    [SerializeField]
    private Animator characterAnimator;
    [SerializeField]
    private float animationDuration = 0.5f;
    [SerializeField]
    public BaballLauncher baballLauncher;
    [SerializeField]
    private HealthPoints healthPoints;
    [SerializeField]
    private int life = 9;
    [SerializeField]
    private float dieDelay = 5;

    [System.Serializable]
    public class UnityPurrateEvent: UnityEvent<Purrate> {}
    [SerializeField]
    private UnityPurrateEvent DestroyPurrate;
    [SerializeField]
    private UnityEvent CatMoves;

    private int maxLife;
    private bool selected = false;
    private bool moving = false;
    private Vector2 position;
    private Vector2 targetPosition;
    private float currentAnimationTime;
    private List<SeaTile.Position> currentPath;

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

    public SeaTile.Position GetCurrentTilePosition(List<SeaTile.Position> seaTileStack)
    {
        SeaTile.Position nearest = null;
        float nearestDistance = 0;

        foreach (SeaTile.Position position in seaTileStack)
        {
            float distance = Vector3.Distance(
                rectTransform.localPosition,
                position.screen
            );

            if (nearest == null || distance < nearestDistance) {
                nearestDistance = distance;
                nearest = position;
            }
        }

        return nearest;
    }

    public void Move(SeaTile.Position target, List<SeaTile.Position> seaTileStack)
    {

        SeaTile.Position current = GetCurrentTilePosition(seaTileStack);
        currentPath = SeaTile.Position.FindPath(seaTileStack, current, target);

        if (baballLauncher.Attacking)
        {
            baballLauncher.StopAttack();
        }

        MoveToNextPathPosition();
    }

    public void MoveToNextPathPosition() {
        if (currentPath != null && currentPath.Count > 0) {
            position = rectTransform.localPosition;
            targetPosition = currentPath[0].screen;
            currentPath.RemoveAt(0);
            currentAnimationTime = 0;
            moving = true;
        }
    }

    public void LaunchAttack(Purrate enemy)
    {
        moving = false;

        baballLauncher.LaunchAttack(enemy);
    }

    public void Strike()
    {
        life -= 1;
        healthPoints.SetHealth(life, maxLife);

        if (life > 0)
        {
            characterAnimator.SetTrigger(hitParameterName);
            return;
        }
        StartCoroutine(DestroyPurrateCoroutine());
    }

    private IEnumerator DestroyPurrateCoroutine()
    {
        characterAnimator.SetTrigger(dieParameterName);
        yield return new WaitForSeconds(dieDelay);
        DestroyPurrate.Invoke(this);
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
                MoveToNextPathPosition();
            }

            currentAnimationTime += Time.deltaTime;
            CatMoves.Invoke();
        }
    }
}
