using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.DAL.EFCore.Entities;
using Aurigma.DirectMail.Sample.DAL.EFCore.Interfaces.Repositories;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.RecipientList;
using AutoMapper;
using DomainStorage = Aurigma.DirectMail.Sample.App.Interfaces.Storages;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Adapters.Repositories;

public class RecipientListAdapter(IRecipientListRepository repository, IMapper mapper)
    : DomainStorage.IRecipientListRepository
{
    private readonly IRecipientListRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<RecipientList> CreateRecipientListAsync(RecipientList list)
    {
        return _mapper.Map<RecipientList>(
            await _repository.CreateRecipientListAsync(_mapper.Map<RecipientListDal>(list))
        );
    }

    public async Task<List<RecipientList>> GetAllAsReadOnlyAsync()
    {
        return _mapper.Map<List<RecipientList>>(await _repository.GetAllAsReadOnlyAsync());
    }

    public async Task<RecipientList> GetRecipientListAsReadOnlyAsync(Guid id)
    {
        return _mapper.Map<RecipientList>(await _repository.GeRecipientListAsReadOnlyAsync(id));
    }

    public async Task<List<RecipientList>> GetRecipientListsByFilterAsync(
        RecipientListFilter filter
    )
    {
        return _mapper.Map<List<RecipientList>>(
            await _repository.GetRecipientListsByFilterAsync(filter)
        );
    }
}

internal class RecipientListAdapterMapperProfile : Profile
{
    public RecipientListAdapterMapperProfile()
    {
        CreateMap<RecipientDal, Recipient>()
            .ForMember(
                dest => dest.Images,
                opt =>
                    opt.MapFrom(src => src.RecipientImageRecipients.Select(ri => ri.RecipientImage))
            );
        CreateMap<Recipient, RecipientDal>();

        CreateMap<RecipientImage, RecipientImageDal>();
        CreateMap<RecipientImageDal, RecipientImage>();

        CreateMap<RecipientListDal, RecipientList>()
            .ForMember(dest => dest.Recipients, opt => opt.MapFrom(src => src.Recipients));

        CreateMap<RecipientList, RecipientListDal>()
            .ForMember(dest => dest.Recipients, opt => opt.MapFrom(src => src.Recipients));
    }
}
