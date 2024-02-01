using UnityEngine;
using System.Collections;

public class CubeGrow : MonoBehaviour
{
    public float targetHeight = 4f;
    public float growthDuration = 0.5f;

    private Vector3 originalScale;
    private Vector3 targetScale;
    private bool shouldGrow = false;

    public void StartGrowing()
    {
        originalScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
        targetScale = new Vector3(originalScale.x, targetHeight, originalScale.z);
        StartCoroutine(Grow());
    }

    IEnumerator Grow()
    {
        float timer = 0f;
        while (timer < growthDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, timer / growthDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale; 
        shouldGrow = false; 
    }
}
