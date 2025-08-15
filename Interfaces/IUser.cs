using AssignmentPRN222.Models;

namespace AssignmentPRN222.Interfaces
{
    public interface IUser
    {
        UserProfile GetUserByUserId(string userId);
    }
}
