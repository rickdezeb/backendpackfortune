﻿using Packfortune.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packfortune.Logic.Interfaces
{
    public interface IAddUserCoins
    {
        Task AddUserCoinDataAsync(User userCoinData);
    }
}
