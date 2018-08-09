using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using RenderHeads.Media.AVProVideo;
using XGameFramework;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class IOSTest : MonoBehaviour
{

    public Text debug;
    public RawImage image;
    public MediaPlayer mediaPlayer;

    [DllImport("__Internal")]
    private static extern void IOS_OpenCamera();
    [DllImport("__Internal")]
    private static extern void IOS_OpenAlbum(int posX, int posY, int width, int height);
    [DllImport("__Internal")]
    private static extern void IOS_ShowView(int posX, int posY, int width, int height);
    [DllImport("__Internal")]
    private static extern void IOS_CloseView();

    private  int ScaleFactor = 2; //设备分辨率/逻辑分辨率
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
            IOS_OpenAlbum(200/ ScaleFactor, Mathf.Abs(-90 / ScaleFactor), 350/ ScaleFactor, 500/ ScaleFactor); 
        }
    }

    public void ShowView(int posX, int posY, int width, int height)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IOS_ShowView(posX, posY, width, height);
        }
    }

    public void CloseView()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IOS_CloseView();
        }
    }

    void IOSBack(string base64)
    {
        debug.text = base64;
       // XDebugger.Log("视频：" + base64);
        //StartCoroutine(LoadTexture(base64));
        StartCoroutine(LoadVideo(base64));
    }

    void IOSBack2(string videoTime)
    {
        debug.text += "视频时长：" + videoTime;
        XDebugger.Log("视频时长：" + videoTime);
        XDebugger.Log("真机的宽高：" +Screen.width+","+Screen.height);
        XDebugger.Log("dpi：" + Screen.dpi);
    }

    IEnumerator LoadTexture(string url)
    {
        byte[] bytes = System.Convert.FromBase64String(url);
        image.texture = Base64StringToTexture2D(url);
        WWWForm form = new WWWForm();
        form.AddBinaryData("myFile", bytes, "Temp.jpg");
        using (var w = UnityWebRequest.Post("http://172.16.1.131:8080/test_upload.php", form))
        {
            yield return w.SendWebRequest();
            if (w.isNetworkError || w.isHttpError)
            {
                debug.text = w.error;
                XDebugger.Log(w.error);
            }
            else
            {
                debug.text = "Finished Uploading Screenshot";
                XDebugger.Log("Finished Uploading Screenshot");
            }
        }  
    }

    IEnumerator LoadVideo(string url)
    {
        byte[] bytes = System.Convert.FromBase64String(url);
        WWWForm form = new WWWForm();
        form.AddBinaryData("myFile", bytes, "Temp.mp4");
        using (var w = UnityWebRequest.Post("http://172.16.1.131:8080/test_upload.php", form))
        {
            yield return w.SendWebRequest();
            if (w.isNetworkError || w.isHttpError)
            {
                debug.text = w.error;
                XDebugger.Log(w.error);
            }
            else
            {
                debug.text = "Finished Uploading Screenshot";
                XDebugger.Log("Finished Uploading Screenshot");
            }
        } 
    }

    IEnumerator LoadVideo2(string url)
    {
        try
        {
            mediaPlayer.m_VideoPath = url; //"video_1.mp4";
            bool isOpenVideoFromFile = mediaPlayer.OpenVideoFromFile(mediaPlayer.m_VideoLocation, mediaPlayer.m_VideoPath, mediaPlayer.m_AutoStart);
            mediaPlayer.Control.MuteAudio(false);
            mediaPlayer.Control.SetLooping(true);

            XDebugger.Log("isOpenVideoFromFile =" + isOpenVideoFromFile);
        }
        catch (System.Exception e)
        {
            XDebugger.Log("Exception =" + e.Message);
        }
        yield return mediaPlayer;
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
        string path = Application.streamingAssetsPath +"/" + "video_1.mp4";
        //StartCoroutine(LoadTexture(path));
        StartCoroutine(LoadVideo2(path));
    }
}
