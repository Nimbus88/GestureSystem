using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;


/** UserClass
 *  Author: Harrison Pham
 *  
 *  Main class to hold user data.
 * 
 */ 
[RequireComponent(typeof(UserReaction))]
public class UserClass : MonoBehaviour
{
    // Variables
    public SMap spatial;
    public GameObject CObjects;
	public List<GameObject> corners;
    public bool doneAdjusting;
    public TagMarker marker;

    private UserReaction _react;

    // Properties
    public UserReaction Reaction {
        get { return this._react; }
    }

    void Start ()
    {
        if (!spatial) { Debug.LogError("Null SMap"); }
		corners = new List<GameObject> ();
        _react = GetComponent<UserReaction>();
        doneAdjusting = false;
    }

    void Update ()
    {

    }

}
