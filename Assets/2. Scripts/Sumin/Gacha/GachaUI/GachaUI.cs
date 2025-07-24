using UnityEngine;
using UnityEngine.UI;

public class GachaUI : UIBase
{
    [Header("뽑기 버튼들")]
    [SerializeField] private GachaDrawButton drawButtonOne;
    [SerializeField] private GachaDrawButton drawButtonTen;

    [Header("캐릭터 가챠")]
    [SerializeField] private Button characterGachaButton;
    [SerializeField] private CharacterGachaSystem characterGachaSystem;
    [SerializeField] private CharacterGachaResultUI characterGachaResultUI;
    [SerializeField] private CharacterGachaBannerUI characterGachaBannerUI;
    private CharacterGachaHandler characterHandler;

    [Header("스킬 가챠")]
    [SerializeField] private Button skillGachaButton;
    [SerializeField] private SkillGachaSystem skillGachaSystem;
    [SerializeField] private SkillGachaResultUI skillGachaResultUI;
    [SerializeField] private SkillGachaBannerUI skillGachaBannerUI;
    private SkillGachaHandler skillHandler;

    [Header("장비 가챠")]
    [SerializeField] private Button equipmentGachaButton;
    [SerializeField] private EquipmentGachaSystem equipmentGachaSystem;
    [SerializeField] private EquipmentGachaResultUI equipmentGachaResultUI;
    [SerializeField] private EquipmentGachaBannerUI equipmentGachaBannerUI;
    private EquipmentGachaHandler equipmentHandler;


    private void Start()
    {
        characterHandler = new CharacterGachaHandler(characterGachaSystem, characterGachaResultUI);
        skillHandler = new SkillGachaHandler(skillGachaSystem, skillGachaResultUI);
        equipmentHandler = new EquipmentGachaHandler(equipmentGachaSystem, equipmentGachaResultUI);

        characterGachaButton.onClick.RemoveAllListeners();
        characterGachaButton.onClick.AddListener(() => OnCharacterGachaSelected());

        skillGachaButton.onClick.RemoveAllListeners();
        skillGachaButton.onClick.AddListener(() => OnSkillGachaSelected());

        equipmentGachaButton.onClick.RemoveAllListeners();
        equipmentGachaButton.onClick.AddListener(() => OnEquipmentGachaSelected());
    }

    public override void Open()
    {
        base.Open();
        // 처음 열었을땐 영웅 소환
        OnCharacterGachaSelected();
        ToggleActiveButtons(characterGachaButton);
    }

    // 버튼 토글
    private void ToggleActiveButtons(Button button)
    {
        characterGachaButton.interactable = true;
        skillGachaButton.interactable = true;
        equipmentGachaButton.interactable = true;
        button.interactable = false;
    }

    public void OnCharacterGachaSelected()
    {
        SetGachaSelection(characterHandler, characterGachaBannerUI, characterGachaButton);
    }

    public void OnSkillGachaSelected()
    {
        SetGachaSelection(skillHandler, skillGachaBannerUI, skillGachaButton);
    }

    public void OnEquipmentGachaSelected()
    {
        SetGachaSelection(equipmentHandler, equipmentGachaBannerUI, equipmentGachaButton);
    }

    private void SetGachaSelection(IGachaHandler handler, IGachaBannerUI bannerUI, Button activeButton)
    {
        drawButtonOne.Initialize(handler, 1);
        drawButtonTen.Initialize(handler, 10);
        InitializeGachaTypeUI();
        bannerUI.ShowBanner();
        ToggleActiveButtons(activeButton);
    }

    // 가챠 종류별 버튼 누를때마다 초기화
    private void InitializeGachaTypeUI()
    {
        ResetAllBanners();
        RefreshDrawButtons();
    }

    // 배너들 초기화
    private void ResetAllBanners()
    {
        characterGachaBannerUI.HideBanner();
        skillGachaBannerUI.HideBanner();
        equipmentGachaBannerUI.HideBanner();
    }

    // 버튼들 초기화
    private void RefreshDrawButtons()
    {
        drawButtonOne.HideButton();
        drawButtonTen.HideButton();
        drawButtonOne.ShowButton();
        drawButtonTen.ShowButton();
    }
}