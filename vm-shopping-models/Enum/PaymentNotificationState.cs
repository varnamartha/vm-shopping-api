using System;
using System.Collections.Generic;
using System.Text;

namespace vm_shopping_models.Enum
{
    public enum PaymentNotificationState
    {
        Ok,
        OrderNotFound,
        NotificationPaymentStatusUnrecognized,
        Error,
        InvalidOrigin
    }
}
