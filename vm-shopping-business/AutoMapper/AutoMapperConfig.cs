using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using vm_shopping_data_access.Entities;
using vm_shopping_models.Entities;


namespace vm_shopping_business.AutoMapper
{
    public class AutoMapperConfig : IAutoMapperConfig
    {
        private MapperConfiguration mapperConfiguration;

        private MapperConfiguration AutoMapper()
        {
            if (mapperConfiguration == null)
            {
                mapperConfiguration = new MapperConfiguration(config =>
                {
                    config.CreateMap<Order, OrderResponse>()
                        .ForMember(dest => dest.ShoppingOrderId, act => act.MapFrom(src => src.Id))
                        .ForMember(dest => dest.URLRedirection, act => act.MapFrom(src => src.GatewayUrlRedirection))
                        .ForMember(dest => dest.Status, act => act.MapFrom(src => src.Status))
                        .ForMember(dest => dest.Product, act => act.MapFrom(src => src.Product));

                    config.CreateMap<ClientRequest, Customer>();

                    config.CreateMap<Customer, ClientResponse>()
                        .ForMember(dest => dest.ClientId, act => act.MapFrom(src => src.Id));

                    config.CreateMap<Status, StatusResponse>()
                        .ForMember(dest => dest.Status, act => act.MapFrom(src => src.Description));

                    config.CreateMap<Product, ProductResponse>()
                        .ForMember(dest => dest.ProductId, act => act.MapFrom(src => src.Id));

                    config.CreateMap<ProductRequest,Product>();

                    config.CreateMap<PaymentNotificationRequest, PaymentNotification>()
                        .ForMember(dest => dest.Message, act => act.MapFrom(src => src.status != null ? src.status.message : null))
                        .ForMember(dest => dest.Reason, act => act.MapFrom(src => src.status != null ? src.status.reason : null))
                        .ForMember(dest => dest.Date, act => act.MapFrom(src => src.status != null ? DateTime.Parse(src.status.date) : new DateTime()));
                });
            }

            return mapperConfiguration;
        }

        public Mapper GetMapper()
        {
            return new Mapper(AutoMapper());
        }
    }
}
