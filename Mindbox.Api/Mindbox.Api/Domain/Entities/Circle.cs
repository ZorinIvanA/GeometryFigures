using System;

namespace Mindbox.Api.Domain.Entities
{
    public class Circle : FigureBase
    {
        public float Radius { get; set; }

        public override float GetArea()
        {
            return (float)(Math.PI * Math.Pow(Radius, 2));
        }
    }
}
