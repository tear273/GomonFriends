using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;

using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
    IStoreController m_StoreController; // The Unity Purchasing system.

    //Your products IDs. They should match the ids of your products in your store.
    public string[] ganetID;

    Action func = null;
    public string environment = "production";
    async void Start()
    {
        try
        {
            var options = new InitializationOptions()
                .SetEnvironmentName(environment);

            await UnityServices.InitializeAsync(options);
        }
        catch (Exception exception)
        {
            // An error occurred during initialization.
        }

        InitializePurchasing();

    }

    void InitializePurchasing()
    {

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());


        for(int i=0; i < ganetID.Length; i++)
        {
            builder.AddProduct(ganetID[i], ProductType.Consumable, new IDs() { { ganetID[i], GooglePlay.Name } });
           // builder.AddProduct(ganetID[i], ProductType.Subscription);
        }

         
        
        UnityPurchasing.Initialize(this, builder);
    }

    public void BuyGanet(string productID, Action func)
    {
        this.func = func;
        var product = m_StoreController.products.WithID(productID);
        m_StoreController.InitiatePurchase(product);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("In-App Purchasing successfully initialized");
        m_StoreController = controller;

        for(int i=0; i<ganetID.Length; i++)
        {
            var product = m_StoreController.products.WithID(ganetID[i]);
            if(product != null && product.availableToPurchase)
            {
                Debug.Log(ganetID[i] + "는 구매가능합니다.");
            }
            else
            {
                Debug.Log(ganetID[i] + "는 구매가능하지 않습니다.");
            }
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error, string? message = null)
    {
        var errorMessage = $"Purchasing failed to initialize. Reason: {error}.";

        if (message != null)
        {
            errorMessage += $" More details: {message}";
        }

        Debug.Log(errorMessage);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        //Retrieve the purchased product
        var product = args.purchasedProduct;

        this.func();
        //Add the purchased product to the players inventory
        this.func = null;      

        Debug.Log($"Purchase Complete - Product: {product.definition.id}");

        //We return Complete, informing IAP that the processing on our side is done and the transaction can be closed.
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError(error.ToString());
        throw new System.NotImplementedException();

        
    }
}
