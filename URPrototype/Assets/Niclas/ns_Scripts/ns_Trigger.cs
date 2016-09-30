using UnityEngine;
using System.Collections;

public class ns_Trigger : MonoBehaviour {

    public GameObject rightDoor;
    public GameObject leftDoor;
    public float timerMax = 5.0f;
    public float timer = 0.0f;

    public void Update()
    {
        if (timer >= 0)
            timer -= Time.deltaTime;

        if (timer < 0)
        {
            rightDoor.SetActive(false);
            leftDoor.SetActive(true);
        }
        else
        {
            rightDoor.SetActive(true);
            leftDoor.SetActive(false);
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
