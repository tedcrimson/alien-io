using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBot : TargetObject {

	public float speed;
	internal int direction;

	private bool moving = true;

	protected override void Start()
	{
		base.Start();
		direction = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if(moving)
			Rigidbody.velocity = direction * this.transform.forward * speed;
	}


	public override void OnEnter(Transform t, float speed)
	{
		base.OnEnter(t, speed);
		moving = false;
	}

	public override void OnExit(Transform t, float speed)
	{
		base.OnExit(t, speed);
		moving = true;
	}

	public override void Revive()
	{
		Destroy(this.gameObject);
	}

	
	public void OnCollisionEnter(Collision other)
    {
		if(other.gameObject.name == "Destroyer")
		{
			Destroy(this.gameObject);
		}

		// direction *= Random.Range(-1, 1);
		// transform.Translate(this.transform.position + direction);
    }
}
