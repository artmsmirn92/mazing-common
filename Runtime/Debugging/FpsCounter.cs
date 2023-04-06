using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using mazing.common.Runtime.Entities;
using mazing.common.Runtime.Extensions;
using mazing.common.Runtime.Helpers;
using mazing.common.Runtime.Ticker;
using mazing.common.Runtime.Utils;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace mazing.common.Runtime.Debugging
{
    public readonly struct FpsCounterRecording
    {
        public FpsCounterRecording(
            float _Fps,
            float _FpsMin, 
            float _FpsMax, List<float> _FpsValues)
        {
            Fps        = _Fps;
            FpsMin     = _FpsMin;
            FpsMax     = _FpsMax;
            FpsValues  = _FpsValues;
        }

        public float       Fps       { get; }
        public float       FpsMin    { get; }
        public float       FpsMax    { get; }
        public List<float> FpsValues { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"Average Fps: {Fps}, min FPS: {FpsMin}, max FPS: {FpsMax}");
            sb.AppendLine(", values:");
            foreach (float fpsValue in FpsValues)
                sb.AppendLine(fpsValue.ToString(CultureInfo.InvariantCulture));
            return sb.ToString();
        }
    }

    public interface IFpsCounter : IInit
    {
        Entity<bool>        IsLowPerformance { get; }
        FpsCounterRecording GetRecording();
        void                Record(float _Duration);
        void                OnActiveCameraChanged(Camera _Camera);
    }

    public class FpsCounterFake : InitBase, IFpsCounter
    {
        public Entity<bool> IsLowPerformance => new Entity<bool>
        {
            Value = false,
            Result = EEntityResult.Success
        };

        public FpsCounterRecording GetRecording() => default;
        
        public void                Record(float _Duration) { }
        public void OnActiveCameraChanged(Camera _Camera) { }
    }
}