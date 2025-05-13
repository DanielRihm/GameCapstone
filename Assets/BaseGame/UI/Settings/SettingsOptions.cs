using LCPS.SlipForge.Enum;
using UnityEngine;

namespace LCPS.SlipForge.UI
{
    public struct SettingsOptions
    {
        public float MasterVolume;
        public float MusicVolume;
        public float SFXVolume;
        public WindowEnum WindowMode;
        public (string str, int width, int height) Resolution;

        public void Initialize(float masterVolume = 1f, float musicVolume = 1f, float sfxVolume = 1f, WindowEnum? windowMode = null, Resolution? resolution = null)
        {
            MasterVolume = masterVolume;
            MusicVolume = musicVolume;
            SFXVolume = sfxVolume;
            WindowMode = windowMode ?? Screen.fullScreenMode switch
            {
                FullScreenMode.ExclusiveFullScreen => WindowEnum.FULLSCREEN,
                FullScreenMode.FullScreenWindow => WindowEnum.BORDERLESS,
                FullScreenMode.Windowed => WindowEnum.WINDOWED,
                _ => WindowEnum.FULLSCREEN,
            };
            var resolutionString = ResolutionUtils.GetResolutionStringNoRefresh(resolution ?? Screen.currentResolution);
            Resolution = (resolutionString, (resolution ?? Screen.currentResolution).width, (resolution ?? Screen.currentResolution).height);
        }
    }
}

