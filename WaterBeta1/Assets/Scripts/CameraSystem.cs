using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSystem : MonoBehaviour {

    private static CameraSystem instance;

    public static CameraSystem Instance()
    {
        if(instance == null)
        {
            Debug.Log("Camera System is Null, Need Check");
        }        
        return instance;
    }
        


    public GameObject followTarget;

    public int backDist;
    public int upDist;
    public int xAngle;
    public int yAngle;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {

    }

    private void CalCamPos()
    {
        Vector3 targetPos = followTarget.transform.position;
        Vector3 dir = followTarget.transform.forward;
        Vector3 pos1 = targetPos - dir * backDist + upDist * Vector3.up;
        
        this.transform.position = pos1;
        
    }
}
