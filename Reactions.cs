using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

/** Reactions
 *  Author: Harrison Pham
 *  
 *  List of interfaces corresponding to a given state and gesture.
 *  There could be many states, and in each state, each gesture
 *  can have different meanings.
 * 
 */ 
namespace Reactions
{
    // calibration
    public interface CaliTap
    {
        void SingleTap(TappedEventArgs args);
        void DoubleTap(TappedEventArgs args);
    }

    public interface CaliHold
    {
        void HoldStart(HoldStartedEventArgs args);
        void HoldCancel(HoldCanceledEventArgs args);
        void HoldComplete(HoldCompletedEventArgs args);
    }

    public interface CaliNav
    {
        void NavStart(NavigationStartedEventArgs args);
        void NavUpdate(NavigationUpdatedEventArgs args);
        void NavCancel(NavigationCanceledEventArgs args);
        void NavComplete(NavigationCompletedEventArgs args);
    }

    public interface CaliMan
    {
        void ManStart(ManipulationStartedEventArgs args);
        void ManUpdate(ManipulationUpdatedEventArgs args);
        void ManCancel(ManipulationCanceledEventArgs args);
        void ManComplete(ManipulationCompletedEventArgs args);
    }


    // adjust
    public interface AdjTap
    {
        void SingleTap(TappedEventArgs args);
        void DoubleTap(TappedEventArgs args);
    }

    public interface AdjHold
    {
        void HoldStart(HoldStartedEventArgs args);
        void HoldCancel(HoldCanceledEventArgs args);
        void HoldComplete(HoldCompletedEventArgs args);
    }

    public interface AdjNav
    {
        void NavStart(NavigationStartedEventArgs args);
        void NavUpdate(NavigationUpdatedEventArgs args);
        void NavCancel(NavigationCanceledEventArgs args);
        void NavComplete(NavigationCompletedEventArgs args);
    }

    public interface AdjMan
    {
        void ManStart(ManipulationStartedEventArgs args);
        void ManUpdate(ManipulationUpdatedEventArgs args);
        void ManCancel(ManipulationCanceledEventArgs args);
        void ManComplete(ManipulationCompletedEventArgs args);
    }

    // interaction
    public interface InterTap
    {
        void SingleTap(TappedEventArgs args);
        void DoubleTap(TappedEventArgs args);
    }

    public interface InterHold
    {
        void HoldStart(HoldStartedEventArgs args);
        void HoldCancel(HoldCanceledEventArgs args);
        void HoldComplete(HoldCompletedEventArgs args);
    }

    public interface InterNav
    {
        void NavStart(NavigationStartedEventArgs args);
        void NavUpdate(NavigationUpdatedEventArgs args);
        void NavCancel(NavigationCanceledEventArgs args);
        void NavComplete(NavigationCompletedEventArgs args);
    }

    public interface InterMan
    {
        void ManStart(ManipulationStartedEventArgs args);
        void ManUpdate(ManipulationUpdatedEventArgs args);
        void ManCancel(ManipulationCanceledEventArgs args);
        void ManComplete(ManipulationCompletedEventArgs args);
    }
}