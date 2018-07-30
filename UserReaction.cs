using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using Reactions;

[RequireComponent(typeof(UserClass))]
/** UserReaction
 *  Author: Harrison Pham
 * 
 * If user executes a gesture, and no IObject was detected,
 * the user executes their "Reaction" to the gesture.
 * 
 * In the Reactions section, programmers specify the desired actions,
 * given the state and gesture.
 * 
 */ 
public class UserReaction : IObject, CaliTap, CaliHold, CaliNav, CaliMan, 
                                     AdjTap, AdjHold, AdjNav, AdjMan, 
                                     InterTap, InterHold, InterNav, InterMan
{
    // Variables
    UserClass _user;


    void Start() {
        _user = GetComponent<UserClass>();
    }

	/////////////////////////////////////////////////
	/// 				Methods					/////
	/////////////////////////////////////////////////

    /** placePoints
     * 
     * Places a virtual marker based on the global position from the parameter.
     * This position is usually a raycast from the user to some collider.
     * 
     */ 
	bool placePoints(Vector3 h) {
		if (_user.corners.Count == 4) { return false; }
		StateMachine sm = StateMachine.Instance;

		Vector3 pos = new Vector3 (h.x, h.y, h.z);
		GameObject o = (GameObject)Instantiate (sm.cornerP, pos, Quaternion.identity);
		int count = _user.corners.Count + 1;
		o.name = ("c"+ count.ToString());
		//o.SetActive (false);
		o.GetComponent<Collider>().enabled = false;
		_user.corners.Add (o);
		o.transform.parent = _user.CObjects.transform;
		return true;
	}

    /** placeMarker
     *  (Vuforia required)
     * 
     * Places a marker, in global space, if an AR Tag was recognized by the device.
     * The position is already specified in the AR Tag prefab.
     * 
     */ 
    void placeMarker()
    {
        if (_user.corners.Count == 4) { return; }
        if (_user.marker)
        {
            if (_user.marker.IsVisible())
            {
                Vector3 h = _user.marker.GetPosition();
                StateMachine sm = StateMachine.Instance;

                Vector3 pos = new Vector3(h.x, h.y, h.z);
                GameObject o = (GameObject)Instantiate(sm.cornerP, pos, Quaternion.identity);
                int count = _user.corners.Count + 1;
                o.name = ("c" + count.ToString());
                //o.SetActive (false);
                o.GetComponent<Collider>().enabled = false;
                _user.corners.Add(o);
                o.transform.parent = _user.CObjects.transform;
            }
        }
    }

    //////////////////////////////////////////////////
    /////               Reactions                /////
    //////////////////////////////////////////////////

    // Calibration
    void CaliTap.SingleTap(TappedEventArgs args) {
        //places points by on location of tap
        /*
        Vector3 pos = Camera.main.transform.position;
        Vector3 dir = Camera.main.transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(pos, dir, out hit))
        {
            Vector3 h = hit.point;
			placePoints (h);
        }
        */
        placeMarker();
    }
    void CaliTap.DoubleTap(TappedEventArgs args) {
        Debug.Log("double tap");
    }

    void CaliHold.HoldStart(HoldStartedEventArgs args) { _user.spatial.HoldStartReaction(args); }
    void CaliHold.HoldCancel(HoldCanceledEventArgs args) { _user.spatial.HoldCancelReaction(args); }
    void CaliHold.HoldComplete(HoldCompletedEventArgs args) { _user.spatial.HoldCompleteReaction(args); }

    void CaliNav.NavStart(NavigationStartedEventArgs args) { }
    void CaliNav.NavUpdate(NavigationUpdatedEventArgs args) { }
    void CaliNav.NavCancel(NavigationCanceledEventArgs args) { }
    void CaliNav.NavComplete(NavigationCompletedEventArgs args) { }

    void CaliMan.ManStart(ManipulationStartedEventArgs args) { }
    void CaliMan.ManUpdate(ManipulationUpdatedEventArgs args) { }
    void CaliMan.ManCancel(ManipulationCanceledEventArgs args) { }
    void CaliMan.ManComplete(ManipulationCompletedEventArgs args) { }

    // Interaction
    void InterTap.SingleTap(TappedEventArgs args) { }
    void InterTap.DoubleTap(TappedEventArgs args) { }

    void InterHold.HoldStart(HoldStartedEventArgs args) { }
    void InterHold.HoldCancel(HoldCanceledEventArgs args) { }
    void InterHold.HoldComplete(HoldCompletedEventArgs args) { }

    void InterNav.NavStart(NavigationStartedEventArgs args) { }
    void InterNav.NavUpdate(NavigationUpdatedEventArgs args) { }
    void InterNav.NavCancel(NavigationCanceledEventArgs args) { }
    void InterNav.NavComplete(NavigationCompletedEventArgs args) { }

    void InterMan.ManStart(ManipulationStartedEventArgs args) { }
    void InterMan.ManUpdate(ManipulationUpdatedEventArgs args) { }
    void InterMan.ManCancel(ManipulationCanceledEventArgs args) { }
    void InterMan.ManComplete(ManipulationCompletedEventArgs args) { }

    // Adjust
    void AdjTap.SingleTap(TappedEventArgs args) { }
    void AdjTap.DoubleTap(TappedEventArgs args) { _user.doneAdjusting = true; }

    void AdjHold.HoldStart(HoldStartedEventArgs args) { }
    void AdjHold.HoldCancel(HoldCanceledEventArgs args) { }
    void AdjHold.HoldComplete(HoldCompletedEventArgs args) { }

    void AdjNav.NavStart(NavigationStartedEventArgs args) { }
    void AdjNav.NavUpdate(NavigationUpdatedEventArgs args) { }
    void AdjNav.NavCancel(NavigationCanceledEventArgs args) { }
    void AdjNav.NavComplete(NavigationCompletedEventArgs args) { }

    void AdjMan.ManStart(ManipulationStartedEventArgs args) { }
    void AdjMan.ManUpdate(ManipulationUpdatedEventArgs args) { }
    void AdjMan.ManCancel(ManipulationCanceledEventArgs args) { }
    void AdjMan.ManComplete(ManipulationCompletedEventArgs args) { }
}
