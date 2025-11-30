using System.Collections;
using UnityEngine;

public interface IHealthable
{
    public void OnHit(float damage, float penetration);
}

public class TempHit : MonoBehaviour, IHealthable
{
    Renderer ren;
    Color orignal;

    // Start is called before the first frame update
    void Start()
    {
        ren = GetComponent<Renderer>();
        orignal = ren.material.color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnHit(float damage, float penetration)
    {
        StartCoroutine("hit");
    }

    IEnumerator hit()
    {
        ren.material.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        ren.material.color = orignal;
    }
}
