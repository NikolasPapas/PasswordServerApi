using System.Collections.Generic;
using PasswordServerApi.DataSqliteDB;
using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;
using System;
using System.Linq;
using Newtonsoft.Json;

namespace PasswordServerApi.Service
{
    public class AccountService : IAccountService
    {
        ApplicationDbContext _dbContext;

        public AccountService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<AccountDto> GetAccounts()
        {
            List<AccountDto> accounts = new List<AccountDto>();
            _dbContext.Accounts.ToList().ForEach(x => accounts.Add(GetAccountDto(JsonConvert.DeserializeObject<AccountModel>(x.JsonData))));
            return accounts;
        }

        public AccountDto UpdateAccount(AccountDto accountDto)
        {
            var updateAccount = GetAccountModel(accountDto);
            var AccountModelData = _dbContext.Accounts.ToList().Find(x => x.EndityId == updateAccount.AccountId);
            AccountModelData.JsonData = JsonConvert.SerializeObject(updateAccount);
            _dbContext.Accounts.Update(AccountModelData);
            _dbContext.SaveChanges();

            return accountDto;
        }

        public AccountDto GetAccount(Guid id)
        {
            return GetAccountDto(JsonConvert.DeserializeObject<AccountModel>((_dbContext.Accounts.ToList().Find(x => Guid.Parse(x.EndityId) == id)).JsonData));
        }

        private AccountDto GetAccountDto(AccountModel dbAccount)
        {
            return new AccountDto()
            {
                AccountId = Guid.Parse(dbAccount.AccountId),
                FirstName = dbAccount.FirstName,
                LastName = dbAccount.LastName,
                UserName = dbAccount.UserName,
                Email = dbAccount.Email,
                Role = dbAccount.Role,
                Password = dbAccount.Password,
                Sex = dbAccount.Sex,
                LastLogIn = dbAccount.LastLogIn,
                CurentToken = dbAccount.CurentToken,
                Passwords = new List<PasswordDto>() { },
            };
        }

        private AccountModel GetAccountModel(AccountDto dtoAccount)
        {
            return new AccountModel()
            {
                AccountId = dtoAccount.AccountId.ToString(),
                FirstName = dtoAccount.FirstName,
                LastName = dtoAccount.LastName,
                UserName = dtoAccount.UserName,
                Email = dtoAccount.Email,
                Role = dtoAccount.Role,
                Password = dtoAccount.Password,
                Sex = dtoAccount.Sex,
                LastLogIn = dtoAccount.LastLogIn,
                CurentToken = dtoAccount.CurentToken,
                PasswordIds = dtoAccount.Passwords.Select(x => x.PasswordId.ToString()).ToList(),
            };
        }
    }
}
