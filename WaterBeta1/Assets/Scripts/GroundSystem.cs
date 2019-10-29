using UnityEngine;

public class GroundSystem : MonoBehaviour {

    public float groundCellSize = 0f;

    public float groundWidth = 0f;

    public float groundLength = 0f;

    [SerializeField]
    private Material groundMaterial;

    private Mesh groundMesh;
    private MeshFilter groundMeshFilter;
    private MeshRenderer groundMeshRenderer;

	// Use this for initialization
	void Start () {
        if(groundWidth <= 0f || groundLength <= 0f || groundCellSize <= 0f)
        {
            Debug.LogError("参数不合法。小于0");
            return;
        }

        groundMeshFilter = gameObject.GetComponent<MeshFilter>();
        if(!groundMeshFilter)
        {
            groundMeshFilter = gameObject.AddComponent<MeshFilter>();
        }
        groundMeshRenderer = gameObject.GetComponent<MeshRenderer>();
        if(!groundMeshRenderer)
        {
            groundMeshRenderer = gameObject.AddComponent<MeshRenderer>();
        }

        if(!groundMesh)
        {
            groundMesh = Utils.GenerateMesh(groundWidth, groundLength, groundCellSize);
        }
        groundMeshFilter.sharedMesh = groundMesh;
        groundMeshRenderer.sharedMaterial = groundMaterial;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
