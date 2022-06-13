using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

public class DragAndDropComponent : MonoBehaviour
{
    [SerializeField] private BaseHandler HandlerPrefab;
    private BaseHandler handler;

    [SerializeField] private float timeToDragPosition = 0.3f, checkError = 0.1f;

    private Camera cam;
    private Vector3 startPosition, mOffset, velocity;
    private Vector2 scale;
    private float mZCoordinate;
    private bool draggableObjectOverTarget, clicksBlocked, hasMouseDownMoment;

    private TagsComparer comparer;

    private BoxCollider2D boxCollider;
    
    private Vector3 startRotation;
    private Transform pivot,target, card;

    public void SetActive(bool val)
    {
        if (boxCollider != null)
            boxCollider.enabled = val;
    }

    public void Init()
    {
        pivot = transform.GetChild(0);
        card = pivot.GetChild(0);
        scale = pivot.localScale;
        cam = Camera.main;
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
        handler = Instantiate(HandlerPrefab, transform);
    }

    public void SetBaseHandler(BaseHandler value) => HandlerPrefab = value;
    public void SetComparer(TagsComparer value) => comparer = value;
    public BaseHandler GetBaseHandler() => handler;
    public void SetStartPosition() => SetStartPosition(transform.position);
    public void SetStartPosition(Vector3 newPosition) => startPosition = newPosition;

   

    void OnMouseDown()
    {
        if (clicksBlocked || hasMouseDownMoment) return;
        hasMouseDownMoment = true;
        Debug.Log("OnMouseDown");
        startRotation = pivot.rotation.eulerAngles;
        pivot.DOScale(scale * 1.3f, 0.3f);
        pivot.DORotate(Vector3.zero, 0.3f);
        card.DORotate(Vector3.zero, 0.3f);
        mZCoordinate = cam.WorldToScreenPoint(transform.position).z;
        mOffset = transform.position - GetTouchPointAsWorldPoint();
        handler.OnStartDrag(transform);
    }

    private Vector3 GetTouchPointAsWorldPoint()
    {
        var point = Vector3.zero;
#if UNITY_EDITOR
        point = Input.mousePosition;
#else
        if(Input.touchCount>0)
            point = Input.touches[0].position;
#endif
        point.z = mZCoordinate;
        return cam.ScreenToWorldPoint(point);
    }

    [SerializeField] private float power = 20;

    void OnMouseDrag()
    {
        if (clicksBlocked || !hasMouseDownMoment) return;
        var direction = transform.position - GetTouchPointAsWorldPoint() + mOffset;
        direction = new Vector3(direction.y, direction.x);
        card.DORotate(direction*power, timeToDragPosition);
        transform.DOMove(GetTouchPointAsWorldPoint() + mOffset, timeToDragPosition);
    }

    void OnMouseUp()
    {
        if (clicksBlocked || !hasMouseDownMoment) return;
        clicksBlocked = true;
        hasMouseDownMoment = false;
        CheckOverTargetRelease();
        handler.OnEndDrag(transform);
        Move();
    }

    private void Move()
    {
        card.DOLocalRotate(Vector3.zero, 0.3f);
        if (draggableObjectOverTarget)
            MoveToTarget();
        else
            MoveToStart();
    }

    void CheckOverTargetRelease()
    {
        target = comparer.CompareContinuous(transform.position, checkError);
        draggableObjectOverTarget = target != null;
    }

    void MoveToTarget()
    {
        handler.OnLateEndDrag(transform);
        var result = handler.Interaction(transform, target);
        if (!result) MoveToStart();
    }

    void MoveToStart()
    {
        pivot.DOScale(scale, 0.3f);
        pivot.DORotate(startRotation, 0.3f);
        handler.BadInteraction(transform);
        transform.DOMove(startPosition, 0.3f).SetEase(Ease.Linear).OnComplete(
            () =>
            {
                handler.OnLateEndDrag(transform);
                clicksBlocked = false;
                Debug.Log("end Move to start");
            });
    }
}