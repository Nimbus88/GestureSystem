using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;
using Vuforia;

/** StateMachine
 *  Author: Harrison Pham
 *  
 * A State machine to help break down the main application
 * into feasible parts.
 * Defines when to transition between states.
 * 
 * States:
 *      InputID - User enters chart ID on a keypad
 *      Calibration - User places AR Tags at, 
 *                    or taps (depricated), corners of 
 *                    physical chart to place virtual anchors
 *                    to create a bounding box of the chart.
 *                    Must go from top left to bottom right,
 *                    in a 'Z' motion.
 *      Adjusting - User may fine tune the bounding box for
 *                  improved accuracy.
 *      Interaction - User may interact with the Virtual overlay
 *                    or Virtual HUD.
 * 
 */ 
public class StateMachine : Singleton<StateMachine> {

    public enum State {
        InputID,
        Calibration,
        Interaction,
        Adjusting,
        Error
    };

    public State state;

    public Transform cam;
	public SMap sMap;
	public UserClass user;
	public VirtualMapRenderer vMap;
    public Scrape wS;
    public GameObject pad;
    public GameObject hud;
    public MapViewer viewer;

	public GameObject cornerP;

	bool enteringAdj = false;

    public bool Debug_SelState = false;
    // Use this for initialization
    void Start () {
    }

    //If not debugging, handle transitions to states
    void Update()
    {

        if (!Debug_SelState)
        {
            if (state == State.InputID)
            {
                // turn off Virtual HUD and Map
                if (hud.activeSelf)
                {
                    hud.SetActive(false);
                }
                if (vMap.gameObject.activeSelf)
                {
                    vMap.gameObject.SetActive(false);
                }

                //transition when user inputted chart ID
                if (wS.passed) { ChangeState(State.Calibration); }
            }
            else if (state == State.Calibration)
            {
                if (user.corners.Count < 4)
                {
                }
                //transition when user has placed 4 markers
                else if (user.corners.Count == 4)
                {
                    ChangeState(State.Adjusting);
                }
                //Off-chance user placed more than 4 markers
                else
                {
                    state = State.Error;
                }
                return;
            }
            else if (state == State.Adjusting)
            {
                //adjust bounds of chart
                if (enteringAdj)
                {
                    enteringAdj = false;
                    user.doneAdjusting = false;
                }
                //transition when user is done adjusting chart
                if (user.doneAdjusting)
                {
                    ChangeState(State.Interaction);
                }
            }
            else if (state == State.Interaction)
            {
                //VirtualMapModifier vmm = vMap.GetComponent<VirtualMapModifier>();
                //vMap.ApplyFilter(vmm.VisibleFilter);
            }
            else
            {
            }

        }
    }

    /** ChangeState
     * 
     * Changes to a different state.
     * Cleans up current state, and sets up new state.
     * 
     */ 
    public void ChangeState(State to)
    {
        //cleanup
        switch (state.ToString())
        {
            case "InputID":
                DoneInput();
                break;
            case "Calibration":
                DoneCalibrate();
                break;
            case "Adjusting":
                DoneAdjust();
                break;
            case "Interaction":
                DoneInteract();
                break;
            default:
                break;
        }

        //transition
        switch (to.ToString())
        {
            case "InputID":
                ToInput();
                break;
            case "Calibration":
                ToCalibrate();
                break;
            case "Adjusting":
                ToAdjust();
                break;
            case "Interaction":
                ToInteract();
                break;
            default:
                break;
        }
    }

    //State Clean up and Set up functions

    void ToInput()
    {
        pad.SetActive(true);
        wS.passed = false;
        state = State.InputID;
    }

    void DoneInput()
    {
        pad.SetActive(false);
    }

    void ToCalibrate()
    {
        cam.GetComponent<VuforiaBehaviour>().enabled = true;

        vMap.gameObject.SetActive(false);

        user.corners.Clear();
        Transform crns = user.CObjects.transform;
        foreach (Transform o in crns)
        {
            GameObject.Destroy(o.gameObject);
        }


        //sMap.ToggleActivity();
        state = State.Calibration;
        GetComponent<Gestures>().GesturesFor(State.Calibration);
    }

    void DoneCalibrate()
    {
        cam.GetComponent<VuforiaBehaviour>().enabled = false;

        user.CObjects.GetComponent<AdjustMap>().setPoints(user.corners.ToArray());
        //sMap.ToggleActivity();

        vMap.gameObject.SetActive(true);
        vMap.CreateMap(user.corners.ToArray());
        //wS.GrabData("13285");
    }

    void ToAdjust()
    {
        enteringAdj = true;
        vMap.GetComponent<LineRenderer>().enabled = true;
        foreach (GameObject o in user.corners)
        {
            //o.SetActive (true);
            o.GetComponent<SphereCollider>().enabled = true;
            Debug.Log(o.transform.position);
        }

        state = State.Adjusting;
        GetComponent<Gestures>().GesturesFor(State.Adjusting);
    }

    void DoneAdjust()
    {
        user.doneAdjusting = false;
        user.CObjects.GetComponent<AdjustMap>().EnableCorners(false);
        //vMap.GetComponent<LineRenderer>().enabled = false;
    }

    void ToInteract()
    {
        hud.SetActive(true);
        vMap.GenerateDatagrams();
        //vMap.GetComponent<MeshRenderer>().enabled = false;
        //vMap.GetComponent<MeshCollider>().enabled = false;
        //GetComponent<Gestures>().SwitchListeners();
        viewer.CreateCamera(vMap);
        state = State.Interaction;
        GetComponent<Gestures>().GesturesFor(State.Interaction);
    }

    void DoneInteract()
    {
        //Destroy(viewer.cpy.gameObject);
        vMap.DeleteAllDatagrams();
        //vMap.GetComponent<LineRenderer>().enabled = true;
        hud.SetActive(false);
        //GetComponent<Gestures>().SwitchListeners();
    }
}
