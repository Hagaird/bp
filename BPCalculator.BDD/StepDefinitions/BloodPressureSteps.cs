using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using BPCalculator.Pages;
using Reqnroll;
using Xunit;

namespace BPCalculator.BDD.StepDefinitions
{
    [Binding]
    public class BloodPressureSteps
    {
        private readonly BloodPressure _bp = new BloodPressure();
        private readonly List<string> _errors = new List<string>();
        private BPCategory _category;

        [Given("a systolic pressure of {int}")]
        public void GivenASystolicPressureOf(int systolic)
        {
            _bp.Systolic = systolic;
        }

        [Given("a diastolic pressure of {int}")]
        public void GivenADiastolicPressureOf(int diastolic)
        {
            _bp.Diastolic = diastolic;
        }

        [When("the blood pressure category is calculated")]
        public void WhenTheCategoryIsCalculated()
        {
            _category = _bp.Category;
        }

        [When("the reading is submitted")]
        public void WhenTheReadingIsSubmitted()
        {
            var model = new BloodPressureModel { BP = _bp };
            model.OnPost();
            _errors.AddRange(model.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        }

        [When("the reading is validated")]
        public void WhenTheReadingIsValidated()
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(_bp, new ValidationContext(_bp), results, validateAllProperties: true);
            _errors.AddRange(results.Select(r => r.ErrorMessage));
        }

        [Then("the category should be {string}")]
        public void ThenTheCategoryShouldBe(string expected)
        {
            Assert.Equal(expected, DisplayName(_category));
        }

        [Then("the error {string} should be shown")]
        public void ThenTheErrorShouldBeShown(string expected)
        {
            Assert.Contains(expected, _errors);
        }

        // the category text shown to the user comes from the enum's Display attribute
        private static string DisplayName(BPCategory category)
        {
            return typeof(BPCategory).GetField(category.ToString())
                .GetCustomAttribute<DisplayAttribute>().Name;
        }
    }
}
