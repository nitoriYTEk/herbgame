using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

    public GameObject[] attackPrefab;
    public GameObject[] chargeAttackPrefab;

    public GameController controller;
    public AudioPlayer audioPlayer;

    public float charge = 0;
    public Slider chargeSlider;
    public float chargePerShot = 0.02f;
    bool isCharged = false;
    public ParticleSystem chargeEffect;

    public float speed = 5.0f;

    public int shotPerSec = 4;
    float shotWaitTime;

    int idleState;
    int runState;

    int attack1State;
    int attack2State;
    int chargeAttackState;

    // Use this for initialization
    void Start () {
        shotWaitTime = 1.0f / shotPerSec;

        idleState = Animator.StringToHash("Base Layer.Idle");
        runState = Animator.StringToHash("Base Layer.Run");

        attack1State = Animator.StringToHash("Base Layer.Attack1");
        attack2State = Animator.StringToHash("Base Layer.Attack2");
        chargeAttackState = Animator.StringToHash("Base Layer.ChargeAttack");
    }

	
	// Update is called once per frame
	void Update () {
        var animator = GetComponent<Animator>();

        if (!isCharged)
        {
            if(charge >= 1.0f)
            {
                isCharged = true;
                charge = 1.0f;
                chargeEffect.Stop();
                chargeEffect.Play();
                audioPlayer.Play(2);
            }
        }

        Vector3 direction = Vector3.zero;

        var nowAnimation = animator.GetCurrentAnimatorStateInfo(0);

        animator.SetBool("running", false);
        if (controller.inputable())
        {
            shotWaitTime += Time.deltaTime;
            bool nowAttack = false;

            if (Input.GetKeyDown("x") && isCharged)
            {
                charge = 0;
                nowAttack = true;
                isCharged = false;
                animator.SetTrigger("chargeAttack");
                foreach (var c in chargeAttackPrefab)
                {
                    GameObject effect = Instantiate(c, transform.position, transform.rotation) as GameObject;
                    effect.GetComponent<AttackEffect>().init(transform);
                }
                audioPlayer.Play(1);
            }
            else if (Input.GetKey("z"))
            {
                if (shotWaitTime > 1.0f / shotPerSec)
                {
                    nowAttack = true;
                    shotWaitTime -= 1.0f / shotPerSec;
                    if (nowAnimation.fullPathHash == idleState || nowAnimation.fullPathHash == runState || nowAnimation.fullPathHash == attack1State || nowAnimation.fullPathHash == attack2State)
                    {
                        animator.SetTrigger("attack");
                        foreach (var a in attackPrefab)
                        {
                            GameObject effect = Instantiate(a, transform.position, transform.rotation) as GameObject;
                            effect.GetComponent<AttackEffect>().init(transform);
                        }
                        charge += chargePerShot;
                        audioPlayer.Play(0);
                    }
                }
            }

            if (shotWaitTime > 1.0f / shotPerSec)
                shotWaitTime = 1.0f / shotPerSec;

            {
                if (Input.GetKey("up"))
                {
                    direction += new Vector3(0, 0, 1);
                    animator.SetBool("running", true);
                }
                if (Input.GetKey("down"))
                {
                    direction += new Vector3(0, 0, -1);
                    animator.SetBool("running", true);
                }
                if (Input.GetKey("right"))
                {
                    direction += new Vector3(1, 0, 0);
                    animator.SetBool("running", true);
                }
                if (Input.GetKey("left"))
                {
                    direction += new Vector3(-1, 0, 0);
                    animator.SetBool("running", true);
                }

                direction *= speed * Time.deltaTime;

                if (nowAnimation.fullPathHash == attack1State || nowAnimation.fullPathHash == attack2State || nowAnimation.fullPathHash == chargeAttackState || animator.IsInTransition(0))
                    direction /= 12;

                GetComponent<CharacterController>().Move(direction);
                //transform.position += direction;

                if (direction.magnitude > 0)
                    transform.forward = direction;
            }
        }

        chargeSlider.value = charge;
    }

    public void receiveReward(float charge, int score)
    {
        this.charge += charge;
        controller.addScore(score);
        audioPlayer.Play(3);
    }
    
    public void inclHit()
    {
        controller.inclHit();
    }
}
