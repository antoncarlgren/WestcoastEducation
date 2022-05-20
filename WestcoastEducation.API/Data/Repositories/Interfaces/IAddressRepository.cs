using WestcoastEducation.API.Data.Entities;
using WestcoastEducation.API.ViewModels.Address;

namespace WestcoastEducation.API.Data.Repositories.Interfaces;

public interface IAddressRepository 
    : IRepository<AddressViewModel, PostAddressViewModel, PatchAddressViewModel>
{
    
}