using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidSystem : MonoBehaviour {

    private static LiquidSystem instance;

    public static LiquidSystem Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<LiquidSystem>();
            }
            return instance;
        }
    }

    public float liquidCellSize = 0f;

    public float liquidWidth = 0f;

    public float liquidLength = 0f;

    public float liquidDepth;

    public int heightMapSize;

    /// <summary>
    /// 粘度系数
    /// </summary>
    [SerializeField]
    public float viscosity;
    /// <summary>
    /// 波速
    /// </summary>
    [SerializeField]
    public float velocity;
    /// <summary>
    /// 力度
    /// </summary>
    [SerializeField]
    public float forceFactor;

    [SerializeField]
    private Material liquidMaterial;

    private Mesh liquidMesh;
    private MeshFilter liquidMeshFilter;
    private MeshRenderer liquidMeshRenderer;
    private LiquidCamera liquidCamera;

    private Vector4 liquidParams;

    private Vector4 liquidArea;

	// Use this for initialization
	void Start () {
        if(!CheckAvailable())
        {
            return;
        }

        liquidMeshFilter = gameObject.GetOrAddComponent<MeshFilter>();
        liquidMeshRenderer = gameObject.GetOrAddComponent<MeshRenderer>();

        liquidMesh = Utils.GenerateMesh(liquidWidth, liquidLength, liquidCellSize);
        liquidMeshFilter.sharedMesh = liquidMesh;
        liquidMeshRenderer.sharedMaterial = liquidMaterial;

        liquidArea = new Vector4(transform.position.x - liquidWidth / 2f, transform.position.z - liquidLength / 2f,
            transform.position.x + liquidWidth / 2f, transform.position.z + liquidLength / 2f);

        liquidCamera = new GameObject("LiquidCamera").AddComponent<LiquidCamera>();
        liquidCamera.transform.SetParent(transform);
        liquidCamera.transform.position = Vector3.zero;
        liquidCamera.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
        liquidCamera.Init(liquidWidth, liquidLength, liquidDepth, liquidParams, forceFactor, heightMapSize);


	}

    public void DrawRender(Renderer render)
    {
        liquidCamera.DrawRender(render);
    }

    public void DrawMesh(Mesh mesh, Matrix4x4 matrix)
    {
        liquidCamera.DrawMesh(mesh, matrix);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private bool CheckAvailable()
    {
        if(liquidCellSize <= 0f || liquidLength <= 0f || liquidWidth <= 0)
        {
            Debug.LogError("参数不合法！liquidCellSize：" + liquidCellSize + "|liquidLength:" + liquidLength + "|liquidWidth:" + liquidWidth);
            return false;
        }

        return CalLiquidParams();
    }

    private bool CalLiquidParams()
    {
        if(velocity <= 0 || viscosity <= 0)
        {
            return false;
        }

        float t = Time.fixedDeltaTime;
        float d = 1.0f / heightMapSize;
        float maxvelovity = d * Mathf.Sqrt(viscosity * t + 2) / (2 * t);
        float c = velocity * maxvelovity;

        float f1 = c * c * t * t / (d * d);
        float f2 = viscosity * t - 2;
        float f3 = viscosity * t + 2;

        float k1 = (4 - 8 * f1) / f3;
        float k2 = f2 / f3;
        float k3 = 2 * f1 / f3;

        liquidParams = new Vector4(k1, k2, k3, d);
        return true;
    }
}
