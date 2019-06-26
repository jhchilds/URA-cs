using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ThingMagic.URA2
{
    class Assets
    {
        //Getter and Setters
        public int id { get; set; }
        public string lane_direction { get; set;}
        public string position_code { get; set; } 
        public string route_suffix { get; set; } 
        public float marker { get; set; } 
        public string city { get; set; } 
        public string county { get; set; } 
        public int district { get; set; } 
        public string streetname { get; set; } 
        public string mutcd_code { get; set; } 
        public int retired { get; set; } 
        public DateTime replaced { get; set; } 
        public int sign_age { get; set; } 
        public string twn_tid { get; set; } 
        public float twn_mi { get; set; } 
        public int qc_flag { get; set; } 
        public float min_twn_fm { get; set; } 
        public float max_twn_tm { get; set; } 
        public string sr_sid { get; set; } 
        public int sign_height { get; set; } 
        public int sign_width { get; set; } 
        

        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        /// <summary>
        /// Selects all information for a Specific Asset (for future use)
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public DataTable Select(Assets asset)
        {
            ///Database Connection
            SqlConnection conn = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();
            try
            {
                //SQL Query to select from database
                string sql = "SELECT * FROM asset WHERE id = @id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", asset.id);



                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                conn.Open();
                adapter.Fill(dt);

                //Testing datatable fill
                foreach (DataRow dataRow in dt.Rows)
                {
                    foreach (var item in dataRow.ItemArray)
                    {
                        Console.WriteLine(item);
                    }
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        //Insert Data into database
        public bool Insert(Assets asset)
        {


            //Creating Default return type and setting value to false
            bool isSuccess = false;

            //Connect to databse
            SqlConnection conn = new SqlConnection(myconnstrng);
            try
            {
                //Create SQL Query for inserting data
                string sql = "INSERT INTO asset (lane_direction, position_code, route_suffix," +
                    " marker, city, county, district, streetname, mutcd_code," +
                    " retired, replaced, sign_age, twn_tid, twn_mi, qc_flag," +
                    " min_twn_fm, max_twn_tm, sr_sid, sign_height, sign_width)" +
                    " VALUES (@lane_direction, @position_code, @route_suffix," +
                    " @marker, @city, @county, @district, @streetname, @mutcd_code," +
                    " @retired, @replaced, @sign_age, @twn_tid, @twn_mi, @qc_flag," +
                    " @min_twn_fm, @max_twn_tm, @sr_sid, @sign_height, @sign_width)";


                SqlCommand cmd = new SqlCommand(sql, conn);
                //Parameters for adding data to databse
                cmd.Parameters.AddWithValue("@lane_direction", asset.lane_direction);
                cmd.Parameters.AddWithValue("@position_code", asset.position_code);
                cmd.Parameters.AddWithValue("@route_suffix", asset.route_suffix);
                cmd.Parameters.AddWithValue("@marker", asset.city);
                cmd.Parameters.AddWithValue("@county", asset.county);
                cmd.Parameters.AddWithValue("@district", asset.district);
                cmd.Parameters.AddWithValue("@streetname", asset.streetname);
                cmd.Parameters.AddWithValue("@mutcd_code", asset.mutcd_code);
                cmd.Parameters.AddWithValue("@retired", asset.retired);
                cmd.Parameters.AddWithValue("@replaced", asset.replaced);
                cmd.Parameters.AddWithValue("@sign_age", asset.sign_age);
                cmd.Parameters.AddWithValue("@twn_tid", asset.twn_tid);
                cmd.Parameters.AddWithValue("@twn_mi", asset.twn_mi);
                cmd.Parameters.AddWithValue("@qc_flag", asset.qc_flag);
                cmd.Parameters.AddWithValue("@min_twn_fm", asset.min_twn_fm);
                cmd.Parameters.AddWithValue("@max_twn_tm", asset.max_twn_tm);
                cmd.Parameters.AddWithValue("@sr_sid", asset.sr_sid);
                cmd.Parameters.AddWithValue("@sign_height", asset.sign_height);
                cmd.Parameters.AddWithValue("@sign_width", asset.sign_width);
             



                //Connection Open Here
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                //If query is successful then value of rows will be > 0 else value will be 0
                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
            return isSuccess;
        }

        //Update Method 
        public bool Update(Assets asset)
        {
            //Create default 
            bool isSuccess = false;

            //Create sql connection
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //Update database values
                string sql = "UPDATE asset SET lane_direction=@lane_direction," +
                    " position_code=@position_code, route_suffix=@route_suffix," +
                    " marker=@marker, city=@city, county=@county," +
                    " district=@district, streetname=@streetname," +
                    " mutcd_code=@mutcd_code, retired=@retired," +
                    " replaced=@replaced, sign_age=@sign_age," +
                    " twn_tid=@twn_tid, twn_mi=@twn_mi," +
                    " qc_flag=@qc_flag, min_twn_fm=@min_twn_fm," +
                    " max_twn_tm=@max_twn_tm, sr_sid=@sr_sid," +
                    " sign_height=@sign_height, sign_width=@sign_width WHERE id=@id";

                //SQL Command
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Parameters to add value
                cmd.Parameters.AddWithValue("id", asset.id);
                cmd.Parameters.AddWithValue("lane_direction", asset.lane_direction);
                cmd.Parameters.AddWithValue("position_code", asset.position_code);
                cmd.Parameters.AddWithValue("route_suffix", asset.route_suffix);
                cmd.Parameters.AddWithValue("marker", asset.city);
                cmd.Parameters.AddWithValue("county", asset.county);
                cmd.Parameters.AddWithValue("district", asset.district);
                cmd.Parameters.AddWithValue("streetname", asset.streetname);
                cmd.Parameters.AddWithValue("mutcd_code", asset.mutcd_code);
                cmd.Parameters.AddWithValue("retired", asset.retired);
                cmd.Parameters.AddWithValue("replaced", asset.replaced);
                cmd.Parameters.AddWithValue("sign_age", asset.sign_age);
                cmd.Parameters.AddWithValue("twn_tid", asset.twn_tid);
                cmd.Parameters.AddWithValue("twn_mi", asset.twn_mi);
                cmd.Parameters.AddWithValue("qc_flag", asset.qc_flag);
                cmd.Parameters.AddWithValue("min_twn_fm", asset.min_twn_fm);
                cmd.Parameters.AddWithValue("max_twn_tm", asset.max_twn_tm);
                cmd.Parameters.AddWithValue("sr_sid", asset.sr_sid);
                cmd.Parameters.AddWithValue("sign_height", asset.sign_height);
                cmd.Parameters.AddWithValue("sign_width", asset.sign_width);
                //Open Connection
                conn.Open();

                int rows = cmd.ExecuteNonQuery();
                //If rows greater than zero than successful query

                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
            return isSuccess;
        }

        /// <summary>
        /// Delete data from the database
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool Delete(Assets asset)
        {
            //Create default return value
            bool isSuccess = false;

            //SQL Connection
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //Delete from database
                string sql = "DELETE FROM asset WHERE id = @id";
                //Sql Command
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", asset.id);
                //Open Connection to server
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                //If query is successful then rows > 0
                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }

            return isSuccess;
        }

    }
}
