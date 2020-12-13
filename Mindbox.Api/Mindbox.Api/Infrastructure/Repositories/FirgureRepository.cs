using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Mindbox.Api.Domain.Entities;
using Mindbox.Api.Domain.Interfaces;
using Mindbox.Api.Infrastructure.Dto;
using Mindbox.Api.Infrastructure.Helpers;

namespace Mindbox.Api.Infrastructure.Repositories
{
    public class FirgureRepository : IGeometryRepository
    {
        private const string DB_NAME = "DataSource=Database\\figures.db";
        public Task<long> SaveFigureAsync(FigureBase figure, CancellationToken cancellationToken)
        {
            var figureDto = figure.FindAppropriateDto();

            using (var connection = new SqliteConnection(DB_NAME))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = $"INSERT INTO Figures (Data, Type) VALUES ('{figureDto.Data}', {figureDto.Type}); SELECT last_insert_rowid() ";

                return Task.FromResult((long)command.ExecuteScalar());
            }
        }

        public Task<FigureBase> GetFigureAsync(int id, CancellationToken cancellationToken)
        {
            using (var connection = new SqliteConnection(DB_NAME))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = $"SELECT f.Id, ft.Name, f.Data FROM Figures f INNER JOIN FigureTypes ft ON f.Type=ft.Id WHERE f.Id={id}";

                using (var reader = command.ExecuteReader())
                {
                    DtoBase dto = null;

                    while (reader.Read())
                    {
                        var figureId = reader.GetInt32(0);
                        var type = reader.GetString(1);
                        var typeInAssembly = Type.GetType(type);
                        if (typeInAssembly == null)
                        {
                            Console.WriteLine($"Не найден тип {type}");
                            continue;
                        }

                        dto = Activator.CreateInstance(typeInAssembly) as DtoBase;
                        dto.Id = figureId;
                        dto.Data = reader.GetString(2);
                    }

                    return Task.FromResult(dto?.ToEntity());
                }
            }
        }
    }
}
