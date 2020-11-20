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
            //Instantiating a new COntactBatch object
            ContactBatch contactBatch = new ContactBatch();

            string conString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("spGetAllBatches", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Getting the BatchId
                    SqlParameter batchIdOutput = new SqlParameter();
                    batchIdOutput.ParameterName = "@BatchID";
                    batchIdOutput.Direction = ParameterDirection.Output;
                    batchIdOutput.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(batchIdOutput);

                    //Getting the DateCreated
                    SqlParameter dateCreatedOutput = new SqlParameter();
                    dateCreatedOutput.ParameterName = "@DateCreated";
                    dateCreatedOutput.Direction = ParameterDirection.Output;
                    dateCreatedOutput.SqlDbType = SqlDbType.DateTime;
                    cmd.Parameters.Add(dateCreatedOutput);

                    //Getting the createdBy
                    SqlParameter CreatedByOutput = new SqlParameter();
                    CreatedByOutput.ParameterName = "@CreatedBy";
                    CreatedByOutput.Direction = ParameterDirection.Output;
                    CreatedByOutput.SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.Add(CreatedByOutput);

                    //Getting the dateModified
                    SqlParameter dateModifiedOutput = new SqlParameter();
                    dateModifiedOutput.ParameterName = "@DateModified";
                    dateModifiedOutput.Direction = ParameterDirection.Output;
                    dateModifiedOutput.SqlDbType = SqlDbType.DateTime;
                    cmd.Parameters.Add(dateModifiedOutput);

                    //Getting the Status field
                    SqlParameter statusOutput = new SqlParameter();
                    statusOutput.ParameterName = "@Status";
                    statusOutput.Direction = ParameterDirection.Output;
                    statusOutput.SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.Add(statusOutput);

                    //Getting the BatchName field
                    SqlParameter batchNameOutput = new SqlParameter();
                    batchNameOutput.ParameterName = "@BatchName";
                    batchNameOutput.Direction = ParameterDirection.Output;
                    batchNameOutput.SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.Add(batchNameOutput);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    contactBatch.BatchID = Convert.ToInt32(cmd.Parameters["@BatchID"].Value);
                    contactBatch.DateCreated = Convert.ToDateTime(cmd.Parameters["@DateCreated"].Value);
                    contactBatch.CreatedBy = Convert.ToString(cmd.Parameters["@CreatedBy"].Value);
                    contactBatch.DateModified = Convert.ToDateTime(cmd.Parameters["@DateModified"].Value);
                    contactBatch.Status = Convert.ToString(cmd.Parameters["@Status"].Value);
                    contactBatch.BatchName = Convert.ToString(cmd.Parameters["@BatchName"].Value);
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

            return new List<ContactBatch>((IEnumerable<ContactBatch>)contactBatch);
        }
    }
}