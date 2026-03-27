using AuthDemo.DTOs.UserDTOs;
using AuthDemo.Exceptions;
using AuthDemo.Helpers;
using AuthDemo.Models;
using AuthDemo.Repositories.Interfaces;
using AuthDemo.Services.Interfaces;

namespace AuthDemo.Services.Implementations
{
    public class AuthService(IUserRepository repo, JwtService jwt, IEmailService emailService) : IAuthService
    {
        private readonly IUserRepository _repo = repo;
        private readonly JwtService _jwtService = jwt;
        private readonly IEmailService _emailService = emailService;
        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            var user = await _repo.GetUserByEmailAsync(loginDto.Email);

            if (user == null)
                throw new UnauthorizedException("Invalid Crendentials");

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);

            if (!isPasswordValid)
                throw new UnauthorizedException("Invalid Crendentials");

            var token = _jwtService.GenerateToken(new TokenUserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Roles = user.UserRoles?.Select(ur => ur.Role.Name).ToList() ?? []
            });

            await _emailService.SendEmailAsync("kelash.raisal@gmail.com", "User Logged In", "Thanks for Trustun US.");
            return new AuthResponseDto
            {
                Id = user.Id,
                Uid = user.Uid,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                token = token,
                Roles = user.UserRoles?.Select(ur => ur.Role.Name).ToList() ?? []
            };
        }

        public async Task<AuthResponseDto> Register(RegisterDto registerDto)
        {
            var user = _repo.GetUserByEmailAsync(registerDto.Email).Result;
            if (user != null)
            {
                throw new ConflictException("User already exists");
            }

            if (registerDto.RolesIds == null || registerDto.RolesIds.Count == 0)
            {
                throw new ValidationException("At least one role must be selected");
            }

            var roleIds = registerDto.RolesIds.Distinct().ToList();
            var validRoleIds = await _repo.GetValidRoleIdsAsync(roleIds);

            if (validRoleIds.Count != roleIds.Count)
            {
                throw new BadRequestException("One or more selected roles are invalid");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var newUser = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Password = hashedPassword,
                UserRoles = validRoleIds.Select(id => new UserRole { RoleId = id }).ToList()
            };

            //get all roles by ids 
            var roles = await _repo.GetRolesByIdsAsync(roleIds);

            await _repo.CreateUserAsync(newUser);
            //generate token
            var token = _jwtService.GenerateToken(new TokenUserDTO
            {
                Id = newUser.Id,
                Name = newUser.Name,
                Email = newUser.Email,
                Roles = roles.Select(r => r.Name).ToList()
            });

            var response = new AuthResponseDto
            {
                Id = newUser.Id,
                Uid = newUser.Uid,
                Name = newUser.Name,
                Email = newUser.Email,
                CreatedAt = newUser.CreatedAt,
                UpdatedAt = newUser.UpdatedAt,
                token = token,
                Roles = roles.Select(r => r.Name).ToList()

            };

            return response;
        }
    }
}
