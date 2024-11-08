﻿using SocialApp.Models;

namespace SocialApp.Interfaces.Repositories
{
    public interface IUserProfileRepository : IBaseRepository<UserProfile>
    {

        public Task<UserProfile> getByUsernameAsync(string Username);

    }
}
