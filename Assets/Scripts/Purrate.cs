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

    private bool selected = false;
    private bool moving = false;
    private Vector2 position;
    private Vector2 targetPosition;
    private float currentAnimationTime;

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
                rectTransform.localPosition = new Vector3(targetPosition.x, targetPosition.y, 0);
                moving = false;
            }

            currentAnimationTime += Time.deltaTime;
        }
    }
}
