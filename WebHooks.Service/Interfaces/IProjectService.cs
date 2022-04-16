﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Service.Dtos;

namespace WebHooks.Service.Interfaces
{
    public interface IProjectService
    {
        Task<List<ProjectDto>> ListAsync();
    }
}
