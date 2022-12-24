using mazing.common.Runtime.Constants;
using mazing.common.Runtime.Entities;
using mazing.common.Runtime.Managers;

namespace mazing.common.Runtime.Settings
{
    public interface IMusicSetting : ISetting<bool> { }
    
    public class MusicSetting : SettingBase<bool>, IMusicSetting
    {
        private IAnalyticsManager AnalyticsManager { get; }

        public MusicSetting(IAnalyticsManager _AnalyticsManager)
        {
            AnalyticsManager = _AnalyticsManager;
        }
        
        public override SaveKey<bool>     Key          => SaveKeysCommon.SettingNotificationsOn;
        public override string            TitleKey     => "Music";
        public override ESettingLocation  Location     => ESettingLocation.MiniButtons;
        public override ESettingType      Type         => ESettingType.OnOff;
        public override string            SpriteOnKey  => "setting_music_on";
        public override string            SpriteOffKey => "setting_music_off";

        public override void Put(bool _Value)
        {
            AnalyticsManager.SendAnalytic(_Value ? 
                AnalyticIds.EnableMusicButtonPressed : AnalyticIds.DisableMusicButtonPressed);
            base.Put(_Value);
        }
    }
}