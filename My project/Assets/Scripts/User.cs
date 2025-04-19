public class User
{

    public string id;
    public int highscore;
    public string username;

    public User(string userId, string name)
    {
        this.id = userId;
        highscore = 0;
        this.username = name;
    }
}
