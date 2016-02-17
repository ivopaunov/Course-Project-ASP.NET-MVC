﻿namespace H8QMedia.Services.Data.Contracts
{
    using System.Linq;
    using H8QMedia.Data.Models;

    public interface IUsersService
    {
        IQueryable<ApplicationUser> GetUser(string name);

        IQueryable<ApplicationUser> UserById(string id);
    }
}