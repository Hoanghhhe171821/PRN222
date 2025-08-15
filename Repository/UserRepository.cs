using AssignmentPRN222.Interfaces;
using AssignmentPRN222.Models;

namespace AssignmentPRN222.Repository
{

    public class UserRepository : IUser
    {
        protected readonly ProjectPrn222Context _dbcontext;
        public UserRepository(ProjectPrn222Context dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public UserProfile GetUserByUserId(string userId)
        {
            var user = _dbcontext.Users.FirstOrDefault(u => u.Id == userId);
            return user;
        }
    }
}
