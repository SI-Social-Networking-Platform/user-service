namespace UserService.Model;

public class FollowedUser
{
    public int FollowerId { get; set; }
    public int FollowedId { get; set; }

    public User Follower { get; set; }
    public User Followed { get; set; }
}