using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[ExecuteInEditMode, ImageEffectAllowedInSceneView]

public class MotionBlurProcessing : MonoBehaviour
{
    private void Awake()
    { 
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.MotionVectors | DepthTextureMode.Depth;

    }


    public Material effectMaterial;

    [Range(0, 10)] public int iterations = 1;
    [Range(0, 5)] public int downRes;


    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        int width = source.width >> downRes;
        int height = source.height >> downRes;

        RenderTexture temp = RenderTexture.GetTemporary(source.width, source.height);



        //Graphics.Blit(source, temp);

        //for (int i = 0; i < iterations; i++)
        //{
        //    RenderTexture temp2 = RenderTexture.GetTemporary(width, height);

        //    Graphics.Blit(temp, temp2, effectMaterial);

        //    RenderTexture.ReleaseTemporary(temp);

        //    temp = temp2;

        //}


        //Graphics.Blit(temp, destination);


        Graphics.Blit(source, destination, effectMaterial);

        RenderTexture.ReleaseTemporary(temp);
    }
}