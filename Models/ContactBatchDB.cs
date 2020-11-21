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

            return contactBatches;
        }

        public static List<ContactVM> GetContactByBatchId(int id)
        {

        }
    }
}