using UnityEngine;

public class FrostAOE : MonoBehaviour
{
    public GameObject owner;

    private void Start()
    {
        AudioManager.Instance?.IceCloudSFX();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("FrostAOE") && other != owner)
            print("oy");
    }
}
