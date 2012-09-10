using System.Collections.Generic;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Infrastructure.DataAccess.Interfaces;
using NoiseCalculator.UI.Web.ApplicationServices.Admin.Interfaces;
using NoiseCalculator.UI.Web.Areas.Admin.EditModels;
using NoiseCalculator.UI.Web.Areas.Admin.ViewModels.Generic;

namespace NoiseCalculator.UI.Web.ApplicationServices.Admin.Implementations
{
    public class HelicopterWorkIntervalService : IHelicopterWorkIntervalService
    {
        private readonly IDAO<HelicopterWorkInterval, int> _helicopterWorkIntervalDAO;

        public HelicopterWorkIntervalService(IDAO<HelicopterWorkInterval, int> helicopterWorkIntervalDAO)
        {
            _helicopterWorkIntervalDAO = helicopterWorkIntervalDAO;
        }


        public GenericDefinitionIndexViewModel Index()
        {
            IEnumerable<HelicopterWorkInterval> helicopterWorkIntervals = _helicopterWorkIntervalDAO.GetAll();

            GenericDefinitionIndexViewModel viewModel = new GenericDefinitionIndexViewModel();
            foreach (var helicopterWorkInterval in helicopterWorkIntervals)
            {
                viewModel.Definitions.Add(new GenericDefinitionViewModel
                    {
                        Id = helicopterWorkInterval.Id, 
                        SystemName = helicopterWorkInterval.Title
                    });
            }

            return viewModel;
        }


        public GenericDefinitionViewModel Create(GenericDefinitionEditModel editModel)
        {
            HelicopterWorkInterval helicopterWorkInterval = new HelicopterWorkInterval
                {
                    Title = editModel.Title
                };

            _helicopterWorkIntervalDAO.Store(helicopterWorkInterval);

            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel
                {
                    Id = helicopterWorkInterval.Id, 
                    SystemName = helicopterWorkInterval.Title
                };

            return viewModel;
        }


        public GenericDefinitionViewModel EditHelicopterTypeForm(int id)
        {
            HelicopterWorkInterval helicopterWorkInterval = _helicopterWorkIntervalDAO.Get(id);

            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel
                {
                    Id = helicopterWorkInterval.Id,
                    SystemName = helicopterWorkInterval.Title,
                    HasTranslationSupport = false
                };

            return viewModel;
        }

        public GenericDefinitionViewModel Edit(int id, GenericDefinitionEditModel editModel)
        {
            HelicopterWorkInterval helicopterWorkInterval = _helicopterWorkIntervalDAO.Get(id);
            helicopterWorkInterval.Title = editModel.Title;

            _helicopterWorkIntervalDAO.Store(helicopterWorkInterval);

            GenericDefinitionViewModel viewModel = new GenericDefinitionViewModel
                {
                    Id = helicopterWorkInterval.Id, 
                    SystemName = helicopterWorkInterval.Title
                };

            return viewModel;
        }

        
        public DeleteConfirmationViewModel DeleteConfirmationForm(int id)
        {
            HelicopterWorkInterval helicopterWorkInterval = _helicopterWorkIntervalDAO.Get(id);

            DeleteConfirmationViewModel viewModel = new DeleteConfirmationViewModel
                {
                    Id = helicopterWorkInterval.Id.ToString(),
                    Title = helicopterWorkInterval.Title
                };

            return viewModel;
        }

        public void Delete(int id)
        {
            HelicopterWorkInterval helicopterWorkInterval = _helicopterWorkIntervalDAO.Load(id);
            _helicopterWorkIntervalDAO.Delete(helicopterWorkInterval);
        }
    }
}