﻿using System.Collections.Generic;

namespace NoiseCalculator.UI.Web.Areas.Admin.ViewModels.GenericTask
{
    public class TaskDefinitionGenericViewModel
    {
        public int Id { get; set; }
        public string SystemName { get; set; }
        public string RoleType { get; set; }

        public IList<TaskListItemViewModel> Tasks { get; set; }

        
        public TaskDefinitionGenericViewModel()
        {
            Tasks = new List<TaskListItemViewModel>();
        }
    }
}