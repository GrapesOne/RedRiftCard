using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class DefaultCardDataLoader : ICardDataLoader
{
    private readonly ArtLoader artLoader = new ArtLoader();

    private string[] titles, descriptions;

    public DefaultCardDataLoader(string[] titles, string[] descriptions)
    {
        this.titles = titles;
        this.descriptions = descriptions;
    }
    public void Init()
    {
        artLoader.Init();
    }
    public async Task<CardData> LoadCardData(CancellationToken token) =>
        new CardData
        {
            Art = await artLoader.GetSprite("https://picsum.photos/64/128", token),
            Title = titles.RandomElement(),
            Description = descriptions.RandomElement(),
            Attack = Random.Range(1, 5),
            Health = Random.Range(1, 5),
            Mana = Random.Range(1, 5)
        };
}
