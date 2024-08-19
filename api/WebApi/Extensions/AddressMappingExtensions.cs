using Core.Entities;
using WebApi.Dtos;

namespace WebApi.Extensions;

public static class AddressMappingExtensions
{
    public static AddressDTO? ToDto(this Adress? address)
    {
        if (address is null)
        {
            return null;
        }

        return new AddressDTO
        {
            Line1 = address.Line1,
            Line2 = address.Line2,
            City = address.City,
            PostalCode = address.PostalCode,
            Country = address.Country,
            State = address.State
        };
    }

    public static Adress ToEntity(this AddressDTO addressDTO)
    {
        if (addressDTO is null)
        {
            throw new ArgumentNullException(nameof(addressDTO));
        }

        return new Adress
        {
            Line1 = addressDTO.Line1,
            Line2 = addressDTO.Line2,
            City = addressDTO.City,
            PostalCode = addressDTO.PostalCode,
            Country = addressDTO.Country,
            State = addressDTO.State
        };
    }

    public static void UpdateFromDto(this Adress address, AddressDTO addressDTO)
    {
        if (address is null)
        {
            throw new ArgumentNullException(nameof(address));
        }

        if (addressDTO is null)
        {
            throw new ArgumentNullException(nameof(addressDTO));
        }

        address.Line1 = addressDTO.Line1;
        address.Line2 = addressDTO.Line2;
        address.City = addressDTO.City;
        address.PostalCode = addressDTO.PostalCode;
        address.Country = addressDTO.Country;
        address.State = addressDTO.State;
    }
}
