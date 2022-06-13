using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public enum GameStage
    {
        Empty = -1,
        StartGame = 0,
        FillDeck,
        RefillDeck,
        AfterFillDeck,
        DistributeCards,
        GiveCardToPlayer,
        GiveCardToOppositePlayer,
        PlayerTurn,
        OppositeTurn,
        EndGame,
        Win,
        Defeat,
        Draw,
    }

    [SerializeField] private GameStage[] StartStages, StandardLoop;
    private GameStage currentStage = GameStage.Empty;
    private int currentStageIndex, phase;

    public static Action<GameStage> StageChangedAction, StageSetAction;
    public static Action ChangeStageAction;

    void Start()
    {
        NextStage();
    }

    void OnEnable()
    {
        ChangeStageAction += NextStage;
    }

    void OnDisable()
    {
        ChangeStageAction -= NextStage;
        StageChangedAction = null;
        StageSetAction = null;
    }

    public void NextStage()
    {
        switch (phase)
        {
            case 0 when currentStageIndex < StartStages.Length:
                SetStage(StartStages[currentStageIndex++]);
                break;
            case 0:
                currentStageIndex = 0;
                SetStage(StandardLoop[currentStageIndex++]);
                phase++;
                break;
            case 1:
            {
                if (currentStageIndex >= StandardLoop.Length) currentStageIndex = 0;
                SetStage(StandardLoop[currentStageIndex++]);
                break;
            }
        }
    }

    public void SetStage(GameStage stage)
    {
        Debug.Log(stage);

        if (currentStage != stage)
        {
            StageChangedAction?.Invoke(stage);
            currentStage = stage;
        }

        StageSetAction?.Invoke(stage);
    }
}