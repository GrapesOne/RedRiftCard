using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ArtLoader
{
    public bool Loading { get; private set; }
    private Sprite defaultSprite;

    public void Init()
    {
        defaultSprite = Resources.Load<Sprite>("DefaultArt");
    }
    public async Task<Sprite> GetSprite(string link, CancellationToken token)
    {
        await UniTask.WaitWhile(() => Loading, cancellationToken: token);
        token.ThrowIfCancellationRequested();
        Loading = true;
        var sprite = defaultSprite;
        if (Client.HasInternet())
        {
            var photo = await ServerRequests.GetPhoto(link, token);
            if (photo != null)
            {
                sprite = Sprite.Create(photo, new Rect(0, 0, photo.width, photo.height), Vector3.up / 2f);
            }
        }

        Loading = false;
        return sprite;
    }
    
}