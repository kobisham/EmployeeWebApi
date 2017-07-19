using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDataAccess;

namespace EmployeeWebApi.Controllers
{
    public class EmployeesController : ApiController
    {
        // GET /api/Employees
        public HttpResponseMessage Get(string gender="All")
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    switch (gender.ToLower())
                    {
                        case "all": return Request.CreateResponse(HttpStatusCode.OK, 
                            entities.Employees.ToList());
                        case "male": return Request.CreateResponse(HttpStatusCode.OK, 
                            entities.Employees.Where(e => e.Gender == "male").ToList());
                        case "female": return Request.CreateResponse(HttpStatusCode.OK, 
                            entities.Employees.Where(e => e.Gender == "female").ToList());
                        default:
                            return Request.CreateResponse(HttpStatusCode.BadRequest,
                       "Value for gender must be all,male or female. " + gender + " is invalid.");
                    }


                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        // GET /api/Employees/1
        public HttpResponseMessage Get(int id)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                var entity= entities.Employees.FirstOrDefault(e => e.ID == id);
                if (entity != null)
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, 
                        "The employee with id " + id.ToString() + " not found.");
             
            }
        }

        // Post /api/Employee
        public HttpResponseMessage Post([FromBody]Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + "/" + employee.ID.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
           
        }

        public HttpResponseMessage Delete(int id)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                try
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if (entity != null)
                    {
                        entities.Employees.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The employee with id " + id.ToString() + " not found.");
                    }
                }
                catch (Exception ex)
                {

                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }

            }
        }

        [HttpPut]
        public HttpResponseMessage Put(int id,[FromBody]Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);

                    if (entity != null)
                    {
                        entity.FirstName = employee.FirstName;
                        entity.Gender = employee.Gender;
                        entity.LastName = employee.LastName;
                        entity.Salary = employee.Salary;
                        entity.DateOfBirth = employee.DateOfBirth;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The employee with id " + id.ToString() + " not found.");
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


    }
}
