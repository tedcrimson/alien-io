using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sucker : MonoBehaviour
{

    public float speed;
	private UFOController ufo;


	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Awake()
	{
		ufo = this.GetComponentInParent<UFOController>();
	}


    void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<TargetObject>();
        if (!ufo.CanPlay || target == null  || !ufo.IsStronger(target)) return;
        target.OnEnter(this.transform.parent, speed);

    }

    void OnTriggerStay(Collider other)
    {
        var target = other.GetComponent<TargetObject>();
        if (!ufo.CanPlay || target == null  || !ufo.IsStronger(target)) return;
        target.OnStay(this.transform.parent, speed * 1.7f);

    }

    /// <summary>
    /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerExit(Collider other)
    {
        var target = other.GetComponent<TargetObject>();
        if (!ufo.CanPlay || target == null  || !ufo.IsStronger(target)) return;
        target.OnExit(this.transform.parent, speed);

    }
}
