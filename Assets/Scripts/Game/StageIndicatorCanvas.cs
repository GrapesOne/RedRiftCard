using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class StageIndicatorCanvas : MonoBehaviour, IGameLoopListener
{
    [SerializeField] private TextMeshProUGUI stageName;

    [SerializeField] private CanvasGroup canvasGroup;

    void OnEnable()
    {
        GameLoop.StageChangedAction += Listen;
    }

    private void StartGameCanvas()
    {
        //todo start window Canvas
        canvasGroup.alpha = 0;
        canvasGroup.gameObject.SetActive(true);
        stageName.text = "Loading Game";
        canvasGroup.DOFade(1, 1f).SetEase(Ease.OutSine).OnComplete(() =>
            GameLoop.ChangeStageAction?.Invoke());
    }

    private void HideCanvas()
    {
        canvasGroup.DOFade(0, 1f).SetEase(Ease.InSine).OnComplete(() =>
        {
            GameLoop.ChangeStageAction?.Invoke();
            canvasGroup.gameObject.SetActive(false);
        });
    }


    public void Listen(GameLoop.GameStage stage)
    {
        switch (stage)
        {
            case GameLoop.GameStage.StartGame:
                StartGameCanvas();
                break;
            case GameLoop.GameStage.AfterFillDeck:
                HideCanvas();
                break;
            case GameLoop.GameStage.Defeat:
                //todo Defeat window
                break;
            case GameLoop.GameStage.Win:
                //todo Win window
                break;
            case GameLoop.GameStage.Draw:
                //todo Draw window
                break;
        }
    }
}