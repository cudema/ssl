using UnityEngine;

public class WeaponEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem weaponTrail;
    [SerializeField] private ParticleSystem weaponTrail_2;
    public Animator animator;

    public void PlayTrail()
    {
        if (weaponTrail == null)
        {
            Debug.LogWarning("weaponTrail�� ������� ����");
            return;
        }

        weaponTrail.gameObject.SetActive(true);
        //weaponTrail.Clear();
        weaponTrail.Play();
        weaponTrail_2.gameObject.SetActive(true);
        //weaponTrail_2.Clear();
        weaponTrail_2.Play();
    }

    public void StopTrail()
    {
        if (weaponTrail == null)
            return;

        // weaponTrail.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        // weaponTrail_2.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}