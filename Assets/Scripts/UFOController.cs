using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOController : TargetObject
{

    public string Name;
    public GameObject topIdicator;
    public MeshRenderer head;
    public Sucker sucker;
    public UnityEngine.UI.Image xpFiller;
    public UnityEngine.UI.Text playerNameText;
    public ParticleSystem circles;
    public float speed;
    public int xp;

    protected Vector3 moveDir;
    private Vector3 targetScale;
    private float targetPosition;
    private Color color;
    private int lastReach;
    private int nextReach;
    private int lvlAddXp;
    private bool canPlay;

    public static UFOController TOP_PLAYER;


    public virtual Color Color
    {
        get
        {
            return color;
        }
        set
        {
            color = value;
            head.material = new Material(head.material);
            head.material.color = color;
            xpFiller.color = color;
			playerNameText.color = color;
            Gradient grad = new Gradient();
            grad.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(color, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(0, 0.0f), new GradientAlphaKey(0, 1.0f), new GradientAlphaKey(1, .35f), new GradientAlphaKey(1, .7f) }
             );
            var module = circles.colorOverLifetime;
            module.color = grad;
        }
    }

    public bool CanPlay
    {
        get{
            return canPlay;
        }
        set{
            if(!value)
                Rigidbody.velocity = Vector3.zero;
            canPlay = value;
        }
    }

    public delegate void UFOInfo(UFOController ufo, UFOController target);
    public delegate void XPUpdate(UFOController ufo, int xp, bool lvlUp);
    public static event XPUpdate OnStatsUpdate;
    public static event UFOInfo OnKill;



    protected override void Start()
    {
        base.Start();
        base.LevelUp();
        targetScale = transform.localScale;
        targetPosition = transform.position.y;
        xpFiller.fillAmount = 0f;
        lastReach = 0;
        nextReach = 5;
        lvlAddXp = 5;

    }


    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    protected virtual void Update()
    {
        transform.localScale = Vector3.Lerp(this.transform.localScale, targetScale, Time.deltaTime * 7);
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(this.transform.position.y, targetPosition, Time.deltaTime * 7), transform.position.z);
    }

    protected override bool LevelUp()
    {
        if (!base.LevelUp()) return false;
        targetScale *= 1.2f;
        targetPosition *= 1.2f;
        this.speed *= 1.2f;
        sucker.speed *= 1.2f;

        // transform.position = new Vector3(transform.position.x, targetPosition, transform.position.z);;
        // transform.localScale = targetScale;
        return true;
    }

    public override bool IsStronger(TargetObject other)
    {
        bool lvlStrength = base.IsStronger(other);
        if (other is UFOController && this.Level == TargetObject.MAX_LEVEL)
        {
            return lvlStrength || this.xp > ((UFOController)other).xp;
        }
        return lvlStrength;
    }

    public void SetTopPlayer()
    {
        if (TOP_PLAYER != this)
        {

            if (TOP_PLAYER != null)
            {
                TOP_PLAYER.topIdicator.SetActive(false);
                // var col1 = TOP_PLAYER.head.material.color;
                // TOP_PLAYER.head.material.color = new Color(col1.r, col1.g, col1.b, 1f);
            }
            topIdicator.SetActive(true);
            // var col2 = head.material.color;
            // head.material.color = new Color(col2.r, col2.g, col2.b, 0.3f);

            TOP_PLAYER = this;
        }
    }



    void OnTriggerStay(Collider col)
    {
        var target = col.GetComponentInParent<TargetObject>();
        if (CanPlay && target && this.IsStronger(target) && col.tag != "MainCamera")
        {
            if (target is UFOController)
            {
                var vec = this.transform.position - col.transform.position;
                vec.y = 0f;
				if(vec.magnitude < targetScale.x / 2 + .5f)
				{
                    OnKill(this, (UFOController)target);
					GetPoints(target.Level+10);
				}
            }
            else if (this.transform.position.y < col.transform.position.y + 1)
            {
				GetPoints(target.Level+1);
				// Destroy(col.gameObject);
                target.Revive();
                // if (this is PlayerController)
                //     return;
                // else
                // 	ufo.gameObject.SetActive(false);
            }
        }
    }


    private void GetPoints(int addXp)
    {
        xp += addXp;
		bool up = false;
        if (xp >= nextReach)
        {
            lastReach = nextReach;
            if(Level % 2 == 0)
                lvlAddXp *= 2;
            
            nextReach += lvlAddXp;
            
            xpFiller.fillAmount = 0f;
            
			up = LevelUp();
        }
        xpFiller.fillAmount = (xp % (nextReach - lastReach)) / (float)(nextReach - lastReach);
        OnStatsUpdate(this, addXp, up);
    }

}
