using Microsoft.EntityFrameworkCore;

namespace spitifi.Models;

public class Page<T> : List<T>
{
    
    /// <summary>
    /// Numero da pagina
    ///
    /// exemplo: indice 7 => pagina 7 de x paginas totais 
    /// </summary>
    public int PageIndex { get; set; }
    
    /// <summary>
    /// Quantidade de coisas em cada pagina
    /// 
    /// Cada pagina está declarada com uma wildcard, podem ser criadas
    /// paginas para varios objetos, como musicas, albuns...
    /// </summary>
    public int PageSize { get; set; }
    
    /// <summary>
    /// numero total de páginas
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Pagina contem pagina precedente?
    ///
    /// Falso -> Pagina tem indice 1
    /// Verdadeiro -> Indice != 1
    ///
    /// Proposito: facilidade de utilização. Evita ter de fazer comparações na UI
    /// </summary>
    public bool HasPreviousPage => PageIndex > 1;
    
    /// <summary>
    /// Existem mais paginas apos esta?
    ///
    /// Falso -> Ultima pagina
    /// Verdadeiro -> Não é a ultima pagina. Existe mais informação apos esta pagina
    ///
    /// Proposito: facilidade de utilização. Evita ter de fazer comparações na UI
    /// </summary>
    public bool HasNextPage => PageIndex < TotalPages;
    
    public Page(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageSize = pageSize == 0 ? 10 : pageSize;
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        this.AddRange(items);
    }

    public static async Task<Page<T>> CreateAsync(
        IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return new Page<T>(items, count, pageIndex, pageSize);
    }
}