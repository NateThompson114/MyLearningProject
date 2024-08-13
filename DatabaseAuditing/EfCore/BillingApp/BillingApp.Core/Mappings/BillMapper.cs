using BillingApp.Core.DTOs;
using BillingApp.Core.Entities;

namespace BillingApp.Core.Mappings;

public static class BillMapper
{
    public static Bill ToEntity(this BillDto dto)
    {
        return new Bill
        {
            CustomerName = dto.CustomerName,
            Amount = dto.Amount,
            BillingDate = dto.BillingDate
        };
    }

    public static BillDto ToDto(this Bill entity)
    {
        return new BillDto
        {
            CustomerName = entity.CustomerName,
            Amount = entity.Amount,
            BillingDate = entity.BillingDate
        };
    }

    public static void UpdateEntityFromDto(this Bill entity, BillDto dto)
    {
        entity.CustomerName = dto.CustomerName;
        entity.Amount = dto.Amount;
        entity.BillingDate = dto.BillingDate;
    }
}