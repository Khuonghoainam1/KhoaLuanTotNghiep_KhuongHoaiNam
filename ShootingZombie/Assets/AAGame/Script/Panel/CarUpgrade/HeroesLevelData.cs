using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroesLevelData", menuName = "HeroesData/HeroesData", order = 1)]
public class HeroesLevelData : ScriptableObject
{
    public List<UserBot> listPlayers = new List<UserBot>();
    public List<UserBot> listBot1 = new List<UserBot>();
    public List<UserBot> listBot2 = new List<UserBot>();
    public List<UserBot> listBot3 = new List<UserBot>();

    private void OnValidate()
    {
        foreach (UserBot userBot in listPlayers)
        {
            userBot.levelMax1 = 30;
            userBot.levelMax2 = 40;
            userBot.levelMax3 = 50;
            userBot.itemCanDeDotPha = 10;
            userBot.goldNeedToUp = 1000;
            userBot.damageDotPha1 = 20;
            userBot.critDotPha1 = 5;
            userBot.damageDotPha2 = 30;
            userBot.critDotPha2 = 10;
            userBot.goldDotPha = 10000;
            userBot.adsDotPha = 5;
            userBot.damageMax = (userBot.levelMax3 - 1) * 3 + userBot.damage + userBot.damageDotPha1 + userBot.damageDotPha2;
            userBot.critMax = (userBot.levelMax3 - 1) * 0.5f + userBot.crit + userBot.critDotPha1 + userBot.critDotPha2;



            listPlayers[1].price = 5000;
            listPlayers[2].priceType = PriceType.Ads;
            listPlayers[2].price = 1;
            listPlayers[3].price = 5000;
            listPlayers[4].price = 50000;
            listPlayers[5].priceType = PriceType.Ads;
            listPlayers[5].price = 5;
        }

        foreach (UserBot userBot in listBot1)
        {
            userBot.levelMax1 = 30;
            userBot.levelMax2 = 40;
            userBot.levelMax3 = 50;
            userBot.itemCanDeDotPha = 10;
            userBot.goldNeedToUp = 300;
            userBot.damageDotPha1 = 20;
            userBot.critDotPha1 = 5;
            userBot.damageDotPha2 = 30;
            userBot.critDotPha2 = 10;
            userBot.goldDotPha = 10000;
            userBot.adsDotPha = 5;
            userBot.damageMax = (userBot.levelMax3 - 1) * 3 + userBot.damage + userBot.damageDotPha1 + userBot.damageDotPha2;
            userBot.critMax = (userBot.levelMax3 - 1) * 0.5f + userBot.crit + userBot.critDotPha1 + userBot.critDotPha2;

            listBot1[1].price = 5000;
            listBot1[2].priceType = PriceType.Ads;
            listBot1[2].price = 1;
            listBot1[3].price = 5000;
            listBot1[4].price = 50000;
            listBot1[5].priceType = PriceType.Ads;
            listBot1[5].price = 5;
        }

        foreach (UserBot userBot in listBot2)
        {
            userBot.levelMax1 = 30;
            userBot.levelMax2 = 40;
            userBot.levelMax3 = 50;
            userBot.itemCanDeDotPha = 10;
            userBot.goldNeedToUp = 300;
            userBot.damageDotPha1 = 20;
            userBot.critDotPha1 = 5;
            userBot.damageDotPha2 = 30;
            userBot.critDotPha2 = 10;
            userBot.goldDotPha = 10000;
            userBot.adsDotPha = 5;
            userBot.damageMax = (userBot.levelMax3 - 1) * 3 + userBot.damage + userBot.damageDotPha1 + userBot.damageDotPha2;
            userBot.critMax = (userBot.levelMax3 - 1) * 0.5f + userBot.crit + userBot.critDotPha1 + userBot.critDotPha2;

            listBot2[1].price = 5000;
            listBot2[2].priceType = PriceType.Ads;
            listBot2[2].price = 1;
            listBot2[3].price = 5000;
            listBot2[4].price = 50000;
            listBot2[5].priceType = PriceType.Ads;
            listBot2[5].price = 5;
        }

        foreach (UserBot userBot in listBot3)
        {
            userBot.levelMax1 = 30;
            userBot.levelMax2 = 40;
            userBot.levelMax3 = 50;
            userBot.itemCanDeDotPha = 10;
            userBot.goldNeedToUp = 300;
            userBot.damageDotPha1 = 20;
            userBot.critDotPha1 = 5;
            userBot.damageDotPha2 = 30;
            userBot.critDotPha2 = 10;
            userBot.goldDotPha = 10000;
            userBot.adsDotPha = 5;
            userBot.damageMax = (userBot.levelMax3 - 1) * 3 + userBot.damage + userBot.damageDotPha1 + userBot.damageDotPha2;
            userBot.critMax = (userBot.levelMax3 - 1) * 0.5f + userBot.crit + userBot.critDotPha1 + userBot.critDotPha2;

            listBot3[1].price = 5000;
            listBot3[2].priceType = PriceType.Ads;
            listBot3[2].price = 1;
            listBot3[3].price = 5000;
            listBot3[4].price = 50000;
            listBot3[5].priceType = PriceType.Ads;
            listBot3[5].price = 5;


        }
    }
}

public enum SkinChar
{
    main,
    luffy,
    tanjiro,
    zenitsu,
    sasuke,
    naruto,

    //bot pistal
    char_blue,
    char_captain,
    char_cyclops,
    char_huggy,
    char_police,
    char_superman,

    //bot ak
    char_deadpool,
    char_fireman,
    char_flash,
    char_ironman,
    char_red,
    char_spiderman,

    //bot bazoka
    char_minion,
    char_pikachu,
    char_pooh,
    char_shipper,
    char_wolverine,
    char_yellow,
}
