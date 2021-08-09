using TurkBelge.CONTROLLER.Singleton;
using TurkBelge.MODEL.Data;
using TurkBelge.MODEL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurkBelge.CONTROLLER.DAL
{
   public class UserInformationDal
    {


        public UserInformation getUserInformation()
        {
            using(DatabaseContext databaseContext = new DatabaseContext())
            {
                //db de tek bir kullanıcı kayıtlı oldugundan fırs ıle cagırabılırız
                return databaseContext.userInformations.First();
            }
        }

       


    }
}
