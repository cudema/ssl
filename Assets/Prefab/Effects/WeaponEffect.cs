using UnityEngine;

public class WeaponEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem weaponTrail;
    public Animator animator;

    public void PlayTrail()
    {
        if (weaponTrail == null)
        {
            Debug.LogWarning("weaponTrail이 연결되지 않음");
            return;
        }

        weaponTrail.gameObject.SetActive(true);
        weaponTrail.Clear();
        weaponTrail.Play();
    }

    public void StopTrail()
    {
        if (weaponTrail == null)
            return;

        weaponTrail.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}