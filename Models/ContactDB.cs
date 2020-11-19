using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace UploadExcelFile.Models
{
    public class ContactDB
    {
        public static int GetBatchID(ContactBatch contactBatch)
        {
            string conString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            int batchID;
            using (SqlConnection con = new SqlConnection(conString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("AddBatchReturnIDWithOutput", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter paramBatchName = new SqlParameter
                    {
                        ParameterName = "@BatchName",
                        Value = contactBatch.BatchName
                    };
                    cmd.Parameters.Add(paramBatchName);

                    SqlParameter paramDateCreated = new SqlParameter
                    {
                        ParameterName = "@DateCreated",
                        Value = contactBatch.DateCreated
                    };
                    cmd.Parameters.Add(paramDateCreated);

                    SqlParameter paramCreatedBy = new SqlParameter
                    {
                        ParameterName = "@CreatedBy",
                        Value = contactBatch.CreatedBy
                    };
                    cmd.Parameters.Add(paramCreatedBy);

                    SqlParameter paramDateModified = new SqlParameter
                    {
                        ParameterName = "@DateModified",
                        Value = contactBatch.DateModified
                    };
                    cmd.Parameters.Add(paramDateModified);

                    SqlParameter paramStatus = new SqlParameter
                    {
                        ParameterName = "@Status",
                        Value = contactBatch.Status
                    };
                    cmd.Parameters.Add(paramStatus);

                    SqlParameter outputParam = new SqlParameter();
                    outputParam.ParameterName = "@BatchID";
                    outputParam.Direction = ParameterDirection.Output;
                    outputParam.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(outputParam);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    batchID = Convert.ToInt32(cmd.Parameters["@BatchID"].Value);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }

            return batchID;
        }
        public static void PostToDatabase(List<ContactVM> contacts, int batchId)
        {
            string connString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                foreach (ContactVM contact in contacts)
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("spCreateContact", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter paramFirstName = new SqlParameter
                        {
                            ParameterName = "@FirstName",
                            Value = contact.FirstName
                        };
                        cmd.Parameters.Add(paramFirstName);

                        SqlParameter paramLastName = new SqlParameter
                        {
                            ParameterName = "@LastName",
                            Value = contact.LastName
                        };
                        cmd.Parameters.Add(paramLastName);

                        SqlParameter paramEmail = new SqlParameter
                        {
                            ParameterName = "@Email",
                            Value = contact.Email
                        };
                        cmd.Parameters.Add(paramEmail);

                        SqlParameter paramTelephone = new SqlParameter
                        {
                            ParameterName = "@Telephone",
                            Value = contact.Telephone
                        };
                        cmd.Parameters.Add(paramTelephone);

                        SqlParameter paramMobile = new SqlParameter
                        {
                            ParameterName = "@Mobile",
                            Value = contact.Mobile
                        };
                        cmd.Parameters.Add(paramMobile);

                        SqlParameter paramCompanyID = new SqlParameter
                        {
                            ParameterName = "@CompanyID",
                            Value = contact.CompanyID
                        };
                        cmd.Parameters.Add(paramCompanyID);

                        SqlParameter paramBatchID = new SqlParameter
                        {
                            ParameterName = "@BatchID",
                            Value = batchId
                        };
                        cmd.Parameters.Add(paramBatchID);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                    }
                }

            }
        }
    }
}