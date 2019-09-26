using PasswordServerApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.Interfaces
{
    public interface IPasswordService
	{
        IEnumerable<PasswordDto> GetPasswords();

        PasswordDto GetPassword(Guid id);

        PasswordDto UpdatePassword(PasswordDto passwordDto);

    }
}
