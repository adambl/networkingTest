using UnityEngine;

public class DamageRadiusController : MonoBehaviour
{
    private void DestroySelf(float delay)
    {
        Destroy(this.gameObject, delay);
    }

    public void Start()
    {
        DestroySelf(1.5f);
    }

}
