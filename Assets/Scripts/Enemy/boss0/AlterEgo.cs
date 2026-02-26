using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterEgo : MonoBehaviour
{
    [SerializeField]
    GameObject render;
    [SerializeField]
    Animator animator;

    float speed = 5f;
    Coroutine going;

    public void Setup(float speed)
    {
        this.speed = speed;
    }

    public void OnRender()
    {
        render.SetActive(true);
    }

    public void OffRender()
    {
        render.SetActive(false);
    }

    public void OnGo()
    {
        if (going != null) StopCoroutine(going);
        going = StartCoroutine(Go());
    }

    public void Stop()
    {
        StopCoroutine(going);
        animator.SetTrigger("End");
    }

    IEnumerator Go()
    {
        animator.SetTrigger("Ready");

        yield return new WaitForSeconds(1f);

        while (true)
        {
            transform.position += transform.forward * speed * Time.deltaTime;

            yield return null;
        }
    }
}
