using System.Threading;
using System.Threading.Tasks;
using Mindbox.Api.Domain.Entities;

namespace Mindbox.Api.Domain.Interfaces
{
    /// <summary>
    /// Репозиторий для взаимодействия с БД
    /// </summary>
    public interface IGeometryRepository
    {
        /// <summary>
        /// Сохраняет фигуру в БД
        /// </summary>
        /// <param name="figure">Фигура для сохранения</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Id сохранённой фигуры</returns>
        Task<long> SaveFigureAsync(FigureBase figure, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает из БД фигуру по её guid
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Фигура</returns>
        Task<FigureBase> GetFigureAsync(int id, CancellationToken cancellationToken);
    }
}
