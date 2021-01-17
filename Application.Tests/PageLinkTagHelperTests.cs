using Application.Helpers;
using Application.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests
{
    public class PageLinkTagHelperTests
    {
        [Fact]
        public void CanGeneratePageLinks()
        {
            //Arrange - Mock UrlHelper
            Mock<IUrlHelper> urlHelper = new Mock<IUrlHelper>();
            urlHelper.SetupSequence(h => h.Action(It.IsAny<UrlActionContext>()))
                .Returns("Test/Page1")
                .Returns("Test/Page2")
                .Returns("Test/Page3");
            //Arrange - Mock UrlHelperFactory
            Mock<IUrlHelperFactory> urlHelperFactory = new Mock<IUrlHelperFactory>();
            urlHelperFactory.Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>())).Returns(urlHelper.Object);
            //Arrange - PageLinkTagHelper
            PageLinkTagHelper helper = new PageLinkTagHelper(urlHelperFactory.Object)
            {
                PageModel = new PagingInfo
                {
                    CurrentPage = 2,
                    TotalItems = 28,
                    ItemsPerPage = 10
                },
                PageAction = "Test"
            };
            //Arrange - TagHelperContext
            TagHelperContext context = new TagHelperContext(new TagHelperAttributeList(), new Dictionary<object, object>(), "");
            //Arrange - Mock TagHelperContent
            Mock<TagHelperContent> content = new Mock<TagHelperContent>();
            TagHelperOutput output = new TagHelperOutput("div", new TagHelperAttributeList(), (cache, encoder) => Task.FromResult(content.Object));

            //Act
            helper.Process(context, output);

            //Assert
            Assert.Equal(@"<a href=""Test/Page1"">1</a><a href=""Test/Page2"">2</a><a href=""Test/Page3"">3</a>", output.Content.GetContent());
        }
    }
}
