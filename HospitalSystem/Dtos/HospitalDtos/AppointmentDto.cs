using System.ComponentModel.DataAnnotations;

namespace HospitalSystem.Dtos.HospitalDtos
{
    public class AppointmentDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اختر الطبيب المعالج")]
        [Display(Name = "الطبيب")]
        public int DoctorId { get; set; }
        public string? DoctorName { get; set; }

        [Required(ErrorMessage = "اختر المريض")]
        [Display(Name = "المريض")]
        public int PatientId { get; set; }
        public string? PatientName { get; set; }

        [Required(ErrorMessage = "تاريخ ووقت الموعد مطلوبان")]
        [Display(Name = "تاريخ الموعد")]
        [DataType(DataType.DateTime)]
        public DateTime AppointmentDate { get; set; } = DateTime.Now;

        [Display(Name = "حالة الموعد")]
        public string Status { get; set; } = "مؤكد";
    }
}