using System.Collections;
using UnityEngine;

public class ExplosionEffectController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(WaitToDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(1.1f);
        Destroy(this.gameObject);
    }
}
