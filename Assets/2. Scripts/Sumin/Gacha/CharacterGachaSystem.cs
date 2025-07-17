using System.Collections.Generic;
using UnityEngine;

public class CharacterGachaSystem : MonoBehaviour
{
    [SerializeField] PlayerUnitTable playerUnitTable;

    private int drawCost = 0;
    public int DrawCost => drawCost;

    private GachaManager<PlayerUnitSO> gachaManager;

    private void Awake()
    {
        gachaManager = new GachaManager<PlayerUnitSO>(new RandomCharacterGachaStrategy());
    }

    private List<PlayerUnitSO> GetCharacterDatas()
    {
        List<PlayerUnitSO> characters = new();

        for (int i =0; i < (int)JobType.Monster; i++)
        {
            characters.AddRange(playerUnitTable.GetPlayerUnitsByJob((JobType)i));
        }

        return characters;
    }

    public PlayerUnitSO[] DrawCharacters(int count)
    {
        List<PlayerUnitSO> characterData = GetCharacterDatas();
        PlayerUnitSO[] results = new PlayerUnitSO[count];

        AccountManager.Instance.UseOpal(drawCost * count);

        for (int i=0; i<count; i++)
        {
            PlayerUnitSO character = gachaManager.Draw(characterData, Define.TierRates);

            if(character != null)
            {
                results[i] = character;

                AccountManager.Instance.AddPlayerUnit(character);
            }
            else
            {
                Debug.LogWarning($"{i}번째 뽑기에 실패했습니다.");
            }
        }

        return results;
    }
    public bool CheckCanDraw(int drawCount)
    {
        drawCost = Define.GachaDrawCosts[GachaType.Character];
        bool canUse = AccountManager.Instance.CanUseOpal(drawCost * drawCount);

        return canUse;
    }
}
