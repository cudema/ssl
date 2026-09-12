using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SemiCut : MonoBehaviour
{
    [SerializeField]
    Image[] images;

    public bool endCut {get; private set;} = false;

    int index = 0;
    bool cutSkip = false;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            cutSkip = true;
        }
    }

    public IEnumerator PlayCut(float fadeInSpeed, float fadeOutSpeed)
    {
        while (index < images.Length)
        {
            cutSkip = false;
            images[index].gameObject.SetActive(true);

            yield return StartCoroutine(PadeIn(fadeInSpeed));

            yield return new WaitUntil(() => Input.anyKeyDown);

            index++;
        }

        cutSkip = false;

        yield return StartCoroutine(FadeOut(fadeOutSpeed));
    }

    IEnumerator PadeIn(float fadeInSpeed)
    {
        while (images[index].color.a < 1)
        {
            images[index].color += new Color(0, 0, 0, 1 / fadeInSpeed * Time.deltaTime);

            if (cutSkip)
            {
                images[index].color += new Color(0, 0, 0, 1);
                cutSkip = false;
            }

            yield return null;
        }
    }

    IEnumerator FadeOut(float fadeSOutpeed)
    {
        while (images[0].color.a > 0)
        {
            foreach (Image temp in images)
            {
                temp.color -= new Color(0, 0, 0, 1 / fadeSOutpeed * Time.deltaTime);
            }

            if (cutSkip)
            {
                foreach (Image temp in images)
                {
                    temp.color -= new Color(0, 0, 0, 1);
                }
                cutSkip = false;
            }

            yield return null;
        }
    }
}
