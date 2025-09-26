using Finsight.Contract.Dto;
using Finsight.Core.Bl;
using Finsight.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Finsight.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserBl _userBl;
        private readonly IJwtTokenFactory _jwtTokenFactory;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IUserBl userBl,
            IJwtTokenFactory jwtTokenFactory,
            ILogger<UserController> logger)
        {
            _userBl = userBl ?? throw new ArgumentNullException(nameof(userBl));
            _jwtTokenFactory = jwtTokenFactory ?? throw new ArgumentNullException(nameof(jwtTokenFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(nameof(GetAllUsers))]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers(CancellationToken cancellationToken)
        {
            var users = await _userBl.GetAllUsersAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Возвращено {UserCount} пользователей.", users.Count);
            return Ok(users);
        }

        [HttpPost(nameof(AddUser))]
        public async Task<ActionResult> AddUser([FromBody] UserDto dto, CancellationToken cancellationToken)
        {
            await _userBl.AddUserAsync(dto, cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Пользователь {Login} создан через API.", dto.Login);
            return Ok();
        }

        [HttpPut(nameof(UpdateUser))]
        public async Task<ActionResult> UpdateUser([FromBody] UserDto dto, CancellationToken cancellationToken)
        {
            await _userBl.UpdateUserAsync(dto, cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Пользователь {UserId} обновлен через API.", dto.Id);
            return Ok();
        }

        [HttpGet(nameof(DeleteUser))]
        public async Task<ActionResult> DeleteUser(Guid userId, CancellationToken cancellationToken)
        {
            await _userBl.DeleteUserAsync(userId, cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Пользователь {UserId} удален через API.", userId);
            return Ok();
        }

        [HttpPost(nameof(Login))]
        public async Task<ActionResult<AuthResponseDto>> Login(string username, string password, CancellationToken cancellationToken)
        {
            var user = await _userBl.LoginAsync(username, password, cancellationToken).ConfigureAwait(false);
            if (user == null)
            {
                _logger.LogWarning("Неуспешная попытка входа для пользователя {Login}.", username);
                return Unauthorized("Неуспешная попытка входа");
            }

            var token = _jwtTokenFactory.CreateToken(user);
            return Ok(new AuthResponseDto
            {
                Token = token,
                User = user
            });
        }
    }
}
