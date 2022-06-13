using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CardHandler : BaseHandler
{
    [SerializeField] private int maxOrder = 100, middleOrder = 95;
    private int callerOrder;
    

    public override void OnStartDrag(Transform caller)
    {
        var cardController = caller.GetComponent<CardController>();
        cardController.Chosen(true);
        callerOrder = cardController.GetOrderLayer();
        cardController.SetOrderLayer(maxOrder);
    }

    public override void OnEndDrag(Transform caller)
    {
        var cardController = caller.GetComponent<CardController>();
        cardController.Chosen(false);
        cardController.SetOrderLayer(middleOrder);
    }

    public override void OnLateEndDrag(Transform caller)
    {
        var cardController = caller.GetComponent<CardController>();
        cardController.SetOrderLayer(callerOrder);
    }

    public override bool Interaction(Transform caller, Transform callee)
    {
        var cardController = caller.GetComponent<CardController>();
        var currentCardHolder = cardController.GetCurrentCardHolder();
        var newCardHolder = callee.GetComponent<AbstractCardHolder>();
        var result = newCardHolder.TakeCard(cardController);

        if (!result)
        {
            return false;
        }
        currentCardHolder.SilentRemoveCard(cardController);
       
        return true;
    }

    public override bool BadInteraction(Transform caller)
    {
        return true;
    }

   
}
