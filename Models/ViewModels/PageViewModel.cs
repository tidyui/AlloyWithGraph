using GraphTest.Models.Graph;
using GraphTest.Models.Pages;

namespace GraphTest.Models.ViewModels;

public class PageViewModel<T> : IPageViewModel<T> where T : SitePageData
{
    public PageViewModel(T currentPage)
    {
        CurrentPage = currentPage;
    }

    public T CurrentPage { get; private set; }

    public LayoutModel Layout { get; set; }

    /// <summary>
    /// These will be fetched directly from Graph
    /// </summary>
    public virtual IList<ArticleContract> Articles { get; set; }

    public IContent Section { get; set; }
}

public static class PageViewModel
{
    /// <summary>
    /// Returns a PageViewModel of type <typeparam name="T"/>.
    /// </summary>
    /// <remarks>
    /// Convenience method for creating PageViewModels without having to specify the type as methods can use type inference while constructors cannot.
    /// </remarks>
    public static PageViewModel<T> Create<T>(T page)
        where T : SitePageData => new(page);
}
