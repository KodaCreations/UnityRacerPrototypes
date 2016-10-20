 using UnityEngine;
using System.Collections;

public class CubeMovement : MonoBehaviour {

    public BezierSpline spline;
    public float speed;
    float currentTime = 0;
    
    // Use this for initialization
	void Start ()
    {
        spline.GetPoint(0f);
	}
	void CalculateConstantSpeedOnSpline()
    {
        Vector3 newPosition = spline.GetPointConstantSpeed(ref currentTime, speed * Time.deltaTime);
        transform.position = newPosition;
        Quaternion rotation = spline.GetRotationAt(currentTime);
        Vector3 lookat = spline.GetDirection2(currentTime);
        transform.rotation = rotation;
    }

	// Update is called once per frame
	void Update ()
    {
        CalculateConstantSpeedOnSpline();
        //currentTime += Time.deltaTime * 0.2f;
        //if (currentTime > 1.0f)
        //    currentTime = 0.0f;
        //transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
        //transform.position = spline.GetPoint(currentTime);
	}
}
