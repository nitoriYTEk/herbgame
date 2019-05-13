using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

    public Transform target;    // ターゲットへの参照

    float maxVelocity;

    Vector3 velocity;

    // Use this for initialization
    void Start () {
        velocity = Vector3.zero;
        maxVelocity = target.gameObject.GetComponent<Player>().speed;
	}

    void Update()
    {
        var camera = GetComponent<Camera>();
        var targetPoint = camera.WorldToViewportPoint(target.position);
        
        if (targetPoint.x < 0.25f)
        {
            velocity.x -= (maxVelocity * (0.25f - targetPoint.x) / 0.125f) * Time.deltaTime * 5;
            velocity.x = Mathf.Max(velocity.x, -maxVelocity);
        } else if (targetPoint.x > 0.75f)
        {
            velocity.x += (maxVelocity * (targetPoint.x - 0.75f) / 0.125f) * Time.deltaTime * 5;
            velocity.x = Mathf.Min(velocity.x, maxVelocity);
        } else {
            if (velocity.x > 0) {
                velocity.x -= maxVelocity * Time.deltaTime * 5;
                velocity.x = Mathf.Max(velocity.x, 0);
            } else if (velocity.x < 0)
            {
                velocity.x += maxVelocity * Time.deltaTime * 5;
                velocity.x = Mathf.Min(velocity.x, 0);
            }
        }

        if (targetPoint.y < 0.25f)
        {
            velocity.z -= (maxVelocity * (0.25f - targetPoint.y) / 0.125f) * Time.deltaTime * 5;
            velocity.z = Mathf.Max(velocity.z, -maxVelocity);
        }
        else if (targetPoint.y > 0.75f)
        {
            velocity.z += (maxVelocity * (targetPoint.y - 0.75f) / 0.125f) * Time.deltaTime * 5;
            velocity.z = Mathf.Min(velocity.z, maxVelocity);
        }
        else {
            if (velocity.z > 0)
            {
                velocity.z -= maxVelocity * Time.deltaTime * 5;
                velocity.z = Mathf.Max(velocity.z, 0);
            }
            else if (velocity.z < 0)
            {
                velocity.z += maxVelocity * Time.deltaTime * 5;
                velocity.z = Mathf.Min(velocity.z, 0);
            }
        }

        transform.position += velocity * Time.deltaTime;
    }
}
