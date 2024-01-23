using AngularCompany.Core.Services.Interface;
using AngularCompany.Core.Utilities.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularCompany.WebApi.Controllers
{
   
    public class SlidersController : SiteBaseController
    {
        #region constractor
        private ISliderService SliderService;

        public SlidersController(ISliderService sliderService)
        {
            this.SliderService = sliderService;
        }
        #endregion
        [HttpGet(template: "Sliders")]
        public async Task<IActionResult> Sliders()
        {
            var resault = await SliderService.GetAllSlider();
            return JsonResponseStatus.Success(resault);
        }
        [HttpGet(template: "GetActiveSlider")]
        public async Task<IActionResult> GetActiveSlider()
        {
            var resault = await SliderService.GetActiveSlider();
            return JsonResponseStatus.Success(resault);
        }
    }
}
