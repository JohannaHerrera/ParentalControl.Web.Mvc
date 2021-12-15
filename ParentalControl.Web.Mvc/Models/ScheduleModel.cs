using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ParentalControl.Web.Mvc.Models
{
    public class ScheduleModel
    {

        public int ScheduleId { get; set; }
        //[DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}",ApplyFormatInEditMode =true)]
        public DateTime ScheduleStartTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime ScheduleEndTime { get; set; }
        public DateTime ScheduleCreationDate { get; set; }
        public int ParentId { get; set; }
        public bool Validate(ScheduleModel scheduleModel)
        {
            bool validation=false;

            if (scheduleModel.ScheduleStartTime >= scheduleModel.ScheduleEndTime)
            {
                //Inicio no puede ser mayor o igual a la hora final
                validation = false;

            }
            else
            {
                validation = true;
            }

            return validation;
        }

    }
}