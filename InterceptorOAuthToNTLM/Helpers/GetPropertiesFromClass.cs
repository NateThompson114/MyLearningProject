using InterceptorOAuthToNTLM.Models;

namespace InterceptorOAuthToNTLM.Helpers
{
    public static class GetPropertiesFromClass
    {
        public static List<RouteInformation> GetRouteInformation<T>() where T : new()
        {
            var instance = new T();
            var type = typeof(T);
            var properties = type.GetProperties();

            return properties.Select(p => new RouteInformation(p.Name, p.GetValue(instance)?.ToString() ?? "null")).ToList();
        }
    }
}
