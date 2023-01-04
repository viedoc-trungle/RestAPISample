using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using TeaBreakApi.Controllers;
using System.Net;
using TeaBreakApi.Domain;
using FluentValidation;
using CSharpFunctionalExtensions;
using TeaBreakApi.Controllers.TeaBreaks.v3;
using TeaBreakApi.Data;

namespace TeaBreakApi
{
    public class ModelStateValidator
    {
        public static IActionResult ValidateModelState(ActionContext context)
        {
            (string fieldName, ModelStateEntry entry) = context.ModelState.First(x => x.Value.Errors.Count > 0);
            string errorSerialized = entry.Errors.First().ErrorMessage;

            Error error = Error.Deserialize(errorSerialized);
            var envelope = Envelope<object>.Error<object>(error, fieldName);
            var envelopeResult = new EnvelopeResult<object>(envelope, HttpStatusCode.BadRequest);

            return envelopeResult;
        }
    }

    public class TeaBreakRequestValidator : AbstractValidator<TeaBreakRequest>
    {
        public TeaBreakRequestValidator()
        {
            RuleFor(x => x.StartTime).GreaterThan(DateTime.Now).WithMessage("You can't schedule a teabreak with negative time");
            RuleFor(x => x.StartTime).LessThan(x => x.EndTime).WithMessage("You can't schedule a teabreak with negative time period");
        }
    }

    public class OrderRequestValidator : AbstractValidator<OrderRequest>
    {
        public OrderRequestValidator(ProviderRepository providerRepository)
        {
            var providers = providerRepository.GetAll();
            RuleFor(x => x.Provider)
                .Must(x => providers.Any(p => p.Id.Equals(x)))
                .WithMessage("Unknown provider");

            RuleFor(x => x)
                .Must(x => providers.Any(p => p.Id.Equals(x.Provider) && p.Products.Any(pd => pd.Id.Equals(x.Product))))
                .WithMessage("Provider doesn't provide the specified product");
        }
    }

    public static class CustomValidators
    {
        public static IRuleBuilderOptions<T, TProperty> NotEmpty<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            return DefaultValidatorExtensions.NotEmpty(ruleBuilder)
                .WithMessage(Errors.General.ValueIsRequired().Serialize());
        }

        public static IRuleBuilderOptions<T, string> Length<T>(this IRuleBuilder<T, string> ruleBuilder, int min, int max)
        {
            return DefaultValidatorExtensions.Length(ruleBuilder, min, max)
                .WithMessage(Errors.General.InvalidLength().Serialize());
        }

        public static IRuleBuilderOptions<T, TElement> MustBeEntity<T, TElement, TValueObject>(
            this IRuleBuilder<T, TElement> ruleBuilder,
            Func<TElement, Result<TValueObject, Error>> factoryMethod)
            where TValueObject : Entity
        {
            return (IRuleBuilderOptions<T, TElement>)ruleBuilder.Custom((value, context) =>
            {
                Result<TValueObject, Error> result = factoryMethod(value);

                if (result.IsFailure)
                {
                    context.AddFailure(result.Error.Serialize());
                }
            });
        }

        public static IRuleBuilderOptions<T, string> MustBeValueObject<T, TValueObject>(
            this IRuleBuilder<T, string> ruleBuilder,
            Func<string, Result<TValueObject, Error>> factoryMethod)
            where TValueObject : ValueObject
        {
            return (IRuleBuilderOptions<T, string>)ruleBuilder.Custom((value, context) =>
            {
                Result<TValueObject, Error> result = factoryMethod(value);

                if (result.IsFailure)
                {
                    context.AddFailure(result.Error.Serialize());
                }
            });
        }

        public static IRuleBuilderOptionsConditions<T, IList<TElement>> ListMustContainNumberOfItems<T, TElement>(
            this IRuleBuilder<T, IList<TElement>> ruleBuilder, int? min = null, int? max = null)
        {
            return ruleBuilder.Custom((list, context) =>
            {
                if (min.HasValue && list.Count < min.Value)
                {
                    context.AddFailure(Errors.General.CollectionIsTooSmall(min.Value, list.Count).Serialize());
                }

                if (max.HasValue && list.Count > max.Value)
                {
                    context.AddFailure(Errors.General.CollectionIsTooLarge(max.Value, list.Count).Serialize());
                }
            });
        }
    }
}
