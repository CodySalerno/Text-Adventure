using System.ComponentModel.DataAnnotations;
namespace Adventure
{
    class Program
    {
        static void Main()
        {
            Character[] Tutorial_Players = Encounters.Tutorial();
            Tutorial_Players[0].Check_Inventory();
        }
    }
    class Character {
        int Health;
        int HealthMax;
        int Strength;
        int Speed;
        readonly string? Name;
        Inventory Inventory;
        public Character(int[] stats, string? name, Inventory inventory)
        {
            this.Health = stats[0];
            this.HealthMax = this.Health;
            this.Strength = stats[1];
            this.Speed = stats[2];
            this.Name = name;
            this.Inventory = inventory;
        }
        internal void Check_Inventory()
        {
            bool emptyInv = this.Inventory.Check_Inventory();
            if (!emptyInv) {
                Console.WriteLine(this.Name + " has an empty inventory");
            }
        }
        internal void Level_Up(int health_gain, int strength_gain, int speed_gain)
        {
            this.HealthMax += health_gain;
            this.Strength += strength_gain;
            this.Speed += speed_gain;
            this.HealMax();
        }
        internal void HealMax()
        {
            this.Health = this.HealthMax;
        }
        bool Attack(Character opponent)
        {
            Console.WriteLine(this.Name + " attacks " + opponent.Name + " dealing " + this.Strength + " damage.");
            opponent.Health -= this.Strength;

            if (opponent.Health <= 0)
            {
                opponent.Health = 0;
                Console.WriteLine(opponent.Name + " has succumbed to its injuries.");
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
            return true; //shouldn't ever trigger but if it does means both people had 0 hp at the start of the fight
        }
    }
    class Inventory //keeps track of a Characters items.
    {
        //public static readonly Inventory emptyInv = new();
        Dictionary<string, int> Items;
        public Inventory() //constructor
        {
            this.Items = new Dictionary<string, int>();
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
        internal bool Check_Inventory()
        {
            if (this.Items.Count == 0)
            {
                return true;
            }
            string display = "";
            foreach (KeyValuePair<string, int> item in this.Items)
            {
                display += ("Item: " + item.Key + " Quantity: " + item.Value + "\n");
            }
            Console.WriteLine(display);
            return false;
        }
    }
    class Encounters 
    {
        public static Character[] Tutorial()
        {
            Console.WriteLine("What is your name");
            Character Player = new(new int[] {5,5,5}, Console.ReadLine(), new Inventory());
            Character Spider = new(new int[] {3,3,3}, "Spider", new Inventory());
            Player.Fight(Spider);
            Player.Level_Up(1,1,1);
            return new Character[] {Player, Spider};
        }
    }
}