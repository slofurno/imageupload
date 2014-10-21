using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace imageupload
{
    public class UserController : ApiController
    {
        public IEnumerable<Object> Get()
        {

            return InitiativeController.userlist.Select(x => new { id = x.userId, name = x.userName });

        }
    }

    public class TopicController : ApiController
    {

        public IEnumerable<Object> Get()
        {
            return InitiativeController.topiclist.Select(x => new { id = x.topicId, name = x.topicName });

        }
    }

    public class AccountController : ApiController
    {
        public IEnumerable<Object> Get()
        {

            return InitiativeController.accountlist.Select(x => new { id = x.accountId, name = x.accountName });

        }

       



    }

    public class InitiativeController : ApiController
    {

        public static List<Account> accountlist;

        public static List<User> userlist;

        public static List<Initiative> initiativelist;

        public static List<Topic> topiclist;


        static InitiativeController()
        {

          Random random = new Random();


            accountlist = new List<Account>
            {

                new Account("Bayer Schering Pharma"),
                new Account("Roche Applied Science"),
                new Account("Merz Pharma"),
                new Account("Merck KGaA")

            };

            userlist = new List<User>
            {

                new User("François Englert", accountlist.FirstOrDefault(x=>x.accountId == random.Next(accountlist.Count-1)) ),


                new User("Isamu Akasaki", accountlist.FirstOrDefault(x=>x.accountId == random.Next(accountlist.Count-1))),
                new User("Serge Haroche", accountlist.FirstOrDefault(x=>x.accountId == random.Next(accountlist.Count-1))),
                new User("Saul Perlmutter", accountlist.FirstOrDefault(x=>x.accountId == random.Next(accountlist.Count-1))),
                new User("Andre Geim", accountlist.FirstOrDefault(x=>x.accountId == random.Next(accountlist.Count-1))),
                new User("Charles Kuen Kao", accountlist.FirstOrDefault(x=>x.accountId == random.Next(accountlist.Count-1))),
                new User("Willard S. Boyle", accountlist.FirstOrDefault(x=>x.accountId == random.Next(accountlist.Count-1)))


            };

            initiativelist = new List<Initiative>{

                new Initiative("init 1"),
                new Initiative("init 2"),
                new Initiative("init 3"),
                new Initiative("init 4"),
                new Initiative("init 5"),
                new Initiative("init 6"),
                new Initiative("init 7"),
                new Initiative("init 8"),
                new Initiative("init 9"),
                new Initiative("init 10"),
                new Initiative("init 11"),

            };



            for (int i = 0; i < initiativelist.Count; i++)
            {

                initiativelist[i].initiativeAccount = accountlist[random.Next(accountlist.Count - 1)];

                initiativelist[i].users = userlist.Where(x => x.userAccount == initiativelist[i].initiativeAccount).ToList();


            }

                


            topiclist = new List<Topic>
            {

                new Topic("topic 1"),
                new Topic("topic 2"),
                new Topic("topic 3"),
                new Topic("topic 4"),
                new Topic("topic 5"),
                new Topic("topic 6"),
                new Topic("topic 7"),
                new Topic("topic 8"),

            };


            List<Topic> templist;

            for (int i = 0; i < initiativelist.Count; i++)
            {


                templist = new List<Topic>(topiclist);

                int r = random.Next(1,topiclist.Count);

                

                for (int j = 0; j < r; j++)
                {

                    var index = random.Next(templist.Count - 1);

                    initiativelist[i].topics.Add(templist[index]);
                    templist.RemoveAt(index);



                }



            }




        }







        
        // GET api/<controller>
        public IEnumerable<Initiative> Get()
        {
            return initiativelist.ToArray();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }

    public class Initiative
    {

        static int nextid = 0;
        public int initiativeId;
        public string initiativeName;
        public List<User> users;
        public List<Topic> topics;
        public Account initiativeAccount;

        public Initiative(string name)
        {
            this.initiativeId = nextid++;
            this.initiativeName = name;
            this.topics = new List<Topic>();
        }

    }

    public class User
    {

        static int nextid = 0;
        public int userId;
        public string userName;
        public Account userAccount;

        public User(string name, Account account)
        {

            this.userId = nextid++;
            this.userName = name;
            this.userAccount = account;


        }

    }

    public class Account
    {
        static int nextid = 0;
        public int accountId;
        public string accountName;

        public Account(string name)
        {
            this.accountId = nextid++;
            this.accountName = name;

        }

    }

    public class Topic
    {
        static int nextid = 0;
        public int topicId;
        public string topicName;

        public Topic(string name)
        {
            this.topicId = nextid++;
            this.topicName = name;



        }

    }

}