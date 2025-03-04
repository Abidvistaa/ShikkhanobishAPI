﻿using Flurl.Http;
using SHikkhanobishAPI.Models;
using SHikkhanobishAPI.Models.Shikkhanobish;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace SHikkhanobishAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ShikkhanobishLoginController : ApiController
    {
        private SqlConnection conn;
        public const int SchoolCost = 3;
        public const int CollegeCost = 4;
        public const double processignCostPercent = 0.2;
        public const int TeacherMonetizationTime = 15;
   

        public void Connection()
        {
            string conString = ConfigurationManager.ConnectionStrings["getConnection"].ToString();
            conn = new SqlConnection(conString);
        }
        public float CalculateRatting(float fs, float fos, float th, float to, float on)
        {
            float toalRating;

            float totalSum = fs * 5 + fos * 4 + th * 3 + to * 2 + on;
            float totalNum = fs + fos + th + to + on;
            if(totalNum == 0)
            {
                toalRating = 0;
            }
            else
            {
                toalRating = totalSum / (fs + fos + th + to + on);
            }
           

            return toalRating;
        }
        #region Send Msg
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public async Task<SendSms> SendSmsAsync(SendSms obj)
        {
            importantKeyAndnfoTable allKey = new importantKeyAndnfoTable();
            SendSms res = new SendSms();
            try
            {

                string apiKey = "R3r40YEsmn1rfuHt49kQsvVuShUT8K/pNCWlYfOa+qI=";
                string clientID = "dc0b42d1-e973-463c-baa1-088ee1934fb5";
                string senderID = "8804445620740";


                string uri = "https://api.smsq.global/api/v2/SendSMS?ApiKey="+ apiKey + "&ClientId="+ clientID + "&SenderId="+ senderID + "&Message="+ obj.msg+ "&MobileNumbers="+obj.number;
                //string ull = "http://services.smsq.global/sms/api?action=send-sms&api_key="+ apiKey +"&to= " + obj.number + "&from=8804445620753&sms=" + obj.msg;
                res = await uri.GetJsonAsync<SendSms>();
                if(res.code == "ok")
                {
                    res.respose = "ok";
                }
                else
                {
                    res.respose = allKey.smsApiKey;
                }
            }
            catch
            {                
                res.code = "not ok";
            }
            return res;
        }
        

        #endregion

        #region Student
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<Student> getStudent()
        {
            List<Student> objRList = new List<Student>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getStudent", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Student objAdd = new Student();
                    objAdd.studentID = Convert.ToInt32(reader["studentID"]);
                    objAdd.phonenumber = reader["phonenumber"].ToString();
                    objAdd.password = reader["password"].ToString();
                    objAdd.totalSpent = Convert.ToInt32(reader["totalSpent"]);
                    objAdd.totalTuitionTime = Convert.ToInt32(reader["totalTuitionTime"]);
                    objAdd.coin = Convert.ToInt32(reader["coin"]);
                    objAdd.freemin = Convert.ToInt32(reader["freemin"]);
                    objAdd.city = reader["city"].ToString();
                    objAdd.name = reader["name"].ToString();
                    objAdd.institutionName = reader["institutionName"].ToString();
                    objAdd.fatherNumber = reader["fatherNumber"].ToString();
                    objAdd.motherNumber = reader["motherNumber"].ToString();
                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Student objAdd = new Student();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Student LoginStudent(Student obj)
        {
            Student objR = new Student();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("LoginStudent", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@phonenumber", obj.phonenumber);
                cmd.Parameters.AddWithValue("@password", obj.password);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.studentID = Convert.ToInt32(reader["studentID"]);
                    objR.phonenumber = reader["phonenumber"].ToString();
                    objR.password = reader["password"].ToString();
                    objR.totalSpent = Convert.ToInt32(reader["totalSpent"]);
                    objR.totalTuitionTime = Convert.ToInt32(reader["totalTuitionTime"]);
                    objR.coin = Convert.ToInt32(reader["coin"]);
                    objR.freemin = Convert.ToInt32(reader["freemin"]);
                    objR.city = reader["city"].ToString();
                    objR.name = reader["name"].ToString();
                    objR.institutionName = reader["institutionName"].ToString();
                    objR.fatherNumber = reader["fatherNumber"].ToString();
                    objR.motherNumber = reader["motherNumber"].ToString();

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Student getStudentWithID(Student obj)
        {
            Student objR = new Student();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getStudentWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.studentID = Convert.ToInt32(reader["studentID"]);
                    objR.phonenumber = reader["phonenumber"].ToString();
                    objR.password = reader["password"].ToString();
                    objR.totalSpent = Convert.ToInt32(reader["totalSpent"]);
                    objR.totalTuitionTime = Convert.ToInt32(reader["totalTuitionTime"]);
                    objR.coin = Convert.ToInt32(reader["coin"]);
                    objR.freemin = Convert.ToInt32(reader["freemin"]);
                    objR.city = reader["city"].ToString();
                    objR.name = reader["name"].ToString();
                    objR.institutionName = reader["institutionName"].ToString();
                    objR.fatherNumber = reader["fatherNumber"].ToString();
                    objR.motherNumber = reader["motherNumber"].ToString();
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setStudent(Student obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setStudent", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                cmd.Parameters.AddWithValue("@phonenumber", obj.phonenumber);
                cmd.Parameters.AddWithValue("@password", obj.password);
                cmd.Parameters.AddWithValue("@totalSpent", obj.totalSpent);
                cmd.Parameters.AddWithValue("@totalTuitionTime", obj.totalTuitionTime);
                cmd.Parameters.AddWithValue("@coin", obj.coin);
                cmd.Parameters.AddWithValue("@freemin", obj.freemin);
                cmd.Parameters.AddWithValue("@city", obj.city);
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@institutionName", obj.institutionName);
                cmd.Parameters.AddWithValue("@fatherNumber", obj.fatherNumber);
                cmd.Parameters.AddWithValue("@motherNumber", obj.motherNumber);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response updateStudent(Student obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("updateStudent", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                cmd.Parameters.AddWithValue("@phonenumber", obj.phonenumber);
                cmd.Parameters.AddWithValue("@password", obj.password);
                cmd.Parameters.AddWithValue("@totalSpent", obj.totalSpent);
                cmd.Parameters.AddWithValue("@totalTuitionTime", obj.totalTuitionTime);
                cmd.Parameters.AddWithValue("@coin", obj.coin);
                cmd.Parameters.AddWithValue("@freemin", obj.freemin);
                cmd.Parameters.AddWithValue("@city", obj.city);
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@institutionName", obj.institutionName);
                cmd.Parameters.AddWithValue("@fatherNumber", obj.fatherNumber);
                cmd.Parameters.AddWithValue("@motherNumber", obj.motherNumber);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        #endregion


        #region Chapter
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<Chapter> getChapter()
        {
            List<Chapter> objRList = new List<Chapter>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getChapter", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Chapter objAdd = new Chapter();
                    objAdd.subjectID = Convert.ToInt32(reader["subjectID"]);
                    objAdd.chapterID = Convert.ToInt32(reader["chapterID"]);
                    objAdd.classID = Convert.ToInt32(reader["classID"]);
                    objAdd.title = reader["title"].ToString();
                    objAdd.avgRatting = Convert.ToDouble(reader["avgRatting"]);
                    objAdd.name = reader["name"].ToString();
                    objAdd.tuitionRequest = Convert.ToInt32(reader["tuitionRequest"]);
                    objAdd.indexNo = Convert.ToInt32(reader["indexNo"]);
                    objAdd.description = reader["description"].ToString();
                    objAdd.purchaseRate = Convert.ToInt32(reader["purchaseRate"]);
                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Chapter objAdd = new Chapter();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Chapter getChapterWithID(Chapter obj)
        {
            Chapter objR = new Chapter();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getChapterWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@chapterID", obj.chapterID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.subjectID = Convert.ToInt32(reader["subjectID"]);
                    objR.chapterID = Convert.ToInt32(reader["chapterID"]);
                    objR.classID = Convert.ToInt32(reader["classID"]);
                    objR.title = reader["title"].ToString();
                    objR.name = reader["name"].ToString();
                    objR.tuitionRequest = Convert.ToInt32(reader["tuitionRequest"]);
                    objR.indexNo = Convert.ToInt32(reader["indexNo"]);
                    objR.description = reader["description"].ToString();
                    objR.purchaseRate = Convert.ToInt32(reader["purchaseRate"]);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setChapter(Chapter obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setChapter", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@subjectID", obj.subjectID);
                cmd.Parameters.AddWithValue("@chapterID", obj.chapterID);
                cmd.Parameters.AddWithValue("@classID", obj.classID);
                cmd.Parameters.AddWithValue("@title", obj.title);
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@tuitionRequest", 0);
                cmd.Parameters.AddWithValue("@avgRatting", 0);
                cmd.Parameters.AddWithValue("@indexNo", 0);
                cmd.Parameters.AddWithValue("@description", "This is a description");
                cmd.Parameters.AddWithValue("@purchaseRate", 500);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response updateChapter(Chapter obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("updateChapter", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@subjectID", obj.subjectID);
                cmd.Parameters.AddWithValue("@chapterID", obj.chapterID);
                cmd.Parameters.AddWithValue("@classID", obj.classID);
                cmd.Parameters.AddWithValue("@title", obj.title);
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@tuitionRequest", 0);
                cmd.Parameters.AddWithValue("@avgRatting", 0);
                cmd.Parameters.AddWithValue("@indexNo", 0);
                cmd.Parameters.AddWithValue("@description", obj.description);
                cmd.Parameters.AddWithValue("@purchaseRate", obj.purchaseRate);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        #endregion


        #region Topic



        [System.Web.Http.AcceptVerbs("GET", "POST")]

        public Response setTopic(Topic obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setTopic", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@topicID", obj.topicID);
                cmd.Parameters.AddWithValue("@chapterID", obj.chapterID);
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@description", obj.description);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]

        public Response setTopicWithID(Topic obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setTopicWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@topicID", obj.topicID);
                cmd.Parameters.AddWithValue("@chapterID", obj.chapterID);
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@description", obj.description);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<Topic> getTopic()
        {
            List<Topic> objRList = new List<Topic>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getTopic", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Topic objAdd = new Topic();
                    objAdd.topicID = Convert.ToInt32(reader["topicID"]);
                    objAdd.chapterID = Convert.ToInt32(reader["chapterID"]);
                    objAdd.name = reader["name"].ToString();
                    objAdd.description = reader["description"].ToString();

                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Topic objAdd = new Topic();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Topic getTopicWithID(Topic obj)
        {
            Topic objR = new Topic();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getTopicWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@topicID", obj.topicID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.topicID = Convert.ToInt32(reader["topicID"]);
                    objR.chapterID = Convert.ToInt32(reader["chapterID"]);
                    objR.name = reader["name"].ToString();
                    objR.description = reader["description"].ToString();

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        #endregion

        #region ClassInfo
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<ClassInfo> getClassInfo()
        {
            List<ClassInfo> objRList = new List<ClassInfo>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getClassInfo", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ClassInfo objAdd = new ClassInfo();
                    objAdd.institutionID = Convert.ToInt32(reader["institutionID"]);
                    objAdd.classID = Convert.ToInt32(reader["classID"]);
                    objAdd.title = reader["title"].ToString();
                    objAdd.name = reader["name"].ToString();
                    objAdd.avgRatting = Convert.ToDouble(reader["avgRatting"]);
                    objAdd.tuitionRequest = Convert.ToInt32(reader["tuitionRequest"]);
                    objAdd.indexNo = Convert.ToInt32(reader["indexNo"]);
                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                ClassInfo objAdd = new ClassInfo();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public ClassInfo getClassInfoWithID(ClassInfo obj)
        {
            ClassInfo objR = new ClassInfo();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getClassInfoWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@institutionID", obj.institutionID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.institutionID = Convert.ToInt32(reader["institutionID"]);
                    objR.classID = Convert.ToInt32(reader["classID"]);
                    objR.title = reader["title"].ToString();
                    objR.name = reader["name"].ToString();
                    objR.tuitionRequest = Convert.ToInt32(reader["tuitionRequest"]);
                    objR.indexNo = Convert.ToInt32(reader["indexNo "]);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setClassInfo(ClassInfo obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setClassInfo", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@institutionID", obj.institutionID);
                cmd.Parameters.AddWithValue("@classID", obj.classID);
                cmd.Parameters.AddWithValue("@title", obj.title);
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@tuitionRequest", 0);
                cmd.Parameters.AddWithValue("@avgRatting", 0);
                cmd.Parameters.AddWithValue("@indexNo ", 0);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response updateClassInfo(ClassInfo obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("updateClassInfo", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@institutionID", obj.institutionID);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        #endregion


        #region Course
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<Course> getCourse()
        {
            List<Course> objRList = new List<Course>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getCourse", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Course objAdd = new Course();
                    objAdd.degreeID = Convert.ToInt32(reader["degreeID"]);
                    objAdd.courseID = Convert.ToInt32(reader["courseID"]);
                    objAdd.title = reader["title"].ToString();
                    objAdd.name = reader["name"].ToString();
                    objAdd.tuitionRequest = Convert.ToInt32(reader["tuitionRequest"]);
                    objAdd.avgRatting = Convert.ToDouble(reader["avgRatting"]);
                    objAdd.uniNameID = Convert.ToInt32(reader["uniNameID"]);
                    objAdd.indexNo = Convert.ToInt32(reader["indexNo"]);
                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Course objAdd = new Course();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Course getCourseWithID(Course obj)
        {
            Course objR = new Course();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getCourseWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@degreeID", obj.degreeID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.degreeID = Convert.ToInt32(reader["degreeID"]);
                    objR.courseID = Convert.ToInt32(reader["courseID"]);
                    objR.title = reader["title"].ToString();
                    objR.name = reader["name"].ToString();
                    objR.tuitionRequest = Convert.ToInt32(reader["tuitionRequest"]);
                    objR.uniNameID = Convert.ToInt32(reader["uniNameID"]);
                    objR.indexNo = Convert.ToInt32(reader["indexNo "]);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setCourse(Course obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setCourse", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@degreeID", obj.degreeID);
                cmd.Parameters.AddWithValue("@courseID", obj.courseID);
                cmd.Parameters.AddWithValue("@title", obj.title);
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@tuitionRequest", obj.tuitionRequest);
                cmd.Parameters.AddWithValue("@avgRatting", obj.avgRatting);
                cmd.Parameters.AddWithValue("@uniNameID", obj.uniNameID);
                cmd.Parameters.AddWithValue("@indexNo ", obj.indexNo);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response updateCourse(Course obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("updateCourse", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@degreeID", obj.degreeID);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        #endregion


        #region Degree
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<Degree> getDegree()
        {
            List<Degree> objRList = new List<Degree>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getDegree", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Degree objAdd = new Degree();
                    objAdd.uniNameID = Convert.ToInt32(reader["uniNameID"]);
                    objAdd.degreeID = Convert.ToInt32(reader["degreeID"]);
                    objAdd.title = reader["title"].ToString();
                    objAdd.name = reader["name"].ToString();
                    objAdd.avgRatting = Convert.ToDouble(reader["avgRatting"]);
                    objAdd.tuitionRequest = Convert.ToInt32(reader["tuitionRequest"]);
                    objAdd.indexNo = Convert.ToInt32(reader["indexNo"]);
                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Degree objAdd = new Degree();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Degree getDegreeWithID(Degree obj)
        {
            Degree objR = new Degree();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getDegreeWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@uniNameID", obj.uniNameID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.uniNameID = Convert.ToInt32(reader["uniNameID"]);
                    objR.degreeID = Convert.ToInt32(reader["degreeID"]);
                    objR.title = reader["title"].ToString();
                    objR.name = reader["name"].ToString();
                    objR.tuitionRequest = Convert.ToInt32(reader["tuitionRequest"]);
                    objR.indexNo = Convert.ToInt32(reader["indexNo "]);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setDegree(Degree obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setDegree", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@uniNameID", obj.uniNameID);
                cmd.Parameters.AddWithValue("@degreeID", obj.degreeID);
                cmd.Parameters.AddWithValue("@title", obj.title);
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@tuitionRequest", obj.tuitionRequest);
                cmd.Parameters.AddWithValue("@avgRatting", obj.avgRatting);
                cmd.Parameters.AddWithValue("@indexNo ", obj.indexNo);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response updateDegree(Degree obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("updateDegree", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@uniNameID", obj.uniNameID);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        #endregion


        #region Institution
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<Institution> getInstitution()
        {
            List<Institution> objRList = new List<Institution>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getInstitution", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Institution objAdd = new Institution();
                    objAdd.institutionID = Convert.ToInt32(reader["institutionID"]);
                    objAdd.title = reader["title"].ToString();
                    objAdd.name = reader["name"].ToString();
                    objAdd.tuitionRequest = Convert.ToInt32(reader["tuitionRequest"]);
                    objAdd.avgRatting = Convert.ToDouble(reader["avgRatting"]);
                    objAdd.indexNo = Convert.ToInt32(reader["indexNo"]);
                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Institution objAdd = new Institution();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Institution getInstitutionWithID(Institution obj)
        {
            Institution objR = new Institution();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getInstitutionWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@institutionID", obj.institutionID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.institutionID = Convert.ToInt32(reader["institutionID"]);
                    objR.title = reader["title"].ToString();
                    objR.name = reader["name"].ToString();
                    objR.tuitionRequest = Convert.ToInt32(reader["tuitionRequest"]);
                    objR.avgRatting = Convert.ToInt32(reader["avgRatting"]);
                    objR.indexNo = Convert.ToInt32(reader["indexNo "]);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setInstitution(Institution obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setInstitution", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@institutionID", obj.institutionID);
                cmd.Parameters.AddWithValue("@title", obj.title);
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@tuitionRequest", 0);
                cmd.Parameters.AddWithValue("@avgRatting", 0);
                cmd.Parameters.AddWithValue("@indexNo ", 0);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response updateInstitution(Institution obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("updateInstitution", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@institutionID", obj.institutionID);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        #endregion


        #region SelectPattern
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<SelectPattern> getSelectPattern()
        {
            List<SelectPattern> objRList = new List<SelectPattern>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getSelectPattern", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    SelectPattern objAdd = new SelectPattern();
                    objAdd.slPatternID = Convert.ToInt32(reader["slPatternID"]);
                    objAdd.firstIndex = Convert.ToInt32(reader["firstIndex"]);
                    objAdd.secondIndex = Convert.ToInt32(reader["secondIndex"]);
                    objAdd.thirdindex = Convert.ToInt32(reader["thirdindex"]);
                    objAdd.forthindex = Convert.ToInt32(reader["forthindex"]);
                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                SelectPattern objAdd = new SelectPattern();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public SelectPattern getSelectPatternWithID(SelectPattern obj)
        {
            SelectPattern objR = new SelectPattern();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getSelectPatternWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@slPatternID", obj.slPatternID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.slPatternID = Convert.ToInt32(reader["slPatternID"]);
                    objR.firstIndex = Convert.ToInt32(reader["firstIndex"]);
                    objR.secondIndex = Convert.ToInt32(reader["secondIndex"]);
                    objR.thirdindex = Convert.ToInt32(reader["thirdindex"]);
                    objR.forthindex = Convert.ToInt32(reader["forthindex"]);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setSelectPattern(SelectPattern obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setSelectPattern", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@slPatternID", obj.slPatternID);
                cmd.Parameters.AddWithValue("@firstIndex", obj.firstIndex);
                cmd.Parameters.AddWithValue("@secondIndex", obj.secondIndex);
                cmd.Parameters.AddWithValue("@thirdindex", obj.thirdindex);
                cmd.Parameters.AddWithValue("@forthindex", obj.forthindex);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response updateSelectPattern(SelectPattern obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("updateSelectPattern", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@slPatternID", obj.slPatternID);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        #endregion


        #region StudentPaymentHistory
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<StudentPaymentHistory> getStudentPaymentHistory()
        {
            List<StudentPaymentHistory> objRList = new List<StudentPaymentHistory>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getStudentPaymentHistory", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    StudentPaymentHistory objAdd = new StudentPaymentHistory();
                    objAdd.studentID = Convert.ToInt32(reader["studentID"]);
                    objAdd.paymentID = reader["paymentID"].ToString();
                    objAdd.date = reader["date"].ToString();
                    objAdd.trxID = reader["trxID"].ToString();
                    objAdd.amountTaka = Convert.ToInt32(reader["amountTaka"]);
                    objAdd.amountCoin = Convert.ToInt32(reader["amountCoin"]);
                    objAdd.medium = reader["medium"].ToString();
                    objAdd.cardID = reader["cardID"].ToString();
                    objAdd.isVoucherUsed = Convert.ToInt32(reader["isVoucherUsed"]);
                    objAdd.voucherID = Convert.ToInt32(reader["voucherID"]);
                    objAdd.name = reader["name"].ToString();
                    objAdd.description = reader["description"].ToString();
                    objAdd.type = reader["type"].ToString();
                    objAdd.invoiceID = reader["invoiceID"].ToString();
                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                StudentPaymentHistory objAdd = new StudentPaymentHistory();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<StudentPaymentHistory> getStudentPaymentHistoryWithID(StudentPaymentHistory obj)
        {
            List<StudentPaymentHistory> objRList = new List<StudentPaymentHistory>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getStudentPaymentHistoryWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    StudentPaymentHistory objR = new StudentPaymentHistory();
                    objR.studentID = Convert.ToInt32(reader["studentID"]);
                    objR.paymentID = reader["paymentID"].ToString();
                    objR.date = reader["date"].ToString();
                    objR.trxID = reader["trxID"].ToString();
                    objR.amountTaka = Convert.ToInt32(reader["amountTaka"]);
                    objR.amountCoin = Convert.ToInt32(reader["amountCoin"]);
                    objR.medium = reader["medium"].ToString();
                    objR.cardID = reader["cardID"].ToString();
                    objR.isVoucherUsed = Convert.ToInt32(reader["isVoucherUsed"]);
                    objR.name = reader["name"].ToString();
                    objR.voucherID = Convert.ToInt32(reader["voucherID"]);
                    objR.description = reader["description"].ToString();
                    objR.type = reader["type"].ToString();
                    objR.invoiceID = reader["invoiceID"].ToString();
                    objRList.Add(objR);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                StudentPaymentHistory objR = new StudentPaymentHistory();
                objR.Response = ex.Message;
                objRList.Add(objR);
            }
            return objRList;
        }

        

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<StudentPaymentHistory> getStudentPaymentHistoryWithInvoiceID(StudentPaymentHistory obj)
        {
            List<StudentPaymentHistory> objRList = new List<StudentPaymentHistory>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getStudentPaymentHistoryWithInvoiceID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@invoiceID", obj.invoiceID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    StudentPaymentHistory objR = new StudentPaymentHistory();
                    objR.studentID = Convert.ToInt32(reader["studentID"]);
                    objR.paymentID = reader["paymentID"].ToString();
                    objR.date = reader["date"].ToString();
                    objR.trxID = reader["trxID"].ToString();
                    objR.amountTaka = Convert.ToInt32(reader["amountTaka"]);
                    objR.amountCoin = Convert.ToInt32(reader["amountCoin"]);
                    objR.medium = reader["medium"].ToString();
                    objR.cardID = reader["cardID"].ToString();
                    objR.isVoucherUsed = Convert.ToInt32(reader["isVoucherUsed"]);
                    objR.name = reader["name"].ToString();
                    objR.voucherID = Convert.ToInt32(reader["voucherID"]);
                    objR.description = reader["description"].ToString();
                    objR.type = reader["type"].ToString();
                    objR.invoiceID = reader["invoiceID"].ToString();
                    objRList.Add(objR);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                StudentPaymentHistory objR = new StudentPaymentHistory();
                objR.Response = ex.Message;
                objRList.Add(objR);
            }
            return objRList;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setStudentPaymentHistory(StudentPaymentHistory obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setStudentPaymentHistory", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                cmd.Parameters.AddWithValue("@paymentID", obj.paymentID);
                cmd.Parameters.AddWithValue("@date", obj.date);
                cmd.Parameters.AddWithValue("@trxID", obj.trxID);
                cmd.Parameters.AddWithValue("@amountTaka", obj.amountTaka);
                cmd.Parameters.AddWithValue("@amountCoin", obj.amountCoin);
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@medium", obj.medium);
                cmd.Parameters.AddWithValue("@isVoucherUsed", obj.isVoucherUsed);
                cmd.Parameters.AddWithValue("@voucherID", obj.voucherID);
                cmd.Parameters.AddWithValue("@cardID", obj.cardID);
                cmd.Parameters.AddWithValue("@description", obj.description);
                cmd.Parameters.AddWithValue("@type", obj.type);
                cmd.Parameters.AddWithValue("@invoiceID", obj.invoiceID);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response updateStudentPaymentHistory(StudentPaymentHistory obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("updateStudentPaymentHistory", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                cmd.Parameters.AddWithValue("@paymentID", obj.paymentID);
                cmd.Parameters.AddWithValue("@date", obj.date);
                cmd.Parameters.AddWithValue("@trxID", obj.trxID);
                cmd.Parameters.AddWithValue("@amountTaka", obj.amountTaka);
                cmd.Parameters.AddWithValue("@amountCoin", obj.amountCoin);
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@medium", obj.medium);
                cmd.Parameters.AddWithValue("@isVoucherUsed", obj.isVoucherUsed);
                cmd.Parameters.AddWithValue("@voucherID", obj.voucherID);
                cmd.Parameters.AddWithValue("@cardID", obj.cardID);
                cmd.Parameters.AddWithValue("@description", obj.description);
                cmd.Parameters.AddWithValue("@type", obj.type);
                cmd.Parameters.AddWithValue("@invoiceID", obj.invoiceID);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        #endregion


        #region StudentTuitionHistory
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<StudentTuitionHistory> getStudentTuitionHistory()
        {
            List<StudentTuitionHistory> objRList = new List<StudentTuitionHistory>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getStudentTuitionHistory", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    StudentTuitionHistory objAdd = new StudentTuitionHistory();
                    objAdd.studentID = Convert.ToInt32(reader["studentID"]);
                    objAdd.tuitionID = reader["tuitionID"].ToString();
                    objAdd.time = reader["time"].ToString();
                    objAdd.teacherID = Convert.ToInt32(reader["teacherID"]);
                    objAdd.cost = Convert.ToInt32(reader["cost"]);
                    objAdd.ratting = Convert.ToInt32(reader["ratting"]);
                    objAdd.date = reader["date"].ToString();
                    objAdd.firstChoiceID = reader["firstChoiceID"].ToString();
                    objAdd.secondChoiceID = reader["secondChoiceID"].ToString();
                    objAdd.thirdChoiceID = reader["thirdChoiceID"].ToString();
                    objAdd.forthChoiceID = reader["forthChoiceID"].ToString();
                    objAdd.topicID = Convert.ToInt32(reader["topicID"]);
                    objAdd.isTextOrVideo = Convert.ToInt32(reader["isTextOrVideo"]);
                    objAdd.topicName = reader["topicName"].ToString();
                    objAdd.videoURL = reader["videoURL"].ToString();
                    objAdd.approval = Convert.ToInt32(reader["approval"]);
                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                StudentTuitionHistory objAdd = new StudentTuitionHistory();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<StudentTuitionHistory> getStudentTuitionHistoryWithID(StudentTuitionHistory obj)
        {
            List<StudentTuitionHistory> objRList = new List<StudentTuitionHistory>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getStudentTuitionHistoryWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    StudentTuitionHistory objR = new StudentTuitionHistory();
                    objR.studentID = Convert.ToInt32(reader["studentID"]);
                    objR.tuitionID = reader["tuitionID"].ToString();
                    objR.time = reader["time"].ToString();
                    objR.teacherID = Convert.ToInt32(reader["teacherID"]);
                    objR.cost = Convert.ToInt32(reader["cost"]);
                    objR.ratting = Convert.ToDouble(reader["ratting"]);
                    objR.firstChoiceID = reader["firstChoiceID"].ToString();
                    objR.secondChoiceID = reader["secondChoiceID"].ToString();
                    objR.thirdChoiceID = reader["thirdChoiceID"].ToString();
                    objR.forthChoiceID = reader["forthChoiceID"].ToString();
                    objR.studentName = reader["studentName"].ToString();
                    objR.teacherName = reader["teacherName"].ToString();
                    objR.date = reader["date"].ToString();
                    objR.firstChoiceName = reader["firstChoiceName"].ToString();
                    objR.secondChoiceName = reader["secondChoiceName"].ToString();
                    objR.thirdChoiceName = reader["thirdChoiceName"].ToString();
                    objR.forthChoiceName = reader["forthChoiceName"].ToString();
                    objR.teacherEarn = Convert.ToDouble(reader["teacherEarn"]);
                    objR.topicID = Convert.ToInt32(reader["topicID"]);
                    objR.isTextOrVideo = Convert.ToInt32(reader["isTextOrVideo"]);
                    objR.topicName = reader["topicName"].ToString();
                    objR.videoURL = reader["videoURL"].ToString();
                    objR.approval = Convert.ToInt32(reader["approval"]);
                    objRList.Add(objR);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                StudentTuitionHistory objR = new StudentTuitionHistory();
                objR.Response = ex.Message;
                objRList.Add(objR);
            }
            return objRList;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public StudentTuitionHistory getTuitionHistoryWithTuitionID(StudentTuitionHistory obj)
        {
            StudentTuitionHistory objR = new StudentTuitionHistory();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getTuitionHistoryWithTuitionID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tuitionID", obj.tuitionID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.studentID = Convert.ToInt32(reader["studentID"]);
                    objR.tuitionID = reader["tuitionID"].ToString();
                    objR.time = reader["time"].ToString();
                    objR.teacherID = Convert.ToInt32(reader["teacherID"]);
                    objR.cost = Convert.ToInt32(reader["cost"]);
                    objR.ratting = Convert.ToDouble(reader["ratting"]);
                    objR.firstChoiceID = reader["firstChoiceID"].ToString();
                    objR.secondChoiceID = reader["secondChoiceID"].ToString();
                    objR.thirdChoiceID = reader["thirdChoiceID"].ToString();
                    objR.forthChoiceID = reader["forthChoiceID"].ToString();
                    objR.studentName = reader["studentName"].ToString();
                    objR.teacherName = reader["teacherName"].ToString();
                    objR.date = reader["date"].ToString();
                    objR.firstChoiceName = reader["firstChoiceName"].ToString();
                    objR.secondChoiceName = reader["secondChoiceName"].ToString();
                    objR.thirdChoiceName = reader["thirdChoiceName"].ToString();
                    objR.forthChoiceName = reader["forthChoiceName"].ToString();
                    objR.teacherEarn = Convert.ToDouble(reader["teacherEarn"]);
                    objR.topicID = Convert.ToInt32(reader["topicID"]);
                    objR.isTextOrVideo = Convert.ToInt32(reader["isTextOrVideo"]);
                    objR.topicName = reader["topicName"].ToString();
                    objR.videoURL = reader["videoURL"].ToString();
                    objR.approval = Convert.ToInt32(reader["approval"]);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setStudentTuitionHistory(StudentTuitionHistory obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setStudentTuitionHistory", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                cmd.Parameters.AddWithValue("@tuitionID", obj.tuitionID);
                cmd.Parameters.AddWithValue("@time", obj.time);
                cmd.Parameters.AddWithValue("@teacherID", obj.teacherID);
                cmd.Parameters.AddWithValue("@cost", obj.cost);
                cmd.Parameters.AddWithValue("@ratting", obj.ratting);
                cmd.Parameters.AddWithValue("@firstChoiceID", obj.firstChoiceID);
                cmd.Parameters.AddWithValue("@secondChoiceID", obj.secondChoiceID);
                cmd.Parameters.AddWithValue("@thirdChoiceID", obj.thirdChoiceID);
                cmd.Parameters.AddWithValue("@forthChoiceID", obj.forthChoiceID);
                cmd.Parameters.AddWithValue("@date", obj.date);
                cmd.Parameters.AddWithValue("@firstChoiceName", obj.firstChoiceName);
                cmd.Parameters.AddWithValue("@secondChoiceName", obj.secondChoiceName);
                cmd.Parameters.AddWithValue("@thirdChoiceName", obj.thirdChoiceName);
                cmd.Parameters.AddWithValue("@forthChoiceName", obj.forthChoiceName);
                cmd.Parameters.AddWithValue("@teacherName", obj.teacherName);
                cmd.Parameters.AddWithValue("@studentName", obj.studentName);
                cmd.Parameters.AddWithValue("@teacherEarn", obj.teacherEarn);
                cmd.Parameters.AddWithValue("@topicID", obj.topicID);
                cmd.Parameters.AddWithValue("@topicName", obj.topicName);
                cmd.Parameters.AddWithValue("@isTextOrVideo", obj.isTextOrVideo);
                cmd.Parameters.AddWithValue("@videoURL", obj.videoURL);
                cmd.Parameters.AddWithValue("@approval", 0);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response updateStudentTuitionHistory(StudentTuitionHistory obj)
        {
            Response response = new Response();
            try
            {

                Connection();
                SqlCommand cmd = new SqlCommand("updateStudentTuitionHistory", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tuitionID", obj.tuitionID);
                cmd.Parameters.AddWithValue("@cost", obj.cost);
                cmd.Parameters.AddWithValue("@ratting", obj.ratting);
                cmd.Parameters.AddWithValue("@teacherEarn", obj.teacherEarn);
                cmd.Parameters.AddWithValue("@time", obj.time);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response FinalizeTuitionHistory(StudentTuitionHistory obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("FinalizeTuitionHistory", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ratting", obj.ratting);
                cmd.Parameters.AddWithValue("@tuitionID", obj.tuitionID);
                cmd.Parameters.AddWithValue("@teacherID", obj.teacherID);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        #endregion

        #region Subject
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<Subject> getSubject()
        {
            List<Subject> objRList = new List<Subject>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getSubject", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Subject objAdd = new Subject();
                    objAdd.classID = Convert.ToInt32(reader["classID"]);
                    objAdd.subjectID = Convert.ToInt32(reader["subjectID"]);
                    objAdd.title = reader["title"].ToString();
                    objAdd.avgRatting = Convert.ToDouble(reader["avgRatting"]);
                    objAdd.name = reader["name"].ToString();
                    objAdd.groupName = reader["groupName"].ToString();
                    objAdd.tuitionRequest = Convert.ToInt32(reader["tuitionRequest"]);
                    objAdd.indexNo = Convert.ToInt32(reader["indexNo"]);
                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Subject objAdd = new Subject();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Subject getSubjectWithID(Subject obj)
        {
            Subject objR = new Subject();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getSubjectWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@classID", obj.classID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.classID = Convert.ToInt32(reader["classID"]);
                    objR.subjectID = Convert.ToInt32(reader["subjectID"]);
                    objR.title = reader["title"].ToString();
                    objR.name = reader["name"].ToString();
                    objR.groupName = reader["groupName"].ToString();
                    objR.tuitionRequest = Convert.ToInt32(reader["tuitionRequest"]);
                    objR.indexNo = Convert.ToInt32(reader["indexNo "]);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setSubject(Subject obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setSubject", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@classID", obj.classID);
                cmd.Parameters.AddWithValue("@subjectID", obj.subjectID);
                cmd.Parameters.AddWithValue("@title", obj.title);
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@groupName", obj.groupName);
                cmd.Parameters.AddWithValue("@tuitionRequest", 0);
                cmd.Parameters.AddWithValue("@avgRatting", 0);
                cmd.Parameters.AddWithValue("@indexNo ", 0);
                cmd.Parameters.AddWithValue("@purchaseRate ", 0);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response updateSubject(Subject obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("updateSubject", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@classID", obj.classID);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        #endregion


        #region UniversityName
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<UniversityName> getUniversityName()
        {
            List<UniversityName> objRList = new List<UniversityName>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getUniversityName", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    UniversityName objAdd = new UniversityName();
                    objAdd.uniNameID = Convert.ToInt32(reader["uniNameID"]);
                    objAdd.title = reader["title"].ToString();
                    objAdd.name = reader["name"].ToString();
                    objAdd.avgRatting = Convert.ToDouble(reader["avgRatting"]);
                    objAdd.tuitionRequest = Convert.ToInt32(reader["tuitionRequest"]);
                    objAdd.indexNo = Convert.ToInt32(reader["indexNo"]);
                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                UniversityName objAdd = new UniversityName();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public UniversityName getUniversityNameWithID(UniversityName obj)
        {
            UniversityName objR = new UniversityName();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getUniversityNameWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@uniNameID", obj.uniNameID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.uniNameID = Convert.ToInt32(reader["uniNameID"]);
                    objR.title = reader["title"].ToString();
                    objR.name = reader["name"].ToString();
                    objR.tuitionRequest = Convert.ToInt32(reader["tuitionRequest"]);
                    objR.indexNo = Convert.ToInt32(reader["indexNo"]);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setUniversityName(UniversityName obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setUniversityName", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@uniNameID", obj.uniNameID);
                cmd.Parameters.AddWithValue("@title", obj.title);
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@tuitionRequest", obj.tuitionRequest);
                cmd.Parameters.AddWithValue("@avgRatting", obj.avgRatting);
                cmd.Parameters.AddWithValue("@indexNo ", obj.indexNo);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response updateUniversityName(UniversityName obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("updateUniversityName", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@uniNameID", obj.uniNameID);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        #endregion

      
        #region Voucher
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<Voucher> getVoucher()
        {
            List<Voucher> objRList = new List<Voucher>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getVoucher", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Voucher objAdd = new Voucher();
                    objAdd.voucherID = Convert.ToInt32(reader["voucherID"]);
                    objAdd.name = reader["name"].ToString();
                    objAdd.amountTaka = Convert.ToInt32(reader["amountTaka"]);
                    objAdd.getAmount = Convert.ToInt32(reader["getAmount"]);
                    objAdd.type = Convert.ToInt32(reader["type"]);
                    objAdd.validFrom = reader["validFrom"].ToString();
                    objAdd.validTo = reader["validTo"].ToString();
                    objAdd.voucherImageSrc = reader["voucherImageSrc"].ToString();
                    objAdd.canUse = Convert.ToInt32(reader["canUse"]);
                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Voucher objAdd = new Voucher();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Voucher getVoucherWithID(Voucher obj)
        {
            Voucher objR = new Voucher();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getVoucherWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@voucherID", obj.voucherID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.voucherID = Convert.ToInt32(reader["voucherID"]);
                    objR.name = reader["name"].ToString();
                    objR.amountTaka = Convert.ToInt32(reader["amountTaka"]);
                    objR.getAmount = Convert.ToInt32(reader["getAmount"]);
                    objR.type = Convert.ToInt32(reader["type"]);
                    objR.validFrom = reader["validFrom"].ToString();
                    objR.validTo = reader["validTo"].ToString();
                    objR.voucherImageSrc = reader["voucherImageSrc"].ToString();
                    objR.canUse = Convert.ToInt32(reader["canUse"]);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setVoucher(Voucher obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setVoucher", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@voucherID", obj.voucherID);
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@amountTaka", obj.amountTaka);
                cmd.Parameters.AddWithValue("@getAmount", obj.getAmount);
                cmd.Parameters.AddWithValue("@type", obj.type);
                cmd.Parameters.AddWithValue("@validFrom", obj.validFrom);
                cmd.Parameters.AddWithValue("@validTo", obj.validTo);
                cmd.Parameters.AddWithValue("@voucherImageSrc", obj.voucherImageSrc);
                cmd.Parameters.AddWithValue("@canUse", obj.canUse);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response updateVoucher(Voucher obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("updateVoucher", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@voucherID", obj.voucherID);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        #endregion


        #region VoucherHistory
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<VoucherHistory> getVoucherHistory()
        {
            List<VoucherHistory> objRList = new List<VoucherHistory>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getVoucherHistory", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    VoucherHistory objAdd = new VoucherHistory();
                    objAdd.studentID = Convert.ToInt32(reader["studentID"]);
                    objAdd.voucherHistoryID = Convert.ToInt32(reader["voucherHistoryID"]);
                    objAdd.paymentID = Convert.ToInt32(reader["paymentID"]);
                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                VoucherHistory objAdd = new VoucherHistory();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public VoucherHistory getVoucherHistoryWithID(VoucherHistory obj)
        {
            VoucherHistory objR = new VoucherHistory();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getVoucherHistoryWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.studentID = Convert.ToInt32(reader["studentID"]);
                    objR.voucherHistoryID = Convert.ToInt32(reader["voucherHistoryID"]);
                    objR.paymentID = Convert.ToInt32(reader["paymentID"]);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setVoucherHistory(VoucherHistory obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setVoucherHistory", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                cmd.Parameters.AddWithValue("@voucherHistoryID", obj.voucherHistoryID);
                cmd.Parameters.AddWithValue("@paymentID", obj.paymentID);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response updateVoucherHistory(VoucherHistory obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("updateVoucherHistory", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        #endregion

        #region Hire Teacher 
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public async Task<Teacher> HireTeacherAsync(HireTeacher obj)
        {
            Response objR = new Response();
            Teacher SelectedTuitionTeacherToCall = new Teacher();
            List<Teacher> matchedTeacherList = new List<Teacher>();
            int SelectedIndex = 0;
            List<Teacher> SortedList = new List<Teacher>();
            List<Teacher> SelectedTeacherList = new List<Teacher>();
            List<float> teacherPointList = new List<float>();
            Teacher t = new Teacher();
            int listCount = 0;
            int track = 0;
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getTeacherIDWithSubID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@subID", obj.subID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Teacher matchedTeacher = new Teacher();
                    matchedTeacher.teacherID = Convert.ToInt32(reader["teacherID"]);
                    matchedTeacher.activeTime = reader["activeTime"].ToString();
                    matchedTeacherList.Add(matchedTeacher);
                }
                conn.Close();

                int pointListCount = 0;
                SortedList = matchedTeacherList.OrderBy(x => x.activeTime).ToList();
                SortedList.Reverse();


              



                if (SortedList.Count != 0)
                {
                    #region checking pure activity
                    var rightNowActiveTeacher = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getTeacherActivityStatus".GetJsonAsync<List<TeacherActivityStatus>>();
                    await Task.Delay(1000);
                    var AfterOneSecActiveTeacher = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getTeacherActivityStatus".GetJsonAsync<List<TeacherActivityStatus>>();

                    List<TeacherActivityStatus> pureActive = new List<TeacherActivityStatus>();


                    for (int i = 0; i < AfterOneSecActiveTeacher.Count; i++)
                    {
                        for (int j = 0; j < rightNowActiveTeacher.Count; j++)
                        {
                            if (AfterOneSecActiveTeacher[i].teacherID == rightNowActiveTeacher[j].teacherID)
                            {
                                pureActive.Add(AfterOneSecActiveTeacher[i]);
                                break;
                            }
                        }
                    }
                    for (int i = 0; i < SortedList.Count; i++)
                    {
                        for (int j = 0; j < pureActive.Count; j++)
                        {
                            if (SortedList[i].teacherID == pureActive[j].teacherID)
                            {
                                Teacher teacher = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getTeacherWithID".PostUrlEncodedAsync(new { teacherID = SortedList[i].teacherID }).ReceiveJson<Teacher>();
                                if(teacher.activeStatus == 1)
                                {
                                    SelectedTeacherList.Add(SortedList[i]);
                                    break;
                                }
                               
                               
                                
                                
                            }
                        }
                        if (SelectedTeacherList.Count == 5)
                            break;
                    }
                    #endregion




                    for (int i = 0; i < SelectedTeacherList.Count; i++)
                    {
                        float thispoint = (5 - i) * 2f + CalculateRatting(SelectedTeacherList[i].fiveStar, SelectedTeacherList[i].fourStar, SelectedTeacherList[i].threeStar, SelectedTeacherList[i].twoStar, SelectedTeacherList[i].oneStar) * 4.1f;
                        teacherPointList.Add(thispoint);
                    }
         
                    if (teacherPointList.Count > 0)
                    {
                        float max = teacherPointList[0];

                        for (int i = 0; i < teacherPointList.Count; i++)
                        {
                            if (max < teacherPointList[i])
                            {
                                max = teacherPointList[i];
                                SelectedIndex = i;
                            }
                        }

                        t = SelectedTeacherList[SelectedIndex];
                    }
                    else
                    {
                        t.teacherID = 0;
                    }
                    
                }
                else
                {
                    t.teacherID = 0;
                }
                
            }
            catch (Exception ex)
            {
                t.Response = ex.Message;
            }
            t.amount = track;
            return t;
        }
        #endregion

        #region Video Call Api Creator
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public VideoApiInfo GetVideoCallInfo()
        {
            VideoApiInfo inf0 = new VideoApiInfo();
            try
            {
                VideoCallAPiMaker maker = new VideoCallAPiMaker();
                inf0.apiKey = VideoCallAPiMaker.Apikey;
                inf0.SessionID = VideoCallAPiMaker.Session.Id;
                inf0.token = VideoCallAPiMaker.Token;
            }
            catch (Exception ex)
            {
                inf0.Response = ex.Message;
            }
            return inf0;
        }
        #endregion

        #region Video CAll Per Min Api Call
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public async Task<PerMinCallResponse> PerMinPassCall(PerMinPassModel obj)
        {
            PerMinCallResponse res = new PerMinCallResponse();
            try
            {
                Student student = new Student();
                Teacher teacher = new Teacher();
                student = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/getStudentWithID".PostUrlEncodedAsync(new { studentID = obj.studentID })
      .ReceiveJson<Student>();
                teacher = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getTeacherWithID".PostUrlEncodedAsync(new { teacherID = obj.teacherID })
      .ReceiveJson<Teacher>();
                int cost = CalculateStudentCost(int.Parse(obj.firstChoiceID),student);
                double teacherEarn = CalculateTeacherEarn(teacher, int.Parse(obj.firstChoiceID));
                bool firstTime;
                if (obj.time == 1)
                {
                    await CreateNewTuitionHistory(student,teacher,obj, cost, teacherEarn);
                    firstTime = true;
                }
                else
                {
                   await UpdateTuitionHistory(obj,cost,teacherEarn);
                    firstTime = false;
                }
                await UpdateStudent(student, cost);
                await UpdateTeacher(teacher, firstTime, teacherEarn);
                res.Massage = "All Ok";
                res.cost = cost;
                res.earned = teacherEarn;
                res.Status = 0;
            }
            catch (Exception ex)
            {
                res.Massage = ex.Message;
                res.Status = 1;
            }
            return res;
        }
       
        public async Task CreateNewTuitionHistory(Student st, Teacher th, PerMinPassModel prmc,int cost,double teacherearn)
        {
            var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/setStudentTuitionHistory".PostUrlEncodedAsync(new { 
                studentID = st.studentID,
                tuitionID = prmc.sessionID,
                time = prmc.time,
                teacherID = th.teacherID,
                cost = cost,
                ratting = 0,
                firstChoiceID = prmc.firstChoiceID,
                secondChoiceID = prmc.secondChoiceID,
                thirdChoiceID = prmc.thirdChoiceID,
                forthChoiceID = prmc.firstChoiceID,
                date = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"),
                studentName = st.name,
                teacherName = th.name,
                firstChoiceName = prmc.firstChoiceName,
                secondChoiceName = prmc.secondChoiceName,
                thirdChoiceName = prmc.thirdChoiceName,
                forthChoiceName = prmc.forthChoiceName,
                teacherEarn = teacherearn
            })
      .ReceiveJson<Response>();
            
        }
        public async Task UpdateTuitionHistory(PerMinPassModel prmc, int cost, double earn)
        {
            int totalCost = cost * prmc.time;
            double totalEarn = earn * prmc.time;
            var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/updateStudentTuitionHistory".PostUrlEncodedAsync(new
            {
                tuitionID = prmc.sessionID,
                time = prmc.time,
                cost = totalCost,
                ratting = 0,
                teacherEarn = totalEarn
            }).ReceiveJson<Response>();
        }
        public async Task UpdateStudent(Student student ,int cost)
        {
            if(cost == 0)
            {
                student.freemin -= 1;
            }
            else
            {
                student.totalSpent += cost;
                student.coin -= cost;
            }
            Response regRes = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/updateStudent"
                .PostUrlEncodedAsync(new
                {
                    studentID = student.studentID,
                    phonenumber = student.phonenumber,
                    password = student.password,
                    totalSpent = student.totalSpent,
                    totalTuitionTime = (student.totalTuitionTime+1),
                    coin = student.coin,
                    freemin = student.freemin,
                    city = student.city,
                    name = student.name,
                    institutionName = "none"
                })
                .ReceiveJson<Response>();
        }
        public async Task UpdateTeacher(Teacher teacher, bool firstTime, double earn)
        {
            if(firstTime)
            {
                teacher.totalTuition += 1;
            }
            if(teacher.totalMinuite+1 >= TeacherMonetizationTime)
            {
                teacher.monetizetionStatus = 1;
            }
            Response regRes = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/updateTeacherInfo"
                .PostUrlEncodedAsync(new
                {
                    teacherID = teacher.teacherID,
                    name = teacher.name,
                    password = teacher.password,
                    phonenumber = teacher.phonenumber,
                    selectionStatus = teacher.selectionStatus,
                    monetizetionStatus = teacher.monetizetionStatus,
                    totalMinuite = teacher.totalMinuite+1,
                    favTeacherCount = teacher.favTeacherCount,
                    reportCount = teacher.reportCount,
                    totalTuition = teacher.totalTuition,
                    fiveStar = teacher.fiveStar,
                    fourStar = teacher.fourStar,
                    threeStar = teacher.threeStar,
                    twoStar = teacher.twoStar,
                    oneStar = teacher.oneStar,
                    amount = teacher.amount+ earn,
                    activeStatus = teacher.activeStatus
                })
                .ReceiveJson<Response>();
        }
        public int CalculateStudentCost(int insID, Student student)
        {
            int cost = 0;
            bool isCostAvaiable;
            if (student.freemin == 0)
            {
                isCostAvaiable = true;

            }
            else
            {
                isCostAvaiable = false;
            }
            
            if(isCostAvaiable)
            {
                if (insID == 101)
                {
                    cost = SchoolCost;
                }
                if (insID == 102)
                {
                    cost = CollegeCost;
                }
            }
            else
            {
                cost = 0;
            }
            
            return cost;
        }
        public double CalculateTeacherEarn(Teacher teacher,  int insID)
        {
            double earn = 0;
            if(teacher.monetizetionStatus == 0)
            {
                earn = 0;
            }
            else
            {
                if(insID == 101)
                {
                    earn = SchoolCost * 0.80;
                }
                if(insID == 102)
                {
                    earn = CollegeCost * 0.80;
                }
            }
            return earn;
        }
        /*
         * studentID: 10000003, 
teacherID:	23926316
time:1
sessionID:'fghfgh543dgffd45sf'
firstChoiceID:101
secondChoiceID:101
thirdChoiceID:101
forthChoiceID:101
firstChoiceName:'School'
secondChoiceName:'Class 6'
thirdChoiceName:'Physics First Paper'
forthChoiceName: 'Chapter 1'
         */
        #endregion

        #region Get Cost
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public CostClass GetCost()
        {
            CostClass allCost = new CostClass();
            allCost.SchoolCost = SchoolCost;
            allCost.CollegeCost = CollegeCost;
            allCost.ProcessignCostPercent = processignCostPercent;
            return allCost;
        }
        #endregion

        #region Dashboard
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<DashBoardUser> GetDashBoardUser()
        {
            List<DashBoardUser> objRList = new List<DashBoardUser>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("GetDashBoardUser", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DashBoardUser objAdd = new DashBoardUser();
                    objAdd.userID = Convert.ToInt32(reader["userID"]);
                    objAdd.name = reader["name"].ToString();
                    objAdd.password = reader["password"].ToString();
                    objAdd.type = reader["type"].ToString();
                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                DashBoardUser objAdd = new DashBoardUser();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }
        #endregion

        #region RequestPayemnt
        private string paymentGatewayBase = WebConfigurationManager.AppSettings["baseUrl"];
        string Baseurl = WebConfigurationManager.AppSettings["baseUrl"] + "/request.php";
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public async Task<String> RequestPayment(requestPayment obj)
        {
            string messgae = "";
            string url;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            TransactionModel rq = new TransactionModel();

            rq.tran_id = RandomString(10);
            rq.cus_name = obj.name;
            rq.amount = obj.amount +"" ;
            rq.cus_phone = obj.phonenumber;
            rq.opt_a = obj.studentID.ToString();
            rq.opt_b = obj.type.ToString();
            rq.cus_email = "mahirmuzahid@gmail.com";

            rq.success_url = "https://api.shikkhanobish.com/api/ShikkhanobishLogin/CallBackPaymentSuccessFull";
            rq.fail_url = "https://api.shikkhanobish.com/api/ShikkhanobishLogin/CallBackPaymentFailed";
            rq.cancel_url = "https://api.shikkhanobish.com/api/ShikkhanobishLogin/CallBackPaymentCancle";

            PropertyInfo[] infos = rq.GetType().GetProperties();

            foreach (PropertyInfo pair in infos)
            {
                string name = pair.Name;
                var value = pair.GetValue(rq, null);

                parameters.Add(pair.Name, value.ToString());
            }
            using (var client = new HttpClient())
            {
                HttpContent DictionaryItems = new FormUrlEncodedContent(parameters);

                using (
                    var result =
                        await client.PostAsync(Baseurl, DictionaryItems))
                {
                    var input = await result.Content.ReadAsStringAsync();
                    var trans = input.Remove(0, 2).Split('"')[0];                   
                    url = paymentGatewayBase + trans;
                }
            }
            return url;
        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public async Task CallBackPaymentSuccessFull(CallBackPayment obj)
        {
            string url = "https://shikkhanobishrealtimeapi.shikkhanobish.com/api/ShikkhanobishSignalR/StudentPaymentStatus?&studentID=" + obj.opt_a + "&successFullPayment=" + true + "&amount=" + obj.amount + "&response=ok" + "&paymentID=" + obj.mer_txnid + "&trxID=" + obj.bank_txn + "&cardID="+obj.card_number+ "&cardType="+obj.card_type;
            HttpClient client = new HttpClient();
            StringContent content = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content).ConfigureAwait(true);
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public async Task CallBackPaymentFailed(CallBackPayment obj)
        {
            string url = "https://shikkhanobishrealtimeapi.shikkhanobish.com/api/ShikkhanobishSignalR/StudentPaymentStatus?&studentID=" + obj.opt_a + "&successFullPayment=" + false + "&amount=" + obj.amount + "&response=" + obj.pg_error_code_details + "&paymentID=" + obj.mer_txnid + "&trxID=" + obj.bank_txn + "&cardID=" + obj.card_number + "&cardType=" + obj.card_type;
            HttpClient client = new HttpClient();
            StringContent content = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content).ConfigureAwait(true);
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public async Task CallBackPaymentCancle(CallBackPayment obj)
        {
            string url = "https://shikkhanobishrealtimeapi.shikkhanobish.com/api/ShikkhanobishSignalR/StudentPaymentStatus?&studentID=" + obj.opt_a + "&successFullPayment=" + false + "&amount=" + obj.amount + "&response=" + obj.pg_error_code_details + "&paymentID=" + obj.mer_txnid + "&trxID=" + obj.bank_txn + "&cardID=" + obj.card_number + "&cardType=" + obj.card_type;
            HttpClient client = new HttpClient();
            StringContent content = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content).ConfigureAwait(true);
        }
        #endregion

        #region ImageSource
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<PromotionalImage> GetPromotionalImage()
        {
            List<PromotionalImage> objRList = new List<PromotionalImage>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("GetPromotionalImage", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PromotionalImage objAdd = new PromotionalImage();
                    objAdd.imgSource = reader["imgSource"].ToString();
                    objRList.Add(objAdd); 
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                PromotionalImage objAdd = new PromotionalImage();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }
        #endregion

        #region GetImageSource
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<PromotionalMassage> GetPromotionalMassage()
        {
            List<PromotionalMassage> objRList = new List<PromotionalMassage>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("GetPromotionalMassage", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PromotionalMassage objAdd = new PromotionalMassage();
                    objAdd.imageSrc = reader["imageSrc"].ToString();
                    objAdd.msgType = Convert.ToInt32(reader["msgType"]);
                    objAdd.userType = reader["userType"].ToString();
                    objAdd.text = reader["text"].ToString();
                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                PromotionalMassage objAdd = new PromotionalMassage();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }
        #endregion

        #region Pending Ratting
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<pendingRatting> getPendingRatting()
        {
            List<pendingRatting> objRList = new List<pendingRatting>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getPendingRatting", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    pendingRatting objAdd = new pendingRatting();
                    objAdd.studentID = Convert.ToInt32(reader["studentID"]);
                    objAdd.tuitionID = reader["tuitionID"].ToString();
                    objAdd.Response = "Ok";
                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                pendingRatting objAdd = new pendingRatting();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setPendingRatting(pendingRatting obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setPendingRatting", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                cmd.Parameters.AddWithValue("@tuitionID", obj.tuitionID);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response deletePendingTuition(pendingRatting obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("deletePendingTuition", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        #endregion

        #region Active Status
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<activeStatusTable> getActiveStatus()
        {
            List<activeStatusTable> objRList = new List<activeStatusTable>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getActiveStatus", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    activeStatusTable objAdd = new activeStatusTable();
                    objAdd.userID = Convert.ToInt32(reader["userID"]);
                    objAdd.activeStatus = Convert.ToInt32(reader["activeStatus"]);
                    objAdd.Response = "ok";
                    
                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                activeStatusTable objAdd = new activeStatusTable();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setActiveStatus(activeStatusTable obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setActiveStatus", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userID", obj.userID);
                cmd.Parameters.AddWithValue("@activeStatus", obj.activeStatus);
                cmd.Parameters.AddWithValue("@type", obj.type);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response updateActiveStatus(activeStatusTable obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("updateActiveStatus", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userID", obj.userID);
                cmd.Parameters.AddWithValue("@activeStatus", obj.activeStatus);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        #endregion

        #region Referral
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<ReferralTable> getRefferalTable()
        {
            List<ReferralTable> objRList = new List<ReferralTable>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getRefferalTable", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ReferralTable objAdd = new ReferralTable();
                    objAdd.referralID = reader["referralID"].ToString();
                    objAdd.studentID = Convert.ToInt32(reader["studentID"]);
                    objAdd.referredStudentID = Convert.ToInt32(reader["referredStudentID"]);

                    objAdd.referralDate = reader["referralDate"].ToString();

                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                ReferralTable objAdd = new ReferralTable();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setReferralTable(ReferralTable obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setReferralTable", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@referralID", obj.referralID);
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }

            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response registerReferral(ReferralTable obj)
        {
            Response response = new Response();

            List<ReferralTable> list = new List<ReferralTable>();
            bool alreadyExist = list.Any(x => x.studentID == obj.studentID);
            if (!alreadyExist)
            {
                try
                {
                    Connection();
                    SqlCommand cmd = new SqlCommand("registerReferral", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@referredStudentID", obj.referredStudentID);
                    cmd.Parameters.AddWithValue("@referralDate", DateTime.Now.ToString("dd/mm/yyyy"));
                    cmd.Parameters.AddWithValue("@referralID", obj.referralID);


                    SqlCommand cmd2 = new SqlCommand("addFererralMin", conn);
                    cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@freemin", 5);
                    cmd2.Parameters.AddWithValue("@studentID", obj.studentID);
                    
                    SqlCommand cmd3 = new SqlCommand("addFererralMin", conn);
                    cmd3.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@freemin", 5);
                    cmd3.Parameters.AddWithValue("@studentID", obj.referredStudentID);
                    conn.Open();
                    int i = cmd.ExecuteNonQuery();
                    int j = cmd2.ExecuteNonQuery();
                    int k = cmd3.ExecuteNonQuery();
                    if (i != 0&& j != 0 && k != 0)
                    {
                        response.Massage = "Succesfull!";
                        response.Status = 0;
                    }
                    else
                    {
                        response.Massage = "Unsuccesfull!";
                        response.Status = 1;
                    }
                }
                catch (Exception ex)
                {
                    response.Massage = ex.Message;
                    response.Status = 0;
                }

            }
            return response;


        }
        #endregion

        #region Question
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<Question> getQuestion()
        {
            List<Question> objRList = new List<Question>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getQuestion", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Question objAdd = new Question();
                    objAdd.questionID = Convert.ToInt32(reader["questionID"]);
                    objAdd.classID = Convert.ToInt32(reader["classID"]);
                    objAdd.subjectID = Convert.ToInt32(reader["subjectID"]);
                    objAdd.chapterID = Convert.ToInt32(reader["chapterID"]);
                    objAdd.topicID = Convert.ToInt32(reader["topicID"]);
                    objAdd.mainQuestion = reader["mainQuestion"].ToString();
                    objAdd.option1 = reader["option1"].ToString();
                    objAdd.option2 = reader["option2"].ToString();
                    objAdd.option3 = reader["option3"].ToString();
                    objAdd.option4 = reader["option4"].ToString();
                    objAdd.rightAnswer = Convert.ToInt32(reader["rightAnswer"]);
                    objAdd.quesImages = reader["quesImages"].ToString();
                    objAdd.review = Convert.ToInt32(reader["review"]);
                    objAdd.Response = "ok";



                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Question objAdd = new Question();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        //......................................//

        // Test: 
        // URL:https:
        /* Parameter
        {
            
        }

        */

        //......................................//

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Question getQuestionWithID(Question obj)
        {
            Question objR = new Question();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getQuestionWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@questionID", obj.questionID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.questionID = Convert.ToInt32(reader["questionID"]);
                    objR.classID = Convert.ToInt32(reader["classID"]);
                    objR.subjectID = Convert.ToInt32(reader["subjectID"]);
                    objR.chapterID = Convert.ToInt32(reader["chapterID"]);
                    objR.topicID = Convert.ToInt32(reader["topicID"]);
                    objR.mainQuestion = reader["mainQuestion"].ToString();
                    objR.option1 = reader["option1"].ToString();
                    objR.option2 = reader["option2"].ToString();
                    objR.option3 = reader["option3"].ToString();
                    objR.option4 = reader["option4"].ToString();
                    objR.rightAnswer = Convert.ToInt32(reader["rightAnswer"]);
                    objR.quesImages = reader["quesImages"].ToString();
                    objR.review = Convert.ToInt32(reader["review"]);
                    objR.Response = "ok";

                }
                conn.Close();

            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        //......................................//

        // Test: 
        // URL:https:
        /* Parameter
        {
            
        }

        */

        //......................................//


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setQuestion(Question obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setQuestion", conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@questionID", obj.questionID);
                cmd.Parameters.AddWithValue("@classID", obj.classID);
                cmd.Parameters.AddWithValue("@subjectID", obj.subjectID);
                cmd.Parameters.AddWithValue("@chapterID", obj.chapterID);
                cmd.Parameters.AddWithValue("@topicID", obj.topicID);
                cmd.Parameters.AddWithValue("@mainQuestion", obj.mainQuestion);
                cmd.Parameters.AddWithValue("@option1", obj.option1);
                cmd.Parameters.AddWithValue("@option2", obj.option2);
                cmd.Parameters.AddWithValue("@option3", obj.option3);
                cmd.Parameters.AddWithValue("@option4", obj.option4);
                cmd.Parameters.AddWithValue("@rightAnswer", obj.rightAnswer);
                cmd.Parameters.AddWithValue("@quesImages", "n/a");
                cmd.Parameters.AddWithValue("@review", obj.review);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        //......................................//

        // Test: 
        // URL:https:
        /* Parameter
        {
            
        }

        */

        //......................................//

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setQuestionWithID(Question obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setQuestionWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@questionID", obj.questionID);
                cmd.Parameters.AddWithValue("@classID", obj.classID);
                cmd.Parameters.AddWithValue("@subjectID", obj.subjectID);
                cmd.Parameters.AddWithValue("@chapterID", obj.chapterID);
                cmd.Parameters.AddWithValue("@topicID", obj.topicID);
                cmd.Parameters.AddWithValue("@mainQuestion", obj.mainQuestion);
                cmd.Parameters.AddWithValue("@option1", obj.option1);
                cmd.Parameters.AddWithValue("@option2", obj.option2);
                cmd.Parameters.AddWithValue("@option3", obj.option3);
                cmd.Parameters.AddWithValue("@option4", obj.option4);
                cmd.Parameters.AddWithValue("@rightAnswer", obj.rightAnswer);
                cmd.Parameters.AddWithValue("@quesImages", obj.quesImages);
                cmd.Parameters.AddWithValue("@review", obj.review);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        //......................................//

        // Test: 
        // URL:https:
        /* Parameter
        {
            
        }

        */

        //......................................//


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response approveOrDisapprovedQuestino(Question obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("approveOrDisapprovedQuestino", conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@questionID", obj.questionID);
                cmd.Parameters.AddWithValue("@review", obj.review);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }

        #endregion

        #region TeacherQuestionHistory

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setTeacherQuestionHistory(TeacherQuestionHistory obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setTeacherQuestionHistory", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@tqID", obj.tqID);
                cmd.Parameters.AddWithValue("@teacherID", obj.teacherID);
                cmd.Parameters.AddWithValue("@questionID", obj.questionID);
                cmd.Parameters.AddWithValue("@review", obj.review);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setTeacherQuestionHistoryWithID(TeacherQuestionHistory obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setTeacherQuestionHistoryWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@tqID", obj.tqID);
                cmd.Parameters.AddWithValue("@teacherID", obj.teacherID);
                cmd.Parameters.AddWithValue("@questionID", obj.questionID);
                cmd.Parameters.AddWithValue("@review", obj.review);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<TeacherQuestionHistory> getTeacherQuestionHistory()
        {
            List<TeacherQuestionHistory> objRList = new List<TeacherQuestionHistory>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getTeacherQuestionHistory", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TeacherQuestionHistory objAdd = new TeacherQuestionHistory();

                    objAdd.tqID = Convert.ToInt32(reader["tqID"]);
                    objAdd.teacherID = Convert.ToInt32(reader["teacherID"]);
                    objAdd.questionID = Convert.ToInt32(reader["questionID"]);
                    objAdd.review = reader["review"].ToString();


                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                TeacherQuestionHistory objAdd = new TeacherQuestionHistory();
                var Response = ex.InnerException;
                objRList.Add(objAdd);
            }
            return objRList;

        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public TeacherQuestionHistory getTeacherQuestionHistoryWithID(TeacherQuestionHistory obj)
        {
            TeacherQuestionHistory objR = new TeacherQuestionHistory();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getTeacherQuestionHistoryWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tqID", obj.tqID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.tqID = Convert.ToInt32(reader["tqID"]);
                    objR.teacherID = Convert.ToInt32(reader["teacherID"]);
                    objR.questionID = Convert.ToInt32(reader["questionID"]);
                    objR.review = reader["review"].ToString();

                }
                conn.Close();

            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        #endregion

        #region TuitionLog
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setTuiTionLog(TuiTionLog obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setTuiTionLog", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tuitionLogID", obj.tuitionLogID);
                cmd.Parameters.AddWithValue("@studentName", obj.studentName);
                cmd.Parameters.AddWithValue("@subjectname", obj.subjectname);
                cmd.Parameters.AddWithValue("@description", obj.description);
                cmd.Parameters.AddWithValue("@subjectID", obj.subjectID);
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                cmd.Parameters.AddWithValue("@date", obj.date);
                cmd.Parameters.AddWithValue("@tuitionLogStatus", obj.tuitionLogStatus);
                cmd.Parameters.AddWithValue("@pendingTeacherID", obj.pendingTeacherID);
                cmd.Parameters.AddWithValue("@chapterID", obj.chapterID);
                cmd.Parameters.AddWithValue("@chapterName", obj.chapterName);
                cmd.Parameters.AddWithValue("@isTextOrVideo", obj.isTextOrVideo);
                cmd.Parameters.AddWithValue("@img1", obj.img1);
                cmd.Parameters.AddWithValue("@img2", obj.img2);
                cmd.Parameters.AddWithValue("@img3", obj.img3);
                cmd.Parameters.AddWithValue("@img4", obj.img4);
                cmd.Parameters.AddWithValue("@approval", 0);
                cmd.Parameters.AddWithValue("@teacherID", 0);
                cmd.Parameters.AddWithValue("@teacherName", "n/a");
                cmd.Parameters.AddWithValue("@ansText", "n/a");
                cmd.Parameters.AddWithValue("@ansImg", "n/a");
                cmd.Parameters.AddWithValue("@ansVideo", "n/a");
                cmd.Parameters.AddWithValue("@startingDate", "n/a");

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }

            //await Task.Delay(120000);
            //var res = await "https://api.shikkhanobish.com/api/ShikkhanobishLogin/deleteTuitionLog".PostUrlEncodedAsync(new { tuitionLogID = id }).ReceiveJson<Response>();

            return response;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setTuiTionLogWithID(TuiTionLog obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setTuiTionLogWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tuitionLogID", obj.tuitionLogID);
                cmd.Parameters.AddWithValue("@startingDate", obj.startingDate);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }

            return response;
        }
        
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public TuiTionLog getTuiTionLogWithID(TuiTionLog obj)
        {
            TuiTionLog objAdd = new TuiTionLog();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getTuiTionLogWithID", conn);
                cmd.Parameters.AddWithValue("@tuitionLogID", obj.tuitionLogID);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objAdd.tuitionLogID = reader["tuitionLogID"].ToString();
                    objAdd.subjectname = reader["subjectname"].ToString();
                    objAdd.studentName = reader["studentName"].ToString();
                    objAdd.date = reader["date"].ToString();
                    objAdd.subjectID = Convert.ToInt32(reader["subjectID"]);
                    objAdd.description = reader["description"].ToString();
                    objAdd.studentID = Convert.ToInt32(reader["studentID"]);
                    objAdd.tuitionLogStatus = Convert.ToInt32(reader["tuitionLogStatus"]);
                    objAdd.pendingTeacherID = Convert.ToInt32(reader["pendingTeacherID"]);
                    objAdd.chapterName = reader["chapterName"].ToString();
                    objAdd.chapterID = Convert.ToInt32(reader["chapterID"]);
                    objAdd.isTextOrVideo = Convert.ToInt32(reader["isTextOrVideo"]);
                    objAdd.img1 = reader["img1"].ToString();
                    objAdd.img2 = reader["img2"].ToString();
                    objAdd.img3 = reader["img3"].ToString();
                    objAdd.img4 = reader["img4"].ToString();
                    objAdd.approval = Convert.ToInt32(reader["approval"]);
                    objAdd.teacherID = reader["teacherID"].ToString();
                    objAdd.teacherName = reader["teacherName"].ToString();
                    objAdd.ansText = reader["ansText"].ToString();
                    objAdd.ansImg = reader["ansImg"].ToString();
                    objAdd.ansVideo = reader["ansVideo"].ToString();
                    objAdd.startingDate = reader["startingDate"].ToString();

                    objAdd.Response = "ok";
                }
                conn.Close();
            }
            catch (Exception ex)
            {

                objAdd.Response = ex.Message;

            }
            return objAdd;
        }


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<TuiTionLog> getTuitionLogWithStudentID(TuiTionLog obj)
        {
            List<TuiTionLog> objRList = new List<TuiTionLog>();

            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getTuitionLogWithStudentID", conn);
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TuiTionLog objAdd = new TuiTionLog();
                    objAdd.tuitionLogID = reader["tuitionLogID"].ToString();
                    objAdd.subjectname = reader["subjectname"].ToString();
                    objAdd.studentName = reader["studentName"].ToString();
                    objAdd.date = reader["date"].ToString();
                    objAdd.subjectID = Convert.ToInt32(reader["subjectID"]);
                    objAdd.description = reader["description"].ToString();
                    objAdd.studentID = Convert.ToInt32(reader["studentID"]);
                    objAdd.tuitionLogStatus = Convert.ToInt32(reader["tuitionLogStatus"]);
                    objAdd.pendingTeacherID = Convert.ToInt32(reader["pendingTeacherID"]);
                    objAdd.chapterName = reader["chapterName"].ToString();
                    objAdd.chapterID = Convert.ToInt32(reader["chapterID"]);
                    objAdd.isTextOrVideo = Convert.ToInt32(reader["isTextOrVideo"]);
                    objAdd.img1 = reader["img1"].ToString();
                    objAdd.img2 = reader["img2"].ToString();
                    objAdd.img3 = reader["img3"].ToString();
                    objAdd.img4 = reader["img4"].ToString();
                    objAdd.approval = Convert.ToInt32(reader["approval"]);
                    objAdd.teacherID = reader["teacherID"].ToString();
                    objAdd.teacherName = reader["teacherName"].ToString();
                    objAdd.ansText = reader["ansText"].ToString();
                    objAdd.ansImg = reader["ansImg"].ToString();
                    objAdd.ansVideo = reader["ansVideo"].ToString();
                    objAdd.startingDate = reader["startingDate"].ToString();

                    objAdd.Response = "ok";

                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                TuiTionLog objAdd = new TuiTionLog();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<TuiTionLog> getTuiTionLogNeW()
        {
            List<TuiTionLog> objRList = new List<TuiTionLog>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getTuiTionLog", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TuiTionLog objAdd = new TuiTionLog();
                    objAdd.tuitionLogID = reader["tuitionLogID"].ToString();
                    objAdd.subjectname = reader["subjectname"].ToString();
                    objAdd.studentName = reader["studentName"].ToString();
                    objAdd.date = reader["date"].ToString();
                    objAdd.subjectID = Convert.ToInt32(reader["subjectID"]);
                    objAdd.description = reader["description"].ToString();
                    objAdd.studentID = Convert.ToInt32(reader["studentID"]);
                    objAdd.tuitionLogStatus = Convert.ToInt32(reader["tuitionLogStatus"]);
                    objAdd.pendingTeacherID = Convert.ToInt32(reader["pendingTeacherID"]);
                    objAdd.chapterName = reader["chapterName"].ToString();
                    objAdd.chapterID = Convert.ToInt32(reader["chapterID"]);
                    objAdd.isTextOrVideo = Convert.ToInt32(reader["isTextOrVideo"]);
                    objAdd.img1 = reader["img1"].ToString();
                    objAdd.img2 = reader["img2"].ToString();
                    objAdd.img3 = reader["img3"].ToString();
                    objAdd.img4 = reader["img4"].ToString();
                    objAdd.approval = Convert.ToInt32(reader["approval"]);
                    objAdd.teacherID = reader["teacherID"].ToString();
                    objAdd.teacherName = reader["teacherName"].ToString();
                    objAdd.ansText = reader["ansText"].ToString();
                    objAdd.ansImg = reader["ansImg"].ToString();
                    objAdd.ansVideo = reader["ansVideo"].ToString();
                    objAdd.startingDate = reader["startingDate"].ToString();

                    objAdd.Response = "ok";

                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                TuiTionLog objAdd = new TuiTionLog();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response deleteTuitionLog(TuiTionLog obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("deleteTuitionLog", conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        #endregion

        #region Student Report
        [System.Web.Http.AcceptVerbs("GET", "POST")]

        public Response setStudentReport(StudentReport obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setStudentReport", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentReportID", obj.studentReportID);
                cmd.Parameters.AddWithValue("@reportType", obj.reportType);
                cmd.Parameters.AddWithValue("@description", obj.description);
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                cmd.Parameters.AddWithValue("@teacherID", obj.teacherID);
                cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setStudentReportWithID(StudentReport obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setStudentReportWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentReportID", obj.studentReportID);
                cmd.Parameters.AddWithValue("@reportType", obj.reportType);
                cmd.Parameters.AddWithValue("@description", obj.description);
                cmd.Parameters.AddWithValue("@description", obj.studentID);
                cmd.Parameters.AddWithValue("@teacherID", obj.teacherID);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<StudentReport> getStudentReport()
        {
            List<StudentReport> objRList = new List<StudentReport>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getStudentReport", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    StudentReport objAdd = new StudentReport();
                    objAdd.studentReportID = Convert.ToInt32(reader["studentReportID"]);
                    objAdd.reportType = Convert.ToInt32(reader["reportType"]);
                    objAdd.description = reader["description"].ToString();
                    objAdd.studentID = Convert.ToInt32(reader["studentID"]);
                    objAdd.teacherID = Convert.ToInt32(reader["teacherID"]);

                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                StudentReport objAdd = new StudentReport();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public StudentReport getStudentReportWithID(StudentReport obj)
        {
            StudentReport objR = new StudentReport();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getStudentWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentReportID", obj.studentReportID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.studentReportID = Convert.ToInt32(reader["studentReportID"]);
                    objR.reportType = Convert.ToInt32(reader["reportType"]);
                    objR.description = reader["description"].ToString();
                    objR.studentID = Convert.ToInt32(reader["studentID"]);
                    objR.teacherID = Convert.ToInt32(reader["teacherID"]);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public async Task<List<StudentReport>> getStudentReportWithStudentID(StudentReport obj)
        {
            List<StudentReport> objRList = new List<StudentReport>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getStudentReportWithStudentID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    StudentReport objAdd = new StudentReport();
                    objAdd.studentReportID = Convert.ToInt32(reader["studentReportID"]);
                    objAdd.reportType = Convert.ToInt32(reader["reportType"]);
                    objAdd.description = reader["description"].ToString();
                    objAdd.studentID = Convert.ToInt32(reader["studentID"]);
                    objAdd.teacherID = Convert.ToInt32(reader["teacherID"]);
                    objAdd.date = reader["date"].ToString();

                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                StudentReport objAdd = new StudentReport();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }

            List<Teacher> allTeacher = await "https://api.shikkhanobish.com/api/ShikkhanobishTeacher/getAllTeacher"
              .PostUrlEncodedAsync(new { })
              .ReceiveJson<List<Teacher>>();
            for (int i = 0; i < objRList.Count; i++)
            {
                if (objRList[i].reportType == 1)
                {
                    objRList[i].ReportTypeText = "Aggresive Behave";
                }
                if (objRList[i].reportType == 2)
                {
                    objRList[i].ReportTypeText = "Bad Internet Connection";
                }
                for (int j = 0; j < allTeacher.Count; j++)
                {
                    if (objRList[i].teacherID == allTeacher[j].teacherID)
                    {
                        objRList[i].teacherName = allTeacher[j].name;
                    }
                }
            }
            return objRList;
            
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public StudentReport getStudentReportWithTeacherID(StudentReport obj)
        {
            StudentReport objR = new StudentReport();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getStudentReportWithStudentID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@teacherID", obj.teacherID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.studentReportID = Convert.ToInt32(reader["studentReportID"]);
                    objR.reportType = Convert.ToInt32(reader["reportType"]);
                    objR.description = reader["description"].ToString();
                    objR.studentID = Convert.ToInt32(reader["studentID"]);
                    objR.teacherID = Convert.ToInt32(reader["teacherID"]);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }



        #endregion

        #region Tag
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setTag(Tag obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setTag", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tagID", obj.tagID);
                cmd.Parameters.AddWithValue("@tagName", obj.tagName);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setTagWithID(Tag obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setTagWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tagID", obj.tagID);
                cmd.Parameters.AddWithValue("@tagName", obj.tagName);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<Tag> getTag()
        {
            List<Tag> objRList = new List<Tag>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getTag", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Tag objAdd = new Tag();
                    objAdd.tagID = Convert.ToInt32(reader["tagID"]);
                    objAdd.tagName = reader["tagName"].ToString();

                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Tag objAdd = new Tag();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Tag getTagWithID(Tag obj)
        {
            Tag objR = new Tag();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getTagWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tagID", obj.tagID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.tagID = Convert.ToInt32(reader["tagID"]);
                    objR.tagName = reader["tagName"].ToString();

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }


        #endregion

        #region Post
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setPost(Post obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setPost", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@postID", obj.postID);
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@post", obj.post);
                cmd.Parameters.AddWithValue("@postDate", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
                cmd.Parameters.AddWithValue("@userID", obj.userID);
                cmd.Parameters.AddWithValue("@userType", obj.userType);
                cmd.Parameters.AddWithValue("@imgSrc", obj.imgSrc);
                cmd.Parameters.AddWithValue("@postTitle", obj.postTitle);
                cmd.Parameters.AddWithValue("@noOfComment", obj.noOfComment);
                cmd.Parameters.AddWithValue("@tagID", obj.tagID);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setPostWithID(Post obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setPostWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@postID", obj.postID);
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@post", obj.post);
                cmd.Parameters.AddWithValue("@postDate", obj.postDate);
                cmd.Parameters.AddWithValue("@userID", obj.userID);
                cmd.Parameters.AddWithValue("@userType", obj.userType);
                cmd.Parameters.AddWithValue("@imgSrc", obj.imgSrc);
                cmd.Parameters.AddWithValue("@postTitle", obj.postTitle);
                cmd.Parameters.AddWithValue("@noOfComment", obj.noOfComment);
                cmd.Parameters.AddWithValue("@tagID", obj.tagID);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response viewCountWithPostID(Post obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("viewCountWithPostID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@postID", obj.postID);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<Post> getPost()
        {
            List<Post> objRList = new List<Post>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getPost", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Post objAdd = new Post();
                    objAdd.postID = reader["postID"].ToString();
                    objAdd.name = reader["name"].ToString();
                    objAdd.post = reader["post"].ToString();
                    objAdd.postDate = reader["postDate"].ToString();
                    objAdd.userID = Convert.ToInt32(reader["userID"]);
                    objAdd.userType = Convert.ToInt32(reader["userType"]);
                    objAdd.imgSrc = reader["imgSrc"].ToString();
                    objAdd.postTitle = reader["postTitle"].ToString();
                    objAdd.noOfComment = Convert.ToInt32(reader["noOfComment"]);
                    objAdd.tagID = Convert.ToInt32(reader["tagID"]);

                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Post objAdd = new Post();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Post getPostWithID(Post obj)
        {
            Post objR = new Post();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getPostWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@postID", obj.postID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.postID = reader["postID"].ToString();
                    objR.name = reader["name"].ToString();
                    objR.post = reader["post"].ToString();
                    objR.postDate = reader["postDate"].ToString();
                    objR.userID = Convert.ToInt32(reader["userID"]);
                    objR.userType = Convert.ToInt32(reader["userType"]);
                    objR.imgSrc = reader["imgSrc"].ToString();
                    objR.postTitle = reader["postTitle"].ToString();
                    objR.noOfComment = Convert.ToInt32(reader["noOfComment"]);
                    objR.tagID = Convert.ToInt32(reader["tagID"]);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Post getPostWithUserID(Post obj)
        {
            Post objR = new Post();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getPostWithUserID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userID", obj.userID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.postID = reader["postID"].ToString();
                    objR.name = reader["name"].ToString();
                    objR.post = reader["post"].ToString();
                    objR.postDate = reader["postDate"].ToString();
                    objR.userID = Convert.ToInt32(reader["userID"]);
                    objR.userType = Convert.ToInt32(reader["userType"]);
                    objR.imgSrc = reader["imgSrc"].ToString();
                    objR.postTitle = reader["postTitle"].ToString();
                    objR.noOfComment = Convert.ToInt32(reader["noOfComment"]);
                    objR.tagID = Convert.ToInt32(reader["tagID"]);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Post getPostWithTagID(Post obj)
        {
            Post objR = new Post();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getPostWithTagID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tagID", obj.tagID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.postID = reader["postID"].ToString();
                    objR.name = reader["name"].ToString();
                    objR.post = reader["post"].ToString();
                    objR.postDate = reader["postDate"].ToString();
                    objR.userID = Convert.ToInt32(reader["userID"]);
                    objR.userType = Convert.ToInt32(reader["userType"]);
                    objR.imgSrc = reader["imgSrc"].ToString();
                    objR.postTitle = reader["postTitle"].ToString();
                    objR.noOfComment = Convert.ToInt32(reader["noOfComment"]);
                    objR.tagID = Convert.ToInt32(reader["tagID"]);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        #endregion

        #region TeacherReputation
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setTeacherReputation(TeacherReputation obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setTeacherReputation", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@trId", obj.trId);
                cmd.Parameters.AddWithValue("@reputationPoint", obj.reputationPoint);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setTeacherReputationWithID(TeacherReputation obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setTeacherReputationWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@trId", obj.trId);
                cmd.Parameters.AddWithValue("@reputationPoint", obj.reputationPoint);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<TeacherReputation> getTeacherReputation()
        {
            List<TeacherReputation> objRList = new List<TeacherReputation>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getTeacherReputation", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TeacherReputation objAdd = new TeacherReputation();
                    objAdd.trId = Convert.ToInt32(reader["trId"]);
                    objAdd.reputationPoint = Convert.ToInt32(reader["reputationPoint"]);

                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                TeacherReputation objAdd = new TeacherReputation();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public TeacherReputation getTeacherReputationWithID(TeacherReputation obj)
        {
            TeacherReputation objR = new TeacherReputation();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getTeacherReputationWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@trId", obj.trId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.trId = Convert.ToInt32(reader["trId"]);
                    objR.reputationPoint = Convert.ToInt32(reader["reputationPoint"]);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }


        #endregion

        #region Answer
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setAnswer(Answer obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setAnswer", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@answerID", obj.answerID);
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@answer", obj.answer);
                cmd.Parameters.AddWithValue("@answerDate", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
                cmd.Parameters.AddWithValue("@userID", obj.userID);
                cmd.Parameters.AddWithValue("@userType", obj.userType);
                cmd.Parameters.AddWithValue("@imgSrc", obj.imgSrc);
                cmd.Parameters.AddWithValue("@review", obj.review);
                cmd.Parameters.AddWithValue("@postID", obj.postID);


                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setAnswerWithID(Answer obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setAnswerWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@answerID", obj.answerID);
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@answer", obj.answer);
                cmd.Parameters.AddWithValue("@answerDate", obj.answerDate);
                cmd.Parameters.AddWithValue("@userID", obj.userID);
                cmd.Parameters.AddWithValue("@userType", obj.userType);
                cmd.Parameters.AddWithValue("@imgSrc", obj.imgSrc);
                cmd.Parameters.AddWithValue("@review", obj.review);
                cmd.Parameters.AddWithValue("@postID", obj.postID);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                    if (i != 0)
                    {
                        response.Massage = "Succesfull!";
                        response.Status = 0;
                    }
                    else
                    {
                        response.Massage = "Unsuccesfull!";
                        response.Status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<Answer> getAnswer()
        {
            List<Answer> objRList = new List<Answer>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getAnswer", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Answer objAdd = new Answer();
                    objAdd.answerID = reader["answerID"].ToString();
                    objAdd.name = reader["name"].ToString();
                    objAdd.answer = reader["answer"].ToString();
                    objAdd.answerDate = reader["answerDate"].ToString();
                    objAdd.userID = Convert.ToInt32(reader["userID"]);
                    objAdd.userType = Convert.ToInt32(reader["userType"]);
                    objAdd.imgSrc = reader["imgSrc"].ToString();
                    objAdd.review = Convert.ToInt32(reader["review"]);
                    objAdd.postID = reader["postID"].ToString();

                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Answer objAdd = new Answer();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Answer getAnswerWithID(Answer obj)
        {
            Answer objR = new Answer();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getAnswerWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@answerID", obj.answerID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.answerID = reader["answerID"].ToString();
                    objR.name = reader["name"].ToString();
                    objR.answer = reader["answer"].ToString();
                    objR.answerDate = reader["answerDate"].ToString();
                    objR.userID = Convert.ToInt32(reader["userID"]);
                    objR.userType = Convert.ToInt32(reader["userType"]);
                    objR.imgSrc = reader["imgSrc"].ToString();
                    objR.review = Convert.ToInt32(reader["review"]);
                    objR.postID = reader["postID"].ToString();

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Answer getAnswerWithPostID(Answer obj)
        {
            Answer objR = new Answer();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getAnswerWithPostID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@postID", obj.postID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.answerID = reader["answerID"].ToString();
                    objR.name = reader["name"].ToString();
                    objR.answer = reader["answer"].ToString();
                    objR.answerDate = reader["answerDate"].ToString();
                    objR.userID = Convert.ToInt32(reader["userID"]);
                    objR.userType = Convert.ToInt32(reader["userType"]);
                    objR.imgSrc = reader["imgSrc"].ToString();
                    objR.review = Convert.ToInt32(reader["review"]);
                    objR.postID = reader["postID"].ToString();

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response deleteAnswerWithID(Answer obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("deleteAnswerWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@answerID", obj.answerID);


                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }


        #endregion

        #region Class Choice
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setClassChoice(ClassChoice obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setClassChoice", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                cmd.Parameters.AddWithValue("@institutionID", obj.institutionID);
                cmd.Parameters.AddWithValue("@classID", obj.classID);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                    if (i != 0)
                    {
                        response.Massage = "Succesfull!";
                        response.Status = 0;
                    }
                    else
                    {
                        response.Massage = "Unsuccesfull!";
                        response.Status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<ClassChoice> getClassChoice()
        {
            List<ClassChoice> objRList = new List<ClassChoice>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getClassChoice", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ClassChoice objAdd = new ClassChoice();
                    objAdd.studentID = Convert.ToInt32(reader["studentID"]);
                    objAdd.institutionID = Convert.ToInt32(reader["institutionID"]);
                    objAdd.classID = Convert.ToInt32(reader["classID"]);

                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                ClassChoice objAdd = new ClassChoice();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public ClassChoice getClassChoiceWithID( ClassChoice obj)
        {
            ClassChoice objAdd = new ClassChoice();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getClassChoiceWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    
                    objAdd.studentID = Convert.ToInt32(reader["studentID"]);
                    objAdd.institutionID = Convert.ToInt32(reader["institutionID"]);
                    objAdd.classID = Convert.ToInt32(reader["classID"]);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objAdd.Response = ex.Message;
            }
            return objAdd;
        }
        #endregion

        #region TagStudentConnector
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setTagStudentConnector(TagStudentConnector obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setTagStudentConnector", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tagStudentID", obj.tagStudentID);
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                cmd.Parameters.AddWithValue("@tagID", obj.tagID);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setTagStudentConnectorWithID(TagStudentConnector obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setTagStudentConnectorWithID", conn);
                cmd.Parameters.AddWithValue("@tagStudentID", obj.tagStudentID);
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                cmd.Parameters.AddWithValue("@tagID", obj.tagID);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                    if (i != 0)
                    {
                        response.Massage = "Succesfull!";
                        response.Status = 0;
                    }
                    else
                    {
                        response.Massage = "Unsuccesfull!";
                        response.Status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setTagStudentConnectorWithStudentID(TagStudentConnector obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setTagStudentConnectorWithStudentID", conn);
                cmd.Parameters.AddWithValue("@tagStudentID", obj.tagStudentID);
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                cmd.Parameters.AddWithValue("@tagID", obj.tagID);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                    if (i != 0)
                    {
                        response.Massage = "Succesfull!";
                        response.Status = 0;
                    }
                    else
                    {
                        response.Massage = "Unsuccesfull!";
                        response.Status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<TagStudentConnector> getTagStudentConnector()
        {
            List<TagStudentConnector> objRList = new List<TagStudentConnector>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getTagStudentConnector", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TagStudentConnector objAdd = new TagStudentConnector();
                    objAdd.tagStudentID = Convert.ToInt32(reader["tagStudentID"]);
                    objAdd.studentID = Convert.ToInt32(reader["studentID"]);
                    objAdd.tagID = Convert.ToInt32(reader["tagID"]);


                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                TagStudentConnector objAdd = new TagStudentConnector();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public TagStudentConnector getTagStudentConnectorWithID(TagStudentConnector obj)
        {
            TagStudentConnector objR = new TagStudentConnector();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getTagStudentConnectorWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tagStudentID", obj.tagStudentID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.tagStudentID = Convert.ToInt32(reader["tagStudentID"]);
                    objR.studentID = Convert.ToInt32(reader["studentID"]);
                    objR.tagID = Convert.ToInt32(reader["tagID"]);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public TagStudentConnector getTagStudentConnectorTagWithID(TagStudentConnector obj)
        {
            TagStudentConnector objR = new TagStudentConnector();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getTagStudentConnectorTagWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tagID", obj.tagID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.tagStudentID = Convert.ToInt32(reader["tagStudentID"]);
                    objR.studentID = Convert.ToInt32(reader["studentID"]);
                    objR.tagID = Convert.ToInt32(reader["tagID"]);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }


        #endregion

        #region TuitionRequestCount
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setTuitionRequestCount(TuitionRequestCount obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setTuitionRequestCount", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tuitionID", obj.tuitionID);
                cmd.Parameters.AddWithValue("@teacherID", obj.teacherID);
                

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<TuitionRequestCount> getTuitionRequestCount()
        {
            List<TuitionRequestCount> objRList = new List<TuitionRequestCount>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getTuitionRequestCount", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TuitionRequestCount objAdd = new TuitionRequestCount();
                    objAdd.teacherID = Convert.ToInt32(reader["teacherID"]);
                    objAdd.tuitionID = reader["tuitionID"].ToString();

                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                TuitionRequestCount objAdd = new TuitionRequestCount();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public TuitionRequestCount getTuitionRequestWithtuitionID(TuitionRequestCount obj)
        {
            TuitionRequestCount objR = new TuitionRequestCount();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getTuitionRequestWithtuitionID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tuitionID", obj.tuitionID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.tuitionID = reader["tuitionID"].ToString();
                    objR.teacherID = Convert.ToInt32(reader["teacherID"]);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public TuitionRequestCount getTuitionRequestWithTeacherID(TuitionRequestCount obj)
        {
            TuitionRequestCount objR = new TuitionRequestCount();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getTuitionRequestWithTeacherID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@teacherID", obj.teacherID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.tuitionID = reader["tuitionID"].ToString();
                    objR.teacherID = Convert.ToInt32(reader["teacherID"]);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }
        #endregion

        #region TeacherReview
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setTeacherReview(TeacherReview obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setTeacherReview", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@reviewID", obj.reviewID);
                cmd.Parameters.AddWithValue("@teacherID", obj.teacherID);
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                cmd.Parameters.AddWithValue("@review", obj.review);
                cmd.Parameters.AddWithValue("@tuitionID", obj.tuitionID);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setTeacherReviewWithID(TeacherReview obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setTeacherReviewWithID", conn);
                cmd.Parameters.AddWithValue("@reviewID", obj.reviewID);
                cmd.Parameters.AddWithValue("@teacherID", obj.teacherID);
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                cmd.Parameters.AddWithValue("@review", obj.review);
                cmd.Parameters.AddWithValue("@tuitionID", obj.tuitionID);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                    if (i != 0)
                    {
                        response.Massage = "Succesfull!";
                        response.Status = 0;
                    }
                    else
                    {
                        response.Massage = "Unsuccesfull!";
                        response.Status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<TeacherReview> getTeacherReview()
        {
            List<TeacherReview> objRList = new List<TeacherReview>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getTeacherReview", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TeacherReview objAdd = new TeacherReview();
                    objAdd.reviewID = reader["reviewID"].ToString();
                    objAdd.teacherID = Convert.ToInt32(reader["teacherID"]);
                    objAdd.studentID = Convert.ToInt32(reader["studentID"]);
                    objAdd.review = reader["review"].ToString();
                    objAdd.tuitionID = reader["tuitionID"].ToString();

                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                TeacherReview objAdd = new TeacherReview();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public TeacherReview getTeacherReviewWithID(TeacherReview obj)
        {
            TeacherReview objR = new TeacherReview();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getTeacherReviewWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@reviewID", obj.reviewID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.reviewID = reader["reviewID"].ToString();
                    objR.teacherID = Convert.ToInt32(reader["teacherID"]);
                    objR.studentID = Convert.ToInt32(reader["studentID"]);
                    objR.review = reader["review"].ToString();
                    objR.tuitionID = reader["tuitionID"].ToString();

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<TeacherReview> getTeacherReviewWithTeacherID(TeacherReview obj)
        {
            List<TeacherReview> objRList = new List<TeacherReview>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getTeacherReviewWithTeacherID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@teacherID", obj.teacherID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TeacherReview objAdd = new TeacherReview();
                    objAdd.reviewID = reader["reviewID"].ToString();
                    objAdd.teacherID = Convert.ToInt32(reader["teacherID"]);
                    objAdd.studentID = Convert.ToInt32(reader["studentID"]);
                    objAdd.review = reader["review"].ToString();
                    objAdd.tuitionID = reader["tuitionID"].ToString();

                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                TeacherReview objAdd = new TeacherReview();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        
        }

        #endregion

        #region UserTimelineTag
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setUserTimelineTag(UserTimelineTag obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setUserTimelineTag", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userID", obj.userID);
                cmd.Parameters.AddWithValue("@tagID", obj.tagID);


                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<UserTimelineTag> getUserTimelineTag()
        {
            List<UserTimelineTag> objRList = new List<UserTimelineTag>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getUserTimelineTag", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    UserTimelineTag objAdd = new UserTimelineTag();
                    objAdd.userID = Convert.ToInt32(reader["userID"]);
                    objAdd.tagID = Convert.ToInt32(reader["tagID"]);

                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                UserTimelineTag objAdd = new UserTimelineTag();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<UserTimelineTag> getUserTimelineTagWithUserID(UserTimelineTag obj)
        {
            List<UserTimelineTag> objRList = new List<UserTimelineTag>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getUserTimelineTagWithUserID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userID", obj.userID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    UserTimelineTag objAdd = new UserTimelineTag();
         
                    objAdd.userID = Convert.ToInt32(reader["userID"]);
                    objAdd.tagID = Convert.ToInt32(reader["tagID"]);

                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                UserTimelineTag objAdd = new UserTimelineTag();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;

        }

        
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public UserTimelineTag getUserTimelineTagWithTagID(UserTimelineTag obj)
        {
            UserTimelineTag objR = new UserTimelineTag();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getUserTimelineTagWithTagID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tagID", obj.tagID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.userID = Convert.ToInt32(reader["userID"]);
                    objR.tagID = Convert.ToInt32(reader["tagID"]);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response deleteUserTimelineTagWithUserID(UserTimelineTag obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("deleteUserTimelineTagWithUserID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userID", obj.userID);
                cmd.Parameters.AddWithValue("@tagID", obj.tagID);


                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response deleteUserTimelineTagWithTagID(UserTimelineTag obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("deleteUserTimelineTagWithTagID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userID", obj.userID);
                cmd.Parameters.AddWithValue("@tagID", obj.tagID);


                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response deleteUserTimelineTagWithBothID(UserTimelineTag obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("deleteUserTimelineTagWithBothID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userID", obj.userID);
                cmd.Parameters.AddWithValue("@tagID", obj.tagID);


                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        #endregion

        #region NotificationType

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<NotificationType> getNotificationType()
        {
            List<NotificationType> objRList = new List<NotificationType>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getNotificationType", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    NotificationType objAdd = new NotificationType();
                    objAdd.ntID = Convert.ToInt32(reader["ntID"]);
                    objAdd.ntIcon = reader["ntIcon"].ToString();

                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                NotificationType objAdd = new NotificationType();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }
        #endregion

        #region Notification

        [System.Web.Http.AcceptVerbs("GET", "POST")]

        public Response setNotification(Notification obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setNotification", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@notificationID", obj.notificationID);
                cmd.Parameters.AddWithValue("@userId", obj.userId);
                cmd.Parameters.AddWithValue("@userType", obj.userType);
                cmd.Parameters.AddWithValue("@notificationType", obj.notificationType);
                cmd.Parameters.AddWithValue("@description", obj.description);
                cmd.Parameters.AddWithValue("@refIDOne", obj.refIDOne);
                cmd.Parameters.AddWithValue("@refIDTwo", obj.refIDTwo);
                cmd.Parameters.AddWithValue("@refIDThree", obj.refIDThree);
                cmd.Parameters.AddWithValue("@refIDFour", obj.refIDFour);
                cmd.Parameters.AddWithValue("@notificationDate", obj.notificationDate);


                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]

        public Response setNotificationWithID(Notification obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setNotificationWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@notificationID", obj.notificationID);
                cmd.Parameters.AddWithValue("@userId", obj.userId);
                cmd.Parameters.AddWithValue("@userType", obj.userType);
                cmd.Parameters.AddWithValue("@notificationType", obj.notificationType);
                cmd.Parameters.AddWithValue("@description", obj.description);
                cmd.Parameters.AddWithValue("@refIDOne", obj.refIDOne);
                cmd.Parameters.AddWithValue("@refIDTwo", obj.refIDTwo);
                cmd.Parameters.AddWithValue("@refIDThree", obj.refIDThree);
                cmd.Parameters.AddWithValue("@refIDFour", obj.refIDFour);
                cmd.Parameters.AddWithValue("@notificationDate", obj.notificationDate);


                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<Notification> getNotification()
        {
            List<Notification> objRList = new List<Notification>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getNotification", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Notification objAdd = new Notification();
                    objAdd.notificationID = reader["notificationID"].ToString();
                    objAdd.userId = Convert.ToInt32(reader["userId"]);
                    objAdd.userType = Convert.ToInt32(reader["userType"]);
                    objAdd.notificationType = Convert.ToInt32(reader["notificationType"]);
                    objAdd.description = reader["description"].ToString();
                    objAdd.refIDOne = reader["refIDOne"].ToString();
                    objAdd.refIDTwo = reader["refIDTwo"].ToString();
                    objAdd.refIDThree = reader["refIDThree"].ToString();
                    objAdd.refIDFour = reader["refIDFour"].ToString();
                    objAdd.notificationDate = reader["notificationDate"].ToString();

                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Notification objAdd = new Notification();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]

        public  List<Notification> getNotificationWithUserIDAndUserType(Notification obj)
        {
            List<Notification> objRList = new List<Notification>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getNotificationWithUserIDAndUserType", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userId", obj.userId);
                cmd.Parameters.AddWithValue("@userType", obj.userType);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Notification objAdd = new Notification();
                    obj.notificationID = reader["notificationID"].ToString();
                    obj.userId = Convert.ToInt32(reader["userId"]);
                    obj.userType = Convert.ToInt32(reader["userType"]);
                    obj.description = reader["description"].ToString();
                    obj.refIDOne = reader["refIDOne"].ToString();
                    obj.refIDTwo = reader["refIDTwo"].ToString();
                    obj.refIDThree = reader["refIDThree"].ToString();
                    obj.refIDFour = reader["refIDFour"].ToString();
                    obj.notificationDate = reader["notificationDate"].ToString();

                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Notification objAdd = new Notification();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }


            [System.Web.Http.AcceptVerbs("GET", "POST")]

        public Response deleteNotification(Notification obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("deleteNotification", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@notificationID", obj.notificationID);


                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        #endregion

        #region Student Evaluation
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setStudentEvaluation(StudentEvaluation obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setStudentEvaluation", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@evaluationID", obj.evaluationID);
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                cmd.Parameters.AddWithValue("@classID", obj.classID);
                cmd.Parameters.AddWithValue("@subjectID", obj.subjectID);
                cmd.Parameters.AddWithValue("@chapterID", obj.chapterID);
                cmd.Parameters.AddWithValue("@accuracy", obj.accuracy);
                cmd.Parameters.AddWithValue("@speed", obj.speed);
                cmd.Parameters.AddWithValue("@numberOFAns", obj.numberOFAns);
                cmd.Parameters.AddWithValue("@correctAns", obj.correctAns);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setStudentEvaluationWithID(StudentEvaluation obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setStudentEvaluationWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@evaluationID", obj.evaluationID);
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                cmd.Parameters.AddWithValue("@classID", obj.classID);
                cmd.Parameters.AddWithValue("@subjectID", obj.subjectID);
                cmd.Parameters.AddWithValue("@chapterID", obj.chapterID);
                cmd.Parameters.AddWithValue("@accuracy", obj.accuracy);
                cmd.Parameters.AddWithValue("@speed", obj.speed);
                cmd.Parameters.AddWithValue("@numberOFAns", obj.numberOFAns);
                cmd.Parameters.AddWithValue("@correctAns", obj.correctAns);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                    if (i != 0)
                    {
                        response.Massage = "Succesfull!";
                        response.Status = 0;
                    }
                    else
                    {
                        response.Massage = "Unsuccesfull!";
                        response.Status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<StudentEvaluation> getStudentEvaluation()
        {
            List<StudentEvaluation> objRList = new List<StudentEvaluation>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getStudentEvaluation", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    StudentEvaluation objAdd = new StudentEvaluation();
                    objAdd.evaluationID = reader["evaluationID"].ToString();
                    objAdd.studentID = Convert.ToInt32(reader["studentID"]);
                    objAdd.classID = Convert.ToInt32(reader["classID"]);
                    objAdd.subjectID = Convert.ToInt32(reader["subjectID"]);
                    objAdd.chapterID = Convert.ToInt32(reader["chapterID"]);
                    objAdd.accuracy = Convert.ToInt32(reader["accuracy"]);
                    objAdd.speed = Convert.ToDouble(reader["speed"]);
                    objAdd.numberOFAns = Convert.ToInt32(reader["numberOFAns"]);
                    objAdd.correctAns = Convert.ToInt32(reader["correctAns"]);

         
                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                StudentEvaluation objAdd = new StudentEvaluation();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public StudentEvaluation getStudentEvaluationWithID(StudentEvaluation obj)
        {
            StudentEvaluation objR = new StudentEvaluation();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getStudentEvaluationWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@evaluationID", obj.evaluationID);
                
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.evaluationID = reader["evaluationID"].ToString();
                    objR.studentID = Convert.ToInt32(reader["studentID"]);
                    objR.classID = Convert.ToInt32(reader["classID"]);
                    objR.subjectID = Convert.ToInt32(reader["subjectID"]);
                    objR.chapterID = Convert.ToInt32(reader["chapterID"]);
                    objR.accuracy = Convert.ToInt32(reader["accuracy"]);
                    objR.speed = Convert.ToDouble(reader["speed"]);
                    objR.numberOFAns = Convert.ToInt32(reader["numberOFAns"]);
                    objR.correctAns = Convert.ToInt32(reader["correctAns"]);


                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public StudentEvaluation getStudentEvaluationWithStudentID(StudentEvaluation obj)
        {
            StudentEvaluation objR = new StudentEvaluation();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getStudentEvaluationWithStudentID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.evaluationID = reader["evaluationID"].ToString();
                    objR.studentID = Convert.ToInt32(reader["studentID"]);
                    objR.classID = Convert.ToInt32(reader["classID"]);
                    objR.subjectID = Convert.ToInt32(reader["subjectID"]);
                    objR.chapterID = Convert.ToInt32(reader["chapterID"]);
                    objR.accuracy = Convert.ToInt32(reader["accuracy"]);
                    objR.speed = Convert.ToDouble(reader["speed"]);
                    objR.numberOFAns = Convert.ToInt32(reader["numberOFAns"]);
                    objR.correctAns = Convert.ToInt32(reader["correctAns"]);


                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public StudentEvaluation getStudentEvaluationWithClassID(StudentEvaluation obj)
        {
            StudentEvaluation objR = new StudentEvaluation();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getStudentEvaluationWithClassID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@classID", obj.classID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.evaluationID = reader["evaluationID"].ToString();
                    objR.studentID = Convert.ToInt32(reader["studentID"]);
                    objR.classID = Convert.ToInt32(reader["classID"]);
                    objR.subjectID = Convert.ToInt32(reader["subjectID"]);
                    objR.chapterID = Convert.ToInt32(reader["chapterID"]);
                    objR.accuracy = Convert.ToInt32(reader["accuracy"]);
                    objR.speed = Convert.ToDouble(reader["speed"]);
                    objR.numberOFAns = Convert.ToInt32(reader["numberOFAns"]);
                    objR.correctAns = Convert.ToInt32(reader["correctAns"]);


                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public StudentEvaluation getStudentEvaluationWithSubjectID(StudentEvaluation obj)
        {
            StudentEvaluation objR = new StudentEvaluation();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getStudentEvaluationWithSubjectID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@subjectID", obj.subjectID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.evaluationID = reader["evaluationID"].ToString();
                    objR.studentID = Convert.ToInt32(reader["studentID"]);
                    objR.classID = Convert.ToInt32(reader["classID"]);
                    objR.subjectID = Convert.ToInt32(reader["subjectID"]);
                    objR.chapterID = Convert.ToInt32(reader["chapterID"]);
                    objR.accuracy = Convert.ToInt32(reader["accuracy"]);
                    objR.speed = Convert.ToDouble(reader["speed"]);
                    objR.numberOFAns = Convert.ToInt32(reader["numberOFAns"]);
                    objR.correctAns = Convert.ToInt32(reader["correctAns"]);


                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public StudentEvaluation getStudentEvaluationWithChapterID(StudentEvaluation obj)
        {
            StudentEvaluation objR = new StudentEvaluation();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getStudentEvaluationWithChapterID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@chapterID", obj.chapterID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.evaluationID = reader["evaluationID"].ToString();
                    objR.studentID = Convert.ToInt32(reader["studentID"]);
                    objR.classID = Convert.ToInt32(reader["classID"]);
                    objR.subjectID = Convert.ToInt32(reader["subjectID"]);
                    objR.chapterID = Convert.ToInt32(reader["chapterID"]);
                    objR.accuracy = Convert.ToInt32(reader["accuracy"]);
                    objR.speed = Convert.ToDouble(reader["speed"]);
                    objR.numberOFAns = Convert.ToInt32(reader["numberOFAns"]);
                    objR.correctAns = Convert.ToInt32(reader["correctAns"]);


                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }
        #endregion

        #region StudentMCQAns


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setStudentMCQAns(StudentMCQAns obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setStudentMCQAns", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@sMCQAnsID", obj.sMCQAnsID);
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                cmd.Parameters.AddWithValue("@questionID", obj.questionID);
                cmd.Parameters.AddWithValue("@isRight", obj.isRight);
                cmd.Parameters.AddWithValue("@ansDate", obj.ansDate);
        

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setStudentMCQAnsWithID(StudentMCQAns obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setStudentMCQAnsWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@sMCQAnsID", obj.sMCQAnsID);
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);
                cmd.Parameters.AddWithValue("@questionID", obj.questionID);
                cmd.Parameters.AddWithValue("@isRight", obj.isRight);
                cmd.Parameters.AddWithValue("@ansDate", obj.ansDate);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                    if (i != 0)
                    {
                        response.Massage = "Succesfull!";
                        response.Status = 0;
                    }
                    else
                    {
                        response.Massage = "Unsuccesfull!";
                        response.Status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<StudentMCQAns> getStudentMCQAns()
        {
            List<StudentMCQAns> objRList = new List<StudentMCQAns>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getStudentMCQAns", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    StudentMCQAns objAdd = new StudentMCQAns();
                    objAdd.sMCQAnsID = reader["sMCQAnsID"].ToString();
                    objAdd.studentID = Convert.ToInt32(reader["studentID"]);
                    objAdd.questionID = reader["questionID"].ToString();
                    objAdd.isRight = Convert.ToInt32(reader["isRight"]);
                    objAdd.ansDate = reader["ansDate"].ToString();

                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                StudentMCQAns objAdd = new StudentMCQAns();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public StudentMCQAns getStudentMCQAnsWithID(StudentMCQAns obj)
        {
            StudentMCQAns objR = new StudentMCQAns();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getStudentMCQAnsWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@sMCQAnsID", obj.sMCQAnsID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.sMCQAnsID = reader["sMCQAnsID"].ToString();
                    objR.studentID = Convert.ToInt32(reader["studentID"]);
                    objR.questionID = reader["questionID"].ToString();
                    objR.isRight = Convert.ToInt32(reader["isRight"]);
                    objR.ansDate = reader["ansDate"].ToString();


                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public StudentMCQAns getStudentMCQAnsWithStudentID(StudentMCQAns obj)
        {
            StudentMCQAns objR = new StudentMCQAns();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getStudentMCQAnsWithStudentID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentID", obj.studentID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.sMCQAnsID = reader["sMCQAnsID"].ToString();
                    objR.studentID = Convert.ToInt32(reader["studentID"]);
                    objR.questionID = reader["questionID"].ToString();
                    objR.isRight = Convert.ToInt32(reader["isRight"]);
                    objR.ansDate = reader["ansDate"].ToString();


                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public StudentMCQAns getStudentMCQAnsWithQuestionID(StudentMCQAns obj)
        {
            StudentMCQAns objR = new StudentMCQAns();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getStudentMCQAnsWithQuestionID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@questionID", obj.questionID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.sMCQAnsID = reader["sMCQAnsID"].ToString();
                    objR.studentID = Convert.ToInt32(reader["studentID"]);
                    objR.questionID = reader["questionID"].ToString();
                    objR.isRight = Convert.ToInt32(reader["isRight"]);
                    objR.ansDate = reader["ansDate"].ToString();


                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        #endregion

        #region PushNotificationToken
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setPushNotificationToken(PushNotificationToken obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setPushNotificationToken", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pushNtID", obj.pushNtID);
                cmd.Parameters.AddWithValue("@userID", obj.userID);
                cmd.Parameters.AddWithValue("@userType", obj.userType);
                cmd.Parameters.AddWithValue("@token", obj.token);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response setPushNotificationTokenWithID(PushNotificationToken obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setPushNotificationTokenWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pushNtID", obj.pushNtID);
                cmd.Parameters.AddWithValue("@userID", obj.userID);
                cmd.Parameters.AddWithValue("@userType", obj.userType);
                cmd.Parameters.AddWithValue("@token", obj.token);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                    if (i != 0)
                    {
                        response.Massage = "Succesfull!";
                        response.Status = 0;
                    }
                    else
                    {
                        response.Massage = "Unsuccesfull!";
                        response.Status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<PushNotificationToken> getPushNotificationToken()
        {
            List<PushNotificationToken> objRList = new List<PushNotificationToken>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getPushNotificationToken", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PushNotificationToken objAdd = new PushNotificationToken();
                    objAdd.pushNtID = Convert.ToInt32(reader["pushNtID"]); 
                    objAdd.userID = Convert.ToInt32(reader["userID"]);
                    objAdd.userType = Convert.ToInt32(reader["userType"]);
                    objAdd.token = reader["token"].ToString();

                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                PushNotificationToken objAdd = new PushNotificationToken();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public PushNotificationToken getPushNotificationTokenWithID(PushNotificationToken obj)
        {
            PushNotificationToken objR = new PushNotificationToken();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getPushNotificationTokenWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pushNtID", obj.pushNtID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.pushNtID = Convert.ToInt32(reader["pushNtID"]); 
                    objR.userID = Convert.ToInt32(reader["userID"]);
                    objR.userType = Convert.ToInt32(reader["userType"]);
                    objR.token = reader["token"].ToString();
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        #endregion

        #region AnswerVote

        

        [System.Web.Http.AcceptVerbs("GET", "POST")]

        public Response setAnswerVote(AnswerVote obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setAnswerVote", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ansvoteID", obj.ansvoteID);
                cmd.Parameters.AddWithValue("@answerID", obj.answerID);
                cmd.Parameters.AddWithValue("@userID", obj.userID);
                cmd.Parameters.AddWithValue("@upOrdownVote", obj.upOrdownVote);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]

        public Response setAnswerVoteWithID(AnswerVote obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setAnswerVoteWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ansvoteID", obj.ansvoteID);
                cmd.Parameters.AddWithValue("@answerID", obj.answerID);
                cmd.Parameters.AddWithValue("@userID", obj.userID);
                cmd.Parameters.AddWithValue("@upOrdownVote", obj.upOrdownVote);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<AnswerVote> getAnswerVote()
        {
            List<AnswerVote> objRList = new List<AnswerVote>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getAnswerVote", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    AnswerVote objAdd = new AnswerVote();
                    objAdd.ansvoteID = Convert.ToInt32(reader["ansvoteID"]);
                    objAdd.answerID = reader["answerID"].ToString();
                    objAdd.userID = Convert.ToInt32(reader["userID"]);
                    objAdd.upOrdownVote = Convert.ToInt32(reader["upOrdownVote"]);

                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                AnswerVote objAdd = new AnswerVote();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public AnswerVote getAnswerVoteWithID(AnswerVote obj)
        {
            AnswerVote objR = new AnswerVote();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getAnswerVoteWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@answerID", obj.answerID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.ansvoteID = Convert.ToInt32(reader["ansvoteID"]);
                    objR.answerID = reader["answerID"].ToString();
                    objR.userID = Convert.ToInt32(reader["userID"]);
                    objR.upOrdownVote = Convert.ToInt32(reader["upOrdownVote"]);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }



        [System.Web.Http.AcceptVerbs("GET", "POST")]

        public Response deleteAnswerVote(AnswerVote obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("deleteAnswerVote", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@answerID", obj.answerID);
                cmd.Parameters.AddWithValue("@userID", obj.userID);
              

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }

        #endregion

        #region ShiEmployee
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public async Task<Response> setshiEmployee(shiEmployee obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setshiEmployee", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employeeID", obj.employeeID);
                cmd.Parameters.AddWithValue("@employeeType", obj.employeeType);
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@phonenumber", obj.phonenumber);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }

            
            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public Response deleteShiEmployee(shiEmployee obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("deleteShiEmployee", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employeeID", obj.employeeID);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }


            return response;
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<shiEmployee> getshiEmployee()
        {
            List<shiEmployee> objRList = new List<shiEmployee>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getshiEmployee", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    shiEmployee objAdd = new shiEmployee();
                    objAdd.employeeID = Convert.ToInt32(reader["employeeID"]);
                    objAdd.employeeType = Convert.ToInt32(reader["employeeType"]);
                    objAdd.name = reader["name"].ToString();
                    objAdd.phonenumber = reader["phonenumber"].ToString();
                    objAdd.Response = "ok";

                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                shiEmployee objAdd = new shiEmployee();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }
        #endregion

        #region ChapterBasedTopic

        [System.Web.Http.AcceptVerbs("GET", "POST")]

        public Response setChapterBasedTopic(ChapterBasedTopic obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setChapterBasedTopic", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@chapterID", obj.chapterID);
                cmd.Parameters.AddWithValue("@topicID", obj.topicID);
     
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }


       
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<ChapterBasedTopic> getChapterBasedTopic()
        {
            List<ChapterBasedTopic> objRList = new List<ChapterBasedTopic>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getChapterBasedTopic", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ChapterBasedTopic objAdd = new ChapterBasedTopic();
                    objAdd.chapterID = Convert.ToInt32(reader["chapterID"]);
                    objAdd.topicID = Convert.ToInt32(reader["topicID"]);

                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                ChapterBasedTopic objAdd = new ChapterBasedTopic();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public ChapterBasedTopic getChapterBasedTopicWithChapterID(ChapterBasedTopic obj)
        {
            ChapterBasedTopic objR = new ChapterBasedTopic();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getAnswerVoteWithID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@chapterID", obj.chapterID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    objR.chapterID = Convert.ToInt32(reader["chapterID"]);
                    objR.topicID = Convert.ToInt32(reader["topicID"]);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                objR.Response = ex.Message;
            }
            return objR;
        }

        #endregion


        #region QuestionPDFLink

        [System.Web.Http.AcceptVerbs("GET", "POST")]

        public Response setQuestionPdfLink(QuestionPdfLink obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("setQuestionPdfLink", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@chapterID", obj.chapterID);
                cmd.Parameters.AddWithValue("@link", obj.link);
                cmd.Parameters.AddWithValue("@noOfQues", obj.noOfQues);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]

        public Response deleteQuestionPdfLink(QuestionPdfLink obj)
        {
            Response response = new Response();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("deleteQuestionPdfLink", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@chapterID", obj.chapterID);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    response.Massage = "Succesfull!";
                    response.Status = 0;
                }
                else
                {
                    response.Massage = "Unsuccesfull!";
                    response.Status = 1;
                }
            }
            catch (Exception ex)
            {
                response.Massage = ex.Message;
                response.Status = 0;
            }
            return response;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<QuestionPdfLink> getQuestionPdfLink()
        {
            List<QuestionPdfLink> objRList = new List<QuestionPdfLink>();
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("getQuestionPdfLink", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    QuestionPdfLink objAdd = new QuestionPdfLink();
                    objAdd.chapterID = Convert.ToInt32(reader["chapterID"]);
                    objAdd.link = reader["link"].ToString();
                    objAdd.noOfQues = Convert.ToInt32(reader["noOfQues"]);

                    objRList.Add(objAdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                QuestionPdfLink objAdd = new QuestionPdfLink();
                objAdd.Response = ex.Message;
                objRList.Add(objAdd);
            }
            return objRList;
        }


        #endregion


    }
}