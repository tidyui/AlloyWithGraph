# Alloy MVC example with Graph data

This example uses the standard Alloy MVC template with an additional Graph query for when a 
ProductPage is loaded. **Please note** that this is a very simplistic example of how to
perform a GraphQL query and render the result, it does not represent any best practices.

## Additional NuGet packages

The following NuGet-packages were added to the solution, but any GraphQL client will work.

* https://www.nuget.org/packages/GraphQL.Client
* https://www.nuget.org/packages/GraphQL.Client.Serializer.Newtonsoft

## Additional models

The following models were added to deserialze the result from the GraphQL query.

* `Models/Graph/GraphModels.cs`

## Modified files

The following files were modified to perform the Graph query and add the result to the view model.

* `Controllers/DefaultPageController.cs`
* `Models/ViewModels/PageViewModel.cs`

## Query

This is what the query looks like in `DefaultPageController.cs`

~~~csharp
var graphQLClient = new GraphQLHttpClient(
    "https://cg.optimizely.com/content/v2?cache=true&stored=false",
    new NewtonsoftJsonSerializer());
graphQLClient.HttpClient.DefaultRequestHeaders.Authorization =
    new("epi-single", "...");

var articleRequest = new GraphQLRequest
{
    Query = """
    {
        ArticleContract {
            items {
            Excerpt
            _metadata {
                key
            }
            }
        }
    }
    """
};
var articleResult = await graphQLClient.SendQueryAsync<ArticleContractResponse>(articleRequest);
~~~