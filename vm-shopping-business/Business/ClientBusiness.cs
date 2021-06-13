using System;
using System.Linq;
using System.Threading.Tasks;
using vm_shopping_business.Business;
using vm_shopping_business.Interfaces;
using vm_shopping_data_access;
using vm_shopping_data_access.Entities;
using vm_shopping_models.Entities;

namespace vm_shopping_business
{
    public class ClientBusiness : BusinessBase, IClientBusiness
    {
        public ClientBusiness(ShoppingDBContext shoppingDBContext) : base(shoppingDBContext)
        {
        }

        public async Task<ClientResponse> SaveClient(ClientRequest client)
        {
            var clientResponse = new ClientResponse();
            try
            {
                var currentClient = shoppingDBContext.Customer.Where(t => t.Mail == client.Mail).FirstOrDefault();
                if (currentClient == null)
                {
                    Customer customer = new Customer
                    {
                        Name = client.Name,
                        Mail = client.Mail,
                        Phone = client.Phone
                    };
                    shoppingDBContext.Add(customer);
                    await shoppingDBContext.SaveChangesAsync();
                    currentClient = customer;
                }

                clientResponse.ClientId = currentClient.Id;
                clientResponse.Name = currentClient.Name;
                clientResponse.Email = currentClient.Mail;
                clientResponse.Phone = currentClient.Phone;
            }
            catch (Exception ex)
            {
                LogError("SaveClient", ex);
            }

            return clientResponse;
        }
    }
}
