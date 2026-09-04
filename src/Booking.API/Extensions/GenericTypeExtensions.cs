namespace Booking.API.Extensions;

public static class GenericTypeExtensions
{
    public static string GetGenericTypeName(this Type type)
    {
        if (!type.IsGenericType)
        {
            return type.Name;
        }

        var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name));
        return $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
    }

    public static string GetGenericTypeName(this object @object) => @object.GetType().GetGenericTypeName();
}
