using HospitalSystem.Models;
using HospitalSystem.Repositories.Base;

namespace HospitalSystem.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetUsersWithRolesAndDoctor();
        User? GetByUsername(string username);
        User? GetByUid(string uid);
        bool UsernameExists(string username, int? exceptUserId = null);
        int? GetDoctorId(int userId);

        // أدوار المستخدم
        List<int> GetRoleIds(int userId);
        List<string> GetRoleNames(int userId);
        void SetRoles(int userId, IEnumerable<int> roleIds);

        // ملفات المستخدم
        List<UserFile> GetFiles(int userId);
        UserFile? GetFile(int fileId);
        void AddFile(UserFile file);
        void DeleteFile(UserFile file);
    }
}
