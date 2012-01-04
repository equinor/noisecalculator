using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoiseCalculator.UI.Web.ViewModels
{
    public class HelideckViewModel
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Role { get; set; }
        public int? HelicopterIdSelected { get; set; }
        public int NoiseProtectionIdSelected { get; set; }
        public int WorkIntervalIdSelected { get; set; }
        
    //<p style="margin-top: 15px;">
    //    <label for="HelicopterId" class="label-130px">Helicopter</label>
    //    :
    //    <select id="HelicopterId" class="helicopterDropDown">
    //        <option>EC225</option>
    //        <option>Helicopter 2</option>
    //        <option>Helicopter 3</option>
    //    </select>
    //</p>
        
    //<p>
    //    <label for="NoiseProtectionId" class="label-130px">Noise Protection</label>
    //    :
    //    <select id="NoiseProtectionId" class="helicopterDropDown">
    //        <option>QuietPro</option>
    //        <option>Double</option>
    //    </select>
    //</p>
        
    //<p>
    //    <label for="WorkIntervalId" class="label-130px">Work Interval</label>
    //    :
    //    <select id="WorkIntervalId" class="helicopterDropDown">
    }
}