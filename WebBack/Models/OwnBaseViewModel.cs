using Lumos.DAL;
using Lumos.Entity;
using Lumos.Web.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models
{
    public class OwnBaseViewModel : BaseViewModel
    {
        private LumosDbContext _currentDb;

        [JsonIgnore]
        public LumosDbContext CurrentDb
        {
            get
            {
                if (_currentDb == null)
                {
                    _currentDb = new LumosDbContext();
                }

                return _currentDb;
            }
        }

        public OwnBaseViewModel()
        {

        }

        public override string CurrentUserId
        {
            get
            {
                return OwnRequest.GetCurrentUserId();
            }
        }
    }
}