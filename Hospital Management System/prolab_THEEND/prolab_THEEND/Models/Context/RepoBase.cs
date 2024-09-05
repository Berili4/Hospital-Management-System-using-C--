using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prolab_THEEND.Models.Context
{
    public class RepoBase
    {
        private static DatabaseContext dbContext;
        public static DatabaseContext CreateOrGetInstance()
        {
            if (dbContext == null)
                dbContext = new DatabaseContext();
            return dbContext;    
        }
    }
}