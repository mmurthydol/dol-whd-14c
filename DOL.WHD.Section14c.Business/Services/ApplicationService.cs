﻿using System.Threading.Tasks;
using DOL.WHD.Section14c.DataAccess;
using DOL.WHD.Section14c.Domain.Models;
using DOL.WHD.Section14c.Domain.Models.Submission;

namespace DOL.WHD.Section14c.Business.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        public ApplicationService(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public Task<int> SubmitApplicationAsync(ApplicationSubmission submission)
        {
            return _applicationRepository.AddAsync(submission);
        }

        public void ProcessModel(ApplicationSubmission vm)
        {
            CleanupModel(vm);
            SetDefaults(vm);
        }

        private void CleanupModel(ApplicationSubmission model)
        {
            // clear out non-selected wage type
            if (model.PayTypeId == ResponseIds.PayType.Hourly)
            {
                model.PieceRateWageInfo = null;
            }
            else if (model.PayTypeId == ResponseIds.PayType.PieceRate)
            {
                model.HourlyWageInfo = null;
            }

            // clear out non-selected prevailing wage method
            CleanupWageTypeInfo(model.HourlyWageInfo);
            CleanupWageTypeInfo(model.PieceRateWageInfo);
        }

        private void SetDefaults(ApplicationSubmission model)
        {
            // set status
            model.Status = null;
            model.StatusId = StatusIds.Pending;

            // default admin fields
            model.CertificateEffectiveDate = null;
            model.CertificateExpirationDate = null;
            model.CertificateNumber = null;
        }

        private void CleanupWageTypeInfo(WageTypeInfo wageTypeInfo)
        {
            var prevailingWageMethod = wageTypeInfo?.PrevailingWageMethodId;
            if (prevailingWageMethod == ResponseIds.PrevailingWageMethod.PrevailingWageSurvey)
            {
                wageTypeInfo.AlternateWageData = null;
                wageTypeInfo.SCAWageDeterminationId = null;
            }
            else if (prevailingWageMethod == ResponseIds.PrevailingWageMethod.AlternateWageData)
            {
                wageTypeInfo.MostRecentPrevailingWageSurvey = null;
                wageTypeInfo.SCAWageDeterminationId = null;
            }
            else if (prevailingWageMethod == ResponseIds.PrevailingWageMethod.SCAWageDetermination)
            {
                wageTypeInfo.MostRecentPrevailingWageSurvey = null;
                wageTypeInfo.AlternateWageData = null;
            }
        }
    }
}
