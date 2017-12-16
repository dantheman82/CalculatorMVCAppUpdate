using CalculatorAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CalculatorAPI.Controllers
{
    public class CalculatorController : ApiController
    {
        #region Constants
        public const string ERROR_STRING = "Error - bad expression";
        #endregion 

            private EFContext db = new EFContext();

            // GET api/<controller>
            [HttpGet]
            public IEnumerable<Calculation> Get()
            {
                return db.Calculations.AsEnumerable();
            }

            // GET api/<controller>
            [HttpGet]
            public string Calculate(string fullExpression)
            {
                if (string.IsNullOrWhiteSpace(fullExpression))
                {
                    return ERROR_STRING;
                }

                double output = double.NaN;
                if (double.TryParse(CalculateDynamicExpression(fullExpression), out output))
                {
                   return output.ToString();                   
                }
                else
                {
                    return ERROR_STRING;
                }
            }

            // GET api/<controller>
            [HttpGet]
            public string Calculate(string operand, string operand1, string operand2)
            {
                Calculation c = new Calculation() { Id = CalculatorController.ID, Operand1 = operand1, Operand2 = operand2 };
                double output = double.NaN;
                if (double.TryParse(CalculateDynamicExpression(c.ToString()), out output))
                {
                    if (double.IsNaN(output))
                    {
                        return ERROR_STRING;
                    }
                    else
                    {
                        return output.ToString();
                    }
                }
                else
                {
                    return ERROR_STRING;
                }
            }

            // GET api/<controller>/5
            public Calculation Get(int id)
            {
                Calculation calculation = db.Calculations.Find(id);
                if (calculation == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
                }

                return calculation;
            }

            // POST api/<controller>
            public HttpResponseMessage Post(Calculation calculation)
            {
                if (ModelState.IsValid)
                {
                    db.Calculations.Add(calculation);
                    db.SaveChanges();

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, calculation);
                    response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = calculation.Id }));
                    return response;
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
            }

            // PUT api/<controller>/5
            public HttpResponseMessage Put(int id, Calculation calculation)
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }

                if (id != calculation.Id)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                db.Entry(calculation).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }

            // DELETE api/<controller>/5
            public HttpResponseMessage Delete(int id)
            {
                Calculation calculation = db.Calculations.Find(id);
                if (calculation == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                db.Calculations.Remove(calculation);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
                }

                return Request.CreateResponse(HttpStatusCode.OK, calculation);
            }

            protected override void Dispose(bool disposing)
            {
                db.Dispose();
                base.Dispose(disposing);
            }

            #region Calculation of Dynamic Expression - C#
            static private DataTable _dtInstance = new DataTable();
            static private double _currentTotal = 0.0;
            static private int _currentID = 0;
            static public DataTable DTInstance
            {
                get
                {
                    return _dtInstance;
                }
            }
            static public double Total
            {
                get
                {
                    return _currentTotal;
                }
            }
            static private int ID
            {
                get
                {
                    return _currentID++;
                }
            }
            
            private bool SetTotal(double val)
            {
                if(Double.IsNaN(val) || Double.IsInfinity(val)){
                    return false;
                }
                else{
                    _currentTotal = val;
                    return true;
                }
            }

            public string CalculateDynamicExpression(string mathExpression)
            {

                try
                {
                    var result = CalculatorController.DTInstance.Compute(mathExpression, "");
                    return result.ToString();
                }
                catch (Exception)
                {
                    return ERROR_STRING;
                }
            }
            #endregion 
    }
}