using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObject : MonoBehaviour
{

	[SerializeField]
    private int level;
    private Rigidbody rigid;

	private bool canPickUp;

	private Vector3 startPos;
	private Quaternion startRot;
	private Vector3 startScale;


	protected static int MAX_LEVEL = 15;

	public bool UnderChase{
		get; set;
	}

    public int Level
    {
        get { return level; }
		set { level = value;}
    }

    public Rigidbody Rigidbody
    {
        get { return rigid; }
    }

	protected virtual void Start()
	{
		rigid = GetComponent<Rigidbody>();
		canPickUp = true;
		startPos = this.transform.localPosition;
		startRot = this.transform.localRotation;
		startScale = this.transform.localScale;
	}

	protected virtual bool LevelUp()
	{
		if(level < MAX_LEVEL){
			level++;
			return true;
		}
		return false;
	}
	
    public virtual bool IsStronger(TargetObject other)
    {
        return this.level > other.level && other.canPickUp;
    }
	

	public virtual void OnEnter(Transform t, float speed)
	{
        Rigidbody.useGravity = true;
        Rigidbody.isKinematic= false;
        Rigidbody.AddTorque((t.position - this.transform.position).normalized * speed, ForceMode.Impulse);
	}

	public virtual void OnStay(Transform t, float speed)
	{
        Rigidbody.velocity = (t.position - this.transform.position).normalized * speed;
		transform.localScale /= 1.1f;
	}

	public virtual void OnExit(Transform t, float speed)
	{
		UnderChase = false;
		transform.localScale = startScale;


	}

	public virtual void Revive()
	{
		StartCoroutine(ReviveInSeconds());
	}

	IEnumerator ReviveInSeconds()
	{
		canPickUp = false;
		var meshes = GetComponentsInChildren<MeshRenderer>();
		foreach (var item in meshes)
		{
			item.enabled = false;
		}
		rigid.velocity = Vector3.zero;
        Rigidbody.isKinematic= true;
        Rigidbody.useGravity= false;

		UnderChase = false;

		yield return new WaitForSeconds(level + 30);
		foreach (var item in meshes)
		{
			item.enabled = true;
		}

		UnderChase = true;
		canPickUp = true;
		this.transform.localPosition = startPos;
		this.transform.localRotation = startRot;
		this.transform.localScale = startScale;

	}

}
