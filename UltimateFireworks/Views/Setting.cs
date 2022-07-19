using BeatSaberMarkupLanguage.Attributes;
using HMUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UltimateFireworks.Configuration;
using UltimateFireworks.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UltimateFireworks.Views
{
    internal class Setting : PersistentSingleton<Setting>, INotifyPropertyChanged
    {
        // For this method of setting the ResourceName, this class must be the first class in the file.
        public string ResourceName => string.Join(".", this.GetType().Namespace, this.GetType().Name);

        [UIValue("modes")]
        private readonly List<object> _modes = new List<object>() { "InSide", "Normal" };

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
        [UIValue("fire-enable")]
        public bool Fire
        {
            get => PluginConfig.Instance.FireEnable;
            set => PluginConfig.Instance.FireEnable = value;
        }
        [UIValue("traile-enable")]
        public bool Traile
        {
            get => PluginConfig.Instance.TraileEnable;
            set => PluginConfig.Instance.TraileEnable = value;
        }
        [UIValue("refrect-enable")]
        public bool Refrect
        {
            get => PluginConfig.Instance.Refrect;
            set => PluginConfig.Instance.Refrect = value;
        }
        [UIValue("radial")]
        public float Radial
        {
            get => PluginConfig.Instance.Radial;
            set => PluginConfig.Instance.Radial = value;
        }
        [UIValue("sound-set")]
        public string SoundSet
        {
            get => PluginConfig.Instance.SoundSet;
            set => PluginConfig.Instance.SoundSet = value;
        }

        [UIValue("sounds")]
        public List<object> Sounds { get; set; } = new List<object>() { "Default", "Kinni-kun" };
        private List<string> _sounds;

        [UIComponent("sound-dropdown")]
        private readonly object _dropDownObject;
        private SimpleTextDropdown _simpleTextDropdown;
        [Inject]
        private readonly ISoundLoader _soundLoader;

        private void Awake()
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

        [UIAction("#post-parse")]
        protected void PostParse()
        {
            this.StartCoroutine(this.CreateList());
        }

        private IEnumerator CreateList()
        {
            yield return new WaitWhile(() => this._soundLoader == null || this._soundLoader.IsLoading);
            try {
                var sounds = new List<string>
                {
                    "Default"
                };
                foreach (var item in this._soundLoader.Sounds) {
                    sounds.Add(new DirectoryInfo(item.Key).Name);
                }
                if (this._dropDownObject is LayoutElement layout) {
                    this._sounds = sounds.OrderBy(x => x).ToList();
                    this._simpleTextDropdown = layout.GetComponentsInChildren<SimpleTextDropdown>(true).FirstOrDefault();
                    this._simpleTextDropdown.SetTexts(this._sounds);
                    this._simpleTextDropdown.ReloadData();
                    this._simpleTextDropdown.didSelectCellWithIdxEvent += this.SimpleTextDropdown_didSelectCellWithIdxEvent;
                    if (string.IsNullOrEmpty(PluginConfig.Instance.SoundSet)) {
                        PluginConfig.Instance.SoundSet = this._sounds.FirstOrDefault() ?? "";
                    }
                    else if (this._sounds.Any(x => x == PluginConfig.Instance.SoundSet)) {
                        this._simpleTextDropdown.SelectCellWithIdx(this._sounds.IndexOf(PluginConfig.Instance.SoundSet));
                    }
                    else {
                        this._simpleTextDropdown.SelectCellWithIdx(0);
                    }
                }
            }
            catch (Exception e) {
                Plugin.Log.Error(e);
            }
        }

        private void SimpleTextDropdown_didSelectCellWithIdxEvent(DropdownWithTableView arg1, int arg2)
        {
            PluginConfig.Instance.SoundSet = this._sounds[arg2];
        }

        protected override void OnDestroy()
        {
            this._simpleTextDropdown.didSelectCellWithIdxEvent -= this.SimpleTextDropdown_didSelectCellWithIdxEvent;
            base.OnDestroy();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string name = null)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(name));
        }

        private void OnPropertyChanged(PropertyChangedEventArgs e)
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
