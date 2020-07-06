using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Structure.AspNetCore.MultiTenancy;
using Structure.Collections.Extensions;
using Structure.Extensions;
using Structure.Helpers;
using Structure.MultiTenancy;
using Structure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Structure.AspNetCore.Mvc.Conventions
{
    public class DefaultRouteModelConvention : IRouteModelConvention
    {
        protected readonly string[] actionPrefixes = new string[] { "GetAll", "Get", "Put", "Update", "Patch", "Delete", "Remove", "Post", "Create", "Insert" };
        protected readonly MultiTenancyOptions multiTenancyOptions;
        protected readonly AspNetCoreOptions aspNetCoreOptions;
        protected readonly RouteTenantResolverOptions routeTenantResolverOptions;
        protected readonly IPluralizationService pluralizationService;
        private readonly ILogger<IRouteModelConvention> logger;

        public DefaultRouteModelConvention(
            IOptions<AspNetCoreOptions> aspNetCoreOptions,
            IOptions<MultiTenancyOptions> multiTenancyOptions,
            IOptions<RouteTenantResolverOptions> routeTenantResolverOptions,
            IPluralizationService pluralizationService,
            ILogger<IRouteModelConvention> logger)
        {
            this.multiTenancyOptions = multiTenancyOptions.Value;
            this.aspNetCoreOptions = aspNetCoreOptions.Value;
            this.routeTenantResolverOptions = routeTenantResolverOptions.Value;
            this.pluralizationService = pluralizationService;
            this.logger = logger;
        }

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                ConfigureController(controller);
            }
        }

        private void ConfigureController(ControllerModel controller)
        {
            ConfigureApiExplorer(controller);
            ConfigureSelector(controller);
            ConfigureParameters(controller);
        }

        private void ConfigureApiExplorer(ControllerModel controller)
        {
            if (controller.ApiExplorer.GroupName.IsNullOrEmpty())
            {
                controller.ApiExplorer.GroupName = controller.ControllerName;
            }

            if (controller.ApiExplorer.IsVisible == null)
            {
                controller.ApiExplorer.IsVisible = true;
            }
        }

        private void ConfigureSelector(ControllerModel controller)
        {
            RemoveEmptySelectors(controller.Selectors);

            var moduleName = GetModuleNameOrDefault(controller.ControllerType.AsType());
            var controllerName = GetControllerName(controller);

            foreach (var action in controller.Actions)
            {
                ConfigureSelector(moduleName, controllerName, action);
            }
        }

        protected virtual string GetControllerName(ControllerModel controller)
        {
            if (controller.RouteValues.ContainsKey("controller"))
            {
                return controller.RouteValues["controller"];
            }

            var controllerName = pluralizationService.Pluralize(controller.ControllerName);

            if (aspNetCoreOptions.Routes.UseKebapCase)
            {
                return controllerName.ToKebabCase();
            };

            return controllerName;
        }

        private void ConfigureSelector(string moduleName, string controllerName, ActionModel action)
        {
            RemoveEmptySelectors(action.Selectors);

            if (!action.Selectors.Any())
            {
                AddSelectorRoutes(moduleName, controllerName, action);
            }
            else
            {
                NormalizeSelectorRoutes(moduleName, controllerName, action);
            }
        }

        private void AddSelectorRoutes(string moduleName, string controllerName, ActionModel action)
        {
            var verb = GetConventionalVerbForMethodName(action.ActionName);
            var selectorModel = new SelectorModel { AttributeRouteModel = CreateAttributeRoute(moduleName, controllerName, verb, action) };
            selectorModel.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { verb }));
            action.Selectors.Add(selectorModel);
        }

        protected virtual string GetConventionalVerbForMethodName(string actionName)
        {
            if (actionName.StartsWithIgnoreCase("Get"))
            {
                return "GET";
            }

            if (actionName.StartsWithIgnoreCase("Put") ||
                actionName.StartsWithIgnoreCase("Update"))
            {
                return "PUT";
            }

            if (actionName.StartsWithIgnoreCase("Patch"))
            {
                return "PATCH";
            }

            if (actionName.StartsWithIgnoreCase("Delete") ||
                actionName.StartsWithIgnoreCase("Remove"))
            {
                return "DELETE";
            }

            if (actionName.StartsWithIgnoreCase("Post") ||
                actionName.StartsWithIgnoreCase("Create") ||
                actionName.StartsWithIgnoreCase("Insert"))
            {
                return "POST";
            }

            return "POST";
        }

        private void NormalizeSelectorRoutes(string moduleName, string controllerName, ActionModel action)
        {
            foreach (var selector in action.Selectors.Where(c => c.AttributeRouteModel == null))
            {
                var verb = GetConventionalVerbForMethodName(action.ActionName);
                selector.AttributeRouteModel = CreateAttributeRoute(moduleName, controllerName, verb, action);
            }
        }

        private void RemoveEmptySelectors(IList<SelectorModel> selectors)
        {
            selectors
                .Where(IsEmptySelector)
                .ToList()
                .ForEach(s => selectors.Remove(s));
        }

        private static bool IsEmptySelector(SelectorModel selector)
        {
            return selector.AttributeRouteModel == null && selector.ActionConstraints.IsNullOrEmpty();
        }

        private string GetModuleNameOrDefault(Type controllerType)
        {
            //TODO
            return null;
        }

        protected virtual AttributeRouteModel CreateAttributeRoute(string moduleName, string controllerName, string verb, ActionModel action)
        {
            var conventionActionName = GetConventionActionName(action, verb);
            var conventionRouteTemplate = GetConventionRouteTemplate(moduleName, controllerName, action, conventionActionName);
            return new AttributeRouteModel(new RouteAttribute(conventionRouteTemplate));
        }

        protected virtual string GetConventionRouteTemplate(string moduleName, string controllerName, ActionModel action, string routeActionName)
        {
            var mvcOptions = aspNetCoreOptions.Routes;

            return string.Empty
                .ConcatStringIf(mvcOptions.UseApiPrefix, mvcOptions.ApiPrefix)
                .ConcatStringIf(mvcOptions.UseRouteVersioning, $"/{mvcOptions.RouteVersioningTemplate}")
                .ConcatStringIf(!mvcOptions.RootPath.IsNullOrEmpty(), $"/{mvcOptions.RootPath}")
                .ConcatStringIf(multiTenancyOptions.IsEnabled, "/{" + routeTenantResolverOptions.ParameterName + "}")
                .ConcatStringIf(!moduleName.IsNullOrEmpty(), $"/{moduleName?.ToLower()}")
                .ConcatString($"/{controllerName.ToLower()}")
                .ConcatStringIf(action.Parameters.Any(c => c.Name == "id"), "/{id}")
                .ConcatStringIf(!routeActionName.IsNullOrEmpty(), $"/{routeActionName}");
        }

        protected virtual string GetConventionActionName(ActionModel action, string verb)
        {
            var actionName = action.ActionName;
            var prefixFounded = false;

            if (!action.Attributes.Any(c => c is ActionNameAttribute actionNameAttribute && !actionNameAttribute.Name.IsNullOrEmpty()))
            {
                foreach (var prefix in actionPrefixes)
                {
                    if (action.ActionName.StartsWithIgnoreCase(prefix))
                    {
                        actionName = action.ActionName.RemoveBeginning(prefix);
                        prefixFounded = true;
                        break;
                    }
                }

                if (!prefixFounded)
                {
                    actionName = action.ActionName;
                }

                if (!actionName.IsNullOrEmpty())
                {
                    actionName = actionName.RemoveFromEndIgnoreCase("Async");
                }

                if (aspNetCoreOptions.Routes.UseKebapCase && !actionName.IsNullOrEmpty())
                {
                    actionName = actionName.ToKebabCase();
                };
            }

            logger.LogDebug($"Controller: {action.Controller.ControllerName} / Verb: {verb} / Method: {action.ActionName} / Convention name: {actionName}");

            return actionName;
        }

        private void ConfigureParameters(ControllerModel controller)
        {
            foreach (var action in controller.Actions)
            {
                foreach (var param in action.Parameters)
                {
                    if (param.BindingInfo != null || param.Name == "id")
                    {
                        continue;
                    }

                    param.BindingInfo = BindingInfo.GetBindingInfo(new[] {
                        !TypeHelper.IsPrimitiveExtendedIncludingNullable(param.ParameterInfo.ParameterType) && CanUseFormBodyBinding(action, param) ?
                        new FromBodyAttribute() :
                        (Attribute)new FromQueryAttribute() });

                }
            }
        }

        private bool CanUseFormBodyBinding(ActionModel action, ParameterModel parameter)
        {
            foreach (var selector in action.Selectors)
            {
                if (selector.ActionConstraints == null)
                {
                    continue;
                }

                foreach (var actionConstraint in selector.ActionConstraints)
                {
                    if (!(actionConstraint is HttpMethodActionConstraint httpMethodActionConstraint))
                    {
                        continue;
                    }

                    if (httpMethodActionConstraint.HttpMethods.All(hm => hm.In("GET", "DELETE", "TRACE", "HEAD")))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}