#if HOME
using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : Singleton<IAPManager>, IStoreListener
{
    private static IStoreController storeController;
    private static IExtensionProvider extensionProvider;

    private readonly string[] _packs = new string[]
    {
        "com.overactgames.joinbox.pack_10_bomb",
        "com.overactgames.joinbox.pack_10_reloads",
        "com.overactgames.joinbox.pack_15_bomb_and_reloads"
    };

    public event Action OnPurchaseComplete;

    private void Start()
    {
        InitializePurchasing();
    }

    private void InitializePurchasing()
    {
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        foreach (string pack in _packs)
        {
            builder.AddProduct(pack, ProductType.Consumable);
        }
        UnityPurchasing.Initialize(this, builder);
    }

    public void BuyProduct(int index)
    {
        if (storeController != null)
        {
            Product product = storeController.products.WithID(_packs[index]);
            if (product != null && product.availableToPurchase)
            {
                storeController.InitiatePurchase(product);
            }
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        extensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("Failed to initialize IAP: " + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log("Failed to purchase " + product.definition.id + ": " + failureReason);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log("Purchased: " + args.purchasedProduct.definition.id);
        OnPurchaseComplete?.Invoke();
        return PurchaseProcessingResult.Complete;
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("Failed to initialize IAPManager: " + error + " : " + message);
    }
}
#endif