using Sabio.Data;
using Sabio.Web.Domain;
using Sabio.Web.Models.Requests.ResetPassword;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sabio.Web.Services
{
    public class TokensService : BaseService
    {
        public static int Insert(TokensRequest model, string userId)
        {
            int TokenId = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.UserTokens_Insert",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Token", model.Token);
                    paramCollection.AddWithValue("@TokenTypeId", model.TokenTypeId);
                    paramCollection.AddWithValue("@UserId", userId);

                    SqlParameter p = new SqlParameter("@Id", System.Data.SqlDbType.Int);
                    p.Direction = System.Data.ParameterDirection.Output;

                    paramCollection.Add(p);

                }, returnParameters: delegate (SqlParameterCollection param)
                {
                    int.TryParse(param["@Id"].Value.ToString(), out TokenId);
                }
            );

            return TokenId;
        }

        public static UserTokens GetById(Guid token)
        {
            UserTokens t = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.UserTokens_SelectByToken"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Token", token);
                }
               , map: delegate (IDataReader reader, short set)
               {
                   t = new UserTokens();

                   int startingIndex = 0;

                   t.Token = reader.GetGuid(startingIndex++);
                   t.TokenTypeId = reader.GetSafeInt16(startingIndex++);
                   t.UserId = reader.GetSafeString(startingIndex++);
                   t.DateAdded = reader.GetSafeDateTime(startingIndex++);
                   t.DateModified = reader.GetSafeDateTime(startingIndex++);
                   t.LanguageCode = reader.GetSafeString(startingIndex++);
               }
               );

            return t;
        }

        public static bool Exists(Guid token)
        {
            bool dbResponse = false;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.UserTokens_Exists"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Token", token);

                    SqlParameter b = new SqlParameter("@Bit", System.Data.SqlDbType.Bit);
                    b.Direction = System.Data.ParameterDirection.Output;

                    paramCollection.Add(b);
                },
                returnParameters: delegate (SqlParameterCollection param)
                {
                    bool.TryParse(param["@Bit"].Value.ToString(), out dbResponse);
                }
            );

            return dbResponse;
        }

        public static bool DeleteToken(Guid token)
        {
            bool deleted = false;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.UserTokens_DeleteByToken"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Token", token);
                    deleted = true;
                });

            return deleted;
        }
    }
}

