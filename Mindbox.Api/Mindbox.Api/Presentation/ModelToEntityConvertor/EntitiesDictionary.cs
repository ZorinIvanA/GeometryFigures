using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mindbox.Api.Domain.Entities;

namespace Mindbox.Api.Presentation.ModelToEntityConvertor
{
    public class EntitiesDictionary : Dictionary<string, Type>
    {
        private static EntitiesDictionary _dictionary;

        private EntitiesDictionary()
        {
            Add("triangle", typeof(Triangle));
            Add("circle", typeof(Circle));
        }

        public static EntitiesDictionary GetInstance()
        {
            return _dictionary ??= new EntitiesDictionary();
        }
    }
}
