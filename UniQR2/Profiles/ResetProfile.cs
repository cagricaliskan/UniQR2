﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniQR2.Models;
using UniQR2.ViewModels;

namespace UniQR2.Profiles
{
    public class ResetProfile : Profile
    {
        public ResetProfile()
        {
            CreateMap<ResetPasswordViewModel, User>().ReverseMap();
        }
    }
}
