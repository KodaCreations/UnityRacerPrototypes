using UnityEngine;
using System.Collections;

public class WASD : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey("w"))
            transform.position += transform.forward * Time.deltaTime * 20;
        else if (Input.GetKey("s"))
            transform.position += -transform.forward * Time.deltaTime * 10;
        if (Input.GetKey("a"))
            transform.Rotate(transform.up, -50 * Time.deltaTime);
        if (Input.GetKey("d"))
            transform.Rotate(transform.up, 50 * Time.deltaTime);
    }
}
