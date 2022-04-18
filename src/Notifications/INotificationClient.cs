using System;
using System.Threading.Tasks;

namespace RepositoryComplianceEnforcer.Notifications
{
    /// <summary>
    /// General notification client for future expansion to email or other alerts
    /// </summary>
    public interface INotificationClient
    {
        Task Notify(string title, string body, dynamic destination);
    }
}

