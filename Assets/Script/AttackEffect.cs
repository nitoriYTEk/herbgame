using UnityEngine;
using System.Collections;

public class AttackEffect : MonoBehaviour {
    
    public float offsetTime;
    public float offsetDistance;
    public float speed = 0;
    public bool penetrate = false;

    public int damage;

    Transform parent;

    ParticleSystem effect;
    Collider collider;

    float elapsed = 0;
    bool playing = false;

    // Use this for initialization
    void Start () {
        effect = GetComponent<ParticleSystem>();
        effect.Stop();

        collider = GetComponent<Collider>();
        collider.enabled = false;
    }

    public void init(Transform parent)
    {
        this.parent = parent;
    }

    // Update is called once per frame
    void Update () {

        elapsed += Time.deltaTime;

        if (playing)
        {
            transform.position = transform.position + transform.forward * speed * Time.deltaTime;

            if(elapsed > offsetTime + effect.duration && effect.particleCount == 0)
            {
                Destroy(gameObject);
            }
        }
        else if(elapsed > offsetTime)
        {
            effect.Play();
            playing = true;
            collider.enabled = true;

            transform.position = parent.position + new Vector3(0, 0.6f, 0) + parent.forward * offsetDistance;
            transform.forward = parent.forward;
        }
	}
    
    void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("Enemy"))
        {
            parent.gameObject.GetComponent<Player>().inclHit();

            if (!penetrate)
            {
                Destroy(gameObject);
            }
        }
    }
}
