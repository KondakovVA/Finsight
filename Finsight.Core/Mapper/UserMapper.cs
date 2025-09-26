using System;
using System.Collections.Generic;
using System.Linq;
using Finsight.Contract.Dto;
using Finsight.Core.Dao.Model;
using Finsight.Core.Extensions;

namespace Finsight.Core.Mapper
{
    public static class UserMapper
    {
        public static UserDto ToDto(this User entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return new UserDto
            {
                Id = entity.Id,
                Login = entity.Login,
                DisplayName = entity.DisplayName,
                Role = entity.Role
            };
        }

        public static List<UserDto> ToDtos(this IEnumerable<User> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);
            return entities.Select(ToDto).ToList();
        }

        public static User ToEntityForCreate(this UserDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new ArgumentException("Для нового пользователя необходимо указать пароль.", nameof(dto));
            }

            var id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id;

            return new User
            {
                Id = id,
                Login = dto.Login,
                DisplayName = dto.DisplayName,
                PasswordHash = dto.Password.HashPassword(),
                Role = dto.Role
            };
        }

        public static void ApplyToEntity(this UserDto dto, User entity)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            entity.Login = dto.Login;
            entity.DisplayName = dto.DisplayName;
            entity.Role = dto.Role;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                entity.PasswordHash = dto.Password.HashPassword();
            }
        }
    }
}
