using Microsoft.Extensions.Logging;
using System;
using vm_shopping_data_access;
using vm_shopping_data_access.Entities;

namespace vm_shopping_business.Business
{
    public class BusinessBase
    {
        public readonly ShoppingDBContext shoppingDBContext;

        public BusinessBase(ShoppingDBContext shoppingDBContext)
        {
            this.shoppingDBContext = shoppingDBContext;
        }

        public void LogError(string methodName, Exception ex)
        {
            var logger = new Logger
            {
                LogDate = DateTime.Now,
                Message = string.Format("{0} : {1}", methodName, ex.Message),
                LogLevel = LogLevel.Error.ToString(),
                Details = ex.InnerException != null ? string.Format("InnerException: {0}", ex.InnerException.Message) : string.Empty,
            };
            shoppingDBContext.Add(logger);
            shoppingDBContext.SaveChanges();
        }

        public void Log(string methodName, string mesagge)
        {
            var logger = new Logger
            {
                LogDate = DateTime.Now,
                Message = string.Format("{0} : {1}", methodName, mesagge),
                LogLevel = LogLevel.Information.ToString(),
                Details = "ok"
            };
            shoppingDBContext.Add(logger);
            shoppingDBContext.SaveChanges();
        }
    }
}
