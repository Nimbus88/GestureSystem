using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;
using Reactions;


[RequireComponent(typeof(SpatialMappingCollider))]
[RequireComponent(typeof(SpatialMappingRenderer))]
/** SMap
 *  (Spatial Map)
 * 
 * Turns on/off the spatial map wire mesh visuals.
 * 
 */ 
public class SMap : IObject, CaliHold {

    SpatialMappingCollider col;
    SpatialMappingRenderer rend;

    bool occlu;
    bool active;

    // Use this for initialization
    void Start () {
        col = GetComponent<SpatialMappingCollider>();
        rend = GetComponent<SpatialMappingRenderer>();

        active = true;
        occlu = true;
    }

    public void TurnOnVisuals(bool on) {
        rend.renderState = on ? SpatialMappingRenderer.RenderState.Visualization : SpatialMappingRenderer.RenderState.Occlusion;
    }

    public void ToggleVisuals()
    {
        occlu = !occlu;
        rend.renderState = occlu?SpatialMappingRenderer.RenderState.Occlusion:SpatialMappingRenderer.RenderState.Visualization;
    }

    public void ToggleActivity()
    {
        active = !active;
        rend.enabled = active ? true : false;
        col.enabled = active ? true : false;
    }

    void CaliHold.HoldStart(HoldStartedEventArgs args) { TurnOnVisuals(true); }
    void CaliHold.HoldCancel(HoldCanceledEventArgs args) { TurnOnVisuals(false); }
    void CaliHold.HoldComplete(HoldCompletedEventArgs args) { TurnOnVisuals(false); }
}
