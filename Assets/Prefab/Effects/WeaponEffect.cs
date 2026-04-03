using UnityEngine;

public class WeaponEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem weaponTrail;
    public Animator animator;

    public void PlayTrail()
    {
        if (weaponTrail == null)
        {
            Debug.LogWarning("weaponTrail�� ������� ����");
            return;
        }

        weaponTrail.gameObject.SetActive(true);
        
        weaponTrail.Play();
    }

    public void StopTrail()
    {
        if (weaponTrail == null)
            return;

        weaponTrail.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}