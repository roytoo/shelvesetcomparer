﻿namespace Tfs.ShelvesetComparer.Views
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using EnvDTE;
    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.Framework.Client;
    using Microsoft.TeamFoundation.Framework.Common;
    using Microsoft.TeamFoundation.VersionControl.Client;
    using TeamExplorer;
    using ViewModel;

    /// <summary>
    ///     Team Explorer view allow to select shelveset for comparison.
    /// </summary>
    public partial class SelectShelvesetTeamExplorerView
    {
        /// <summary>
        ///     Depending property for Parent Section.
        /// </summary>
        public static readonly DependencyProperty ParentSectionProperty = DependencyProperty.Register("ParentSection",
            typeof (SelectShelvesetSection), typeof (SelectShelvesetSection));

        /// <summary>
        ///     Initializes a new instance of the SelectShelvesetTeamExplorerView class
        /// </summary>
        /// <param name="parentSection">Reference to the Team Explorer section where the view is initialized.</param>
        public SelectShelvesetTeamExplorerView(SelectShelvesetSection parentSection)
        {
            this.InitializeComponent();
            this.ParentSection = parentSection;
            this.ListShelvesets.ItemsSource = this.ParentSection.Shelvesets;
            this.SecondShelvesetUserTextBox.Visibility = ExtensionSettings.Instance.TwoUsersView
                ? Visibility.Visible
                : Visibility.Hidden;
            this.SecondShelvesetUserTextBox.Height = ExtensionSettings.Instance.TwoUsersView ? 25 : 0;
            this.ShowAsButton.IsChecked = ExtensionSettings.Instance.ShowAsButton;
            this.TwoUsersView.IsChecked = ExtensionSettings.Instance.TwoUsersView;
            this.DataContext = this;
        }

        /// <summary>
        ///     Gets the Team Explorer section in to which the view is created.
        /// </summary>
        public SelectShelvesetSection ParentSection
        {
            get
            {
                return (SelectShelvesetSection) this.GetValue(ParentSectionProperty);
            }

            private set
            {
                this.SetValue(ParentSectionProperty, value);
            }
        }

        /// <summary>
        ///     Event Handler for Selection change in the Shelvesets list
        /// </summary>
        /// <param name="sender">The ListShelvesets ListView control</param>
        /// <param name="e">The event arguments</param>
        public void ListShelvesetsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ListShelvesets.SelectedItems.Count > 2)
            {
                this.ListShelvesets.SelectedItems.RemoveAt(0);
            }
        }

        /// <summary>
        ///     Event Handler for keying user name for the first shelveset.
        /// </summary>
        /// <param name="sender">The first shelveset user text box</param>
        /// <param name="e">The event arguments</param>
        private async void FirstShelvesetUserTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            this.ClearError();
            if (e.Key != Key.Enter)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(this.FirstShelvesetUserTextBox.Text) &&
                this.UserIdentityExists(this.FirstShelvesetUserTextBox.Text) == false)
            {
                this.ShowError("User Account Name or display name could not be found");
                return;
            }

            await this.ParentSection.RefreshShelvesets();
            this.ListShelvesets.ItemsSource = this.ParentSection.Shelvesets;
        }

        /// <summary>
        ///     Event Handler for keying user name for the second shelveset.
        /// </summary>
        /// <param name="sender">The second shelveset user text box</param>
        /// <param name="e">The event arguments</param>
        private async void SecondShelvesetUserTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            this.ClearError();
            if (e.Key == Key.Enter)
            {
                if (!string.IsNullOrWhiteSpace(this.SecondShelvesetUserTextBox.Text) &&
                    this.UserIdentityExists(this.SecondShelvesetUserTextBox.Text) == false)
                {
                    this.ShowError("User Account Name or display name could not be found");
                    return;
                }

                await this.ParentSection.RefreshShelvesets();
                this.ListShelvesets.ItemsSource = this.ParentSection.Shelvesets;
            }
        }

        /// <summary>
        ///     Returns whether the user identity of the given account name or display name exists or not
        /// </summary>
        /// <param name="userDisplayNameOrAccount">The given account name or display name</param>
        /// <returns>True if the given user account or display name exists. False otherwise</returns>
        private bool UserIdentityExists(string userDisplayNameOrAccount)
        {
            ITeamFoundationContext context = this.ParentSection.Context;
            IIdentityManagementService ims = context.TeamProjectCollection.GetService<IIdentityManagementService>();

            // First try search by AccountName
            TeamFoundationIdentity userIdentity = ims.ReadIdentity(IdentitySearchFactor.AccountName,
                userDisplayNameOrAccount, MembershipQuery.None, ReadIdentityOptions.ExtendedProperties);
            if (userIdentity == null)
            {
                // Next we try search by DisplayName
                userIdentity = ims.ReadIdentity(IdentitySearchFactor.DisplayName, userDisplayNameOrAccount,
                    MembershipQuery.None, ReadIdentityOptions.ExtendedProperties);
                if (userIdentity == null)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///     Displays the error panel
        /// </summary>
        /// <param name="error">The error text</param>
        private void ShowError(string error)
        {
            this.ErrorText.Text = error;
            this.ErrorPanel.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Clears the error panel
        /// </summary>
        private void ClearError()
        {
            this.ErrorText.Text = string.Empty;
            this.ErrorPanel.Visibility = Visibility.Hidden;
        }

        /// <summary>
        ///     Event Handler for the list button.
        /// </summary>
        /// <param name="sender">The list button</param>
        /// <param name="e">Event parameters</param>
        private async void ListButton_Click(object sender, RoutedEventArgs e)
        {
            this.ClearError();
            await this.ParentSection.RefreshShelvesets();
            this.ListShelvesets.ItemsSource = this.ParentSection.Shelvesets;
        }

        /// <summary>
        ///     Event Handler for the compare button.
        /// </summary>
        /// <param name="sender">The compare button</param>
        /// <param name="e">Event parameters</param>
        private void CompareButtons_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.ClearError();
                if (this.ListShelvesets.SelectedItems != null && this.ListShelvesets.SelectedItems.Count < 2)
                {
                    this.ShowError(ShelvesetComparer.Resources.ShelvesetNotSelectedErrorMessage);
                    return;
                }

                var dte2 = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof (DTE)) as EnvDTE80.DTE2;
                var selectedItems = this.ListShelvesets.SelectedItems;
                if (selectedItems != null)
                {
                    var firstSheleveset = selectedItems[0] as Shelveset;
                    var secondSheleveset = selectedItems[1] as Shelveset;
                    ShelvesetComparerViewModel.Instance.Initialize(firstSheleveset, secondSheleveset);
                }
                else
                {
                    this.ShowError(ShelvesetComparer.Resources.ShelvesetNotSelectedErrorMessage);
                    return;
                }

                dte2?.ExecuteCommand("Tools.ShelvesetComparer");
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        /// <summary>
        ///     Event Handler for mouse double click on the shelvesets list.
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">Event arguments</param>
        private void ListShelvesets_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e != null && e.ChangedButton == MouseButton.Left && this.ListShelvesets.SelectedItems.Count == 1)
            {
                this.ViewShelvesetDetails(this.ListShelvesets);
            }
        }

        /// <summary>
        ///     Event Handler for key up event on the shelvesets list.
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">Event arguments</param>
        private void ListShelvesets_KeyUp(object sender, KeyEventArgs e)
        {
            if (e != null && e.Key == Key.Enter && this.ListShelvesets.SelectedItems.Count == 1)
            {
                this.ViewShelvesetDetails(this.ListShelvesets);
            }
        }

        /// <summary>
        ///     Opens up the shelveset details team explorer page for given shelveset
        /// </summary>
        /// <param name="listView">The listView control to read selected shelveset from</param>
        private void ViewShelvesetDetails(ListView listView)
        {
            if (listView.SelectedItems.Count == 1)
            {
                var shelveset = listView.SelectedItems[0] as Shelveset;
                if (shelveset != null)
                {
                    this.ParentSection.ViewShelvesetDetails(shelveset);
                }
            }
        }

        /// <summary>
        ///     Event handler for Options Windows link
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The events argument</param>
        private void ShowOptionsWindow_Click(object sender, RoutedEventArgs e)
        {
            this.ShowPanel("option");
        }

        /// <summary>
        ///     Event Handler for the Show Compare button
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event argument.</param>
        private void ShowCompareWindow_Click(object sender, RoutedEventArgs e)
        {
            this.ShowPanel("select");
        }

        /// <summary>
        ///     Event handler for the save button
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event argument.</param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ExtensionSettings.Instance.ShowAsButton = this.ShowAsButton.IsChecked.GetValueOrDefault(false);
            ExtensionSettings.Instance.TwoUsersView = this.TwoUsersView.IsChecked.GetValueOrDefault(false);
            this.ShowPanel("select");
        }

        /// <summary>
        ///     Helper method to switch between option and select panel
        /// </summary>
        /// <param name="panelToShow">The name of the panel to show</param>
        private void ShowPanel(string panelToShow)
        {
            var showOptionsPanel = panelToShow == "option" ? true : false;
            var showSelectPanel = panelToShow == "select" ? true : false;

            this.ShelvesetComparisonPanel.Visibility = showSelectPanel ? Visibility.Visible : Visibility.Hidden;
            this.SelectShelvesetPanel.Visibility = showSelectPanel ? Visibility.Visible : Visibility.Hidden;
            this.OptionsPanel.Visibility = showOptionsPanel ? Visibility.Visible : Visibility.Hidden;
            this.SecondShelvesetUserTextBox.Visibility = ExtensionSettings.Instance.TwoUsersView
                ? Visibility.Visible
                : Visibility.Hidden;
            this.SecondShelvesetUserTextBox.Height = ExtensionSettings.Instance.TwoUsersView ? 25 : 0;
        }
    }
}