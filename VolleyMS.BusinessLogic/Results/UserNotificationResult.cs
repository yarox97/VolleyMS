using VolleyMS.Core.Models;

public class UserNotificationResult
{
    public Notification Notification { get; }
    public bool IsChecked { get; }

    public UserNotificationResult(Notification notification, bool isChecked)
    {
        Notification = notification;
        IsChecked = isChecked;
    }
}
