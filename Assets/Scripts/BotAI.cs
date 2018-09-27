using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BotAI : UFOController
{
    public float radius;

    private float waitTime = 2f;


	// private TargetObject target;
    private List<Collider> targets;


    protected override void Start()
    {
        base.Start();
        // radius = 5;
        StartCoroutine(Compute());
     }

    protected override void Update()
    {
        if(CanPlay)
            Rigidbody.velocity = moveDir * speed;
        
        base.Update();
    }

    
    protected override bool LevelUp()
    {
        if (!base.LevelUp()) return false;
        radius *= 1.2f;
        waitTime /= 1.2f;
        return true;
    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color;
        Gizmos.DrawWireSphere(this.transform.position, radius);
    }

    public bool ShootRayCast()
    {
        var colliders = Physics.OverlapSphere(transform.position, radius, 1<<9);
        if(colliders == null)return false;

        TargetObject closest = null;
        targets = colliders.Where((x) => {
            var tar = x.GetComponent<TargetObject>();
            if(tar != null && tar != this && !tar.UnderChase){
                bool greater = IsStronger(tar);
                if (greater && (closest == null || (tar.transform.position - transform.position).magnitude < (closest.transform.position - transform.position).magnitude))
                    closest = tar;

                return greater;
            }
            else
                return false;
        }).ToList();

        // Debug.Log(this.name + "Change");

        if(targets==null || closest == null)return false;

        // Debug.Log(this.name + "  " + closest.name);
        

        var ketili = closest.transform.position - this.transform.position;
        ketili.y = 0;

        moveDir = ketili.normalized;
        return true;
    }


    IEnumerator Compute()
    {
        while (true)
        {
            if (!ShootRayCast())
				RandomMove();
			yield return new WaitForSeconds(waitTime);
        }
    }

	private void RandomMove()
	{
		moveDir = (Vector3.right * Random.Range(-radius, radius) + Vector3.forward * Random.Range(-radius, radius)).normalized;
	}	


    
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.layer == 10)
        {
            moveDir = -moveDir;
        }
    }
    
    // void OnCollisionExit(Collision other)
    // {
    //     if(other.gameObject.tag == "Bound")
    //     {
    //         moveDir = -moveDir;
    //         outOfBounds = false;
    //     }
    // }
}
