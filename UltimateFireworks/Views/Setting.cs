using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using UltimateFireworks.Configuration;

namespace UltimateFireworks.Views
{
    internal class Setting : PersistentSingleton<Setting>, INotifyPropertyChanged
    {
        // For this method of setting the ResourceName, this class must be the first class in the file.
        public string ResourceName => string.Join(".", GetType().Namespace, GetType().Name);

        [UIValue("modes")]
        List<object> _modes = new List<object>() { "InSide", "Normal" };

        /// <summary>説明 を取得、設定</summary>
        private string currentMode_;
        /// <summary>説明 を取得、設定</summary>
        [UIValue("current-mode")]
        public string CurrentMode
        {
            get => this.currentMode_;

            set
            {
                this.currentMode_ = value;
                this.NotifyPropertyChanged();
            }
        }

        [UIValue("scale-value")]
        public int ScaleValue
        {
            get => PluginConfig.Instance.Scale;
            set => PluginConfig.Instance.Scale = value;
        }

        [UIValue("gravity-value")]
        public float GravityValue
        {
            get => PluginConfig.Instance.GravityModifierMultiplier;
            set => PluginConfig.Instance.GravityModifierMultiplier = value;
        }

        void Awake()
        {
            switch (PluginConfig.Instance.Mode) {
                case PluginConfig.FireWorksMode.Normal:
                    this.CurrentMode = this._modes[1].ToString();
                    break;
                case PluginConfig.FireWorksMode.InSide:
                    this.CurrentMode = this._modes[0].ToString();
                    break;
                default:
                    this.CurrentMode = this._modes[0].ToString();
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void NotifyPropertyChanged([CallerMemberName]string name = null)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(name));
        }

        void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.CurrentMode)) {
                if (this.CurrentMode == this._modes[0].ToString()) {
                    PluginConfig.Instance.Mode = PluginConfig.FireWorksMode.InSide;
                }
                else {
                    PluginConfig.Instance.Mode = PluginConfig.FireWorksMode.Normal;
                }
            }
            this.PropertyChanged?.Invoke(this, e);
        }
    }
}
