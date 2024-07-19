using GameObjects;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;

public class EnemyProjectileObject : MonoBehaviour
{
    private Vector2 origin, target;
    private float airtime, gravity, currTime, originz;
    private Transform shadow, vfx;
    private Vector3 shadowScale;
    private PlayerObject player;
    [SerializeField] private float initialSpeedZ;

    private void Awake()
    {
        currTime = 0f;
        originz = BedObject.Instance.transform.position.z;
        shadow = transform.Find("Shadow");
        vfx = transform.Find("VFX");
        shadowScale = shadow.localScale;
    }
    public void Launch(Vector2 origin, Vector2 target, float airtime = 2f)
    {
        this.origin = origin;
        this.target = target;
        this.airtime = airtime;
        gravity = initialSpeedZ/airtime;
    }

    private void Update()
    {
        currTime += Time.deltaTime;
        transform.position = LinearMovement(currTime);
        vfx.localPosition = new Vector3(0, -ParabolicHeight(currTime) * Mathf.Sin(Mathf.Abs(transform.rotation.x)), ParabolicHeight(currTime) * Mathf.Cos(Mathf.Abs(transform.rotation.x))); 
        if (currTime > airtime)
        {
            bool detected = false;
            SpriteRenderer sprite = vfx.GetComponent<SpriteRenderer>();
            Collider[] colliders = Physics.OverlapSphere(transform.position, sprite.bounds.extents.x);
            foreach (var col in colliders)
            {
                if (col.TryGetComponent(out PlayerObject player))
                {
                    // player.stun
                    detected = true;
                }
            }
            if (!detected)
            {
                BedObject.Instance.GetHit(target);
            }
            Destroy(gameObject);
        }
    }

    private Vector3 LinearMovement(float currTime)
    {
        if (currTime > airtime) currTime = airtime;
        float x = origin.x + (target.x - origin.x) * (currTime / airtime);
        float y = origin.y + (target.y - origin.y) * (currTime / airtime);
        return new Vector3(x, y, 0);
    }

    private float ParabolicHeight(float currTime)
    {
        float vt = (initialSpeedZ - gravity * currTime);
        float z = - (vt * currTime);
        return z;
    }
}
