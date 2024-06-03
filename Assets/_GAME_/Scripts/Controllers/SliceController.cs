using BreadCutter.Views;
using EzySlice;
using UnityEngine;

public class SliceController : BaseController
{
    #region Injection

    public SliceController()
    {
    }

    #endregion
    
    public override void Initialize()
    {
        _signalBus.Subscribe<SliceSignal>(OnSliceSignal);
    }

    private void OnSliceSignal(SliceSignal signal)
    {
        Slice(signal.SlicePosition, signal.SliceDirection, signal.SliceObject.breadMeshGO, signal.SliceObject.sliceMaterial, signal.SliceObject);
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
        slice.AddComponent<BoxCollider>().size = bread._sliceSize;;
        slice.AddComponent<Rigidbody>();//.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    public override void Dispose()
    {
        _signalBus.Unsubscribe<SliceSignal>(OnSliceSignal);
    }
}
