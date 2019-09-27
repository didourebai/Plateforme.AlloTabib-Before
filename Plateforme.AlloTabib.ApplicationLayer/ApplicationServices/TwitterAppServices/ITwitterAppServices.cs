using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.TwitterAppServices
{
    public interface ITwitterAppServices
    {
        ResultOfType<TwitterUserModel> VerifyCredentials();
    }
}
