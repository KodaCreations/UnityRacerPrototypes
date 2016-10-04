using UnityEngine;
using System.Collections;

public class Arrows : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey("up"))
            transform.position += transform.forward * Time.deltaTime * 20;
        else if (Input.GetKey("down"))
            transform.position += -transform.forward * Time.deltaTime * 10;
        if (Input.GetKey("left"))
            transform.Rotate(transform.up, -50 * Time.deltaTime);
        if (Input.GetKey("right"))
            transform.Rotate(transform.up, 50 * Time.deltaTime);
    }
}
