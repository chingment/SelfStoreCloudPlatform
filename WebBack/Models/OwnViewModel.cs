﻿using Lumos.DAL;
using Lumos.Entity;
using Lumos.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models
{
    public class OwnViewModel : BaseViewModel
    {
        private LumosDbContext _currentDb;

        [JsonIgnore]
        public LumosDbContext CurrentDb
        {
            get
            {
                return _currentDb;
            }
        }

        public OwnViewModel()
        {
            _currentDb = new LumosDbContext();
        }

        public override int Operater
        {
            get
            {
                return OwnRequest.GetCurrentUserId();
            }
        }
    }
}