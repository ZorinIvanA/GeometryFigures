using System;
using System.Threading;
using System.Threading.Tasks;
using Mindbox.Api.Domain.Entities;
using Mindbox.Api.Domain.Interfaces;

namespace Mindbox.Api.Domain
{
    public class GeometryService : IGeometryService
    {
        public IGeometryRepository _repository { get; set; }
        public GeometryService(IGeometryRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Task<FigureBase> FindFigureAsync(int id, CancellationToken cancellationToken)
        {
            return _repository.GetFigureAsync(id, cancellationToken);
        }

        public Task<long> SaveFigureAsync(FigureBase figure, CancellationToken cancellationToken)
        {
            if (figure == null)
                return Task.FromException<long>(new ArgumentNullException(nameof(figure)));

            return _repository.SaveFigureAsync(figure, cancellationToken);
        }
    }
}
