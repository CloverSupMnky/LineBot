using LineBot.Asset.Enum;
using LineBot.Asset.Model.Resp;
using LineBot.Entitys.Models;
using LineBot.Module.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URF.Core.Abstractions;
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
        private readonly IUnitOfWork unitOfWork;

        public RentalManagementService(
            ITrackableRepository<RentFixedFee> rentFixedFeeRepo,
            ITrackableRepository<UtilityFee> utilityFeeRepo,
            ITrackableRepository<Person> personRepo,
            ITrackableRepository<PersonalLiability> personalLiabilityRepo,
            IUnitOfWork unitOfWork)
        {
            this.rentFixedFeeRepo = rentFixedFeeRepo;
            this.utilityFeeRepo = utilityFeeRepo;
            this.personRepo = personRepo;
            this.personalLiabilityRepo = personalLiabilityRepo;
            this.unitOfWork = unitOfWork;
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

        /// <summary>
        /// 刪除租金項目
        /// </summary>
        public async Task DeleteRentItem(RentDetail detail)
        {
           await this.DeleteRentItemByTypeAndSeqNo(detail.Type,detail.SeqNo);

           await this.unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 依租金類型、序號刪除項目
        /// </summary>
        private async Task DeleteRentItemByTypeAndSeqNo(
            RentType type, int seqNo)
        {
            switch (type)
            {
                case RentType.Fixed:
                    await this.DeleteFixed(seqNo);
                    break;
                case RentType.Utility:
                    await this.DeleteUtility(seqNo);
                    break;
                case RentType.PersonalLiability:
                    await this.DeletePersonalLiability(seqNo);
                    break;
            }
        }

        /// <summary>
        /// 刪除租金固定費用
        /// </summary>
        private async Task DeleteFixed(int seqNo)
        {
            var rentFixed = this.rentFixedFeeRepo.Queryable()
                .FirstOrDefault(r => r.SeqNo == seqNo);

            if (rentFixed == null) return;

            await this.rentFixedFeeRepo.DeleteAsync(rentFixed);
        }

        /// <summary>
        /// 刪除租金公共費用
        /// </summary>
        private async Task DeleteUtility(int seqNo)
        {
            var utilityFee = this.utilityFeeRepo.Queryable()
                .FirstOrDefault(u => u.SeqNo == seqNo);

            if (utilityFee == null) return;

            await this.utilityFeeRepo.DeleteAsync(utilityFee);
        }

        /// <summary>
        /// 刪除人員欠債費用
        /// </summary>
        private async Task DeletePersonalLiability(int seqNo)
        {
            var personalLiability = this.personalLiabilityRepo.Queryable()
                .FirstOrDefault(p => p.SeqNo == seqNo);

            if (personalLiability == null) return;

            await this.personalLiabilityRepo.DeleteAsync(personalLiability);
        }
    }
}
