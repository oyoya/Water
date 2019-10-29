using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 获取水平高度和法线
/// </summary>
public class LiquidCamera : MonoBehaviour {

    private Camera camera;

    private RenderTexture curTexture;
    private RenderTexture preTexture;
    private RenderTexture heightTexture;
    private RenderTexture normalTexture;

    private CommandBuffer commandBuf;

    private Material forceMat;
    private Material waveMat;
    private Material normalMat;

    private Vector4 liquidParams;

    /// <summary>
    /// 初始化参数和对象
    /// </summary>
    public void Init(float width, float height, float depth, Vector4 liquidParams, float force, int texSize)
    {
        camera = gameObject.AddComponent<Camera>();
        camera.aspect = width / height;
        camera.backgroundColor = Color.black;
        camera.cullingMask = 0;
        camera.depth = 0;
        camera.farClipPlane = depth;
        camera.nearClipPlane = 0;
        camera.orthographic = true;
        camera.orthographicSize = height * 0.5f;
        camera.clearFlags = CameraClearFlags.Depth;
        camera.allowHDR = false;

        commandBuf = new CommandBuffer();
        camera.AddCommandBuffer(CameraEvent.AfterImageEffectsOpaque, commandBuf);

        Shader.SetGlobalFloat("internal_Force", force);
        forceMat = new Material(Shader.Find("Unlit/Force"));

        curTexture = RenderTexture.GetTemporary(texSize, texSize, 16);
        curTexture.name = "Cur";
        preTexture = RenderTexture.GetTemporary(texSize, texSize, 16);
        preTexture.name = "Pre";
        heightTexture = RenderTexture.GetTemporary(texSize, texSize, 16);
        heightTexture.name = "LiquidHeightTexture";
        normalTexture = RenderTexture.GetTemporary(texSize, texSize, 16);
        normalTexture.anisoLevel = 1;
        normalTexture.name = "LiquidNormalTexture";
        camera.targetTexture = curTexture;


        this.liquidParams = liquidParams;

        normalMat = new Material(Shader.Find("Unlit/LiquidNormal"));
        waveMat = new Material(Shader.Find("Unlit/LiquidHeight"));
        waveMat.SetVector("_LiquidParams", liquidParams);
    }

    public void DrawRender(Renderer render)
    {
        if(render)
        {
            commandBuf.DrawRenderer(render, forceMat);
        }
    }

    public void DrawMesh(Mesh mesh, Matrix4x4 matrix)
    {
        if(mesh)
        {
            commandBuf.DrawMesh(mesh, matrix, forceMat);
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        waveMat.SetTexture("_PreTex", preTexture);
        Graphics.Blit(source, destination, waveMat);
        Graphics.Blit(destination, heightTexture);
        Graphics.Blit(heightTexture, normalTexture, normalMat);
        Graphics.Blit(source, preTexture);
    }

    private void OnPostRender()
    {
        commandBuf.Clear();
        commandBuf.ClearRenderTarget(true, false, Color.black);
        commandBuf.SetRenderTarget(curTexture);

        Shader.SetGlobalTexture("liquidHeightTexture", heightTexture);
        Shader.SetGlobalTexture("liquidNormalTexture", normalTexture);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDestroy()
    {
        if(curTexture)
        {
            RenderTexture.ReleaseTemporary(curTexture);
        }
        if(preTexture)
        {
            RenderTexture.ReleaseTemporary(preTexture);
        }
        if(heightTexture)
        {
            RenderTexture.ReleaseTemporary(heightTexture);
        }
        if(normalTexture)
        {
            RenderTexture.ReleaseTemporary(normalTexture);
        }
        if(commandBuf != null)
        {
            commandBuf.Clear();
        }
        if(forceMat)
        {
            Destroy(forceMat);
        }
        if(waveMat)
        {
            Destroy(waveMat);
        }
        if(normalMat)
        {
            Destroy(normalMat);
        }
    }
}
 