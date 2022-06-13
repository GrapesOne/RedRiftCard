using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

public class CardController : MonoBehaviour
{
    [SerializeField] private SortingGroup sortingGroup;
    [SerializeField] private CardView cardView;
    [SerializeField] private DragAndDropComponent dadc;
    private CardData cardData;
    private AbstractCardHolder currentCardHolder;

    private bool active;

    public void DOScale(float value = default, float duration = 0.3f, Ease ease = Ease.Linear)
    {
        transform.DOScale(value, duration).SetEase(ease);
    }
    public void DOPivotScale(float value = default, float duration = 0.3f, Ease ease = Ease.Linear)
    {
        transform.GetChild(0).DOScale(value, duration).SetEase(ease);
    }

    public void DOLocalMove(Vector3 value = default, float duration = 0.3f, Ease ease = Ease.Linear)
    {
        transform.DOLocalMove(value, duration).SetEase(ease).OnComplete(
            () => dadc.SetStartPosition());
    }

    public void DOLocalRotate(Vector3 value = default, float duration = 0.3f, Ease ease = Ease.Linear)
    {
        transform.DOLocalRotate(value, duration).SetEase(ease);
    }

    public void DOPivotLocalRotate(Vector3 value = default, float duration = 0.3f, Ease ease = Ease.Linear)
    {
        transform.GetChild(0).DOLocalRotate(value, duration).SetEase(ease);
    }

    public void SetCardData(CardData newCardData)
    {
        cardData = newCardData;
        cardView.UpdateAllText(cardData);
        cardView.UpdateArtSprite(cardData);
    }

    public void ChangeAttack(int newValue)
    {
        cardData.Attack = newValue;
        cardView.UpdateAttack(cardData);
    }

    public void ChangeHealth(int newValue)
    {
        cardData.Health = newValue;
        cardView.UpdateHealth(cardData);
        if (cardData.Health <= 0) currentCardHolder.RemoveCard(this);
    }

    public void ChangeMana(int newValue)
    {
        cardData.Mana = newValue;
        cardView.UpdateMana(cardData);
    }

    public void AddAttack(int value)
    {
        cardData.Attack += value;
        cardView.UpdateAttack(cardData);
    }

    public void AddHealth(int value)
    {
        cardData.Health += value;
        cardView.UpdateHealth(cardData);
        if (cardData.Health <= 0) currentCardHolder.RemoveCard(this);
    }

    public void AddMana(int value)
    {
        cardData.Mana -= value;
        cardView.UpdateMana(cardData);
    }

    public void InitDadc()
    {
        dadc.Init();
    }

    public void SetHandler(BaseHandler value)
    {
        dadc.SetBaseHandler(value);
    }

    public void SetComparer(TagsComparer value)
    {
        dadc.SetComparer(value);
    }

    public void CanDrag(bool value)
    {
        dadc.SetActive(value);
    }


    public void Chosen(bool val)
    {
        active = val;
        cardView.Chosen(val);
    }

    public AbstractCardHolder GetCurrentCardHolder()
    {
        return currentCardHolder;
    }

    public void SetCardHolder(AbstractCardHolder cardHolder)
    {
        currentCardHolder = cardHolder;
    }

    public void SetOrderLayer(int order)
    {
        sortingGroup.sortingOrder = order;
    }

    public int GetOrderLayer()
    {
        return sortingGroup.sortingOrder;
    }

    void OnValidate()
    {
        sortingGroup = GetComponent<SortingGroup>();
    }
}