using System;
using UnityEngine;
using UnityEngine.Events;
using MathUtils = mazing.common.Runtime.Utils.MathUtils;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace mazing.common.Runtime.Utils
{
    public static class CommonUtils
    {
        public const float FpsThresholdLowPerformance = 30f;
        
        public static RuntimePlatform Platform 
        { 
            get
            {
#if UNITY_ANDROID
                return RuntimePlatform.Android;
#elif UNITY_IOS || UNITY_IPHONE
                 return RuntimePlatform.IPhonePlayer;
#elif UNITY_STANDALONE_OSX
                 return RuntimePlatform.OSXPlayer;
#elif UNITY_STANDALONE_WIN
                 return RuntimePlatform.WindowsPlayer;
#elif UNITY_WEBGL
                return RuntimePlatform.WebGLPlayer;
#endif
            }
        }
        
        public static string GetOsName()
        {
            switch (Platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.WebGLPlayer:
                    return "WebGl";
                case RuntimePlatform.WindowsPlayer:
                    return "WindowsPlayer";
                case RuntimePlatform.OSXPlayer:
                    return "OSX";
                default: return null;
            }
        }
        
        public static void CopyToClipboard(string _Text)
        {
            var te = new TextEditor {text = _Text};
            te.SelectAll();
            te.Copy();
        }
        
        public static void DoOnInitializedEx<T>(T _InitObject, UnityAction _Action) where T : IInit
        {
            if (_InitObject.Initialized)
                _Action?.Invoke();
            else
                _InitObject.Initialize += _Action;
        }
        
        //https://answers.unity.com/questions/246116/how-can-i-generate-a-guid-or-a-uuid-from-within-un.html
        public static string GetUniqueId()
        {
            var epochStart = new DateTime(1970, 1, 1, 8, 0, 0, DateTimeKind.Utc);
            double timestamp = (DateTime.UtcNow - epochStart).TotalSeconds;
            string uniqueId = Application.systemLanguage                           // Language
                              + "-" + Platform                                     // Device    
                              + "-" + $"{Convert.ToInt32(timestamp):X}"            // Time
                              + "-" + $"{Convert.ToInt32(Time.time * 1000000):X}"  // Time in game
                              + "-" + $"{MathUtils.RandomGen.Next(1000000000):X}"; //random number
            return uniqueId;
        }
        
        public static GameObject FindOrCreateGameObject(string _Name, out bool _WasFound)
        {
            var go = GameObject.Find(_Name);
            _WasFound = go != null;
            if (go == null)
                go = new GameObject(_Name);
            return go;
        }
        
        public static void GetTouch(
            int         _Index,
            out int     _ID,
            out Vector2 _Position,
            out float   _Pressure,
            out bool    _Began, 
            out bool    _Ended)
        {
#if ENABLE_INPUT_SYSTEM
            var touch = Touch.activeTouches[_Index];
            _ID       = touch.finger.index;
            _Position = touch.screenPosition;
            _Pressure = touch.pressure;
            _Began    = touch.phase == UnityEngine.InputSystem.TouchPhase.Began;
            _Ended    = touch.phase == UnityEngine.InputSystem.TouchPhase.Canceled;
#else
			var touch = Input.GetTouch(_Index);
			_ID       = touch.fingerId;
			_Position = touch.position;
			_Pressure = touch.pressure;
            _Began    = touch.phase == TouchPhase.Began;
            _Ended    = touch.phase == TouchPhase.Ended;
#endif
        }

        public static int GetHash(string _String)
        {
            return Animator.StringToHash(_String);
        }
    }
}