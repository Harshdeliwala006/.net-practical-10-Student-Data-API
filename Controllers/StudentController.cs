using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Practical10.Models;
using System.Data;

namespace Practical10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StudentsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/Students
        [HttpGet]
        public IActionResult GetStudents()
        {
            List<Student> students = new List<Student>();

            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Students";

                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    students.Add(new Student
                    {
                        StudentId = Convert.ToInt32(dr["StudentId"]),
                        EnrollmentNo = dr["EnrollmentNo"].ToString(),
                        StudentName = dr["StudentName"].ToString(),
                        Department = dr["Department"].ToString(),
                        PassingYear = Convert.ToInt32(dr["PassingYear"]),
                        IsPlaced = Convert.ToBoolean(dr["IsPlaced"])
                    });
                }

                dr.Close();
            }

            return Ok(students);
        }


        // GET: api/Students/1
        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            Student student = null;

            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Students WHERE StudentId = @StudentId";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@StudentId", id);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    student = new Student
                    {
                        StudentId = Convert.ToInt32(dr["StudentId"]),
                        EnrollmentNo = dr["EnrollmentNo"].ToString(),
                        StudentName = dr["StudentName"].ToString(),
                        Department = dr["Department"].ToString(),
                        PassingYear = Convert.ToInt32(dr["PassingYear"]),
                        IsPlaced = Convert.ToBoolean(dr["IsPlaced"])
                    };
                }

                dr.Close();
            }

            if (student == null)
            {
                return NotFound("Student not found");
            }

            return Ok(student);
        }


        // POST: api/Students
        [HttpPost]
        public IActionResult AddStudent(Student student)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Students
                (StudentId, EnrollmentNo, StudentName, Department, PassingYear, IsPlaced)
                VALUES
                (@StudentId, @EnrollmentNo, @StudentName, @Department, @PassingYear, @IsPlaced)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@StudentId", student.StudentId);
                cmd.Parameters.AddWithValue("@EnrollmentNo", student.EnrollmentNo);
                cmd.Parameters.AddWithValue("@StudentName", student.StudentName);
                cmd.Parameters.AddWithValue("@Department", student.Department);
                cmd.Parameters.AddWithValue("@PassingYear", student.PassingYear);
                cmd.Parameters.AddWithValue("@IsPlaced", student.IsPlaced);

                con.Open();

                cmd.ExecuteNonQuery();
            }

            return Ok("Student added successfully");
        }


        // PUT: api/Students/1
        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, Student student)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Students SET
                    EnrollmentNo = @EnrollmentNo,
                    StudentName = @StudentName,
                    Department = @Department,
                    PassingYear = @PassingYear,
                    IsPlaced = @IsPlaced
                    WHERE StudentId = @StudentId";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@StudentId", id);
                cmd.Parameters.AddWithValue("@EnrollmentNo", student.EnrollmentNo);
                cmd.Parameters.AddWithValue("@StudentName", student.StudentName);
                cmd.Parameters.AddWithValue("@Department", student.Department);
                cmd.Parameters.AddWithValue("@PassingYear", student.PassingYear);
                cmd.Parameters.AddWithValue("@IsPlaced", student.IsPlaced);

                con.Open();

                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    return NotFound("Student not found");
                }
            }

            return Ok("Student updated successfully");
        }


        // DELETE: api/Students/1
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Students WHERE StudentId = @StudentId";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@StudentId", id);

                con.Open();

                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    return NotFound("Student not found");
                }
            }

            return Ok("Student deleted successfully");
        }
    }
}