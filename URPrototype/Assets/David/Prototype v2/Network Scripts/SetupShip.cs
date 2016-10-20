using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SetupShip : NetworkBehaviour {
    public GameObject cameraPrefab;

	// Use this for initialization
    void Start()
    {
        if (isLocalPlayer)
        {
            Ship thisShip = this.GetComponent<Ship>();
            thisShip.enabled = true;
            thisShip.transform.position = new Vector3(0, 10, 0);
            thisShip.transform.Rotate(thisShip.transform.up, 180);
            GameObject camera = Instantiate(cameraPrefab); //Kameran försvinner inte när skeppet försvinner
            camera.GetComponent<CameraFollow>().target = thisShip.transform;
        }
	}
	
	// Update is called once per frame
	void Update () {

	}
}
