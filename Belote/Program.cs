using System;
using System.Collections.Generic;
using System.Linq;

namespace Belote
{
    class Program
    {
        public static List<Player> Players { get; set; }
        public const int PointsMaximum = 142;
        public static int PointsHorsJeu { get; set; }
        public static int Meneur { get; set; }
        public static int Allie { get; set; }
        public static int PlayersNumber { get; set; }
        public static bool IsAnnonce { get; set; }

        public class Player
        {
            public Player(string name)
            {
                Name = name;
                Points = new Dictionary<int, int>();
            }

            public string Name { get; set; }
            public int PointsDixDeDer { get; set; }
            public int PointsAnnonce { get; set; }
            public int PointsBeloteRebelote { get; set; }

            public Dictionary<int, int> Points { get; set; }
        }

        static void Main(string[] args)
        {
            InitValues(true);
            GetPlayers();
            StartGame();
        }

        private static void InitValues(bool initPlayersList)
        {
            if (initPlayersList)
                Players = new List<Player>();
            PointsHorsJeu = 0;
            Meneur = -1;
            Allie = -1;
            IsAnnonce = false;
        }

        private static void GetPlayers()
        {
            Console.WriteLine("How many players will there be ? (3 or 4)");
            PlayersNumber = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine($"There are {PlayersNumber} players");

            Console.WriteLine();
            Console.WriteLine("Enter players names");
            for (var i = 0; i < PlayersNumber; i++)
            {
                Console.Write($"Player {i + 1} = ");
                Players.Add(new Player(Console.ReadLine()));
            }
            Console.WriteLine();
        }

        private static void SelectionQuiPrend()
        {
            //Selection de qui prends
            Console.WriteLine("Qui prends ?");
            ShowPlayersNames(false);
            var preneur = Convert.ToInt32(Console.ReadLine()) - 1;
            Console.WriteLine($"{Players[preneur].Name} prends");
            Meneur = preneur;

            //add allie si jeu a 4 personnes
            if (Players[0].PlayersNumber == 4)
            {
                preneur += 2;
                if (preneur >= 4)
                    preneur -= 4;
                Allie = preneur;
                Console.WriteLine($"{Players[Meneur].Name} joue avec {Players[Allie].Name}");
            }
            Console.WriteLine();
        }

        private static void SelectionPointsHorsJeu()
        {
            Console.WriteLine("Combien de points sont hors jeu ?");
            PointsHorsJeu = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine($"{PointsHorsJeu} points sont hors-jeu");
            Console.WriteLine();
        }

        private static void SelectionAnnonces()
        {
            Console.WriteLine("Un joueur as-t-il fait une annonce ?");
            ShowPointsAnnonce();
            ShowTeamNames(true);
            var teamAnnonce = Convert.ToInt32(Console.ReadLine());
            if (teamAnnonce != 0)
            {
                IsAnnonce = true;
                Console.WriteLine("Combien de points ?");
                ShowPointsAnnonce();
                AddPointsAnnonceToPlayers(teamAnnonce, Convert.ToInt32(Console.ReadLine()));
            }
            else
                Console.WriteLine("Aucune annonce n'a ete faite");
            Console.WriteLine();
        }

        public static void AddPointsAnnonceToPlayers(int teamAnnonce, int pointsAnnonce)
        {
            if (teamAnnonce == 1)
            {
                Players[Meneur].PointsAnnonce = pointsAnnonce;
                Console.WriteLine($"{Players[Meneur].Name} annonce {pointsAnnonce} points");
                if (Allie != -1)
                {
                    Players[Allie].PointsAnnonce = pointsAnnonce;
                    Console.WriteLine($"{Players[Allie].Name} annonce {pointsAnnonce} points");
                }
            }
            else
            {
                for (var i = 0; i < PlayersNumber; i++)
                {
                    if (i != Meneur && i != Allie)
                    {
                        Players[i].PointsAnnonce = pointsAnnonce;
                        Console.WriteLine($"{Players[i].Name} annonce {pointsAnnonce} points");
                    }
                }
            }
        }

        public static void SelectionDixDeDer()
        {
            Console.WriteLine("Qui a eu le dix de der ?");
            ShowTeamNames(false);
            AddDixDeDer(Convert.ToInt32(Console.ReadLine()));
            Console.WriteLine();
        }

        public static void AddDixDeDer(int teamAnnonce)
        {
            if (teamAnnonce == 1)
            {
                Players[Meneur].PointsDixDeDer = 10;
                Console.WriteLine($"{Players[Meneur].Name} as le dix de der");
                if (Allie != -1)
                {
                    Players[Allie].PointsDixDeDer = 10;
                    Console.WriteLine($"{Players[Allie].Name} as le dix de der");
                }
            }
            else
            {
                for (var i = 0; i < PlayersNumber; i++)
                {
                    if (i != Meneur && i != Allie)
                    {
                        Players[i].PointsDixDeDer = 10;
                        Console.WriteLine($"{Players[i].Name} as le dix de der");
                    }
                }
            }
        }

        public static void SelectionBeloteRebelote()
        {
            if (IsAnnonce)
            {
                Console.WriteLine("Qui a eu un belote rebelote ?");
                ShowTeamNames(true);
                var teamBeloteRebelote = Convert.ToInt32(Console.ReadLine());
                if (teamBeloteRebelote != 0)
                    AddBeloteRebeloteToTeam(teamBeloteRebelote);
                Console.WriteLine();
            }
        }

        public static void AddBeloteRebeloteToTeam(int team)
        {
            if (team == 1)
            {
                Players[Meneur].PointsBeloteRebelote = 20;
                Console.WriteLine($"{Players[Meneur].Name} have belote rebelote");
                if (Allie != -1)
                {
                    Players[Allie].PointsBeloteRebelote = 20;
                    Console.WriteLine($"{Players[Allie].Name} have belote rebelote");
                }
            }
            else
            {
                for (var i = 0; i < PlayersNumber; i++)
                {
                    if (i != Meneur && i != Allie)
                    {
                        Players[i].PointsBeloteRebelote = 20;
                        Console.WriteLine($"{Players[i].Name} have belote rebelote");
                    }
                }
            }
        }

        public static void SelectionPointsPreneur()
        {
            Console.WriteLine("Combien de points a fait le preneur ? (sans dix de der)");
            CalculatePoints(Convert.ToInt32(Console.ReadLine()));
            Console.WriteLine();
        }

        public static void CalculatePoints(int totalPointsMeneur)
        {
            // Si preneur gagne
            if (totalPointsMeneur + Players[Meneur].PointsDixDeDer > (PointsMaximum - PointsHorsJeu) / 2)
            {
                Players[Meneur].Points.Add(totalPointsMeneur, Players[Meneur].PointsDixDeDer + Players[Meneur].PointsBeloteRebelote + Players[Meneur].PointsAnnonce);
                Console.WriteLine($"{Players[Meneur].Name} add {totalPointsMeneur} points and {Players[Meneur].PointsDixDeDer + Players[Meneur].PointsBeloteRebelote + Players[Meneur].PointsAnnonce} bonus");
                if (Allie != -1)
                {
                    Players[Allie].Points.Add(totalPointsMeneur, Players[Allie].PointsDixDeDer + Players[Allie].PointsBeloteRebelote + Players[Allie].PointsAnnonce);
                    Console.WriteLine($"{Players[Allie].Name} add {totalPointsMeneur} points and {Players[Allie].PointsDixDeDer + Players[Allie].PointsBeloteRebelote + Players[Allie].PointsAnnonce} bonus");
                }
                for (var i = 0; i < PlayersNumber; i++)
                {
                    if (i != Meneur && i != Allie)
                    {
                        Players[i].Points.Add(PointsMaximum - PointsHorsJeu - totalPointsMeneur, Players[i].PointsDixDeDer + Players[i].PointsBeloteRebelote + Players[i].PointsAnnonce);
                        Console.WriteLine($"{Players[i].Name} add {PointsMaximum - PointsHorsJeu - totalPointsMeneur} points and {Players[i].PointsDixDeDer + Players[i].PointsBeloteRebelote + Players[i].PointsAnnonce} bonus");
                    }
                }
            }
            // Si preneur perd
            else
            {
                for (var i = 0; i < PlayersNumber; i++)
                {
                    if (i != Meneur && i != Allie)
                    {
                        Players[i].Points.Add(PointsMaximum - PointsHorsJeu, Players[i].PointsDixDeDer + Players[i].PointsBeloteRebelote + Players[i].PointsAnnonce);
                        Console.WriteLine($"{Players[i].Name} add {PointsMaximum - PointsHorsJeu} points and {Players[i].PointsDixDeDer + Players[i].PointsBeloteRebelote + Players[i].PointsAnnonce} bonus");
                    }
                }
                Players[Meneur].Points.Add(0, Players[Meneur].PointsDixDeDer + Players[Meneur].PointsBeloteRebelote + Players[Meneur].PointsAnnonce);
                Console.WriteLine($"{Players[Meneur].Name} add 0 points and {Players[Meneur].PointsDixDeDer + Players[Meneur].PointsBeloteRebelote + Players[Meneur].PointsAnnonce} bonus");
                if (Allie != -1)
                {
                    Players[Allie].Points.Add(0, Players[Allie].PointsDixDeDer + Players[Allie].PointsBeloteRebelote + Players[Allie].PointsAnnonce);
                    Console.WriteLine($"{Players[Allie].Name} add 0 points and {Players[Allie].PointsDixDeDer + Players[Allie].PointsBeloteRebelote + Players[Allie].PointsAnnonce} bonus");
                }
            }
        }

        private static void StartGame()
        {
            while (true)
            {
                SelectionQuiPrend();
                SelectionPointsHorsJeu();
                SelectionAnnonces();
                SelectionDixDeDer();
                SelectionBeloteRebelote();
                SelectionPointsPreneur();
                ShowTotalPoints();

                InitValues(false);
            }
        }

        

        private static void ShowPlayersNames(bool isOptional)
        {
            Console.WriteLine($"1 = {Players[0].Name}\n2 = {Players[1].Name}\n3 = {Players[2].Name}");
            if (isOptional)
                Console.WriteLine("0 = non");
        }

        public static void ShowTotalPoints()
        {
            Console.WriteLine();
            for (var i = 0; i < PlayersNumber; i ++)
            {
                Players[i].PointsAnnonce = 0;
                Players[i].PointsBeloteRebelote = 0;
                Players[i].PointsDixDeDer = 0;
                var totalPlayer = 0;
                foreach (var points in Players[i].Points)
                {
                    totalPlayer += points.Value + points.Key;
                }
                Console.WriteLine($"{Players[i].Name} have {totalPlayer} points");
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

        private static void ShowPointsAnnonce()
        {
            Console.WriteLine("20 = Suite de 3\n" +
                              "50 = Suite de 4\n" +
                              "100 = Suite de 5\n" +
                              "100 = Carre de Rois, Dames, As ou 10\n" +
                              "150 = Carre de 9\n" +
                              "200 = Carre de Valets");
        }

        private static void ShowTeamNames(bool isOptional)
        {
            Console.WriteLine("1 = Preneur\n2 = Non-preneurs");
            if (isOptional)
                Console.WriteLine("0 = non");
        }
    }
}
