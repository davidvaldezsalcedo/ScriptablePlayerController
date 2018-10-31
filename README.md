# ScriptablePlayerController
Player Controller based on Scriptable Objects that uses states. It is meant to be reusable and extendable. The Scriptable Object variable library is meant to be used so that singletons are avoided.


## HOW TO

The StateManager script along with the ActionHook script should be attached to the main Game Object. There is an example set up in the Data folder for the scriptable objects.

Attach the scripts and create / drag and drop the scriptable objects onto the state manager and action hook.

The Action Hook takes the input manager, and the input manager contains the input axis.

You can extend it by coding the state actions you need for your player / AI. here you can make locomotions, animation implementations, player behavior based on camera (rotations etc..).

I have included an extendable spell casting system as well. Here you can create a condition scriptable object and under the dictionary add the input desired, the spell prefab for that input, and the type of spell, which is a scriptable object you can create as well by creating a script that inherits from SpellBase. 
