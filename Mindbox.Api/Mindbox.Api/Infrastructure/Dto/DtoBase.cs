using System;
using Mindbox.Api.Domain.Entities;

namespace Mindbox.Api.Infrastructure.Dto
{
    public abstract class DtoBase
    {
        public int Id { get; set; }
        public abstract int Type { get; }

        public string Data { get; set; }

        public DtoBase(string data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public virtual FigureBase ToEntity()
        {
            var entity = ToEntityCompleted();//Задействуем TemplateMethod для сохранения общей механики - Id
            entity.Id = Id;

            return entity;
        }
        protected abstract FigureBase ToEntityCompleted();
    }
}
