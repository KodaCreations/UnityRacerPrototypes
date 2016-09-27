using UnityEngine;
using System.Collections;

public class MPPlayer : MonoBehaviour {

    public float MaxMovementSpeed;
    float currentSpeed;
    public float rotationSpeed;
    public float acceleration;
	// Use this for initialization
    void Start()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        Color red = new Color(0.5f, 0.0f, 0.0f, 1.0f);
        renderer.material.shader = Shader.Find("Specular");
        renderer.material.SetColor("_Color", red);
	}
	
	// Update is called once per frame
	void Update () {

        float rotation = 0;
        if (Input.GetKey(KeyCode.W))
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            currentSpeed -= acceleration * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            rotation = -rotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rotation = rotationSpeed * Time.deltaTime;
        }
        if (currentSpeed > MaxMovementSpeed)
            currentSpeed = MaxMovementSpeed;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetupMPPlayer test = gameObject.GetComponent<SetupMPPlayer>();
            test.CmdSpawnBullet();
        }

        transform.Translate(0, 0, currentSpeed * Time.deltaTime);
        transform.Rotate(0, rotation, 0);
	}
}
