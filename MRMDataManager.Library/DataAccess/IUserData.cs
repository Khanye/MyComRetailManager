﻿using MRMDataManager.Library.Models;
using System.Collections.Generic;

namespace MRMDataManager.Library.DataAccess
{
    public interface IUserData
    {
        void CreateUser(UserModel user);
        List<UserModel> GetUserById(string Id);
    }
}