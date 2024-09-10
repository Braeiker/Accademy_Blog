using Blog.Dto;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Service
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        private async Task<bool> Save()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Restituisci tutti gli utenti
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            return await _context.Users
                .Select(u => new UserDto { Id = u.Id, Name = u.Name })
                .ToListAsync();
        }

        // Restituisci utente per nome
        public async Task<UserDto> GetUserByNameAsync(string name)
        {
            return await _context.Users
                .Where(u => u.Name == name)
                .Select(u => new UserDto { Id = u.Id, Name = u.Name })
                .FirstOrDefaultAsync();
        }

        // Creare un nuovo utente
        public async Task<bool> CreateUserAsync(UserDto newUserDto)
        {
            if (newUserDto == null)
            {
                throw new ArgumentNullException(nameof(newUserDto));
            }

            var newUser = new User
            {
                Name = newUserDto.Name,
                Email = newUserDto.Email,
                Age = newUserDto.Age
            };

            _context.Users.Add(newUser);
            return await Save();
        }

        // Aggiornare un utente esistente
        public async Task<bool> UpdateUserAsync(string name, UserDto updatedUserDto)
        {
            if (updatedUserDto == null)
            {
                throw new ArgumentNullException(nameof(updatedUserDto));
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Name == name);
            if (existingUser == null)
            {
                return false;
            }

            existingUser.Name = updatedUserDto.Name;
            existingUser.Email = updatedUserDto.Email;
            existingUser.Age = updatedUserDto.Age;

            _context.Users.Update(existingUser);
            return await Save();
        }


        // Eliminare un utente esistente
        public async Task<bool> DeleteUserAsync(string name)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == name);
                if (user == null)
                {
                    return false;
                }

                _context.Users.Remove(user);
                return await Save();
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Restituisci tutti gli utenti con post e commenti
        public async Task<List<User>> GetAllUsersWithDetailsAsync()
        {
            return await _context.Users
                .Include(u => u.Posts)
                .Include(u => u.Comments)
                .ToListAsync();
        }

        // Restituisci utente per nome con post e commenti
        public async Task<User> GetUserByNameWithDetailsAsync(string name)
        {
            return await _context.Users
                .Include(u => u.Posts)
                .Include(u => u.Comments)
                .FirstOrDefaultAsync(u => u.Name == name);
        }
    }
}
