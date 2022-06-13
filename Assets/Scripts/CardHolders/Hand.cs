using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Hand : AbstractCardHolder, IGameLoopListener
{
    [SerializeField] private float maxAngle, standardAngle, distanceBetweenCards, upOffset;
    [SerializeField] private int maxCards;
    
    [SerializeField] private bool isPlayerHand;

    [SerializeField] private Trash trash;
    

    void OnEnable()
    {
        GameLoop.StageChangedAction += Listen;
    }

    public override bool TakeCard(CardController card)
    {
        if (cardsInHolder.Count == maxCards)
        {
            Debug.LogWarning("FullHand");
            return false;
        }

        if (isPlayerHand) cardsInHolder.AddFirst(card);
        else cardsInHolder.AddLast(card);
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

        var median = (cardsInHolder.Count - 1) * 0.5f;
        Vector3 upVector3 = Vector3.zero,
            dbcVector3,
            reverseDbcVector3,
            angleVector3 = Vector3.zero,
            reverseAngleVector3 = Vector3.zero;

        if (cardsInHolder.Count <= 3)
        {
            dbcVector3 = new Vector3(distanceBetweenCards * 2f, 0, 0);
            reverseDbcVector3 = new Vector3(-distanceBetweenCards * (cardsInHolder.Count - 1), 0, 0);
        }
        else
        {
            upVector3 = new Vector3(0, upOffset, 0);
            var angle = standardAngle * cardsInHolder.Count <= maxAngle ? standardAngle : maxAngle / cardsInHolder.Count;
            angleVector3 = new Vector3(0, 0, angle);
            dbcVector3 = new Vector3(distanceBetweenCards, 0, 0);
            reverseDbcVector3 = new Vector3(-distanceBetweenCards * median, 0, 0);
            reverseAngleVector3 = new Vector3(0, 0, angle * median);
        }
        
        var i = 0;
        foreach (var card in cardsInHolder)
        {
            card.SetOrderLayer(i);
            card.DOLocalMove(reverseDbcVector3 + dbcVector3 * i - upVector3 * Mathf.Abs(median - i));
            card.DOLocalRotate();
            card.DOPivotLocalRotate(reverseAngleVector3 - angleVector3 * i++);
        }
    }

    public override async Task ForEachCard(Action<CardController> action)
    {
        foreach (var card in  cardsInHolder.ToArray())
        {
            card.Chosen(true);
            action(card);
            await UniTask.Delay(300);
            card.Chosen(false);
        }
    }

    public void Listen(GameLoop.GameStage stage)
    {
        if (isPlayerHand)
        {
            if (stage == GameLoop.GameStage.PlayerTurn)
            {
                foreach (var card in cardsInHolder)
                {
                    card.CanDrag(true);
                }
            }
            else
            {
                foreach (var card in cardsInHolder)
                {
                    card.CanDrag(false);
                }
            }
        }
        else
        {
            foreach (var card in cardsInHolder)
            {
                card.CanDrag(false);
            }
            if (stage == GameLoop.GameStage.OppositeTurn)
            {
                //todo player turn logic
            }
            else
            {
                //todo wait your turn logic
            }
        }
    }
}