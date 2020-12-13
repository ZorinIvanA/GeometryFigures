using System;

namespace Mindbox.Api.Domain.Exceptions
{
    public class BusinessValidationException : Exception
    {
        public BusinessValidationException(string title, string message) : base(message)
        {
            Title = (string.IsNullOrWhiteSpace(title)) ? title : "Неизвестная ошибка проверки данных";
        }

        public string Title { get; set; }
    }
}
