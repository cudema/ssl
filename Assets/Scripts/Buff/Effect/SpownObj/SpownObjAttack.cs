using System.Collections.Generic;
using UnityEngine;

public class SpownObjAttack : EffectItemSpownObj
{
    public float damage;
    public float range;
    public int attackCount;
    public float attackTime;
    public float stiffenTime;
    public bool isFollowingPlayer;

    SphereCollider attackRange;
    HashSet<IHealthable> hitObj = new HashSet<IHealthable>();
    void Awake()
    {
        attackRange = gameObject.AddComponent<SphereCollider>();
    }

    void Start()
    {
        attackRange.isTrigger = true;
        attackRange.radius = range;
        attackRange.includeLayers = 1 << LayerMask.NameToLayer("Enemy");
        attackRange.excludeLayers = ~(1 << LayerMask.NameToLayer("Enemy"));
    }

    void OnEnable()
    {
        tempTime = 0;
        particle.Play();
        hitObj.Clear();
    }

    void OnTriggerEnter(Collider other)
    {
        IHealthable tmep = other.GetComponent<IHealthable>();
        EnemyBase enemy = other.GetComponent<EnemyBase>();
        if (tmep != null && hitObj.Add(tmep))
        {

            tmep.OnHit(Player.instance.AttackDamage * damage, Player.instance.playerStats.stats[StatType.Penetration].Value);
            //hitEffect.position = other.transform.position;
            //Debug.Log(other.transform.position);
            //effect.Play();

            enemy.OnAttackStiffen(stiffenTime);
        }
    }

    float tempTime = 0;

    void Update()
    {
        if (tempTime > attackTime)
        {
            gameObject.SetActive(false);
        }
        tempTime += Time.deltaTime;

        if (isFollowingPlayer)
        {
            transform.position = Player.instance.transform.position - new Vector3(0, 1, 0);
        }
    }
}