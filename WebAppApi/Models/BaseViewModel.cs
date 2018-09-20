using Lumos.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models
{
    public class BaseViewModel
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

        public BaseViewModel()
        {
            _currentDb = new LumosDbContext();
        }
    }
}