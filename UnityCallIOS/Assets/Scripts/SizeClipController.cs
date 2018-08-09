using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SizeClipController : MonoBehaviour
{
    public enum NormalStatus
    {
        none,
        up,
        down,
        left,
        right
    }

    public NormalStatus normalStatus = NormalStatus.none;

    public Transform clipObjTrans;
    public MeshRenderer sizeRenderer;
    public MeshRenderer shortSizeRender_1;
    public MeshRenderer shortSizeRender_2;
    private Material clipMaterial;
    private Material clipMaterial2;
    private Material clipMaterial3;
    public Vector3 clipPos;
    private Vector3 clipNormal;
    [SerializeField]
    bool isCalculate;

    void Start()
    {
        if (sizeRenderer)
        {
            clipMaterial = sizeRenderer.sharedMaterial;
        }
        if (shortSizeRender_1)
        {
            clipMaterial2 = shortSizeRender_1.sharedMaterial;
        }
        if (shortSizeRender_2)
        {
            clipMaterial3 = shortSizeRender_2.sharedMaterial;
        }
    }

    void SetMaterialValue(Vector3 pos, Vector3 normal)
    {
        if (clipMaterial)
        {
            clipMaterial.SetVector("_ClipObjPos", pos);
            clipMaterial.SetVector("_ClipObjNormal", normal);
        }

        if (clipMaterial2)
        {
            clipMaterial2.SetVector("_ClipObjPos", pos);
            clipMaterial2.SetVector("_ClipObjNormal", normal);
        }

        if (clipMaterial3)
        {
            clipMaterial3.SetVector("_ClipObjPos", pos);
            clipMaterial3.SetVector("_ClipObjNormal", normal);
        }
    }

    public void SetCalculate(bool value)
    {
        isCalculate = value;
    }


    void Update()
    {
        if (isCalculate)
        {
            clipPos = clipObjTrans.position;

            if (normalStatus == NormalStatus.down)
            {
                //计算XY平面上的法向量，用XY平面作为剔除面
                clipNormal = clipObjTrans.rotation * Vector3.down;
            }
            else if (normalStatus == NormalStatus.up)
            {
                //计算XY平面上的法向量，用XY平面作为剔除面
                clipNormal = clipObjTrans.rotation * Vector3.up;
            }
            else if (normalStatus == NormalStatus.left)
            {
                //计算XY平面上的法向量，用XY平面作为剔除面
                clipNormal = clipObjTrans.rotation * Vector3.left;
            }
            else if (normalStatus == NormalStatus.right)
            {
                //计算XY平面上的法向量，用XY平面作为剔除面
                clipNormal = clipObjTrans.rotation * Vector3.right;
            }

            SetMaterialValue(clipPos, clipNormal);
        }


    }
}