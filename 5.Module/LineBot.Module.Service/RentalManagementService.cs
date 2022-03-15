using LineBot.Asset.Enum;
using LineBot.Asset.Model.Resp;
using LineBot.Entitys.Models;
using LineBot.Module.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URF.Core.Abstractions.Trackable;

namespace LineBot.Module.Service
{
    /// <summary>
    /// 租金管理
    /// </summary>
    public class RentalManagementService : IRentalManagementService
    {
        private readonly ITrackableRepository<RentFixedFee> rentFixedFeeRepo;
        private readonly ITrackableRepository<UtilityFee> utilityFeeRepo;
        private readonly ITrackableRepository<Person> personRepo;
        private readonly ITrackableRepository<PersonalLiability> personalLiabilityRepo;

        public RentalManagementService(
            ITrackableRepository<RentFixedFee> rentFixedFeeRepo,
            ITrackableRepository<UtilityFee> utilityFeeRepo,
            ITrackableRepository<Person> personRepo,
            ITrackableRepository<PersonalLiability> personalLiabilityRepo)
        {
            this.rentFixedFeeRepo = rentFixedFeeRepo;
            this.utilityFeeRepo = utilityFeeRepo;
            this.personRepo = personRepo;
            this.personalLiabilityRepo = personalLiabilityRepo;
        }

        /// <summary>
        /// 取得租金明細
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RentDetail> GetRentDetail()
        {
            // 取得租金固定費用明細
            var fixedFeeDetails = this.GetFixedFeeDetails();

            // 取得租金公共費用明細
            var utilityFeeDetails = this.GetUtilityFeeDetails();

            // 取得租金人員欠債明細
            var personalLiabilityFeeDetails =
                this.GetPersonalLiabilityFeeDetails();

            return fixedFeeDetails
                .Concat(utilityFeeDetails)
                .Concat(personalLiabilityFeeDetails);
        }

        /// <summary>
        /// 取得租金固定費用明細
        /// </summary>
        /// <returns></returns>
        private IEnumerable<RentDetail> GetFixedFeeDetails()
        {
            return this.rentFixedFeeRepo.Queryable()
                .Select(r => new RentDetail
                {
                    Type = RentType.Fixed,
                    Name = r.ItemName,
                    Fee = r.Fee.ToString("N0"),
                    SeqNo = r.SeqNo
                });
        }

        /// <summary>
        /// 取得租金公共費用明細
        /// </summary>
        /// <returns></returns>
        private IEnumerable<RentDetail> GetUtilityFeeDetails()
        {
            return this.utilityFeeRepo.Queryable()
                .Select(u => new RentDetail
                {
                    Type = RentType.Utility,
                    Name = u.ItemName,
                    Fee = u.Fee.ToString("N0"),
                    SeqNo = u.SeqNo
                });
        }

        /// <summary>
        /// 取得租金人員欠債明細
        /// </summary>
        /// <returns></returns>
        private IEnumerable<RentDetail> GetPersonalLiabilityFeeDetails()
        {
            var personDic = this.personRepo.Queryable()
                .ToDictionary(p => p.PersonId, p => p.PersonName);

            return this.personalLiabilityRepo.Queryable()
                .Select(p => new RentDetail
                {
                    Type = RentType.PersonalLiability,
                    Name = 
                    $"{personDic[p.DebtorId]} 欠 {personDic[p.CreditorId]} ({p.Description})",
                    Fee = p.Fee.ToString("N0"),
                    SeqNo = p.SeqNo
                });
        }
    }
}
