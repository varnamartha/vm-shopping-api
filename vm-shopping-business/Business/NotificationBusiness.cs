using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vm_shopping_business.Interfaces;
using vm_shopping_data_access;
using vm_shopping_data_access.Entities;
using vm_shopping_models.Entities;
using vm_shopping_models.Enum;

namespace vm_shopping_business.Business
{
    public class NotificationBusiness : BusinessBase, INotificationBusiness
    {
        public NotificationBusiness(ShoppingDBContext shoppingDBContext) : base(shoppingDBContext)
        {
        }

        public async Task<NotificationResponse> PaymentNotificationSync(PaymentNotificationRequest paymentNotificationRequest)
        {
            var notificationResponse = new NotificationResponse();
            try
            {
                var isValidSignature = validateSignatureWithPaymentNotificationRequest(paymentNotificationRequest);
                if(!isValidSignature)
                {
                    notificationResponse.state = Enum.GetName(typeof(PaymentNotificationState), PaymentNotificationState.InvalidOrigin);
                    return notificationResponse;
                }

                var order = shoppingDBContext.Order.Where(t => t.GatewayPaymentId == paymentNotificationRequest.requestId.ToString()).FirstOrDefault();
                if (order == null)
                {
                    notificationResponse.state = Enum.GetName(typeof(PaymentNotificationState), PaymentNotificationState.OrderNotFound);
                    return notificationResponse;
                }

                var status =  shoppingDBContext.Status
                                   .Where(t => t.Description.ToUpper() == paymentNotificationRequest.status.status.ToUpper()).FirstOrDefault();
                if (status == null)
                {
                    notificationResponse.state = Enum.GetName(typeof(PaymentNotificationState), PaymentNotificationState.NotificationPaymentStatusUnrecognized);
                    return notificationResponse;
                }

                //Updating order
                order.UpdatedAt = DateTime.Now;
                order.StatusId = status.Id;

                //Creating Payment notification
                PaymentNotification paymentNotification = new PaymentNotification
                {
                    OrderId = order.Id,
                    Message = paymentNotificationRequest.status.message,
                    Reason = paymentNotificationRequest.status.reason,
                    Reference = paymentNotificationRequest.reference,
                    Signature = paymentNotificationRequest.signature,
                    Date = DateTime.Parse(paymentNotificationRequest.status.date)

                };
                shoppingDBContext.PaymentNotification.Add(paymentNotification);
                await shoppingDBContext.SaveChangesAsync();

                notificationResponse.state = Enum.GetName(typeof(PaymentNotificationState), PaymentNotificationState.Ok);
            }
            catch (Exception ex)
            {
                LogError("PaymentNotificationSync", ex);
                notificationResponse.state = Enum.GetName(typeof(PaymentNotificationState), PaymentNotificationState.Error);
            }
            return notificationResponse;
        }

        internal bool validateSignatureWithPaymentNotificationRequest(PaymentNotificationRequest PaymentNotificationRequest)
        {
            //ToDo: Validate the signature

            //Gayway payment notes:
            //Puedes validar que se trate de una respuesta de PlacetoPay haciendo un SHA - 1 con los datos requestId +status + date + secretKey.
            //Si este valor coincide con el valor proporcionado por el signature, puedes autenticar la respuesta y proceder a asentar en la base de datos.

            return true;
        }
    }
}
