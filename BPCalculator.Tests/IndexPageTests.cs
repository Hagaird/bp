using BPCalculator.Pages;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Xunit;

namespace BPCalculator.Tests
{
    public class IndexPageTests
    {
        [Fact]
        public void OnGet_SetsDefaultReading()
        {
            var model = new BloodPressureModel();

            model.OnGet();

            Assert.Equal(100, model.BP.Systolic);
            Assert.Equal(60, model.BP.Diastolic);
        }

        [Fact]
        public void OnPost_ValidReading_HasNoErrors()
        {
            var model = new BloodPressureModel { BP = new BloodPressure { Systolic = 120, Diastolic = 80 } };

            var result = model.OnPost();

            Assert.IsType<PageResult>(result);
            Assert.True(model.ModelState.IsValid);
        }

        [Theory]
        [InlineData(80, 80)]
        [InlineData(80, 90)]
        public void OnPost_SystolicNotGreaterThanDiastolic_AddsError(int systolic, int diastolic)
        {
            var model = new BloodPressureModel { BP = new BloodPressure { Systolic = systolic, Diastolic = diastolic } };

            model.OnPost();

            Assert.False(model.ModelState.IsValid);
            Assert.Equal("Systolic must be greater than Diastolic", model.ModelState[""].Errors[0].ErrorMessage);
        }
    }
}
