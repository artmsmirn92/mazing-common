using mazing.common.Runtime.Entities;
using mazing.common.Runtime.Enums;

namespace mazing.common.Runtime.Managers
{
    public interface IAudioManager : IInit
    {
        bool IsPlaying(AudioClipArgs    _Args);
        void InitClip(AudioClipArgs     _Args);
        void PlayClip(AudioClipArgs     _Args);
        void PauseClip(AudioClipArgs    _Args);
        void UnpauseClip(AudioClipArgs  _Args);
        void StopClip(AudioClipArgs     _Args);
        void EnableAudio(bool           _Enable, EAudioClipType _Type);
        void MuteAudio(EAudioClipType   _Type);
        void UnmuteAudio(EAudioClipType _Type);
    }
}