using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Trash : AbstractCardHolder
{
    public override bool TakeCard(CardController card)
    {
        cardsInHolder.AddFirst(card); 
        card.SetCardHolder(this);
        card.transform.SetParent(cardsHolder);
        UpdateCardsPosition();
        return true;
    }

    public override bool RemoveCard(CardController card)
    {
        if (!cardsInHolder.Contains(card)) return false;
        cardsInHolder.Remove(card);
        UpdateCardsPosition();
        return true;
    }
    public override bool SilentRemoveCard(CardController card)
    {
        if (!cardsInHolder.Contains(card)) return false;
        cardsInHolder.Remove(card);
        UpdateCardsPosition();
        return true;
    }

    protected override void UpdateCardsPosition()
    {
        foreach (var card in cardsInHolder)
        {
            card.DOLocalMove();
            card.DOLocalRotate();
            card.DOPivotLocalRotate();
        }
    }

    public override async Task ForEachCard(Action<CardController> action)
    {
        foreach (var card in cardsInHolder)
        {
            action(card);
            await UniTask.Yield();
        }
    }
}
