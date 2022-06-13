using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ServerRequests
{
    public static async Task<Texture2D> GetPhoto(
        string photoUrl,
        CancellationToken token = new CancellationToken(),
        int width = 200,
        int height = 300)
    {
        var imageData = await Client.DownloadFileBytesAsync(photoUrl, token);
        if (imageData.status == Status.NoInternetConnection
            || imageData.status == Status.TaskCanceled) return null;
        var t = new Texture2D(1, 1);
        t.LoadImage(imageData.data);
        TextureScale.Bilinear(t, width, height);
        return t;
    }
}