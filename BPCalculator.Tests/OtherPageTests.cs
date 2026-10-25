using BPCalculator.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace BPCalculator.Tests
{
    public class OtherPageTests
    {
        [Fact]
        public void ErrorPage_OnGet_SetsRequestId()
        {
            var model = new ErrorModel(NullLogger<ErrorModel>.Instance)
            {
                PageContext = new() { HttpContext = new DefaultHttpContext { TraceIdentifier = "trace-123" } }
            };

            model.OnGet();

            Assert.False(string.IsNullOrEmpty(model.RequestId));
            Assert.True(model.ShowRequestId);
        }

        [Fact]
        public void ErrorPage_WithoutRequestId_HidesIt()
        {
            var model = new ErrorModel(NullLogger<ErrorModel>.Instance);

            Assert.False(model.ShowRequestId);
        }

        [Fact]
        public void PrivacyPage_OnGet_DoesNotThrow()
        {
            var model = new PrivacyModel(NullLogger<PrivacyModel>.Instance);

            var exception = Record.Exception(() => model.OnGet());

            Assert.Null(exception);
        }
    }
}
