using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace BPCalculator.Tests
{
    public class BloodPressureTests
    {
        [Theory]
        // low: systolic below 90 and diastolic below 60
        [InlineData(70, 40, BPCategory.Low)]
        [InlineData(89, 59, BPCategory.Low)]
        // ideal: lower limits of 90 systolic or 60 diastolic are inclusive
        [InlineData(90, 40, BPCategory.Ideal)]
        [InlineData(70, 60, BPCategory.Ideal)]
        [InlineData(119, 79, BPCategory.Ideal)]
        // pre-high: lower limits of 120 systolic or 80 diastolic are inclusive
        [InlineData(120, 40, BPCategory.PreHigh)]
        [InlineData(100, 80, BPCategory.PreHigh)]
        [InlineData(139, 89, BPCategory.PreHigh)]
        // high: lower limits of 140 systolic or 90 diastolic are inclusive
        [InlineData(140, 40, BPCategory.High)]
        [InlineData(100, 90, BPCategory.High)]
        [InlineData(190, 100, BPCategory.High)]
        public void Category_ReturnsExpectedCategory(int systolic, int diastolic, BPCategory expected)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };

            Assert.Equal(expected, bp.Category);
        }

        [Theory]
        [InlineData(70, 40)]
        [InlineData(190, 100)]
        public void Validation_AcceptsValuesInRange(int systolic, int diastolic)
        {
            Assert.Empty(Validate(new BloodPressure { Systolic = systolic, Diastolic = diastolic }));
        }

        [Theory]
        [InlineData(69, 60, "Invalid Systolic Value")]
        [InlineData(191, 60, "Invalid Systolic Value")]
        [InlineData(120, 39, "Invalid Diastolic Value")]
        [InlineData(120, 101, "Invalid Diastolic Value")]
        public void Validation_RejectsValuesOutOfRange(int systolic, int diastolic, string expectedError)
        {
            var results = Validate(new BloodPressure { Systolic = systolic, Diastolic = diastolic });

            Assert.Contains(results, r => r.ErrorMessage == expectedError);
        }

        private static List<ValidationResult> Validate(BloodPressure bp)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(bp, new ValidationContext(bp), results, validateAllProperties: true);
            return results;
        }
    }
}
