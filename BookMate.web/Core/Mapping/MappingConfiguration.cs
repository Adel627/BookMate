using BookMate.web.Core.ViewModels.Book;
using Mapster;
using Microsoft.AspNetCore.Identity;
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
            config.NewConfig<BookFormViewModel, Book>()
               .Ignore(dest => dest.Categories)
               .Ignore(dest => dest.ImageUrl);
            config.NewConfig<Book , BookFormViewModel>()
                .Map(dest => dest.SelectedCategories , src => src.Categories.Select(c=>c.CategoryId));
         
            config.NewConfig<IdentityRole , SelectListItem>()
                .Map(dest => dest.Text , src => src.Name)
                .Map(dest => dest.Value , src => src.Name);
            //Subscriber
            config.NewConfig<Governorate, SelectListItem>()
                .Map(dest => dest.Text, src => src.Name)
                .Map(dest => dest.Value, src => src.Id);

            config.NewConfig<Area, SelectListItem>()
               .Map(dest => dest.Text, src => src.Name)
               .Map(dest => dest.Value, src => src.Id);
        }
    }
}
