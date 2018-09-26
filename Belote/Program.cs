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
            Players = new List<Player>();
            GetPlayers();
            StartGame();
        }

        private static void StartGame()
        {
            while (true)
            {
                // Selection de qui prend
                Console.WriteLine("\nQui prends ?");
                ShowPlayersNames(false);
                var preneur = Convert.ToInt32(Console.ReadLine()) - 1;
                Console.WriteLine($"{Players[preneur].Name} prends");

                // Selection des points hors-jeu
                Console.WriteLine("\nCombien de points sont hors jeu ?");
                PointsHorsJeu = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine($"{PointsHorsJeu} points sont hors-jeu");

                // Selection des Annonces
                Console.WriteLine("\nUn joueur as-t-il fait une annonce ?");
                ShowTeamNames(true);
                var teamAnnonce = Convert.ToInt32(Console.ReadLine());
                if (teamAnnonce != 0)
                {
                    Console.WriteLine("Combien de points ?");
                    ShowPointsAnnonce();
                    var pointsAnnonce = Convert.ToInt32(Console.ReadLine());
                    AddPointsAnnonceToPlayers(teamAnnonce, pointsAnnonce, preneur);
                }

                // Dix de der
                Console.WriteLine("\nQui a eu le dix de der ?");
                ShowTeamNames(false);
                AddDixDeDer(Convert.ToInt32(Console.ReadLine()), preneur);

                // Belote Rebelote
                Console.WriteLine("\nQui a eu un belote rebelote ?");
                ShowTeamNames(true);
                var teamBeloteRebelote = Convert.ToInt32(Console.ReadLine());
                if (teamBeloteRebelote != 0)
                    AddBeloteRebeloteToTeam(teamBeloteRebelote, preneur);

                // Points preneur
                Console.WriteLine("\nCombien de points a fait le preneur ? (sans dix de der)");
                var totalPointsPreneur = Convert.ToInt32(Console.ReadLine());
                CalculatePoints(totalPointsPreneur, preneur);

                //Show Total Points
                ShowTotalPoints();
            }
        }

        public static void ShowTotalPoints()
        {
            Console.WriteLine();
            for (var i = 0; i < 3; i ++)
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

        public static void CalculatePoints(int totalPointsPreneur, int preneur)
        {
            // Si preneur gagne
            if (totalPointsPreneur + Players[preneur].PointsDixDeDer > (PointsMaximum - PointsHorsJeu) / 2)
            {
                Players[preneur].Points.Add(totalPointsPreneur, Players[preneur].PointsDixDeDer + Players[preneur].PointsBeloteRebelote + Players[preneur].PointsAnnonce);
                Console.WriteLine($"{Players[preneur].Name} add {totalPointsPreneur} points and {Players[preneur].PointsDixDeDer + Players[preneur].PointsBeloteRebelote + Players[preneur].PointsAnnonce} bonus");
                for (var i = 0; i < 3; i++)
                {
                    if (i != preneur)
                    {
                        Players[i].Points.Add(PointsMaximum - PointsHorsJeu - totalPointsPreneur, Players[i].PointsDixDeDer + Players[i].PointsBeloteRebelote + Players[i].PointsAnnonce);
                        Console.WriteLine($"{Players[i].Name} add {PointsMaximum - PointsHorsJeu - totalPointsPreneur} points and {Players[i].PointsDixDeDer + Players[i].PointsBeloteRebelote + Players[i].PointsAnnonce} bonus");
                    }
                }
            }
            // Si preneur perd
            else
            {
                for (var i = 0; i < 3; i++)
                {
                    if (i != preneur)
                    {
                        Players[i].Points.Add(PointsMaximum - PointsHorsJeu, Players[i].PointsDixDeDer + Players[i].PointsBeloteRebelote + Players[i].PointsAnnonce);
                        Console.WriteLine($"{Players[i].Name} add {PointsMaximum - PointsHorsJeu} points and {Players[i].PointsDixDeDer + Players[i].PointsBeloteRebelote + Players[i].PointsAnnonce} bonus");
                    }
                }
                Players[preneur].Points.Add(0, Players[preneur].PointsDixDeDer + Players[preneur].PointsBeloteRebelote + Players[preneur].PointsAnnonce);
                Console.WriteLine($"{Players[preneur].Name} add 0 points and {Players[preneur].PointsDixDeDer + Players[preneur].PointsBeloteRebelote + Players[preneur].PointsAnnonce} bonus");
            }
        }

        public static void AddBeloteRebeloteToTeam(int team, int preneur)
        {
            if (team == 1)
            {
                Players[preneur].PointsBeloteRebelote = 20;
                Console.WriteLine($"{Players[preneur].Name} have belote rebelote");
            }
            else
            {
                for (var i = 0; i < 3; i++)
                {
                    if (i != preneur)
                    {
                        Players[i].PointsBeloteRebelote = 20;
                        Console.WriteLine($"{Players[i].Name} have belote rebelote");
                    }
                }
            }
        }

        public static void AddDixDeDer(int teamAnnonce, int preneur)
        {
            if (teamAnnonce == 1)
                Players[preneur].PointsDixDeDer = 10;
            else
            {
                for (var i = 0; i < 3; i++)
                {
                    if (i != preneur)
                    {
                        Players[i].PointsDixDeDer = 10;
                        Console.WriteLine($"{Players[i].Name} as le dix de der");
                    }
                }
            }
        }

        public static void AddPointsAnnonceToPlayers(int teamAnnonce, int pointsAnnonce, int preneur)
        {
            if (teamAnnonce == 1)
                Players[preneur].PointsAnnonce = pointsAnnonce;
            else
            {
                for (var i = 0; i < 3; i++)
                {
                    if (i != preneur)
                    {
                        Players[i].PointsAnnonce = pointsAnnonce;
                        Console.WriteLine($"{Players[i].Name} annonce {pointsAnnonce} points");
                    }
                }
            }
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

        private static void ShowPlayersNames(bool isOptional)
        {
            Console.WriteLine($"1 = {Players[0].Name}\n2 = {Players[1].Name}\n3 = {Players[2].Name}");
            if (isOptional)
                Console.WriteLine("0 = non");
        }

        private static void GetPlayers()
        {
            Console.WriteLine("Enter players names.");
            Console.Write("Player 1 = "); Players.Add(new Player(Console.ReadLine()));
            Console.Write("Player 2 = "); Players.Add(new Player(Console.ReadLine()));
            Console.Write("Player 3 = "); Players.Add(new Player(Console.ReadLine()));
        }
    }
}
