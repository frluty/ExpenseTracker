using TT.Domain.Entities;

namespace TT.Application.UserItems.Dtos;
public record UserDto(int Id, string FirstName, string LastName, string Currency, DateTime Created)
{
    public static UserDto FromUserEntity( User user) =>
        new(user.Id, user.FirstName!, user.LastName!, user.Currency!, user.Created);
}