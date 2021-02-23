using System;
using System.Runtime.CompilerServices;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace UltimateFireworks.Configuration
{
    internal class PluginConfig
    {
        public static PluginConfig Instance { get; set; }
        public virtual FireWorksMode Mode { get; set; } = FireWorksMode.InSide;
        public virtual int Scale { get; set; } = 10;
        public virtual float GravityModifierMultiplier { get; set; } = 20f;

        public event Action<PluginConfig> ReloadEvent;
        public event Action<PluginConfig> ChangedEvent;

        /// <summary>
        /// This is called whenever BSIPA reads the config from disk (including when file changes are detected).
        /// </summary>
        public virtual void OnReload()
        {
            // Do stuff after config is read from disk.
            this.ReloadEvent?.Invoke(this);
        }

        /// <summary>
        /// Call this to force BSIPA to update the config file. This is also called by BSIPA if it detects the file was modified.
        /// </summary>
        public virtual void Changed()
        {
            // Do stuff when the config is changed.
            this.ChangedEvent?.Invoke(this);
        }

        /// <summary>
        /// Call this to have BSIPA copy the values from <paramref name="other"/> into this config.
        /// </summary>
        public virtual void CopyFrom(PluginConfig other)
        {
            // This instance's members populated from other
            this.Mode = other.Mode;
        }

        public enum FireWorksMode
        {
            Normal,
            InSide
        }
    }
}