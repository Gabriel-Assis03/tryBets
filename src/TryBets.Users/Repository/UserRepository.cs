using TryBets.Users.Models;
using TryBets.Users.DTO;

namespace TryBets.Users.Repository;

public class UserRepository : IUserRepository
{
    protected readonly ITryBetsContext _context;
    public UserRepository(ITryBetsContext context)
    {
        _context = context;
    }

    public User Post(User user)
    {
        var validNewUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
        if (validNewUser != null) return null!;
        var newUser = _context.Users.Add(user).Entity;
        _context.SaveChanges();
        return user;
    }
    public User Login(AuthDTORequest login)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == login.Email);
        if (user == null || user.Password != login.Password) return null!;
        return new User{
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
        };
    }
}