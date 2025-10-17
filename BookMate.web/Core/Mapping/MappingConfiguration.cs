using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Internal;

namespace BookMate.web.Core.Mapping
{
    public class MappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Author, SelectListItem>()
                 .Map(dest => dest.Value, src => src.Id)
                .Map(dest => dest.Text, src => src.Name);
               
            config.NewConfig<Category, SelectListItem>()
                .Map(dest => dest.Text, src => src.Name)
                .Map(dest => dest.Value, src => src.Id);
        }
    }
}
