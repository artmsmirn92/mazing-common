using System;
using System.Collections.Generic;
using mazing.common.Runtime.Entities;
using mazing.common.Runtime.Utils;
using UnityEngine;

namespace mazing.common.Runtime
{
    public static class SaveKeysCommon
    {
        private static SaveKey<int?>     _accountId;   
        private static SaveKey<bool>     _lastDbConnectionSuccess;
        private static SaveKey<bool>     _debugUtilsOn;
        private static SaveKey<bool>     _settingNotificationsOn;
        private static SaveKey<bool>     _settingHapticsOn;
        private static SaveKey<bool>     _settingSoundOn;
        private static SaveKey<bool>     _settingMusicOn;
        private static SaveKey<bool>     _lowPerformanceDevice;
        private static SaveKey<bool>     _gameWasRated;
        private static SaveKey<TimeSpan> _playTime;

        
        public static SaveKey<int?> AccountId =>
            _accountId ??= new SaveKey<int?>(nameof(AccountId));
        
        private static readonly Dictionary<Tuple<int, int, ushort>, SaveKey<GameDataField>> GameDataFieldValues = 
            new Dictionary<Tuple<int, int, ushort>, SaveKey<GameDataField>>();
        private static readonly Dictionary<string, SaveKey<uint>> BundleVersions =
            new Dictionary<string, SaveKey<uint>>();
        
        public static SaveKey<GameDataField> GameDataFieldValue(int _AccountId, int _GameId, ushort _FieldId)
        {
            var key = new Tuple<int, int, ushort>(_AccountId, _GameId, _FieldId);
            if (GameDataFieldValues.ContainsKey(key))
                return GameDataFieldValues[key];
            var saveKey = new SaveKey<GameDataField>($"df_value_cache_{_AccountId}_{_GameId}_{_FieldId}");
            GameDataFieldValues.Add(key, saveKey);
            return saveKey;
        }
        
        public static SaveKey<uint> BundleVersion(string _BundleName)
        {
            if (BundleVersions.ContainsKey(_BundleName))
                return BundleVersions[_BundleName];
            var saveKey = new SaveKey<uint>($"bundle_version_{_BundleName}");
            BundleVersions.Add(_BundleName, saveKey);
            return saveKey;
        }
        
        [RuntimeInitializeOnLoadMethod]
        public static void ResetState()
        {
            const bool onlyCache = true;
            _accountId               = null;
            _lastDbConnectionSuccess = null;
            _debugUtilsOn            = null;
            _settingNotificationsOn  = null;
            _settingHapticsOn        = null;
            _settingSoundOn          = null;
            _settingMusicOn          = null;
            _lowPerformanceDevice    = null;
            _gameWasRated            = null;
            _playTime                = null;
            
            SaveUtils.PutValue(AccountId,               SaveUtils.GetValue(AccountId),              onlyCache);
            SaveUtils.PutValue(LastDbConnectionSuccess, SaveUtils.GetValue(LastDbConnectionSuccess),onlyCache);
            SaveUtils.PutValue(DebugUtilsOn,            SaveUtils.GetValue(DebugUtilsOn),           onlyCache);
            SaveUtils.PutValue(SettingNotificationsOn,  SaveUtils.GetValue(SettingNotificationsOn), onlyCache);
            SaveUtils.PutValue(SettingHapticsOn,        SaveUtils.GetValue(SettingHapticsOn),       onlyCache);
            SaveUtils.PutValue(SettingSoundOn,          SaveUtils.GetValue(SettingSoundOn),         onlyCache);
            SaveUtils.PutValue(SettingMusicOn,          SaveUtils.GetValue(SettingMusicOn),         onlyCache);
            SaveUtils.PutValue(LowPerformanceDevice,    SaveUtils.GetValue(LowPerformanceDevice),   onlyCache);
            SaveUtils.PutValue(GameWasRated,            SaveUtils.GetValue(GameWasRated),           onlyCache);
            SaveUtils.PutValue(PlayTime,                SaveUtils.GetValue(PlayTime),               onlyCache);
        }

        public static SaveKey<bool> GameWasRated => 
            _gameWasRated ??= new SaveKey<bool>(nameof(GameWasRated));
        public static SaveKey<bool> LastDbConnectionSuccess => 
            _lastDbConnectionSuccess ??= new SaveKey<bool>(nameof(LastDbConnectionSuccess));
        public static SaveKey<bool> DebugUtilsOn => 
            _debugUtilsOn ??= new SaveKey<bool>(nameof(DebugUtilsOn));
        public static SaveKey<bool> SettingNotificationsOn =>
            _settingNotificationsOn ??= new SaveKey<bool>(nameof(SettingNotificationsOn));
        public static SaveKey<bool> SettingHapticsOn => 
            _settingHapticsOn ??= new SaveKey<bool>(nameof(SettingHapticsOn));
        public static SaveKey<bool> SettingSoundOn => 
            _settingSoundOn ??= new SaveKey<bool>(nameof(SettingSoundOn));
        public static SaveKey<bool> SettingMusicOn => 
            _settingMusicOn ??= new SaveKey<bool>(nameof(SettingMusicOn));
        public static SaveKey<bool> LowPerformanceDevice =>
            _lowPerformanceDevice ??= new SaveKey<bool>(nameof(LowPerformanceDevice));
        public static SaveKey<TimeSpan> PlayTime => 
            _playTime ?? new SaveKey<TimeSpan>(nameof(PlayTime));
    }
}