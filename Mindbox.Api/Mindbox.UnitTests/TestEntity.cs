using System;
using System.Collections.Generic;
using System.Text;
using Mindbox.Api.Domain.Entities;

namespace Mindbox.UnitTests
{
    public class TestEntity : FigureBase
    {
        public override float GetArea()
        {
            return 3;
        }
    }
}
