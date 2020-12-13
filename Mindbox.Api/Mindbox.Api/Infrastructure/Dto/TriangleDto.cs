using System;
using System.Linq;
using Mindbox.Api.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace Mindbox.Api.Infrastructure.Dto
{
    public class TriangleDto : DtoBase
    {
        public override int Type => 1;


        public TriangleDto(string data) : base(data)
        {
        }

        protected override FigureBase ToEntityCompleted()
        {
            var sides = JObject.Parse(Data).SelectTokens("$.Sides").Values<float>().ToArray();

            if (sides == null || sides.Length == 0 || sides.Length > 3 || sides.Any(x => x <= 0))
                throw new ArgumentOutOfRangeException(nameof(sides), "sides должен содержать три значения, каждое из которых больше 0");

            return new Triangle
            {
                A = sides[0],
                B = sides[1],
                C = sides[2]
            };
        }

    }
}
