using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mindbox.Api.Domain.Entities;
using Mindbox.Api.Domain.Exceptions;
using Mindbox.Api.Domain.Interfaces;
using Mindbox.Api.Presentation.Models;
using Mindbox.Api.Presentation.ModelToEntityConvertor;
using Newtonsoft.Json.Linq;

namespace Mindbox.Api.Presentation
{
    [Route("api/figure")]
    [ApiController]
    public class GeometryController : ControllerBase
    {
        const string  TYPE_KEYWORD="figureType";
        private const string DATA_KEYWORD = "data";

        private readonly IGeometryService _service;

        public GeometryController(IGeometryService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpPost]
        public async Task<IActionResult> CreateFigure([FromBody] JsonElement content, CancellationToken cancellationToken)
        {
            if (!content.TryGetProperty(TYPE_KEYWORD, out var type))
                return BadRequest(new ProblemDetails
                {
                    Detail = "Не переданы данные о фигуре",
                    Instance = "POST figure/",
                    Title = "Ошибка переданных данных",
                    Status = 400
                });

            var entity = ModelToEntity(content);
            if (entity == null)
                return BadRequest(new ProblemDetails
                {
                    Detail = "Такой тип фигуры не найден",
                    Instance = "POST figure/",
                    Title = "Ошибка переданных данных",
                    Status = 400
                });
            

            return await ProcessCreationResult(_service.SaveFigureAsync, entity, cancellationToken);
        }

        private FigureBase ModelToEntity(JsonElement jObject)
        {
            try
            {
                var typeToCreate = EntitiesDictionary.GetInstance()[jObject.GetProperty(TYPE_KEYWORD).GetString()];
                var data = jObject.GetProperty(DATA_KEYWORD).GetRawText();
                return JsonSerializer.Deserialize(data, typeToCreate) as FigureBase;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFigureArea([FromRoute] int id, CancellationToken cancellationToken)
        {
            return await ProcessResult(async () =>
            {
                var figureToFind = await _service.FindFigureAsync(id, cancellationToken);
                if (figureToFind == null)
                    throw new BusinessValidationException("Ошибка получения фигуры", $"Фигура с ID {id} не найдена");

                return figureToFind.GetArea();
            }, nameof(GetFigureArea));
        }

        protected async Task<IActionResult> ProcessCreationResult<T, TInput, CancellationToken>(
            Func<TInput, CancellationToken, Task<T>> action,
            TInput input, CancellationToken cancellationToken, [CallerMemberName] string instance = "nothing")
        {
            if (action == null || string.IsNullOrWhiteSpace(instance))
                return StatusCode(StatusCodes.Status500InternalServerError);

            try
            {
                return StatusCode(StatusCodes.Status201Created, await action.Invoke(input, cancellationToken));
            }
            catch (BusinessValidationException e)
            {
                return GetBusinessError(instance, e);
            }
            catch (Exception e)
            {
                return GetInternalError(instance, e);
            }
        }


        protected async Task<IActionResult> ProcessResult<T, TInput, CancellationToken>(Func<TInput, CancellationToken, Task<T>> action,
            TInput input, CancellationToken cancellationToken, [CallerMemberName] string instance = "nothing")
        {
            if (action == null || string.IsNullOrWhiteSpace(instance))
                return StatusCode(StatusCodes.Status500InternalServerError);

            try
            {
                return Ok(await action.Invoke(input, cancellationToken));
            }
            catch (BusinessValidationException e)
            {
                return GetBusinessError(instance, e);
            }
            catch (Exception e)
            {
                return GetInternalError(instance, e);
            }
        }

        protected async Task<IActionResult> ProcessResult<T>(Func<Task<T>> action, [CallerMemberName] string instance = "nothing")
        {
            if (action == null || string.IsNullOrWhiteSpace(instance))
                return StatusCode(StatusCodes.Status500InternalServerError);

            try
            {
                return Ok(await action.Invoke());
            }
            catch (BusinessValidationException e)
            {
                return GetBusinessError(instance, e);
            }
            catch (Exception e)
            {
                return GetInternalError(instance, e);
            }
        }

        private static IActionResult GetInternalError(string instance, Exception e)
        {
            return new ObjectResult(new ProblemDetails
            {
                Title = "Неизвестная ошибка",
                Detail = e.Message,
                Instance = instance,
                Status = StatusCodes.Status500InternalServerError
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        private IActionResult GetBusinessError(string instance, BusinessValidationException e)
        {
            return BadRequest(new ProblemDetails
            {
                Title = e.Title,
                Detail = e.Message,
                Instance = instance
            });
        }
    }
}
