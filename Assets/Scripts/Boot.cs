using UnityEngine;

public class Boot : MonoBehaviour
{
    [SerializeField] private UILoadingScreen _loadingScreen;
    [SerializeField] private UIDailySpinScreen _spinScreen;

    public bool testing;

    private void Start()
    {
        if(testing)
        {
            LoadingComplete();
            return;
        }
        if (PlayerPrefs.GetInt("BOOT") == 0)
        {
            _loadingScreen.Show();
            if (!_loadingScreen.initializeOnStart)
            {
                _loadingScreen.StartFilling();
            }
            _loadingScreen.OnFillComplete += LoadingComplete;
        }
        else
        {
            LoadingComplete();
        }
    }

    private void LoadingComplete()
    {
        UIController.Instance.ShowScreen<UIMainScreen>();
        _loadingScreen.Hide();
    }
}
