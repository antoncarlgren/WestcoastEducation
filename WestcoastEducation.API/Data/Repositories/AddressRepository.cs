using AutoMapper;
using WestcoastEducation.API.Data.Entities;
using WestcoastEducation.API.Data.Repositories.Interfaces;
using WestcoastEducation.API.ViewModels.Address;

namespace WestcoastEducation.API.Data.Repositories;

public class AddressRepository 
    : RepositoryBase<Address, AddressViewModel, PostAddressViewModel, PatchAddressViewModel>,
    IAddressRepository
{
    public AddressRepository(ApplicationContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public override async Task AddAsync(PostAddressViewModel model)
    {
        await Context.Addresses.AddAsync(Mapper.Map<Address>(model));
    }

    public override async Task UpdateAsync(string id, PostAddressViewModel model)
    {
        var address = await Context.Addresses.FindAsync(id);

        if (address is null)
        {
            throw new Exception($"Could not find {nameof(Address).ToLower()} with id {id}.");
        }

        address.City = model.City;
        address.StreetName = model.StreetName;
        address.ZipCode = model.ZipCode;

        Context.Addresses.Update(address);
    }

    public override async Task UpdateAsync(string id, PatchAddressViewModel model)
    {
        var address = await Context.Addresses.FindAsync(id);

        if (address is null)
        {
            throw new Exception($"Could not find {nameof(Address).ToLower()} with id {id}.");
        }

        address.City = model.City;
        address.StreetName = model.StreetName;
        address.ZipCode = model.ZipCode;

        Context.Addresses.Update(address);
    }
}