using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Service
{
    public class RoleService
    {
        private readonly AppDbContext _context;

        public RoleService(AppDbContext context)
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

        // Create
        public async Task<bool> CreateRoleAsync(string newRole)
        {
            if (newRole != "USER" && newRole != "ADMIN")
            {
                return false;
            }

            var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleUser == newRole);
            if (existingRole != null)
            {
                return false;
            }

            var role = new Role { RoleUser = newRole };
            await _context.Roles.AddAsync(role);
            await Save();
            return true;
        }

        // Read - Get all roles
        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        // Read - Get role by ID
        public async Task<Role> GetRoleByIdAsync(Guid roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }

        // Read - Get role by name
        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleUser == roleName);
        }

        // Update
        public async Task<bool> UpdateRoleAsync(Guid roleId, string newRoleName)
        {
            if (newRoleName != "USER" && newRoleName != "ADMIN")
            {
                return false;
            }

            var role = await _context.Roles.FindAsync(roleId);
            if (role == null)
            {
                return false;
            }

            var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleUser == newRoleName);
            if (existingRole != null)
            {
                return false;
            }

            role.RoleUser = newRoleName;
            _context.Roles.Update(role);
            await Save();
            return true;
        }

        // Delete
        public async Task<bool> DeleteRoleAsync(Guid roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null)
            {
                return false;
            }

            _context.Roles.Remove(role);
            await Save();
            return true;
        }
    }
}
