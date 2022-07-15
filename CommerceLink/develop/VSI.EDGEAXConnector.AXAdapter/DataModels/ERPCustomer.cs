//using AutoMapper;

//namespace VSI.EDGEAXConnector.AXAdapter.DataModels
//{

//    /// <summary>
//    /// This Class maps ERP Customer to CRT Customer.
//    /// </summary>
//    public class ERPCustomer : Customer
//    {

//        #region Public Methods

//        /// <summary>
//        /// GetDerivedObject maps ERP Customer to CRT Customer.
//        /// </summary>
//        /// <param name="c"></param>
//        /// <returns></returns>
//        public static ERPCustomer GetDerivedObject(Customer c)
//        {
//            Mapper.CreateMap<Customer, ERPCustomer>();
//            return Mapper.Map<Customer, ERPCustomer>(c);
//        }

//        /// <summary>
//        /// Getter Setter for Permanent Address.
//        /// </summary>
//        public Address PermanentAddress { get; set; }

//        /// <summary>
//        /// Item Variable.
//        /// </summary>
//        public object Item { get; set; }

//        #endregion
//    }
//}
