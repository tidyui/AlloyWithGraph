using GraphTest.Models.Pages;
using GraphTest.Models.ViewModels;
using EPiServer.Framework.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using GraphQL;
using System.Net.WebSockets;
using GraphTest.Models.Graph;

namespace GraphTest.Controllers;

/// <summary>
/// Concrete controller that handles all page types that don't have their own specific controllers.
/// </summary>
/// <remarks>
/// Note that as the view file name is hard coded it won't work with DisplayModes (ie Index.mobile.cshtml).
/// For page types requiring such views add specific controllers for them. Alternatively the Index action
/// could be modified to set ControllerContext.RouteData.Values["controller"] to type name of the currentPage
/// argument. That may however have side effects.
/// </remarks>
[TemplateDescriptor(Inherited = true)]
public class DefaultPageController : PageControllerBase<SitePageData>
{
    public async Task<ViewResult> Index(SitePageData currentPage)
    {
        var model = CreateModel(currentPage);

        if (model is PageViewModel<ProductPage> productModel)
        {
            // Go fetch some data from GraphQL
            var graphQLClient = new GraphQLHttpClient(
                "https://cg.optimizely.com/content/v2?cache=true&stored=false",
                new NewtonsoftJsonSerializer());
            graphQLClient.HttpClient.DefaultRequestHeaders.Authorization =
                new("epi-single", "...");

            var graphRequest = new GraphQLRequest
            {
                Query = """
                {
                  _Image(where: { _metadata: { displayName: { contains: ".gif" } } }) {
                    items {
                      ... on ImageMedia {
                        _metadata {
                          key
                          displayName
                          url {
                            default
                          }
                        }
                      }
                    }
                  }
                }
                """
            };
            var graphResult = await graphQLClient.SendQueryAsync<ImageResponse>(graphRequest);
            if (graphResult != null && graphResult.Data != null)
            {
                productModel.Images = graphResult.Data.Image.Items;
            }
        }
        return View($"~/Views/{currentPage.GetOriginalType().Name}/Index.cshtml", model);
    }

    /// <summary>
    /// Creates a PageViewModel where the type parameter is the type of the page.
    /// </summary>
    /// <remarks>
    /// Used to create models of a specific type without the calling method having to know that type.
    /// </remarks>
    private static IPageViewModel<SitePageData> CreateModel(SitePageData page)
    {
        var type = typeof(PageViewModel<>).MakeGenericType(page.GetOriginalType());
        return Activator.CreateInstance(type, page) as IPageViewModel<SitePageData>;
    }
}
