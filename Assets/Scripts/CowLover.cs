using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowLover : MonoBehaviour {
	public GameObject cowPrefab;
	public Bounds bounds;
    public int cowCount;

    // Use this for initialization
    void Awake () {
		var min= bounds.min;
		var max= bounds.max;
		for(int i = 0; i <cowCount; i ++)
		{
			var cow = Instantiate(cowPrefab, bounds.center + new Vector3(Random.Range(-min.x, -max.x), Random.Range(-min.y, -max.y), Random.Range(-min.z, -max.z)), Quaternion.Euler(Vector3.up * Random.Range(0, 360)));
			// var r = Random.Range(0, 4);
			// cow.GetComponent<TargetObject>().Level = r;
			// cow.transform.localScale *=  (r+1);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
