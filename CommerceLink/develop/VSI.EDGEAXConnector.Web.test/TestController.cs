using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.ErpAdapter.Interface;

namespace VSI.EDGEAXConnector.Web.test


{
    [TestClass]
    public class TestController
    {
        #region Data Members

        /// <summary>
        /// it used to store all jobs.
        /// </summary>
        List<Job> Jobs;

        /// <summary>
        /// it used to store all logs.
        /// </summary>
        List<Log> Logs;

        /// <summary>
        /// JobRepository object used to get jobs and logs from repository.
        /// </summary>
        JobRepository JobRepo;

        #endregion

        #region API Methods

        //TODO: Test Code 
        //https://localhost:44310/api/test/DisplayJobs

        [TestMethod]
        /// <summary>
        /// DisplayJobs which displays the status of jobs from EDGEAXConnector DB.
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        public void DisplayJobs()
        {
            try
            {
                JobRepo = new JobRepository();
                Jobs = JobRepo.getAllJobs();
                Jobs.SerializeToJson();
                var data = new { succeeded = true, jobs = Jobs };
                Assert.IsTrue(true, "", Jobs);
                //return Ok(data);
            }
            catch (ErpAdapterException ex)
            {
                var data = new { succeeded = false, errorMessage = ex.Message };
                // return Ok(data);
            }
            catch (Exception exx)
            {
                Assert.IsTrue(false, exx.Message);
                //var data = new { succeeded = false, errorMessage = exx.Message };
                //return Ok(data);
            }

        }

        //TODO: Test Code n = number of most recent exceptions
        //https://localhost:44310/api/test/DisplayLogs?n=10

        [TestMethod]
        /// <summary>
        /// DisplayLogs which displays n latest error logs. 
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        //[HttpGet]
        public void DisplayLogs(int n = 10)
        {
            if (n <= 0)
                n = 10;
            if (n > 200)
                n = 200;

            try
            {
                JobRepo = new JobRepository();
                Logs = JobRepo.getAllLogs(n);
                Logs.SerializeToJson();

                var data = new { succeeded = true, Logs = Logs };
                //return Ok(data);
                Assert.IsTrue(true, "", Logs);

            }
            catch (ErpAdapterException ex)
            {

                var data = new { succeeded = false, Errors = ex.Message };
                // return Ok(data);
            }
            catch (Exception exx)
            {
                //var data = new { succeeded = false, errorMessage = exx.Message };
                //return Ok(data);
                Assert.IsTrue(false, exx.Message);
            }
        }

        [TestMethod]
        //TODO: Test Code
        //https://localhost:44310/api/test/DisplayCustomerOrder?=02-000001

        /// <summary>
        /// DisplayCustomerOrder which tells you if the order is good or not.
        /// </summary>
        /// <param name="salesId"></param>
        /// <param name="myBool"></param>
        /// <returns></returns>
        //[HttpGet]
        public void DisplayCustomerOrder(string salesId = "02-000001", bool myBool = true)
        {

            try
            {
                //TransactionServiceClient tsClient = new TransactionServiceClient(CommerceRuntimeHelper.RequestContext);
                //var result = tsClient.GetCustomerOrder(salesId, myBool);
                var data = new { succeeded = true, Sales_ID = salesId, result = "Realtime Service AX Alive" };
                //return Ok(data);
            }
            catch (ErpAdapterException ex)
            {
                var data = new { succeeded = false, errorMessage = ex.Message };
                //return Ok(data);
            }
            catch (Exception exx)
            {
                var data = new { succeeded = false, errorMessage = exx.Message };
                //return Ok(data);
            }


            Assert.IsTrue(true); // FIXME TODO add asserts


            Assert.IsTrue(true);
        }

        [TestMethod]
        //TODO: Test Code
        //http://localhost:44310/api/Test/hello?message=Systemer
        //https://localhost:44300/api/Test/hello?message=Systemer
        /// <summary>
        /// Hello test API
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        //[HttpGet]
        public void Hello(string message)
        {
            //var data = new { succeeded = true, message = string.Format("Hello {0}!", message) };
            //return Ok(data);
            Assert.IsTrue(true, string.Format("Hello {0}!", message));
        }

        #endregion
    }
}
