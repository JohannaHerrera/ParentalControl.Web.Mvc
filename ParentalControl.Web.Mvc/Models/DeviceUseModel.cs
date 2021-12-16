using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParentalControl.Web.Mvc.Models
{
    public class DeviceUseModel
    {
        // Id del uso del dispositivo
        public int DeviceUseId { get; set; }
        // Día de uso del dispositivo
        public string DeviceUseDay { get; set; }
        // Fecha de creación
        public DateTime DeviceUseCreationDate { get; set; }
        // Id del Infante
        public int InfantAccountId { get; set; }
        // Id del Horario
        public int ScheduleId { get; set; }
    }
}