using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace Guestbook
{
    
    // POST OBJECT //
    public class Post{
        public int id { get; set; }
        public string userid { get; set; }
        public string topic { get; set; }
        public string post { get; set; }
        public DateTime created { get; set; }

        public Post(int _id, string _userid, string _topic, string _post, DateTime _created)
        {
            id = _id;
            userid = _userid;
            topic = _topic;
            post = _post;
            created = _created;

        }

    }

    // TOPIC OBJECT //
    public class Topic{
        public int id { get; set; }
        public string userid { get; set; }
        public string topic { get; set; }
        public DateTime created { get; set; }

        public Topic(int _id, string _userid, string _topic, DateTime _created)
        {
            id = _id;
            userid = _userid;
            topic = _topic;
            created = _created;

        }

    }

    // USER OBJECT //
    public class User{
        public int id { get; set; }
        public string userid { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public DateTime created { get; set; }

        public User(int _id, string _usderId, string _password, string _name, string _email, DateTime _created)
        {
            id = _id;
            userid = _usderId;
            name = _name;
            password = _password;
            email = _email;
            created = _created;

        }

    }

    public class Handler{
        public static bool loggedIn = false;
        public static string CurrTopic = null;
        public static string CurrUser = null;
        public static bool alert = false;
        public static string alertString = null;

        public static List<User> users = new List<User>(); // LIST OF [USER] OBJECTS
        public static List<Topic> topics = new List<Topic>(); // LIST OF [TOPIC] OBJECTS
        public static List<Post> posts = new List<Post>(); // LIST OF [POST] OBJECTS


        // ---- //
        // MISC //
        // ---- //
        /**************************************************/
        // UPDATE HANDLER USERS, TOPICS AND POSTS //
        public static void Update(){
            List<User> users = getUsers("ALL");
            List<Topic> topics = getTopics();
            List<Post> posts = getPosts("ALL");

            Console.WriteLine("// ---------- //");
            Console.WriteLine("// UPDATING //");
            Console.WriteLine("Users Count: " + users.Count);
            Console.WriteLine("Topics Count: " + topics.Count);
            Console.WriteLine("Posts Count: " + posts.Count);
            Console.WriteLine("// ---------- //");

        }

        // EMPTY DB AND CREATE INITIAL TABLES //
        /* For dev purpose */
        /* Delete before live! */
        public static void newDB() {
            // New Database connection //
            MySqlConnection connection = getConnection();
            connection.Open();

            var command = new MySqlCommand();
            command.Connection = connection;

            
            // CREATE USERS TABEL //
            command.CommandText = "DROP TABLE IF EXISTS users";
            command.ExecuteNonQuery();
            command.CommandText = @"CREATE TABLE users(
                id INTEGER AUTO_INCREMENT, 
                userid TEXT NOT NULL,
                password TEXT, 
                name TEXT, 
                email TEXT, 
                created DATETIME,
                PRIMARY KEY (id),
                UNIQUE (userid)
                )";
            command.ExecuteNonQuery();
            
            // CREATE TOPICS TABEL //
            command.CommandText = "DROP TABLE IF EXISTS topics";
            command.ExecuteNonQuery();
            command.CommandText = @"CREATE TABLE topics(
                id INTEGER AUTO_INCREMENT,
                userid TEXT,
                topic TEXT,
                created DATETIME,
                PRIMARY KEY (id)
                )";

            command.ExecuteNonQuery();

            // CREATE POSTS TABEL //
            command.CommandText = "DROP TABLE IF EXISTS posts";
            command.ExecuteNonQuery();
            command.CommandText = @"CREATE TABLE posts(
                id INTEGER AUTO_INCREMENT,
                userid TEXT,
                topic TEXT,
                post TEXT,
                created DATETIME,
                PRIMARY KEY (id)
                )";
            command.ExecuteNonQuery();

            // CLOSE DB CONNECTION AND DISPOSE QUERY //
            command.Dispose();
            connection.Close();

        }

        // WRITE SOME INITIAL VAULES TO DATABASE //
        /* For dev purpose */
        /* Delete before live! */
        public static void initialValuesDB() {


            // [TOPIC] int _id, string _userid, string _topic, int _posts, DateTime _created
            // [POST] int _id, string _userid, string _topic, string _post, DateTime _created
            // [USER] int _id, string _usderId, string _password, string _name, string _email, DateTime _created

            DateTime created = DateTime.Now;
            string query = "";

            // TOPICS //
            query = "INSERT INTO topics(userid, topic, created) values('JaneDoe', 'R??dda hemmavideor p?? VHS och Betamax', '2020-10-30 16:03');";
            ExecutQuery(query);

            query = "INSERT INTO topics(userid, topic, created) values('Photolle', 'Dr??nare under 250 gram', '2021-08-04 22:53');";  
            ExecutQuery(query);

            query = "INSERT INTO topics(userid, topic, created) values('cat_fish_1', 'DJI tr??den', '2015-03-15 09:41');";  
            ExecutQuery(query);

            query = "INSERT INTO topics(userid, topic, created) values('hellmix', 'Dashcam men vilken?', '2021-08-12 17:56');";  
            ExecutQuery(query);

            // POSTS //
            query = "INSERT INTO posts(userid, topic, post, created) values('JaneDoe', 'R??dda hemmavideor p?? VHS och Betamax', 'R??dda hemmavideor p?? VHS och Betamax \n Vi har hemmavideor p?? b??de VHS och Betamax. De jag vill r??dda f??rst ??r de p?? VHS. Vi har en gammal VHS-spelare men den ??r inte i toppskick. Vi ska ev. se om det ??r n??got vi kan g??ra med den eller om det helt enkelt f??r duga. F??rs??kte se om det gick att k??pa en ny VHS-spelare men det verkar inte s??. De verkar ??ven g?? dyrt p?? Tradera samtidigt som vi inte har m??jlighet att kontrollera hur de fungerar. Betamax ??r jag d??ligt insatt i. Aldrig anv??nt det sj??lv med det kommer fr??n min far. \n Hur har ni gjort som har r??ddat VHS och kanske Betamax?', '2020-10-30 16:03');";  
            ExecutQuery(query);

            query = "INSERT INTO posts(userid, topic, post, created) values('Bowman99', 'R??dda hemmavideor p?? VHS och Betamax', 'Jag fick tag p?? en hyfsat oanv??nd fin VHS-spelare och anv??nde den adaptern du visade fr??n Kjell. Funkade kalas. Vart finns du? Du skulle kunna f?? l??na VHS-spelaren om du vill?', '2020-10-30 16:16');";  
            ExecutQuery(query);

            query = "INSERT INTO posts(userid, topic, post, created) values('anon301768', 'R??dda hemmavideor p?? VHS och Betamax', 'Jag anv??nder en s??n h??r n??r jag kopierade alla min vhs band. \n https://www.elgato.com/en/video-capture \n Jag har en s?? gott som ny SONY SLV-SE800 som funkar perfekt. \n https://picclick.fr/SONY-SLV-SE800-Video-Cassette-Recorder-VH...', '2020-11-07 13:25');";  
            ExecutQuery(query);

            query = "INSERT INTO posts(userid, topic, post, created) values('Photolle', 'Dr??nare under 250 gram', 'Hej! \n Jag undrar. Om man k??per dji mini 2 som v??ger 249 gram. Beh??ver jag ett dr??nakort eller g?? n??gon utbildning? Eller kan jag bara skriva mitt telefonnummer etc p?? dr??naren och s?? kan jag bara tuta och k??ra inom synh??ll och max 120 m upp. Eller vad ??r reglerna. L??nka helst inte till transportstyrelsens hemsida f??r jag f??rst??r typ inte vad dem menar.', '2021-08-04 22:53');";  
            ExecutQuery(query);

            query = "INSERT INTO posts(userid, topic, post, created) values('Printscreen', 'Dr??nare under 250 gram', 'Tillverkaren av din tillt??nkta dr??nare t??cker reglerna nu i ??verg??ngsperioden: \n https://djistockholm.se/nyheter/flyger-jag-verkligen-lagligt \n Under ??verg??ngsperioden g??ller f??ljande: Under 250 gram (A0/A1): Flygning ??ver m??nniskor till??tet men inte ??ver folksamlingar. \n Du m??ste ??nd?? ha ett dr??narkort. S??na h??r saker tas upp under utbildningen.', '2021-08-04 23:23');";  
            ExecutQuery(query);

            query = "INSERT INTO posts(userid, topic, post, created) values('Joppis', 'Dr??nare under 250 gram', 'Transportstyrelsen har en guide som du kan anv??nda ocks??. Typ som en lathund. \n https://dronarsidan.transportstyrelsen.se/guide', '2021-08-04 23:29');";  
            ExecutQuery(query);

            query = "INSERT INTO posts(userid, topic, post, created) values('cat_fish_1', 'DJI tr??den', 'DJI tr??den \n En tr??d f??r oss som flyger med DJI dr??nare d??r vi kan byta erfarenheter, tips, bilder och videos och lite s??nt. Flyger sj??lv en Phantom 2 vision + v3 som jag ??r heln??jd med!', '2015-03-15 09:41');";  
            ExecutQuery(query);

            query = "INSERT INTO posts(userid, topic, post, created) values('DaBone', 'DJI tr??den', 'Kan ju vara trevligt. \n Sj??lv har jag en Phantom 2 med H3-3D och GoPro Hero 3, en nyligen inf??rskaffad Inspire 1 och en Hexacopter med DJI:s A2. \n Anv??nder FPV-utrustning p?? alla tre. Phantomen har jag Fatshark till och de andra har LCD-sk??rm/iPad. \n En bild p?? min nya, som jag ??n s?? l??nge ??r v??ldigt n??jd med.', '2015-03-15 14:14');";  
            ExecutQuery(query);

            query = "INSERT INTO posts(userid, topic, post, created) values('cat_fish_1', 'DJI tr??den', 'Ingen liten slant du spenderat! Var ocks?? inne p?? Inspire 1 men vet ej om jag hade anv??nt den mycket nog f??r de priset', '2015-03-15 17:26');";  
            ExecutQuery(query);

            query = "INSERT INTO posts(userid, topic, post, created) values('hellmix', 'Dashcam men vilken?', 'Dashcam men vilken? \n G??r i tankarna nu att k??pa en dashcam men vet inte vilken man ska v??lja s?? v??nder mig hit om r??d. \n Har snubblat in p?? den h??r https://blackvue.com/product/dr900x-2ch/ efter man hittade kod 1fasthawk som ger 50$ rabatt men jag vet inte riktig vad f??r tillbeh??r man ska v??lja. \n Men fastade f??r den lite eftersom den spelar in i 4K som jag f??rst??r ??r lite som ett krav f??r att kunna se regnr om n??got skulle h??nda samt den ??r liten och diskret samt det medf??ljer en separat kamera f??r att h??lla koll bak??t. \n Men r??d och tips uppskattas.', '2021-08-12 17:56');";  
            ExecutQuery(query);

            query = "INSERT INTO posts(userid, topic, post, created) values('HighHorse', 'Dashcam men vilken?', 'Har letat lite efter dashcam en stund med och tittat p?? denna https://www.70mai.com/en/dashcams/ (Xiaomi) verkar vara bra valuta f??r pengarna. \n Blackvue DR900X k??nns ju r??tt dyr efter vad man f??r, 1500 vs 5000 men har inte s?? bra koll.', '2021-08-12 18:29');";  
            ExecutQuery(query);

            // USERS //
            query = "INSERT INTO users(userid, password, name, email, created) values('JaneDoe', 'Jane1234', 'Jane Danielsson', 'jane.danielsson@mail.com', '" +created+ "');";  
            ExecutQuery(query);

            query = "INSERT INTO users(userid, password, name, email, created) values('Photolle', 'Olle1234', 'Olle Svennson', 'Olle.Svennson@mail.com', '" +created+ "');";  
            ExecutQuery(query);


        }

        // EXCECUTE AN MYSQL QUERY ON DB //
        // Requires a [STRING MYSQL_QUERY] //
        public static void ExecutQuery(string _query) {
            // New Database connection //
            MySqlConnection connection = getConnection();
            connection.Open();

            var command = new MySqlCommand();
            command.Connection = connection;

            command.CommandText = _query;
            command.ExecuteNonQuery();
            //Console.WriteLine(_query);

            // CLOSE DB CONNECTION AND DISPOSE QUERY //
            command.Dispose();
            connection.Close();

        }

        // CONNECT TO DATABASE //
        public static MySqlConnection getConnection() {
            string connectionString = @"server=localhost;userid=root;password=;database=guestbookdb;SslMode = none";

                MySqlConnection connection = new MySqlConnection(connectionString);

                //Console.WriteLine("Connection Open!");
                return connection;
        }    
        /**************************************************/



        // ----------- //
        // WRITE TO DB //
        // ----------- //
        /**************************************************/
        // WRITE TOPIC TO DATABASE //
        // Requires a [STRING TOPIC] and a [STRING POST] //
        public static void writeDBTopic(string topic, string post) {
            // New Database connection //
            MySqlConnection connection = getConnection();
            connection.Open();

            // [TOPIC] int _id, string _userid, string _topic, int _posts, DateTime _created
            // [POST] int _id, string _userid, string _topic, string _post, DateTime _created
            // [USER] int _id, string _usderId, string _password, string _name, string _email, DateTime _created

            var query = "INSERT INTO topics(created, userid, topic) VALUES(@created, @userid, @topic)";
            var command = new MySqlCommand(query, connection);

            DateTime created = DateTime.Now;
            command.Parameters.AddWithValue("@created", created);
            command.Parameters.AddWithValue("@userid", CurrUser);
            command.Parameters.AddWithValue("@topic", topic);
            command.Prepare();
            command.ExecuteNonQuery();

            Console.WriteLine("Write to [TOPICS] DB: " + Handler.CurrUser + ", " + topic + ", " + created);

            writeDBPost(topic, post);

            // CLOSE DB CONNECTION AND DISPOSE QUERY //
            command.Dispose();
            connection.Close();

        }

        // WRITE POST TO DATABASE //
        // Requires a [STRING TOPIC] and a [STRING POST] //
        public static void writeDBPost(string topic, string post) {
            // New Database connection //
            MySqlConnection connection = getConnection();
            connection.Open();

            // [TOPIC] int _id, string _userid, string _topic, int _posts, DateTime _created
            // [POST] int _id, string _userid, string _topic, string _post, DateTime _created
            // [USER] int _id, string _usderId, string _password, string _name, string _email, DateTime _created

            var query = "INSERT INTO posts(created, userid, post, topic) VALUES(@created, @userid, @post, @topic)";
            var command = new MySqlCommand(query, connection);

            DateTime created = DateTime.Now;
            command.Parameters.AddWithValue("@created", created);
            command.Parameters.AddWithValue("@userid", CurrUser);
            command.Parameters.AddWithValue("@topic", topic);
            command.Parameters.AddWithValue("@post", post);
            command.Prepare();
            command.ExecuteNonQuery();

            Console.WriteLine("Write to [POSTS] DB: " + CurrUser + ", " + topic + ", " + post + ", " + created);

            // CLOSE DB CONNECTION AND DISPOSE QUERY //
            command.Dispose();
            connection.Close();

        }

        // WRITE USER TO DATABASE //
        // Requires a [STRING USERiD], a [STRING PASSWORD], a [STRING NAME], and a [STRING EMAIL] //
        public static void writeDBUser(string userid, string password, string name, string email) {
            // New Database connection //
            MySqlConnection connection = getConnection();
            connection.Open();

            // [TOPIC] int _id, string _userid, string _topic, int _posts, DateTime _created
            // [POST] int _id, string _userid, string _topic, string _post, DateTime _created
            // [USER] int _id, string _usderId, string _password, string _name, string _email, DateTime _created

            string query = "INSERT INTO users(userid, password, name, email, created) VALUES(@userid, @password, @name, @email, @created)";
            var command = new MySqlCommand(query, connection);

            DateTime created = DateTime.Now;
            command.Parameters.AddWithValue("@userid", userid);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@created", created);
            command.Prepare();
            command.ExecuteNonQuery();

            Console.WriteLine("Write to [POSTS] DB: " + userid + ", " + password + ", " + name + ", " + email + ", " + created);

             

            // CLOSE DB CONNECTION AND DISPOSE QUERY //
            command.Dispose(); 
            connection.Close();

        }

        // CHECK FOR USER //
        // Requires a [STRING USERiD] //
        public static bool checkDBUserExist(string userid) {
                        // New Database connection //
            MySqlConnection connection = getConnection();
            connection.Open();

            bool exist = false;

            string query = "SELECT * FROM posts ORDER BY created ASC";
            var command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string DBuserid = reader.GetString(1);

                if (DBuserid == userid)
                {
                    exist = true;
                }

            }

            // CLOSE DB READER, CONNECTION AND DISPOSE QUERY //
            reader.Close();
            command.Dispose();
            connection.Close();

            return exist;

        }
        /**************************************************/



        // ------------ //
        // READ FROM DB //
        // ------------ //
        /**************************************************/
        // GET TOPICS //
        // Returns [List<Topic>] with all entries //
        public static List<Topic> getTopics() {
            List<Topic> topics = new List<Topic>();

            // New Database connection //
            MySqlConnection connection = getConnection();
            connection.Open();

            string query = "SELECT * FROM topics ORDER BY created DESC";
            using var command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                // int _id, string _userid, string _topic, DateTime _created
                int id = Convert.ToInt32(reader.GetString(0));
                string userid = reader.GetString(1);
                string topic = reader.GetString(2);
                DateTime created = Convert.ToDateTime(reader.GetString(3));
                
                topics.Add(new Topic(id, userid, topic, created));

                //Console.WriteLine("GET [TOPIC]: " + id + "," + userid + ", " + topic + ", " + posts + ", " + created);
            }

            // CLOSE DB READER, CONNECTION AND DISPOSE QUERY //
            reader.Close();
            command.Dispose();
            connection.Close();

            return topics;            

        }

        // GET POSTS //
        // Requires a [STRING TOPIC] or ["ALL"] //
        // Returns [List<Post>] with all or input topic //
        public static List<Post> getPosts(string _topic) {
            List<Post> posts = new List<Post>();

            // New Database connection //
            MySqlConnection connection = getConnection();
            connection.Open();

            string query = "SELECT * FROM posts ORDER BY created ASC";
            using var command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                // int _id, string _userid, string _topic, string _post, DateTime _created
                int id = Convert.ToInt32(reader.GetString(0));
                string userid = reader.GetString(1);
                string topic = reader.GetString(2);
                string post = reader.GetString(3);
                DateTime created = Convert.ToDateTime(reader.GetString(4));
                
                if (topic == _topic)
                {
                    posts.Add(new Post(id, userid, topic, post, created));
                    //Console.WriteLine("GET [POST]: " + id + "," + userid + ", " + topic + ", " + post + ", " + created);
                }

                if (_topic == "ALL")
                {
                    posts.Add(new Post(id, userid, topic, post, created));
                    //Console.WriteLine("GET [POST]: " + id + "," + userid + ", " + topic + ", " + post + ", " + created);
                }
                

            }

            // CLOSE DB READER, CONNECTION AND DISPOSE QUERY //
            reader.Close();
            command.Dispose();
            connection.Close();

            return posts;            

        }

        public static DateTime getLatestPostTime(string _topic) {
            List<Post> posts = new List<Post>();

            // New Database connection //
            MySqlConnection connection = getConnection();
            connection.Open();

            string query = "SELECT * FROM posts";
            using var command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                // int _id, string _userid, string _topic, string _post, DateTime _created
                int id = Convert.ToInt32(reader.GetString(0));
                string userid = reader.GetString(1);
                string topic = reader.GetString(2);
                string post = reader.GetString(3);
                DateTime created = Convert.ToDateTime(reader.GetString(4));
                
                if (topic == _topic)
                {
                    posts.Add(new Post(id, userid, topic, post, created));
                    //Console.WriteLine("GET [POST]: " + id + "," + userid + ", " + topic + ", " + post + ", " + created);
                }
                

            }

            // CLOSE DB READER, CONNECTION AND DISPOSE QUERY //
            reader.Close();
            command.Dispose();
            connection.Close();

            DateTime latest = new DateTime();

            for (int i = 0; i < posts.Count; i++)
            {
                if (latest < posts[i].created){
                    latest = posts[i].created;
                }
                    
            }


            return latest;            

        }

        // GET USERS //
        // Requires a [STRING USERID] or ["ALL"] //
        // Returns [List<User>] with all or input USERID //
        public static List<User> getUsers(string _user) {
            List<User> users = new List<User>();

            // New Database connection //
            MySqlConnection connection = getConnection();
            connection.Open();

            string query = "SELECT * FROM users";
            using var command = new MySqlCommand(query, connection);

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                // int _id, string _usderId, string _password, string _name, string _email, DateTime _created
                int id = Convert.ToInt32(reader.GetString(0));
                string userid = reader.GetString(1);
                string password = reader.GetString(2);
                string name = reader.GetString(3);
                string email = reader.GetString(4);
                DateTime created = Convert.ToDateTime(reader.GetString(5));
                
                if (userid == _user)
                {
                    users.Add(new User(id, userid, password, name, email, created));

                    //Console.WriteLine("GET [USER]: " + id + "," + userid + ", " + password + ", " + name + ", " + email + ", " + created);
                }

                if (_user == "ALL")
                {
                    users.Add(new User(id, userid, password, name, email, created));

                    //Console.WriteLine("GET [USER]: " + id + "," + userid + ", " + password + ", " + name + ", " + email + ", " + created);
                }



                

            }

            Console.WriteLine("[USER]" + users[0].name);

            // CLOSE DB READER, CONNECTION AND DISPOSE QUERY //
            reader.Close();
            command.Dispose();
            connection.Close();

            return users;            

        }
        /**************************************************/



        //------ //
        // LOGIN //
        // ----- //
        /**************************************************/
        // LOGIN //
        // Requires a valid [STRING USERID] and [STRING PASSWORD] //
        // Returns [BOOL] //
        public static bool login(string userid, string password){
            bool accepted = false;

            // New Database connection //
            MySqlConnection connection = getConnection();
            connection.Open();

            string query = "select * from users where userid = '" + userid + "' AND password = '" + password + "'";
            var command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read()){
                Console.WriteLine("Successfully Sign In!");
                accepted = true;
            }
            else{
                Console.WriteLine("Username And Password Not Match!");
                accepted = false;
            }

            reader.Close();
            command.Dispose();
            connection.Close();

            return accepted;
        }

        // LOGOUT //
        // Requires a valid [STRING USERID] and [STRING PASSWORD] //
        // Returns [BOOL] //
        public static void logout(){
            CurrUser = "";
            loggedIn = false;

        }
        /**************************************************/



    }


}