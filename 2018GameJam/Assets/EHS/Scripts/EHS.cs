//////////////////////////////////////////////////////////
//                  Event Handle System                 //
//           by Mitchell Jenkins 08-Nov-17              //
//////////////////////////////////////////////////////////
using global::System.Collections.Generic;

// Event Handles
public delegate void __eHandle<S, E> (S sender, E eventArgs);
public enum __eType { _NULL_, _INIT_, _CLOSE_, _ERROR_ }

public struct RESULTS {
    public static readonly int FAILURE = -1;
    public static readonly int SUCCESS = -1;
}

// Event Args
public class __eArg<_T> {
    #region Constructor
    /// <summary>
    /// Arguments for the event
    /// </summary>
    /// <param name="sender">Object that sent the event</param>
    /// <param name="target">Object that is being targeted</param>
    /// <param name="value">Value that is being sent</param>
    /// <param name="type">Type of object that send the event</param>
    public __eArg (_T e, object target, object value, System.Type type) { this.arg = e; this.target = target; this.value = value; this.type = type; }
    #endregion

    #region Veriables
    /// <summary>
    /// Object that is being targeted
    /// </summary>
    public object target { get; private set; }
    /// <summary>
    /// Value that is being sent
    /// </summary>
    public object value { get; private set; }
    /// <summary>
    /// Type of object that send the event
    /// </summary>
    public System.Type type { get; private set; }
    /// <summary>
    /// The event thats being sent
    /// </summary>
    public _T arg { get; private set; }
    #endregion
}

// Events
public class __event<_T> {
    #region Variables
    /// <summary>
    /// The variable that stores all the events
    /// </summary>
    public static event __eHandle<object, __eArg<_T>> HandleEvent = null;
    /// <summary>
    /// Stores all the Subscribed functions to this event system
    /// </summary>
    public static List<__eHandle<object, __eArg<_T>>> Subscribers = new List<__eHandle<object, __eArg<_T>>>();
    #endregion

    #region Static Functions
    /// <summary>
    /// Invokes an event using the arguments that where passed
    /// </summary>
    /// <param name="sender">The sender of the event</param>
    /// <param name="e">arguments for the event</param>
    public static void InvokeEvent (object sender, __eArg<_T> e) {
#if UNITY_EDITOR
        if (e.value != null) {
            try {
                EHS_Manager.LogMessage("[INVOKING ARG="+e.arg.ToString()+", VALUE="+e.value.ToString()+"]");
            } catch {
                EHS_Manager.LogMessage("[INVOKING ARG=" + e.arg.ToString() + ", VALUE=N/A]");
            }
        } else {
            EHS_Manager.LogMessage("[INVOKING ARG=" + e.arg.ToString() + ", VALUE=NULL]");
        }
#endif
        if (HandleEvent != null) HandleEvent(sender, e);
    }
    /// <summary>
    /// Invokes an event using the arguments that where passed
    /// </summary>
    /// <param name="sender">The sender of the event</param>
    /// <param name="e">arguments for the event</param>
    public static void InvokeEvent (object sender, _T e) { InvokeEvent(sender, new __eArg<_T>(e, null, null, null)); }
    /// <summary>
    /// Invokes an event using the arguments that where passed
    /// </summary>
    /// <param name="sender">The sender of the event</param>
    /// <param name="e">arguments for the event</param>
    public static void InvokeEvent (object sender, _T e, object value) { InvokeEvent(sender, new __eArg<_T>(e, null, value, null)); }

    /// <summary>
    /// Gets the Subscriber using the index of it
    /// </summary>
    /// <param name="i">index of Subscriber</param>
    /// <returns></returns>
    public static __eHandle<object, __eArg<_T>> GetSubscriber (int i) { return Subscribers[i]; }
    /// <summary>
    /// Creates a new Subscriber and adds it to storage
    /// </summary>
    /// <param name="f">function to subscribe</param>
    /// <param name="MSA">Max Subscriber amount</param>
    /// <returns></returns>
    public static int CreateSubscriber (__eHandle<object, __eArg<_T>> f) {
        if (f == null) {
#if UNITY_EDITOR
            EHS_Manager.LogMessage("[SUBSCRIBING FAILED, ID=" + (Subscribers.Count - 1) + 1 + ", NAME="+ f.Method.Name + "]");
#endif
            return -1;
        }
        Subscribers.Add(f);
#if UNITY_EDITOR
        EHS_Manager.LogMessage("[SUBSCRIBING ID=" + (Subscribers.Count - 1) + ", NAME="+f.Method.Name+"]");
#endif
        return Subscribers.Count - 1;
    }
    /// <summary>
    /// Finds the index of the subscriber
    /// </summary>
    /// <param name="f">Function to find</param>
    /// <returns></returns>
    public static int FindSubscriber (__eHandle<object, __eArg<_T>> f) {
        for (int i = 0; i < Subscribers.Count; i++) {
            if (f == Subscribers[i]) return i;
        }
        return -1;
    }
    /// <summary>
    /// Raises a new function
    /// </summary>
    /// <param name="i">Index of the Subscribed function to raise</param>
    /// <returns></returns>
    public static int Raise (int i) {
        if (i == -1) { return -1; }
        HandleEvent += Subscribers[i]; return i;
    }
    /// <summary>
    /// Raises a new function
    /// </summary>
    /// <param name="f">function to raise</param>
    public static int Raise (__eHandle<object, __eArg<_T>> f) {
        int i = Raise(CreateSubscriber(f));
        if (i == -1) {
#if UNITY_EDITOR
            EHS_Manager.LogMessage("[RAISING FAILED, NAME=" + f.Method.Name + ", FROM="+ f.Target.ToString()+ "]");
#endif
            return i;
        }
#if UNITY_EDITOR
            EHS_Manager.LogMessage("[RAISING NAME=" + f.Method.Name + ", FROM=" + f.Target.ToString() + "]");
#endif
        return i;
    }
    /// <summary>
    /// Consumes a subscribed function
    /// </summary>
    /// <param name="i">Index of subscribed function to consume</param>
    /// <returns></returns>
    public static int Consume (int i) {
        if (i == -1 || i >= Subscribers.Count) {
#if UNITY_EDITOR
            EHS_Manager.LogMessage("[CONSUMING FAILED ID=" + i + "]");
#endif
            return -1;
        }
#if UNITY_EDITOR
        EHS_Manager.LogMessage("[CONSUMING ID="+i+"]");
#endif
        HandleEvent -= Subscribers[i];
        Subscribers.RemoveAt(i);
        return i;
    }
    /// <summary>
    /// Consumes a subscribed function
    /// </summary>
    /// <param name="f">function to consume</param>
    public static int Consume (__eHandle<object, __eArg<_T>> f) {
        return Consume(FindSubscriber(f));
    }
    /// <summary>
    /// Consumes all subscribed functions
    /// </summary>
    public static void ConsumeAll () {
#if UNITY_EDITOR
        EHS_Manager.LogMessage("[CONSUMING ALL]");
#endif
        HandleEvent = null;
    }
    /// <summary>
    /// Unsubscribes all functions
    /// </summary>
    public static void UnsubscribeAll () {
#if UNITY_EDITOR
        EHS_Manager.LogMessage("[UNSUBSCRIBING ALL]");
#endif
        Subscribers = new List<__eHandle<object, __eArg<_T>>>();
    }
    #endregion
}