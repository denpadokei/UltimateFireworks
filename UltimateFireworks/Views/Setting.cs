using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Settings;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UltimateFireworks.Configuration;
using UltimateFireworks.HarmonyPatches;
using UltimateFireworks.Interfaces;
using UltimateFireworks.Models;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UltimateFireworks.Views
{
    [HotReload]
    internal class Setting : BSMLAutomaticViewController, IInitializable, IDisposable
    {
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プロパティ
        /// <summary>
        /// For this method of setting the ResourceName, this class must be the first class in the file.
        /// </summary>
        public string ResourceName => string.Join(".", this.GetType().Namespace, this.GetType().Name);

        [UIValue("modes")]
        private readonly List<object> _modes = new List<object>() { "InSide", "Normal" };

        /// <summary>説明 を取得、設定</summary>
        private string _currentMode;
        /// <summary>説明 を取得、設定</summary>
        [UIValue("current-mode")]
        public string CurrentMode
        {
            get => this._currentMode;

            set => this.SetProperty(ref this._currentMode, value);
        }

        /// <summary>説明 を取得、設定</summary>
        private int _scaleValue;
        /// <summary>説明 を取得、設定</summary>
        [UIValue("scale-value")]
        public int ScaleValue
        {
            get => this._scaleValue;

            set => this.SetProperty(ref this._scaleValue, value);
        }

        /// <summary>説明 を取得、設定</summary>
        private float _gravityValue;
        /// <summary>説明 を取得、設定</summary>
        [UIValue("gravity-value")]
        public float GravityValue
        {
            get => this._gravityValue;

            set => this.SetProperty(ref this._gravityValue, value);
        }

        /// <summary>説明 を取得、設定</summary>
        private bool _fire;
        /// <summary>説明 を取得、設定</summary>
        [UIValue("fire-enable")]
        public bool Fire
        {
            get => this._fire;

            set => this.SetProperty(ref this._fire, value);
        }

        /// <summary>説明 を取得、設定</summary>
        private bool _traile;
        /// <summary>説明 を取得、設定</summary>
        [UIValue("traile-enable")]
        public bool Traile
        {
            get => this._traile;

            set => this.SetProperty(ref this._traile, value);
        }

        /// <summary>説明 を取得、設定</summary>
        private bool _refrect;
        /// <summary>説明 を取得、設定</summary>
        [UIValue("refrect-enable")]
        public bool Refrect
        {
            get => this._refrect;

            set => this.SetProperty(ref this._refrect, value);
        }

        /// <summary>説明 を取得、設定</summary>
        private float _radial;
        /// <summary>説明 を取得、設定</summary>
        [UIValue("radial")]
        public float Radial
        {
            get => this._radial;

            set => this.SetProperty(ref this._radial, value);
        }

        /// <summary>説明 を取得、設定</summary>
        private string _soundSet;
        /// <summary>説明 を取得、設定</summary>
        [UIValue("sound-set")]
        public string SoundSet
        {
            get => this._soundSet;

            set => this.SetProperty(ref this._soundSet, value);
        }

        [UIValue("sounds")]
        public List<object> Sounds { get; set; } = new List<object>() { "Default", "Kinni-kun" };
        private List<string> _sounds;

        [UIComponent("sound-dropdown")]
        private readonly object _dropDownObject;
        private SimpleTextDropdown _simpleTextDropdown;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // コマンド
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // コマンド用メソッド
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // オーバーライドメソッド
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // パブリックメソッド
        public void Initialize()
        {
            BSMLSettings.Instance.AddSettingsMenu("UltimateFireworks", this.ResourceName, this);
            this.ApplyRealValue();
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プライベートメソッド
        private void LoadValue()
        {
            this.ScaleValue = PluginConfig.Instance.Scale;
            this.GravityValue = PluginConfig.Instance.GravityModifierMultiplier;
            this.Fire = PluginConfig.Instance.FireEnable;
            this.Traile = PluginConfig.Instance.TraileEnable;
            this.Refrect = PluginConfig.Instance.Refrect;
            this.Radial = PluginConfig.Instance.Radial;
            this.SoundSet = PluginConfig.Instance.SoundSet;
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

        private void ApplyAllValue()
        {
            PluginConfig.Instance.Scale = this.ScaleValue;
            PluginConfig.Instance.GravityModifierMultiplier = this.GravityValue;
            PluginConfig.Instance.FireEnable = this.Fire;
            PluginConfig.Instance.TraileEnable = this.Traile;
            PluginConfig.Instance.Refrect = this.Refrect;
            PluginConfig.Instance.Radial = this.Radial;
            PluginConfig.Instance.SoundSet = this.SoundSet;
            if (this.CurrentMode == this._modes[0].ToString()) {
                PluginConfig.Instance.Mode = PluginConfig.FireWorksMode.InSide;
            }
            else {
                PluginConfig.Instance.Mode = PluginConfig.FireWorksMode.Normal;
            }
        }

        private void ApplyPreviewValue()
        {
            FireworksItemControllerOverride.Scale = this.ScaleValue;
            FireworksItemControllerOverride.GravityModifierMultiplier = this.GravityValue;
            FireworksItemControllerOverride.FireEnable = this.Fire;
            FireworksItemControllerOverride.TraileEnable = this.Traile;
            FireworksItemControllerOverride.Refrect = this.Refrect;
            FireworksItemControllerOverride.Radial = this.Radial;
            this._ultimateFireworksController.SoundSet = this.SoundSet;
            if (this.CurrentMode == this._modes[0].ToString()) {
                this._ultimateFireworksController.Mode = PluginConfig.FireWorksMode.InSide;
            }
            else {
                this._ultimateFireworksController.Mode = PluginConfig.FireWorksMode.Normal;
            }
        }

        private void ApplyRealValue()
        {
            FireworksItemControllerOverride.Scale = PluginConfig.Instance.Scale;
            FireworksItemControllerOverride.GravityModifierMultiplier = PluginConfig.Instance.GravityModifierMultiplier;
            FireworksItemControllerOverride.FireEnable = PluginConfig.Instance.FireEnable;
            FireworksItemControllerOverride.TraileEnable = PluginConfig.Instance.TraileEnable;
            FireworksItemControllerOverride.Refrect = PluginConfig.Instance.Refrect;
            FireworksItemControllerOverride.Radial = PluginConfig.Instance.Radial;
            this._ultimateFireworksController.SoundSet = PluginConfig.Instance.SoundSet;
            this._ultimateFireworksController.Mode = PluginConfig.Instance.Mode;
        }

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

        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            base.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
            this.ApplyRealValue();
            this._ultimateFireworksController.enabled = false;
        }

        [UIAction("#post-parse")]
        protected void PostParse()
        {
            this.LoadValue();
            MainThreadInvoker.Instance.Enqueue(this.CreateList());
        }

        [UIAction("#apply")]
        protected void OnApply()
        {
            this.ApplyAllValue();
            this.ApplyRealValue();
        }

        [UIAction("preview-action")]
        protected void Preview()
        {
            if (this._fireworksController) {
                this._fireworksController.enabled = !this._fireworksController.enabled;
            }
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
            Plugin.Log.Debug("OnDestroy call");
            this._simpleTextDropdown.didSelectCellWithIdxEvent -= this.SimpleTextDropdown_didSelectCellWithIdxEvent;
            base.OnDestroy();
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string membberName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) {
                return false;
            }
            field = value;
            this.OnPropertyChanged(new PropertyChangedEventArgs(membberName));
            return true;
        }

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
            this.ApplyPreviewValue();
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // メンバ変数
        private ISoundLoader _soundLoader;
        private UltimateFireworksController _ultimateFireworksController;
        private FireworksController _fireworksController;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // 構築・破棄
        [Inject]
        internal void Constractor(ISoundLoader soundLoader, UltimateFireworksController ultimateFireworksController, FireworksController fireworksController)
        {
            Plugin.Log.Debug("Constractor call");
            this._soundLoader = soundLoader;
            this._ultimateFireworksController = ultimateFireworksController;
            this._fireworksController = fireworksController;
        }

        public void Dispose()
        {
            BSMLSettings.Instance.RemoveSettingsMenu(this);
        }
        #endregion
    }
}
