using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{

    [SyncVar]
    public Vector3 direction;
    public float speed;

    public void Inizitalize(Vector3 _startPosition, Vector3 _direction, float _speed)
    {
        transform.position = _startPosition;
        direction = _direction;
        speed = _speed;
    }
    // Use this for initialization
    void Start()
    {

    }
	// Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
	}
}
