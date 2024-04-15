using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // This is the AudioManager class! Call methods you write here in other scripts with:
    // AudioManager.Instance.MethodInThisClassThatYouWrote();

    // This part comes last but I put it at the top b/c it's important
    // In order to get it to work in game, you'll want to make a new prefab object and give a script
    // component with this script. Then, you'll drag and place all of your audio files that you put
    // in your Assets/Audio folder (put them there if they aren't yet) into the fields of of the script.
    // Save the prefab. To add it to the scene, place the prefab as the a child object of your game manager.

    // This is an audio source, it's what actually plays audio in your game.
    // For how this is organized, an audio source is meant to only play one file at a time. (except sfx)
    // Typically, this means you have one audio source for every concurrent longer sound (music, ambient,
    // voiced dialogue), plus an additional one for sound effects.


    [SerializeField] 
    private AudioSource musicBottomLayerSource; // maybe make these programmatically
    [SerializeField]
    private AudioSource musicTopLayerSource;
    [SerializeField]
    private AudioSource sfx;

    // https://docs.unity3d.com/ScriptReference/AudioSource.html has a list of fields & methods that
    // you can use, such as normalSource.loop (boolean), normalSource.Play() and normalSource.clip .


    // Next up, audio clips. These are literally audio files.
    // Using mp3 is not disallowed, but it will make your loops a little choppy because it's compressed.
    // using .wav or .ogg is recommended
    [SerializeField]
    private AudioClip musicBottomLayer;

    [SerializeField]
    private AudioClip musicFireLayer;
    [SerializeField]
    private AudioClip musicIceLayer;
    [SerializeField]
    private AudioClip musicLichLayer;


    private int currentMusicState = 0; // 0 default, 1 fire, 2 ice, 3 lich


    // All sfx will be played as oneshots through the sfx source


    [SerializeField]
    private List<AudioClip> summoningNoise;

    [SerializeField] private AudioClip idleGloopage;
    [SerializeField] private AudioClip footstepLoop;

    [SerializeField] private List<AudioClip> jump;
    [SerializeField] private List<AudioClip> gloopShot;
    [SerializeField] private List<AudioClip> gloopImpact;
    [SerializeField] private List<AudioClip> damage;
    // ...

    // demon sounds
    [SerializeField] private List<AudioClip> scorchRay;
    [SerializeField] private List<AudioClip> hellCharge;
    //[SerializeField] private List<AudioClip> demonVocals;


    // troll sounds
    [SerializeField] private List<AudioClip> trollJump;
    [SerializeField] private List<AudioClip> trollLand;
    [SerializeField] private List<AudioClip> iciclesHitGround;
    [SerializeField] private List<AudioClip> iceCloudCrystalNoise;

    // lich sounds
    [SerializeField] private List<AudioClip> undeadTeleport;
    [SerializeField] private List<AudioClip> skullShoot;
    [SerializeField] private List<AudioClip> skullImpact;
    //[SerializeField] private List<AudioClip> lichVocals;


    [SerializeField] private List<AudioClip> click;
    [SerializeField] private List<AudioClip> extremeMode;




    // Start is called before the first frame update.
    // For us, that means assign all of the clips and volumes from the serialized fields,
    // or setting up any state needed for whatever dynamic music system you end up making.
    // We don't need to assign any clips to sfx yet, we'll get there.
    void Start()
    {
        //musicSource.clip = song;
        //musicSource.volume = songVolume;
        //musicSource.loop = true;
        //musicSource.Play();


        //ambienceSource.clip = natureSounds;
        //ambienceSource.volume = natureSoundsVolume;
        //ambienceSource.loop = true;
        //ambienceSource.Play();

        // ...

    }



    // Now actually come the methods you are writing that will alter what audio happens.
    // Call methods you write here in other scripts with "AudioManager.Instance.MethodInThisClassThatYouWrote();"

    //public void PauseAdjust()
    //{
    //    StartCoroutine(Fade(musicSource, .1f, songVolume / 2));
    //}

    //public void UnpauseAdjust()
    //{
    //    StartCoroutine(Fade(musicSource, .1f, songVolume));
    //}

    // ...

    public void TransitionMusicDefault()
    {
        // TODO
    }

    public void TransitionMusicFire()
    {
        // TODO
    }

    public void TransitionMusicIce()
    {
        // TODO
    }
    public void TransitionMusicLich()
    {
        // TODO
    }

    public void SummoningSFX()
    {
        SFX(summoningNoise);
    }

    // Finally, sound effects.
    // Sound effects are played as one shots, meaning the clip is never actually saved to the audio source

    public void StartWalkingSFX()
    {
        // TODO
    }

    public void StopWalking()
    {
        // TODO
    }

    // Character 
    public void JumpSFX() => SFX(jump); 
    public void ShootGloopSFX() => SFX(gloopShot);
    public void GloopImpactSFX() => SFX(gloopImpact);
    public void DamageSFX() => SFX(damage);


    // demon
    public void ScorchRaySFX() => SFX(scorchRay);
    public void HellChargeSFX() => SFX(hellCharge);

    // troll
    public void TrollJumpSFX() => SFX(trollJump);
    public void TrollLandingSFX() => SFX(trollLand);
    public void IcicleImpactSFX() => SFX(iciclesHitGround);
    public void IceCloudSFX() => SFX(iceCloudCrystalNoise);

    // lich
    public void LichTeleportSFX() => SFX(undeadTeleport);
    public void SkullShootSFX() => SFX(skullShoot);
    public void SkullImpactSFX() => SFX(skullImpact);

    // menu
    public void ClickSFX() => SFX(click);
    public void ExtremeModeSFX() => SFX(extremeMode);




    // An optional second parameter specifies the volume to play the clip at, but if not included, the
    // default volume is 0.5f, where 0 is silent and 1 is full blast. 
    //public void TapSFX()
    //{
    //    sfx.PlayOneShot(tap, 0.8f);
    //}



    // ...

    // That's the end of stuff you really need to edit to get your game specific stuff running,
    // below is some general stuff to make the whole class work

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - //

    private void SFX(List<AudioClip> list, float vol = 0.5f)
    {
        if (list.Count == 0)
        {
            Debug.Log("UNIMPLEMENTED SOUND EFFECT");
            return;
        }

        sfx.PlayOneShot(list[ Random.Range(0, list.Count) ], vol);
    }


    // This is some singleton code that helps other parts of the code find the AudioManager.
    // You can write methods that other scripts can call by referencing the AudioManager singleton.
    // Call methods you write here in other scripts with:
    // AudioManager.Instance.MethodInThisClassThatYouWrote();
    // you will need to import AudioManager in that script though
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject();
                    _instance = go.AddComponent<AudioManager>();
                    Debug.Log("Generating new audio manager");
                }
            }
            return _instance;
        }
    }

    // This is the audio fading coroutine, you can write others like it if you so wish
    // Note that you can call this twice at once, so you can do crossfades as long as the duration is the same for both
    private IEnumerator Fade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

}



