using System.Collections.Generic;

namespace Algorithms
{
    public class WebProject : IClassified<WebProject>
    {
        public string Name { get; private set; }
	    public double FirstMeasurement { get { return UsersPerDay; } }
	    public double SecondMeasurement { get { return UsersPerMonth; } }

	    public long UsersPerDay { get; private set; }

        public long UsersPerMonth { get; private set; }

        private WebProject(string name, long usersPerDay, long usersPerMonth)
        {
            Name = name;
            UsersPerDay = usersPerDay;
            UsersPerMonth = usersPerMonth;
        }

	    public static readonly List<WebProject> Projects = new List<WebProject>
        {
            new WebProject("Yandex", 30109, 58432),
            new WebProject("Mail.ru", 26327, 59031),
            new WebProject("Vk.com", 25702, 52103),
            new WebProject("Odnoklassniki", 17456, 41073),
            new WebProject("Google", 13714, 47941),
            new WebProject("Youtube", 10848, 44565),
            new WebProject("Wikipedia", 4681, 33353),
            new WebProject("Avito", 4067, 25846),
            new WebProject("Rambler", 4018, 16553),
            new WebProject("Facebook", 3739, 23807),
            new WebProject("Livejournal", 2519, 19084),
            new WebProject("Kinopoisk", 1589, 15370),
            new WebProject("Rutracker", 1518, 13046)
        };
    }
}
