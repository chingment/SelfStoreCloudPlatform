using Lumos.DAL;
using Lumos.Entity;
using Lumos.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMerch.Models
{
    public class OwnBaseViewModel : BaseViewModel
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

        public OwnBaseViewModel()
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