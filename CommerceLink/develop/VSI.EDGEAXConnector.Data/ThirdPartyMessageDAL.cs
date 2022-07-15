using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Enums.Enums.TMV;

namespace VSI.EDGEAXConnector.Data
{
    public class ThirdPartyMessageDAL : BaseClass
    {
        public ThirdPartyMessageDAL(string storeKey) : base(storeKey)
        {
        }

        public List<ThirdPartyMessage> GetSalesOrdersList(TransactionStatus transactionStatus, int threadCount)
        {
            using (var db = this.GetConnection())
            {
                var query = db.ThirdPartyMessage
                              .Where(m => m.TransactionStatus == (int)transactionStatus &&
                                          m.RetryCount > 0 &&
                                          (
                                              m.RenewalDate == null || m.RenewalDate <= DateTime.UtcNow
                                          )
                            );

                if (threadCount > 0)
                {
                    query = query.Take(threadCount);
                }
                return query.ToList();
            }
        }

        public long UpdateTransactionStatus(string thirdPartyId, TransactionStatus transactionStatus, string user, string salesId = null, DateTime? renewalDate = null, string description = null)
        {
            long messageId = 0;

            using (var db = this.GetConnection())
            {
                var thirdPartyMessage = db.ThirdPartyMessage.FirstOrDefault(m => m.ThirdPartyId == thirdPartyId && m.TransactionStatus != (int)TransactionStatus.MissingParameter_IngramOrderMarkedDeleted);
                if (thirdPartyMessage != null)
                {
                    thirdPartyMessage.TransactionStatus = (int)transactionStatus;
                    thirdPartyMessage.ModifiedBy = user;
                    thirdPartyMessage.ModifiedOn = DateTime.UtcNow;

                    if (!string.IsNullOrEmpty(description))
                    {
                        thirdPartyMessage.Description = description;
                    }

                    if (!string.IsNullOrEmpty(salesId))
                    {
                        thirdPartyMessage.SalesId = salesId;
                    }

                    if (renewalDate != null)
                    {
                        thirdPartyMessage.RenewalDate = Convert.ToDateTime(renewalDate);
                    }

                    if ( transactionStatus == TransactionStatus.SynchedWithThirdPartyFailure ||
                         transactionStatus == TransactionStatus.ErrorInSalesOrderSycnhing ||
                         transactionStatus == TransactionStatus.TransferIngramRequest_OrderTransferedSynchInThirdPartyFailed ||
                         transactionStatus == TransactionStatus.TransferIngramRequest_Other ||
                         transactionStatus == TransactionStatus.TransferIngramRequest_None
                      )
                    {
                        thirdPartyMessage.RetryCount--;
                        if (thirdPartyMessage.RetryCount <= 0)
                        {
                            switch (transactionStatus)
                            {
                                case TransactionStatus.SynchedWithThirdPartyFailure:
                                case TransactionStatus.TransferIngramRequest_OrderTransferedSynchInThirdPartyFailed:
                                    thirdPartyMessage.TransactionStatus = (int)TransactionStatus.CanceledIngramStatusUpdation;
                                    break;

                                case TransactionStatus.ErrorInSalesOrderSycnhing:
                                case TransactionStatus.TransferIngramRequest_Other:
                                case TransactionStatus.TransferIngramRequest_None:
                                    thirdPartyMessage.TransactionStatus = (int)TransactionStatus.ErpOrderCreationRetryLimitExceed;
                                    break;

                                default:
                                    // if retry exceeds then default
                                    thirdPartyMessage.TransactionStatus = (int)TransactionStatus.CanceledIngramStatusUpdation;
                                    break;
                            }
                        }
                    }

                    db.SaveChanges();

                    messageId = thirdPartyMessage.ThirdPartyMessageId;
                }
            }

            return messageId;
        }

        public bool CheckIsSalesOrderExist(string thirdPartyId, TransactionStatus transactionStatus, string user)
        {
            using (var db = this.GetConnection())
            {
                var model = db.ThirdPartyMessage.FirstOrDefault(m => m.ThirdPartyId == thirdPartyId && m.TransactionStatus == (int)transactionStatus && (m.ThirdPartyStatus == "inquiring" || m.ThirdPartyStatus == "pending"));
                if (model != null)
                {
                    return true;
                }
                return false;
            }
        }

        public ThirdPartyMessage GetThirdPartyMessage(string channelReferenceId, string salesId)
        {
            using (var db = this.GetConnection())
            {
                if (string.IsNullOrEmpty(channelReferenceId))
                {
                    return db.ThirdPartyMessage.OrderByDescending(x => x.ThirdPartyMessageId)
                                .FirstOrDefault(m => m.SalesId == salesId);
                }
                else
                {
                    return db.ThirdPartyMessage.OrderByDescending(x => x.ThirdPartyMessageId)
                                .FirstOrDefault(m => m.ThirdPartyId == channelReferenceId);
                }
            }
        }

        public long UpdateThirdPartyStatus(string thirdPartyId, string thirdPartyStatus, string user)
        {
            long messageId = 0;

            using (var db = this.GetConnection())
            {
                var model = db.ThirdPartyMessage.FirstOrDefault(m => m.ThirdPartyId == thirdPartyId && m.TransactionStatus != (int)TransactionStatus.MissingParameter_IngramOrderMarkedDeleted);
                if (model != null)
                {
                    model.ThirdPartyStatus = thirdPartyStatus;
                    model.ModifiedBy = user;
                    model.ModifiedOn = DateTime.UtcNow;

                    db.SaveChanges();

                    messageId = model.ThirdPartyMessageId;
                }
            }

            return messageId;
        }

        public void Add(List<ThirdPartyMessage> messages)
        {
            using (var db = this.GetConnection())
            {
                db.ThirdPartyMessage.AddRange(messages);
                db.SaveChanges();
            }
        }

        public List<string> SaleOrderIdsNotExist(List<string> ids)
        {
            using (var db = this.GetConnection())
            {
                var idsToReturn = new List<string>();
                foreach (string id in ids)
                {
                    if (!db.ThirdPartyMessage.Any(msg => msg.ThirdPartyId == id && msg.TransactionStatus != (int)TransactionStatus.MissingParameter_IngramOrderMarkedDeleted))
                    {
                        idsToReturn.Add(id);
                    }

                }
                return idsToReturn;
            }
        }

        /// <summary>
        /// Get the PR Number of the first Purchase Order Type for AssetId
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="thirdPartyId"></param>
        /// <returns></returns>
        public ThirdPartyMessage GetThirdPartyIdUsingAssetID(string assetId, string thirdPartyId)
        {
            ThirdPartyMessage result = null;

            using (var db = this.GetConnection())
            {
                ThirdPartyMessage latesthirdPartyMessage = db.ThirdPartyMessage.OrderByDescending(i => i.ThirdPartyMessageId).FirstOrDefault(x => x.AssetId == assetId && x.ThirdPartyId == thirdPartyId);

                result = db.ThirdPartyMessage.OrderByDescending(i => i.ThirdPartyMessageId).FirstOrDefault(x => x.AssetId == assetId && (x.OrderType.ToUpper().Equals("PURCHASE") || x.OrderType.ToUpper().Equals("CHANGE")) && x.ThirdPartyMessageId < latesthirdPartyMessage.ThirdPartyMessageId);
            }

            return result;
        }
        
    }
}
