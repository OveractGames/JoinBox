using UnityEngine;

public class Boot : MonoBehaviour
{
    [SerializeField] private UILoadingScreen _loadingScreen;
    [SerializeField] private UIDailySpinScreen _spinScreen;

    private void Start()
    {
        _loadingScreen.Show();
        if (!_loadingScreen.initializeOnStart)
        {
            _loadingScreen.StartFilling();
        }
        _loadingScreen.OnFillComplete += LoadingComplete;
    }

    private void DailySpinComplete()
    {
        _spinScreen.Hide();
        _loadingScreen.Hide();
        UIController.Instance.ShowScreen<UIMainScreen>();
    }

    private void LoadingComplete()
    {
        if (SpinRewardManager.Instance.CanSpin())
        {
            _spinScreen.Show();
            _spinScreen.OnSpinComplete += DailySpinComplete;
        }
        else
        {
            UIController.Instance.ShowScreen<UIMainScreen>();
            _loadingScreen.Hide();
            Debug.Log("You cannot receive a spin reward today. Please come back tomorrow.");
        }
    }
}
