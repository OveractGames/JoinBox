using System;
using System.Globalization;
using UnityEngine;

public class SpinRewardManager : Singleton<SpinRewardManager>
{
    private const string LAST_SPIN_DATE_KEY = "LastSpinDate";

    public int spinIntervalDays = 1;

    public bool CanSpin()
    {
        DateTime lastSpinDate;
        if (!DateTime.TryParse(PlayerPrefs.GetString(LAST_SPIN_DATE_KEY), out lastSpinDate))
        {
            lastSpinDate = DateTime.ParseExact("1970-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        return (DateTime.Now - lastSpinDate).TotalDays >= spinIntervalDays;
    }

    public void SaveSpinReward(int rewardCount)
    {
        Debug.Log("You received a spin reward of " + rewardCount + "!");

        PlayerPrefs.SetString(LAST_SPIN_DATE_KEY,
            DateTime.Now.Year + "-"
            + DateTime.Now.Month.ToString().PadLeft(2, '0') + "-"
            + DateTime.Now.Day.ToString().PadLeft(2, '0'));

        Debug.Log(string.Format("Player spin wheel, saving date: {0}",
            PlayerPrefs.GetString(LAST_SPIN_DATE_KEY)));

        int coins = PlayerPrefs.GetInt("COINS", 500);
        PlayerPrefs.SetInt("COINS", coins + rewardCount);
    }
}
