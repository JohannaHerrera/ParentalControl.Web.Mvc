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
        public string Validate(ScheduleModel scheduleModel)
        {
            string message = string.Empty;

            if (scheduleModel.ScheduleStartTime > scheduleModel.ScheduleEndTime)
            {
                return "La hora inicio debe ser menor a la hora fin"; // Y si quiero darle 23 horas de uso?
            }

            return message;
        }

    }
}