using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CoinType
{
    Coin_S = 0,
    Coin_L
}

public class CoinParticleSystem : MonoBehaviour
{
    ParticleSystem ps;
    [SerializeField]
    float magnetAcceleration = 15f;
    [SerializeField]
    float delayTime = 2.0f;

    [SerializeField]
    Color Coin_SColor;
    [SerializeField]
    Material sMaterial;
    [SerializeField]
    Color Coin_LColor;
    [SerializeField]
    Material lMaterial;

    ParticleSystem.Particle[] particles;
    float speed = 0;

    void Start()
    {
        if (ps == null) ps = GetComponent<ParticleSystem>();
        if (particles == null || particles.Length < ps.main.maxParticles)
        {
            particles = new ParticleSystem.Particle[ps.main.maxParticles];
        }
        ps.trigger.AddCollider(Player.instance.GetComponent<Collider>());
    }

    public void OnCoinParticlePlay(CoinType type, int spawnCoinCount)
    {
        ParticleSystemRenderer tempMain = ps.GetComponent<ParticleSystemRenderer>();
        ParticleSystem.MainModule pm = ps.main;

        switch (type)
        {
            case CoinType.Coin_S:
                tempMain.material = sMaterial;
                pm.startColor = Coin_SColor;
                break;
            case CoinType.Coin_L:
                tempMain.material = lMaterial;
                pm.startColor = Coin_LColor;
                break;
            default:
                //tempMain.startColor = Color.white;
                break;
        }

        if (particles == null || particles.Length < spawnCoinCount)
        {
            particles = new ParticleSystem.Particle[spawnCoinCount];
        }

        speed = 0;
        ps.Emit(spawnCoinCount);
        //ps.Play();
    }

    void LateUpdate()
    {
        if (!ps.isPlaying) return;

        int numParticlesAlive = ps.GetParticles(particles);

        // 여기에 추적 로직 (지난 답변의 for문 내용)
        for (int i = 0; i < numParticlesAlive; i++)
        {
            float elapsed = particles[i].startLifetime - particles[i].remainingLifetime;
            if (elapsed >= delayTime)
            {
                // 중력의 영향을 받지 않도록 속도를 제어하거나 위치를 직접 이동
                Vector3 targetDir = (Player.instance.transform.position - particles[i].position).normalized;
                
                // 자연스러운 가속을 위해 MoveTowards 대신 거리에 따른 속도 증가를 사용할 수도 있습니다.
                particles[i].position = Vector3.MoveTowards(
                    particles[i].position, 
                    Player.instance.transform.position, 
                    speed * Time.deltaTime
                );
                
                // Trail이 있다면 방향을 정렬해주기 위해 속도값도 업데이트
                particles[i].velocity = targetDir * speed;
            }
        }

        ps.SetParticles(particles, numParticlesAlive);

        speed += magnetAcceleration * Time.deltaTime;
    }

    void OnParticleTrigger()
    {
        List<ParticleSystem.Particle> enteredParticles = new List<ParticleSystem.Particle>();
        int numInside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, enteredParticles);
        //Debug.Log(numInside);
        for (int i = 0; i < numInside; i++)
        {
            ParticleSystem.Particle p = enteredParticles[i];

            p.remainingLifetime = 0;
            enteredParticles[i] = p;
            if (p.startColor == Coin_SColor) EconomyManager.Instance.AddGold(1);
            else if (p.startColor == Coin_LColor) EconomyManager.Instance.AddGold(10);
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, enteredParticles);
    }
}
