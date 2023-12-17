using UnityEngine;

public class PlayerPrefsManager : Singleton<PlayerPrefsManager>
{
    private const string BombKey = "BOMBS";
    private const string ReloadsKey = "RELOADS";
    private const string CoinsKey = "COINS";
    private const string LevelKey = "LEVEL";

    public int BombCount
    {
        get => PlayerPrefs.GetInt(BombKey);
        private set => PlayerPrefs.SetInt(BombKey, value);
    }

    public int ReloadsCount
    {
        get => PlayerPrefs.GetInt(ReloadsKey);
        private set => PlayerPrefs.SetInt(ReloadsKey, value);
    }

    public int CoinsCount
    {
        get => PlayerPrefs.GetInt(CoinsKey);
        private set => PlayerPrefs.SetInt(CoinsKey, value);
    }

    public int LevelIndex
    {
        get => PlayerPrefs.GetInt(LevelKey, 1);
        private set => PlayerPrefs.SetInt(LevelKey, value);
    }

    public void AddBomb(int count)
    {
        BombCount += count ;
    }

    public void AddReloads(int count)
    {
        ReloadsCount += count;
    }

    public void AddCoins(int count)
    {
        CoinsCount += count;
    }

    public void DecreaseBomb()
    {
        int currentBombCount = BombCount;
        if (currentBombCount > 0)
        {
            PlayerPrefs.SetInt(BombKey, currentBombCount - 1);
        }
    }

    public void DecreaseReloads()
    {
        int currentReloadsCount = ReloadsCount;
        if (currentReloadsCount > 0)
        {
            PlayerPrefs.SetInt(ReloadsKey, currentReloadsCount - 1);
        }
    }

    public void IncreaseLevel()
    {
        LevelIndex++;
    }

}

