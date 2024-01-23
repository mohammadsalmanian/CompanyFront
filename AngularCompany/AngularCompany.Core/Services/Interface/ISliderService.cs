

using AngularCompany.DataLayer.Entitys.Site;

namespace AngularCompany.Core.Services.Interface
{
    public interface ISliderService : IDisposable
    {
        Task<List<Slider>> GetAllSlider();
        Task<List<Slider>> GetActiveSlider();
        Task AddSlider(Slider slider);
        Task UpdateSlider(Slider slider);
        Task RemoveSlider(long Id);

    }
}
