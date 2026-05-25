using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

// ------------------------------------------------------------------
// botak2
// ------------------------------------------------------------------
// A sample bot original made for Robocode by Mathew Nelson.
// Ported to Robocode Tank Royale by Flemming N. Larsen.
//
// Moves around the outer edge with the gun facing in.
// ------------------------------------------------------------------
public class botak2 : Bot
{
    bool peek; // Don't turn if there's a bot there
    double moveAmount; // How much to move

    // The main method starts our bot
    static void Main()
    {
        new botak2().Start();
    }

    // Constructor, which loads the bot config file
    botak2() : base(BotInfo.FromFile("botak2.json")) { }

    // Called when a new round is started -> initialize and do some movement
    public override void Run()
    {
        // Set colors
        BodyColor = Color.Green;
        TurretColor = Color.Green;
        RadarColor = Color.Green;
        BulletColor = Color.Green;
        ScanColor = Color.Green;

        // Initialize moveAmount to the maximum possible for the arena
        moveAmount = Math.Max(100, 100);
        // Initialize peek to false
        peek = false;

        // turn to face a wall.
        // `Direction % 90` means the remainder of Direction divided by 90.
        TurnRight(120);
        Forward(moveAmount);
        TurnGunRight(360);

        // Turn the gun to turn right 90 degrees.
        peek = true;
        TurnGunRight(360);
        // TurnRight(75);

        // Main loop
        while (IsRunning)
        {
            // Peek before we turn when forward() completes.
            peek = true;
            // Move up the wall
            Forward(moveAmount);
            TurnGunRight(360);

            // Don't peek now
            peek = false;
            // Turn to the next wall
            Forward(moveAmount);
            TurnRight(90);
            TurnGunRight(360);
        }
    }

    // We hit another bot -> move away a bit
    public override void OnHitBot(HitBotEvent e)
    {
        // If he's in front of us, set back up a bit.
        var bearing = BearingTo(e.X, e.Y);
        if (bearing > -90 && bearing < 90)
        {
            Back(100);
        }
        else
        { // else he's in back of us, so set ahead a bit.
            Forward(100);
        }
    }

    // We scanned another bot -> fire!
    public override void OnScannedBot(ScannedBotEvent e)
    {
        SetFire(2);
        // Note that scan is called automatically when the bot is turning.
        // By calling it manually here, we make sure we generate another scan event if there's a bot
        // on the next wall, so that we do not start moving up it until it's gone.
        if (peek)
            Rescan();
    }
}