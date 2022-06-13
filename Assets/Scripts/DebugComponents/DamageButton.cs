using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageButton : MonoBehaviour, IGameLoopListener
{
    [SerializeField] private Button button;
    [SerializeField] private Hand playerHand, oppositeHand;
    private Hand currentHand;

    void OnEnable()
    {
        GameLoop.StageChangedAction += Listen;
        button.interactable = false;
        button.onClick.AddListener(InflictDamage);
    }

    void OnDisable()
    {
        button.onClick.RemoveListener(InflictDamage);
        GameLoop.StageChangedAction -= Listen;
    }

    async void InflictDamage()
    {
        button.interactable = false;
        await currentHand.ForEachCard(InternalInflictDamage);
        GameLoop.ChangeStageAction?.Invoke();
    }

    void InternalInflictDamage(CardController card)
    {
        if (card == null) return;
        switch (Random.Range(0, 3))
        {
            case 0:
                card.ChangeAttack(Random.Range(-2, 10));
                break;
            case 1:
                card.ChangeHealth(Random.Range(-2, 10));
                break;
            case 2:
                card.ChangeMana(Random.Range(-2, 10));
                break;
        }
    }

    void OnValidate()
    {
        button = GetComponent<Button>();
    }

    public void Listen(GameLoop.GameStage stage)
    {
        switch (stage)
        {
            case GameLoop.GameStage.PlayerTurn:
                currentHand = playerHand;
                button.interactable = true;
                break;
            case GameLoop.GameStage.OppositeTurn:
                currentHand = oppositeHand;
                button.interactable = true;
                break;
        }
    }
}