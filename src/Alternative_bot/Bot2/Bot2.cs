using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Bot2 : Bot
{
    static void Main(string[] args)
    {
        new Bot2().Start();
    }

    Bot2() : base(BotInfo.FromFile("Bot2.json")) { }

    public override void Run()
    {
        // Warna bot
        BodyColor = Color.Gray;
        TurretColor = Color.DarkGray;
        RadarColor = Color.Red;
        BulletColor = Color.Yellow;
        ScanColor = Color.Green;

        while (IsRunning)
        {
            // Putar radar cari musuh
            TurnRadarRight(360);

            // Gerak random
            Forward(150);
            TurnRight(90);

            Fire(1);
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        Console.WriteLine($"Enemy ditemukan di {e.X}, {e.Y}");

        // Arahkan gun ke musuh
        TurnGunRight(GunBearingTo(e.X, e.Y));

        // Tembak lebih kuat
        Fire(2);

        // Dekati musuh
        Forward(50);
    }

    public override void OnHitWall(HitWallEvent e)
    {
        Console.WriteLine("Nabrak tembok!");

        Back(100);
        TurnRight(90);
    }

    public override void OnHitBot(HitBotEvent e)
    {
        Console.WriteLine("Nabrak bot!");

        Fire(3);

        Back(50);
        TurnRight(45);
    }
}