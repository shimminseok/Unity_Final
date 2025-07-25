using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private BgmType currentBgm = BgmType.VillageMorning_Loop;

    [SerializeField] private List<AudioClip> bgmList = new();
    [SerializeField] private AudioSource bgmAudioSource;

    [SerializeField] private List<AudioClip> sfxList = new();
    [SerializeField] private AudioSource sfxAudioSource;

    private void Start()
    {
        bgmAudioSource.volume = 1f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            ChangeBGM(currentBgm);
        }
    }

    public void ChangeBGM(BgmType bgmType)
    {
        if (bgmAudioSource.clip == bgmList[(int)bgmType])
        {
            return;
        }

        bgmAudioSource.DOFade(0f, 1f).OnComplete(() =>
        {
            bgmAudioSource.clip = bgmList[(int)bgmType];
            bgmAudioSource.Play();
            bgmAudioSource.DOFade(1f, 1f);
        });
    }
}