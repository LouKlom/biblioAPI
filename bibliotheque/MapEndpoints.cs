using bibliotheque.Endpoints;

namespace bibliotheque;

public static class MapEndpoints
{
    public static IEndpointRouteBuilder MapEndpoint(this IEndpointRouteBuilder group)
    {
        group.MapClient();
        return group;
    }
}
