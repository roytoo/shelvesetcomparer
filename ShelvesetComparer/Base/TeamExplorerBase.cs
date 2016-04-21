/*
    <copyright file="TeamExplorerBase.cs" company="http://shelvesetcomparer.codeplex.com">
        Copyright http://shelvesetcomparer.codeplex.com. All Rights Reserved.
        This code released under the terms of the Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
        This is sample code only, do not use in production environments.
    </copyright>
 */

namespace Tfs.ShelvesetComparer.Base
{
    using System;
    using System.ComponentModel;
    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.Controls;

    /// <summary>
    ///     Team Explorer extension common base class.
    /// </summary>
    public class TeamExplorerBase : IDisposable, INotifyPropertyChanged
    {
        #region [Fields]

        /// <summary>
        /// The context subscribed.
        /// </summary>
        private bool contextSubscribed;

        /// <summary>
        /// The service provider.
        /// </summary>
        private IServiceProvider serviceProvider;

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region [Properties]

        /// <summary>
        /// Gets or sets the service provider.
        /// </summary>
        public IServiceProvider ServiceProvider
        {
            get { return this.serviceProvider; }

            set
            {
                // Unsubscribe from Team Foundation context changes
                if (this.serviceProvider != null)
                {
                    this.UnsubscribeContextChanges();
                }

                this.serviceProvider = value;

                // Subscribe to Team Foundation context changes
                if (this.serviceProvider != null)
                {
                    this.SubscribeContextChanges();
                }
            }
        }

        /// <summary>
        ///     Gets the current context.
        /// </summary>
        protected ITeamFoundationContext CurrentContext
        {
            get
            {
                ITeamFoundationContextManager tfcontextManager = this.GetService<ITeamFoundationContextManager>();
                return tfcontextManager != null ? tfcontextManager.CurrentContext : null;
            }
        }
        #endregion

        /// <summary>
        ///     Gets the service.
        /// </summary>
        /// <typeparam name="T">An <see cref="IServiceProvider"/> implementation.</typeparam>
        /// <returns>An instance of <see cref="IServiceProvider"/> or null.</returns>
        public T GetService<T>()
        {
            if (this.ServiceProvider != null)
            {
                return (T) this.ServiceProvider.GetService(typeof (T));
            }

            return default(T);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.UnsubscribeContextChanges();
            }
        }

        /// <summary>
        /// Shows the notification.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        protected Guid ShowNotification(string message, NotificationType type)
        {
            ITeamExplorer teamExplorer = this.GetService<ITeamExplorer>();
            if (teamExplorer != null)
            {
                Guid guid = Guid.NewGuid();
                teamExplorer.ShowNotification(message, type, NotificationFlags.None, null, guid);
                return guid;
            }

            return Guid.Empty;
        }

        /// <summary>
        /// Raises the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Subscribes the context changes.
        /// </summary>
        protected void SubscribeContextChanges()
        {
            if (this.ServiceProvider == null || this.contextSubscribed)
            {
                return;
            }

            ITeamFoundationContextManager tfcontextManager = this.GetService<ITeamFoundationContextManager>();
            if (tfcontextManager != null)
            {
                tfcontextManager.ContextChanged += this.ContextChanged;
                this.contextSubscribed = true;
            }
        }

        /// <summary>
        /// Unsubscribes the context changes.
        /// </summary>
        protected void UnsubscribeContextChanges()
        {
            if (this.ServiceProvider == null || !this.contextSubscribed)
            {
                return;
            }

            ITeamFoundationContextManager tfcontextManager = this.GetService<ITeamFoundationContextManager>();
            if (tfcontextManager != null)
            {
                tfcontextManager.ContextChanged -= this.ContextChanged;
            }
        }

        /// <summary>
        /// Contexts the changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ContextChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void ContextChanged(object sender, ContextChangedEventArgs e)
        {
        }
    }
}