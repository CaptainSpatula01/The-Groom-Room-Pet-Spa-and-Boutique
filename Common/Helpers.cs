﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using groomroom.Entities;

namespace groomroom.Common;

public static class Helpers
{
    public static UserGetDto ToDto(this User user)
    {
        var userDto = new UserGetDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
        };

        return userDto;
    }
}