using Application.DTOs;
using Domain.Entity;
using Domain.Enum;
using Mapster;

namespace Application.Mapper
{
    public static class MapConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<TransactionCreateDto, Transaction>.NewConfig();

            TypeAdapterConfig<Transaction, TransactionReadDto>.NewConfig()
                .Map(dest => dest.Status, src => src.Status.ToString());
        }
    }
}