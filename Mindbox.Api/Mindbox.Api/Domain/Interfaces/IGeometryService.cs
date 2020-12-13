using System.Threading;
using System.Threading.Tasks;
using Mindbox.Api.Domain.Entities;

namespace Mindbox.Api.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс для бизнес-логики
    /// </summary>
    public interface IGeometryService
    {
        /// <summary>
        /// Ищет фигуру по её id
        /// </summary>
        /// <param name="id">Id фигуры в БД</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Найденная фигура</returns>
        Task<FigureBase> FindFigureAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Сохраняет фигуру в БД
        /// </summary>
        /// <param name="figure">Фигура, которую необходимо сохранить</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        Task<long> SaveFigureAsync(FigureBase figure, CancellationToken cancellationToken);
    }
}
