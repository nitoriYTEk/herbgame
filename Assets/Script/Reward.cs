using UnityEngine;
using System.Collections;

public class Reward : MonoBehaviour {

    Rigidbody rigidBody;
    public float chargeIncl = 0;
    public int scoreIncl = 0;

    float elapsed = 0;
    Vector3 hv = new Vector3(0, 1, 0);
    Transform player;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        elapsed += Time.deltaTime * 2;
        rigidBody.velocity = (player.position + hv - transform.position).normalized * elapsed * elapsed;
    }

    public void init(Transform player)
    {
        this.player = player;
    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("Player"))
        {
            hit.gameObject.GetComponent<Player>().receiveReward(chargeIncl, scoreIncl);
            Destroy(gameObject);
        }
    }
}
