using Sabio.Web.Models.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Sabio.Data;
using System.Linq;
using System.Web;
using Sabio.Web.Models.Requests.Referrals;
using Sabio.Web.Domain;

namespace Sabio.Web.Services
{
    public class ReferralService : BaseService, IReferralService
    {
        public int Insert(AddReferral model, string userId)
        {
            int referralId = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.PartnerReferral_Insert",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@UserId", userId);
                    paramCollection.AddWithValue("@PartnerType", model.PartnerType);
                    paramCollection.AddWithValue("@PartnerName", model.PartnerName);
                    paramCollection.AddWithValue("@Description", model.Description);
                    paramCollection.AddWithValue("@URL", model.URL.ToString());
                    paramCollection.AddWithValue("@AffiliateCode", model.AffiliateCode);


                    SqlParameter r = new SqlParameter("@Id", System.Data.SqlDbType.Int);
                    r.Direction = System.Data.ParameterDirection.Output;

                    paramCollection.Add(r);

                }, returnParameters: delegate (SqlParameterCollection param)
                {
                    int.TryParse(param["@Id"].Value.ToString(), out referralId);
                }
            );

            return referralId;
        }

        public void Update(UpdateReferral model, string userId)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.PartnerReferral_Update",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Id", model.Id);
                    paramCollection.AddWithValue("@UserId", userId);
                    paramCollection.AddWithValue("@PartnerType", model.PartnerType);
                    paramCollection.AddWithValue("@PartnerName", model.PartnerName);
                    paramCollection.AddWithValue("@Description", model.Description);
                    paramCollection.AddWithValue("@URL", model.URL.ToString());
                    paramCollection.AddWithValue("@AffiliateCode", model.AffiliateCode);

                }
            , returnParameters: delegate (SqlParameterCollection param)
            {

            }
            );
        }

        public List<Referral> Get()
        {
            List<Referral> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.PartnerReferral_Select"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {

                }
               , map: delegate (IDataReader reader, short set)
               {
                   Referral r = new Referral();

                   int startingIndex = 0;

                   r.Id = reader.GetSafeInt32(startingIndex++);

                   r.PartnerType = reader.GetSafeInt32(startingIndex++);
                   r.PartnerName = reader.GetSafeString(startingIndex++);
                   r.Description = reader.GetSafeString(startingIndex++);
                   r.Url = reader.GetSafeUri(startingIndex++);
                   r.AffiliateCode = reader.GetSafeString(startingIndex++);

                   r.DateAdded = reader.GetSafeDateTime(startingIndex++);
                   r.DateModified = reader.GetSafeDateTime(startingIndex++);
                   r.LanguageCode = reader.GetSafeString(startingIndex++);

                   if (list == null)
                   {
                       list = new List<Referral>();
                   }

                   list.Add(r);
               }
               );


            return list;
        }

        public Referral GetById(int Id)
        {
            Referral r = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.PartnerReferral_SelectById"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Id", Id);
                }
               , map: delegate (IDataReader reader, short set)
               {
                   r = new Referral();

                   int startingIndex = 0;

                   r.Id = reader.GetSafeInt32(startingIndex++);

                   r.PartnerType = reader.GetSafeInt32(startingIndex++);
                   r.PartnerName = reader.GetSafeString(startingIndex++);
                   r.Description = reader.GetSafeString(startingIndex++);
                   r.Url = reader.GetSafeUri(startingIndex++);
                   r.AffiliateCode = reader.GetSafeString(startingIndex++);

                   r.DateAdded = reader.GetSafeDateTime(startingIndex++);
                   r.DateModified = reader.GetSafeDateTime(startingIndex++);
                   r.LanguageCode = reader.GetSafeString(startingIndex++);

               }
               );

            return r;

        }

        public void Delete(int Id)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.PartnerReferral_Delete"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Id", Id);
                });
        }

        public List<Referral> GetByPartnerType(int Type)
        {
            List<Referral> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.PartnerReferral_SelectByPartnerType"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@PartnerType", Type);
                }
               , map: delegate (IDataReader reader, short set)
               {
                   Referral r = new Referral();

                   int startingIndex = 0;

                   r.Id = reader.GetSafeInt32(startingIndex++);

                   r.PartnerType = reader.GetSafeInt32(startingIndex++);
                   r.PartnerName = reader.GetSafeString(startingIndex++);
                   r.Description = reader.GetSafeString(startingIndex++);
                   r.Url = reader.GetSafeUri(startingIndex++);
                   r.AffiliateCode = reader.GetSafeString(startingIndex++);

                   r.DateAdded = reader.GetSafeDateTime(startingIndex++);
                   r.DateModified = reader.GetSafeDateTime(startingIndex++);
                   r.LanguageCode = reader.GetSafeString(startingIndex++);

                   if (list == null)
                   {
                       list = new List<Referral>();
                   }

                   list.Add(r);
               }
               );

            return list;

        }
    }
}