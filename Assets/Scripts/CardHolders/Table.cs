using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Table : AbstractCardHolder, IGameLoopListener
{
    [SerializeField] private float distanceBetweenCards;
    [SerializeField] private int maxCards;
    
    [SerializeField] private bool isPlayerTable;

    [SerializeField] private Trash trash;


    public override bool TakeCard(CardController card)
    {
        if (cardsInHolder.Count == maxCards)
        {
            UpdateCardsPosition();
            Debug.LogWarning("FullTable");
            return false;
        }

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
        trash.TakeCard(card);
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
        if (cardsInHolder == null || cardsInHolder.Count == 0) return;
        
        var dbcVector3 = new Vector3(distanceBetweenCards * 2f, 0, 0);
        var reverseDbcVector3 = new Vector3(-distanceBetweenCards * (cardsInHolder.Count - 1), 0, 0);
        var i = 0;
        foreach (var card in cardsInHolder)
        {
            card.SetOrderLayer(i);
            card.DOLocalMove(reverseDbcVector3 + dbcVector3 * i++ );
            card.DOPivotScale(1);
            card.DOLocalRotate();
            card.DOPivotLocalRotate();
        }
        
    }

    public override async Task ForEachCard(Action<CardController> action)
    {
        foreach (var card in cardsInHolder)
        {
            action(card);
            await UniTask.Delay(300);
        }
    }

    public void Listen(GameLoop.GameStage stage)
    {
        switch (stage)
        {
            case GameLoop.GameStage.GiveCardToPlayer:
                UpdateCardsPosition();
                break;
        }
    }
}
