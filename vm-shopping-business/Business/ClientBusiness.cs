using System;
using System.Linq;
using System.Threading.Tasks;
using vm_shopping_business.AutoMapper;
using vm_shopping_business.Business;
using vm_shopping_business.Interfaces;
using vm_shopping_data_access;
using vm_shopping_data_access.Entities;
using vm_shopping_models.Entities;

namespace vm_shopping_business
{
    public class ClientBusiness : BusinessBase, IClientBusiness
    {
        public readonly IAutoMapperConfig autoMapperConfig;

        public ClientBusiness(IAutoMapperConfig autoMapperConfig, ShoppingDBContext shoppingDBContext) : base(shoppingDBContext)
        {
            this.autoMapperConfig = autoMapperConfig;
        }

        public async Task<ClientResponse> SaveClient(ClientRequest client)
        {
            var clientResponse = new ClientResponse();
            try
            {
                var currentClient = shoppingDBContext.Customer.Where(t => t.Mail == client.Mail).FirstOrDefault();
                if (currentClient == null)
                {
                    Customer customer = autoMapperConfig.GetMapper().Map<ClientRequest, Customer>(client);

                    shoppingDBContext.Add(customer);
                    await shoppingDBContext.SaveChangesAsync();
                    currentClient = customer;
                }

                clientResponse = autoMapperConfig.GetMapper().Map<Customer, ClientResponse>(currentClient);
            }
            catch (Exception ex)
            {
                LogError("SaveClient", ex);
            }

            return clientResponse;
        }
    }
}
