using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Deck : MonoBehaviour, IGameLoopListener
{
    [SerializeField] CardCreator cardCreator = new CardCreator();

    [SerializeField] Transform CardHolder;
    [SerializeField] private Hand playerHand, oppositeHand;

    private LinkedList<CardController> cards = new LinkedList<CardController>();
    private CancellationTokenSource tokenSource = new CancellationTokenSource();

    private bool playerTurn;

    private void OnEnable()
    {
        GameLoop.StageChangedAction += Listen;
        cardCreator.Init();
    }

    private void OnDisable()
    {
        tokenSource.Cancel();
    }

    public void Listen(GameLoop.GameStage stage)
    {
        switch (stage)
        {
            case GameLoop.GameStage.FillDeck:
                FillDeck();
                break;
            case GameLoop.GameStage.DistributeCards:
                DistributeCards(Random.Range(5, 6));
                break;
            case GameLoop.GameStage.GiveCardToPlayer:
                GiveCard(playerHand, true);
                break;
            case  GameLoop.GameStage.GiveCardToOppositePlayer:
                GiveCard(oppositeHand, false);
                break;
        }
    }


    private async void FillDeck(int deckSize = 14)
    {
        for (int i = 0; i < deckSize; i++)
        {
            if (tokenSource.IsCancellationRequested) return;
            var card = await cardCreator.CreateNewCard(tokenSource.Token);
            card.transform.SetParent(CardHolder);
            card.transform.localPosition = Vector3.zero;
            card.transform.localRotation = Quaternion.identity;
            card.SetOrderLayer(i);
            cards.AddFirst(card);
        }

        GameLoop.ChangeStageAction?.Invoke();
    }

    private async void DistributeCards(int cardsForHand)
    {
        cardsForHand *= 2;
        for (var i = 0; i < cardsForHand; i++)
        {
            bool result;
            var card = cards.Last.Value;
            if (cards.Count == 0) break;
            if (playerTurn)
            {
                result = playerHand.TakeCard(card);
                if (!result) BurnCard(card);
                card.SetComparer(new TagsComparer(TagComponent.Tag.player));
                card.InitDadc();
            }
            else
            {
                result = oppositeHand.TakeCard(card);
                if (!result) BurnCard(card);
            }
            
            cards.RemoveLast();
            playerTurn = !playerTurn;
            await UniTask.Delay(300);
        }

        GameLoop.ChangeStageAction?.Invoke();
    }

    private async void GiveCard(Hand hand, bool isPlayer)
    {
        if (cards.Count != 0)
        {
            var card = cards.Last.Value;
            var result = hand.TakeCard(card);

            if (!result)
            {
                BurnCard(card);
            } else if (isPlayer)
            {
                card.SetComparer(new TagsComparer(TagComponent.Tag.player));
                card.InitDadc();
            }
            cards.RemoveLast();
            await UniTask.Delay(300);
        }

        GameLoop.ChangeStageAction?.Invoke();
    }

    private void BurnCard(CardController cardController)
    {
        //todo throwing in the trash
        Destroy(cardController.gameObject, 0.2f);
    }
}