using System.Collections;
using UnityEngine;

public class EggCollectionManager : MonoBehaviour
{
    public static EggCollectionManager Instance;
    void Awake() => Instance = this;

    public void SpawnEggPuff(GameObject egg)
    {
        StartCoroutine(FadeAndDestroy(egg));
    }

    private IEnumerator FadeAndDestroy(GameObject egg)
    {
        SpriteRenderer sr = egg.GetComponentInChildren<SpriteRenderer>();
        Color color = sr.color;

        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (sr)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
                sr.color = new Color(color.r, color.g, color.b, alpha);
            
                yield return null; 
            }
        }

        sr.color = new Color(color.r, color.g, color.b, 0f);

        Destroy(egg);
    }
}
