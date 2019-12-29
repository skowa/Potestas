using Potestas.WebPlugin.Entities;

namespace Potestas.WebPlugin.Mappers
{
	internal static class CoordinatesMapper
	{
		internal static Coordinates ToCoordinates(this CoordinatesDTO coordinatesDto)
		{
			return new Coordinates(coordinatesDto.X, coordinatesDto.Y);
		}

		internal static CoordinatesDTO ToCoordinatesDto(this Coordinates coordinates)
		{
			return new CoordinatesDTO
			{
				X = coordinates.X,
				Y = coordinates.Y
			};
		}
	}
}