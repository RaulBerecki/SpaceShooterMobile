using UnityEngine;

public class SatelliteColisionCatcher : MonoBehaviour
{
    public SatelliteController SatelliteController;
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
            SatelliteController.HitSatellite(this.transform);
        }
    }
    private void OnBecameVisible()
    {
        SatelliteController.isVisible = true;
    }
    private void OnBecameInvisible()
    {
        if (SatelliteController.isVisible)
            Destroy(parent);
    }
}
