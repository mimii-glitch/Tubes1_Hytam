using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

// ------------------------------------------------------------------
// BlackMan
// ------------------------------------------------------------------
// A sample bot original made for Robocode by Mathew Nelson.
// Ported to Robocode Tank Royale by Flemming N. Larsen.
//
// This bot moves around in a BlackMan pattern.
// ------------------------------------------------------------------
public class BlackMan : Bot
{
    bool movingForward;

    // The main method starts our bot
    static void Main()
    {
        new BlackMan().Start();
    }

    // Constructor, which loads the bot config file
    BlackMan() : base(BotInfo.FromFile("BlackMan.json")) { }

    // Called when a new round is started -> initialize and do some movement
    public override void Run()
    {
        BodyColor = Color.Black;   // lime
        TurretColor = Color.Black; // green
        RadarColor = Color.Red;  // dark cyan
        BulletColor = Color.FromArgb(0xFF, 0xC8, 0xC8); // yellow
        ScanColor = Color.Red;   // light red

        movingForward = true;

        // Loop while as long as the bot is running
        while (IsRunning)
        {
            // Tell the game we will want to move ahead 40000 -- some large number
            SetForward(40000);
            movingForward = true;
            // Tell the game we will want to turn right 90
            SetTurnRight(90);
            TurnGunRight(360);

            WaitFor(new TurnCompleteCondition(this));
            TurnGunRight(360);
            WaitFor(new TurnCompleteCondition(this));
            SetTurnRight(150);
            TurnGunRight(360);
            // ... and wait for that turn to finish.
            WaitFor(new TurnCompleteCondition(this));
            // then back to the top to do it all again.
        }
    }

    // ==================================================================
    // KONSEP GREEDY: Greedy Collision Avoidance (Penghindaran Tabrakan)
    // Pilihan lokal terbaik (locally optimal choice) ketika menabrak dinding
    // adalah langsung membalikkan arah gerakan secara instan (ReverseDirection)
    // untuk meminimalkan pengurangan energi berkelanjutan akibat tabrakan.
    // ==================================================================
    // We collided with a wall -> reverse the direction
    public override void OnHitWall(HitWallEvent e)
    {
        // Bounce off!
        ReverseDirection();
    }

    // ReverseDirection: Switch from ahead to back & vice versa
    public void ReverseDirection()
    {
        if (movingForward)
        {
            SetBack(40000);
            movingForward = false;
        }
        else
        {
            SetForward(40000);
            movingForward = true;
        }
    }

    // ==================================================================
    // KONSEP GREEDY: Greedy Shooting (Penembakan Seketika)
    // Begitu sensor mendeteksi musuh (ScannedBot), bot langsung mengambil keputusan
    // lokal terbaik tanpa berpikir panjang dengan menembakkan peluru berdaya 2.5 (Fire(2.5)).
    // Keputusan ini didasari oleh harapan instan untuk melukai musuh (lokal optimal)
    // tanpa mempertimbangkan status panas senjata (gun heat), sisa energi, atau jarak.
    // ==================================================================
    // We scanned another bot -> fire!
    public override void OnScannedBot(ScannedBotEvent e)
    {
        Fire(3);
    }

    // ==================================================================
    // KONSEP GREEDY: Greedy Crash Escape
    // Jika bertabrakan dengan bot lain, pilihan instan terbaik untuk menghindari
    // kerugian energi lebih lanjut adalah langsung menjauh dengan membalikkan arah.
    // ==================================================================
    // We hit another bot -> back up!
    public override void OnHitBot(HitBotEvent e)
    {
        // If we're moving into the other bot, reverse!
        if (e.IsRammed)
        {
            ReverseDirection();
        }
    }
}

// Condition that is triggered when the turning is complete
public class TurnCompleteCondition : Condition
{
    private readonly Bot bot;

    public TurnCompleteCondition(Bot bot)
    {
        this.bot = bot;
    }

    public override bool Test()
    {
        // turn is complete when the remainder of the turn is zero
        return bot.TurnRemaining == 0;
    }
}