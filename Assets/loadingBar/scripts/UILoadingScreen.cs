using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UILoadingScreen : UIScreen
{
    [SerializeField] private Image _fillImage;

    [SerializeField] private float _fillSpeed = 0.0f;

    private bool isFilling = false;

    public bool initializeOnStart = false;

    public event UnityAction OnFillComplete;

    void Start()
    {
        _fillImage.fillAmount = 0.0f;
        if (initializeOnStart)
        {
            StartFilling();
        }
    }

    void Update()
    {
        if (isFilling)
        {
            if (_fillImage.fillAmount < 1f)
            {
                _fillImage.fillAmount += Time.deltaTime * _fillSpeed;
            }
            else
            {
                isFilling = false;
                OnFillComplete?.Invoke();
            }
        }
    }

    public void StartFilling()
    {
        _fillImage.fillAmount = 0.0f;
        isFilling = true;
    }

    public override void Show()
    {
        base.Show();
        StartFilling();
    }

    public override void Hide()
    {
        base.Hide();
    }
}
