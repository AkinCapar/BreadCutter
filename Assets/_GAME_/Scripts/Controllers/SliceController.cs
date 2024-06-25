using BreadCutter.Controllers;
using BreadCutter.Data;
using BreadCutter.Utils;
using BreadCutter.Views;
using EzySlice;
using UnityEngine;

public class SliceController : BaseController
{
    #region Injection

    private CurrencyController _currencyController;

    public SliceController(CurrencyController currencyController)
    {
        _currencyController = currencyController;
    }

    #endregion
    
    public override void Initialize()
    {
        _signalBus.Subscribe<SliceSignal>(OnSliceSignal);
    }

    private void OnSliceSignal(SliceSignal signal)
    {
        Slice(signal.SlicePosition, signal.SliceDirection, signal.SliceObject.breadMeshGO, signal.SliceObject.sliceMaterial, signal.SliceObject);
        _currencyController.AddCurrency(new CurrencyData(CurrencyType.Coin, signal.SliceObject.pricePerSlice));
    }

    private void Slice(Vector3 slicePos, Vector3 sliceDir, GameObject obj, Material mat, BreadView bread)
    {
        SlicedHull slicedObject = obj.Slice(slicePos, sliceDir, mat);
        GameObject breadLeft = slicedObject.CreateLowerHull(obj, mat);
        GameObject slice = slicedObject.CreateUpperHull(obj, mat);
        
        breadLeft.transform.position = obj.transform.position;
        breadLeft.transform.parent = bread.transform;
        slice.transform.position = obj.transform.position;
        slice.transform.tag = Constants.Tags.Slice;
        bread.OnSliced(breadLeft);
        //slice.AddComponent<BoxCollider>().size = bread.sliceSize;
        slice.AddComponent<CapsuleCollider>().height = 1f;
        CapsuleCollider coll = slice.GetComponent<CapsuleCollider>();
        coll.radius = .2f;
        coll.direction = 2;
        slice.AddComponent<Rigidbody>();
    }

    public override void Dispose()
    {
        _signalBus.Unsubscribe<SliceSignal>(OnSliceSignal);
    }
}
