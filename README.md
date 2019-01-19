# ScriptablePlayerController
StateMachine based on Scriptable Objects and a custom behavior graph editor. It is meant to be reusable and extendable. It is meant to be used with a Scriptable Object variable library so that the singleton anti-pattern is avoided in the game.


## HOW TO

The Main script to be added to the GameObject (PlayerController or (TypeOf)EnemyController is meant to inherit from the StateManager script. Add your main script along with the ActionHook script to the main Game Object. There is an example set up in the Data folder for the scriptable objects.

Attach the scripts and create / drag and drop the scriptable objects onto the PlayerController or (TypeOf)EnemyController and action hook. Drag the first state the object will be starting in into the CurrentState slot.

The Action Hook takes the input manager, and the input manager contains the inputs the player will use. the ActionHook is meant for anything that needs to be updated at all times (such as Inputs).

You can extend it by coding the StateActions and Conditions to switch States needed for your player / AI. Here you can make locomotions, animation implementations, player behavior based on camera (rotations etc..), any conditions such as, has the AI arrived to a destination? or can the player still attack?.

To use these, create a script and add the name space StateMachine, then inherit from StateActions, or Conditions, (or Actions for the ActionHook). Then code the behavior you want and then add a context menu to be able to right click on the project and create the scriptable object.

To use the behavior editor simply open the menu window undet BehaviorEditor at the top. Right click on the project and create a new Graph. Then, on the top left tab of the new window, add the graph you want to use. Here you can right click and create states. and right click in the states to create conditions, then right click in the conditions to attach them to other states. you can add the states, conditions, and state actions to all the slots in the boxes as desired. to attach a condition to a duplicate state, right click and create a Portal, then add the state desired.
