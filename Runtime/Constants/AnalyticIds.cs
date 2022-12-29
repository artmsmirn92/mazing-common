using System.Linq;
using mazing.common.Runtime.Enums;

namespace mazing.common.Runtime.Constants
{
    public static class AnalyticIds
    {
        #region analytics
        
        public const string SessionStart      = "session_start";
        public const string LevelReadyToStart = "level_ready_to_start";
        public const string LevelStarted      = "level_started";
        public const string LevelFinished     = "level_finished";
        
        public const string Purchase            = "purchase";
        public const string AchievementUnlocked = "achievement_unlocked";

        public const string EnableMusicButtonPressed    = "enable_music_button_pressed";
        public const string DisableMusicButtonPressed   = "disable_music_button_pressed";
        public const string EnableSoundButtonPressed    = "enable_sound_button_pressed";
        public const string DisableSoundButtonPressed   = "disable_sound_button_pressed";
        public const string EnableHapticsButtonPressed  = "enable_sound_button_pressed";
        public const string DisableHapticsButtonPressed = "disable_sound_button_pressed";
        
        public const string AdShown        = "ad_shown";
        public const string AdClicked      = "ad_clicked";
        public const string AdReward       = "ad_reward";
        public const string AdClosed       = "ad_closed";
        public const string AdFailedToShow = "ad_failed_to_show";
        

        public static string GetLanguageChangedAnalyticId(ELanguage _Language)
        {
            return $"lang_changed_to_{_Language}";
        }

        public static string GetLevelFinishedAnalyticId(long _LevelIndex)
        {
            var validLevelsForAnalytic = new long[]
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9,
                10, 20, 30, 40, 50, 60, 70, 80, 90,
                100, 200, 300, 400, 500, 600, 700, 800, 900, 1000
            };
            string anId = validLevelsForAnalytic.Contains(_LevelIndex) ?
                $"level_{_LevelIndex}_finished" : null;
            return anId;
        }

        #endregion

        #region parameters

        public const string Parameter1ForTestAnalytic  = "test_analytic_parameter_1";
        public const string ParameterLevelIndex        = "level_index";
        public const string ParameterLevelType         = "level_type";
        public const string ParameterMoneyCount        = "money_count";
        public const string ParameterDiesCount         = "dies_count";
        public const string ParameterLevelTime         = "level_time";
        public const string ParameterAchievementId     = "achievement_id";
        public const string ParameterPurchaseProductId = "purchase_product_id";
        public const string ParameterPrice             = "price";
        public const string ParameterCurrency          = "currency";
        public const string ParameterAdSource          = "ad_source";
        public const string ParameterAdType            = "ad_type";


        #endregion
    }
}