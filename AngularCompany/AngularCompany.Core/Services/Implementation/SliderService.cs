
using AngularCompany.Core.Services.Interface;
using AngularCompany.DataLayer.Entitys.Site;
using AngularCompany.DataLayer.Repository;
using Microsoft.EntityFrameworkCore;

namespace AngularCompany.Core.Services.Implementation
{
    public class SliderService : ISliderService
    {
        #region constractor
        private IGenericRepository<Slider> sliderRepository;

        public SliderService(IGenericRepository<Slider> sliderRepository)
        {
            this.sliderRepository = sliderRepository;
        }
        #endregion
        public async Task AddSlider(Slider slider)
        {
            await sliderRepository.AddEntity(slider);
            await sliderRepository.SaveChenges();
        }

        public async Task<List<Slider>> GetAllSlider()
        {
            return await sliderRepository.GetEntityQuerys().ToListAsync();

        }
        public async Task<List<Slider>> GetActiveSlider()
        {
            return await sliderRepository.GetEntityQuerys().Where(t => !t.IsDeleted).ToListAsync();
        }
        public async Task RemoveSlider(long Id)
        {
            await sliderRepository.RemoveEntity(Id);
            await sliderRepository.SaveChenges();
        }

        public async Task UpdateSlider(Slider slider)
        {
            sliderRepository.UpdateEntity(slider);
            await sliderRepository.SaveChenges();
        }
        public void Dispose()
        {
            sliderRepository?.Dispose();
        }
    }
}
