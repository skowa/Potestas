using Potestas.Web.Models;

namespace Potestas.Web.Mappers
{
	internal static class CoordinatesMapper
	{
		internal static Coordinates ToCoordinates(this CoordinatesModel coordinatesDto)
		{
			return new Coordinates(coordinatesDto.X, coordinatesDto.Y);
		}

		internal static CoordinatesModel ToCoordinatesModel(this Coordinates coordinates)
		{
			return new CoordinatesModel
			{
				X = coordinates.X,
				Y = coordinates.Y
			};
		}
	}
}