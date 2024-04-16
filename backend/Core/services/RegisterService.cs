using System;
using System.Linq;
using System.Collections.Generic;

using backend.Data;
using backend.Dto;
using backend.Common;
using System.Text;

namespace backend.Service
{
    public class RegisterService
    {
        private IotDbContext _dbContext;

        public RegisterService(IotDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Register> GetRegisters()
        {
            return _dbContext.Registers.ToList();
        }

        public Register CreateRegister(RegisterInfosDto dto)
        {
            Register register = new Register()
            {
                name = dto.name,
                image = dto.image,
                type = dto.type ?? 1,
                creation_time = dto.creation_time ?? DateTime.UtcNow.ToUniversalTime().AddHours(-3)
            };

            _dbContext.Registers.Add(register);

            _dbContext.SaveChanges();

            return register;
        }

        public Register UpdateRegister(RegisterUpdateDto dto)
        {
            Register register = _dbContext.Registers.FirstOrDefault(r => r.id == dto.id);

            if(register == null)
                return null;
            
            register.name = dto.name;

            _dbContext.SaveChanges();

            return register;
        }

        public Register DeleteRegister(int id)
        {
            Register register = _dbContext.Registers.FirstOrDefault(r => r.id == id);

            if(register == null)
                return null;

            _dbContext.Registers.Remove(register);

            _dbContext.SaveChanges();

            return register;
        }

    }
}