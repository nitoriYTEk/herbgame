using UnityEngine;
using System.Collections;

public class HerbFactory : MonoBehaviour {

    public GameController controller;

    public int maxHerb = 100;
    public int herbCount = 0;
    public GameObject[] target;
    public float[] targetRate;

    Transform player;
    float elapsed = 0;

	// Use this for initialization
	void Start () {
        var obj = GameObject.FindGameObjectWithTag("Player");
        player = obj.GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        elapsed += Time.deltaTime;
        
        int herbability = maxHerb - herbCount;
        
        if (controller.herbGeneratable() && Random.Range(0f, maxHerb) < 10 * herbability * Time.deltaTime * (1.0f + controller.getTimeRatio()))
        {
            var rate = Random.Range(0f, 100f);
            for (int i = 0; i < targetRate.Length; i++)
            {
                if ((rate -= targetRate[i]) < 0) {
                    var obj = Instantiate(target[i], new Vector3(Random.Range(-9f, 9f), 0.5f, Random.Range(-9f, 9f)), Quaternion.identity) as GameObject;
                    obj.GetComponent<Herb>().init(this, player);
                    break;
                }
            }
        }
    }
}
