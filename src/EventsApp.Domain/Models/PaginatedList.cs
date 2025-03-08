namespace EventsApp.Domain.Models;

public class PaginatedList<T>
{
    /// <summary>
    /// Элементы текущей страницы
    /// </summary>
    public List<T> Items { get; }

    /// <summary>
    /// Текущая страница
    /// </summary>
    public int PageIndex { get; }
        
    /// <summary>
    /// Количество всех страниц
    /// </summary>
    public int TotalPages { get; }

    /// <summary>
    /// Свойство наличия следующей страницы
    /// </summary>
    public bool HasNextPage => PageIndex < TotalPages;

    /// <summary>
    /// Свойство наличия предыдущей страницы
    /// </summary>
    public bool HasPreviousPage => PageIndex > 1;

    public PaginatedList(List<T> items, int pageIndex, int totalPages)
    {
        Items = items;
        PageIndex = pageIndex;
        TotalPages = totalPages;
    }
}

