using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextureHelper
{

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

    //图片转base64string
    public static string Texture2dToBase64(string texture2d_path)
    {
        //将图片文件转为流文件
        FileStream fs = new System.IO.FileStream(texture2d_path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        byte[] thebytes = new byte[fs.Length];

        fs.Read(thebytes, 0, (int)fs.Length);
        //转为base64string
        string base64_texture2d = Convert.ToBase64String(thebytes);
        return base64_texture2d;
    }
}
