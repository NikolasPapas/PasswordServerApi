using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PasswordServerApi.DataSqliteDB;
using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;
using PasswordServerApi.DataSqliteDB.DataModels;

namespace PasswordServerApi.Service
{
    public class PasswordService : IPasswordService
    {
        ApplicationDbContext _dbContext;

        public PasswordService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<PasswordDto> GetPasswords()
        {
            List<PasswordDto> passwords = new List<PasswordDto>();
            _dbContext.Passwords.ToList().ForEach(x => passwords.Add(GetPasswordDto(JsonConvert.DeserializeObject<PasswordModel>(x.JsonData))));
            return passwords;
        }

        public PasswordDto GetPassword(Guid id)
        {
            return GetPasswordDto(JsonConvert.DeserializeObject<PasswordModel>((_dbContext.Passwords.ToList().Find(x => Guid.Parse(x.EndityId) == id)).JsonData));
        }

        public PasswordDto UpdatePassword(PasswordDto passwordDto)
        {
            var updatePassword = GetPasswordModel(passwordDto);
            var passwordModelData = _dbContext.Passwords.ToList().Find(x => x.EndityId == updatePassword.Password);
            passwordModelData.JsonData = JsonConvert.SerializeObject(updatePassword);
            _dbContext.Passwords.Update(passwordModelData);
            _dbContext.SaveChanges();
            return passwordDto;
        }


        private PasswordDto GetPasswordDto(PasswordModel dbPassword)
        {
            return new PasswordDto()
            {
                PasswordId = Guid.Parse(dbPassword.PasswordId),
                Name = dbPassword.Name,
                UserName = dbPassword.UserName,
                Password = dbPassword.Password,
                LogInLink = dbPassword.LogInLink,
                Sensitivity = dbPassword.Sensitivity,
                Strength = dbPassword.Strength
            };

        }

        private PasswordModel GetPasswordModel(PasswordDto dtoPassword)
        {
            return new PasswordModel()
            {
                PasswordId = dtoPassword.PasswordId.ToString(),
                Name = dtoPassword.Name,
                UserName = dtoPassword.UserName,
                Password = dtoPassword.Password,
                LogInLink = dtoPassword.LogInLink,
                Sensitivity = dtoPassword.Sensitivity,
                Strength = dtoPassword.Strength
            };
        }


    }
}
