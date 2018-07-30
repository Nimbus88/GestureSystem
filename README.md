# Sample Gestures System
A "point 'n click" gestures system, utilizing the Microsoft HoloLens, I created for my senior project in university. 

## Disclaimer
This repository is not meant to be used directly. It mainly serves as a resource of a possible approach to a gesture system.

## Reasoning for Gesture System
When following [Microsoft's Mixed Reality tutorials](https://docs.microsoft.com/en-us/windows/mixed-reality/academy), the implementation seemed "clunky" to me (Note: this was around October 2017. Microsoft's implementation has updated since, however, the video guides are still the same). Since I was with a team for this project, I didn't want them to run into many issues when implementing their own gesture features. To minimize this, I had to come up with a system where they can easily specify what gesture(s) they want an object to "react" to.

## Ideology
How I envisioned the gestures to work was a user just needed to look at a hologram, do a gesture, and the hologram "reacts" to that gesture. In other words, I needed to know what a hologram is, and each hologram has a different reaction depending on the gesture. Our application was also broken into states; meaning gestures in state A can mean differently in state B. 

To detect if a user is looking at a hologram, I use raycasting from the user's view, and see if the ray hits an object that is a hologram, or in this case an IObject (Interatable Object).

For the "reactions" of an IObject, I created several interfaces corresponding to all possible state-gesture combinations (eg. CalibrationTap could be one of the interfaces), and IObjects can interface with one or many of them. I did this because I didn't like seeing functions with big If-statements, and I didn't want to be forced to specify unused state-gesture combinations. This also allowed my team and I to not worry about how a gesture or IObject will be detected, and focus on what the IObject should do. The biggest drawback to this design would be some refactoring if more states were to be added.

## The Scripts
### <u>Main Scripts</u>
Gestures - Listeners for gesture detection. Tells IObjects to do their reactions, or the user if none are detected.

IObject - Has a static function to find IObjects by raycasting. Handles the execution of an IObject's reaction, given the state and gesture.

Reactions - Lists all possible state-gesture interafaces.

StateMachine - Handles the transitions and conditions from state to state.

### <u>Example/Side Scripts</u>
UserClass - Representation of the user.

UserReaction - Definitons of reactions a user should do given a state and gesture (Note: many of them are empty due to us not knowing if the user should react to them).

SMap - Toggles the visualization of the spatial mapping wiremesh. Only occurs when the user is in the calibration state and does the Hold gesture.

## Tools Used
Unity - Development environment

Microsoft HoloLens - Main device for project

Visual Studio - Coding, deploying application to HoloLens, HoloLens emulator
