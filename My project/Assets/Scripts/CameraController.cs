using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Vector3 originalPosition;

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPas = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {

            float offsetX = Random.Range(-1, 1) *magnitude; 
            float offsetY = Random.Range(-1,1) *magnitude;

            transform.localPosition = new Vector3(offsetX, offsetY, -10);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
