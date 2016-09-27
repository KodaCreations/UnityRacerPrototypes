using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SetupMPPlayer : NetworkBehaviour
{

    public GameObject bulletPrefab;
    [Command]
    public void CmdSpawnBullet()
    {
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.GetComponent<Bullet>().Inizitalize(transform.position, transform.forward, 10);
        NetworkServer.Spawn(bullet);
    }

    // Use this for initialization
    void Start()
    {
        if (isLocalPlayer)
        {
            this.GetComponent<MPPlayer>().enabled = true;
        }

    }
	// Update is called once per frame
	void Update () {
	
	}
}
