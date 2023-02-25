﻿using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Utilities;
using Microsoft.AspNetCore.Authentication;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace MoneyManager_API.Extensions;

public static class ControllerExtensions
{
    public static IActionResult ValidationError(this ControllerBase controller, ValidationResult validationResult)
    {
        if (controller is null) throw new ArgumentNullException(nameof(controller));
        if (validationResult is null) throw new ArgumentNullException(nameof(validationResult));
        
        var problemDetailsFactory = controller.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();

        var modelStateDictionary = new ModelStateDictionary();
        foreach (var validationError in validationResult.Errors) modelStateDictionary.AddModelError(validationError.PropertyName, validationError.ErrorMessage);

        var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(controller.HttpContext, modelStateDictionary, StatusCodes.Status422UnprocessableEntity, "Your input could not fulfil some of our validation rules");
        return controller.UnprocessableEntity(problemDetails);
    }
    
    public static ActionResult Error(this ControllerBase controller, OperationResult operationResult)
    {
        if (controller is null) throw new ArgumentNullException(nameof(controller));
        if (operationResult is null) throw new ArgumentNullException(nameof(operationResult));

        var statusCode = operationResult.Errors.Any(e => e.IsNotExpected) ? StatusCodes.Status500InternalServerError : StatusCodes.Status400BadRequest;
        return controller.Problem(operationResult.ToString(), controller.Request.Path, statusCode, "Your actions was not executed successfully.");
    }

    public static string AbsoluteUrl(this ControllerBase controller, string actionName, string controllerName,
        object values)
    {
        if (controller is null) throw new ArgumentNullException(nameof(controller));

        var request = controller.HttpContext.Request;
        return controller.Url.Action(actionName, controllerName, values, request.Scheme, request.Host.Value);
    }
    
    public static async Task<Guid> GetUserId(this ControllerBase controller)
    {
        if (controller is null) throw new ArgumentNullException(nameof(controller));
        
        var token = await controller.HttpContext.GetTokenAsync("access_token");
        if(token is null)
            return Guid.Empty;
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        var userId = jwt.Claims.FirstOrDefault(u => u.Type == "unique_name").Value;

        if (userId is not null)
            return Guid.Parse(userId);
        else return Guid.Empty;
    }
}