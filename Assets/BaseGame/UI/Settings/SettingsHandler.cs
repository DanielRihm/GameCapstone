using Assets.BaseGame.Engine.Data;
using LCPS.SlipForge.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LCPS.SlipForge.UI
{
    public class SettingsHandler : MonoBehaviour
    {
        public static event Action OnSettingsOpened;

        [SerializeField] private GameObject MenuObject;
        [SerializeField] private Slider MasterVolume;
        [SerializeField] private Slider MusicVolume;
        [SerializeField] private Slider SFXVolume;
        [SerializeField] private TMP_Dropdown WindowMode;
        [SerializeField] private TMP_Dropdown ResolutionMode;
        [SerializeField] private GameObject CustomResolution;
        [SerializeField] private Button ApplyButton;
        [SerializeField] private Button RevertButton;

        private IMenu _settingsMenu;
        private TMP_InputField _customWidth;
        private TMP_InputField _customHeight;
        private SettingsOptions _settingsOptions;

        private readonly SortedList<WindowEnum, string> _windowModeOptions = new() {
            { WindowEnum.FULLSCREEN, "Fullscreen" },
            { WindowEnum.BORDERLESS, "Windowed Borderless" },
            { WindowEnum.WINDOWED, "Windowed" }
        };

        public static void OpenSettings()
        {
            OnSettingsOpened?.Invoke();
        }

        private void ApplyWindowMode(TMP_Dropdown windowDropdown)
        {
            Debug.Log("Window Mode: " + windowDropdown.options[windowDropdown.value].text);
            Debug.Log("Window Mode Value: " + windowDropdown.value);

            switch ((WindowEnum)windowDropdown.value)
            {
                case WindowEnum.FULLSCREEN:
                    // if resolution is set to custom, set to borderless
                    if (ResolutionMode.value == ResolutionMode.options.Count - 1)
                    {
                        windowDropdown.value = (int)WindowEnum.BORDERLESS;
                        ApplyWindowMode(windowDropdown);
                        return;
                    }
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case WindowEnum.BORDERLESS:
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    break;
                case WindowEnum.WINDOWED:
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    break;
                default:
                    Debug.LogWarning("Invalid Window Mode");
                    break;
            }
        }

        private void ApplyResolution(TMP_Dropdown resolutionDropdown)
        {
            Debug.Log("Resolution: " + resolutionDropdown.options[resolutionDropdown.value].text);
            Debug.Log("Resolution Value: " + resolutionDropdown.value);

            if (resolutionDropdown.value == resolutionDropdown.options.Count - 1)
            {
                EnableCustomResolution();
            }
            else
            {
                // set resolution
                (int width, int height) = ResolutionUtils.GetResolutionFromString(resolutionDropdown.options[resolutionDropdown.value].text);
                Screen.SetResolution(width, height, Screen.fullScreenMode);
                CustomResolution.SetActive(false);
            }
        }

        private void EnableCustomResolution()
        {
            // custom resolution
            Debug.Log("Custom Resolution");

            // if game is in exclusive fullscreen, set to windowed borderless
            // exclusive fullscreen does not support custom resolutions
            if (WindowMode.value == (int)FullScreenMode.ExclusiveFullScreen)
            {
                WindowMode.value = (int)WindowEnum.BORDERLESS;
                ApplyWindowMode(WindowMode);
            }

            CustomResolution.SetActive(true);
        }

        private void ApplyCustomResolution()
        {
            if (!CustomResolution.activeSelf) return;
            if (int.TryParse(_customWidth.text, out int width) && int.TryParse(_customHeight.text, out int height))
            {
                Screen.SetResolution(width, height, Screen.fullScreenMode);
            }
            else
            {
                Debug.LogWarning("Invalid custom resolution");
            }
        }

        private void SetResolutionSettings()
        {
            int resolutionIndex = ResolutionMode.options.FindIndex(res => res.text == _settingsOptions.Resolution.str);
            if (resolutionIndex == -1)
            {
                // resolution not found, set to custom
                ResolutionMode.value = ResolutionMode.options.Count - 1;
                CustomResolution.SetActive(true);
                _customWidth.text = _settingsOptions.Resolution.width.ToString();
                _customHeight.text = _settingsOptions.Resolution.height.ToString();
            }
            else
            {
                ResolutionMode.value = resolutionIndex;
            }
        }

        private void SetSavedSettings()
        {
            MasterVolume.value = _settingsOptions.MasterVolume;
            MusicVolume.value = _settingsOptions.MusicVolume;
            SFXVolume.value = _settingsOptions.SFXVolume;
            WindowMode.value = (int)_settingsOptions.WindowMode;
            SetResolutionSettings();

            // apply settings
            ApplyResolution(ResolutionMode);
            ApplyWindowMode(WindowMode);
            SoundManager.Instance.SetMasterVolume(MasterVolume.value);
            SoundManager.Instance.SetMusicVolume(MusicVolume.value);
            SoundManager.Instance.SetSFXVolume(SFXVolume.value);
        }

        private void SaveCurrentSettings()
        {
            _settingsOptions.MasterVolume = MasterVolume.value;
            _settingsOptions.MusicVolume = MusicVolume.value;
            _settingsOptions.SFXVolume = SFXVolume.value;
            _settingsOptions.WindowMode = (WindowEnum)WindowMode.value;
            _settingsOptions.Resolution = (ResolutionUtils.GetResolutionStringNoRefresh(Screen.currentResolution), Screen.currentResolution.width, Screen.currentResolution.height);

            PlayerPreferences.SaveSettings(_settingsOptions);
        }

        private void SetUpWindowMode()
        {
            // clear window dropdown options
            WindowMode.ClearOptions();
            // create list of strings indexed by WindowEnum and add to dropdown
            List<string> windowModeOptions = _windowModeOptions.Values.ToList();
            WindowMode.AddOptions(windowModeOptions);
        }

        private void SetUpResolutionMode()
        {
            // clear resolution dropdown options
            ResolutionMode.ClearOptions();
            // add resolutions to dropdown
            ResolutionMode.AddOptions(ResolutionUtils.GetResolutionList());
            ResolutionMode.options.Add(new TMP_Dropdown.OptionData("Custom"));
        }

        private void Start()
        {
            _settingsMenu = MenuObject.GetComponent<IMenu>();
            OnSettingsOpened += () => { _settingsMenu.Open(true); };
            CustomResolution.SetActive(false);

            // --------------------- DROPDOWN SETUP ---------------------
            SetUpWindowMode();
            SetUpResolutionMode();

            // --------------------- CUSTOM RESOLUTION SETUP ---------------------
            _customWidth = CustomResolution.transform.Find("ResolutionWidth").GetComponent<TMP_InputField>();
            _customHeight = CustomResolution.transform.Find("ResolutionHeight").GetComponent<TMP_InputField>();

            // --------------------- APPLY INITIAL VALUES ---------------------
            _settingsOptions = new SettingsOptions();
            _settingsOptions.Initialize();
            _settingsOptions = PlayerPreferences.LoadSettings(_settingsOptions);
            SetSavedSettings();

            // --------------------- ADD LISTENERS ---------------------
            MasterVolume.onValueChanged.AddListener(SoundManager.Instance.SetMasterVolume);
            MusicVolume.onValueChanged.AddListener(SoundManager.Instance.SetMusicVolume);
            SFXVolume.onValueChanged.AddListener(SoundManager.Instance.SetSFXVolume);
            WindowMode.onValueChanged.AddListener(delegate { ApplyWindowMode(WindowMode); });
            ResolutionMode.onValueChanged.AddListener(delegate { ApplyResolution(ResolutionMode); });
            _customWidth.onSubmit.AddListener(delegate { ApplyCustomResolution(); });
            _customHeight.onSubmit.AddListener(delegate { ApplyCustomResolution(); });
            ApplyButton.onClick.AddListener(() => { SaveCurrentSettings(); });
            RevertButton.onClick.AddListener(() => { SetSavedSettings(); });
        }

        private void OnDestroy()
        {
            OnSettingsOpened -= () => { _settingsMenu.Open(true); };
        }
    }
}
