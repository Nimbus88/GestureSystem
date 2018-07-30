using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using Reactions;

/** IObject (Interactable Object)
 *  Author: Harrison Pham
 * 
 * Abstract class that lets its children define their own
 * "Reaction" to various gesture events.
 * 
 */
public class IObject : MonoBehaviour
{

    /** selectObject
     * 
     * This static functions uses raycasting to see if the user 
     * is looking at an IObject.
     * 
     * It is static, to allow any class to call it.
     * 
     */
    public static IObject selectObject()
    {
        //Get the postion the user/camera
        Vector3 pos = Camera.main.transform.position;
        //Get the direction the user/camera is looking at
        Vector3 dir = Camera.main.transform.forward;

        //Raycasting
        RaycastHit hit;
        //If the Raycast hit something
        if (Physics.Raycast(pos, dir, out hit))
        {
            IObject io = hit.transform.GetComponent<IObject>();
            //If the object that was hit, is an IObject
            if (io)
            {
                //Debug.Log("Found Object");
                return io;
            }
            //If the object is not and IObject
            else
            {
                Debug.Log(hit.transform.name);
                //Debug.Log("NULL");
                return null;
            }
        }
        //If the Raycast didn't hit anything
        else
        {
            //Debug.Log("Nothing");
            return null;
        }
    }

    /** Reaction Functions
     * 
     * These functions are called by a Gesture Manager;
     * telling the selected IObject to "react" to a certain 
     * gesture event.
     * 
     * Ideally, these are abstract, allowing each child of an IObject
     * to define their own reactions to gesture events.
     * 
     */


    ///////////////////////////////////////////////////////////////////
    ///                 Pressed/Tapped Reactions                   ////
    ///////////////////////////////////////////////////////////////////
    //  Gestures for tapping and object

    /** PressedReaction
     * 
     * Function for reactions to a pressed/tapped event
     * 
     */
    public void PressedReaction(TappedEventArgs args)
    {
        if (args.tapCount == 2)
            DoubleTapReaction(args);
        else
            SingleTapReaction(args);
    }

    /** SingleTapReaction
     * 
     * Function for reactions to a single tap event
     * 
     */
    public void SingleTapReaction(TappedEventArgs args)
    {
        StateMachine.State state = StateMachine.Instance.state;
        switch(state)
        {
            case StateMachine.State.Calibration:
                CaliTap c = this as CaliTap;
                if (c != null) { c.SingleTap(args); }
                break;
            case StateMachine.State.Interaction:
                InterTap i = this as InterTap;
                if (i != null) { i.SingleTap(args); }
                break;
            case StateMachine.State.Adjusting:
                AdjTap a = this as AdjTap;
                if (a != null) { a.SingleTap(args); }
                break;
            default:
                Debug.Log("Unkonwn State");
                break;
        }
    }

    /** DoubleTapReaction
     * 
     * Function for reactions to a double tap event
     * 
     */
    public void DoubleTapReaction(TappedEventArgs args)
    {
        StateMachine.State state = StateMachine.Instance.state;
        switch (state)
        {
            case StateMachine.State.Calibration:
                CaliTap c = this as CaliTap;
                if (c != null) { c.DoubleTap(args); }
                break;
            case StateMachine.State.Interaction:
                InterTap i = this as InterTap;
                if (i != null) { i.DoubleTap(args); }
                break;
            case StateMachine.State.Adjusting:
                AdjTap a = this as AdjTap;
                if (a != null) { a.DoubleTap(args); }
                break;
            default:
                Debug.Log("Unkonwn State");
                break;
        }
    }


    ////////////////////////////////////////////////////////////////////
    ///                      Hold Reactions                         ////
    ////////////////////////////////////////////////////////////////////
    // Gestures for pinching and holding an object

    /** HoldStartReaction
    * 
    * Called at the start of the Manipulation/move event.
    * 
    */
    public void HoldStartReaction(HoldStartedEventArgs args)
    {
        StateMachine.State state = StateMachine.Instance.state;
        switch (state)
        {
            case StateMachine.State.Calibration:
                CaliHold c = this as CaliHold;
                if (c != null) { c.HoldStart(args); }
                break;
            case StateMachine.State.Interaction:
                InterHold i = this as InterHold;
                if (i != null) { i.HoldStart(args); }
                break;
            case StateMachine.State.Adjusting:
                AdjHold a = this as AdjHold;
                if (a != null) { a.HoldStart(args); }
                break;
            default:
                Debug.Log("Unkonwn State");
                break;
        }
    }

    /** HoldCancelReaction
     * 
     * Called when the Manipulation/move event is Canceled.
     * 
     */
    public void HoldCancelReaction(HoldCanceledEventArgs args)
    {
        StateMachine.State state = StateMachine.Instance.state;
        switch (state)
        {
            case StateMachine.State.Calibration:
                CaliHold c = this as CaliHold;
                if (c != null) { c.HoldCancel(args); }
                break;
            case StateMachine.State.Interaction:
                InterHold i = this as InterHold;
                if (i != null) { i.HoldCancel(args); }
                break;
            case StateMachine.State.Adjusting:
                AdjHold a = this as AdjHold;
                if (a != null) { a.HoldCancel(args); }
                break;
            default:
                Debug.Log("Unkonwn State");
                break;
        }
    }

    /** HoldCompleteReaction
     * 
     * Called when the Manipulation/move event is completed.
     * 
     */
    public void HoldCompleteReaction(HoldCompletedEventArgs args)
    {
        StateMachine.State state = StateMachine.Instance.state;
        switch (state)
        {
            case StateMachine.State.Calibration:
                CaliHold c = this as CaliHold;
                if (c != null) { c.HoldComplete(args); }
                break;
            case StateMachine.State.Interaction:
                InterHold i = this as InterHold;
                if (i != null) { i.HoldComplete(args); }
                break;
            case StateMachine.State.Adjusting:
                AdjHold a = this as AdjHold;
                if (a != null) { a.HoldComplete(args); }
                break;
            default:
                Debug.Log("Unkonwn State");
                break;
        }
    }


    ////////////////////////////////////////////////////////////////////
    ///                   Navigation Reactions                      ////
    ////////////////////////////////////////////////////////////////////
    // Gestures for pinching and dragging an object
    // Mainly used for "Scrolling" or "Rotating"

    /** NavStartReaction
     * 
     * Called at the start of an Navigation event.
     * 
     */
    public void NavStartReaction(NavigationStartedEventArgs args)
    {
        StateMachine.State state = StateMachine.Instance.state;
        switch (state)
        {
            case StateMachine.State.Calibration:
                CaliNav c = this as CaliNav;
                if (c != null) { c.NavStart(args); }
                break;
            case StateMachine.State.Interaction:
                InterNav i = this as InterNav;
                if (i != null) { i.NavStart(args); }
                break;
            case StateMachine.State.Adjusting:
                AdjNav a = this as AdjNav;
                if (a != null) { a.NavStart(args); }
                break;
            default:
                Debug.Log("Unkonwn State");
                break;
        }
    }

    /** NavUpdateReaction
     * 
     * Called during the Navigation event.
     * 
     */
    public void NavUpdateReaction(NavigationUpdatedEventArgs args)
    {
        StateMachine.State state = StateMachine.Instance.state;
        switch (state)
        {
            case StateMachine.State.Calibration:
                CaliNav c = this as CaliNav;
                if (c != null) { c.NavUpdate(args); }
                break;
            case StateMachine.State.Interaction:
                InterNav i = this as InterNav;
                if (i != null) { i.NavUpdate(args); }
                break;
            case StateMachine.State.Adjusting:
                AdjNav a = this as AdjNav;
                if (a != null) { a.NavUpdate(args); }
                break;
            default:
                Debug.Log("Unkonwn State");
                break;
        }
    }

    /** NavCancelReaction
     * 
     * Called when the Navigation event is Canceled.
     * 
     */
    public void NavCancelReaction(NavigationCanceledEventArgs args)
    {
        StateMachine.State state = StateMachine.Instance.state;
        switch (state)
        {
            case StateMachine.State.Calibration:
                CaliNav c = this as CaliNav;
                if (c != null) { c.NavCancel(args); }
                break;
            case StateMachine.State.Interaction:
                InterNav i = this as InterNav;
                if (i != null) { i.NavCancel(args); }
                break;
            case StateMachine.State.Adjusting:
                AdjNav a = this as AdjNav;
                if (a != null) { a.NavCancel(args); }
                break;
            default:
                Debug.Log("Unkonwn State");
                break;
        }
    }

    /** NavCompleteReaction
     * 
     * Called when the Navigation event is completed.
     * 
     */
    public void NavCompleteReaction(NavigationCompletedEventArgs args)
    {
        StateMachine.State state = StateMachine.Instance.state;
        switch (state)
        {
            case StateMachine.State.Calibration:
                CaliNav c = this as CaliNav;
                if (c != null) { c.NavComplete(args); }
                break;
            case StateMachine.State.Interaction:
                InterNav i = this as InterNav;
                if (i != null) { i.NavComplete(args); }
                break;
            case StateMachine.State.Adjusting:
                AdjNav a = this as AdjNav;
                if (a != null) { a.NavComplete(args); }
                break;
            default:
                Debug.Log("Unkonwn State");
                break;
        }
    }


    ////////////////////////////////////////////////////////////////////
    ///                   Manipulation Reactions                    ////
    ////////////////////////////////////////////////////////////////////
    // Gestures are the same as Navigation
    // Mainly used for "Moving" or "Scaling"

    /** ManStartReaction
     * 
     * Called at the start of the Manipulation/move event.
     * 
     */
    public void ManStartReaction(ManipulationStartedEventArgs args)
    {
        StateMachine.State state = StateMachine.Instance.state;
        switch (state)
        {
            case StateMachine.State.Calibration:
                CaliMan c = this as CaliMan;
                if (c != null) { c.ManStart(args); }
                break;
            case StateMachine.State.Interaction:
                InterMan i = this as InterMan;
                if (i != null) { i.ManStart(args); }
                break;
            case StateMachine.State.Adjusting:
                AdjMan a = this as AdjMan;
                if (a != null) { a.ManStart(args); }
                break;
            default:
                Debug.Log("Unkonwn State");
                break;
        }
    }

    /** ManUpdateReaction
     * 
     * Called during the Manipulation/move event.
     * 
     */
    public void ManUpdateReaction(ManipulationUpdatedEventArgs args)
    {
        StateMachine.State state = StateMachine.Instance.state;
        switch (state)
        {
            case StateMachine.State.Calibration:
                CaliMan c = this as CaliMan;
                if (c != null) { c.ManUpdate(args); }
                break;
            case StateMachine.State.Interaction:
                InterMan i = this as InterMan;
                if (i != null) { i.ManUpdate(args); }
                break;
            case StateMachine.State.Adjusting:
                AdjMan a = this as AdjMan;
                if (a != null) { a.ManUpdate(args); }
                break;
            default:
                Debug.Log("Unkonwn State");
                break;
        }
    }

    /** ManCancelReaction
     * 
     * Called when the Manipulation/move event is Canceled.
     * 
     */
    public void ManCancelReaction(ManipulationCanceledEventArgs args)
    {
        StateMachine.State state = StateMachine.Instance.state;
        switch (state)
        {
            case StateMachine.State.Calibration:
                CaliMan c = this as CaliMan;
                if (c != null) { c.ManCancel(args); }
                break;
            case StateMachine.State.Interaction:
                InterMan i = this as InterMan;
                if (i != null) { i.ManCancel(args); }
                break;
            case StateMachine.State.Adjusting:
                AdjMan a = this as AdjMan;
                if (a != null) { a.ManCancel(args); }
                break;
            default:
                Debug.Log("Unkonwn State");
                break;
        }
    }

    /** ManCompleteReaction
     * 
     * Called when the Manipulation/move event is completed.
     * 
     */
    public void ManCompleteReaction(ManipulationCompletedEventArgs args)
    {
        StateMachine.State state = StateMachine.Instance.state;
        switch (state)
        {
            case StateMachine.State.Calibration:
                CaliMan c = this as CaliMan;
                if (c != null) { c.ManComplete(args); }
                break;
            case StateMachine.State.Interaction:
                InterMan i = this as InterMan;
                if (i != null) { i.ManComplete(args); }
                break;
            case StateMachine.State.Adjusting:
                AdjMan a = this as AdjMan;
                if (a != null) { a.ManComplete(args); }
                break;
            default:
                Debug.Log("Unkonwn State");
                break;
        }
    }

}

