using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public enum BgmType
{
    VillageMorning_Loop = 0,
    VillageAfternoon_Loop,
    VillageEvening_Loop,
    PeacefulTownSquare_Loop,
    ForestPath_Loop,
    DeepForest_Loop,
    OpenField_Loop,
    RollingHills_Loop,
    CalmLake_Loop,
    SnowyMountain_Loop,
    DesertJourney_Loop,
    RuinsExploration_Loop,
    AncientTemple,
    CastleHall,
    ShopTheme_Loop,
    InnRest_Loop,
    CaveAmbience_Loop,
    DarkDungeon_Loop,
    BattleTheme1_Loop,
    BattleTheme2_Loop,
    BattleTheme3_Loop,
    BossBattle1_Loop,
    BossBattle2_Loop,
    FinalBattle_Loop,
    VictoryFanfare,
    DefeatStinger,
    EpicAdventure,
    HerosTheme,
    SuspensefulEncounter,
    MysteriousDiscovery,
    // Bonus
    Bonus_HerosTheme,
    Bonus_BattleTheme1_Loop,
    Bonus_DarkDungeon
}



public enum AudioType
{
    BGM,
    SFX
}

public class AudioManager : SceneOnlySingleton<AudioManager>
{
    [SerializeField] private BgmType currentBgm = BgmType.VillageMorning_Loop;
        /* 사운드 조절 기능 */
    [SerializeField][Range(0, 1)] private float soundEffectVolume = 1f;
    [SerializeField][Range(0, 1)] private float soundEffectPitchVariance = 0.1f;
    [SerializeField][Range(0, 1)] private float musicVolume = 0.5f;

    /* 모든 사운드 저장 */
    /* 저장된 사운드를 꺼내쓰기 쉽도록 Dictionary에 저장 */
    public Dictionary<string, AudioClip> AudioDictionary = new();
    protected ObjectPoolManager objectPoolManager;
    private string sfxPlayerPoolName = "sfxSource";
    [SerializeField] private AudioSource bgmAudioSource;

    protected override void Awake()
    {
        base.Awake();
        InitializeAudioManager();
    }

    protected void Start()
    {
        objectPoolManager = ObjectPoolManager.Instance;
    }

    private void InitializeAudioManager()
    {
        bgmAudioSource = GetComponent<AudioSource>();
        if (bgmAudioSource == null)
        {
            bgmAudioSource = gameObject.AddComponent<AudioSource>();
        }
        //LoadAssetManager.Instance.OnLoadAssetsChangeScene(SceneManager.GetActiveScene().name);
        LoadAssetManager.Instance.LoadAudioClipAsync(SceneManager.GetActiveScene().name + "BGM", clip =>
        {
            PlayBGM(clip);
        });
        LoadAssetManager.Instance.LoadAssetBundle(nameof(AlwaysLoad.AlwaysLoadSound)); // 항상 로드해와야 하는 사운드
        LoadAssetManager.Instance.LoadAssetBundle(SceneManager.GetActiveScene().name); // 특정 씬에서 로드해와야 하는 사운드
        
        bgmAudioSource.volume = musicVolume;
        bgmAudioSource.loop = true;
    }


    /* 볼륨 조절 기능. 나중에 옵션으로 사운드를 BGM,SFX 따로 조절할 수 있도록 만든 형태 */
    public void SetVolume(AudioType type, float volume)
    {
        volume = Mathf.Clamp01(volume);

        if (type == AudioType.BGM)
        {
            musicVolume = volume;
            if (bgmAudioSource != null)
            {
                bgmAudioSource.volume = musicVolume;
            }
        }
        else if (type == AudioType.SFX)
        {
            soundEffectVolume = volume;
        }
    }

    /* BGM은 Loop를 돌며 계속해서 반복 재생 */
    public void PlayBGM(string clipName, bool isLoop = true)
    {
        if (bgmAudioSource == null || AudioDictionary == null)
        {
            Debug.LogError("SoundManager: BGM 재생 실패 - AudioSource 또는 AudioDictionary가 null입니다.");
            return;
        }

        if (AudioDictionary.ContainsKey(clipName))
        {
            bgmAudioSource.Stop();
            bgmAudioSource.clip = AudioDictionary[clipName];
            bgmAudioSource.loop = isLoop;
            bgmAudioSource.Play();
        }
        else
        {
            Debug.LogError($"SoundManager: PlayBGM - {clipName}은 존재하지 않는 오디오 클립입니다.");
        }
    }

    /* BGM을 정지할 때 쓰는 메서드 */
    public void StopBGM()
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.Stop();
        }
    }

    /* 효과음 재생용 메서드 */
    public void PlaySFX(string clipName)
    {
        if (string.IsNullOrEmpty(clipName))
        {
            Debug.LogError("SoundManager: PlaySFX - clipName이 null 또는 빈 문자열입니다.");
            return;
        }

        if (objectPoolManager == null)
        {
            Debug.LogError("SoundManager: SoundPoolManager를 찾을 수 없습니다.");
            return;
        }

        if (AudioDictionary != null && AudioDictionary.ContainsKey(clipName))
        {
            GameObject sfxPlayer = objectPoolManager.GetObject(sfxPlayerPoolName);
            PoolableAudioSource sfxSource = sfxPlayer.GetComponent<PoolableAudioSource>();
            if (sfxPlayer != null)
            {
                sfxSource.Play(AudioDictionary[clipName], soundEffectVolume);
            }
            else
            {
                Debug.LogError("SoundManager: SoundSource 객체를 가져올 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError($"SoundManager: PlaySFX - {clipName}은 존재하지 않는 오디오 클립입니다.");
        }
    }

    /* 효과음 재생 후 해당 효과음 제어를 위해 만들어진 효과음 Prefab을 Return받는데 사용되는 메서드 */
    public PoolableAudioSource PlaySfxReturnSoundSource(string clipName)
    {
        if (string.IsNullOrEmpty(clipName))
        {
            Debug.LogError("SoundManager: PlaySfxReturnSoundSource - clipName이 null 또는 빈 문자열입니다.");
            return null;
        }

        // if (objectPoolManager == null)
        // {
        //     objectPoolManager = SoundPoolManager.Instance;
        //     if (objectPoolManager == null)
        //     {
        //         Debug.LogError("SoundManager: SoundPoolManager를 찾을 수 없습니다.");
        //         return null;
        //     }
        // }
        //
        // if (_audioDictionary != null && _audioDictionary.ContainsKey(clipName))
        // {
        //     SoundSource soundSource = objectPoolManager.GetObject(0, Vector3.zero, Quaternion.identity);
        //     if (soundSource != null)
        //     {
        //         soundSource.Play(_audioDictionary[clipName], _soundEffectVolume);
        //         return soundSource;
        //     }
        //     else
        //     {
        //         Debug.LogError("SoundManager: SoundSource 객체를 가져올 수 없습니다.");
        //         return null;
        //     }
        // }
        // else
        // {
        //     Debug.LogError($"SoundManager: PlaySfxReturnSoundSource - {clipName}은 존재하지 않는 오디오 클립입니다.");
        return null;
        // }
        }
}