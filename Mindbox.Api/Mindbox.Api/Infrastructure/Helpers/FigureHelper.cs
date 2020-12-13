using System.Collections.Generic;
using Mindbox.Api.Domain.Entities;
using Mindbox.Api.Infrastructure.Dto;

namespace Mindbox.Api.Infrastructure.Helpers
{
    public static class FigureHelper
    {
        public static DtoBase FindAppropriateDto(this FigureBase figure)
        {
            if (figure is Triangle triangle)
                return CreateTriangleDto(triangle);
            if (figure is Circle circle)
                return CreateCircleDto(circle);

            throw new KeyNotFoundException($"Dto для типа {figure.GetType()} не найдено");
        }

        private static CircleDto CreateCircleDto(Circle circle)
        {
            return new CircleDto($"{{\"Radius\":{circle.Radius}}}");
        }

        private static TriangleDto CreateTriangleDto(Triangle figure)
        {
            return new TriangleDto($"{{\"Sides\":[{figure.A},{figure.B},{figure.C}]}}");
        }
    }
}
