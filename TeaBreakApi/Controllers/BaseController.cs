using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using TeaBreakApi.Domain;
using System.Net;

namespace TeaBreakApi.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected new IActionResult Ok<T>(T result = null) where T : class
        {
            return new EnvelopeResult<T>(Envelope<T>.Ok(result), HttpStatusCode.OK);
        }

        protected IActionResult NotFound<T>(Error error, string invalidField = null) where T : class
        {
            return new EnvelopeResult<T>(Envelope<T>.Error<T>(error, invalidField), HttpStatusCode.NotFound);
        }

        protected IActionResult Error<T>(Error error, string invalidField = null) where T : class
        {
            return new EnvelopeResult<T>(Envelope<T>.Error<T>(error, invalidField), HttpStatusCode.BadRequest);
        }

        protected IActionResult FromResult<T>(Result<T, Error> result) where T : class
        {
            if (result.IsSuccess)
                return Ok();

            return Error<T>(result.Error);
        }
    }

    public sealed class EnvelopeResult<T> : IActionResult where T : class
    {
        private readonly Envelope<T> _envelope;
        private readonly int _statusCode;

        public EnvelopeResult(Envelope<T> envelope, HttpStatusCode statusCode)
        {
            _statusCode = (int)statusCode;
            _envelope = envelope;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(_envelope)
            {
                StatusCode = _statusCode
            };

            return objectResult.ExecuteResultAsync(context);
        }
    }

    //public class Envelope
    //{
    //    public T Result { get; }
    //    public string ErrorCode { get; }
    //    public string ErrorMessage { get; }
    //    public string InvalidField { get; }
    //    public DateTime TimeGenerated { get; }

    //    private Envelope(T result, Error error, string invalidField)
    //    {
    //        Result = result;
    //        ErrorCode = error?.Code;
    //        ErrorMessage = error?.Message;
    //        InvalidField = invalidField;
    //        TimeGenerated = DateTime.UtcNow;
    //    }

    //    public static Envelope<T> Ok<T>(T result = null) where T : class
    //    {
    //        return new Envelope<T>(result, null, null);
    //    }

    //    public static Envelope<T> Error<T>(Error error, string invalidField) where T : class
    //    {
    //        return new Envelope<T>(null, error, invalidField);
    //    }
    //}

    public class Envelope<T>
    {
        public T Result { get; }
        public string ErrorCode { get; }
        public string ErrorMessage { get; }
        public string InvalidField { get; }
        public DateTime TimeGenerated { get; }

        private Envelope(T result, Error error, string invalidField)
        {
            Result = result;
            ErrorCode = error?.Code;
            ErrorMessage = error?.Message;
            InvalidField = invalidField;
            TimeGenerated = DateTime.UtcNow;
        }

        public static Envelope<T> Ok<T>(T result = null) where T : class
        {
            return new Envelope<T>(result, null, null);
        }

        public static Envelope<T> Error<T>(Error error, string invalidField) where T : class
        {
            return new Envelope<T>(null, error, invalidField);
        }
    }
}
