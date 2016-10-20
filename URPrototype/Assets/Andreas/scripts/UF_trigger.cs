using UnityEngine;
using System.Collections;

public class UF_trigger : MonoBehaviour {

    public GameObject rightDoor;
    public GameObject leftDoor;
    public float timerMax = 3.0f;
    public float timer = 0.0f;
	
	// Update is called once per frame
	void Update () {
        if (timer >= 0)
            timer -= Time.deltaTime;

        if (timer < 0)
        {
            rightDoor.SetActive(true);
            leftDoor.SetActive(false);
        }
        else
        {
            rightDoor.SetActive(false);
            leftDoor.SetActive(true);
        }
	}

    private void OnTriggerEnter()
    {
        if (timer <= 0)
        {
            timer = timerMax;
        }
    }
}
