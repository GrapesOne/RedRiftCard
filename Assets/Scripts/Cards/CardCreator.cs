using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class CardCreator
{
    [SerializeField] private string[] titles, descriptions;

    private ICardDataLoader cardDataLoader;
    [SerializeField] private CardController cardTemplate;
    [SerializeField] private CardHandler handler;

    public void Init()
    {
        cardDataLoader = new DefaultCardDataLoader(titles, descriptions);
        cardDataLoader.Init();
    }

    public async Task<CardController> CreateNewCard(CancellationToken token)
    {
        var cardData = await cardDataLoader.LoadCardData(token);
        var go = Object.Instantiate(cardTemplate);
        go.SetCardData(cardData);
        go.SetHandler(handler);
        return go;
    }
}