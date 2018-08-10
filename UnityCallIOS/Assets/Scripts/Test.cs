using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour {

    public RawImage image;
    private Texture2D[] textures = new Texture2D[8];
    Coroutine coroutine;
    // Use this for initialization
    void Start () {
      
       // image.texture = TextureHelper.Base64StringToTexture2D(textureBase64);
        for (int i = 0; i < 8; i++)
        {
            string path = Application.streamingAssetsPath + "/video_6.mp4-"+i+".jpg";
            string textureBase64 = TextureHelper.Texture2dToBase64(path);
            textures[i] = TextureHelper.Base64StringToTexture2D(textureBase64);
        }
    }
	
    public void StatPlay()
    {
        coroutine = StartCoroutine(AnimationPlayThread(8));
    }

    public void Stop()
    {
        StopCoroutine(coroutine);
    }

    IEnumerator AnimationPlayThread(float fps)
    {
        int i = 0;
        while (true)
        {
            if (i < 8)
            {
                // spriteRenderer.sprite = sprites[i];
                image.texture = textures[i];
             i++;
            }
            else
            {
                i = 0;
            }
            yield return new WaitForSeconds(1 / fps);
        }
    }

}
