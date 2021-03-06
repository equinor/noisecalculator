﻿using System.Collections.Generic;
using NoiseCalculator.Domain.Entities;
using NoiseCalculator.Domain.Resources;

namespace NoiseCalculator.Domain.DomainServices
{
    public class FootnotesService : IFootnotesService
    {
        public IEnumerable<string> CalculateFootnotes(IEnumerable<SelectedTask> selectedTasks)
        {
            var footnotes = GetStaticFootnotes();
            //footnotes.AddRange(CalculateDynamicFootnotes(selectedTasks));
            
            return footnotes;
        }

        public IEnumerable<string> CalculateFootnotesForReport(IEnumerable<SelectedTask> selectedTasks)
        {
            var footnotes = GetStaticFootnotesForReport();
            //footnotes.AddRange(CalculateDynamicFootnotes(selectedTasks));

            return footnotes;
        }
        
        private static IEnumerable<string> GetStaticFootnotes()
        {
            var staticFootnotes = new List<string>
                {
                    DomainResources.FooterCalculations,
                    DomainResources.FooterNoiseProtection
                };

            return staticFootnotes;
        }

        private static IEnumerable<string> GetStaticFootnotesForReport()
        {
            var staticFootnotes = new List<string>
                {
                    DomainResources.FooterCalculationsForReport,
                    DomainResources.FooterCalculationsAreaNoiseForReport,
                    DomainResources.FooterNoiseProtection
                };

            return staticFootnotes;
        }

        //private IEnumerable<string> CalculateDynamicFootnotes(IEnumerable<SelectedTask> selectedTasks)
        //{
        //    var enumerable = selectedTasks as IList<SelectedTask> ?? selectedTasks.ToList();
        //    var hasNoisyWork = (enumerable.Any(x => x.Task.Role.RoleType != RoleTypeEnum.AreaNoise));
        //    var hasRegularTasks = (hasNoisyWork && enumerable.Any(x => x.HelicopterTaskId == 0));

        //    IList<string> dynamicFootnotes = new List<string>();

        //    if (!hasNoisyWork) return dynamicFootnotes;

        //    dynamicFootnotes.Add(DomainResources.FooterDynamicNoiseProtection);
        //    dynamicFootnotes.Add(DomainResources.FooterDynamicCorrectionForMeasuredNoiseLevel);

        //    if (hasRegularTasks)
        //    {
        //        dynamicFootnotes.Add(DomainResources.FooterDynamicValidForAreaNoiseUpTo90dBA);
        //    }

        //    return dynamicFootnotes;
        //}
    }
}
