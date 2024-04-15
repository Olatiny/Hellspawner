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


    private AudioSource musicBaseLayerSource; // maybe make these programmatically
    private AudioSource musicDemonLayerSource;
    private AudioSource musicTrollLayerSource;
    private AudioSource musicLichLayerSource;
    private AudioSource currentLayerSource;
    private AudioSource sfx;
    private AudioSource idleGloop;

    // https://docs.unity3d.com/ScriptReference/AudioSource.html has a list of fields & methods that
    // you can use, such as normalSource.loop (boolean), normalSource.Play() and normalSource.clip .


    // Next up, audio clips. These are literally audio files.
    // Using mp3 is not disallowed, but it will make your loops a little choppy because it's compressed.
    // using .wav or .ogg is recommended
    [Header("Music")]
    [SerializeField] private AudioClip musicBaseLayer;
    [SerializeField] private AudioClip musicDemonLayer;
    [SerializeField] private AudioClip musicTrollLayer;
    [SerializeField] private AudioClip musicLichLayer;
    [SerializeField] private float musicVolume = 0.5f;



    // All sfx will be played as oneshots through the sfx source

    [Header("Character")]
    [SerializeField] private List<AudioClip> summoningNoise;

    [SerializeField] private AudioClip idleGloopage;
    [SerializeField] private AudioClip footstepLoop;

    [SerializeField] private List<AudioClip> jump;
    [SerializeField] private List<AudioClip> gloopShot;
    [SerializeField] private List<AudioClip> gloopImpact;
    [SerializeField] private List<AudioClip> damage;

    [Header("Demon")]// demon sounds
    [SerializeField] private List<AudioClip> scorchRay;
    [SerializeField] private List<AudioClip> hellCharge;
    //[SerializeField] private List<AudioClip> demonVocals;


    [Header("Troll")]// troll sounds
    [SerializeField] private List<AudioClip> trollJump;
    [SerializeField] private List<AudioClip> trollLand;
    [SerializeField] private List<AudioClip> iciclesHitGround;
    [SerializeField] private List<AudioClip> iceCloudCrystalNoise;

    [Header("Lich")]// lich sounds
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
        // sfx
        sfx = gameObject.AddComponent<AudioSource>();

        // base layer
        musicBaseLayerSource = gameObject.AddComponent<AudioSource>();
        musicBaseLayerSource.clip = musicBaseLayer;
        musicBaseLayerSource.loop = true;
        musicBaseLayerSource.volume = musicVolume;
        // demon layer
        musicDemonLayerSource = gameObject.AddComponent<AudioSource>();
        musicDemonLayerSource.clip = musicDemonLayer;
        musicDemonLayerSource.loop = true;
        musicDemonLayerSource.volume = 0.0f;
        // troll layer
        musicTrollLayerSource = gameObject.AddComponent<AudioSource>();
        musicTrollLayerSource.clip = musicTrollLayer;
        musicTrollLayerSource.loop = true;
        musicTrollLayerSource.volume = 0.0f;
        // lich layer
        musicLichLayerSource = gameObject.AddComponent<AudioSource>();
        musicLichLayerSource.clip = musicLichLayer;
        musicLichLayerSource.loop = true;
        musicLichLayerSource.volume = 0.0f;


        // play all the clips at the same time
        musicBaseLayerSource.Play();
        musicDemonLayerSource.Play();
        musicTrollLayerSource.Play();
        musicLichLayerSource.Play();


        // Handle the idle gloop
        idleGloop = gameObject.AddComponent<AudioSource>();
        idleGloop.clip = idleGloopage;
        idleGloop.volume = 0.6f;
        idleGloop.loop = true;
    }



    // Now actually come the methods you are writing that will alter what audio happens.
    // Call methods you write here in other scripts with "AudioManager.Instance.MethodInThisClassThatYouWrote();"

    public void PauseAdjust()
    {

        StartCoroutine(Fade(musicBaseLayerSource, .1f, musicVolume * 0.4f));
        if (currentLayerSource != null)
            StartCoroutine(Fade(currentLayerSource, .1f, musicVolume *0.4f));
    }

    public void UnpauseAdjust()
    {
        StartCoroutine(Fade(musicBaseLayerSource, .1f, musicVolume));
        if (currentLayerSource != null)
            StartCoroutine(Fade(currentLayerSource, .1f, musicVolume));
    }

    // ...

    public void TransitionMusicDefault()
    {
        // if an audio source is already silenced, then fading it should do nothing
        StartCoroutine(Fade(musicDemonLayerSource, .1f, 0));
        StartCoroutine(Fade(musicTrollLayerSource, .1f, 0));
        StartCoroutine(Fade(musicLichLayerSource, .1f, 0));
        currentLayerSource = null;
    }

    public void TransitionMusicFire()
    {
        StartCoroutine(Fade(musicDemonLayerSource, .1f, musicVolume));
        StartCoroutine(Fade(musicTrollLayerSource, .1f, 0));
        StartCoroutine(Fade(musicLichLayerSource, .1f, 0));
        currentLayerSource = musicDemonLayerSource;
    }

    public void TransitionMusicIce()
    {
        StartCoroutine(Fade(musicDemonLayerSource, .1f, 0));
        StartCoroutine(Fade(musicTrollLayerSource, .1f, musicVolume));
        StartCoroutine(Fade(musicLichLayerSource, .1f, 0));
        currentLayerSource = musicTrollLayerSource;
    }
    public void TransitionMusicLich()
    {
        StartCoroutine(Fade(musicDemonLayerSource, .1f, 0));
        StartCoroutine(Fade(musicTrollLayerSource, .1f, 0));
        StartCoroutine(Fade(musicLichLayerSource, .1f, musicVolume));
        currentLayerSource = musicLichLayerSource;
    }

    public void BeginIdleGloop()
    {
        idleGloop.Play();
    }

    public void StopIdleGloop()
    {
        idleGloop.Stop();
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
    public void ShootGloopSFX() => SFX(gloopShot, 0.35f);
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



