using mazing.common.Runtime.Constants;
using mazing.common.Runtime.Entities;
using mazing.common.Runtime.Managers;

namespace mazing.common.Runtime.Settings
{
    public interface IHapticsSetting : ISetting<bool> { }
    
    public class HapticsSetting : SettingBase<bool>, IHapticsSetting
    {
        private IAnalyticsManager AnalyticsManager { get; }

        public HapticsSetting(IAnalyticsManager _AnalyticsManager)
        {
            AnalyticsManager = _AnalyticsManager;
        }
        
        public override SaveKey<bool>     Key          => SaveKeysCommon.SettingHapticsOn;
        public override string            TitleKey     => "Haptics";
        public override ESettingLocation  Location     => ESettingLocation.MiniButtons;
        public override ESettingType      Type         => ESettingType.OnOff;
        public override string            SpriteOnKey  => "setting_haptics_on";
        public override string            SpriteOffKey => "setting_haptics_off";

        public override void Put(bool _Value)
        {
            AnalyticsManager.SendAnalytic(_Value ? 
                AnalyticIds.EnableHapticsButtonPressed : AnalyticIds.DisableHapticsButtonPressed);
            base.Put(_Value);
        }
    }
}