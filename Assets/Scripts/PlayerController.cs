using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : UFOController
{

    public Transform directionIndicator;
    public Transform xpContainer;
    private Vector3 temp;

    public override Color Color
    {
        set
        {
            base.Color = value;
            directionIndicator.GetComponentInChildren<UnityEngine.UI.Image>().color = value;
        }
    }


    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    protected override void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                temp = Input.mousePosition;
                directionIndicator.gameObject.SetActive(true);
                return;
            }
            var newPos = Input.mousePosition;

            var diff = newPos - temp;
            if (diff.magnitude > 50f)
            {

                moveDir = diff.normalized;
                moveDir.z = moveDir.y;
                moveDir.y = 0f;
                // Debug.Log(moveDir);
                // Debug.DrawLine(this.transform.position, transform.TransformDirection(moveDir));
                directionIndicator.right = transform.TransformDirection(moveDir);
                var movePos = moveDir * Time.deltaTime * speed;
                if (CanPlay)
                    Rigidbody.velocity = moveDir * speed;
                    // this.transform.Translate(moveDir * Time.deltaTime * speed, Space.Self);


            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Rigidbody.velocity = Vector3.zero;
            directionIndicator.gameObject.SetActive(false);
        }
        base.Update();
    }


#if UNITY_EDITOR
    /// <summary>
    /// OnGUI is called for rendering and handling GUI events.
    /// This function can be called multiple times per frame (one call per event).
    /// </summary>
    void OnGUI()
    {
        if(GUI.Button(new Rect(0, 0, 200, 200), "Level Up")){
            LevelUp();
        }
    }

#endif


}
