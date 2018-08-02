using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;

public class IOSTest : MonoBehaviour
{

    public Text debug;
    public RawImage image;

    [DllImport("__Internal")]
    private static extern void IOS_OpenCamera();
    [DllImport("__Internal")]
    private static extern void IOS_OpenAlbum();

    public void OpenCamera()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IOS_OpenCamera();
        }
    }

    public void OpenAlbum()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IOS_OpenAlbum();
        }
    }

    void IOSBack(string base64)
    {
        debug.text = base64;
        //StartCoroutine(LoadTexture(base64));
        StartCoroutine(LoadVideo(base64));
    }

    IEnumerator LoadTexture(string url)
    {
        byte[] bytes = System.Convert.FromBase64String(url);
        image.texture = Base64StringToTexture2D(url);
        WWWForm form = new WWWForm();
        form.AddBinaryData("myFile", bytes, "Temp.jpg");
        using (var w = UnityWebRequest.Post("http://192.168.1.125:8080/test_upload.php", form))
        {
            yield return w.SendWebRequest();
            if (w.isNetworkError || w.isHttpError)
            {
                debug.text = w.error;
            }
            else
            {
                debug.text = "Finished Uploading Screenshot";
            }
        }  
    }

    IEnumerator LoadVideo(string url)
    {
        byte[] bytes = System.Convert.FromBase64String(url);
        WWWForm form = new WWWForm();
        form.AddBinaryData("myFile", bytes, "Temp.mp4");
        using (var w = UnityWebRequest.Post("http://192.168.1.125:8080/test_upload.php", form))
        {
            yield return w.SendWebRequest();
            if (w.isNetworkError || w.isHttpError)
            {
                debug.text = w.error;
            }
            else
            {
                debug.text = "Finished Uploading Screenshot";
            }
        } 
    }

    /// <summary>
	/// 将ios传过的string转成u3d中的texture
	/// </summary>
	/// <param name="base64"></param>
	/// <returns></returns>
	public static Texture2D Base64StringToTexture2D(string base64)
    {
        Texture2D tex = new Texture2D(4, 4, TextureFormat.ARGB32, false);
        try
        {
            byte[] bytes = System.Convert.FromBase64String(base64);
            tex.LoadImage(bytes);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
        }
        return tex;
    }

    public void Test()
    {
        string path = Application.streamingAssetsPath + "/" + "video_1.mp4";
        //StartCoroutine(LoadTexture(path));
        StartCoroutine(LoadVideo(path));
    }


}
