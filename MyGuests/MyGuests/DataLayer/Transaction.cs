using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MySql.Data.MySqlClient;

namespace MyGuests.DL
{
    /// <summary>
    /// Summary description for Transaction
    /// </summary>
    public class dlTransaction
    {
        object ret_result = 1;
        //getting Connection 
        MySqlConnection DBconn = new MySqlConnection(ConfigurationManager.ConnectionStrings["MyGuests"].ToString());
        MySqlCommand com = new MySqlCommand();
        MySqlTransaction DBTransaction = null;
        //Description for ExecuteNonQuery()
        //Functionality: It runs ExecuteNonQuery function for 
        //               Inserttion/Updation/Deletion of Data
        //Parameters: (String1, Array of Objects)
        // String is the Stored procedure Name
        // The array is the Collection of Name,DataType(DB),Value of the InputParameters
        // These three Properties of repeat for 'N' no of Input Parameter
        // The last two elements of this array will be OuptputParameterName, Its DataType(DB)
        // only if the output Parameter is required for that SP
        //The length of Parameters string array = 3 x No of input parameters + 2 if there is an output param for sp
        //The function returns the Output Parameter Value (if it exists)
        // returns '1' or '-1' based on success/failure of transaction (if there is no Output parameter for sp 
        public object dlExecuteNonQuery(string query, params object[] parameters)
        {
            string outparam_name;
            outparam_name = setComProperties(ref com, query, parameters);
            try
            {
                //opening connection 
                if (DBconn.State == ConnectionState.Closed)
                    DBconn.Open();
                //DBTransaction = DBconn.BeginTransaction();
                //starting Transaction 
                com.ExecuteNonQuery();
                //DBTransaction.Commit();
                ////Getting the return value from SP
                if (parameters.Length % 3 == 2)
                    ret_result = com.Parameters[outparam_name].Value;
            }
            catch (MySqlException ex1)
            {
                if (DBTransaction != null)
                    DBTransaction.Rollback();
                ret_result = -1;
                return 1;
                throw;
            }
            catch (DBConcurrencyException ex2)
            {
                ret_result = -1;
                throw;
            }
            catch (Exception ex3)
            {
                ret_result = -1;
                // throw;
            }
            finally
            {
                if (DBconn.State != ConnectionState.Closed)
                    DBconn.Close();

            }
            return ret_result;

        }


        //Description for FillDataSet()
        //Functionality: It runs Fill() function for 
        //               getting the data from DB using SELECT Statements
        //Parameters: (String1, Array of Objects)
        // String is the Stored procedure Name
        // The array is the Collection of Name,DataType(DB),Value of the InputParameters
        // These three Properties of repeat for 'N' no of Input Parameter
        //The length of Parameters string array = 3 x No of input parameters
        //The function returns result set from DB in the form of a DataSet object
        // returns a null dataset on exceptions

        public DataSet dlFillDataSet(string query, params object[] parameters)
        {

            string outparam_name;
            outparam_name = setComProperties(ref com, query, parameters);
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter(com);
            try
            {
                da.Fill(ds);
            }
            catch (MySqlException)
            {
                ds = null;
                throw;
            }
            catch (DBConcurrencyException)
            {
                ds = null;
                throw;
            }
            catch
            {
                ds = null;
                throw;
            }
            finally
            {
                if (DBconn.State != ConnectionState.Closed)
                    DBconn.Close();
            }

            return ds;

        }
        // dlBulkCopy Requires Query: SP Name
        //                     Values: The values of the parameters in a DataTable
        //                     an Object String Containing the 'n' number of parameternames(column names in DataTable and itz type (comma separated)
        //      Example: if SP name is 'UpdateEmployeeTable'
        //      and DataTable 'Employee' contains,
        //        EmployeeId        EmployeeName
        //          101             Anant Patil
        //Then call this function like : dlBulkCopy("UpdateEmployeeTable",Employee, "EmployeeId","INT","EmployeeName","VARCHAR"

        public object dlBulkCopy(string query, DataTable Values, params object[] paramNames)
        {
            ret_result = "";
            string outparam_name;
            object[] parameters = new object[paramNames.Length / 2 * 3];
            object[] parameters1 = new object[parameters.Length - 1];

            try
            {
                //opening connection 
                if (DBconn.State == ConnectionState.Closed)
                    DBconn.Open();

                DBTransaction = DBconn.BeginTransaction();

                com.Transaction = DBTransaction;

                foreach (DataRow row in Values.Rows)
                {
                    for (int x = 0; x < parameters.Length; x += 3)
                    {
                        parameters[x] = paramNames[2 * x / 3];
                        parameters[x + 1] = paramNames[2 * x / 3 + 1];
                        parameters[x + 2] = (object)row[parameters[x].ToString()];
                    }
                    if (parameters[parameters.Length - 1].ToString().Equals("OUTPUT"))
                    {

                        for (int k = 0; k < parameters.Length - 1; k++)
                        {
                            parameters1[k] = parameters[k];
                        }
                        outparam_name = setComProperties(ref com, query, parameters1);
                    }
                    else
                    {
                        outparam_name = setComProperties(ref com, query, parameters);
                    }

                    if ((query.Split(' ')).Length > 2)
                        com.CommandType = CommandType.Text;

                    com.ExecuteNonQuery();

                    ////Getting the return value from SP
                    if (parameters1.Length % 3 == 2)
                    {
                        if (!outparam_name.Trim().Equals("No Output Param"))
                            ret_result = ret_result + "<br/>" + com.Parameters[outparam_name].Value.ToString();
                    }
                }
                DBTransaction.Commit();
            }
            catch (MySqlException)
            {
                if (DBTransaction != null)
                    DBTransaction.Rollback();
                ret_result = -1;
                throw;
            }
            catch (DBConcurrencyException)
            {
                ret_result = -1;
                throw;
            }
            catch
            {
                ret_result = -1;
                throw;
            }
            finally
            {
                if (DBconn.State != ConnectionState.Closed)
                    DBconn.Close();


            }
            return ret_result;
        }



        private string setComProperties(ref MySqlCommand com, string query, object[] parameters)
        {
            com.CommandText = query;
            com.CommandType = CommandType.StoredProcedure;
            com.Connection = DBconn;
            com.CommandTimeout = 6000;
            com.Parameters.Clear();
            // com.CommandTimeout = 1;
            //com = setComInputParameters();

            //-----------Adding Input Parameter----------------------------------// 
            //The length of Parameters string array = 3 x No of input parameters + 2 if there is an output param for sp
            //                                      = 3 x No of input parameters if there is no output param for sp
            if (parameters.Length > 0)
                for (int i = 0; i < parameters.Length - 2; i += 3)
                {
                    MySqlParameter input_params = new MySqlParameter();
                    input_params.ParameterName = parameters[i].ToString();
                    set_sqldbtype(ref input_params, parameters[i + 1].ToString());
                    input_params.Value = parameters[i + 2];
                    input_params.Direction = ParameterDirection.Input;
                    com.Parameters.Add(input_params);
                }

            //-----------Adding Output Parameter----------------------------------//
            // If the output parameter is not required for a Stored procedure, then dont add the o/p parameter
            // This can be checked by looking at the length of paramaters stringarray
            //
            string outparam_name = "No Output Param";
            if (parameters.Length > 0 && parameters.Length % 3 == 2)
            {
                MySqlParameter output_param = new MySqlParameter();
                outparam_name = parameters[parameters.Length - 2].ToString();
                output_param.ParameterName = outparam_name;
                set_sqldbtype(ref output_param, parameters[parameters.Length - 1].ToString());
                output_param.Direction = ParameterDirection.Output;
                com.Parameters.Add(output_param);
            }
            //  -------------------------------------------------------------------//       

            return outparam_name;
        }


        private void set_sqldbtype(ref MySqlParameter ioparameter, string data_type)
        {
            if (data_type.ToUpper() == "VARCHAR")
            {
                ioparameter.MySqlDbType = MySqlDbType.VarChar;
                ioparameter.Size = 5000;
            }
            else if (data_type.ToUpper() == "DATETIME")
                ioparameter.MySqlDbType = MySqlDbType.Datetime;
            else if (data_type.ToUpper() == "INT" || data_type.ToUpper() == "SYSTEM.DOUBLE")
                ioparameter.MySqlDbType = MySqlDbType.Int32;
            else if (data_type.ToUpper() == "INTEGER")
                ioparameter.MySqlDbType = MySqlDbType.Int32;
            else if (data_type.ToUpper() == "BIT")
                ioparameter.MySqlDbType = MySqlDbType.Bit;
            else if (data_type.ToUpper() == "DECIMAL")
                ioparameter.MySqlDbType = MySqlDbType.Decimal;
        }


        public object dlExecuteNonQuery_Text(DataTable scriptsTable)
        {
            com.CommandType = CommandType.Text;
            com.Connection = DBconn;
            com.CommandTimeout = 6000;


            try
            {
                //opening connection 
                if (DBconn.State == ConnectionState.Closed)
                    DBconn.Open();
                //starting Transaction 
                DBTransaction = DBconn.BeginTransaction();
                com.Transaction = DBTransaction;
                for (int rowNo = 0; rowNo < scriptsTable.Rows.Count; rowNo++)
                {
                    com.CommandText = Convert.ToString(scriptsTable.Rows[rowNo]["Script"]);
                    com.ExecuteNonQuery();
                }
                DBTransaction.Commit();
            }
            catch (Exception ex1)
            {
                if (DBTransaction != null)
                    DBTransaction.Rollback();
                ret_result = -1;
                throw;
            }
            finally
            {
                if (DBconn.State != ConnectionState.Closed)
                    DBconn.Close();

            }
            return ret_result;

        }
    }





}