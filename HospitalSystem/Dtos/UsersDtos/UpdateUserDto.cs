using System.ComponentModel.DataAnnotations.Schema;
using HospitalSystem.Dtos;

namespace HospitalSystem.Dtos.UsersDtos
{
    public class UpdateUserDto : CreateUserDto
    {
        public int Id { get; set; }
    }
}