using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class ManageBlessing : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] blessingLevelTexts;
    [SerializeField] private TextMeshProUGUI[] blessingPercentTexts;

    private void OnEnable()
    {
        blessingLevelTexts[0].text =
            GameManager.Instance.blessStressLevel + " / " + GameManager.Instance.BlessList[4].Max;
        blessingPercentTexts[0].text =
            "-" + (GameManager.Instance.BlessList[4].Amount * GameManager.Instance.blessStressLevel).ToString(
                CultureInfo.InvariantCulture) + "%";

        blessingLevelTexts[1].text =
            GameManager.Instance.blessChipLevel + " / " + GameManager.Instance.BlessList[1].Max;
        blessingPercentTexts[1].text =
            "+" + (GameManager.Instance.BlessList[1].Amount * GameManager.Instance.blessChipLevel).ToString(CultureInfo
                .InvariantCulture) + "%";
        
        blessingLevelTexts[2].text = GameManager.Instance.blessGoldLevel + " / " + GameManager.Instance.BlessList[2].Max;
        blessingPercentTexts[2].text =
            "+" + (GameManager.Instance.BlessList[2].Amount * GameManager.Instance.blessGoldLevel).ToString(CultureInfo
                .InvariantCulture) + "%";

        blessingLevelTexts[3].text =
            GameManager.Instance.blessDungeonLevel + " / " + GameManager.Instance.BlessList[3].Max;
        blessingPercentTexts[3].text =
            "+" + (GameManager.Instance.BlessList[3].Amount * GameManager.Instance.blessDungeonLevel).ToString(
                CultureInfo.InvariantCulture) + "%";
    }

    public void StressLevelUp()
    {
        if (GameManager.Instance.Bless <= 0 || GameManager.Instance.blessStressLevel >= GameManager.Instance.BlessList[4].Max)
            return;

        GameManager.Instance.Bless -= 1;
        GameManager.Instance.blessStressLevel += 1;

        blessingLevelTexts[0].text =
            GameManager.Instance.blessStressLevel + " / " + GameManager.Instance.BlessList[4].Max;
        blessingPercentTexts[0].text =
            "-" + (GameManager.Instance.BlessList[4].Amount * GameManager.Instance.blessStressLevel).ToString(
                CultureInfo.InvariantCulture) + "%";
    }

    public void ChipLevelUp()
    {
        if (GameManager.Instance.Bless <= 0 || GameManager.Instance.blessChipLevel >= GameManager.Instance.BlessList[1].Max)
            return;
        
        GameManager.Instance.Bless -= 1;
        GameManager.Instance.blessChipLevel += 1;

        blessingLevelTexts[1].text =
            GameManager.Instance.blessChipLevel + " / " + GameManager.Instance.BlessList[1].Max;
        blessingPercentTexts[1].text =
            "+" + (GameManager.Instance.BlessList[1].Amount * GameManager.Instance.blessChipLevel).ToString(CultureInfo
                .InvariantCulture) + "%";
    }

    public void GoldLevelUp()
    {
        if (GameManager.Instance.Bless <= 0 || GameManager.Instance.blessGoldLevel >= GameManager.Instance.BlessList[2].Max)
            return;
        
        GameManager.Instance.Bless -= 1;
        GameManager.Instance.blessGoldLevel += 1;

        blessingLevelTexts[2].text = GameManager.Instance.blessGoldLevel + " / " + GameManager.Instance.BlessList[2].Max;
        blessingPercentTexts[2].text =
            "+" + (GameManager.Instance.BlessList[2].Amount * GameManager.Instance.blessGoldLevel).ToString(CultureInfo
                .InvariantCulture) + "%";
    }

    public void DungeonLevelUp()
    {
        if (GameManager.Instance.Bless <= 0 || GameManager.Instance.blessDungeonLevel >= GameManager.Instance.BlessList[3].Max)
            return;
        
        GameManager.Instance.Bless -= 1;
        GameManager.Instance.blessDungeonLevel += 1;

        blessingLevelTexts[3].text =
            GameManager.Instance.blessDungeonLevel + " / " + GameManager.Instance.BlessList[3].Max;
        blessingPercentTexts[3].text =
            "+" + (GameManager.Instance.BlessList[3].Amount * GameManager.Instance.blessDungeonLevel).ToString(
                CultureInfo.InvariantCulture) + "%";
    }
}
