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
        private readonly ITrackableRepository<Sysparam> sysparamRepo;

        public RentalManagementService(
            ITrackableRepository<RentFixedFee> rentFixedFeeRepo,
            ITrackableRepository<UtilityFee> utilityFeeRepo,
            ITrackableRepository<Person> personRepo,
            ITrackableRepository<PersonalLiability> personalLiabilityRepo,
            IUnitOfWork unitOfWork,
            ITrackableRepository<Sysparam> sysparamRepo)
        {
            this.rentFixedFeeRepo = rentFixedFeeRepo;
            this.utilityFeeRepo = utilityFeeRepo;
            this.personRepo = personRepo;
            this.personalLiabilityRepo = personalLiabilityRepo;
            this.unitOfWork = unitOfWork;
            this.sysparamRepo = sysparamRepo;
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
                    await this.rentFixedFeeRepo.DeleteAsync(seqNo);
                    break;
                case RentType.Utility:
                    await this.utilityFeeRepo.DeleteAsync(seqNo);
                    break;
                case RentType.PersonalLiability:
                    await this.personalLiabilityRepo.DeleteAsync(seqNo);
                    break;
            }
        }

        /// <summary>
        /// 依 GroupId 取得系統參數資料
        /// </summary>
        public IEnumerable<SystemparamDTO> GetSysparamByGroupId(string groupId)
        {
            return this.sysparamRepo.Queryable()
                .Where(s => s.GroupId == groupId)
                .Select(s => new SystemparamDTO 
                {
                    ItemId = s.ItemId,
                    ItemValue = s.ItemValue
                });
        }

        /// <summary>
        /// 新增固定租金項目
        /// </summary>
        public async Task InsertFixedFee(RentFixedFee rentFixedFee)
        {
            this.rentFixedFeeRepo.Insert(rentFixedFee);

            await this.unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 新增公共租金項目
        /// </summary>
        /// <param name="utilityFee"></param>
        public async Task InsertUtilityFee(UtilityFee utilityFee)
        {
            DateTime startTime = new DateTime(1970, 1, 1, 8, 0, 0);

            utilityFee.CreateOn = 
                Convert.ToInt32((DateTime.Now - startTime).TotalSeconds);
            utilityFee.IsClosed = false;

            this.utilityFeeRepo.Insert(utilityFee);

            await this.unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 取得人員資料
        /// </summary>
        public IEnumerable<PersonDTO> GetPersons()
        {
            return this.personRepo.Queryable().Select(p => new PersonDTO 
            {
                PersonId = p.PersonId,
                PersonName = p.PersonName
            });
        }

        /// <summary>
        /// 新增人員欠債項目
        /// </summary>
        public async Task InsertPersonalLiabilityFee(
            PersonalLiability personalLiability)
        {
            DateTime startTime = new DateTime(1970, 1, 1, 8, 0, 0);

            personalLiability.CreateOn =
                Convert.ToInt32((DateTime.Now - startTime).TotalSeconds);
            personalLiability.IsClosed = false;

            this.personalLiabilityRepo.Insert(personalLiability);

            await this.unitOfWork.SaveChangesAsync();
        }
    }
}
