using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Neat.Tools
{
    public static class UserEventExtensions
    {
        public static void InvokeUserEvent(this object o, object key)
        {
            if (o.HasUserEventEntry())
                UserEvent.entries[o].InvokeEvent(key);
        }
        public static void InvokeUserEvent(this object o, object key, object sender)
        {
            if (o.HasUserEventEntry())
                UserEvent.entries[o].InvokeEvent(key, sender);
        }
        public static void InvokeUserEvent(this object o, object key, object sender, object receiver)
        {
            if (o.HasUserEventEntry())
                UserEvent.entries[o].InvokeEvent(key, sender, receiver);
        }

        public static void SetUserListener(this object o, object key, UnityAction value)
        {
            UserEvent e = o.GetOrAddEventEntry();
            e.SetListener(key, value);
        }
        public static void SetUserListener(this object o, object key, UnityAction<object> value)
        {
            UserEvent e = o.GetOrAddEventEntry();
            e.SetListener(key, value);
        }
        public static void SetUserListener(this object o, object key, UnityAction<object, object> value)
        {
            UserEvent e = o.GetOrAddEventEntry();
            e.SetListener(key, value);
        }

        public static void RemoveUserEntry(this object o)
        {
            UserEvent.RemoveEntry(o);
        }
        public static void RemoveUserListener(this object o, object key)
        {
            if (o.HasUserEventEntry())
            {
                var e = UserEvent.entries[o];
                e.RemoveListener(key);
            }
        }

        private static UserEvent GetOrAddEventEntry(this object o)
        {
            if (UserEvent.entries == null)
                UserEvent.entries = new Dictionary<object, UserEvent>();

            if (!UserEvent.entries.ContainsKey(o))
                UserEvent.entries[o] = new UserEvent();

            return UserEvent.entries[o];
        }
        private static bool HasUserEventEntry(this object o)
        {
            return (UserEvent.entries != null && UserEvent.entries.ContainsKey(o));
        }
    }

    public class UserEvent
    {
        public static Dictionary<object, UserEvent> entries;

        // INSTANCED VERSION

        private Dictionary<object, UnityEvent> e0;
        private Dictionary<object, UnityEvent<object>> e1;
        private Dictionary<object, UnityEvent<object, object>> e2;

        public void InvokeEvent(object key)
        {
            bool success = false;
            if (e0 != null && e0.ContainsKey(key))
            {
                e0[key].Invoke();
                success = true;
            }

            if (!success)
                InvokeError(key, 0);
        }
        public void InvokeEvent(object key, object sender)
        {
            bool success = false;
            if (e1 != null && e1.ContainsKey(key))
            {
                e1[key].Invoke(sender);
                success = true;
            }

            if (!success)
                InvokeError(key, 2);
        }
        public void InvokeEvent(object key, object sender, object receiver)
        {
            bool success = false;
            if (e2 != null && e2.ContainsKey(key))
            {
                e2[key].Invoke(sender, receiver);
                success = true;
            }

            if (!success)
                InvokeError(key, 2);
        }

        public void SetListener(object key, UnityAction value)
        {
            UnityEvent e = GetOrAddE0(key);
            e.AddListener(value);
        }
        public void SetListener(object key, UnityAction<object> value)
        {
            UnityEvent<object> e = GetOrAddE1(key);
            e.AddListener(value);
        }
        public void SetListener(object key, UnityAction<object, object> value)
        {
            UnityEvent<object, object> e = GetOrAddE2(key);
            e.AddListener(value);
        }

        public void RemoveListener(object key)
        {
            if (e0 != null && e0.ContainsKey(key))
                e0.Remove(key);

            if (e1 != null && e1.ContainsKey(key))
                e1.Remove(key);

            if (e2 != null && e2.ContainsKey(key))
                e2.Remove(key);
        }
        public static void RemoveEntry(object o)
        {
            if (entries != null && UserEvent.entries.ContainsKey(o))
            {
                entries.Remove(o);
            }
        }
        private static void InvokeError(object key, int numParams)
        {
            Debug.LogWarning("Tried to invoke event with key \"" + key.ToString()
                + "\" but found no event with " + numParams.ToString() + " matching parameter(s)");
        }

        private UnityEvent GetOrAddE0(object key)
        {
            if (e0 == null)
                e0 = new Dictionary<object, UnityEvent>();

            if (e0.ContainsKey(key))
                return e0[key];
            else
                return e0[key] = new UnityEvent();
        }
        private UnityEvent<object> GetOrAddE1(object key)
        {
            if (e1 == null)
                e1 = new Dictionary<object, UnityEvent<object>>();

            if (e1.ContainsKey(key))
                return e1[key];
            else
                return e1[key] = new UnityEvent<object>();
        }
        private UnityEvent<object, object> GetOrAddE2(object key)
        {
            if (e2 == null)
                e2 = new Dictionary<object, UnityEvent<object, object>>();

            if (e2.ContainsKey(key))
                return e2[key];
            else
                return e2[key] = new UnityEvent<object, object>();
        }

        // STATIC VERSION

        private static Dictionary<object, UnityEvent> se0;
        private static Dictionary<object, UnityEvent<object>> se1;
        private static Dictionary<object, UnityEvent<object, object>> se2;



        
        

        private static UnityEvent GetOrAddSE0(object key)
        {
            if (se0 == null)
                se0 = new Dictionary<object, UnityEvent>();

            if (se0.ContainsKey(key))
                return se0[key];
            else
                return se0[key] = new UnityEvent();
        }
        private static UnityEvent<object> GetOrAddSE1(object key)
        {
            if (se1 == null)
                se1 = new Dictionary<object, UnityEvent<object>>();

            if (se1.ContainsKey(key))
                return se1[key];
            else
                return se1[key] = new UnityEvent<object>();
        }
        private static UnityEvent<object, object> GetOrAddSE2(object key)
        {
            if (se2 == null)
                se2 = new Dictionary<object, UnityEvent<object, object>>();

            if (se2.ContainsKey(key))
                return se2[key];
            else
                return se2[key] = new UnityEvent<object, object>();
        }

        private static void _InvokeStaticEvent(object key, bool debug = false)
        {
            bool success = false;
            if (se0 != null && se0.ContainsKey(key))
            {
                se0[key].Invoke();
                success = true;
            }

            if (!success && debug)
                InvokeError(key, 0);
        }
        private static void _InvokeStaticEvent(object key, object sender, bool debug = false)
        {
            bool success = false;
            _InvokeStaticEvent(key, debug);
            if (se1 != null && se1.ContainsKey(key))
            {
                se1[key].Invoke(sender);
                success = true;
            }

            if (!success && debug)
                InvokeError(key, 1);
        }
        public static void InvokeStaticEvent(object key, object sender = null, object receiver = null, bool debug = false)
        {
            bool success = false;
            _InvokeStaticEvent(key, sender, debug);
            if (se2 != null && se2.ContainsKey(key))
            {
                se2[key].Invoke(sender, receiver);
                success = true;
            }

            if (!success && debug)
                InvokeError(key, 2);
        }
        public static void SetStaticListener(object key, UnityAction value)
        {
            UnityEvent e = GetOrAddSE0(key);
            e.AddListener(value);
        }
        public static void SetStaticListener(object key, UnityAction<object> value)
        {
            UnityEvent<object> e = GetOrAddSE1(key);
            e.AddListener(value);
        }
        public static void SetStaticListener(object key, UnityAction<object, object> value)
        {
            UnityEvent<object, object> e = GetOrAddSE2(key);
            e.AddListener(value);
        }

        public static void RemoveStaticListener(object key, UnityAction value)
        {
            try
            {
                se0[key].RemoveListener(value);
            }
            catch { }
        }
        public static void RemoveStaticListener(object key, UnityAction<object> value)
        {
            try
            {
                se1[key].RemoveListener(value);
            }
            catch { }
        }
        public static void RemoveStaticListener(object key, UnityAction<object, object> value)
        {
            try
            {
                se2[key].RemoveListener(value);
            }
            catch { }
        }


        // GENERIC TESTS

        public static void SetStaticListenerGeneric<T1, T2>(object key, UnityAction<T1, T2> value)
        {
            // if no dictionary with value types exist
            if (typePairToDictionary == null)
                typePairToDictionary = new Dictionary<System.Type, object>();

            // validate type dictionary
            System.Type t = typeof(UserEventType<T1, T2>);
            if (!typePairToDictionary.ContainsKey(t))
                typePairToDictionary[t] = new Dictionary<object, UnityEvent<T1, T2>>();

            // validate event
            var adict = (Dictionary<object, UnityEvent<T1, T2>>)typePairToDictionary[t];
            if (!adict.ContainsKey(key))
                adict[key] = new UnityEvent<T1, T2>();

            // add action
            adict[key].AddListener(value);
        }
        public static void SetStaticListenerGeneric<T>(object key, UnityAction<T> value)
        {
            // if no dictionary with value types exist
            if (typePairToDictionary == null)
                typePairToDictionary = new Dictionary<System.Type, object>();

            // validate type dictionary
            System.Type t = typeof(UserEventType<T>);
            if (!typePairToDictionary.ContainsKey(t))
                typePairToDictionary[t] = new Dictionary<object, UnityEvent<T>>();

            // validate event
            var adict = (Dictionary<object, UnityEvent<T>>)typePairToDictionary[t];
            if (!adict.ContainsKey(key))
                adict[key] = new UnityEvent<T>();

            // add action
            adict[key].AddListener(value);
        }
        public static void SetStaticListenerGeneric(object key, UnityAction value)
        {
            // if no dictionary with value types exist
            if (typePairToDictionary == null)
                typePairToDictionary = new Dictionary<System.Type, object>();

            // validate type dictionary
            System.Type t = typeof(EventType);
            if (!typePairToDictionary.ContainsKey(t))
                typePairToDictionary[t] = new Dictionary<object, UnityEvent>();

            // validate event
            var adict = (Dictionary<object, UnityEvent>)typePairToDictionary[t];
            if (!adict.ContainsKey(key))
                adict[key] = new UnityEvent();

            // add action
            adict[key].AddListener(value);
        }

        public static void InvokeStaticListenerGeneric<T1, T2>(object key, T1 sender, T2 receiver)
        {
            var type = typeof(UserEventType<T1, T2>);
            if (typePairToDictionary.ContainsKey(type))
            {
                var adict = (Dictionary<object, UnityEvent<T1, T2>>)typePairToDictionary[type];
                if (adict.ContainsKey(key))
                {
                    adict[key].Invoke(sender, receiver);
                }
            }

            type = typeof(UserEventType<T1>);
            if (typePairToDictionary.ContainsKey(type))
            {
                var adict = (Dictionary<object, UnityEvent<T1>>)typePairToDictionary[type];
                if (adict.ContainsKey(key))
                {
                    adict[key].Invoke(sender);
                }
            }

            type = typeof(UserEventType);
            if (typePairToDictionary.ContainsKey(type))
            {
                var adict = (Dictionary<object, UnityEvent>)typePairToDictionary[type];
                if (adict.ContainsKey(key))
                {
                    adict[key].Invoke();
                }
            }
        }
        public static void InvokeStaticListenerGeneric<T1>(object key, T1 sender)
        {
            var type = typeof(UserEventType<T1>);
            if (typePairToDictionary.ContainsKey(type))
            {
                var adict = (Dictionary<object, UnityEvent<T1>>)typePairToDictionary[type];
                if (adict.ContainsKey(key))
                {
                    adict[key].Invoke(sender);
                }
            }

            type = typeof(UserEventType);
            if (typePairToDictionary.ContainsKey(type))
            {
                var adict = (Dictionary<object, UnityEvent>)typePairToDictionary[type];
                if (adict.ContainsKey(key))
                {
                    adict[key].Invoke();
                }
            }
        }
        public static void InvokeStaticListenerGeneric(object key)
        {
            var type = typeof(UserEventType);
            if (typePairToDictionary.ContainsKey(type))
            {
                var adict = (Dictionary<object, UnityEvent>)typePairToDictionary[type];
                if (adict.ContainsKey(key))
                {
                    adict[key].Invoke();
                }
            }
        }
        private class UserEventType<T1, T2> { }
        private class UserEventType<T1> { }
        private class UserEventType { }
        private static Dictionary<System.Type, object> typePairToDictionary;
    }
}