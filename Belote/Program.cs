using System;
using System.Collections.Generic;
using System.Linq;

namespace Belote
{
    class Program
    {
        public const int PointsMaximum = 142;
        public static List<Player> Players { get; set; }
        public static int PointsHorsJeu { get; set; }
        public static int MeneurId { get; set; }
        public static int AllieId { get; set; }
        public static int PlayersNumber { get; set; }
        public static bool IsAnnonce { get; set; }

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
            MeneurId = -1;
            AllieId = -1;
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
            MeneurId = preneur;

            //add allie si jeu a 4 personnes
            if (PlayersNumber == 4)
            {
                preneur += 2;
                if (preneur >= 4)
                    preneur -= 4;
                AllieId = preneur;
                Console.WriteLine($"{Players[MeneurId].Name} joue avec {Players[AllieId].Name}");
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
                Players[MeneurId].PointsAnnonce = pointsAnnonce;
                Console.WriteLine($"{Players[MeneurId].Name} annonce {pointsAnnonce} points");
                if (AllieId != -1)
                {
                    Players[AllieId].PointsAnnonce = pointsAnnonce;
                    Console.WriteLine($"{Players[AllieId].Name} annonce {pointsAnnonce} points");
                }
            }
            else
            {
                for (var i = 0; i < PlayersNumber; i++)
                {
                    if (i != MeneurId && i != AllieId)
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
                Players[MeneurId].PointsDixDeDer = 10;
                Console.WriteLine($"{Players[MeneurId].Name} as le dix de der");
                if (AllieId != -1)
                {
                    Players[AllieId].PointsDixDeDer = 10;
                    Console.WriteLine($"{Players[AllieId].Name} as le dix de der");
                }
            }
            else
            {
                for (var i = 0; i < PlayersNumber; i++)
                {
                    if (i != MeneurId && i != AllieId)
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
                Players[MeneurId].PointsBeloteRebelote = 20;
                Console.WriteLine($"{Players[MeneurId].Name} have belote rebelote");
                if (AllieId != -1)
                {
                    Players[AllieId].PointsBeloteRebelote = 20;
                    Console.WriteLine($"{Players[AllieId].Name} have belote rebelote");
                }
            }
            else
            {
                for (var i = 0; i < PlayersNumber; i++)
                {
                    if (i != MeneurId && i != AllieId)
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
            if (totalPointsMeneur + Players[MeneurId].PointsDixDeDer > (PointsMaximum - PointsHorsJeu) / 2)
            {
                Players[MeneurId].Points.Add(totalPointsMeneur, Players[MeneurId].PointsDixDeDer + Players[MeneurId].PointsBeloteRebelote + Players[MeneurId].PointsAnnonce);
                Console.WriteLine($"{Players[MeneurId].Name} add {totalPointsMeneur} points and {Players[MeneurId].PointsDixDeDer + Players[MeneurId].PointsBeloteRebelote + Players[MeneurId].PointsAnnonce} bonus");
                if (AllieId != -1)
                {
                    Players[AllieId].Points.Add(totalPointsMeneur, Players[AllieId].PointsDixDeDer + Players[AllieId].PointsBeloteRebelote + Players[AllieId].PointsAnnonce);
                    Console.WriteLine($"{Players[AllieId].Name} add {totalPointsMeneur} points and {Players[AllieId].PointsDixDeDer + Players[AllieId].PointsBeloteRebelote + Players[AllieId].PointsAnnonce} bonus");
                }
                for (var i = 0; i < PlayersNumber; i++)
                {
                    if (i != MeneurId && i != AllieId)
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
                    if (i != MeneurId && i != AllieId)
                    {
                        Players[i].Points.Add(PointsMaximum - PointsHorsJeu, Players[i].PointsDixDeDer + Players[i].PointsBeloteRebelote + Players[i].PointsAnnonce);
                        Console.WriteLine($"{Players[i].Name} add {PointsMaximum - PointsHorsJeu} points and {Players[i].PointsDixDeDer + Players[i].PointsBeloteRebelote + Players[i].PointsAnnonce} bonus");
                    }
                }
                Players[MeneurId].Points.Add(0, Players[MeneurId].PointsDixDeDer + Players[MeneurId].PointsBeloteRebelote + Players[MeneurId].PointsAnnonce);
                Console.WriteLine($"{Players[MeneurId].Name} add 0 points and {Players[MeneurId].PointsDixDeDer + Players[MeneurId].PointsBeloteRebelote + Players[MeneurId].PointsAnnonce} bonus");
                if (AllieId != -1)
                {
                    Players[AllieId].Points.Add(0, Players[AllieId].PointsDixDeDer + Players[AllieId].PointsBeloteRebelote + Players[AllieId].PointsAnnonce);
                    Console.WriteLine($"{Players[AllieId].Name} add 0 points and {Players[AllieId].PointsDixDeDer + Players[AllieId].PointsBeloteRebelote + Players[AllieId].PointsAnnonce} bonus");
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
