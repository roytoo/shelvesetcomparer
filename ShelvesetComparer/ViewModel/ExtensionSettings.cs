// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionSettings.cs" company="">
//
// </copyright>
// <summary>
//   The extension settings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tfs.ShelvesetComparer.ViewModel
{
    using Microsoft.VisualStudio.Settings;
    using Microsoft.VisualStudio.Shell.Settings;

    /// <summary>
    /// The extension settings.
    /// </summary>
    public class ExtensionSettings
    {
        #region [Constants]

        /// <summary>
        /// The collection path.
        /// </summary>
        private const string CollectionPath = "ShelveSetComparer.Next";

        #endregion

        #region [Fields]

        /// <summary>
        /// The writable settings store.
        /// </summary>
        private WritableSettingsStore writableSettingsStore;

        /// <summary>
        /// The readable setting store.
        /// </summary>
        private SettingsStore readableSettingStore;

        #endregion

        #region [Properties]

        /// <summary>
        ///     Gets the Instance object
        /// </summary>
        public static ExtensionSettings Instance { get; private set; }

        /// <summary>
        ///     Gets or sets the a property that ShowAsButton
        /// </summary>
        public bool ShowAsButton
        {
            get
            {
                return this.readableSettingStore.GetBoolean(CollectionPath, nameof(ShowAsButton));
            }

            set
            {
                this.writableSettingsStore.SetBoolean(CollectionPath, nameof(ShowAsButton), value);
            }
        }

        /// <summary>
        ///     Gets or sets the setting indicating whether second user is shown in team explorer or not.
        /// </summary>
        public bool TwoUsersView
        {
            get
            {
                return this.readableSettingStore.GetBoolean(CollectionPath, nameof(TwoUsersView));
            }

            set
            {
                this.writableSettingsStore.SetBoolean(CollectionPath, nameof(TwoUsersView), value);
            }
        }

        #endregion

        #region [Constructor]

        /// <summary>
        ///     Prevents a default instance of the <see cref=" ExtensionSettings"/> class from being created.
        /// </summary>
        private ExtensionSettings()
        {
        }

        #endregion

        #region [Methods]

        /// <summary>
        ///     Creates a new instances of the ExtensionSetting class
        /// </summary>
        /// <param name="package">The Visual studio extension package</param>
        public static void CreateInstance(ShelvesetComparerPackage package)
        {
            if (Instance != null)
            {
                return;
            }

            Instance = new ExtensionSettings();
            Instance.Initialize(package);
        }

        /// <summary>
        ///     Initializes properties in the package
        /// </summary>
        /// <param name="package">The package</param>
        private void Initialize(ShelvesetComparerPackage package)
        {
            SettingsManager settingsManager = new ShellSettingsManager(package);
            this.writableSettingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            if (!this.writableSettingsStore.CollectionExists(CollectionPath))
            {
                this.writableSettingsStore.CreateCollection(CollectionPath);
                this.ShowAsButton = true;
                this.TwoUsersView = true;
            }

            this.readableSettingStore = settingsManager.GetReadOnlySettingsStore(SettingsScope.UserSettings);
        }

        #endregion
    }
}