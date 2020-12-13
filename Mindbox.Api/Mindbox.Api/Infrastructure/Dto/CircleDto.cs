using Mindbox.Api.Domain.Entities;
using Newtonsoft.Json.Linq;

namespace Mindbox.Api.Infrastructure.Dto
{
    public class CircleDto : DtoBase
    {
        public override int Type => 2;
        public float Radius { get; private set; }

        public CircleDto(string data) : base(data)
        {
            Radius = JObject.Parse(data).SelectToken("$.Radius").Value<float>();
        }

        protected override FigureBase ToEntityCompleted()
        {
            return new Circle
            {
                Radius = Radius
            };
        }

    }
}
