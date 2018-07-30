using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;


/** Gestures
 * Author: Harrison Pham
 * This class is the "2.0 ver" of GesturesManager
 * 
 * The class listens for gesture events, 
 *      (user has to define which to listen to **See Awake())
 * checks if the user is looking at an IObject,
 * Then calls the associated "Reaction", to the current gesture,
 * from the IObject.
 * 
 * The Class uses Recognizers to listen/poll for gestures.
 * When an event occurs, call the associated function **See Awake()
 * Use the reference below for a list of the gesture types, 
 * and gesture events.
 * Gesture Types:
 * https://docs.unity3d.com/ScriptReference/XR.WSA.Input.GestureSettings.html
 * 
 * Gesture Events:
 * https://docs.unity3d.com/550/Documentation/ScriptReference/VR.WSA.Input.GestureRecognizer.html
 * 
 */
public class Gestures : MonoBehaviour {

    public Transform cam;
    public UserClass user;

    /**Gesture Recognizers
     * 
     * These types are essential for listening/polling gestures.
     * There are multiple recognizers, due to some gestures being
     * very similar, thus there is a need to distinguish which gestures 
     * is needed at certain cases.
     * 
     * For this instance, there are 2 cases. When the user is navigating/scrolling on
     * an object, and when they are manipulating/moving an object.
     * 
     */
    //activeRecognizer is set to the case or state the user is in (navigating or manipulating)
    GestureRecognizer activeRecognizer;
    //navigation is the listener to navigation events
    GestureRecognizer navigation;
    //manipulation is the listener to manipulation events
    GestureRecognizer manipulation;

    /**Gesture Recognizers (by State)
     * 
     * These are gesture recongizers for each state
     * 
     */
    GestureRecognizer caliAdj;
    GestureRecognizer inter_Hold;
    GestureRecognizer inter_Nav;
    GestureRecognizer inter_Man;

    //enum for cycling through gestures in interaction state
    public enum interType
    {
        Hold, Nav, Man
    };
    interType inty;

    //selected is the currently selected IObject, if there is one, during a gesture event
    IObject selected;

    //navOn is a bool to switch between the navigation and manipulation recognizers
    bool navOn;



    /** Awake
     * 
     * Here is where we setup the listeners.
     * Each listener is given the type of gestures it should listen to,
     * and it subscribes to the events associated to the gesture types.
     * 
     * To subscribe to a gesture event, it is of the following form
     *      Recognizer.EventName += (Programmer defined function)
     * **See the top of the class for references to Gesture Types and Events.
     * 
     * By default, navigation is the active recognizer
     * 
     */ 
    void Awake () {
        //SetUp gesture listening for navigation
        navigation = new GestureRecognizer();
        //Specify which gestures the listener should listen to
        navigation.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.Hold);
        //Listener subscribes to associated gesture events
		navigation.Tapped += All_Tap;
		navigation.HoldStarted += Hold_Started;
		navigation.HoldCanceled += Hold_Canceled;
		navigation.HoldCompleted += Hold_Completed;

        //SetUp gesture listening for manipulation
        manipulation = new GestureRecognizer();
        //Specify which gestures the listener should listen to
		manipulation.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.DoubleTap | GestureSettings.ManipulationTranslate);
        //Listener subscribes to associated gesture events
        manipulation.Tapped += All_Tap;
		manipulation.ManipulationStarted += Man_Start;
		manipulation.ManipulationUpdated += Man_Update;
		manipulation.ManipulationCanceled += Man_Cancel;
		manipulation.ManipulationCompleted += Man_Complete;

        //Set the navigation as the default active listener
        navOn = false;
        //SwitchTo(navigation);
        SwitchTo(manipulation);


        //State Gestures
        caliAdj = new GestureRecognizer();
        caliAdj.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.DoubleTap | GestureSettings.ManipulationTranslate);
        caliAdj.Tapped += All_Tap;
        caliAdj.ManipulationStarted += Man_Start;
        caliAdj.ManipulationUpdated += Man_Update;
        caliAdj.ManipulationCanceled += Man_Cancel;
        caliAdj.ManipulationCompleted += Man_Complete;

        inter_Hold = new GestureRecognizer();
        inter_Hold.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.DoubleTap | GestureSettings.Hold);
        inter_Hold.Tapped += All_Tap;
        inter_Hold.HoldStarted += Hold_Started;
        inter_Hold.HoldCanceled += Hold_Canceled;
        inter_Hold.HoldCompleted += Hold_Completed;

        inter_Man = new GestureRecognizer();
        inter_Man.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.DoubleTap | GestureSettings.ManipulationTranslate);
        inter_Man.Tapped += All_Tap;
        inter_Man.ManipulationStarted += Man_Start;
        inter_Man.ManipulationUpdated += Man_Update;
        inter_Man.ManipulationCanceled += Man_Cancel;
        inter_Man.ManipulationCompleted += Man_Complete;

        inter_Nav = new GestureRecognizer();
        inter_Nav.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.DoubleTap | 
                                        GestureSettings.NavigationX | GestureSettings.NavigationY);
        inter_Nav.Tapped += All_Tap;
        inter_Nav.NavigationStarted += Nav_Start;
        inter_Nav.NavigationUpdated += Nav_Update;
        inter_Nav.NavigationCanceled += Nav_Cancel;
        inter_Nav.NavigationCompleted += Nav_Complete;
    }

    /** OnDestroy
     * 
     * Clean up purposes.
     * Here, the listeners must unsubscribe to their gesture events.
     * 
     * To unsubscribe from a gesture event, it is of the following form
     *      Recognizer.EventName -= (Programmer defined function)
     * **See the top of the class for references to Gesture Types and Events.
     * 
     */
    void OnDestroy () {
        //unsubscribe from navigation events
		navigation.Tapped -= All_Tap;
		navigation.HoldStarted -= Hold_Started;
		navigation.HoldCanceled -= Hold_Canceled;
		navigation.HoldCompleted -= Hold_Completed;

        //unsubscribe form manipulation events
		manipulation.Tapped -= All_Tap;
		manipulation.ManipulationStarted -= Man_Start;
		manipulation.ManipulationUpdated -= Man_Update;
		manipulation.ManipulationCanceled -= Man_Cancel;
		manipulation.ManipulationCompleted -= Man_Complete;

        //State Gestures
        caliAdj.Tapped -= All_Tap;
        caliAdj.ManipulationStarted -= Man_Start;
        caliAdj.ManipulationUpdated -= Man_Update;
        caliAdj.ManipulationCanceled -= Man_Cancel;
        caliAdj.ManipulationCompleted -= Man_Complete;

        inter_Hold.Tapped -= All_Tap;
        inter_Hold.HoldStarted -= Hold_Started;
        inter_Hold.HoldCanceled -= Hold_Canceled;
        inter_Hold.HoldCompleted -= Hold_Completed;

        inter_Man.Tapped -= All_Tap;
        inter_Man.ManipulationStarted -= Man_Start;
        inter_Man.ManipulationUpdated -= Man_Update;
        inter_Man.ManipulationCanceled -= Man_Cancel;
        inter_Man.ManipulationCompleted -= Man_Complete;

        inter_Nav.Tapped -= All_Tap;
        inter_Nav.NavigationStarted -= Nav_Start;
        inter_Nav.NavigationUpdated -= Nav_Update;
        inter_Nav.NavigationCanceled -= Nav_Cancel;
        inter_Nav.NavigationCompleted -= Nav_Complete;
    }

    /**
     * These are not really needed, but are left just in case
     * 
     */ 
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    //General functions (non-specific to a listener)

    /** SwitchTo
     * 
     * This switches between the navigation and manipulation recognizers.
     * Here is where the listeners are told to start/stop listening to gestures.
     * 
     */ 
    void SwitchTo (GestureRecognizer gr) {
        //return if the specified recognizer isn't initialized
        if (gr == null) { return; }

        //If there is an active recognizer currently listening to event,
        //cancel all its current events and have it stop listening.
        if (activeRecognizer != null) {
            if (activeRecognizer == gr) { return; }

            activeRecognizer.CancelGestures();
            activeRecognizer.StopCapturingGestures();
        }

        //Tell the specified recongizer that it can start listening to gestures,
        //and set it as the active recognizer
        gr.StartCapturingGestures();
        activeRecognizer = gr;
    }

    public void GesturesFor(StateMachine.State state)
    {
        switch (state.ToString())
        {
            case "Calibration":
                SwitchTo(caliAdj);
                break;
            case "Adjusting":
                SwitchTo(caliAdj);
                break;
            case "Interaction":
                SwitchTo(inter_Hold);
                inty = interType.Hold;
                break;
            default:
                break;
        }
    }

    public void CycleInteractionGestures()
    {
        int cur = (int)inty;
        cur++;
        if (cur == Enum.GetNames(typeof(interType)).Length) { cur = 0; }
        inty = (interType)cur;

        switch ((int)inty)
        {
            case (int)interType.Hold:
                SwitchTo(inter_Hold);
                break;
            case (int)interType.Nav:
                SwitchTo(inter_Nav);
                break;
            case (int)interType.Man:
                SwitchTo(inter_Man);
                break;
            default:
                break;
        }
    }

    /** SwitchListeners
     * 
     * Switch between the navigation and manipulation recognizers 
     * based on a global bool.
     * 
     */
    public void SwitchListeners () {
        navOn = !navOn;
        if (navOn) { SwitchTo(navigation); }
        else { SwitchTo(manipulation); }
    }

    //Navigation specific functions (Mainly event handling)
    void All_Tap (TappedEventArgs args) {
        IObject obj = IObject.selectObject();
        if (obj)
            obj.PressedReaction(args);
        else
            user.Reaction.PressedReaction(args);
    }

    void Nav_Start (NavigationStartedEventArgs args) {
        if (selected) { return; }

        selected = IObject.selectObject();
        if (selected)
            selected.NavStartReaction(args);
    }

    void Nav_Update (NavigationUpdatedEventArgs args) {
        if (selected == null) { return; }

        selected.NavUpdateReaction(args);
    }

    void Nav_Complete (NavigationCompletedEventArgs args) {
        if (selected == null) { return; }
        selected.NavCompleteReaction(args);
        selected = null;
    }

    void Nav_Cancel (NavigationCanceledEventArgs args) {
        if (selected == null) { return; }
        selected.NavCancelReaction(args);
        selected = null;
    }

    //Manipulation specfic functions (Mainly event handling)
    void Man_Start (ManipulationStartedEventArgs args) {
        if (selected) { return; }

        selected = IObject.selectObject();
        if (selected)
            selected.ManStartReaction(args);

    }

    void Man_Update (ManipulationUpdatedEventArgs args) {
        if (selected == null) { return; }

        selected.ManUpdateReaction(args);

    }

    void Man_Complete (ManipulationCompletedEventArgs args) {
        if (selected == null) { return; }
        selected.ManCompleteReaction(args);
        selected = null;
    }

    void Man_Cancel (ManipulationCanceledEventArgs args) {
        if (selected == null) { return; }
        selected.ManCancelReaction(args);
        selected = null;
    }

    void Hold_Started(HoldStartedEventArgs args) {
        if (selected) { return; }
        IObject obj = IObject.selectObject();
        if (obj)
        {
            selected = obj;
            obj.HoldStartReaction(args);
        }
        else
            user.Reaction.HoldStartReaction(args);
    }

    void Hold_Canceled(HoldCanceledEventArgs args) {
        if (selected == null) {
            user.Reaction.HoldCancelReaction(args);
            return;
        }

        selected.HoldCancelReaction(args);
        selected = null;
    }

    void Hold_Completed(HoldCompletedEventArgs args) {
        if (selected == null) {
            user.Reaction.HoldCompleteReaction(args);
            return;
        }

        selected.HoldCompleteReaction(args);
        selected = null;
    }
}
