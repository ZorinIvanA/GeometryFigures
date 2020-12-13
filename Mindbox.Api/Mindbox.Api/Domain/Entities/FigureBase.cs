namespace Mindbox.Api.Domain.Entities
{
    public abstract class FigureBase
    {
        public int Id { get; set; }
        public abstract float GetArea();
    }
}
