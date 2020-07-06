namespace Structure.AspNetCore.Mvc
{
    public class MvcRouteOptions
    {
        public bool UseRouteVersioning { get; set; }
        public string RouteVersioningTemplate { get; set; } = "v{v:apiVersion}";
        public bool UseApiPrefix { get; set; }
        public string ApiPrefix { get; set; } = "api";
        public string RootPath { get; set; }
        public bool UseKebapCase { get; set; }
        
        public MvcRouteOptions()
        {
            UseApiPrefix = true;
        }
    }
}
