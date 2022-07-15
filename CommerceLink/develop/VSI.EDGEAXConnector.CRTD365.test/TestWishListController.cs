using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VSI.EDGEAXConnector.CRTD365.Controllers;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using EdgeAXCommerceLink.RetailProxy.Extensions;
using System.Web.Script.Serialization;
using AutoMapper;
using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.CRTD365.test
{
    [TestClass]
    public class TestWishListController
    {
        string storeKey = "";

        [TestMethod]
        public void testCreateWishList()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            List<ErpInventoryInfo> lstInventoryInfo = new List<ErpInventoryInfo>();
            ErpCommerceList commerceList = new ErpCommerceList();
            //commerceList.Id = 68719478988;
            commerceList.CustomerId = "004043";
            commerceList.IsFavorite = false;
            commerceList.IsRecurring = false;
            commerceList.IsPrivate = false;
            commerceList.IsFavorite = false;
            //commerceList.DueDateTime = DateTime.Parse("1900-01-01 00:00:00");
            commerceList.Name = "List " + DateTime.UtcNow.Month.ToString() + "-" + DateTime.UtcNow.Day.ToString() + "-" + DateTime.UtcNow.Hour.ToString() + "-" + DateTime.UtcNow.Minute.ToString();

            ErpCommerceListLine commerceListLine1 = new ErpCommerceListLine();
            commerceListLine1.CustomerId = commerceList.CustomerId;
            commerceListLine1.ProductId = 22565430265;
            commerceListLine1.Quantity = 1;
            commerceListLine1.IsFavorite = false;
            commerceListLine1.IsRecurring = false;
            commerceListLine1.IsPrivate = false;
            commerceListLine1.UnitOfMeasure = "ea";



            commerceList.CommerceListLines.Add(commerceListLine1);

            ErpCommerceList erpCommerceList = new ErpCommerceList();

            try
            {
                WishListController wishListController = new WishListController(storeKey);
                erpCommerceList = wishListController.CreateWishList(commerceList);
            }
            catch (System.Exception e)
            {
                Assert.Fail("Exception : " + e.StackTrace);
            }

            Assert.IsNotNull(erpCommerceList);
            System.Console.WriteLine("Create Wish List Info = " + JsonConvert.SerializeObject(erpCommerceList));
        }

        [TestMethod]
        public void testGetWishList()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            List<ErpCommerceList> erpCommerceList = null; // new List<ErpCommerceList>();
            long wishListId = 0; // 68719478987; // 5637144578; //  68719478988;
            string customerId = "004043"; // "Maggen Tribbiani";
            bool favoriteFilter = false;
            bool publicFilter = false;

            WishListController wishListController = new WishListController(storeKey);

            try
            {
                erpCommerceList = wishListController.GetWishList(wishListId, customerId, favoriteFilter, publicFilter);
            }
            catch (System.Exception e)
            {
                Assert.Fail("Exception : " + e.StackTrace);
            }

            Assert.IsNotNull(erpCommerceList);
            System.Console.WriteLine("Get Wish List Info = " + JsonConvert.SerializeObject(erpCommerceList));
        }

        [TestMethod]
        public void testDeleteWishList()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            long wishListId = 68719479003;
            string customerId = "004043";
            object commerceListDelete = new object();

            try
            {
                WishListController wishListController = new WishListController(storeKey);
                commerceListDelete = wishListController.DeleteWishList(wishListId, customerId);
            }
            catch (System.Exception e)
            {
                Assert.Fail("Exception : " + e.StackTrace);
            }

            Assert.IsNotNull(commerceListDelete);
            System.Console.WriteLine("Delete Wish List Info = " + JsonConvert.SerializeObject(commerceListDelete));
        }

        [TestMethod]
        public void testCreateWishListLine()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            string filterAccountNumber = "004043";

            ErpCommerceListLine commerceListLine = new ErpCommerceListLine();
            commerceListLine.CommerceListId = "68719479003";
            commerceListLine.CustomerId = "004043"; // 68719478988;
            commerceListLine.ProductId = 22565430313; // 81327
            commerceListLine.Quantity = 1;
            commerceListLine.IsFavorite = false;
            commerceListLine.IsRecurring = false;
            commerceListLine.IsPrivate = false;
            commerceListLine.UnitOfMeasure = "ea";

            ErpCommerceList erpCommerceList = new ErpCommerceList();

            try
            {
                WishListController wishListController = new WishListController(storeKey);
                
                erpCommerceList = wishListController.CreateWishListLine(commerceListLine, filterAccountNumber);
            }
            catch (System.Exception e)
            {
                Assert.Fail("Exception : " + e.StackTrace);
            }

            Assert.IsNotNull(erpCommerceList);
            System.Console.WriteLine("Create Wish List Line Info = " + JsonConvert.SerializeObject(erpCommerceList));
        }

        [TestMethod]
        public void testDeleteWishListLine()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            long wishListLineId = 68719476739;
            long wishListId = 68719479003;
            string customerId = "004043";
            object commerceListDelete = new object();

            try
            {
                WishListController wishListController = new WishListController(storeKey);
                commerceListDelete = wishListController.DeleteWishListLine(wishListLineId, wishListId, customerId);
            }
            catch (System.Exception e)
            {
                Assert.Fail("Exception : " + e.StackTrace);
            }

            Assert.IsNotNull(commerceListDelete);
            System.Console.WriteLine("Delete Wish List Line Info = " + JsonConvert.SerializeObject(commerceListDelete));
        }

        [TestMethod]
        public void testUpdateWishListLine()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            string filterAccountNumber = "004043";

            ErpCommerceListLine commerceListLine = new ErpCommerceListLine();
            commerceListLine.LineId = "68719478990";
            commerceListLine.CommerceListId = "68719479003";
            commerceListLine.CustomerId = "004043"; // 68719478988;
            commerceListLine.ProductId = 22565430265; // 81327
            commerceListLine.Quantity = 10;
            commerceListLine.IsFavorite = false;
            commerceListLine.IsRecurring = false;
            commerceListLine.IsPrivate = false;
            commerceListLine.UnitOfMeasure = "ea";

            ErpCommerceList erpCommerceList = new ErpCommerceList();

            try
            {
                WishListController wishListController = new WishListController(storeKey);

                erpCommerceList = wishListController.UpdateWishListLine(commerceListLine, filterAccountNumber);
            }
            catch (System.Exception e)
            {
                Assert.Fail("Exception : " + e.StackTrace);
            }

            Assert.IsNotNull(erpCommerceList);
            System.Console.WriteLine("Update Wish List Line Info = " + JsonConvert.SerializeObject(erpCommerceList));
        }

    }
}
