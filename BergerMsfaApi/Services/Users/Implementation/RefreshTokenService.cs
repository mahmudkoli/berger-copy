using Berger.Data.MsfaEntity.Users;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Users.Interfaces;
using System;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.Users.Implementation
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRepository<RefreshToken> _refreshTokenRepo;


        public RefreshTokenService(
            IRepository<RefreshToken> refreshTokenRepo
            )
        {
            _refreshTokenRepo = refreshTokenRepo;
        }

        public async Task<RefreshToken> GetByIdAsync(Guid id)
        {
            var singleValue = await _refreshTokenRepo.FindAsync(x => x.Id == id);

            //_ = singleValue ?? throw new Exception("Not found refresh token.");

            return singleValue;
        }

        public async Task<RefreshToken> GetByTokenAsync(string token)
        {
            var singleValue = await _refreshTokenRepo.FindAsync(x => x.Token == token);

            //_ = singleValue ?? throw new Exception("Not found refresh token.");

            return singleValue;
        }

        public async Task<Guid> AddAsync(RefreshToken entity)
        {
            await _refreshTokenRepo.CreateAsync(entity);
            await _refreshTokenRepo.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<Guid> UpdateAsync(RefreshToken entity)
        {
            await _refreshTokenRepo.UpdateAsync(entity);
            await _refreshTokenRepo.SaveChangesAsync();

            return entity.Id;
        }
    }
}
