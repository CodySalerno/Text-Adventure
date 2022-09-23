namespace Adventure
{
    class Program
    {
        static void Main()
        {
            Encounters.Tutorial();

        }
    }
    class Character {
        int Health;
        int HealthMax;
        int Strength;
        int Speed;
        readonly string Name;
        Inventory Inventory;


        public Character(int[] stats, string name, Inventory inventory)
        {
            this.Health = stats[0];
            this.HealthMax = this.Health;
            this.Strength = stats[1];
            this.Speed = stats[2];
            this.Name = name;
            this.Inventory = inventory;
        }
        void Level_Up(int health_gain, int strength_gain, int speed_gain)
        {
            this.HealthMax += health_gain;
            this.Strength += strength_gain;
            this.Speed += speed_gain;
        }
        void HealMax()
        {
            this.Health = this.HealthMax;
        }
        bool Attack(Character opponent)
        {
            opponent.Health -= this.Strength;

            if (opponent.Health <= 0)
            {
                opponent.Health = 0;
                return true;
            }
            return false;
        }
        internal bool Fight(Character opponent)
        {
            bool Self_First;
            if (this.Speed == opponent.Speed) //chooses random start
            {
                Random random = new();
                if (random.Next(0, 1) == 0)
                {

                    Self_First = true;
                }
                else
                {
                    Self_First= false;
                }
            }
            else if (this.Speed > opponent.Speed) //this attacks first because it's faster
            {
                Self_First = true;
            }
            else
            {
                Self_First = false;
            }
            while (this.Health > 0 && opponent.Health > 0) //opponent attacks first because it's faster
            {
               if (Self_First)
                {
                    Attack(opponent);
                    Self_First = false;
                }
               else
                {
                    opponent.Attack(this);
                    Self_First = true;
                }
                if (this.Health == 0) return false;
                else return true;
                
            }
        }


    }

    class Inventory //keeps track of a Characters items.
    {
        public static readonly Inventory emptyInv = new();
        readonly Dictionary<string, int> Items;
        public Inventory() //constructor
        {
            Items = new Dictionary<string, int>();
        }
        public Dictionary<string, int> Add(string name, int quantity)
        {
            if (Items.ContainsKey(name)) //if item already in inventory, increase supply
            {
                Items[name] += quantity;
            }
            else
            {
                Items.Add(name, quantity);
            }
            return Items;
        }
    }
    class Encounters 
    {
        public static void Tutorial()
        {
            Console.WriteLine("What is your name");
            string player_name = Console.ReadLine();
            int[] player_stats = {5, 5, 5};
            Inventory player_inventory = new();
            Character Player = new(player_stats, player_name, player_inventory);
            string spider_name = "Spider";
            int[] spider_stats = {3,3,3};
            Inventory spider_inv = Inventory.emptyInv;
            Character Spider = new(spider_stats, spider_name, spider_inv);
            Player.Fight(Spider);


        }
    }
}