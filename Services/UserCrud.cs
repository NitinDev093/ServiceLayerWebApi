using Microsoft.AspNetCore.Mvc;
using ServiceLayerWebApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace ServiceLayerWebApi.Services
{
    public class UserCrud
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;
        public UserCrud()
        {
        }
        public UserCrud(IConfiguration config)
        {
            _configuration = config;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        [Route("InsertUser")]
        [HttpPost]
        public ApiResponseModel<string> InsertUser(UserRequestModel requestModel)
        {
            ApiResponseModel<string> response = new ApiResponseModel<string>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_insertUsers", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", requestModel.Name);
                cmd.Parameters.AddWithValue("@email", requestModel.Email);
                cmd.Parameters.AddWithValue("@password", requestModel.Password);
                cmd.Parameters.AddWithValue("@age", requestModel.Age);
                conn.Open();
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    response.IsSuccess = true;
                    response.Message = "User inserted successfully";
                    return response;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Failed to insert user";
                    return response;
                }
            }
        }

        [Route("GetUsers")]
        [HttpGet]
        public ApiResponseModel<List<DataBaseResponseModel>> Getusers()
        {
            try
            {
                ApiResponseModel<List<DataBaseResponseModel>> response = new ApiResponseModel<List<DataBaseResponseModel>>();
                List<DataBaseResponseModel> users = new List<DataBaseResponseModel>();
                using (SqlConnection sqlcon = new(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("usp_insertUsersTABLE", sqlcon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    sqlcon.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DataBaseResponseModel user = new DataBaseResponseModel();
                        user.Id = Convert.ToInt32(rdr["id"]);
                        user.Name = rdr["name"].ToString();
                        user.Email = rdr["email"].ToString();
                        user.Password = rdr["password"].ToString();
                        user.Age = Convert.ToInt32(rdr["age"]);
                        users.Add(user);
                    }
                    if (users.Count > 0 && users != null)
                    {
                        response.IsSuccess = true;
                        response.Message = "Users retrieved successfully";
                        response.Data = users;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "No users found";
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
