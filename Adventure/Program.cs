namespace Adventure
{
    class Program
    {
        public bool isAlive = true;
        static void Main()
        {
            Character Player = Encounters.Tutorial();
            Character Cave_Players = Encounters.Cave(Player);
        }
        public static void GameOver(int reason=0)
        {
            switch (reason)
            {
                case 0:
                    Console.WriteLine("Game Over, you have died.");
                    Environment.Exit(0);
                    break;
                case 1:
                    Console.WriteLine("Congratulations, you've slain the dragon and beaten the game.");
                    Environment.Exit(0);
                    break;
            }
        }
    }
    class Character
    {
        public int Health;
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
        internal void Check_stats()
        {
            Console.WriteLine("Health: " + this.Health + "/" + this.HealthMax + " Strength: " + this.Strength + " Speed: " + this.Speed);
        }
        internal void Add_Inventory(string item, int value)
        {
            this.Inventory.AddToInv(item, value);
        }
        internal void Check_Inventory()
        {
            bool emptyInv = this.Inventory.Check_Inventory();
            if (emptyInv)
            {
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
        bool Attack(Character opponent) //returns if opponent is still alive
        {
            Console.WriteLine(this.Name + " attacks " + opponent.Name + " dealing " + this.Strength + " damage.");
            opponent.Health -= this.Strength; //deals damage
            if (opponent.Health > 0) //checks if opponent dead
            {
                return true;
            }
            Console.WriteLine(opponent.Name + " has succumbed to its injuries.");
            opponent.Health = 0;
            return false;
        }
        internal bool Fight(Character opponent)
        {
            bool both_alive = true;
            bool Self_First; //decides who should attack this or opponent
            if (this.Speed == opponent.Speed) //chooses random start
            {
                Random random = new();
                if (random.Next(0, 1) == 0)
                {
                    Self_First = true;
                }
                else
                {
                    Self_First = false;
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
            while (both_alive) //opponent attacks first because it's faster
            {
                if (Self_First)
                {
                    both_alive = Attack(opponent);
                    Self_First = false;
                }
                else
                {
                    both_alive = opponent.Attack(this);
                    Self_First = true;
                }

            }
            if (this.Health == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    class Inventory : Dictionary<string, int>//keeps track of a Characters items.
    {
        internal Inventory AddToInv(string name, int quantity)
        {
            if (this.ContainsKey(name)) //if item already in inventory, increase supply
            {
                this[name] += quantity;
            }
            else
            {
                this.Add(name, quantity);
            }
            return this;
        }
        internal bool Check_Inventory()
        {
            if (this.Count == 0)
            {
                return true;
            }
            foreach (KeyValuePair<string, int> item in this)
            {
                Console.WriteLine("Item: " + item.Key + " Quantity: " + item.Value);
            }
            return false;
        }
    }
    class Encounters
    {
        public static Character Tutorial()
        {
            Console.WriteLine("What is your name");
            Character Player = new(new int[] { 3, 3, 4 }, Console.ReadLine(), new Inventory());
            Character Spider = new(new int[] { 1, 1, 1 }, "Spider", new Inventory());
            Player.Fight(Spider);
            Player.Check_stats();
            Player.Level_Up(1, 1, 1);
            Player.Check_stats();
            return Player;
        }
        public static Character Cave(Character input)
        {
            Character Player = input; //Player character
            Console.WriteLine("You enter a dark cave");
            Console.WriteLine("There's a sleeping dragon to your right, and a door to your left which would you like to approach?");
            string response = Console.ReadLine().ToLower();
            if (response.Contains("dragon"))
            {
                Encounters.Dragon(Player);
            }
            else if (response.Contains("door"))
            {
                Encounters.Door(Player);
            }
            else
            {
                Console.WriteLine("I'm sorry, I don't understand");
                Cave(input);
            }
            return Player;
        }
        public static Character Dragon(Character input)
        {
            Character Player = input;
            Console.WriteLine("You approach the dragon and it raises its head and prepares to attack.");
            Character dragon = new(new int[] { 5, 5, 3 }, "Dragon", new Inventory());
            dragon.Check_stats();
            Player.Fight(dragon);
            Player.Check_stats();
            if (Player.Health == 0)
            {
                Program.GameOver(); //defeat
            }
            if (dragon.Health == 0)
            {
                Program.GameOver(1); //game won
            }
            return Player;
        }
        public static Character Door(Character input)
        {
            Character Player = input;
            Console.WriteLine("You find a treasure chest with a sword inside, do you take it?");
            string response = Console.ReadLine().ToLower();
            if (response.Contains('y'))
            {
                Player.Level_Up(0, 3, -1);
                Player.Check_stats();
            }
            Encounters.Dragon(Player);
            return Player;
        }
    }
}