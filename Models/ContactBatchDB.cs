using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace UploadExcelFile.Models
{
    public class ContactBatchDB
    {
        public static List<ContactBatch> GetAllBatches()
        {
            string connString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            List<ContactBatch> contactBatches = new List<ContactBatch>();
            using (SqlConnection con = new SqlConnection(connString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetAllBatches", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            contactBatches.Add(new ContactBatch
                            {
                                BatchID = Convert.ToInt32(sdr["BatchID"]),
                                BatchName = sdr["BatchName"].ToString(),
                                DateCreated = Convert.ToDateTime(sdr["DateCreated"]),
                                CreatedBy = sdr["CreatedBy"].ToString()
                            });
                        }
                    }
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

            return contactBatches;
        }

        public static List<ContactVM> GetContactByBatchId(int id)
        {
            string connString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            List<ContactVM> contactVM = new List<ContactVM>();
            using (SqlConnection con = new SqlConnection(connString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetContactByBatches", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter paramBatchId = new SqlParameter
                    {
                        ParameterName = "BatchID",
                        Value = id
                    };
                    cmd.Parameters.Add(paramBatchId);

                    con.Open();

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            contactVM.Add(new ContactVM
                            {
                                FirstName = sdr["FirstName"].ToString(),
                                LastName = sdr["LastName"].ToString(),
                                Email = sdr["Email"].ToString(),
                                Telephone = sdr["Telephone"].ToString(),
                                Mobile = sdr["Mobile"].ToString(),
                                CompanyID = Convert.ToInt32(sdr["CompanyID"])
                            });
                        }
                    }
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

            return contactVM;
        }
    }
}