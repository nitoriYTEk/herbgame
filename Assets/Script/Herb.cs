using UnityEngine;
using System.Collections;

public class Herb : MonoBehaviour {

    public int hp;
    public GameObject reward;

    HerbFactory parent;
    Transform player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void init(HerbFactory parent, Transform player)
    {
        this.player = player;
        this.parent = parent;
        parent.herbCount++;
    }
    

    void OnTriggerEnter(Collider hit)
    {
        transform.rotation = parent.transform.rotation;

        if (hit.CompareTag("PlayerAttack"))
        {
            var effect = hit.gameObject.GetComponent<AttackEffect>();
            hp -= effect.damage;

            if (hp <= 0)
            {
                var obj = Instantiate(reward, transform.position, Quaternion.identity) as GameObject;
                obj.GetComponent<Reward>().init(player);
                parent.herbCount--;
                Destroy(gameObject);
            }
        }
    }
}
