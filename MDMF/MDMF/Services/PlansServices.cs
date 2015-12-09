using Sabio.Web.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Sabio.Web.Models.Requests.Plans;
using Sabio.Data;
using Sabio.Web.Models.Requests.Tests;
using Sabio.Web.Domain.Tests;
using Sabio.Web.Models;


namespace Sabio.Web.Services
{
    public class PlansService : BaseService, IPlansService
    {
        public int Insert(PlansAddRequest model, string UserId)
        {
            int id = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Plans_Insert_v2"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Name", model.Name);
                   paramCollection.AddWithValue("@Description", model.Description);
                   paramCollection.AddWithValue("@Types", model.Types);
                   paramCollection.AddWithValue("@Cost", model.Cost);
                   paramCollection.AddWithValue("@GoLiveDate", model.GoLiveDate);
                   paramCollection.AddWithValue("@Tag", model.Tag);
                   paramCollection.AddWithValue("@TagId", model.TagId);

                   SqlParameter p = new SqlParameter("@id", System.Data.SqlDbType.Int);
                   p.Direction = System.Data.ParameterDirection.Output;

                   paramCollection.Add(p);

               }, returnParameters: delegate (SqlParameterCollection param)
               {
                   int.TryParse(param["@id"].Value.ToString(), out id);
               }
               );


            return id;
        }

        public void Update(PlansUpdateRequest model)
        {


            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Plans_Update_v2"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Name", model.Name);
                   paramCollection.AddWithValue("@Description", model.Description);
                   paramCollection.AddWithValue("@Types", model.Types);
                   paramCollection.AddWithValue("@Cost", model.Cost);
                   paramCollection.AddWithValue("@GoLiveDate", model.GoLiveDate);
                   paramCollection.AddWithValue("@Tag", model.Tag);
                   paramCollection.AddWithValue("@TagId", model.TagId);

                   paramCollection.AddWithValue("@Id", model.Id);


               }
               , returnParameters: delegate (SqlParameterCollection param)

               {

               }
               );


        }

        public Plan GetPlan(int Id)
        {
            Plan p = null;
            DataProvider.ExecuteCmd(GetConnection, "dbo.Plans_SelectByID_v2"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Id", Id);
               }
               , map: delegate (IDataReader reader, short set)
               {
                   p = MapPlan(reader);

               }
               );

            return p;
        }

        public List<Plan> GetPlansByType(int type)
        {
            List<Plan> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.PlanTypes_SelectByPlanType_v2"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Types", type);
               }
               , map: delegate (IDataReader reader, short set)
               {

                   Plan p = MapPlan(reader);


                   if (list == null)
                   {
                       list = new List<Plan>();
                   }

                   list.Add(p);
               }
               );


            return list;
        }

        public List<UserPlans> GetPlanByUserId(string userId)
        {
            List<UserPlans> list = null;
            List<PlanFeature> featureList = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.UserPlans_SelectByUserId_v3"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)

               { paramCollection.AddWithValue("@UserId", userId); }

               , map: delegate (IDataReader reader, short set)
               {

                   UserPlans u = new UserPlans();

                   PlanType pt = new PlanType();

                   PlanFeature pf = new PlanFeature();

                   PriorityLevel pl = new PriorityLevel();

                   if (set == 0)
                   {
                       int startingIndex = 0;

                       u.PlanId = reader.GetSafeInt32(startingIndex++);
                       u.Subscribed = reader.GetSafeBool(startingIndex++);
                       u.PlanName = reader.GetSafeString(startingIndex++);
                       u.PlanDescription = reader.GetSafeString(startingIndex++);
                       pt.Id = reader.GetSafeInt32(startingIndex++);
                       pt.Name = reader.GetSafeString(startingIndex++);
                       u.Cost = reader.GetSafeInt32(startingIndex++);
                       u.GoLiveDate = reader.GetSafeDateTime(startingIndex++);
                       u.TagId = reader.GetSafeInt32(startingIndex++);
                       u.TagName = reader.GetSafeString(startingIndex++);

                       u.Type = pt;

                       if (list == null)
                       {
                           list = new List<UserPlans>();
                       }

                       list.Add(u);

                   }

                   if (set == 1)
                   {
                       int startingIndex = 0;

                       pf.Title = reader.GetSafeString(startingIndex++);
                       pf.Description = reader.GetSafeString(startingIndex++);
                       pf.Id = reader.GetSafeInt32(startingIndex++);
                       pf.PlanId = reader.GetSafeInt32(startingIndex++);
                       pf.DateAdded = reader.GetSafeDateTime(startingIndex++);
                       pf.DateModified = reader.GetSafeDateTime(startingIndex++);
                       pf.LanguageCode = reader.GetSafeString(startingIndex++);

                       pl.Name = reader.GetSafeString(startingIndex++);
                       pl.Id = reader.GetSafeInt32(startingIndex++);

                       pf.SortOrder = pl;

                       if (featureList == null)
                       {
                           featureList = new List<PlanFeature>();
                       }

                       featureList.Add(pf);

                   }

               });

            if (featureList != null)
            {
                for (var i = 0; featureList.Count > i; i++)
                {

                    for (var e = 0; list.Count > e; e++)
                    {
                        if (list[e].PlanId == featureList[i].PlanId)
                        {

                            if (list[e].FeatureInfo == null)
                            {
                                list[e].FeatureInfo = new List<PlanFeature>();
                            }

                            list[e].FeatureInfo.Add(featureList[i]);

                        }
                    }

                }
            }

            return list;
        }

        public List<Plan> GetAllPlans()
        {
            List<Plan> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Plans_Select_v2"
               , inputParamMapper: null
               , map: delegate (IDataReader reader, short set)
               {
                   Plan p = MapPlan(reader);

                   if (list == null)
                   {
                       list = new List<Plan>();
                   }

                   list.Add(p);
               }
               );

            return list;
        }

        public List<Plan> GetAllPlansWithFeatures()
        {
            List<Plan> list = null;
            List<PlanFeature> featureList = null;
            DataProvider.ExecuteCmd(GetConnection, "dbo.PlansAndFeatures_SelectAll_v2"
              , inputParamMapper: null
              , map: delegate (IDataReader reader, short set)
              {

                  if (set == 0)
                  {
                      Plan p = MapPlan(reader);


                      if (list == null)
                      {
                          list = new List<Plan>();
                      }

                      list.Add(p);
                  }


                  else if (set == 1)
                  {
                      PlanFeature pf = MapPlanFeature(reader);

                      if (featureList == null)
                      {
                          featureList = new List<PlanFeature>();
                      }

                      featureList.Add(pf);
                  }


              }
               );

            if (list != null)
            {

            }

            for (var i = 0; featureList.Count > i; i++)//Loops Features counts them all.
            {
                for (var e = 0; list.Count > e; e++)//Loops Plans counts them all.
                {
                    if (list[e].Id == featureList[i].PlanId)// if the Plan Id is equal to the Plan Feature ID
                    {
                        if (list[e].PlanFeature == null)//Null Check
                        {
                            list[e].PlanFeature = new List<PlanFeature>();
                        }
                        list[e].PlanFeature.Add(featureList[i]);//Adds Specific Plan feature to a specific Plan based on ID.
                    }
                }
            }
            return list;
        }


        public List<Plan> GetPlansWithFeaturesNotSubscribed(string userId)
        {
            List<Plan> list = null;
            List<PlanFeature> featureList = null;
            DataProvider.ExecuteCmd(GetConnection, "dbo.PlansAndFeatures_SelectByNotSubscribed_v2"
              , inputParamMapper: delegate (SqlParameterCollection paramCollection)
              {
                  paramCollection.AddWithValue("@UserId", userId);
              }
              , map: delegate (IDataReader reader, short set)
              {

                  if (set == 0)
                  {
                      Plan p = MapPlan(reader);


                      if (list == null)
                      {
                          list = new List<Plan>();
                      }

                      list.Add(p);
                  }


                  else if (set == 1)
                  {
                      PlanFeature pf = MapPlanFeature(reader);

                      if (featureList == null)
                      {
                          featureList = new List<PlanFeature>();
                      }

                      featureList.Add(pf);
                  }


              }
               );

            if (list != null)
            {

            }

            for (var i = 0; featureList.Count > i; i++)//Loops Features counts them all.
            {
                for (var e = 0; list.Count > e; e++)//Loops Plans counts them all.
                {
                    if (list[e].Id == featureList[i].PlanId)// if the Plan Id is equal to the Plan Feature ID
                    {
                        if (list[e].PlanFeature == null)//Null Check
                        {
                            list[e].PlanFeature = new List<PlanFeature>();
                        }
                        list[e].PlanFeature.Add(featureList[i]);//Adds Specific Plan feature to a specific Plan based on ID.
                    }
                }
            }
            return list;
        }


        public void DeletePlan(int id)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Plans_Delete"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Id", id);

               });
        }

        public void DeletePlanWithFeatures(int id)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Plans_DeleteWithFeatures"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@PlanId", id);

               });
        }

        private static Plan MapPlan(IDataReader reader)
        {
            Plan p = new Plan();

            PlanType pt = new PlanType();

            int startingIndex = 0;

            p.Id = reader.GetSafeInt32(startingIndex++);
            p.Name = reader.GetSafeString(startingIndex++);
            p.Description = reader.GetSafeString(startingIndex++);
            p.Cost = reader.GetSafeInt32(startingIndex++);
            p.GoLiveDate = reader.GetSafeDateTime(startingIndex++);
            p.DateAdded = reader.GetSafeDateTime(startingIndex++);
            p.DateModified = reader.GetSafeDateTime(startingIndex++);
            p.LanguageCode = reader.GetSafeString(startingIndex++);
            p.Tag = reader.GetSafeString(startingIndex++);

            int secondIndex = 9;
            pt.Name = reader.GetSafeString(secondIndex++);
            pt.Id = reader.GetSafeInt32(secondIndex++);

            p.Type = pt;

            return p;
        }


        public int Insert(PlanFeatureAddRequest model, string userId)
        {
            int id = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.PlanFeatures_Insert"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Title", model.Title);
                   paramCollection.AddWithValue("@Description", model.Description);
                   paramCollection.AddWithValue("@Category", model.Category);
                   paramCollection.AddWithValue("@SortOrder", model.SortOrder);
                   paramCollection.AddWithValue("@PlanId", model.PlanId);
                   paramCollection.AddWithValue("@UserId", userId);

                   SqlParameter p = new SqlParameter("@Id", System.Data.SqlDbType.Int);
                   p.Direction = System.Data.ParameterDirection.Output;

                   paramCollection.Add(p);

               }, returnParameters: delegate (SqlParameterCollection param)
               {
                   Int32.TryParse(param["@id"].Value.ToString(), out id);
               }
               );


            return id;

        }

        public void Update(PlanFeatureUpdateRequest model, string userId)
        {

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.PlanFeatures_Update"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Title", model.Title);
                   paramCollection.AddWithValue("@Description", model.Description);
                   paramCollection.AddWithValue("@Category", model.Category);
                   paramCollection.AddWithValue("@SortOrder", model.SortOrder);
                   paramCollection.AddWithValue("@PlanId", model.PlanId);
                   paramCollection.AddWithValue("@UserId", userId);
                   paramCollection.AddWithValue("@Id", model.Id);

               });
        }

        public PlanFeature GetPlanFeature(int id)
        {
            PlanFeature pf = null;
            DataProvider.ExecuteCmd(GetConnection, "dbo.PlanFeatures_SelectById"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Id", id);
               }
               , map: delegate (IDataReader reader, short set)
               {
                   pf = MapPlanFeature(reader);
               }
               );

            return pf;
        }

        public List<PlanFeature> GetPlanFeaturesById(int planId)
        {
            List<PlanFeature> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.PlanFeatures_SelectByPlanId"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@PlanId", planId);
               }
               , map: delegate (IDataReader reader, short set)
               {
                   PlanFeature pf = MapPlanFeature(reader);

                   if (list == null)
                   {
                       list = new List<PlanFeature>();
                   }

                   list.Add(pf);
               }
               );


            return list;
        }

        public List<PlanFeature> GetAllPlanFeatures()
        {
            List<PlanFeature> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.PlanFeatures_Select"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {

               }
               , map: delegate (IDataReader reader, short set)
               {
                   PlanFeature pf = MapPlanFeature(reader);

                   if (list == null)
                   {
                       list = new List<PlanFeature>();
                   }

                   list.Add(pf);
               }
               );


            return list;
        }

        public void DeletePlanFeature(int id)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.PlanFeatures_Delete"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Id", id);

               });
        }

        private static PlanFeature MapPlanFeature(IDataReader reader)
        {
            PlanFeature pf = new PlanFeature();

            PriorityLevel pl = new PriorityLevel();

            int startingIndex = 0;

            pf.Id = reader.GetSafeInt32(startingIndex++);
            pf.Title = reader.GetSafeString(startingIndex++);
            pf.Description = reader.GetSafeString(startingIndex++);
            pf.Category = reader.GetSafeInt32(startingIndex++);
            pf.PlanId = reader.GetSafeInt32(startingIndex++);
            pf.DateAdded = reader.GetSafeDateTime(startingIndex++);
            pf.DateModified = reader.GetSafeDateTime(startingIndex++);
            pf.LanguageCode = reader.GetSafeString(startingIndex++);

            pl.Name = reader.GetSafeString(startingIndex++);
            pl.Id = reader.GetSafeInt32(startingIndex++);

            pf.SortOrder = pl;

            return pf;
        }


        public int InsertPlanType(PlanTypeAddRequest model)
        {
            int id = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.PlanTypes_Insert"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Name", model.Name);

                   SqlParameter p = new SqlParameter("@id", System.Data.SqlDbType.Int);
                   p.Direction = System.Data.ParameterDirection.Output;

                   paramCollection.Add(p);

               }, returnParameters: delegate (SqlParameterCollection param)
               {
                   int.TryParse(param["@id"].Value.ToString(), out id);
               }
               );


            return id;
        }

        public void UpdatePlanType(PlanTypeUpdateRequest model)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.PlanTypes_Update"
              , inputParamMapper: delegate (SqlParameterCollection paramCollection)
              {
                  paramCollection.AddWithValue("@Name", model.Name);
                  paramCollection.AddWithValue("@Id", model.Id);

              });
        }

        public PlanType GetPlanTypesById(int typeId)
        {
            PlanType pt = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.PlanTypes_SelectById"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Id", typeId);
               }
               , map: delegate (IDataReader reader, short set)
               {
                   pt = new PlanType();

                   int startingIndex = 0;

                   pt.Id = reader.GetSafeInt32(startingIndex++);
                   pt.Name = reader.GetSafeString(startingIndex++);

               }
               );
            return pt;
        }

        public List<PlanType> GetAllPlanTypes()
        {
            List<PlanType> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.PlanTypes_Select"
               , inputParamMapper: null
               , map: delegate (IDataReader reader, short set)
               {
                   PlanType pt = new PlanType();

                   int startingIndex = 0;

                   pt.Id = reader.GetSafeInt32(startingIndex++);
                   pt.Name = reader.GetSafeString(startingIndex++);


                   if (list == null)
                   {
                       list = new List<PlanType>();
                   }

                   list.Add(pt);
               }
               );
            return list;
        }

        public void DeletePlanType(int typeId)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.PlanTypes_DeletePlans"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@TypeId", typeId);

               });
        }

        //Code for Subscription
        public void UserSubcriptionToPlan(UserPlanSubscribe model, string userId, bool Subscribed, int PlanId)
        {


            DataProvider.ExecuteNonQuery(GetConnection, "dbo.UserPlans_Insert"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {

                   paramCollection.AddWithValue("@UserId", userId);
                   paramCollection.AddWithValue("@PlanId", PlanId);
                   paramCollection.AddWithValue("@Subscribed", Subscribed);


                   //SqlParameter p = new SqlParameter("@Id", System.Data.SqlDbType.Int);
                   //p.Direction = System.Data.ParameterDirection.Output;

                   //paramCollection.Add(p);

               }, returnParameters: delegate (SqlParameterCollection param)
               {

               }
               );



        }

        public List<PriorityLevel> GetPriorityLevels()
        {
            List<PriorityLevel> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.PriorityLevel_Select"
               , inputParamMapper: null
               , map: delegate (IDataReader reader, short set)
               {
                   PriorityLevel pl = new PriorityLevel();

                   int startingIndex = 0;

                   pl.Id = reader.GetSafeInt32(startingIndex++);
                   pl.Name = reader.GetSafeString(startingIndex++);


                   if (list == null)
                   {
                       list = new List<PriorityLevel>();
                   }

                   list.Add(pl);
               }
               );
            return list;
        }

        public List<UserComment> SelectPlanCommentsByUserId(string UserId)
        {
            List<UserComment> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.UserPlans_SelectCommentsByUserId"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@UserId", UserId);
               }

               , map: delegate (IDataReader reader, short set)
               {
                   UserComment c = new UserComment();

                   int startingIndex = 0;

                   c.Id = reader.GetSafeInt32(startingIndex++);
                   c.Comment = reader.GetSafeString(startingIndex++);
                   c.CommentDate = reader.GetSafeDateTime(startingIndex++);
                   c.IsFlagged = reader.GetSafeBool(startingIndex++);
                   c.IsVisible = reader.GetSafeBool(startingIndex++);
                   c.ParentId = reader.GetSafeInt32(startingIndex++, null);
                   c.OwnerId = reader.GetSafeInt32(startingIndex++);
                   c.OwnerType = reader.GetSafeEnum<CommentOwnerType>(startingIndex++);
                   c.Deleted = reader.GetSafeBool(startingIndex++);
                   c.DeletedDate = reader.GetSafeDateTime(startingIndex++);
                   c.LanguageCode = reader.GetSafeString(startingIndex++);
                   c.UpdateDate = reader.GetSafeDateTime(startingIndex++);
                   c.Title = reader.GetSafeString(startingIndex++);

                   if (list == null)
                   {
                       list = new List<UserComment>();
                   }
                   list.Add(c);
               }
               );
            return list;
        }
    }
}