using UnityEngine;

public class FrostAOE : MonoBehaviour
{
    public GameObject owner;

    private void Start()
    {
        AudioManager.Instance?.IceCloudSFX();
    }
}
