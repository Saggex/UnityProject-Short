using UnityEngine;
using UnityEngine.Events;

public class AnimationEventBridge : MonoBehaviour
{
    /*
     How to Use:
        Attach this script to the GameObject with the Animator (or any target object).
        In the Inspector, hook up the onAnimationEvent to call any function you want.
        In your Animation Clip:
            Add an AnimationEvent.
            Set the function name to TriggerEvent, TriggerEventWithString, or TriggerEventWithFloat.
            (Optional) Pass a string or float argument if needed.
     
     */

    [Tooltip("This UnityEvent will be invoked when triggered by an AnimationEvent.")]
    public UnityEvent onAnimationEvent;

    /// <summary>
    /// Call this from an AnimationEvent.
    /// </summary>
    public void TriggerEvent()
    {
        onAnimationEvent?.Invoke();
    }

    /// <summary>
    /// Call this from an AnimationEvent, passing a string parameter.
    /// </summary>
    public void TriggerEventWithString(string value)
    {
        Debug.Log($"AnimationEvent triggered with string: {value}");
        onAnimationEvent?.Invoke();
    }

    /// <summary>
    /// Call this from an AnimationEvent, passing a float parameter.
    /// </summary>
    public void TriggerEventWithFloat(float value)
    {
        Debug.Log($"AnimationEvent triggered with float: {value}");
        onAnimationEvent?.Invoke();
    }
}
