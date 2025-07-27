using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorCollisionCatcher : MonoBehaviour
{
    public MeteorController MeteorController;
    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            Destroy(collision.gameObject);
            MeteorController.HitMeteor(this.transform);
        }
    }
    private void OnBecameVisible()
    {
        MeteorController.isVisible = true;
    }
    private void OnBecameInvisible()
    {
        if (MeteorController.isVisible)
            Destroy(parent);
    }
}
