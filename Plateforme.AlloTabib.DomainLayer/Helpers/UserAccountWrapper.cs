using Plateforme.AlloTabib.DomainLayer.Entities;
using Plateforme.AlloTabib.DomainLayer.Models;

namespace Plateforme.AlloTabib.DomainLayer.Helpers
{
    public static class UserAccountWrapper
    {
        public static UserAccountDataModel ConvertBoardEntityToDataModel(UserAccount userAccount)
        {
            if (userAccount == null)
                return null;

            return new UserAccountDataModel
            {

                Email = userAccount.Email,
                CreationDate = userAccount.CreationDate,
                LastModificationDate = userAccount.LastModificationDate,
                Type = userAccount.Type,
                Password =
                    CrossCuttingLayer.Encryption.RijndaelEncryption.Decrypt(userAccount.Password)

            };
        }
    }
}
