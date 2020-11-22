using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.SessionState;

namespace UploadExcelFile.Models
{
    [SessionState(SessionStateBehavior.Required)]
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
                        ParameterName = "@BatchID",
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

        public static List<ContactVM> DeleteContactByBatchId(int id)
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
                        ParameterName = "@BatchID",
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
                                CompanyID = Convert.ToInt32(sdr["CompanyID"]),
                                BatchId = Convert.ToInt32(sdr["BatchID"])
                            });
                        }
                    }
                }
                catch (Exception ex)
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

        public static void DeleteFileByBatchId(int id)
        {
            string connString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("spDeleteFileByBatchId", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter paramBatchId = new SqlParameter
                    {
                        ParameterName = "@BatchID",
                        Value = id
                    };
                    cmd.Parameters.Add(paramBatchId);

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

        public static List<ContactVM> EditContactByBatchId(int id)
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
                        ParameterName = "@BatchID",
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
                                CompanyID = Convert.ToInt32(sdr["CompanyID"]),
                                BatchId = Convert.ToInt32(sdr["BatchID"])
                            });
                        }
                    }
                }
                catch (Exception ex)
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

        public static void UpdateContactByBatchId(List<ContactVM> contacts, int batchId)
        {
            string connString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                foreach (ContactVM contact in contacts)
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("spUpdateContactByBatchId", con);
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
                    catch (Exception ex)
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
