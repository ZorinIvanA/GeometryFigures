using System;

namespace Mindbox.Api.Domain.Entities
{
    public class Triangle : FigureBase
    {
        public float A { get; set; }
        public float B { get; set; }
        public float C { get; set; }

        public override float GetArea()
        {
            var p = (A + B + C) / 2;
            return (float)Math.Sqrt(p * (p - A) * (p - B) * (p - C));
        }
    }
}
